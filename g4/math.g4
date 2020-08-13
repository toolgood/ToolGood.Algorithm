grammar math;

prog: expr;

expr:
	expr op = ('*' | '/' | '%') expr	# MulDiv_fun
	| expr op = ('+' | '-' | '&') expr	# AddSub_fun
	| expr op = (
		'>'
		| '>='
		| '<'
		| '<='
		| '='
		| '=='
		| '!='
		| '<>'
	) expr														# Judge_fun
	| expr op = ('&&' | '||' | AND | OR) expr					# AndOr_fun
	| expr '.' ISNUMBER '(' ')'									# ISNUMBER_fun
	| expr '.' ISTEXT '(' ')'									# ISTEXT_fun
	| expr '.' ISNONTEXT '(' ')'								# ISNONTEXT_fun
	| expr '.' ISLOGICAL '(' ')'								# ISLOGICAL_fun
	| expr '.' ISEVEN '(' ')'									# ISEVEN_fun
	| expr '.' ISODD '(' ')'									# ISODD_fun
	| expr '.' ISERROR '(' expr? ')'							# ISERROR_fun
	| expr '.' ISNULL '(' expr? ')'								# ISNULL_fun
	| expr '.' ISNULLORERROR '(' expr? ')'						# ISNULLORERROR_fun
	| expr '.' DEC2BIN ('(' expr? ')')							# DEC2BIN_fun
	| expr '.' DEC2HEX ('(' expr? ')')							# DEC2HEX_fun
	| expr '.' DEC2OCT ('(' expr? ')')							# DEC2OCT_fun
	| expr '.' HEX2BIN ('(' expr? ')')							# HEX2BIN_fun
	| expr '.' HEX2DEC ('(' ')')								# HEX2DEC_fun
	| expr '.' HEX2OCT ('(' expr? ')')							# HEX2OCT_fun
	| expr '.' OCT2BIN ('(' expr? ')')							# OCT2BIN_fun
	| expr '.' OCT2DEC ('(' ')')								# OCT2DEC_fun
	| expr '.' OCT2HEX ('(' expr? ')')							# OCT2HEX_fun
	| expr '.' BIN2OCT ('(' expr? ')')							# BIN2OCT_fun
	| expr '.' BIN2DEC ('(' ')')								# BIN2DEC_fun
	| expr '.' BIN2HEX ('(' expr? ')')							# BIN2HEX_fun
	| expr '.' INT '(' ')'										# INT_fun
	| expr '.' ASC '(' ')'										# ASC_fun
	| expr '.' JIS '(' ')'										# JIS_fun
	| expr '.' CHAR '(' ')'										# CHAR_fun
	| expr '.' CLEAN '(' ')'									# CLEAN_fun
	| expr '.' CODE '(' ')'										# CODE_fun
	| expr '.' CONCATENATE '(' (expr (',' expr)*)? ')'			# CONCATENATE_fun
	| expr '.' EXACT '(' expr ')'								# EXACT_fun
	| expr '.' FIND '(' expr (',' expr)? ')'					# FIND_fun
	| expr '.' LEFT '(' expr? ')'								# LEFT_fun
	| expr '.' LEN '(' ')'										# LEN_fun
	| expr '.' LOWER '(' ')'									# LOWER_fun
	| expr '.' MID '(' expr ',' expr ')'						# MID_fun
	| expr '.' PROPER '(' ')'									# PROPER_fun
	| expr '.' REPLACE '(' expr ',' expr (',' expr)? ')'		# REPLACE_fun
	| expr '.' REPT '(' expr ')'								# REPT_fun
	| expr '.' RIGHT '(' expr? ')'								# RIGHT_fun
	| expr '.' RMB '(' ')'										# RMB_fun
	| expr '.' SEARCH '(' expr (',' expr)? ')'					# SEARCH_fun
	| expr '.' SUBSTITUTE '(' expr ',' expr (',' expr)? ')'		# SUBSTITUTE_fun
	| expr '.' T '(' ')'										# T_fun
	| expr '.' TEXT '(' expr ')'								# TEXT_fun
	| expr '.' TRIM '(' ')'										# TRIM_fun
	| expr '.' UPPER '(' ')'									# UPPER_fun
	| expr '.' VALUE '(' ')'									# VALUE_fun
	| expr '.' DATEVALUE '(' ')'								# DATEVALUE_fun
	| expr '.' TIMEVALUE '(' ')'								# TIMEVALUE_fun
	| expr '.' YEAR ('(' ')')?									# YEAR_fun
	| expr '.' MONTH ('(' ')')?									# MONTH_fun
	| expr '.' DAY ('(' ')')?									# DAY_fun
	| expr '.' HOUR ('(' ')')?									# HOUR_fun
	| expr '.' MINUTE ('(' ')')?								# MINUTE_fun
	| expr '.' SECOND ('(' ')')?								# SECOND_fun
	| expr '.' URLENCODE '(' ')'								# URLENCODE_fun
	| expr '.' URLDECODE '(' ')'								# URLDECODE_fun
	| expr '.' HTMLENCODE '(' ')'								# HTMLENCODE_fun
	| expr '.' HTMLDECODE '(' ')'								# HTMLDECODE_fun
	| expr '.' BASE64TOTEXT '(' expr? ')'						# BASE64TOTEXT_fun
	| expr '.' BASE64URLTOTEXT '(' expr? ')'					# BASE64URLTOTEXT_fun
	| expr '.' TEXTTOBASE64 '(' expr? ')'						# TEXTTOBASE64_fun
	| expr '.' TEXTTOBASE64URL '(' expr? ')'					# TEXTTOBASE64URL_fun
	| expr '.' REGEX '(' expr ')'								# REGEX_fun
	| expr '.' REGEXREPALCE '(' expr ',' expr ')'				# REGEXREPALCE_fun
	| expr '.' ISREGEX '(' expr ')'								# ISREGEX_fun
	| expr '.' MD5 '(' expr? ')'								# MD5_fun
	| expr '.' SHA1 '(' expr? ')'								# SHA1_fun
	| expr '.' SHA256 '(' expr? ')'								# SHA256_fun
	| expr '.' SHA512 '(' expr? ')'								# SHA512_fun
	| expr '.' CRC32 '(' expr? ')'								# CRC32_fun
	| expr '.' HMACMD5 '(' expr (',' expr)? ')'					# HMACMD5_fun
	| expr '.' HMACSHA1 '(' expr (',' expr)? ')'				# HMACSHA1_fun
	| expr '.' HMACSHA256 '(' expr (',' expr)? ')'				# HMACSHA256_fun
	| expr '.' HMACSHA512 '(' expr (',' expr)? ')'				# HMACSHA512_fun
	| expr '.' TRIMSTART '(' expr? ')'							# TRIMSTART_fun
	| expr '.' TRIMEND '(' expr? ')'							# TRIMEND_fun
	| expr '.' INDEXOF '(' expr (',' expr (',' expr)?)? ')'		# INDEXOF_fun
	| expr '.' LASTINDEXOF '(' expr (',' expr (',' expr)?)? ')'	# LASTINDEXOF_fun
	| expr '.' SPLIT '(' expr ')'								# SPLIT_fun
	| expr '.' JOIN '(' expr (',' expr)* ')'					# JOIN_fun
	| expr '.' SUBSTRING '(' expr (',' expr)? ')'				# SUBSTRING_fun
	| expr '.' STARTSWITH '(' expr (',' expr)? ')'				# STARTSWITH_fun
	| expr '.' ENDSWITH '(' expr (',' expr)? ')'				# ENDSWITH_fun
	| expr '.' ISNULLOREMPTY '(' ')'							# ISNULLOREMPTY_fun
	| expr '.' ISNULLORWHITESPACE '(' ')'						# ISNULLORWHITESPACE_fun
	| expr '.' REMOVESTART '(' expr (',' expr)? ')'				# REMOVESTART_fun
	| expr '.' REMOVEEND '(' expr (',' expr)? ')'				# REMOVEEND_fun
	| expr '.' JSON '(' ')'										# JSON_fun
	| expr '.' VLOOKUP '(' expr ',' expr (',' expr)? ')'		# VLOOKUP_fun
	| expr '.' LOOKUP '(' expr ',' expr ')'						# LOOKUP_fun
	| expr '.' PARAMETER '(' (expr (',' expr)*)? ')'			# DiyFunction_fun
	| expr '[' parameter ']'									# GetJsonValue_fun
	| expr '.' parameter2										# GetJsonValue_fun
	| expr2														# expr2_fun;

expr2:
	'{' expr (',' expr)* '}'								# Array_fun2
	| '(' expr ')'											# Bracket_fun2
	| IF '(' expr ',' expr (',' expr)? ')'					# IF_fun2
	| ISNUMBER '(' expr ')'									# ISNUMBER_fun2
	| ISTEXT '(' expr ')'									# ISTEXT_fun2
	| ISERROR '(' expr (',' expr)? ')'						# ISERROR_fun2
	| ISNONTEXT '(' expr ')'								# ISNONTEXT_fun2
	| ISLOGICAL '(' expr ')'								# ISLOGICAL_fun2
	| ISEVEN '(' expr ')'									# ISEVEN_fun2
	| ISODD '(' expr ')'									# ISODD_fun2
	| IFERROR '(' expr ',' expr (',' expr)? ')'				# IFERROR_fun2
	| ISNULL '(' expr (',' expr)? ')'						# ISNULL_fun2
	| ISNULLORERROR '(' expr (',' expr)? ')'				# ISNULLORERROR_fun2
	| AND '(' expr (',' expr)* ')'							# AND_fun2
	| OR '(' expr (',' expr)* ')'							# OR_fun2
	| NOT '(' expr ')'										# NOT_fun2
	| TRUE ('(' ')')?										# TRUE_fun2
	| FALSE ('(' ')')?										# FALSE_fun2
	| E ('(' ')')?											# E_fun2
	| PI ('(' ')')?											# PI_fun2
	| DEC2BIN ('(' expr (',' expr)? ')')					# DEC2BIN_fun2
	| DEC2HEX ('(' expr (',' expr)? ')')					# DEC2HEX_fun2
	| DEC2OCT ('(' expr (',' expr)? ')')					# DEC2OCT_fun2
	| HEX2BIN ('(' expr (',' expr)? ')')					# HEX2BIN_fun2
	| HEX2DEC ('(' expr ')')								# HEX2DEC_fun2
	| HEX2OCT ('(' expr (',' expr)? ')')					# HEX2OCT_fun2
	| OCT2BIN ('(' expr (',' expr)? ')')					# OCT2BIN_fun2
	| OCT2DEC ('(' expr ')')								# OCT2DEC_fun2
	| OCT2HEX ('(' expr (',' expr)? ')')					# OCT2HEX_fun2
	| BIN2OCT ('(' expr (',' expr)? ')')					# BIN2OCT_fun2
	| BIN2DEC ('(' expr ')')								# BIN2DEC_fun2
	| BIN2HEX ('(' expr (',' expr)? ')')					# BIN2HEX_fun2
	| ABS '(' expr ')'										# ABS_fun2
	| QUOTIENT '(' expr (',' expr) ')'						# QUOTIENT_fun2
	| MOD '(' expr (',' expr) ')'							# MOD_fun2
	| SIGN '(' expr ')'										# SIGN_fun2
	| SQRT '(' expr ')'										# SQRT_fun2
	| TRUNC '(' expr ')'									# TRUNC_fun2
	| INT '(' expr ')'										# INT_fun2
	| GCD '(' expr (',' expr)+ ')'							# GCD_fun2
	| LCM '(' expr (',' expr)+ ')'							# LCM_fun2
	| COMBIN '(' expr ',' expr ')'							# COMBIN_fun2
	| PERMUT '(' expr ',' expr ')'							# PERMUT_fun2
	| DEGREES '(' expr ')'									# DEGREES_fun2
	| RADIANS '(' expr ')'									# RADIANS_fun2
	| COS '(' expr ')'										# COS_fun2
	| COSH '(' expr ')'										# COSH_fun2
	| SIN '(' expr ')'										# SIN_fun2
	| SINH '(' expr ')'										# SINH_fun2
	| TAN '(' expr ')'										# TAN_fun2
	| TANH '(' expr ')'										# TANH_fun2
	| ACOS '(' expr ')'										# ACOS_fun2
	| ACOSH '(' expr ')'									# ACOSH_fun2
	| ASIN '(' expr ')'										# ASIN_fun2
	| ASINH '(' expr ')'									# ASINH_fun2
	| ATAN '(' expr ')'										# ATAN_fun2
	| ATANH '(' expr ')'									# ATANH_fun2
	| ATAN2 '(' expr ',' expr ')'							# ATAN2_fun2
	| ROUND '(' expr ',' expr ')'							# ROUND_fun2
	| ROUNDDOWN '(' expr ',' expr ')'						# ROUNDDOWN_fun2
	| ROUNDUP '(' expr ',' expr ')'							# ROUNDUP_fun2
	| CEILING '(' expr (',' expr)? ')'						# CEILING_fun2
	| FLOOR '(' expr (',' expr)? ')'						# FLOOR_fun2
	| EVEN '(' expr ')'										# EVEN_fun2
	| ODD '(' expr ')'										# ODD_fun2
	| MROUND '(' expr ',' expr ')'							# MROUND_fun2
	| RAND '(' ')'											# RAND_fun2
	| RANDBETWEEN '(' expr ',' expr ')'						# RANDBETWEEN_fun2
	| FACT '(' expr ')'										# FACT_fun2
	| FACTDOUBLE '(' expr ')'								# FACTDOUBLE_fun2
	| POWER '(' expr ',' expr ')'							# POWER_fun2
	| EXP '(' expr ')'										# EXP_fun2
	| LN '(' expr ')'										# LN_fun2
	| LOG '(' expr (',' expr)? ')'							# LOG_fun2
	| LOG10 '(' expr ')'									# LOG10_fun2
	| MULTINOMIAL '(' expr (',' expr)* ')'					# MULTINOMIAL_fun2
	| PRODUCT '(' expr (',' expr)* ')'						# PRODUCT_fun2
	| SQRTPI '(' expr ')'									# SQRTPI_fun2
	| SUMSQ '(' expr (',' expr)* ')'						# SUMSQ_fun2
	| ASC '(' expr ')'										# ASC_fun2
	| JIS '(' expr ')'										# JIS_fun2
	| CHAR '(' expr ')'										# CHAR_fun2
	| CLEAN '(' expr ')'									# CLEAN_fun2
	| CODE '(' expr ')'										# CODE_fun2
	| CONCATENATE '(' expr (',' expr)* ')'					# CONCATENATE_fun2
	| EXACT '(' expr ',' expr ')'							# EXACT_fun2
	| FIND '(' expr ',' expr (',' expr)? ')'				# FIND_fun2
	| FIXED '(' expr (',' expr (',' expr)?)? ')'			# FIXED_fun2
	| LEFT '(' expr (',' expr)? ')'							# LEFT_fun2
	| LEN '(' expr ')'										# LEN_fun2
	| LOWER '(' expr ')'									# LOWER_fun2
	| MID '(' expr ',' expr ',' expr ')'					# MID_fun2
	| PROPER '(' expr ')'									# PROPER_fun2
	| REPLACE '(' expr ',' expr ',' expr (',' expr)? ')'	# REPLACE_fun2
	| REPT '(' expr ',' expr ')'							# REPT_fun2
	| RIGHT '(' expr (',' expr)? ')'						# RIGHT_fun2
	| RMB '(' expr ')'										# RMB_fun2
	| SEARCH '(' expr ',' expr (',' expr)? ')'				# SEARCH_fun2
	| SUBSTITUTE '(' expr ',' expr ',' expr (',' expr)? ')'	# SUBSTITUTE_fun2
	| T '(' expr ')'										# T_fun2
	| TEXT '(' expr ',' expr ')'							# TEXT_fun2
	| TRIM '(' expr ')'										# TRIM_fun2
	| UPPER '(' expr ')'									# UPPER_fun2
	| VALUE '(' expr ')'									# VALUE_fun2
	| DATEVALUE '(' expr ')'								# DATEVALUE_fun2
	| TIMEVALUE '(' expr ')'								# TIMEVALUE_fun2
	| DATE '(' expr ',' expr ',' expr (
		',' expr (',' expr (',' expr)?)?
	)? ')'														# DATE_fun2
	| TIME '(' expr ',' expr (',' expr)? ')'					# TIME_fun2
	| NOW '(' ')'												# NOW_fun2
	| TODAY '(' ')'												# TODAY_fun2
	| YEAR '(' expr ')'											# YEAR_fun2
	| MONTH '(' expr ')'										# MONTH_fun2
	| DAY '(' expr ')'											# DAY_fun2
	| HOUR '(' expr ')'											# HOUR_fun2
	| MINUTE '(' expr ')'										# MINUTE_fun2
	| SECOND '(' expr ')'										# SECOND_fun2
	| WEEKDAY '(' expr (',' expr)? ')'							# WEEKDAY_fun2
	| DATEDIF '(' expr ',' expr ',' expr ')'					# DATEDIF_fun2
	| DAYS360 '(' expr ',' expr (',' expr)? ')'					# DAYS360_fun2
	| EDATE '(' expr ',' expr ')'								# EDATE_fun2
	| EOMONTH '(' expr ',' expr ')'								# EOMONTH_fun2
	| NETWORKDAYS '(' expr ',' expr (',' expr)? ')'				# NETWORKDAYS_fun2
	| WORKDAY '(' expr ',' expr (',' expr)? ')'					# WORKDAY_fun2
	| WEEKNUM '(' expr (',' expr)? ')'							# WEEKNUM_fun2
	| MAX '(' expr (',' expr)+ ')'								# MAX_fun2
	| MEDIAN '(' expr (',' expr)+ ')'							# MEDIAN_fun2
	| MIN '(' expr (',' expr)+ ')'								# MIN_fun2
	| QUARTILE '(' expr ',' expr ')'							# QUARTILE_fun2
	| MODE '(' expr (',' expr)* ')'								# MODE_fun2
	| LARGE '(' expr ',' expr ')'								# LARGE_fun2
	| SMALL '(' expr ',' expr ')'								# SMALL_fun2
	| PERCENTILE '(' expr ',' expr ')'							# PERCENTILE_fun2
	| PERCENTRANK '(' expr ',' expr ')'							# PERCENTRANK_fun2
	| AVERAGE '(' expr (',' expr)* ')'							# AVERAGE_fun2
	| AVERAGEIF '(' expr ',' expr (',' expr)? ')'				# AVERAGEIF_fun2
	| GEOMEAN '(' expr (',' expr)* ')'							# GEOMEAN_fun2
	| HARMEAN '(' expr (',' expr)* ')'							# HARMEAN_fun2
	| COUNT '(' expr (',' expr)* ')'							# COUNT_fun2
	| COUNTIF '(' expr (',' expr)* ')'							# COUNTIF_fun2
	| SUM '(' expr (',' expr)* ')'								# SUM_fun2
	| SUMIF '(' expr ',' expr (',' expr)? ')'					# SUMIF_fun2
	| AVEDEV '(' expr (',' expr)* ')'							# AVEDEV_fun2
	| STDEV '(' expr (',' expr)* ')'							# STDEV_fun2
	| STDEVP '(' expr (',' expr)* ')'							# STDEVP_fun2
	| DEVSQ '(' expr (',' expr)* ')'							# DEVSQ_fun2
	| VAR '(' expr (',' expr)* ')'								# VAR_fun2
	| VARP '(' expr (',' expr)* ')'								# VARP_fun2
	| NORMDIST '(' expr ',' expr ',' expr ',' expr ')'			# NORMDIST_fun2
	| NORMINV '(' expr ',' expr ',' expr ')'					# NORMINV_fun2
	| NORMSDIST '(' expr ')'									# NORMSDIST_fun2
	| NORMSINV '(' expr ')'										# NORMSINV_fun2
	| BETADIST '(' expr ',' expr ',' expr ')'					# BETADIST_fun2
	| BETAINV '(' expr ',' expr ',' expr ')'					# BETAINV_fun2
	| BINOMDIST '(' expr ',' expr ',' expr ',' expr ')'			# BINOMDIST_fun2
	| EXPONDIST '(' expr ',' expr ',' expr ')'					# EXPONDIST_fun2
	| FDIST '(' expr ',' expr ',' expr ')'						# FDIST_fun2
	| FINV '(' expr ',' expr ',' expr ')'						# FINV_fun2
	| FISHER '(' expr ')'										# FISHER_fun2
	| FISHERINV '(' expr ')'									# FISHERINV_fun2
	| GAMMADIST '(' expr ',' expr ',' expr ',' expr ')'			# GAMMADIST_fun2
	| GAMMAINV '(' expr ',' expr ',' expr ')'					# GAMMAINV_fun2
	| GAMMALN '(' expr ')'										# GAMMALN_fun2
	| HYPGEOMDIST '(' expr ',' expr ',' expr ',' expr ')'		# HYPGEOMDIST_fun2
	| LOGINV '(' expr ',' expr ',' expr ')'						# LOGINV_fun2
	| LOGNORMDIST '(' expr ',' expr ',' expr ')'				# LOGNORMDIST_fun2
	| NEGBINOMDIST '(' expr ',' expr ',' expr ')'				# NEGBINOMDIST_fun2
	| POISSON '(' expr ',' expr ',' expr ')'					# POISSON_fun2
	| TDIST '(' expr ',' expr ',' expr ')'						# TDIST_fun2
	| TINV '(' expr ',' expr ')'								# TINV_fun2
	| WEIBULL '(' expr ',' expr ',' expr ',' expr ')'			# WEIBULL_fun2
	| URLENCODE '(' expr ')'									# URLENCODE_fun2
	| URLDECODE '(' expr ')'									# URLDECODE_fun2
	| HTMLENCODE '(' expr ')'									# HTMLENCODE_fun2
	| HTMLDECODE '(' expr ')'									# HTMLDECODE_fun2
	| BASE64TOTEXT '(' expr (',' expr)? ')'						# BASE64TOTEXT_fun2
	| BASE64URLTOTEXT '(' expr (',' expr)? ')'					# BASE64URLTOTEXT_fun2
	| TEXTTOBASE64 '(' expr (',' expr)? ')'						# TEXTTOBASE64_fun2
	| TEXTTOBASE64URL '(' expr (',' expr)? ')'					# TEXTTOBASE64URL_fun2
	| REGEX '(' expr ',' expr ')'								# REGEX_fun2
	| REGEXREPALCE '(' expr ',' expr ',' expr ')'				# REGEXREPALCE_fun2
	| ISREGEX '(' expr ',' expr ')'								# ISREGEX_fun2
	| GUID '(' ')'												# GUID_fun2
	| MD5 '(' expr (',' expr)? ')'								# MD5_fun2
	| SHA1 '(' expr (',' expr)? ')'								# SHA1_fun2
	| SHA256 '(' expr (',' expr)? ')'							# SHA256_fun2
	| SHA512 '(' expr (',' expr)? ')'							# SHA512_fun2
	| CRC32 '(' expr (',' expr)? ')'							# CRC32_fun2
	| HMACMD5 '(' expr ',' expr (',' expr)? ')'					# HMACMD5_fun2
	| HMACSHA1 '(' expr ',' expr (',' expr)? ')'				# HMACSHA1_fun2
	| HMACSHA256 '(' expr ',' expr (',' expr)? ')'				# HMACSHA256_fun2
	| HMACSHA512 '(' expr ',' expr (',' expr)? ')'				# HMACSHA512_fun2
	| TRIMSTART '(' expr (',' expr)? ')'						# TRIMSTART_fun2
	| TRIMEND '(' expr (',' expr)? ')'							# TRIMEND_fun2
	| INDEXOF '(' expr ',' expr (',' expr (',' expr)?)? ')'		# INDEXOF_fun2
	| LASTINDEXOF '(' expr ',' expr (',' expr (',' expr)?)? ')'	# LASTINDEXOF_fun2
	| SPLIT '(' expr ',' expr ')'								# SPLIT_fun2
	| JOIN '(' expr (',' expr)+ ')'								# JOIN_fun2
	| SUBSTRING '(' expr ',' expr (',' expr)? ')'				# SUBSTRING_fun2
	| STARTSWITH '(' expr ',' expr (',' expr)? ')'				# STARTSWITH_fun2
	| ENDSWITH '(' expr ',' expr (',' expr)? ')'				# ENDSWITH_fun2
	| ISNULLOREMPTY '(' expr ')'								# ISNULLOREMPTY_fun2
	| ISNULLORWHITESPACE '(' expr ')'							# ISNULLORWHITESPACE_fun2
	| REMOVESTART '(' expr (',' expr (',' expr)?)? ')'			# REMOVESTART_fun2
	| REMOVEEND '(' expr (',' expr (',' expr)?)? ')'			# REMOVEEND_fun2
	| JSON '(' expr ')'											# JSON_fun2
	| VLOOKUP '(' expr ',' expr ',' expr (',' expr)? ')'		# VLOOKUP_fun2
	| LOOKUP '(' expr ',' expr ',' expr ')'						# LOOKUP_fun2
	| PARAMETER '(' (expr (',' expr)*)? ')'						# DiyFunction_fun2
	| '[' parameter ']'											# PARAMETER_fun2
	| '-'? NUM													# NUM_fun2
	| STRING													# STRING_fun2
	| NULL														# NULL_fun2;

parameter: expr | parameter2;

parameter2:
	E
	| IF
	| IFERROR
	| ISNUMBER
	| ISTEXT
	| ISERROR
	| ISNONTEXT
	| ISLOGICAL
	| ISEVEN
	| ISODD
	| IFERROR
	| ISNULL
	| ISNULLORERROR
	| AND
	| OR
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
	| ACOS
	| ACOSH
	| ASIN
	| ASINH
	| ATAN
	| ATANH
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
	| SUMSQ
	| ASC
	| JIS
	| CHAR
	| CLEAN
	| CODE
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
	| REGEXREPALCE
	| ISREGEX
	| GUID
	| MD5
	| SHA1
	| SHA256
	| SHA512
	| CRC32
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
	| VLOOKUP
	| LOOKUP
	| NULL
	| PARAMETER;
SUB: '-';
NUM: '0' ('.' [0-9]+)? | [1-9][0-9]* ('.' [0-9]+)?;
STRING: '\'' ( ~'\'' | '\\\'')* '\'' | '"' ( ~'"' | '\\"')* '"';
NULL: 'NULL';

// 逻辑函数
IF: 'IF';
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
NOT: 'NOT';
TRUE: 'TRUE';
FALSE: 'FALSE';
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
ACOS: 'ACOS';
ACOSH: 'ACOSH';
ASIN: 'ASIN';
ASINH: 'ASINH';
ATAN: 'ATAN';
ATANH: 'ATANH';
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
SUMSQ: 'SUMSQ';
// 文本函数
ASC: 'ASC';
JIS: 'JIS' | 'WIDECHAR';
CHAR: 'CHAR';
CLEAN: 'CLEAN';
CODE: 'CODE';
CONCATENATE: 'CONCATENATE';
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
PERCENTILE: 'PERCENTILE';
PERCENTRANK: 'PERCENTRANK';
AVERAGE: 'AVERAGE';
AVERAGEIF: 'AVERAGEIF';
GEOMEAN: 'GEOMEAN';
HARMEAN: 'HARMEAN';
COUNT: 'COUNT';
COUNTIF: 'COUNTIF';
SUM: 'SUM';
SUMIF: 'SUMIF';
AVEDEV: 'AVEDEV';
STDEV: 'STDEV';
STDEVP: 'STDEVP';
DEVSQ: 'DEVSQ';
VAR: 'VAR';
VARP: 'VARP';
NORMDIST: 'NORMDIST';
NORMINV: 'NORMINV';
NORMSDIST: 'NORMSDIST';
NORMSINV: 'NORMSINV';
BETADIST: 'BETADIST';
BETAINV: 'BETAINV';
BINOMDIST: 'BINOMDIST';
EXPONDIST: 'EXPONDIST';
FDIST: 'FDIST';
FINV: 'FINV';
FISHER: 'FISHER';
FISHERINV: 'FISHERINV';
GAMMADIST: 'GAMMADIST';
GAMMAINV: 'GAMMAINV';
GAMMALN: 'GAMMALN';
HYPGEOMDIST: 'HYPGEOMDIST';
LOGINV: 'LOGINV';
LOGNORMDIST: 'LOGNORMDIST';
NEGBINOMDIST: 'NEGBINOMDIST';
POISSON: 'POISSON';
TDIST: 'TDIST';
TINV: 'TINV';
WEIBULL: 'WEIBULL';
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
REGEXREPALCE: 'REGEXREPALCE';
ISREGEX: 'ISREGEX' | 'ISMATCH';
GUID: 'GUID';
MD5: 'MD5';
SHA1: 'SHA1';
SHA256: 'SHA256';
SHA512: 'SHA512';
CRC32: 'CRC32';
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
VLOOKUP: 'VLOOKUP';
LOOKUP: 'LOOKUP';

PARAMETER: ([A-Z_]| FullWidthLetter)([A-Z0-9_] | FullWidthLetter)*;

fragment FullWidthLetter
    : '\u00c0'..'\u00d6'
    | '\u00d8'..'\u00f6'
    | '\u00f8'..'\u00ff'
    | '\u0100'..'\u1fff'
    | '\u2c00'..'\u2fff'
    | '\u3040'..'\u318f'
    | '\u3300'..'\u337f'
    | '\u3400'..'\u3fff'
    | '\u4e00'..'\u9fff'
    | '\ua000'..'\ud7ff'
    | '\uf900'..'\ufaff'
    | '\uff00'..'\ufff0'
    // | '\u10000'..'\u1F9FF'  //not support four bytes chars
    // | '\u20000'..'\u2FA1F'
    ;

WS: [ \t\r\n]+ -> skip;