grammar math;

prog: expr EOF;

expr:
	expr '.' f=(ISNUMBER | ISTEXT | ISNONTEXT | ISLOGICAL) '(' ')'	# IS_fun
	| expr '.' f=(ISERROR | ISNULL | ISNULLORERROR) '(' expr? ')'	# ISNULL_check_fun
	| expr '.' INT '(' ')'										# INT_fun
	| expr '.' EXACT '(' expr ')'								# EXACT_fun
	| expr '.' f=(LEFT | RIGHT) '(' expr? ')'					# LR_fun
	| expr '.' LEN '(' ')'										# LEN_fun
	| expr '.' f=(LOWER | UPPER) '(' ')'						# CASE_fun
	| expr '.' MID '(' expr ',' expr ')'						# MID_fun
	| expr '.' REPLACE '(' expr ',' expr (',' expr)? ')'		# REPLACE_fun
	| expr '.' RMB '(' ')'										# RMB_fun
	| expr '.' T '(' ')'										# T_fun
	| expr '.' TEXT '(' expr ')'								# TEXT_fun
	| expr '.' TRIM '(' ')'										# TRIM_fun
	| expr '.' VALUE '(' ')'									# VALUE_fun
	| expr '.' DATEVALUE '(' expr? ')'							# DATEVALUE_fun
	| expr '.' TIMEVALUE '(' ')'								# TIMEVALUE_fun
	| expr '.' f=(YEAR | MONTH | DAY | HOUR | MINUTE | SECOND) '(' ')'	# DATE_TIME_fun
	| expr '.' f=(URLENCODE | URLDECODE) '(' ')'				# ENCODE_fun
	| expr '.' REGEX '(' expr ')'								# REGEX_fun
	| expr '.' REGEXREPLACE '(' expr ',' expr ')'				# REGEXREPLACE_fun
	| expr '.' ISREGEX '(' expr ')'								# ISREGEX_fun
	| expr '.' f=(MD5 | SHA1 | SHA256 | SHA512) '(' ')'			# HASH_fun
	| expr '.' f=(TRIMSTART | TRIMEND) '(' expr? ')'				# TRIM_SE_fun
	| expr '.' f=(INDEXOF | LASTINDEXOF) '(' expr (',' expr (',' expr)?)? ')'	# INDEXOF_fun
	| expr '.' SPLIT '(' expr ')'								# SPLIT_fun
	| expr '.' JOIN '(' expr (',' expr)* ')'					# JOIN_fun
	| expr '.' SUBSTRING '(' expr (',' expr)? ')'				# SUBSTRING_fun
	| expr '.' f=(STARTSWITH | ENDSWITH) '(' expr (',' expr)? ')'	# STRINGSuffix_fun
	| expr '.' f=(ISNULLOREMPTY | ISNULLORWHITESPACE) '(' ')'	# ISNULLOR_fun
	| expr '.' f=(REMOVESTART | REMOVEEND) '(' expr (',' expr)? ')'	# REMOVE_fun
	| expr '.' JSON '(' ')'										# JSON_fun
	| expr '.' PARAMETER '(' (expr (',' expr)*)? ')'			# DiyFunction_fun
	| expr '.' f=(ADDYEARS | ADDMONTHS | ADDDAYS | ADDHOURS | ADDMINUTES | ADDSECONDS) '(' expr ')'	# ADD_DateTime_fun
	| expr '.' TIMESTAMP '(' expr? ')'							# TIMESTAMP_fun
	| expr '.' HAS '(' expr ')'									# HAS_fun
	| expr '.' HASVALUE '(' expr ')'							# HASVALUE_fun
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
	| expr op = '&&' expr										# AndOr_fun
	| expr op = '||' expr										# AndOr_fun
	| expr '?' expr ':' expr									# IF_fun
	// 运算符优先级 结束
	| ARRAY '(' expr (',' expr)* ')'						# Array_fun
	| IF '(' expr ',' expr (',' expr)? ')'					# IF_fun
	| IFS '(' expr ',' expr (',' expr ',' expr)* ')'		# IFS_fun
	| SWITCH '(' expr ',' expr ',' expr ( ',' expr)* ')'	# SWITCH_fun
	| f=(ISNUMBER | ISTEXT | ISNONTEXT | ISLOGICAL | ISEVEN | ISODD) '(' expr ')'	# IS_fun
	| f=(ISERROR | ISNULL | ISNULLORERROR) '(' expr (',' expr)? ')'	# ISNULL_check_fun
	| IFERROR '(' expr ',' expr (',' expr)? ')'				# IFERROR_fun
	| f=(AND | OR | XOR) '(' expr (',' expr)* ')'				# LOGIC_fun
	| NOT '(' expr ')'										# NOT_fun
	| TRUE ('(' ')')?										# TRUE_fun
	| FALSE ('(' ')')?										# FALSE_fun
	| E ('(' ')')											# E_fun
	| PI ('(' ')')											# PI_fun

	
	| f=(DEC2BIN | DEC2HEX | DEC2OCT | HEX2BIN | HEX2DEC | HEX2OCT | OCT2BIN | OCT2DEC | OCT2HEX | BIN2OCT | BIN2DEC | BIN2HEX) ('(' expr (',' expr)? ')')	# Convert_fun
	| ABS '(' expr ')'										# ABS_fun
	| SIGN '(' expr ')'										# SIGN_fun
	| SQRT '(' expr ')'										# SQRT_fun
	| INT '(' expr ')'										# INT_fun
	| QUOTIENT '(' expr ',' expr ')'						# QUOTIENT_fun
	| MOD '(' expr ',' expr ')'								# MOD_fun
	| TRUNC '(' expr(',' expr)? ')'							# TRUNC_fun
	| GCD '(' expr (',' expr)* ')'							# GCD_fun
	| LCM '(' expr (',' expr)* ')'							# LCM_fun
	| COMBIN '(' expr ',' expr ')'							# COMBIN_fun
	| PERMUT '(' expr ',' expr ')'							# PERMUT_fun
	| f=(DEGREES | RADIANS | COS | COSH | SIN | SINH | TAN | TANH | COT | COTH | CSC | CSCH | SEC | SECH | ACOS | ACOSH | ASIN | ASINH | ATAN | ATANH | ACOT | ACOTH) '(' expr ')'	# TRIG_fun
	| ATAN2 '(' expr ',' expr ')'							# ATAN2_fun
	| f=(ROUNDDOWN | ROUNDUP) '(' expr ',' expr ')'			# ROUND_UD_fun
	| f=(ROUND | CEILING | FLOOR) '(' expr (',' expr)? ')'		# ROUND_fun
	| EVEN '(' expr ')'										# EVEN_fun
	| ODD '(' expr ')'										# ODD_fun
	| MROUND '(' expr ',' expr ')'							# MROUND_fun
	| RAND '(' ')'											# RAND_fun
	| RANDBETWEEN '(' expr ',' expr ')'						# RANDBETWEEN_fun
	| FACT '(' expr ')'										# FACT_fun
	| FACTDOUBLE '(' expr ')'								# FACTDOUBLE_fun
	| POWER '(' expr ',' expr ')'							# POWER_fun
	| EXP '(' expr ')'										# EXP_fun
	| LN '(' expr ')'										# LN_fun
	| LOG '(' expr (',' expr)? ')'							# LOG_fun
	| LOG10 '(' expr ')'									# LOG10_fun
	| MULTINOMIAL '(' expr (',' expr)* ')'					# MULTINOMIAL_fun
	| PRODUCT '(' expr (',' expr)* ')'						# PRODUCT_fun
	| SQRTPI '(' expr ')'									# SQRTPI_fun
	| ERF '(' expr ')'										# ERF_fun
	| ERFC '(' expr ')'										# ERFC_fun
	| BESSELI '(' expr ',' expr ')'							# BESSELI_fun
	| BESSELJ '(' expr ',' expr ')'							# BESSELJ_fun
	| BESSELK '(' expr ',' expr ')'							# BESSELK_fun
	| BESSELY '(' expr ',' expr ')'							# BESSELY_fun
	| DELTA '(' expr (',' expr)? ')'						# DELTA_fun
	| GESTEP '(' expr (',' expr)? ')'						# GESTEP_fun
	| SUMSQ '(' expr (',' expr)* ')'						# SUMSQ_fun
	| SUMPRODUCT '(' expr (',' expr)* ')'				# SUMPRODUCT_fun
	| SUMX2MY2 '(' expr ',' expr ')'					# SUMX2MY2_fun
	| SUMX2PY2 '(' expr ',' expr ')'					# SUMX2PY2_fun
	| SUMXMY2 '(' expr ',' expr ')'						# SUMXMY2_fun
	| ARABIC '(' expr ')'								# ARABIC_fun
	| ROMAN '(' expr (',' expr)? ')'					# ROMAN_fun
	| SERIESSUM '(' expr ',' expr ',' expr ',' expr ')'	# SERIESSUM_fun
	| RANK '(' expr ',' expr (',' expr)? ')'			# RANK_fun
	| FORECAST '(' expr ',' expr ',' expr ')'			# FORECAST_fun
	| INTERCEPT '(' expr ',' expr ')'					# INTERCEPT_fun
	| SLOPE '(' expr ',' expr ')'						# SLOPE_fun
	| CORREL '(' expr ',' expr ')'						# CORREL_fun
	| PEARSON '(' expr ',' expr ')'						# PEARSON_fun
	| YEARFRAC '(' expr ',' expr (',' expr)? ')'		# YEARFRAC_fun
	| ASC '(' expr ')'									# ASC_fun
	| JIS '(' expr ')'										# JIS_fun
	| CHAR '(' expr ')'										# CHAR_fun
	| CLEAN '(' expr ')'									# CLEAN_fun
	| CODE '(' expr ')'										# CODE_fun
	| f=(UNICHAR | UNICODE) '(' expr ')'						# UNICODE_fun
	| CONCATENATE '(' expr (',' expr)* ')'					# CONCATENATE_fun
	| EXACT '(' expr ',' expr ')'							# EXACT_fun
	| FIND '(' expr ',' expr (',' expr)? ')'				# FIND_fun
	| FIXED '(' expr (',' expr (',' expr)?)? ')'			# FIXED_fun
	| f=(LEFT | RIGHT) '(' expr (',' expr)? ')'				# LR_fun
	| LEN '(' expr ')'										# LEN_fun
	| f=(LOWER | UPPER) '(' expr ')'							# CASE_fun
	| MID '(' expr ',' expr ',' expr ')'					# MID_fun
	| PROPER '(' expr ')'									# PROPER_fun
	| REPLACE '(' expr ',' expr ',' expr (',' expr)? ')'	# REPLACE_fun
	| REPT '(' expr ',' expr ')'							# REPT_fun
	| RMB '(' expr ')'										# RMB_fun
	| SEARCH '(' expr ',' expr (',' expr)? ')'				# SEARCH_fun
	| SUBSTITUTE '(' expr ',' expr ',' expr (',' expr)? ')'	# SUBSTITUTE_fun
	| T '(' expr ')'										# T_fun
	| TEXT '(' expr ',' expr ')'							# TEXT_fun
	| TRIM '(' expr ')'										# TRIM_fun
	| VALUE '(' expr ')'									# VALUE_fun
	| DATEVALUE '(' expr (',' expr)? ')'					# DATEVALUE_fun
	| TIMEVALUE '(' expr ')'								# TIMEVALUE_fun
	| DATE '(' expr ',' expr ',' expr (
		',' expr (',' expr (',' expr)?)?
	)? ')'														# DATE_fun
	| TIME '(' expr ',' expr (',' expr)? ')'					# TIME_fun
	| NOW '(' ')'												# NOW_fun
	| TODAY '(' ')'												# TODAY_fun
	| f=(YEAR | MONTH | DAY | HOUR | MINUTE | SECOND) '(' expr ')'	# DATE_TIME_fun
	| WEEKDAY '(' expr (',' expr)? ')'							# WEEKDAY_fun
	| DATEDIF '(' expr ',' expr ',' expr ')'					# DATEDIF_fun
	| DAYS '(' expr ',' expr ')'								# DAYS_fun
	| DAYS360 '(' expr ',' expr (',' expr)? ')'					# DAYS360_fun
	| EDATE '(' expr ',' expr ')'								# EDATE_fun
	| EOMONTH '(' expr ',' expr ')'								# EOMONTH_fun
	| NETWORKDAYS '(' expr ',' expr (',' expr)? ')'				# NETWORKDAYS_fun
	| WORKDAY '(' expr ',' expr (',' expr)? ')'					# WORKDAY_fun
	| WEEKNUM '(' expr (',' expr)? ')'							# WEEKNUM_fun
	| f=(MAX | MEDIAN | MIN | MODE | AVERAGE | GEOMEAN | HARMEAN | COUNT | SUM | AVEDEV | STDEV | STDEVP | DEVSQ | VAR | VARP) '(' expr (',' expr)* ')'	# STAT_fun
	| f=(QUARTILE | LARGE | SMALL | PERCENTILE | PERCENTRANK) '(' expr ',' expr ')'	# RANK2_fun
	| COVAR '(' expr ',' expr ')'									# COVAR_fun
	| COVARIANCES '(' expr ',' expr ')'							# COVARIANCES_fun
	| AVERAGEIF '(' expr ',' expr (',' expr)? ')'				# AVERAGEIF_fun
	| COUNTIF '(' expr ',' expr ')'								# COUNTIF_fun
	| SUMIF '(' expr ',' expr (',' expr)? ')'					# SUMIF_fun
	| NORMDIST '(' expr ',' expr ',' expr ',' expr ')'			# NORMDIST_fun
	| NORMINV '(' expr ',' expr ',' expr ')'					# NORMINV_fun
	| NORMSDIST '(' expr ')'									# NORMSDIST_fun
	| NORMSINV '(' expr ')'										# NORMSINV_fun
	| BETADIST '(' expr ',' expr ',' expr ')'					# BETADIST_fun
	| BETAINV '(' expr ',' expr ',' expr ')'					# BETAINV_fun
	| BINOMDIST '(' expr ',' expr ',' expr ',' expr ')'			# BINOMDIST_fun
	| EXPONDIST '(' expr ',' expr ',' expr ')'					# EXPONDIST_fun
	| FDIST '(' expr ',' expr ',' expr ')'						# FDIST_fun
	| FINV '(' expr ',' expr ',' expr ')'						# FINV_fun
	| FISHER '(' expr ')'										# FISHER_fun
	| FISHERINV '(' expr ')'									# FISHERINV_fun
	| GAMMADIST '(' expr ',' expr ',' expr ',' expr ')'			# GAMMADIST_fun
	| GAMMAINV '(' expr ',' expr ',' expr ')'					# GAMMAINV_fun
	| GAMMALN '(' expr ')'										# GAMMALN_fun
	| HYPGEOMDIST '(' expr ',' expr ',' expr ',' expr ')'		# HYPGEOMDIST_fun
	| LOGINV '(' expr ',' expr ',' expr ')'						# LOGINV_fun
	| LOGNORMDIST '(' expr ',' expr ',' expr ')'				# LOGNORMDIST_fun
	| NEGBINOMDIST '(' expr ',' expr ',' expr ')'				# NEGBINOMDIST_fun
	| POISSON '(' expr ',' expr ',' expr ')'					# POISSON_fun
	| TDIST '(' expr ',' expr ',' expr ')'						# TDIST_fun
	| TINV '(' expr ',' expr ')'								# TINV_fun
	| WEIBULL '(' expr ',' expr ',' expr ',' expr ')'			# WEIBULL_fun
	| PMT '(' expr ',' expr ',' expr (',' expr (',' expr)?)? ')'	# PMT_fun
	| PPMT '(' expr ',' expr ',' expr ',' expr (',' expr (',' expr)?)? ')'	# PPMT_fun
	| IPMT '(' expr ',' expr ',' expr ',' expr (',' expr (',' expr)?)? ')'	# IPMT_fun
	| PV '(' expr ',' expr ',' expr (',' expr (',' expr)?)? ')'	# PV_fun
	| FV '(' expr ',' expr ',' expr (',' expr (',' expr)?)? ')'	# FV_fun
	| NPER '(' expr ',' expr ',' expr (',' expr (',' expr)?)? ')'	# NPER_fun
	| RATE '(' expr ',' expr ',' expr (',' expr (',' expr (',' expr)?)?)? ')'	# RATE_fun
	| NPV '(' expr ',' expr (',' expr)* ')'						# NPV_fun
	| XNPV '(' expr ',' expr ',' expr ')'						# XNPV_fun
	| IRR '(' expr (',' expr)? ')'								# IRR_fun
	| MIRR '(' expr ',' expr ',' expr ')'						# MIRR_fun
	| XIRR '(' expr ',' expr (',' expr)? ')'					# XIRR_fun
	| SLN '(' expr ',' expr ',' expr ')'						# SLN_fun
	| DB '(' expr ',' expr ',' expr ',' expr (',' expr)? ')'	# DB_fun
	| DDB '(' expr ',' expr ',' expr ',' expr (',' expr)? ')'	# DDB_fun
	| SYD '(' expr ',' expr ',' expr ',' expr ')'				# SYD_fun
	| f=(URLENCODE | URLDECODE | HTMLENCODE | HTMLDECODE | BASE64TOTEXT | BASE64URLTOTEXT | TEXTTOBASE64 | TEXTTOBASE64URL) '(' expr ')'	# ENCODE_fun
	| REGEX '(' expr ',' expr ')'								# REGEX_fun
	| REGEXREPLACE '(' expr ',' expr ',' expr ')'				# REGEXREPLACE_fun
	| ISREGEX '(' expr ',' expr ')'								# ISREGEX_fun
	| GUID '(' ')'												# GUID_fun
	| f=(MD5 | SHA1 | SHA256 | SHA512) '(' expr ')'				# HASH_fun
	| f=(HMACMD5 | HMACSHA1 | HMACSHA256 | HMACSHA512) '(' expr ',' expr ')'	# HMAC_fun
	| f=(TRIMSTART | TRIMEND) '(' expr (',' expr)? ')'			# TRIM_SE_fun
	| f=(INDEXOF | LASTINDEXOF) '(' expr ',' expr (',' expr (',' expr)?)? ')'	# INDEXOF_fun
	| SPLIT '(' expr ',' expr ')'								# SPLIT_fun
	| JOIN '(' expr (',' expr)+ ')'								# JOIN_fun
	| SUBSTRING '(' expr ',' expr (',' expr)? ')'				# SUBSTRING_fun
	| f=(STARTSWITH | ENDSWITH) '(' expr ',' expr (',' expr)? ')'	# STRINGSuffix_fun
	| f=(ISNULLOREMPTY | ISNULLORWHITESPACE) '(' expr ')'		# ISNULLOR_fun
	| f=(REMOVESTART | REMOVEEND) '(' expr (',' expr (',' expr)?)? ')'	# REMOVE_fun
	| JSON '(' expr ')'											# JSON_fun
	| f=(LOOKCEILING | LOOKFLOOR) '(' expr ',' expr ')'			# LOOK_fun
	| PARAMETER '(' (expr (',' expr)*)? ')'						# DiyFunction_fun
	| f=(ADDYEARS | ADDMONTHS | ADDDAYS | ADDHOURS | ADDMINUTES | ADDSECONDS) '(' expr ',' expr ')'	# ADD_DateTime_fun
	| TIMESTAMP '(' expr (',' expr)? ')'						# TIMESTAMP_fun
	| PARAM '(' expr (',' expr)? ')'							# PARAM_fun
	| ERROR '(' expr? ')'										# ERROR_fun
	| HAS '('expr ',' expr ')'									# HAS_fun
	| HASVALUE '(' expr ','expr ')'								# HASVALUE_fun
	| '{' arrayJson (',' arrayJson)* ','* '}'					# ArrayJson_fun
	| '[' expr (',' expr)* ','* ']'								# Array_fun
	| ALGORITHMVERSION											# Version_fun
	| PARAMETER													# PARAMETER_fun
	| num  unit=(UNIT | T)?										# NUM_fun
	| STRING													# STRING_fun
	| NULL														# NULL_fun;

num: '-'? NUM;
 

arrayJson: key=(NUM | STRING) ':' expr
	| parameter2 ':' expr;

parameter2:
	E
	| IF
	| IFS
	| SWITCH
	| IFERROR
	| ISNUMBER
	| ISTEXT
	| ISERROR
	| ISNONTEXT
	| ISLOGICAL
	| ISEVEN
	| ISODD
	| ISNULL
	| ISNULLORERROR
	| AND
	| OR
	| XOR
	| NOT
	| TRUE
	| FALSE
	| PI
	| DEC2BIN
	| DEC2HEX
	| DEC2OCT
	| HEX2BIN
	| HEX2DEC
	| HEX2OCT
	| OCT2BIN
	| OCT2DEC
	| OCT2HEX
	| BIN2OCT
	| BIN2DEC
	| BIN2HEX
	| ABS
	| QUOTIENT
	| MOD
	| SIGN
	| SQRT
	| TRUNC
	| INT
	| GCD
	| LCM
	| COMBIN
	| PERMUT
	| DEGREES
	| RADIANS
	| COS
	| COSH
	| SIN
	| SINH
	| TAN
	| TANH
	| COT
	| COTH
	| CSC
	| CSCH
	| SEC
	| SECH
	| ACOS
	| ACOSH
	| ASIN
	| ASINH
	| ATAN
	| ATANH
	| ACOT
	| ACOTH
	| ATAN2
	| ROUND
	| ROUNDDOWN
	| ROUNDUP
	| CEILING
	| FLOOR
	| EVEN
	| ODD
	| MROUND
	| RAND
	| RANDBETWEEN
	| FACT
	| FACTDOUBLE
	| POWER
	| EXP
	| LN
	| LOG
	| LOG10
	| MULTINOMIAL
	| PRODUCT
	| SQRTPI
	| ERF
	| ERFC
	| BESSELI
	| BESSELJ
	| BESSELK
	| BESSELY
	| DELTA
	| GESTEP
	| SUMSQ
	| SUMPRODUCT
	| SUMX2MY2
	| SUMX2PY2
	| SUMXMY2
	| ARABIC
	| ROMAN
	| SERIESSUM
	| RANK
	| FORECAST
	| INTERCEPT
	| SLOPE
	| CORREL
	| PEARSON
	| YEARFRAC
	| ASC
	| JIS
	| CHAR
	| CLEAN
	| CODE
	| UNICHAR
	| UNICODE
	| CONCATENATE
	| EXACT
	| FIND
	| FIXED
	| LEFT
	| LEN
	| LOWER
	| MID
	| PROPER
	| REPLACE
	| REPT
	| RIGHT
	| RMB
	| SEARCH
	| SUBSTITUTE
	| T
	| TEXT
	| TRIM
	| UPPER
	| VALUE
	| DATEVALUE
	| TIMEVALUE
	| DATE
	| TIME
	| NOW
	| TODAY
	| YEAR
	| MONTH
	| DAY
	| HOUR
	| MINUTE
	| SECOND
	| WEEKDAY
	| DATEDIF
	| DAYS
	| DAYS360
	| EDATE
	| EOMONTH
	| NETWORKDAYS
	| WORKDAY
	| WEEKNUM
	| MAX
	| MEDIAN
	| MIN
	| QUARTILE
	| MODE
	| LARGE
	| SMALL
	| PERCENTILE
	| PERCENTRANK
	| AVERAGE
	| AVERAGEIF
	| GEOMEAN
	| HARMEAN
	| COUNT
	| COUNTIF
	| SUM
	| SUMIF
	| AVEDEV
	| STDEV
	| STDEVP
	| COVAR
	| COVARIANCES
	| DEVSQ
	| VAR
	| VARP
	| NORMDIST
	| NORMINV
	| NORMSDIST
	| NORMSINV
	| BETADIST
	| BETAINV
	| BINOMDIST
	| EXPONDIST
	| FDIST
	| FINV
	| FISHER
	| FISHERINV
	| GAMMADIST
	| GAMMAINV
	| GAMMALN
	| HYPGEOMDIST
	| LOGINV
	| LOGNORMDIST
	| NEGBINOMDIST
	| POISSON
	| TDIST
	| TINV
	| WEIBULL
	| URLENCODE
	| URLDECODE
	| HTMLENCODE
	| HTMLDECODE
	| BASE64TOTEXT
	| BASE64URLTOTEXT
	| TEXTTOBASE64
	| TEXTTOBASE64URL
	| REGEX
	| REGEXREPLACE
	| ISREGEX
	| GUID
	| MD5
	| SHA1
	| SHA256
	| SHA512
	| HMACMD5
	| HMACSHA1
	| HMACSHA256
	| HMACSHA512
	| TRIMSTART
	| TRIMEND
	| INDEXOF
	| LASTINDEXOF
	| SPLIT
	| JOIN
	| SUBSTRING
	| STARTSWITH
	| ENDSWITH
	| ISNULLOREMPTY
	| ISNULLORWHITESPACE
	| REMOVESTART
	| REMOVEEND
	| JSON
	| LOOKCEILING
	| LOOKFLOOR
	| ADDYEARS
	| ADDMONTHS
	| ADDDAYS
	| ADDHOURS
	| ADDMINUTES
	| ADDSECONDS
	| TIMESTAMP
	| PMT
	| PPMT
	| IPMT
	| PV
	| FV
	| NPER
	| RATE
	| NPV
	| XNPV
	| IRR
	| MIRR
	| XIRR
	| SLN
	| DB
	| DDB
	| SYD
	| NULL
	| ERROR
	| UNIT
	| HAS
	| HASVALUE
	| ALGORITHMVERSION
	| PARAM
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
ERROR: 'ERROR';

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

// 逻辑函数
IF: 'IF';
IFS: 'IFS';
SWITCH: 'SWITCH';
IFERROR: 'IFERROR';
ISNUMBER: 'ISNUMBER';
ISTEXT: 'ISTEXT';
ISERROR: 'ISERROR';
ISNONTEXT: 'ISNONTEXT';
ISLOGICAL: 'ISLOGICAL';
ISEVEN: 'ISEVEN';
ISODD: 'ISODD';
ISNULL: 'ISNULL';
ISNULLORERROR: 'ISNULLORERROR';
AND: 'AND';
OR: 'OR';
XOR: 'XOR';
NOT: 'NOT';
TRUE: 'TRUE' | 'YES';
FALSE: 'FALSE' | 'NO';
// 数学与三角函数
E: 'E';
PI: 'PI';
DEC2BIN: 'DEC2BIN';
DEC2HEX: 'DEC2HEX';
DEC2OCT: 'DEC2OCT';
HEX2BIN: 'HEX2BIN'; //  将十六进制数转换为二进制数
HEX2DEC: 'HEX2DEC'; // 将十六进制数转换为十进制数
HEX2OCT: 'HEX2OCT'; //  将十六进制数转换为八进制数
OCT2BIN: 'OCT2BIN'; //   将八进制数转换为二进制数
OCT2DEC: 'OCT2DEC'; //   将八进制数转换为十进制数
OCT2HEX: 'OCT2HEX'; //  将八进制数转换为十六进制数
BIN2OCT: 'BIN2OCT';
BIN2DEC: 'BIN2DEC';
BIN2HEX: 'BIN2HEX';
ABS: 'ABS';
QUOTIENT: 'QUOTIENT';
MOD: 'MOD';
SIGN: 'SIGN';
SQRT: 'SQRT';
TRUNC: 'TRUNC';
INT: 'INT';
GCD: 'GCD';
LCM: 'LCM';
COMBIN: 'COMBIN';
PERMUT: 'PERMUT';
DEGREES: 'DEGREES';
RADIANS: 'RADIANS';
COS: 'COS';
COSH: 'COSH';
SIN: 'SIN';
SINH: 'SINH';
TAN: 'TAN';
TANH: 'TANH';
COT: 'COT';
COTH: 'COTH';
CSC: 'CSC';
CSCH: 'CSCH';
SEC: 'SEC';
SECH: 'SECH';
ACOS: 'ACOS';
ACOSH: 'ACOSH';
ASIN: 'ASIN';
ASINH: 'ASINH';
ATAN: 'ATAN';
ATANH: 'ATANH';
ACOT: 'ACOT';
ACOTH: 'ACOTH';
ATAN2: 'ATAN2';
ROUND: 'ROUND';
ROUNDDOWN: 'ROUNDDOWN';
ROUNDUP: 'ROUNDUP';
CEILING: 'CEILING';
FLOOR: 'FLOOR';
EVEN: 'EVEN';
ODD: 'ODD';
MROUND: 'MROUND';
RAND: 'RAND';
RANDBETWEEN: 'RANDBETWEEN';
FACT: 'FACT';
FACTDOUBLE: 'FACTDOUBLE';
POWER: 'POWER';
EXP: 'EXP';
LN: 'LN';
LOG: 'LOG';
LOG10: 'LOG10';
MULTINOMIAL: 'MULTINOMIAL';
PRODUCT: 'PRODUCT';
SQRTPI: 'SQRTPI';
ERF: 'ERF';
ERFC: 'ERFC';
BESSELI: 'BESSELI';
BESSELJ: 'BESSELJ';
BESSELK: 'BESSELK';
BESSELY: 'BESSELY';
DELTA: 'DELTA';
GESTEP: 'GESTEP';
SUMSQ: 'SUMSQ';
SUMPRODUCT: 'SUMPRODUCT';
SUMX2MY2: 'SUMX2MY2';
SUMX2PY2: 'SUMX2PY2';
SUMXMY2: 'SUMXMY2';
ARABIC: 'ARABIC';
ROMAN: 'ROMAN';
SERIESSUM: 'SERIESSUM';
RANK: 'RANK';
FORECAST: 'FORECAST';
INTERCEPT: 'INTERCEPT';
SLOPE: 'SLOPE';
CORREL: 'CORREL';
PEARSON: 'PEARSON';
YEARFRAC: 'YEARFRAC';
// 文本函数
ASC: 'ASC';
JIS: 'JIS' | 'WIDECHAR';
CHAR: 'CHAR';
CLEAN: 'CLEAN';
CODE: 'CODE';
UNICHAR: 'UNICHAR';
UNICODE: 'UNICODE';
CONCATENATE: 'CONCATENATE'|'CONCAT';
EXACT: 'EXACT';
FIND: 'FIND';
FIXED: 'FIXED';
LEFT: 'LEFT';
LEN: 'LEN';
LOWER: 'LOWER' | 'TOLOWER';
MID: 'MID';
PROPER: 'PROPER';
REPLACE: 'REPLACE';
REPT: 'REPT';
RIGHT: 'RIGHT';
RMB: 'RMB';
SEARCH: 'SEARCH';
SUBSTITUTE: 'SUBSTITUTE';
T: 'T';
TEXT: 'TEXT';
TRIM: 'TRIM';
UPPER: 'UPPER' | 'TOUPPER';
VALUE: 'VALUE';
// 日期与时间函数
DATEVALUE: 'DATEVALUE';
TIMEVALUE: 'TIMEVALUE';
DATE: 'DATE';
TIME: 'TIME';
NOW: 'NOW';
TODAY: 'TODAY';
YEAR: 'YEAR';
MONTH: 'MONTH';
DAY: 'DAY';
HOUR: 'HOUR';
MINUTE: 'MINUTE';
SECOND: 'SECOND';
WEEKDAY: 'WEEKDAY';
DATEDIF: 'DATEDIF';
DAYS: 'DAYS';
DAYS360: 'DAYS360';
EDATE: 'EDATE';
EOMONTH: 'EOMONTH';
NETWORKDAYS: 'NETWORKDAYS';
WORKDAY: 'WORKDAY';
WEEKNUM: 'WEEKNUM';
// 统计函数
MAX: 'MAX';
MEDIAN: 'MEDIAN';
MIN: 'MIN';
QUARTILE: 'QUARTILE';
MODE: 'MODE';
LARGE: 'LARGE';
SMALL: 'SMALL';
PERCENTILE: 'PERCENTILE'|'PERCENTILE.INC';
PERCENTRANK: 'PERCENTRANK'|'PERCENTRANK.INC';
AVERAGE: 'AVERAGE';
AVERAGEIF: 'AVERAGEIF';
GEOMEAN: 'GEOMEAN';
HARMEAN: 'HARMEAN';
COUNT: 'COUNT';
COUNTIF: 'COUNTIF';
SUM: 'SUM';
SUMIF: 'SUMIF';
AVEDEV: 'AVEDEV';
STDEV: 'STDEV' | 'STDEV.S';
STDEVP: 'STDEVP' | 'STDEV.P';
COVAR:'COVAR' | 'COVARIANCE.P';
COVARIANCES:'COVARIANCE.S';
DEVSQ: 'DEVSQ';
VAR: 'VAR' | 'VAR.S';
VARP: 'VARP'| 'VAR.P';
NORMDIST: 'NORMDIST'| 'NORM.DIST';
NORMINV: 'NORMINV' | 'NORM.INV';
NORMSDIST: 'NORMSDIST'|'NORM.S.DIST';
NORMSINV: 'NORMSINV' |'NORM.S.INV';
BETADIST: 'BETADIST'|'BETA.DIST';
BETAINV: 'BETAINV'|'BETA.INV';
BINOMDIST: 'BINOMDIST'|'BINOM.DIST';
EXPONDIST: 'EXPONDIST'|'EXPON.DIST';
FDIST: 'FDIST'|'F.DIST';
FINV: 'FINV'|'F.INV';
FISHER: 'FISHER';
FISHERINV: 'FISHERINV';
GAMMADIST: 'GAMMADIST'|'GAMMA.DIST';
GAMMAINV: 'GAMMAINV'|'GAMMA.INV';
GAMMALN: 'GAMMALN'|'GAMMALN.PRECISE';
HYPGEOMDIST: 'HYPGEOMDIST'|'HYPGEOM.DIST';
LOGINV: 'LOGINV'|'LOGNORM.INV';
LOGNORMDIST: 'LOGNORMDIST'|'LOGNORM.DIST';
NEGBINOMDIST: 'NEGBINOMDIST'|'NEGBINOM.DIST';
POISSON: 'POISSON'|'POISSON.DIST';
TDIST: 'TDIST'|'T.DIST';
TINV: 'TINV'|'T.INV';
WEIBULL: 'WEIBULL';
// 财务函数
PMT: 'PMT';
PPMT: 'PPMT';
IPMT: 'IPMT';
PV: 'PV';
FV: 'FV';
NPER: 'NPER';
RATE: 'RATE';
NPV: 'NPV';
XNPV: 'XNPV';
IRR: 'IRR';
MIRR: 'MIRR';
XIRR: 'XIRR';
SLN: 'SLN';
DB: 'DB';
DDB: 'DDB';
SYD: 'SYD';
// 增加函数 类C# 方法
URLENCODE: 'URLENCODE';
URLDECODE: 'URLDECODE';
HTMLENCODE: 'HTMLENCODE';
HTMLDECODE: 'HTMLDECODE';
BASE64TOTEXT: 'BASE64TOTEXT';
BASE64URLTOTEXT: 'BASE64URLTOTEXT';
TEXTTOBASE64: 'TEXTTOBASE64';
TEXTTOBASE64URL: 'TEXTTOBASE64URL';
REGEX: 'REGEX';
REGEXREPLACE: 'REGEXREPLACE';
ISREGEX: 'ISREGEX' | 'ISMATCH';
GUID: 'GUID';
MD5: 'MD5';
SHA1: 'SHA1';
SHA256: 'SHA256';
SHA512: 'SHA512';
HMACMD5: 'HMACMD5';
HMACSHA1: 'HMACSHA1';
HMACSHA256: 'HMACSHA256';
HMACSHA512: 'HMACSHA512';
TRIMSTART: 'TRIMSTART' | 'LTRIM';
TRIMEND: 'TRIMEND' | 'RTRIM';
INDEXOF: 'INDEXOF';
LASTINDEXOF: 'LASTINDEXOF';
SPLIT: 'SPLIT';
JOIN: 'JOIN';
SUBSTRING: 'SUBSTRING';
STARTSWITH: 'STARTSWITH';
ENDSWITH: 'ENDSWITH';
ISNULLOREMPTY: 'ISNULLOREMPTY';
ISNULLORWHITESPACE: 'ISNULLORWHITESPACE';
REMOVESTART: 'REMOVESTART';
REMOVEEND: 'REMOVEEND';
JSON: 'JSON';
LOOKCEILING: 'LOOKCEILING';
LOOKFLOOR: 'LOOKFLOOR';
ARRAY: 'ARRAY';
ALGORITHMVERSION:'ALGORITHMVERSION'|'ENGINEVERSION';

ADDYEARS: 'ADDYEARS';
ADDMONTHS: 'ADDMONTHS';
ADDDAYS: 'ADDDAYS';
ADDHOURS: 'ADDHOURS';
ADDMINUTES: 'ADDMINUTES';
ADDSECONDS: 'ADDSECONDS';
TIMESTAMP: 'TIMESTAMP';
HAS: 'HAS' | 'HASKEY' |'CONTAINS'|'CONTAINSKEY';
HASVALUE: 'HASVALUE' | 'CONTAINSVALUE';
PARAM: 'PARAM' | 'PARAMETER' | 'GETPARAMETER';

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

WS: [ \t\r\n\u000C]+ -> skip;
COMMENT: '/*' .*? '*/' -> skip;
LINE_COMMENT: '//' ~[\r\n]* -> skip;