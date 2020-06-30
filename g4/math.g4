grammar math;

prog : expr+;

expr: expr op=('*'|'/'|'%') expr                        # MulDiv_fun
    | expr op=('+'|'-'|'&') expr                        # AddSub_fun
    | expr op=('='|'>'|'<'|'>='|'<='|'<>'|'!=') expr    # Judge_fun
    | '{' expr (',' expr)* '}'                          # Array_fun
    | '(' expr ')'                                      # Bracket_fun

    | IF '(' expr ',' expr (',' expr )? ')'             #IF_fun  
    | IFERROR '(' expr ',' expr (',' expr )? ')'        #IFERROR_fun  
    | IFNUMBER '(' expr ',' expr (',' expr )? ')'       #IFNUMBER_fun  
    | IFTEXT '(' expr ',' expr (',' expr )? ')'         #IFTEXT_fun  
    | ISNUMBER '(' expr ')'                             #ISNUMBER_fun 
    | ISTEXT '(' expr ')'                               #ISTEXT_fun 
    | AND '(' expr (',' expr )* ')' #AND_fun  
    | OR '(' expr (',' expr )* ')'  #OR_fun 
    | NOT '(' expr ')'  #NOT_fun 
    | TRUE '(' ')'  #TRUE_fun 
    | FALSE '(' ')' #FALSE_fun 

    | PI '(' ')'    #PI_fun 
    | ABS '(' expr ')'  #ABS_fun 
    | QUOTIENT '(' expr (',' expr ) ')' #QUOTIENT_fun  
    | MOD '(' expr (',' expr ) ')'  #MOD_fun 
    | SIGN '(' expr ')' #SIGN_fun 
    | SQRT '(' expr ')' #SQRT_fun 
    | TRUNC '(' expr ')'    #TRUNC_fun 
    | GCD '(' expr (',' expr )+ ')' #GCD_fun 
    | LCM '(' expr (',' expr )+ ')' #LCM_fun 
    | COMBIN '(' expr ',' expr ')'  #COMBIN_fun 
    | PERMUT '(' expr ',' expr ')'  #PERMUT_fun 
    | DEGREES '(' expr ')'  #DEGREES_fun 
    | RADIANS '(' expr ')'  #RADIANS_fun 
    | COS '(' expr ')'  #COS_fun 
    | COSH '(' expr ')'  #COSH_fun 
    | SIN '(' expr ')'  #SIN_fun 
    | SINH '(' expr ')'  #SINH_fun 
    | TAN '(' expr ')'  #TAN_fun 
    | TANH '(' expr ')'  #TANH_fun 
    | ACOS '(' expr ')'  #ACOS_fun 
    | ACOSH '(' expr ')'  #ACOSH_fun 
    | ASIN '(' expr ')'  #ASIN_fun 
    | ASINH '(' expr ')'  #ASINH_fun 
    | ATAN '(' expr ')'  #ATAN_fun 
    | ATANH '(' expr ')'  #ATANH_fun 
    | ATAN2 '(' expr ')'  #ATAN2_fun 
    | ROUND '(' expr ',' expr ')'  #ROUND_fun 
    | ROUNDDOWN '(' expr ',' expr ')'  #ROUNDDOWN_fun 
    | ROUNDUP '(' expr ',' expr ')'  #ROUNDUP_fun 
    | CEILING '(' expr ',' expr ')'  #CEILING_fun 
    | FLOOR '(' expr ',' expr ')'  #FLOOR_fun 
    | EVEN '(' expr ')'  #EVEN_fun 
    | ODD '(' expr ')'  #ODD_fun 
    | MROUND '(' expr ',' expr ')'  #MROUND_fun 
    | RAND '(' ')'  #RAND_fun 
    | RANDBETWEEN '(' expr ',' expr ')'  #RANDBETWEEN_fun 
    | FACT '(' expr ')'  #FACT_fun 
    | FACTDOUBLE '(' expr ')'  #FACTDOUBLE_fun 
    | POWER '(' expr ',' expr ')'  #POWER_fun 
    | EXP '(' expr ')'  #EXP_fun 
    | LN '(' expr ')'  #LN_fun 
    | LOG '(' expr ')'  #LOG_fun 
    | LOG10 '(' expr ')'  #LOG10_fun 
    | MULTINOMIAL '(' expr (',' expr )* ')'  #MULTINOMIAL_fun 
    | PRODUCT '(' expr (',' expr )* ')'  #PRODUCT_fun 
    | SQRTPI '(' expr ')'  #SQRTPI_fun 
    | SUMSQ '(' expr (',' expr )* ')'  #SUMSQ_fun 

    | ASC '(' expr ')'  #ASC_fun 
    | (JIS | WIDECHAR) '(' expr ')'  #JIS_fun 
    | CHAR '(' expr ')'  #CHAR_fun 
    | CLEAN '(' expr ')'  #CLEAN_fun 
    | CODE '(' expr ')'  #CODE_fun 
    | CONCATENATE '(' expr (',' expr )* ')'  #CONCATENATE_fun 
    | EXACT '(' expr ',' expr ')'  #EXACT_fun 
    | FIND '(' expr ',' expr (',' expr)? ')'  #FIND_fun 
    | FIXED '(' expr (',' expr (',' expr)?)? ')'  #FIXED_fun 
    | LEFT '(' expr (',' expr )? ')'  #LEFT_fun 
    | LEN '(' expr ')'  #LEN_fun 
    | LOWER '(' expr ')'  #LOWER_fun 
    | MID '(' expr ',' expr ',' expr ')'  #MID_fun 
    | PROPER '(' expr ')'  #PROPER_fun 
    | REPLACE '(' expr ',' expr ',' expr ',' expr ')'  #REPLACE_fun 
    | REPLACE '(' expr ',' expr ',' expr ')'  #REPLACE_fun 
    | REPT '(' expr ',' expr ')'  #REPT_fun 
    | RIGHT '(' expr (',' expr)? ')'  #RIGHT_fun 
    | RMB '(' expr ')'  #RMB_fun 
    | SEARCH '(' expr ',' expr (',' expr)? ')'  #SEARCH_fun 
    | SUBSTITUTE '(' expr ',' expr ',' expr (',' expr)? ')'  #SUBSTITUTE_fun 
    | T '(' expr ')'  #T_fun 
    | TEXT '(' expr ',' expr ')'  #TEXT_fun 
    | TRIM '(' expr ')'  #TRIM_fun 
    | UPPER '(' expr ')'  #UPPER_fun 
    | VALUE '(' expr ')'  #VALUE_fun 

    | DATEVALUE '(' expr ')'  #DATEVALUE_fun 
    | TIMEVALUE '(' expr ')'  #TIMEVALUE_fun 
    | DATE '(' expr ',' expr ',' expr (',' expr (',' expr (',' expr)?)?)? ')'  #DATE_fun 
    | TIME '(' expr ',' expr ',' expr ')'  #TIME_fun 
    | NOW '(' ')'  #NOW_fun 
    | TODAY '(' ')'  #TODAY_fun 
    | YEAR '(' expr ')'  #YEAR_fun 
    | MONTH '(' expr ')'  #MONTH_fun 
    | DAY '(' expr ')'  #DAY_fun 
    | HOUR '(' expr ')'  #HOUR_fun 
    | MINUTE '(' expr ')'  #MINUTE_fun 
    | SECOND '(' expr ')'  #SECOND_fun 
    | WEEKDAY '(' expr ')'  #WEEKDAY_fun 
    | DATEDIF '(' expr ',' expr ',' expr ')'  #DATEDIF_fun 
    | DAYS360 '(' expr ',' expr (',' expr)? ')'  #DAYS360_fun 
    | EDATE '(' expr ',' expr ')'  #EDATE_fun 
    | EOMONTH '(' expr ',' expr ')'  #EOMONTH_fun 
    | NETWORKDAYS '(' expr ',' expr (',' expr)? ')'  #NETWORKDAYS_fun 
    | WORKDAY '(' expr ',' expr (',' expr)? ')'  #WORKDAY_fun 
    | WEEKNUM '(' expr (',' expr)? ')'  #WEEKNUM_fun 

    | MAX '(' expr ')'  #MAX_fun 
    | MEDIAN '(' expr ')'  #MEDIAN_fun 
    | MIN '(' expr ')'  #MIN_fun 
    | QUARTILE '(' expr ',' expr ')'  #QUARTILE_fun 
    | MODE '(' expr (',' expr)* ')'  #MODE_fun 
    | LARGE '(' expr ',' expr ')'  #LARGE_fun 
    | SMALL '(' expr ',' expr ')'  #SMALL_fun 
    | PERCENTILE '(' expr ',' expr ')'  #PERCENTILE_fun 
    | PERCENTRANK '(' expr ',' expr ')'  #PERCENTRANK_fun 
    | AVERAGE '(' expr (',' expr)* ')'  #AVERAGE_fun 
    | AVERAGEIF '(' expr (',' expr)* ')'  #AVERAGEIF_fun 
    | GEOMEAN '(' expr (',' expr)* ')'  #GEOMEAN_fun 
    | HARMEAN '(' expr (',' expr)* ')'  #HARMEAN_fun 
    | COUNT '(' expr (',' expr)* ')'  #COUNT_fun 
    | COUNTIF '(' expr (',' expr)* ')'  #COUNTIF_fun 
    | SUM '(' expr (',' expr)* ')'  #SUM_fun 
    | SUMIF '(' expr ',' expr ')'  #SUMIF_fun 
    | AVEDEV '(' expr (',' expr)* ')'  #AVEDEV_fun 
    | STDEV '(' expr (',' expr)* ')'  #STDEV_fun 
    | STDEVP '(' expr (',' expr)* ')'  #STDEVP_fun 
    | DEVSQ '(' expr (',' expr)* ')'  #DEVSQ_fun 
    | VAR '(' expr (',' expr)* ')'  #VAR_fun 
    | VARP '(' expr (',' expr)* ')'  #VARP_fun 
    | NORMDIST '(' expr ',' expr ',' expr ',' expr ')'  #NORMDIST_fun 
    | NORMINV '(' expr ',' expr ',' expr ')'  #NORMINV_fun 
    | NORMSDIST '(' expr ')'  #NORMSDIST_fun 
    | NORMSINV '(' expr ')'  #NORMSINV_fun 
    | BETADIST '(' expr ',' expr ',' expr ')'  #BETADIST_fun 
    | BETAINV '(' expr ',' expr ',' expr ')'  #BETAINV_fun 
    | BINOMDIST '(' expr ',' expr ',' expr ',' expr ')'  #BINOMDIST_fun 
    | EXPONDIST '(' expr ',' expr ',' expr ')'  #EXPONDIST_fun 
    | FDIST '(' expr ',' expr ',' expr ')'  #FDIST_fun 
    | FINV '(' expr ',' expr ',' expr ')'  #FINV_fun 
    | FISHER '(' expr ')'  #FISHER_fun 
    | FISHERINV '(' expr ')'  #FISHERINV_fun 
    | GAMMADIST '(' expr ',' expr ',' expr ',' expr ')'  #GAMMADIST_fun 
    | GAMMAINV '(' expr ',' expr ',' expr ')'  #GAMMAINV_fun 
    | GAMMALN '(' expr ')'  #GAMMALN_fun 
    | HYPGEOMDIST '(' expr ',' expr ',' expr ',' expr ')'  #HYPGEOMDIST_fun 
    | LOGINV '(' expr ',' expr ',' expr ')'  #LOGINV_fun 
    | LOGNORMDIST '(' expr ',' expr ',' expr ')'  #LOGNORMDIST_fun 
    | NEGBINOMDIST '(' expr ',' expr ',' expr ')'  #NEGBINOMDIST_fun 
    | POISSON '(' expr ',' expr ',' expr ')'  #POISSON_fun 
    | TDIST '(' expr ',' expr ',' expr ')'  #TDIST_fun 
    | TINV '(' expr ',' expr ')'  #TINV_fun 
    | WEIBULL '(' expr ',' expr ',' expr ')'  #WEIBULL_fun 

    | URLENCODE '(' expr ')'  #URLENCODE_fun 
    | URLDECODE '(' expr ')'  #URLDECODE_fun 
    | HTMLENCODE '(' expr ')'  #HTMLENCODE_fun 
    | HTMLDECODE '(' expr ')'  #HTMLDECODE_fun 
    | BASE64TOTEXT '(' expr (',' expr)? ')'  #BASE64TOTEXT_fun 
    | BASE64URLTOTEXT '(' expr (',' expr)? ')'  #BASE64URLTOTEXT_fun 
    | TEXTTOBASE64 '(' expr (',' expr)? ')'  #TEXTTOBASE64_fun 
    | TEXTTOBASE64URL '(' expr (',' expr)? ')'  #TEXTTOBASE64URL_fun 
    | REGEX '(' expr ',' expr (',' expr (',' expr)?)? ')'  #REGEX_fun 
    | REGEXREPALCE '(' expr ',' expr ',' expr ')'  #REGEXREPALCE_fun 
    | ISREGEX '(' expr ',' expr ')'  #ISREGEX_fun 
    | ISMATCH '(' expr ',' expr ')'  #ISMATCH_fun 
    | GUID '(' ')'  #GUID_fun 
    | MD5 '(' expr (',' expr)? ')'  #MD5_fun 
    | SHA1 '(' expr (',' expr)? ')'  #SHA1_fun 
    | SHA256 '(' expr (',' expr)? ')'  #SHA256_fun 
    | SHA512 '(' expr (',' expr)? ')'  #SHA512_fun 
    | CRC8 '(' expr (',' expr)? ')'  #CRC8_fun 
    | CRC16 '(' expr (',' expr)? ')'  #CRC16_fun 
    | CRC32 '(' expr (',' expr)? ')'  #CRC32_fun 
    | HMACMD5 '(' expr ',' expr (',' expr)? ')'  #HMACMD5_fun 
    | HMACSHA1 '(' expr ',' expr (',' expr)? ')'  #HMACSHA1_fun 
    | HMACSHA256 '(' expr ',' expr (',' expr)? ')'  #HMACSHA256_fun 
    | HMACSHA512 '(' expr ',' expr (',' expr)? ')'  #HMACSHA512_fun 
    | TRIMSTART '(' expr ')'  #TRIMSTART_fun 
    | LTRIM '(' expr (',' expr)? ')'  #LTRIM_fun 
    | TRIMEND '(' expr ')'  #TRIMEND_fun 
    | RTRIM '(' expr (',' expr)? ')'  #RTRIM_fun 
    | INDEXOF '(' expr ',' expr (',' expr (',' expr)?)? ')'  #INDEXOF_fun 
    | LASTINDEXOF '(' expr ',' expr (',' expr (',' expr)?)? ')'  #LASTINDEXOF_fun 
    | SPLIT '(' expr ',' expr (',' expr)? ')'  #SPLIT_fun 
    | JOIN '(' expr ',' expr ')'  #JOIN_fun 
    | SUBSTRING '(' expr ',' expr (',' expr)? ')'  #SUBSTRING_fun 
    | STARTSWITH '(' expr ',' expr (',' expr)? ')'  #STARTSWITH_fun 
    | ENDSWITH '(' expr ',' expr (',' expr)? ')'  #ENDSWITH_fun 
    | ISNULLOREMPTY '(' expr ')'  #ISNULLOREMPTY_fun 
    | ISNULLORWHITESPACE '(' expr ')'  #ISNULLORWHITESPACE_fun 
    | TOUPPER '(' expr ')'  #TOUPPER_fun 
    | TOLOWER '(' expr ')'  #TOLOWER_fun 
    | REMOVESTART '(' expr ',' expr ')'  #REMOVESTART_fun 
    | REMOVEEND '(' expr ',' expr ')'  #REMOVEEND_fun 
    | REMOVEBOTH '(' expr ',' expr ',' expr ')'  #REMOVEBOTH_fun 
    | JSON '(' expr ')'  #JSON_fun 
    | TRYJSON '(' expr ')'  #TRYJSON_fun 
    | (P| PARAM) '(' expr ')'  #P_fun 
    | NUM         # NUM_fun
    | STRING  # STRING_fun
    | PARAMETER  # PARAMETER_fun
;


NUM : '-'? '0' ('.' [0-9]+)? 
 | '-'? [1-9][0-9]* ('.' [0-9]+)? 
 ;
STRING : '\'' ('\\' . | . ) '\''
 | '"' ('\\' . | .) '"' 
 ;
PARAMETER: '[' ~('[') ']';

MUL : '*' ; 
DIV : '/' ;
ADD : '+' ;
SUB : '-' ;
MOD_2 : '%' ;
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