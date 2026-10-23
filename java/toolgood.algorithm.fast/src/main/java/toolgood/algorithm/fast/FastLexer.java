package toolgood.algorithm.fast;

import toolgood.algorithm.internals.visitors.CharUtil;

/**
 * 手写词法分析器，等价替代 ANTLR 生成的 {@code mathLexer}。
 * <p>
 * 直接扫描 {@link String}，不做字符流包装、不产生 Token 对象；大小写/全角/中文归一化
 * 通过 {@link CharUtil#StandardChar(char)} 完成，行为与原 ANTLR 词法一致。
 * </p>
 */
final class FastLexer {
    private final String text;
    private int pos;
    private String error;
    /** 归一化后的标识符缓冲，查关键字表时复用，避免每次 substring */
    private char[] keyBuf = new char[32];

    FastLexer(String text) {
        this.text = text == null ? "" : text;
    }

    /** 词法错误信息，null 表示无错误 */
    String getErrorMessage() {
        return error;
    }

    /**
     * 读取下一个词法单元。返回 false 表示词法错误（此时 {@link #getErrorMessage()} 非 null）。
     */
    boolean next(FastLexeme token) {
        while (true) {
            if (pos >= text.length()) {
                token.type = FastToken.EOF;
                token.start = pos;
                token.end = pos - 1;
                return true;
            }
            int c = ch(pos);
            if (c >= '0' && c <= '9') {
                token.start = pos;
                readNumber(token);
                return true;
            }
            if (c == '\'' || c == '"' || c == '`') {
                token.start = pos;
                return readString(c, token);
            }
            if (isIdentifierStart(c)) {
                token.start = pos;
                readIdentifier(token);
                return true;
            }
            if (c == ' ' || c == '\n' || c == '\t' || c == '\f') {
                pos++;
                continue;
            }
            if (c == '/') {
                int c2 = ch(pos + 1);
                if (c2 == '*') {
                    consumeBlockComment();
                    continue;
                }
                if (c2 == '/') {
                    consumeLineComment();
                    continue;
                }
            }
            token.start = pos;
            return readOperator(c, token);
        }
    }

    /** 返回指定下标的归一化字符，越界返回 -1 */
    private int ch(int index) {
        if (index < 0 || index >= text.length()) {
            return -1;
        }
        return CharUtil.StandardChar(text.charAt(index));
    }

    private static boolean isDigit(int c) {
        return c >= '0' && c <= '9';
    }

    private static boolean isIdentifierStart(int c) {
        return (c >= 'A' && c <= 'Z') || c == '_';
    }

    private static boolean isIdentifierPart(int c) {
        return (c >= 'A' && c <= 'Z') || c == '_' || (c >= '0' && c <= '9');
    }

    private int scanIdentifierPart(int from) {
        int p = from;
        while (isIdentifierPart(ch(p))) {
            p++;
        }
        return p;
    }

    private void consumeBlockComment() {
        pos += 2;
        while (true) {
            int c = ch(pos);
            if (c < 0) {
                return;
            }
            pos++;
            if (c == '*' && ch(pos) == '/') {
                pos++;
                return;
            }
        }
    }

    private void consumeLineComment() {
        pos += 2;
        while (true) {
            int c = ch(pos);
            if (c < 0 || c == '\n') {
                return;
            }
            pos++;
        }
    }

    private boolean readString(int quote, FastLexeme token) {
        pos++; // 开引号
        while (true) {
            int c = ch(pos);
            if (c < 0) {
                error = "Unterminated string literal at position " + token.start + ".";
                token.type = FastToken.Invalid;
                return false;
            }
            pos++;
            if (c == quote) {
                break;
            }
            if (c == '\\') {
                if (ch(pos) < 0) {
                    error = "Unterminated string literal at position " + token.start + ".";
                    token.type = FastToken.Invalid;
                    return false;
                }
                pos++;
            }
        }
        token.type = FastToken.STRING;
        token.end = pos - 1;
        return true;
    }

    private void readNumber(FastLexeme token) {
        pos++;
        while (isDigit(ch(pos))) {
            pos++;
        }

        int c = ch(pos);
        if (c == '.' && isDigit(ch(pos + 1))) {
            pos += 2;
            while (isDigit(ch(pos))) {
                pos++;
            }
            c = ch(pos);
        }

        if (c == 'E') {
            int c2 = ch(pos + 1);
            if (isDigit(c2) || c2 == '+' || c2 == '-') {
                pos += 2;
                while (isDigit(ch(pos))) {
                    pos++;
                }
            }
            token.type = FastToken.NUM;
            token.end = pos - 1;
            return;
        }

        // 单位后缀： [KDCM]? 'M' [23]? | 'M'? 'L' | 'K'? 'G' | 'T'
        if (c == 'C' || c == 'D') {
            if (ch(pos + 1) == 'M') {
                pos += 2;
                int c3 = ch(pos);
                if (c3 == '2' || c3 == '3') {
                    pos++;
                }
            }
        } else if (c == 'M') {
            pos++;
            int c2 = ch(pos);
            if (c2 == 'M') {
                pos++;
                int c3 = ch(pos);
                if (c3 == '2' || c3 == '3') {
                    pos++;
                }
            } else if (c2 == '2' || c2 == '3' || c2 == 'L') {
                pos++;
            }
        } else if (c == 'K') {
            int c2 = ch(pos + 1);
            if (c2 == 'M') {
                pos += 2;
                int c3 = ch(pos);
                if (c3 == '2' || c3 == '3') {
                    pos++;
                }
            } else if (c2 == 'G') {
                pos += 2;
            }
        } else if (c == 'G' || c == 'T' || c == 'L') {
            pos++;
        }

        token.type = FastToken.NUM;
        token.end = pos - 1;
    }

    private void readIdentifier(FastLexeme token) {
        int start = pos;
        int e0 = scanIdentifierPart(start + 1);

        // 关键字允许带点，如 BETA.DIST / NORM.S.DIST：优先按最长形式匹配
        int e1 = -1, e2 = -1;
        if (ch(e0) == '.' && isIdentifierStart(ch(e0 + 1))) {
            e1 = scanIdentifierPart(e0 + 2);
            if (ch(e1) == '.' && isIdentifierStart(ch(e1 + 1))) {
                e2 = scanIdentifierPart(e1 + 2);
            }
        }
        if (e2 > 0) {
            int type2 = findKeyword(start, e2);
            if (type2 != 0) {
                pos = e2;
                token.type = type2;
                token.end = e2 - 1;
                return;
            }
        }
        if (e1 > 0) {
            int type1 = findKeyword(start, e1);
            if (type1 != 0) {
                pos = e1;
                token.type = type1;
                token.end = e1 - 1;
                return;
            }
        }
        pos = e0;
        token.end = e0 - 1;
        int type0 = findKeyword(start, e0);
        token.type = type0 != 0 ? type0 : FastToken.PARAMETER;
    }

    /** 把 [start, end) 的字符按 CharUtil 归一化后查表 */
    private int findKeyword(int start, int end) {
        int len = end - start;
        if (len <= 0) {
            return 0;
        }
        if (keyBuf.length < len) {
            keyBuf = new char[Math.max(len, keyBuf.length * 2)];
        }
        for (int i = 0; i < len; i++) {
            keyBuf[i] = CharUtil.StandardChar(text.charAt(start + i));
        }
        return FastKeywords.find(keyBuf, len);
    }

    private boolean readOperator(int c, FastLexeme token) {
        pos++;
        switch (c) {
            case '(':
                token.type = FastToken.LPAREN;
                break;
            case ')':
                token.type = FastToken.RPAREN;
                break;
            case '.':
                token.type = FastToken.DOT;
                break;
            case ',':
                token.type = FastToken.COMMA;
                break;
            case '[':
                token.type = FastToken.LBRACKET;
                break;
            case ']':
                token.type = FastToken.RBRACKET;
                break;
            case '!':
                if (ch(pos) == '=') {
                    pos++;
                    if (ch(pos) == '=') {
                        pos++;
                    }
                    token.type = FastToken.OPNE;
                } else {
                    token.type = FastToken.BANG;
                }
                break;
            case '?':
                token.type = FastToken.QUESTION;
                break;
            case ':':
                token.type = FastToken.COLON;
                break;
            case '{':
                token.type = FastToken.LBRACE;
                break;
            case '}':
                token.type = FastToken.RBRACE;
                break;
            case '&':
                if (ch(pos) == '&') {
                    pos++;
                    token.type = FastToken.OPAND;
                } else {
                    token.type = FastToken.OPCAT;
                }
                break;
            case '|':
                if (ch(pos) == '|') {
                    pos++;
                    token.type = FastToken.OPOR;
                    break;
                }
                error = "Unexpected character '|' at position " + token.start + ".";
                token.type = FastToken.Invalid;
                return false;
            case '+':
                token.type = FastToken.OPADD;
                break;
            case '-':
                token.type = FastToken.OPSUB;
                break;
            case '*':
                token.type = FastToken.OPMUL;
                break;
            case '/':
                token.type = FastToken.OPDIV;
                break;
            case '%':
                token.type = FastToken.OPMOD;
                break;
            case '>':
                if (ch(pos) == '=') {
                    pos++;
                    token.type = FastToken.OPGE;
                } else {
                    token.type = FastToken.OPGT;
                }
                break;
            case '<': {
                int c2 = ch(pos);
                if (c2 == '=') {
                    pos++;
                    token.type = FastToken.OPLE;
                } else if (c2 == '>') {
                    pos++;
                    token.type = FastToken.OPNE;
                } else {
                    token.type = FastToken.OPLT;
                }
                break;
            }
            case '=':
                if (ch(pos) == '=') {
                    pos++;
                    if (ch(pos) == '=') {
                        pos++;
                    }
                }
                token.type = FastToken.OPEQ;
                break;
            default:
                error = "Unexpected character '" + text.charAt(token.start) + "' at position " + token.start + ".";
                token.type = FastToken.Invalid;
                return false;
        }
        token.end = pos - 1;
        return true;
    }
}
