#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""从 math.g4 / mathLexer.cs / MathFunctionVisitor.cs 生成 Fast 项目所需的 Token 常量、关键字表和函数工厂。

用法: python gen_fast_tokens.py
输出: ../math/FastToken.cs, ../math/FastKeywordTable.cs, ./FastFunctionFactory.cs
"""
import io
import os
import re

HERE = os.path.dirname(os.path.abspath(__file__))
CSPROJ = os.path.abspath(os.path.join(HERE, '..', '..'))      # csharp/
ROOT = os.path.abspath(os.path.join(CSPROJ, '..'))            # 仓库根目录
G4 = os.path.join(ROOT, 'g4', 'math.g4')
ALG = os.path.join(CSPROJ, 'ToolGood.Algorithm')
LEXER = os.path.join(ALG, 'math', 'mathLexer.cs')
VISITOR = os.path.join(ALG, 'Internals', 'Visitors', 'MathFunctionVisitor.cs')


def read(p):
    with io.open(p, encoding='utf-8-sig') as f:
        return f.read()


# ---------------------------------------------------------------- tokens
lexer_src = read(LEXER)
block = lexer_src[lexer_src.index('public const int'):]
block = block[:block.index(';')]
tokens = [(m.group(1), int(m.group(2))) for m in re.finditer(r'(\w+)\s*=\s*(\d+)', block)]
# WS / COMMENT / LINE_COMMENT 在词法里被直接跳过，不进入语法层
tokens = [t for t in tokens if t[1] <= 300]
assert len(tokens) == 300 and tokens[-1][0] == 'PARAMETER', tokens[-1]
known = set(n for n, _ in tokens)

# ---------------------------------------------------------------- grammar lists
g4 = read(G4)


def names_of(text):
    out = []
    for m in re.finditer(r'\b([A-Z][A-Z0-9_]*)\b', text):
        n = m.group(1)
        if n in known and n not in out:
            out.append(n)
    return out


# alt 5: expr '.' f=(...) '(' ... ')'   -> 方法调用
a = g4.index("| expr '.' f=(")
b = g4.index("'(' (expr (',' expr)*)? ')'", a)
method_list = names_of(g4[a:b])

# alt 18: | f=(ERROR | ... ) '(' expr ...
a = g4.index('| f=(ERROR')
b = g4.index("'(' expr (',' expr)* ')'", a)
func_list = names_of(g4[a:b])

# parameter2 规则
a = g4.index('parameter2:')
b = g4.index('OPAND:', a)
param2_list = names_of(g4[a:b])

# ---------------------------------------------------------------- visitor switch
vis = read(VISITOR)
a = vis.index('public FunctionBase VisitFunction_fun')
sw = vis[a:]
mapping = {}
for m in re.finditer(r'case mathLexer\.(\w+):\s*(?://[^\n]*\n\s*)?return new (Function_\w+)\(([^;]*?)\);', sw):
    mapping[m.group(1)] = (m.group(2), m.group(3))

# ---------------------------------------------------------------- keywords
# 直接从 math.g4 的词法规则展开出全部字面量：RHS 是 '字面量' 与 [KDCM]?/'?' 组成的序列，
# 支持 ('A'|'B') 分组和 'X'? 可选项，展开成字符串集合。
skip_rules = {'NUM', 'STRING', 'PARAMETER', 'WS', 'COMMENT', 'LINE_COMMENT'}


def expand(rhs):
    """把词法规则右侧展开成字面量集合，无法解析返回 None"""
    pos = [0]

    def skip_ws():
        while pos[0] < len(rhs) and rhs[pos[0]] in ' \t\r\n':
            pos[0] += 1

    def alts():
        sets = [seq()]
        while True:
            skip_ws()
            if pos[0] < len(rhs) and rhs[pos[0]] == '|':
                pos[0] += 1
                sets.append(seq())
            else:
                return sets

    def seq():
        out = ['']
        while True:
            skip_ws()
            if pos[0] >= len(rhs) or rhs[pos[0]] in '|)':
                return out
            item = elem()
            if item is None:
                return None
            out = [a + b for a in out for b in item]

    def elem():
        skip_ws()
        if pos[0] >= len(rhs):
            return None
        c = rhs[pos[0]]
        if c == "'":
            end = rhs.index("'", pos[0] + 1)
            lit = rhs[pos[0] + 1:end]
            pos[0] = end + 1
            vals = [lit]
        elif c == '(':
            pos[0] += 1
            sets = alts()
            skip_ws()
            if pos[0] >= len(rhs) or rhs[pos[0]] != ')':
                return None
            pos[0] += 1
            vals = []
            for s in sets:
                if s is None:
                    return None
                vals.extend(s)
        else:
            return None
        skip_ws()
        if pos[0] < len(rhs) and rhs[pos[0]] == '?':
            pos[0] += 1
            vals = vals + ['']
        return vals

    result = alts()
    skip_ws()
    if pos[0] != len(rhs):
        return None
    out = []
    for s in result:
        if s is None:
            return None
        out.extend(s)
    return out


kw = {}
rules = g4[g4.index("OPAND: '&&';"):]
for m in re.finditer(r'^([A-Z][A-Z0-9_]*):[ \t]*(.+?);[ \t]*$', rules, re.M):
    name, rhs = m.group(1), m.group(2)
    if name in skip_rules or name not in known:
        continue
    lits = expand(rhs)
    if lits is None:
        print('  skip rule %s: %s' % (name, rhs))
        continue
    for lit in set(lits):
        kw[lit.upper()] = name

assert 'TRUE' in kw.values() and kw['CEILING'] == 'CEILING'

# ---------------------------------------------------------------- 校验
missing = [t for t in (method_list + func_list) if t not in mapping]
assert not missing, 'visitor 缺少分支: %s' % missing
assert 'PARAMETER' in mapping
bad = [t for t in mapping if t not in known]
assert not bad, bad

print('method tokens  :', len(method_list))
print('function tokens:', len(func_list))
print('parameter2     :', len(param2_list))
print('keywords       :', len(kw))

# ---------------------------------------------------------------- FastToken.cs
out = []
w = out.append
w('// <auto-generated>')
w('//   由 tools/gen_fast_tokens.py 根据 math.g4 / mathLexer.cs 生成，请勿手工修改。')
w('// </auto-generated>')
w('namespace ToolGood.Algorithm.math')
w('{')
w('\t/// <summary>')
w('\t/// 手写词法/语法分析使用的 Token 常量，值与 math.g4 中 mathLexer 保持一致。')
w('\t/// </summary>')
w('\tinternal static class FastToken')
w('\t{')
w('\t\tpublic const int EOF = -1;')
w('\t\tpublic const int Invalid = 0;')
w('\t\tpublic const int')
per = 6
line = '\t\t\t'
for name, val in tokens:
    piece = '%s = %d, ' % (name, val)
    if len(line) + len(piece) > 110:
        w(line.rstrip())
        line = '\t\t\t'
    line += piece
w(line.rstrip().rstrip(','))
w('\t\t\t;')
w('')
w('\t\t// 便于阅读的字符型 Token 别名')
w('\t\tpublic const int LPAREN = T__0;    // \'(\'')
w('\t\tpublic const int RPAREN = T__1;    // \')\'')
w('\t\tpublic const int DOT = T__2;       // \'.\'')
w('\t\tpublic const int COMMA = T__3;     // \',\'')
w('\t\tpublic const int LBRACKET = T__4;  // \'[\'')
w('\t\tpublic const int RBRACKET = T__5;  // \']\'')
w('\t\tpublic const int BANG = T__6;      // \'!\'')
w('\t\tpublic const int QUESTION = T__7;  // \'?\'')
w('\t\tpublic const int COLON = T__8;     // \':\'')
w('\t\tpublic const int LBRACE = T__9;    // \'{\'')
w('\t\tpublic const int RBRACE = T__10;   // \'}\'')
w('')
w('\t\t/// <summary>可以出现在 <c>NAME(</c> 形式中的函数名</summary>')
w('\t\tinternal static readonly bool[] IsFunctionHead = new bool[PARAMETER + 1];')
w('')
w('\t\t/// <summary>可以出现在 <c>expr.NAME(</c> 形式中的函数名</summary>')
w('\t\tinternal static readonly bool[] IsMethodHead = new bool[PARAMETER + 1];')
w('')
w('\t\t/// <summary>可以出现在 <c>expr.NAME</c> / <c>{NAME:expr}</c> 形式中的关键字</summary>')
w('\t\tinternal static readonly bool[] IsParameter2 = new bool[PARAMETER + 1];')
w('')
w('\t\tstatic FastToken()')
w('\t\t{')
for t in func_list:
    w('\t\t\tIsFunctionHead[%s] = true;' % t)
w('\t\t\tIsFunctionHead[PARAMETER] = true;')
w('')
for t in method_list:
    w('\t\t\tIsMethodHead[%s] = true;' % t)
w('')
for t in param2_list:
    w('\t\t\tIsParameter2[%s] = true;' % t)
w('\t\t}')
w('\t}')
w('}')
w('')
open(os.path.join(HERE, '..', 'math', 'FastToken.cs'), 'w', encoding='utf-8-sig', newline='\r\n').write('\n'.join(out))

# ---------------------------------------------------------------- FastKeywordTable.cs
out = []
w = out.append
w('// <auto-generated>')
w('//   由 tools/gen_fast_tokens.py 根据 math.g4 / mathLexer.cs 生成，请勿手工修改。')
w('// </auto-generated>')
w('using System;')
w('')
w('namespace ToolGood.Algorithm.math')
w('{')
w('\t/// <summary>')
w('\t/// 关键字查找表。')
w('\t/// 用 64 位哈希 + 原文校验代替 <c>Dictionary&lt;string,int&gt;</c>，')
w('\t/// 避免为每个标识符创建子串，热路径上零分配。')
w('\t/// </summary>')
w('\tinternal static class FastKeywordTable')
w('\t{')
w('\t\tprivate const int Size = 1024;  // 2 的幂')
w('\t\tprivate static readonly int[] _types = new int[Size];')
w('\t\tprivate static readonly string[] _keys = new string[Size];')
w('\t\tprivate static readonly ulong[] _hashes = new ulong[Size];')
w('')
w('\t\tstatic FastKeywordTable()')
w('\t\t{')
# emit grouped by type to keep generated file compact
by_type = {}
for text, t in sorted(kw.items()):
    by_type.setdefault(t, []).append(text)
for t, texts in sorted(by_type.items(), key=lambda kv: tokens[[n for n, _ in tokens].index(kv[0])]):
    for text in texts:
        w('\t\t\tRegister("%s", FastToken.%s);' % (text, t))
w('\t\t}')
w('')
w('\t\tprivate static void Register(string text, int type)')
w('\t\t{')
w('\t\t\tvar hash = Hash(text.AsSpan());')
w('\t\t\tvar index = (int)(hash & (Size - 1));')
w('\t\t\twhile(_keys[index] != null) {')
w('\t\t\t\tif(_hashes[index] == hash && _keys[index] == text) { return; }  // 重复定义，保留先注册的')
w('\t\t\t\tindex = (index + 1) & (Size - 1);')
w('\t\t\t}')
w('\t\t\t_hashes[index] = hash;')
w('\t\t\t_keys[index] = text;')
w('\t\t\t_types[index] = type;')
w('\t\t}')
w('')
w('\t\t/// <summary>')
w('\t\t/// 计算文本哈希。逐字符按 CharUtil 归一化(大小写、全角、中文等)后再取哈希，')
w('\t\t/// 保证 "<c>error</c>" / "<c>ＥＲＲＯＲ</c>" 与注册的 "<c>ERROR</c>" 得到同一哈希。')
w('\t\t/// </summary>')
w('\t\tinternal static ulong Hash(ReadOnlySpan<char> text)')
w('\t\t{')
w('\t\t\tulong hash = 14695981039346656037UL;')
w('\t\t\tfor(int i = 0; i < text.Length; i++) {')
w('\t\t\t\thash ^= CharUtilStandard(text[i]);')
w('\t\t\t\thash *= 1099511628211UL;')
w('\t\t\t}')
w('\t\t\treturn hash;')
w('\t\t}')
w('')
w('\t\t/// <summary>')
w('\t\t/// 按哈希查找 Token 类型，未找到返回 0')
w('\t\t/// </summary>')
w('\t\tinternal static int Find(ulong hash, ReadOnlySpan<char> text)')
w('\t\t{')
w('\t\t\tvar index = (int)(hash & (Size - 1));')
w('\t\t\twhile(true) {')
w('\t\t\t\tvar key = _keys[index];')
w('\t\t\t\tif(key == null) { return 0; }')
w('\t\t\t\tif(_hashes[index] == hash && EqualsStandard(text, key.AsSpan())) { return _types[index]; }')
w('\t\t\t\tindex = (index + 1) & (Size - 1);')
w('\t\t\t}')
w('\t\t}')
w('')
w('\t\t/// <summary>')
w('\t\t/// 与 CharUtil 等价的大小写/全角不敏感比较')
w('\t\t/// </summary>')
w('\t\tprivate static bool EqualsStandard(ReadOnlySpan<char> left, ReadOnlySpan<char> right)')
w('\t\t{')
w('\t\t\tif(left.Length != right.Length) { return false; }')
w('\t\t\tfor(int i = 0; i < left.Length; i++) {')
w('\t\t\t\tvar l = left[i];')
w('\t\t\t\tvar r = right[i];')
w('\t\t\t\tif(l == r) { continue; }')
w('\t\t\t\tif(CharUtilStandard(l) != CharUtilStandard(r)) { return false; }')
w('\t\t\t}')
w('\t\t\treturn true;')
w('\t\t}')
w('')
w('\t\t[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]')
w('\t\tprivate static char CharUtilStandard(char c)')
w('\t\t{')
w('\t\t\treturn ToolGood.Algorithm.Internals.Visitors.CharUtil.StandardChar(c);')
w('\t\t}')
w('\t}')
w('}')
w('')
open(os.path.join(HERE, '..', 'math', 'FastKeywordTable.cs'), 'w', encoding='utf-8-sig', newline='\r\n').write('\n'.join(out))

# ---------------------------------------------------------------- FastFunctionFactory.cs
out = []
w = out.append
w('// <auto-generated>')
w('//   由 tools/gen_fast_tokens.py 根据 MathFunctionVisitor.cs 生成，请勿手工修改。')
w('// </auto-generated>')
w('using System;')
w('using ToolGood.Algorithm.Internals.Functions;')
w('using ToolGood.Algorithm.Internals.Functions.Compare;')
w('using ToolGood.Algorithm.Internals.Functions.Csharp;')
w('using ToolGood.Algorithm.Internals.Functions.CsharpSecurity;')
w('using ToolGood.Algorithm.Internals.Functions.CsharpWeb;')
w('using ToolGood.Algorithm.Internals.Functions.DateTimes;')
w('using ToolGood.Algorithm.Internals.Functions.Financial;')
w('using ToolGood.Algorithm.Internals.Functions.Flow;')
w('using ToolGood.Algorithm.Internals.Functions.MathBase;')
w('using ToolGood.Algorithm.Internals.Functions.MathSum;')
w('using ToolGood.Algorithm.Internals.Functions.MathSum2;')
w('using ToolGood.Algorithm.Internals.Functions.MathTransformation;')
w('using ToolGood.Algorithm.Internals.Functions.MathTrigonometric;')
w('using ToolGood.Algorithm.Internals.Functions.Operator;')
w('using ToolGood.Algorithm.Internals.Functions.String;')
w('using ToolGood.Algorithm.Internals.Functions.Value;')
w('')
w('namespace ToolGood.Algorithm.math')
w('{')
w('\tinternal static class FastFunctionFactory')
w('\t{')
w('\t\t/// <summary>')
w('\t\t/// 按函数名 Token 创建函数节点，<paramref name="type"/> 为 FastToken 常量')
w('\t\t/// </summary>')
w('\t\tinternal static FunctionBase Create(int type, FunctionBase[] funcs, string name)')
w('\t\t{')
w('\t\t\tswitch(type) {')
w('\t\t\t\t// CONST2_fun / CONST_fun：无参常量函数（对应 MathFunctionVisitor.VisitCONST2_fun / VisitCONST_fun）')
w('\t\t\t\tcase FastToken.TRUE: return new Function_ValueBoolean(true);')
w('\t\t\t\tcase FastToken.FALSE: return new Function_ValueBoolean(false);')
w('\t\t\t\tcase FastToken.ALGORITHMVERSION: return new Function_Version();')
w('\t\t\t\tcase FastToken.NULL: return new Function_NULL();')
w('\t\t\t\tcase FastToken.E: return new Function_Number_E();')
w('\t\t\t\tcase FastToken.PI: return new Function_Number_PI();')
w('\t\t\t\tcase FastToken.RAND: return new Function_RAND();')
w('\t\t\t\tcase FastToken.GUID: return new Function_GUID();')
w('\t\t\t\tcase FastToken.NOW: return new Function_NOW();')
w('\t\t\t\tcase FastToken.TODAY: return new Function_TODAY();')
for t, _ in tokens:
    if t not in mapping:
        continue
    fn, args = mapping[t]
    if t == 'PARAMETER':
        w('\t\t\t\tcase FastToken.PARAMETER: return new Function_DiyFunction(name, funcs);')
    else:
        w('\t\t\t\tcase FastToken.%s: return new %s(%s);' % (t, fn, args))
w('\t\t\t\tdefault: throw new ArgumentException($"Function \'{name}\' is not implemented.");')
w('\t\t\t}')
w('\t\t}')
w('\t}')
w('}')
w('')
open(os.path.join(HERE, '..', 'math', 'FastFunctionFactory.cs'), 'w', encoding='utf-8-sig', newline='\r\n').write('\n'.join(out))
print('done')
