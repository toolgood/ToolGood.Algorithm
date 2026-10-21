"""Formula language front end.

The C# project uses ANTLR generated files (``mathLexer.cs``, ``mathParser.cs``,
``mathVisitor.cs``, ``mathBaseVisitor.cs``).  This port implements the same
grammar by hand: :mod:`toolgood_algorithm.math.lexer` (tokenizer) and
:mod:`toolgood_algorithm.math.parser` (AST + parser).
"""
from __future__ import annotations

from toolgood_algorithm.math.lexer import ParseError, Token, Tokenizer
from toolgood_algorithm.math.parser import AstNode, parse_ast, parse_function

__all__ = ["AstNode", "ParseError", "Token", "Tokenizer", "parse_ast", "parse_function"]
