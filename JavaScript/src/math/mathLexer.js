// 手写词法分析器：参考 csharp/ToolGood.Algorithm.Fast/math/mathLexer.cs 实现，
// 替代 ANTLR 生成版本，用于提升词法分析性能。
// token 编号与 math.g4(mini 语法)生成的 mathParser 保持一致。
// jshint ignore: start
import antlr4 from '../antlr4/index.web.js';

const T__0 = 1;    // '.'
const T__1 = 2;    // '('
const T__2 = 3;    // ','
const T__3 = 4;    // ')'
const T__4 = 5;    // '['
const T__5 = 6;    // ']'
const T__6 = 7;    // '!'
const T__7 = 8;    // '%'
const T__8 = 9;    // '*'
const T__9 = 10;   // '/'
const T__10 = 11;  // '+'
const T__11 = 12;  // '-'
const T__12 = 13;  // '&'
const T__13 = 14;  // '>'
const T__14 = 15;  // '>='
const T__15 = 16;  // '<'
const T__16 = 17;  // '<='
const T__17 = 18;  // '='
const T__18 = 19;  // '=='
const T__19 = 20;  // '==='
const T__20 = 21;  // '!=='
const T__21 = 22;  // '!='
const T__22 = 23;  // '<>'
const T__23 = 24;  // '&&'
const T__24 = 25;  // '||'
const T__25 = 26;  // '?'
const T__26 = 27;  // ':'
const T__27 = 28;  // '{'
const T__28 = 29;  // '}'
const NUM = 30;
const STRING = 31;
const NULL = 32;
const UNIT = 33;
const T = 34;
const PARAMETER = 35;

// 关键字字典（键为标准化后的大写文本，与 AntlrCharStream.LA 返回的字符一致）
const _keywords = new Map([
	// 单位
	['M', UNIT], ['KM', UNIT], ['DM', UNIT], ['CM', UNIT], ['MM', UNIT],
	['M2', UNIT], ['KM2', UNIT], ['DM2', UNIT], ['CM2', UNIT], ['MM2', UNIT],
	['M3', UNIT], ['KM3', UNIT], ['DM3', UNIT], ['CM3', UNIT], ['MM3', UNIT],
	['L', UNIT], ['ML', UNIT], ['G', UNIT], ['KG', UNIT],
	// T
	['T', T],
	// NULL
	['NULL', NULL],
	// 带点的参数名
	['PERCENTILE.INC', PARAMETER],
	['PERCENTRANK.INC', PARAMETER],
	['STDEV.S', PARAMETER],
	['STDEV.P', PARAMETER],
	['COVARIANCE.P', PARAMETER],
	['COVARIANCE.S', PARAMETER],
	['VAR.S', PARAMETER],
	['VAR.P', PARAMETER],
	['NORM.DIST', PARAMETER],
	['NORM.INV', PARAMETER],
	['NORM.S.DIST', PARAMETER],
	['NORM.S.INV', PARAMETER],
	['BETA.DIST', PARAMETER],
	['BETA.INV', PARAMETER],
	['BINOM.DIST', PARAMETER],
	['EXPON.DIST', PARAMETER],
	['F.DIST', PARAMETER],
	['F.INV', PARAMETER],
	['GAMMA.DIST', PARAMETER],
	['GAMMA.INV', PARAMETER],
	['GAMMALN.PRECISE', PARAMETER],
	['HYPGEOM.DIST', PARAMETER],
	['LOGNORM.INV', PARAMETER],
	['LOGNORM.DIST', PARAMETER],
	['NEGBINOM.DIST', PARAMETER],
	['POISSON.DIST', PARAMETER],
	['T.DIST', PARAMETER],
	['T.INV', PARAMETER]
]);

export default class mathLexer extends antlr4.Lexer {

	static grammarFileName = "";
	static channelNames = [];
	static modeNames = [];
	static literalNames = [];
	static symbolicNames = [];
	static ruleNames = [];

	constructor(input, output, errorOutput) {
		super(input);
		this._output = output;
		this._errorOutput = errorOutput;
		this._startCharIndex = 0;
		this._startLine = 1;
		this._startColumn = 0;
		this._line = 1;
		this._column = 0;
	}

	get line() {
		return this._line;
	}

	set line(line) {
		this._line = line;
	}

	get column() {
		return this._column;
	}

	set column(column) {
		this._column = column;
	}

	reset() {
		if (this._input !== null) {
			this._input.seek(0);
		}
		this._line = 1;
		this._column = 0;
		this._startLine = 1;
		this._startColumn = 0;
		this._hitEOF = false;
	}

	nextToken() {
		for (;;) {
			let c = this._input.LA(1);
			if (c === antlr4.Token.EOF) {
				this._hitEOF = true;
				return this.emitEOF();
			}
			this._startCharIndex = this._input.index;
			this._startLine = this._line;
			this._startColumn = this._column;

			if (this.IsDigit(c)) {
				return this.ReadNumber();
			} else if (c === 39 || c === 34 || c === 96) { // ' " `
				return this.ReadString(c);
			} else if (this.IsIdentifierStart(c)) {
				return this.ReadIdentifier();
			} else if (this.IsWhitespace(c)) {
				this.ConsumeWhitespace();
				continue;
			} else if (c === 47) { // '/'
				let c2 = this._input.LA(2);
				if (c2 === 42) { // '*'
					this.ConsumeBlockComment();
					continue;
				} else if (c2 === 47) { // '/'
					this.ConsumeLineComment();
					continue;
				}
			}
			let token = this.ReadOperator(c);
			if (token !== null) {
				return token;
			}
			// 未知字符：已在 ReadOperator 中上报错误并消费字符，继续下一个 token
		}
	}

	CreateToken(type) {
		let stopIndex = this._input.index - 1;
		return this._factory.create(this._tokenFactorySourcePair, type, null,
			antlr4.Token.DEFAULT_CHANNEL, this._startCharIndex, stopIndex,
			this._startLine, this._startColumn);
	}

	// 上报词法错误（与 ANTLR 生成的 Lexer.notifyListeners 行为一致）
	ReportError() {
		let text = this._input.getText(this._startCharIndex, this._input.index - 1);
		let msg = "token recognition error at: '" + this.getErrorDisplay(text) + "'";
		this.getErrorListener().syntaxError(this, null, this._startLine, this._startColumn, msg, null);
	}

	// 消费一个字符并维护行列信息（\r 被 AntlrCharStream 标准化为 \n）
	Consume() {
		let c = this._input.LA(1);
		if (c === 10) { // '\n'
			this._line += 1;
			this._column = 0;
		} else {
			this._column += 1;
		}
		this._input.consume();
	}

	IsWhitespace(c) {
		return c === 32 || c === 10; // ' ' '\n'（\t、\f 被标准化为 ' '，\r 被标准化为 '\n'）
	}

	IsDigit(c) {
		return c >= 48 && c <= 57; // '0'-'9'
	}

	IsIdentifierStart(c) {
		return (c >= 65 && c <= 90) || c === 95; // 'A'-'Z' '_'（全角字母/CJK 已被 AntlrCharStream 标准化）
	}

	IsIdentifierPart(c) {
		return (c >= 65 && c <= 90) || c === 95 || (c >= 48 && c <= 57); // 'A'-'Z' '_' '0'-'9'
	}

	ConsumeWhitespace() {
		this.Consume();
		while (this.IsWhitespace(this._input.LA(1))) {
			this.Consume();
		}
	}

	ConsumeBlockComment() {
		this.Consume(); // '/'
		this.Consume(); // '*'
		while (true) {
			let c = this._input.LA(1);
			if (c === antlr4.Token.EOF) break;
			this.Consume();
			if (c === 42 && this._input.LA(1) === 47) { // '*' '/'
				this.Consume();
				break;
			}
		}
	}

	ConsumeLineComment() {
		this.Consume(); // '/'
		this.Consume(); // '/'
		while (true) {
			let c = this._input.LA(1);
			if (c === antlr4.Token.EOF || c === 10) break; // EOF '\n'
			this.Consume();
		}
	}

	ReadString(quote) {
		this.Consume(); // 引号
		while (true) {
			let c = this._input.LA(1);
			if (c === antlr4.Token.EOF) { break; }
			this.Consume();
			if (c === quote) { break; }
			if (c === 92) { // '\\'
				if (this._input.LA(1) !== antlr4.Token.EOF) {
					this.Consume();
				}
			}
		}
		return this.CreateToken(STRING);
	}

	ReadNumber() {
		let c = this._input.LA(1);
		if (c === 48) { // '0'：以 0 开头，后面不能再跟整数数字
			this.Consume();
			c = this._input.LA(1);
		} else {
			this.Consume(); // [1-9]
			c = this._input.LA(1);
			while (this.IsDigit(c)) {
				this.Consume();
				c = this._input.LA(1);
			}
		}

		// 小数部分：'.' 后必须跟数字
		if (c === 46 && this.IsDigit(this._input.LA(2))) { // '.'
			this.Consume();
			this.Consume();
			c = this._input.LA(1);
			while (this.IsDigit(c)) {
				this.Consume();
				c = this._input.LA(1);
			}
		}

		// E 指数部分：'E' [+-]? [0-9][0-9]?（指数最多 2 位，与 ANTLR 语法一致）
		if (c === 69) { // 'E'
			let c2 = this._input.LA(2);
			if (this.IsDigit(c2) || c2 === 43 || c2 === 45) { // 数字 '+' '-'
				let savedIndex = this._input.index;
				let savedColumn = this._column;
				this.Consume(); // 'E'
				if (c2 === 43 || c2 === 45) {
					this.Consume(); // '+' '-'
					if (this.IsDigit(this._input.LA(1))) {
						this.Consume();
						if (this.IsDigit(this._input.LA(1))) {
							this.Consume();
						}
					} else {
						// 符号后无数字：回退（例如 1e+X）
						this._input.seek(savedIndex);
						this._column = savedColumn;
					}
				} else {
					this.Consume(); // 第一位指数数字
					if (this.IsDigit(this._input.LA(1))) {
						this.Consume(); // 第二位指数数字
					}
				}
			}
		}
		return this.CreateToken(NUM);
	}

	ReadIdentifier() {
		let startIndex = this._startCharIndex;
		let sb = String.fromCharCode(this._input.LA(1)); // 标准化后的首字符
		this.Consume();
		let c = this._input.LA(1);
		while (this.IsIdentifierPart(c)) {
			sb += String.fromCharCode(c);
			this.Consume();
			c = this._input.LA(1);
		}

		// 最后一个与关键字匹配的结束位置（无点标识符作为 PARAMETER 兜底）
		let lastHitIndex = this._input.index;
		let lastHitType = _keywords.get(sb);
		if (lastHitType === undefined) {
			lastHitType = PARAMETER;
		}

		// 贪心读取 '.' 段，若整体未命中则回退到最后一个命中位置
		while (c === 46 && this.IsIdentifierStart(this._input.LA(2))) { // '.' + 标识符首字符
			sb += '.';
			this.Consume(); // '.'
			sb += String.fromCharCode(this._input.LA(1));
			this.Consume(); // 段首字符
			c = this._input.LA(1);
			while (this.IsIdentifierPart(c)) {
				sb += String.fromCharCode(c);
				this.Consume();
				c = this._input.LA(1);
			}
			let ft = _keywords.get(sb);
			if (ft !== undefined) {
				lastHitIndex = this._input.index;
				lastHitType = ft;
			}
		}

		if (this._input.index > lastHitIndex) {
			// 回退：'.' 段中不含换行，列号按字符数减少
			this._column -= (this._input.index - lastHitIndex);
			this._input.seek(lastHitIndex);
		}
		return this.CreateToken(lastHitType);
	}

	ReadOperator(c) {
		switch (c) {
			case 46: // '.'
				this.Consume();
				return this.CreateToken(T__0);
			case 40: // '('
				this.Consume();
				return this.CreateToken(T__1);
			case 44: // ','
				this.Consume();
				return this.CreateToken(T__2);
			case 41: // ')'
				this.Consume();
				return this.CreateToken(T__3);
			case 91: // '['
				this.Consume();
				return this.CreateToken(T__4);
			case 93: // ']'
				this.Consume();
				return this.CreateToken(T__5);
			case 33: // '!'
				this.Consume();
				if (this._input.LA(1) === 61) { // '='
					this.Consume();
					if (this._input.LA(1) === 61) { // '='
						this.Consume();
						return this.CreateToken(T__20); // '!=='
					}
					return this.CreateToken(T__21); // '!='
				}
				return this.CreateToken(T__6); // '!'
			case 37: // '%'
				this.Consume();
				return this.CreateToken(T__7);
			case 42: // '*'
				this.Consume();
				return this.CreateToken(T__8);
			case 47: // '/'
				this.Consume();
				return this.CreateToken(T__9);
			case 43: // '+'
				this.Consume();
				return this.CreateToken(T__10);
			case 45: // '-'
				this.Consume();
				return this.CreateToken(T__11);
			case 38: // '&'
				this.Consume();
				if (this._input.LA(1) === 38) { // '&'
					this.Consume();
					return this.CreateToken(T__23); // '&&'
				}
				return this.CreateToken(T__12); // '&'
			case 62: // '>'
				this.Consume();
				if (this._input.LA(1) === 61) { // '='
					this.Consume();
					return this.CreateToken(T__14); // '>='
				}
				return this.CreateToken(T__13); // '>'
			case 60: // '<'
				this.Consume();
				let c2 = this._input.LA(1);
				if (c2 === 61) { // '='
					this.Consume();
					return this.CreateToken(T__16); // '<='
				} else if (c2 === 62) { // '>'
					this.Consume();
					return this.CreateToken(T__22); // '<>'
				}
				return this.CreateToken(T__15); // '<'
			case 61: // '='
				this.Consume();
				if (this._input.LA(1) === 61) { // '='
					this.Consume();
					if (this._input.LA(1) === 61) { // '='
						this.Consume();
						return this.CreateToken(T__19); // '==='
					}
					return this.CreateToken(T__18); // '=='
				}
				return this.CreateToken(T__17); // '='
			case 63: // '?'
				this.Consume();
				return this.CreateToken(T__25);
			case 58: // ':'
				this.Consume();
				return this.CreateToken(T__26);
			case 123: // '{'
				this.Consume();
				return this.CreateToken(T__27);
			case 125: // '}'
				this.Consume();
				return this.CreateToken(T__28);
			case 124: // '|'
				this.Consume();
				if (this._input.LA(1) === 124) { // '|'
					this.Consume();
					return this.CreateToken(T__24); // '||'
				}
				// 单个 '|' 非法：上报错误并跳过
				this.ReportError();
				return null;
		}
		// 未知字符：上报错误并跳过
		this.Consume();
		this.ReportError();
		return null;
	}
}
