grammar mathjs;

prog: expr EOF;

expr:
	'(' expr ')'												
	| expr '.'  PARAMETER '(' (expr (',' expr)*)? ')'			
	| expr ('[' (PARAMETER | expr) ']' | '.' parameter2)			
	// 运算符优先级 开始
	| '!' expr													
	| expr '%'													
	| expr (OPMUL | OPDIV | OPMOD) expr							
	| expr (OPADD | OPSUB | OPCAT) expr							
	| expr (OPGT | OPGE | OPLT | OPLE) expr					
	| expr (OPNE | OPEQ ) expr	
	| expr OPAND expr										
	| expr OPOR expr										
	| expr '?' expr ':' expr				
	// 运算符优先级 结束

	| (TRUE | FALSE) ('(' ')')?								
	| (RAND | NOW | TODAY | GUID | E | PI) '(' ')'			
	| PARAMETER '(' expr (',' expr)* ')'						
	| '{' arrayJson (',' arrayJson)* ','? '}'					
	| '[' expr (',' expr)* ','? ']'		
	| OPSUB? NUM										
	| STRING													
	| PARAMETER								
	| ALGORITHMVERSION											
	| NULL														
	;

arrayJson: key=(NUM | STRING) ':' expr
	| parameter2 ':' expr;

parameter2:
	E | PI | TRUE | FALSE | NULL
	| RAND | NOW | TODAY | GUID
	| ALGORITHMVERSION
	| PARAMETER;
 
OPADD: '+';
OPSUB: '-';
OPMUL: '*';
OPDIV: '/';
OPMOD: '%';
OPCAT :'&';
OPGT: '>';
OPGE: '>=';
OPLT: '<';
OPLE: '<=';
OPNE: '!=' | '!==' | '<>';
OPEQ: '=' | '==' | '===';
OPAND: '&&';
OPOR: '||';

NUM: [0-9]+ ('.' [0-9]+)? ('E' [+-]? [0-9]+)?
	| [0-9]+ ('.' [0-9]+)? ('M'
		| 'KM'	| 'DM'	| 'CM'	| 'MM'
		| 'M2'	| 'KM2'	| 'DM2'	| 'CM2'	| 'MM2'
		| 'M3'	| 'KM3'	| 'DM3'	| 'CM3'	| 'MM3'	| 'L'	| 'ML'
		| 'G'	| 'KG' | 'T')
	;

STRING:
	'\'' (~['\\] | '\\' .)* '\''
	| '"' (~["\\] | '\\' .)* '"'
	| '`' (~[`\\] | '\\' .)* '`';

// BOOL_fun
TRUE: 'TRUE' | 'YES';
FALSE: 'FALSE' | 'NO';

// CONST_fun
RAND: 'RAND';
NOW: 'NOW';
TODAY: 'TODAY';
GUID: 'GUID';
E: 'E';
PI: 'PI';

ALGORITHMVERSION: 'ALGORITHMVERSION' | 'ENGINEVERSION';
NULL: 'NULL';
PARAMETER: ([A-Z_] | FullWidthLetter) ( [A-Z0-9_] | FullWidthLetter	)*;

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
	// | '\u10000'..'\u1F9FF' //not support four bytes chars | '\u20000'..'\u2FA1F'
	;

WS: [ \t\r\n\f]+ -> skip;
COMMENT: '/*' .*? '*/' -> skip;
LINE_COMMENT: '//' ~[\r\n]* -> skip;