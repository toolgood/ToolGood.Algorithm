grammar mathjs;

prog: expr EOF;

expr:
	expr '.' T '(' ')'											# DiyFunction_fun
	| expr '.' AND '(' ')'										# DiyFunction_fun
	| expr '.' OR '(' ')'										# DiyFunction_fun
	| expr '.' PARAMETER '(' (expr (',' expr)*)? ')'			# DiyFunction_fun
	| expr '[' PARAMETER ']'									# GetJsonValue_fun
	| expr '[' expr ']'											# GetJsonValue_fun
	| expr '.' parameter2										# GetJsonValue_fun

	// 运算符优先级 开始
	| '(' expr ')'												# Bracket_fun
	| '!' expr													# NOT_fun
	| expr '%'													# Percentage_fun
	| expr op = ('*' | '/' | '%') expr							# MulDiv_fun
	| expr op = ('+' | '-' | '&') expr							# AddSub_fun
	| expr op = ('>' | '>=' | '<' | '<=') expr					# Judge_fun
	| expr op = ('=' | '==' | '===' | '!==' | '!=' | '<>') expr	# Judge_fun
	| expr op = '&&'  expr										# AndOr_fun
	| expr op = '||'  expr										# AndOr_fun
	| expr '?' expr ':' expr									# DiyFunction_fun
	| T '(' expr ')'											# DiyFunction_fun
	| AND '(' (expr (',' expr)*)? ')'							# DiyFunction_fun
	| OR '(' (expr (',' expr)*)? ')'							# DiyFunction_fun
	| PARAMETER '(' (expr (',' expr)*)? ')'						# DiyFunction_fun
	| '{' arrayJson (',' arrayJson)* ','* '}'					# ArrayJson_fun
	| '[' expr (',' expr)* ','* ']'								# Array_fun
	| PARAMETER													# PARAMETER_fun
	| num unit=(UNIT | T)										# NUM_fun
	| STRING													# STRING_fun
	| NULL														# NULL_fun
	;

num: '-'? NUM;
arrayJson: key=(NUM | STRING) ':' expr
	| parameter2 ':' expr;

parameter2:
	NULL
	| T
	| AND
	| OR
	| UNIT
	| PARAMETER;
 

SUB: '-';
NUM:
	'0' ('.' [0-9]+)?
	| [1-9][0-9]* ('.' [0-9]+)?
	| ('0' ('.' [0-9]+)? | [1-9][0-9]* ('.' [0-9]+)?) 'E' [+-]? [0-9][0-9]?;
STRING:
	'\'' (~'\'' | '\\\'')* '\''
	| '"' ( ~'"' | '\\"')* '"'
	| '`' ( ~'`' | '\\`')* '`';
NULL: 'NULL';

UNIT:
	'M'
	| 'KM'
	| 'DM'
	| 'CM'
	| 'MM'
	| 'M2'
	| 'KM2'
	| 'DM2'
	| 'CM2'
	| 'MM2'
	| 'M3'
	| 'KM3'
	| 'DM3'
	| 'CM3'
	| 'MM3'
	| 'L'
	| 'ML'
	| 'G'
	| 'KG';

T: 'T';
AND: 'AND';
OR: 'OR';

PARAMETER:'PERCENTILE.INC'
	|'PERCENTRANK.INC'
	|'STDEV.S'
	|'STDEV.P'
	|'COVARIANCE.P'
	|'COVARIANCE.S'
	|'VAR.S'
	|'VAR.P'
	|'NORM.DIST'
	|'NORM.INV'
	|'NORM.S.DIST'
	|'NORM.S.INV'
	|'BETA.DIST'
	|'BETA.INV'
	|'BINOM.DIST'
	|'EXPON.DIST'
	|'F.DIST'
	|'F.INV'
	|'GAMMA.DIST'
	|'GAMMA.INV'
	|'GAMMALN.PRECISE'
	|'HYPGEOM.DIST'
	|'LOGNORM.INV'
	|'LOGNORM.DIST'
	|'NEGBINOM.DIST'
	|'POISSON.DIST'
	|'T.DIST'
	|'T.INV'
	| ([A-Z_] | FullWidthLetter) ( [A-Z0-9_] | FullWidthLetter)*;

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

WS: [ \t\r\n\u000C]+ -> skip;
COMMENT: '/*' .*? '*/' -> skip;
LINE_COMMENT: '//' ~[\r\n]* -> skip;