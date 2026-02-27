/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
import { default as atn } from './atn/index.js';
import { default as dfa } from './dfa/index.js';
import { default as tree } from './tree/index.js';
import { default as error } from './error/index.js';

import Lexer from './Lexer.js';
import Parser from './Parser.js';
import Token from './Token.js';

import ParserRuleContext from './context/ParserRuleContext.js';
 

export default {
    atn, dfa, tree, error, Lexer, Parser, ParserRuleContext, Token
}

// export {
//     Token, CommonToken, CharStreams, CharStream, InputStream, CommonTokenStream, Lexer, Parser,
//     RuleNode, TerminalNode, ParseTreeWalker, RuleContext, ParserRuleContext, Interval, IntervalSet,
//     PredictionMode, LL1Analyzer, ParseTreeListener, ParseTreeVisitor, ATN, ATNDeserializer, PredictionContextCache, LexerATNSimulator, ParserATNSimulator, DFA,
//     RecognitionException, NoViableAltException, FailedPredicateException, ErrorListener, DiagnosticErrorListener, BailErrorStrategy, DefaultErrorStrategy,
//     arrayToString, TokenStreamRewriter, InputMismatchException
// }
