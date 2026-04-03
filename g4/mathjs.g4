grammar mathjs;

prog: expr EOF;

expr:
	'(' expr ')'
	| expr '.'  PARAMETER ('(' (expr (',' expr)*)? ')')?
	| expr '[' (PARAMETER | expr) ']'
	| '!' expr
	| expr OPMOD
	| expr (OPMOD|OPSUB|OP) expr
	| expr '?' expr ':' expr

	| PARAMETER '(' (expr (',' expr)*)? ')'
	| '{' arrayJson (',' arrayJson)* ','? '}'
	| '[' expr (',' expr)* ','? ']'
	| OPSUB? NUM
	| STRING
	| PARAMETER
	;

arrayJson: (NUM | STRING | PARAMETER) ':' expr;

OPMOD: '%';
OPSUB: '-';
OP: [+*/&><=]| [>=<!] '=' '='? | '<>' | '&&' | '||' ;

NUM: [0-9]+ ('.' [0-9]+)? ('E' [+-]? [0-9]+)?
	| [0-9]+ ('.' [0-9]+)? (
		[KDCM]? 'M' [23]?
		| 'M'? 'L'	 
		| 'K'? 'G' | 'T')
	;
STRING:
	'\'' (~['\\] | '\\' .)* '\''
	| '"' (~["\\] | '\\' .)* '"'
	| '`' (~[`\\] | '\\' .)* '`';
 
PARAMETER: ([A-Z_] | FullWidthLetter) ([A-Z0-9_] | FullWidthLetter)*;

fragment FullWidthLetter:
	'\u00c0' ..'\u00d6'
	| '\u00d8' ..'\u00f6'
	| '\u00f8' ..'\u00ff'
	| '\u0100' ..'\u1fff'
	| '\u2c00' ..'\u2fff'
	| '\u3040' ..'\u318f'
	| '\u3300' ..'\u337f'
	| '\u3400' ..'\u3fff'
	| '\u4e00' ..'\u9fff'
	| '\ua000' ..'\ud7ff'
	| '\uf900' ..'\ufaff'
	| '\uff00' ..'\ufff0'
	;

WS: [ \t\r\n\f]+ -> skip;
COMMENT: '/*' .*? '*/' -> skip;
LINE_COMMENT: '//' ~[\r\n]* -> skip;