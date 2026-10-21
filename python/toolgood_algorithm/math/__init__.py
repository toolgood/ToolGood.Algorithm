"""Formula language front end.

The C# project uses ANTLR generated files (``mathLexer.cs``, ``mathParser.cs``,
``mathVisitor.cs``, ``mathBaseVisitor.cs``).  This port implements the same
grammar by hand: :mod:`toolgood_algorithm.math.parser` contains the tokenizer,
the AST and the recursive descent parser.
"""
from __future__ import annotations

from .parser import ParseError, parse_ast, parse_function

__all__ = ["ParseError", "parse_ast", "parse_function"]
