grammar MathExcel;

prog : expr+;

expr:  NUM       
    | STRING
    | PARAMETER
    | expr op=('*'|'/'|'%') expr # MulDiv_fun
    | expr op=('+'|'-'|'&') expr # AddSub_fun
    | expr op=('='|'>'|'<'|'>='|'<='|'<>'|'!=') expr # Judge_fun
    | '{' expr (',' expr)* '}'
    | '(' expr ')'     # Bracket_fun

    | IF '(' expr ',' expr (',' expr )? ')'
    | IFERROR '(' expr ',' expr (',' expr )? ')'
    | IFNUMBER '(' expr ',' expr (',' expr )? ')'
    | IFTEXT '(' expr ',' expr (',' expr )? ')'
    | ISNUMBER '(' expr ')'
    | ISTEXT '(' expr ')'
    | ISTEXT '(' expr ')'
    | AND '(' expr (',' expr )* ')'
    | OR '(' expr (',' expr )* ')'
    | NOT '(' expr ')'
    | TRUE '(' ')'
    | FALSE '(' ')'

    | PI '(' ')'
    | ABS '(' expr ')'
    | QUOTIENT '(' expr (',' expr ) ')'
    | MOD '(' expr (',' expr ) ')'
    | SIGN '(' expr ')'
    | SQRT '(' expr ')'
    | TRUNC '(' expr ')'
    | GCD '(' expr (',' expr )+ ')'
    | LCM '(' expr (',' expr )+ ')'
    | COMBIN '(' expr ',' expr ')'
    | PERMUT '(' expr ',' expr ')'
    | DEGREES '(' expr ')'
    | RADIANS '(' expr ')'
    | COS '(' expr ')'
    | COSH '(' expr ')'
    | SIN '(' expr ')'
    | SINH '(' expr ')'
    | TAN '(' expr ')'
    | TANH '(' expr ')'
    | ACOS '(' expr ')'
    | ACOSH '(' expr ')'
    | ASIN '(' expr ')'
    | ASINH '(' expr ')'
    | ATAN '(' expr ')'
    | ATANH '(' expr ')'
    | ATAN2 '(' expr ')'
    | ROUND '(' expr ',' expr ')'
    | ROUNDDOWN '(' expr ',' expr ')'
    | ROUNDUP '(' expr ',' expr ')'
    | CEILING '(' expr ',' expr ')'
    | FLOOR '(' expr ',' expr ')'
    | EVEN '(' expr ')'
    | ODD '(' expr ')'
    | MROUND '(' expr ',' expr ')'
    | RAND '(' ')'
    | RANDBETWEEN '(' expr ',' expr ')'
    | FACT '(' expr ')'
    | FACTDOUBLE '(' expr ')'
    | POWER '(' expr ',' expr ')'
    | EXP '(' expr ')'
    | LN '(' expr ')'
    | LOG '(' expr ')'
    | LOG10 '(' expr ')'
    | MULTINOMIAL '(' expr (',' expr )* ')'
    | PRODUCT '(' expr (',' expr )* ')'
    | SQRTPI '(' expr ')'
    | SUMSQ '(' expr (',' expr )* ')'

    | ASC '(' expr ')'
    | (JIS | WIDECHAR) '(' expr ')'
    | CHAR '(' expr ')'
    | CLEAN '(' expr ')'
    | CODE '(' expr ')'
    | CONCATENATE '(' expr (',' expr )* ')'
    | EXACT '(' expr ',' expr ')'
    | FIND '(' expr ',' expr (',' expr)? ')'
    | FIXED '(' expr (',' expr (',' expr)?)? ')'
    | LEFT '(' expr (',' expr )? ')'
    | LEN '(' expr ')'
    | LOWER '(' expr ')'
    | MID '(' expr ',' expr ',' expr ')'
    | PROPER '(' expr ')'
    | REPLACE '(' expr ',' expr ',' expr ',' expr ')'
    | REPLACE '(' expr ',' expr ',' expr ')'
    | REPT '(' expr ',' expr ')'
    | RIGHT '(' expr (',' expr)? ')'
    | RMB '(' expr ')'
    | SEARCH '(' expr ',' expr (',' expr)? ')'
    | SUBSTITUTE '(' expr ',' expr ',' expr (',' expr)? ')'
    | T '(' expr ')'
    | TEXT '(' expr ',' expr ')'
    | TRIM '(' expr ')'
    | UPPER '(' expr ')'
    | VALUE '(' expr ')'

    | DATEVALUE '(' expr ')'
    | TIMEVALUE '(' expr ')'
    | DATE '(' expr ',' expr ',' expr (',' expr (',' expr (',' expr)?)?)? ')'
    | TIME '(' expr ',' expr ',' expr ')'
    | NOW '(' ')'
    | TODAY '(' ')'
    | YEAR '(' expr ')'
    | MONTH '(' expr ')'
    | DAY '(' expr ')'
    | HOUR '(' expr ')'
    | MINUTE '(' expr ')'
    | SECOND '(' expr ')'
    | WEEKDAY '(' expr ')'
    | DATEDIF '(' expr ',' expr ',' expr ')'
    | DAYS360 '(' expr ',' expr (',' expr)? ')'
    | EDATE '(' expr ',' expr ')'
    | EOMONTH '(' expr ',' expr ')'
    | NETWORKDAYS '(' expr ',' expr (',' expr)? ')'
    | WORKDAY '(' expr ',' expr (',' expr)? ')'
    | WEEKNUM '(' expr (',' expr)? ')'

    | MAX '(' expr ')'
    | MEDIAN '(' expr ')'
    | MIN '(' expr ')'
    | QUARTILE '(' expr ',' expr ')'
    | MODE '(' expr (',' expr)* ')'
    | LARGE '(' expr ',' expr ')'
    | SMALL '(' expr ',' expr ')'
    | PERCENTILE '(' expr ',' expr ')'
    | PERCENTRANK '(' expr ',' expr ')'
    | AVERAGE '(' expr (',' expr)* ')'
    | AVERAGEIF '(' expr (',' expr)* ')'
    | GEOMEAN '(' expr (',' expr)* ')'
    | HARMEAN '(' expr (',' expr)* ')'
    | COUNT '(' expr (',' expr)* ')'
    | COUNTIF '(' expr (',' expr)* ')'
    | SUM '(' expr (',' expr)* ')'
    | SUMIF '(' expr ',' expr ')'
    | AVEDEV '(' expr (',' expr)* ')'
    | STDEV '(' expr (',' expr)* ')'
    | STDEVP '(' expr (',' expr)* ')'
    | DEVSQ '(' expr (',' expr)* ')'
    | VAR '(' expr (',' expr)* ')'
    | VARP '(' expr (',' expr)* ')'
    | NORMDIST '(' expr ',' expr ',' expr ',' expr ')'
    | NORMINV '(' expr ',' expr ',' expr ')'
    | NORMSDIST '(' expr ')'
    | NORMSINV '(' expr ')'
    | BETADIST '(' expr ',' expr ',' expr ')'
    | BETAINV '(' expr ',' expr ',' expr ')'
    | BINOMDIST '(' expr ',' expr ',' expr ',' expr ')'
    | EXPONDIST '(' expr ',' expr ',' expr ')'
    | FDIST '(' expr ',' expr ',' expr ')'
    | FINV '(' expr ',' expr ',' expr ')'
    | FISHER '(' expr ')'
    | FISHERINV '(' expr ')'
    | GAMMADIST '(' expr ',' expr ',' expr ',' expr ')'
    | GAMMAINV '(' expr ',' expr ',' expr ')'
    | GAMMALN '(' expr ')'
    | HYPGEOMDIST '(' expr ',' expr ',' expr ',' expr ')'
    | LOGINV '(' expr ',' expr ',' expr ')'
    | LOGNORMDIST '(' expr ',' expr ',' expr ')'
    | NEGBINOMDIST '(' expr ',' expr ',' expr ')'
    | POISSON '(' expr ',' expr ',' expr ')'
    | TDIST '(' expr ',' expr ',' expr ')'
    | TINV '(' expr ',' expr ')'
    | WEIBULL '(' expr ',' expr ',' expr ')'

    | URLENCODE '(' expr ')'
    | URLDECODE '(' expr ')'
    | HTMLENCODE '(' expr ')'
    | HTMLDECODE '(' expr ')'
    | BASE64TOTEXT '(' expr (',' expr)? ')'
    | BASE64URLTOTEXT '(' expr (',' expr)? ')'
    | TEXTTOBASE64 '(' expr (',' expr)? ')'
    | TEXTTOBASE64URL '(' expr (',' expr)? ')'
    | REGEX '(' expr ',' expr (',' expr (',' expr)?)? ')'
    | REGEXREPALCE '(' expr ',' expr ',' expr ')'
    | ISREGEX '(' expr ',' expr ')'
    | ISMATCH '(' expr ',' expr ')'
    | GUID '(' ')'
    | MD5 '(' expr (',' expr)? ')'
    | SHA1 '(' expr (',' expr)? ')'
    | SHA256 '(' expr (',' expr)? ')'
    | SHA512 '(' expr (',' expr)? ')'
    | CRC8 '(' expr (',' expr)? ')'
    | CRC16 '(' expr (',' expr)? ')'
    | CRC32 '(' expr (',' expr)? ')'
    | HMACMD5 '(' expr ',' expr (',' expr)? ')'
    | HMACSHA1 '(' expr ',' expr (',' expr)? ')'
    | HMACSHA256 '(' expr ',' expr (',' expr)? ')'
    | HMACSHA512 '(' expr ',' expr (',' expr)? ')'
    | TRIMSTART '(' expr ')'
    | LTRIM '(' expr (',' expr)? ')'
    | TRIMEND '(' expr ')'
    | RTRIM '(' expr (',' expr)? ')'
    | INDEXOF '(' expr ',' expr (',' expr (',' expr)?)? ')'
    | LASTINDEXOF '(' expr ',' expr (',' expr (',' expr)?)? ')'
    | SPLIT '(' expr ',' expr (',' expr)? ')'
    | JOIN '(' expr ',' expr ')'
    | SUBSTRING '(' expr ',' expr (',' expr)? ')'
    | STARTSWITH '(' expr ',' expr (',' expr)? ')'
    | ENDSWITH '(' expr ',' expr (',' expr)? ')'
    | ISNULLOREMPTY '(' expr ')'
    | ISNULLORWHITESPACE '(' expr ')'
    | TOUPPER '(' expr ')'
    | TOLOWER '(' expr ')'
    | REMOVESTART '(' expr ',' expr ')'
    | REMOVEEND '(' expr ',' expr ')'
    | REMOVEBOTH '(' expr ',' expr ',' expr ')'
    | JSON '(' expr ')'
    | TRYJSON '(' expr ')'
    | (P| PARAM) '(' expr ')'
;


NUM : 0 ('.' [0-9]+)? 
 | [1-9][0-9]* ('.' [0-9]+)? 
 ;
STRING : '\'' ('\\' . | . ) '\''
 | '"' ('\\' . | .) '"' 
 ;
PARAMETER: '[' ~('[') ']';

MUL : '*' ; 
DIV : '/' ;
ADD : '+' ;
SUB : '-' ;
MOD : '%' ;
MERGE : '&' ;
POINT : '.' ;

// 逻辑函数
IF : 'IF' ;
IFERROR : 'IFERROR' ;
IFNUMBER : 'IFNUMBER' ;
IFTEXT : 'IFTEXT' ;
ISNUMBER : 'ISNUMBER' ;
ISTEXT : 'ISTEXT' ;
AND : 'AND' ;
OR : 'OR' ;
NOT : 'NOT' ;
TRUE : 'TRUE' ;
FALSE : 'FALSE' ;
// 数学与三角函数
PI : 'PI' ;
ABS : 'ABS' ;
QUOTIENT : 'QUOTIENT' ;
MOD : 'MOD' ;
SIGN : 'SIGN' ;
SQRT : 'SQRT' ;
TRUNC : 'TRUNC' ;
INT : 'INT' ;
GCD : 'GCD' ;
LCM : 'LCM' ;
COMBIN : 'COMBIN' ;
PERMUT : 'PERMUT' ;
DEGREES : 'DEGREES' ;
RADIANS : 'RADIANS' ;
COS : 'COS' ;
COSH : 'COSH' ;
SIN : 'SIN' ;
SINH : 'SINH' ;
TAN : 'TAN' ;
TANH : 'TANH' ;
ACOS : 'ACOS' ;
ACOSH : 'ACOSH' ;
ASIN : 'ASIN' ;
ASINH : 'ASINH' ;
ATAN : 'ATAN' ;
ATANH : 'ATANH' ;
ATAN2 : 'ATAN2' ;
ROUND : 'ROUND' ;
ROUNDDOWN : 'ROUNDDOWN' ;
ROUNDUP : 'ROUNDUP' ;
CEILING : 'CEILING' ;
FLOOR : 'FLOOR' ;
EVEN : 'EVEN' ;
ODD : 'ODD' ;
MROUND : 'MROUND' ;
RAND : 'RAND' ;
RANDBETWEEN : 'RANDBETWEEN' ;
FACT : 'FACT' ;
FACTDOUBLE : 'FACTDOUBLE' ;
POWER : 'POWER' ;
EXP : 'EXP' ;
LN : 'LN' ;
LOG : 'LOG' ;
LOG10 : 'LOG10' ;
MULTINOMIAL : 'MULTINOMIAL' ;
PRODUCT : 'PRODUCT' ;
SQRTPI : 'SQRTPI' ;
SUMSQ : 'SUMSQ' ;
// 文本函数
ASC : 'ASC' ;
JIS : 'JIS' ;
WIDECHAR : 'WIDECHAR' ;
CHAR : 'CHAR' ;
CLEAN : 'CLEAN' ;
CODE : 'CODE' ;
CONCATENATE : 'CONCATENATE' ;
EXACT : 'EXACT' ;
FIND : 'FIND' ;
FIXED : 'FIXED' ;
LEFT : 'LEFT' ;
LEN : 'LEN' ;
LOWER : 'LOWER' ;
MID : 'MID' ;
PROPER : 'PROPER' ;
REPLACE : 'REPLACE' ;
REPT : 'REPT' ;
RIGHT : 'RIGHT' ;
RMB : 'RMB' ;
SEARCH : 'SEARCH' ;
SUBSTITUTE : 'SUBSTITUTE' ;
T : 'T' ;
TEXT : 'TEXT' ;
TRIM : 'TRIM' ;
UPPER : 'UPPER' ;
VALUE : 'VALUE' ;
// 日期与时间函数
DATEVALUE : 'DATEVALUE' ;
TIMEVALUE : 'TIMEVALUE' ;
DATE : 'DATE' ;
TIME : 'TIME' ;
NOW : 'NOW' ;
TODAY : 'TODAY' ;
YEAR : 'YEAR' ;
MONTH : 'MONTH' ;
DAY : 'DAY' ;
HOUR : 'HOUR' ;
MINUTE : 'MINUTE' ;
SECOND : 'SECOND' ;
WEEKDAY : 'WEEKDAY' ;
DATEDIF : 'DATEDIF' ;
DAYS360 : 'DAYS360' ;
EDATE : 'EDATE' ;
EOMONTH : 'EOMONTH' ;
NETWORKDAYS : 'NETWORKDAYS' ;
WORKDAY : 'WORKDAY' ;
WEEKNUM : 'WEEKNUM' ;
// 统计函数
MAX : 'MAX' ;
MEDIAN : 'MEDIAN' ;
MIN : 'MIN' ;
QUARTILE : 'QUARTILE' ;
MODE : 'MODE' ;
LARGE : 'LARGE' ;
SMALL : 'SMALL' ;
PERCENTILE : 'PERCENTILE' ;
PERCENTRANK : 'PERCENTRANK' ;
AVERAGE : 'AVERAGE' ;
AVERAGEIF : 'AVERAGEIF' ;
GEOMEAN : 'GEOMEAN' ;
HARMEAN : 'HARMEAN' ;
COUNT : 'COUNT' ;
COUNTIF : 'COUNTIF' ;
SUM : 'SUM' ;
SUMIF : 'SUMIF' ;
AVEDEV : 'AVEDEV' ;
STDEV : 'STDEV' ;
STDEVP : 'STDEVP' ;
DEVSQ : 'DEVSQ' ;
VAR : 'VAR' ;
VARP : 'VARP' ;
NORMDIST : 'NORMDIST' ;
NORMINV : 'NORMINV' ;
NORMSDIST : 'NORMSDIST' ;
NORMSINV : 'NORMSINV' ;
BETADIST : 'BETADIST' ;
BETAINV : 'BETAINV' ;
BINOMDIST : 'BINOMDIST' ;
EXPONDIST : 'EXPONDIST' ;
FDIST : 'FDIST' ;
FINV : 'FINV' ;
FISHER : 'FISHER' ;
FISHERINV : 'FISHERINV' ;
GAMMADIST : 'GAMMADIST' ;
GAMMAINV : 'GAMMAINV' ;
GAMMALN : 'GAMMALN' ;
HYPGEOMDIST : 'HYPGEOMDIST' ;
LOGINV : 'LOGINV' ;
LOGNORMDIST : 'LOGNORMDIST' ;
NEGBINOMDIST : 'NEGBINOMDIST' ;
POISSON : 'POISSON' ;
TDIST : 'TDIST' ;
TINV : 'TINV' ;
WEIBULL : 'WEIBULL' ;
// 增加函数 类C#方法
URLENCODE : 'URLENCODE' ;
URLDECODE : 'URLDECODE' ;
HTMLENCODE : 'HTMLENCODE' ;
HTMLDECODE : 'HTMLDECODE' ;
BASE64TOTEXT : 'BASE64TOTEXT' ;
BASE64URLTOTEXT : 'BASE64URLTOTEXT' ;
TEXTTOBASE64 : 'TEXTTOBASE64' ;
TEXTTOBASE64URL : 'TEXTTOBASE64URL' ;
REGEX : 'REGEX' ;
REGEXREPALCE : 'REGEXREPALCE' ;
ISREGEX : 'ISREGEX' ;
ISMATCH : 'ISMATCH' ;
GUID : 'GUID' ;
MD5 : 'MD5' ;
SHA1 : 'SHA1' ;
SHA256 : 'SHA256' ;
SHA512 : 'SHA512' ;
CRC8 : 'CRC8' ;
CRC16 : 'CRC16' ;
CRC32 : 'CRC32' ;
HMACMD5 : 'HMACMD5' ;
HMACSHA1 : 'HMACSHA1' ;
HMACSHA256 : 'HMACSHA256' ;
HMACSHA512 : 'HMACSHA512' ;
TRIMSTART : 'TRIMSTART' ;
LTRIM : 'LTRIM' ;
TRIMEND : 'TRIMEND' ;
RTRIM : 'RTRIM' ;
INDEXOF : 'INDEXOF' ;
LASTINDEXOF : 'LASTINDEXOF' ;
SPLIT : 'SPLIT' ;
JOIN : 'JOIN' ;
SUBSTRING : 'SUBSTRING' ;
STARTSWITH : 'STARTSWITH' ;
ENDSWITH : 'ENDSWITH' ;
ISNULLOREMPTY : 'ISNULLOREMPTY' ;
ISNULLORWHITESPACE : 'ISNULLORWHITESPACE' ;
TOUPPER : 'TOUPPER' ;
TOLOWER : 'TOLOWER' ;
REMOVESTART : 'REMOVESTART' ;
REMOVEEND : 'REMOVEEND' ;
REMOVEBOTH : 'REMOVEBOTH' ;
JSON : 'JSON' ;
TRYJSON : 'TRYJSON' ;
P : 'P' ;
PARAM : 'PARAM' ;

WS : [ \t\r\n]+ -> skip;