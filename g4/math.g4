grammar math;

prog: expr;

expr:
	 expr ('?'|'？') expr (':'|'：') expr 					# IF_fun
	| expr op = ('*' | '/' | '%' ) expr	# MulDiv_fun
	| expr op = ('+' | '-' | '&') expr	# AddSub_fun
	| expr op = (
		'>'
		| '>='
		| '<'
		| '<='
		| '='
		| '=='
		| '==='
		| '!='
		| '！='
		| '<>'
	) expr														# Judge_fun
	| expr op = ('&&' | '||' | AND | OR) expr					# AndOr_fun
	| expr '.' ISNUMBER LB RB									# ISNUMBER_fun
	| expr '.' ISTEXT LB RB									# ISTEXT_fun
	| expr '.' ISNONTEXT LB RB								# ISNONTEXT_fun
	| expr '.' ISLOGICAL LB RB								# ISLOGICAL_fun
	| expr '.' ISEVEN LB RB									# ISEVEN_fun
	| expr '.' ISODD LB RB									# ISODD_fun
	| expr '.' ISERROR LB expr? RB							# ISERROR_fun
	| expr '.' ISNULL LB expr? RB								# ISNULL_fun
	| expr '.' ISNULLORERROR LB expr? RB						# ISNULLORERROR_fun
	| expr '.' DEC2BIN (LB expr? RB)							# DEC2BIN_fun
	| expr '.' DEC2HEX (LB expr? RB)							# DEC2HEX_fun
	| expr '.' DEC2OCT (LB expr? RB)							# DEC2OCT_fun
	| expr '.' HEX2BIN (LB expr? RB)							# HEX2BIN_fun
	| expr '.' HEX2DEC (LB RB)								# HEX2DEC_fun
	| expr '.' HEX2OCT (LB expr? RB)							# HEX2OCT_fun
	| expr '.' OCT2BIN (LB expr? RB)							# OCT2BIN_fun
	| expr '.' OCT2DEC (LB RB)								# OCT2DEC_fun
	| expr '.' OCT2HEX (LB expr? RB)							# OCT2HEX_fun
	| expr '.' BIN2OCT (LB expr? RB)							# BIN2OCT_fun
	| expr '.' BIN2DEC (LB RB)								# BIN2DEC_fun
	| expr '.' BIN2HEX (LB expr? RB)							# BIN2HEX_fun
	| expr '.' INT LB RB										# INT_fun
	| expr '.' ASC LB RB										# ASC_fun
	| expr '.' JIS LB RB										# JIS_fun
	| expr '.' CHAR LB RB										# CHAR_fun
	| expr '.' CLEAN LB RB									# CLEAN_fun
	| expr '.' CODE LB RB										# CODE_fun
	| expr '.' CONCATENATE LB (expr (COMMA expr)*)? RB			# CONCATENATE_fun
	| expr '.' EXACT LB expr RB								# EXACT_fun
	| expr '.' FIND LB expr (COMMA expr)? RB					# FIND_fun
	| expr '.' LEFT LB expr? RB								# LEFT_fun
	| expr '.' LEN LB RB										# LEN_fun
	| expr '.' LOWER LB RB									# LOWER_fun
	| expr '.' MID LB expr COMMA expr RB						# MID_fun
	| expr '.' PROPER LB RB									# PROPER_fun
	| expr '.' REPLACE LB expr COMMA expr (COMMA expr)? RB		# REPLACE_fun
	| expr '.' REPT LB expr RB								# REPT_fun
	| expr '.' RIGHT LB expr? RB								# RIGHT_fun
	| expr '.' RMB LB RB										# RMB_fun
	| expr '.' SEARCH LB expr (COMMA expr)? RB					# SEARCH_fun
	| expr '.' SUBSTITUTE LB expr COMMA expr (COMMA expr)? RB		# SUBSTITUTE_fun
	| expr '.' T LB RB										# T_fun
	| expr '.' TEXT LB expr RB								# TEXT_fun
	| expr '.' TRIM LB RB										# TRIM_fun
	| expr '.' UPPER LB RB									# UPPER_fun
	| expr '.' VALUE LB RB									# VALUE_fun
	| expr '.' DATEVALUE LB RB								# DATEVALUE_fun
	| expr '.' TIMEVALUE LB RB								# TIMEVALUE_fun
	| expr '.' YEAR (LB RB)?									# YEAR_fun
	| expr '.' MONTH (LB RB)?									# MONTH_fun
	| expr '.' DAY (LB RB)?									# DAY_fun
	| expr '.' HOUR (LB RB)?									# HOUR_fun
	| expr '.' MINUTE (LB RB)?								# MINUTE_fun
	| expr '.' SECOND (LB RB)?								# SECOND_fun
	| expr '.' URLENCODE LB RB								# URLENCODE_fun
	| expr '.' URLDECODE LB RB								# URLDECODE_fun
	| expr '.' HTMLENCODE LB RB								# HTMLENCODE_fun
	| expr '.' HTMLDECODE LB RB								# HTMLDECODE_fun
	| expr '.' BASE64TOTEXT LB expr? RB						# BASE64TOTEXT_fun
	| expr '.' BASE64URLTOTEXT LB expr? RB					# BASE64URLTOTEXT_fun
	| expr '.' TEXTTOBASE64 LB expr? RB						# TEXTTOBASE64_fun
	| expr '.' TEXTTOBASE64URL LB expr? RB					# TEXTTOBASE64URL_fun
	| expr '.' REGEX LB expr RB								# REGEX_fun
	| expr '.' REGEXREPALCE LB expr COMMA expr RB				# REGEXREPALCE_fun
	| expr '.' ISREGEX LB expr RB								# ISREGEX_fun
	| expr '.' MD5 LB expr? RB									# MD5_fun
	| expr '.' SHA1 LB expr? RB									# SHA1_fun
	| expr '.' SHA256 LB expr? RB								# SHA256_fun
	| expr '.' SHA512 LB expr? RB								# SHA512_fun
	| expr '.' CRC32 LB expr? RB								# CRC32_fun
	| expr '.' HMACMD5 LB expr (COMMA expr)? RB					# HMACMD5_fun
	| expr '.' HMACSHA1 LB expr (COMMA expr)? RB				# HMACSHA1_fun
	| expr '.' HMACSHA256 LB expr (COMMA expr)? RB				# HMACSHA256_fun
	| expr '.' HMACSHA512 LB expr (COMMA expr)? RB				# HMACSHA512_fun
	| expr '.' TRIMSTART LB expr? RB							# TRIMSTART_fun
	| expr '.' TRIMEND LB expr? RB								# TRIMEND_fun
	| expr '.' INDEXOF LB expr (COMMA expr (COMMA expr)?)? RB		# INDEXOF_fun
	| expr '.' LASTINDEXOF LB expr (COMMA expr (COMMA expr)?)? RB	# LASTINDEXOF_fun
	| expr '.' SPLIT LB expr RB									# SPLIT_fun
	| expr '.' JOIN LB expr (COMMA expr)* RB					# JOIN_fun
	| expr '.' SUBSTRING LB expr (COMMA expr)? RB				# SUBSTRING_fun
	| expr '.' STARTSWITH LB expr (COMMA expr)? RB				# STARTSWITH_fun
	| expr '.' ENDSWITH LB expr (COMMA expr)? RB				# ENDSWITH_fun
	| expr '.' ISNULLOREMPTY LB RB								# ISNULLOREMPTY_fun
	| expr '.' ISNULLORWHITESPACE LB RB							# ISNULLORWHITESPACE_fun
	| expr '.' REMOVESTART LB expr (COMMA expr)? RB				# REMOVESTART_fun
	| expr '.' REMOVEEND LB expr (COMMA expr)? RB				# REMOVEEND_fun
	| expr '.' JSON LB RB										# JSON_fun
	| expr '.' VLOOKUP LB expr COMMA expr (COMMA expr)? RB		# VLOOKUP_fun
	| expr '.' LOOKUP LB expr COMMA expr RB						# LOOKUP_fun
	| expr '.' PARAMETER LB (expr (COMMA expr)*)? RB			# DiyFunction_fun
	| expr '[' parameter ']'									# GetJsonValue_fun
	| expr '.' parameter2										# GetJsonValue_fun
	| expr '%'													# Percentage_fun
	| ('!'| '！') expr											# Not_fun
	| expr2														# expr2_fun;

expr2:
	 LB expr RB											# Bracket_fun2
	| ARRAY LB expr (COMMA expr)* RB						# Array_fun2
	| IF LB expr COMMA expr (COMMA expr)? RB					# IF_fun2
	| ISNUMBER LB expr RB									# ISNUMBER_fun2
	| ISTEXT LB expr RB									# ISTEXT_fun2
	| ISERROR LB expr (COMMA expr)? RB						# ISERROR_fun2
	| ISNONTEXT LB expr RB								# ISNONTEXT_fun2
	| ISLOGICAL LB expr RB								# ISLOGICAL_fun2
	| ISEVEN LB expr RB									# ISEVEN_fun2
	| ISODD LB expr RB									# ISODD_fun2
	| IFERROR LB expr COMMA expr (COMMA expr)? RB				# IFERROR_fun2
	| ISNULL LB expr (COMMA expr)? RB						# ISNULL_fun2
	| ISNULLORERROR LB expr (COMMA expr)? RB				# ISNULLORERROR_fun2
	| AND LB expr (COMMA expr)* RB							# AND_fun2
	| OR LB expr (COMMA expr)* RB							# OR_fun2
	| NOT LB expr RB										# NOT_fun2
	| TRUE (LB RB)?										# TRUE_fun2
	| FALSE (LB RB)?										# FALSE_fun2
	| E (LB RB)?											# E_fun2
	| PI (LB RB)?											# PI_fun2
	| DEC2BIN (LB expr (COMMA expr)? RB)					# DEC2BIN_fun2
	| DEC2HEX (LB expr (COMMA expr)? RB)					# DEC2HEX_fun2
	| DEC2OCT (LB expr (COMMA expr)? RB)					# DEC2OCT_fun2
	| HEX2BIN (LB expr (COMMA expr)? RB)					# HEX2BIN_fun2
	| HEX2DEC (LB expr RB)								# HEX2DEC_fun2
	| HEX2OCT (LB expr (COMMA expr)? RB)					# HEX2OCT_fun2
	| OCT2BIN (LB expr (COMMA expr)? RB)					# OCT2BIN_fun2
	| OCT2DEC (LB expr RB)								# OCT2DEC_fun2
	| OCT2HEX (LB expr (COMMA expr)? RB)					# OCT2HEX_fun2
	| BIN2OCT (LB expr (COMMA expr)? RB)					# BIN2OCT_fun2
	| BIN2DEC (LB expr RB)								# BIN2DEC_fun2
	| BIN2HEX (LB expr (COMMA expr)? RB)					# BIN2HEX_fun2
	| ABS LB expr RB										# ABS_fun2
	| QUOTIENT LB expr (COMMA expr) RB						# QUOTIENT_fun2
	| MOD LB expr (COMMA expr) RB							# MOD_fun2
	| SIGN LB expr RB										# SIGN_fun2
	| SQRT LB expr RB										# SQRT_fun2
	| TRUNC LB expr RB									# TRUNC_fun2
	| INT LB expr RB										# INT_fun2
	| GCD LB expr (COMMA expr)+ RB							# GCD_fun2
	| LCM LB expr (COMMA expr)+ RB							# LCM_fun2
	| COMBIN LB expr COMMA expr RB							# COMBIN_fun2
	| PERMUT LB expr COMMA expr RB							# PERMUT_fun2
	| DEGREES LB expr RB									# DEGREES_fun2
	| RADIANS LB expr RB									# RADIANS_fun2
	| COS LB expr RB										# COS_fun2
	| COSH LB expr RB										# COSH_fun2
	| SIN LB expr RB										# SIN_fun2
	| SINH LB expr RB										# SINH_fun2
	| TAN LB expr RB										# TAN_fun2
	| TANH LB expr RB										# TANH_fun2
	| ACOS LB expr RB										# ACOS_fun2
	| ACOSH LB expr RB									# ACOSH_fun2
	| ASIN LB expr RB										# ASIN_fun2
	| ASINH LB expr RB									# ASINH_fun2
	| ATAN LB expr RB										# ATAN_fun2
	| ATANH LB expr RB									# ATANH_fun2
	| ATAN2 LB expr COMMA expr RB							# ATAN2_fun2
	| ROUND LB expr COMMA expr RB							# ROUND_fun2
	| ROUNDDOWN LB expr COMMA expr RB						# ROUNDDOWN_fun2
	| ROUNDUP LB expr COMMA expr RB							# ROUNDUP_fun2
	| CEILING LB expr (COMMA expr)? RB						# CEILING_fun2
	| FLOOR LB expr (COMMA expr)? RB						# FLOOR_fun2
	| EVEN LB expr RB										# EVEN_fun2
	| ODD LB expr RB										# ODD_fun2
	| MROUND LB expr COMMA expr RB							# MROUND_fun2
	| RAND LB RB											# RAND_fun2
	| RANDBETWEEN LB expr COMMA expr RB						# RANDBETWEEN_fun2
	| FACT LB expr RB										# FACT_fun2
	| FACTDOUBLE LB expr RB								# FACTDOUBLE_fun2
	| POWER LB expr COMMA expr RB							# POWER_fun2
	| EXP LB expr RB										# EXP_fun2
	| LN LB expr RB										# LN_fun2
	| LOG LB expr (COMMA expr)? RB							# LOG_fun2
	| LOG10 LB expr RB									# LOG10_fun2
	| MULTINOMIAL LB expr (COMMA expr)* RB					# MULTINOMIAL_fun2
	| PRODUCT LB expr (COMMA expr)* RB						# PRODUCT_fun2
	| SQRTPI LB expr RB									# SQRTPI_fun2
	| SUMSQ LB expr (COMMA expr)* RB						# SUMSQ_fun2
	| ASC LB expr RB										# ASC_fun2
	| JIS LB expr RB										# JIS_fun2
	| CHAR LB expr RB										# CHAR_fun2
	| CLEAN LB expr RB									# CLEAN_fun2
	| CODE LB expr RB										# CODE_fun2
	| CONCATENATE LB expr (COMMA expr)* RB					# CONCATENATE_fun2
	| EXACT LB expr COMMA expr RB							# EXACT_fun2
	| FIND LB expr COMMA expr (COMMA expr)? RB				# FIND_fun2
	| FIXED LB expr (COMMA expr (COMMA expr)?)? RB			# FIXED_fun2
	| LEFT LB expr (COMMA expr)? RB							# LEFT_fun2
	| LEN LB expr RB										# LEN_fun2
	| LOWER LB expr RB									# LOWER_fun2
	| MID LB expr COMMA expr COMMA expr RB					# MID_fun2
	| PROPER LB expr RB									# PROPER_fun2
	| REPLACE LB expr COMMA expr COMMA expr (COMMA expr)? RB	# REPLACE_fun2
	| REPT LB expr COMMA expr RB							# REPT_fun2
	| RIGHT LB expr (COMMA expr)? RB						# RIGHT_fun2
	| RMB LB expr RB										# RMB_fun2
	| SEARCH LB expr COMMA expr (COMMA expr)? RB				# SEARCH_fun2
	| SUBSTITUTE LB expr COMMA expr COMMA expr (COMMA expr)? RB	# SUBSTITUTE_fun2
	| T LB expr RB										# T_fun2
	| TEXT LB expr COMMA expr RB							# TEXT_fun2
	| TRIM LB expr RB										# TRIM_fun2
	| UPPER LB expr RB									# UPPER_fun2
	| VALUE LB expr RB									# VALUE_fun2
	| DATEVALUE LB expr RB								# DATEVALUE_fun2
	| TIMEVALUE LB expr RB								# TIMEVALUE_fun2
	| DATE LB expr COMMA expr COMMA expr (
		COMMA expr (COMMA expr (COMMA expr)?)?
	)? RB														# DATE_fun2
	| TIME LB expr COMMA expr (COMMA expr)? RB					# TIME_fun2
	| NOW LB RB												# NOW_fun2
	| TODAY LB RB												# TODAY_fun2
	| YEAR LB expr RB											# YEAR_fun2
	| MONTH LB expr RB										# MONTH_fun2
	| DAY LB expr RB											# DAY_fun2
	| HOUR LB expr RB											# HOUR_fun2
	| MINUTE LB expr RB										# MINUTE_fun2
	| SECOND LB expr RB										# SECOND_fun2
	| WEEKDAY LB expr (COMMA expr)? RB							# WEEKDAY_fun2
	| DATEDIF LB expr COMMA expr COMMA expr RB					# DATEDIF_fun2
	| DAYS360 LB expr COMMA expr (COMMA expr)? RB					# DAYS360_fun2
	| EDATE LB expr COMMA expr RB								# EDATE_fun2
	| EOMONTH LB expr COMMA expr RB								# EOMONTH_fun2
	| NETWORKDAYS LB expr COMMA expr (COMMA expr)? RB				# NETWORKDAYS_fun2
	| WORKDAY LB expr COMMA expr (COMMA expr)? RB					# WORKDAY_fun2
	| WEEKNUM LB expr (COMMA expr)? RB							# WEEKNUM_fun2
	| MAX LB expr (COMMA expr)+ RB								# MAX_fun2
	| MEDIAN LB expr (COMMA expr)+ RB							# MEDIAN_fun2
	| MIN LB expr (COMMA expr)+ RB								# MIN_fun2
	| QUARTILE LB expr COMMA expr RB							# QUARTILE_fun2
	| MODE LB expr (COMMA expr)* RB								# MODE_fun2
	| LARGE LB expr COMMA expr RB								# LARGE_fun2
	| SMALL LB expr COMMA expr RB								# SMALL_fun2
	| PERCENTILE LB expr COMMA expr RB							# PERCENTILE_fun2
	| PERCENTRANK LB expr COMMA expr RB							# PERCENTRANK_fun2
	| AVERAGE LB expr (COMMA expr)* RB							# AVERAGE_fun2
	| AVERAGEIF LB expr COMMA expr (COMMA expr)? RB				# AVERAGEIF_fun2
	| GEOMEAN LB expr (COMMA expr)* RB							# GEOMEAN_fun2
	| HARMEAN LB expr (COMMA expr)* RB							# HARMEAN_fun2
	| COUNT LB expr (COMMA expr)* RB							# COUNT_fun2
	| COUNTIF LB expr (COMMA expr)* RB							# COUNTIF_fun2
	| SUM LB expr (COMMA expr)* RB								# SUM_fun2
	| SUMIF LB expr COMMA expr (COMMA expr)? RB					# SUMIF_fun2
	| AVEDEV LB expr (COMMA expr)* RB							# AVEDEV_fun2
	| STDEV LB expr (COMMA expr)* RB							# STDEV_fun2
	| STDEVP LB expr (COMMA expr)* RB							# STDEVP_fun2
	| DEVSQ LB expr (COMMA expr)* RB							# DEVSQ_fun2
	| VAR LB expr (COMMA expr)* RB								# VAR_fun2
	| VARP LB expr (COMMA expr)* RB								# VARP_fun2
	| NORMDIST LB expr COMMA expr COMMA expr COMMA expr RB			# NORMDIST_fun2
	| NORMINV LB expr COMMA expr COMMA expr RB					# NORMINV_fun2
	| NORMSDIST LB expr RB									# NORMSDIST_fun2
	| NORMSINV LB expr RB										# NORMSINV_fun2
	| BETADIST LB expr COMMA expr COMMA expr RB					# BETADIST_fun2
	| BETAINV LB expr COMMA expr COMMA expr RB					# BETAINV_fun2
	| BINOMDIST LB expr COMMA expr COMMA expr COMMA expr RB			# BINOMDIST_fun2
	| EXPONDIST LB expr COMMA expr COMMA expr RB					# EXPONDIST_fun2
	| FDIST LB expr COMMA expr COMMA expr RB						# FDIST_fun2
	| FINV LB expr COMMA expr COMMA expr RB						# FINV_fun2
	| FISHER LB expr RB										# FISHER_fun2
	| FISHERINV LB expr RB									# FISHERINV_fun2
	| GAMMADIST LB expr COMMA expr COMMA expr COMMA expr RB			# GAMMADIST_fun2
	| GAMMAINV LB expr COMMA expr COMMA expr RB					# GAMMAINV_fun2
	| GAMMALN LB expr RB										# GAMMALN_fun2
	| HYPGEOMDIST LB expr COMMA expr COMMA expr COMMA expr RB		# HYPGEOMDIST_fun2
	| LOGINV LB expr COMMA expr COMMA expr RB						# LOGINV_fun2
	| LOGNORMDIST LB expr COMMA expr COMMA expr RB				# LOGNORMDIST_fun2
	| NEGBINOMDIST LB expr COMMA expr COMMA expr RB				# NEGBINOMDIST_fun2
	| POISSON LB expr COMMA expr COMMA expr RB					# POISSON_fun2
	| TDIST LB expr COMMA expr COMMA expr RB						# TDIST_fun2
	| TINV LB expr COMMA expr RB								# TINV_fun2
	| WEIBULL LB expr COMMA expr COMMA expr COMMA expr RB			# WEIBULL_fun2
	| URLENCODE LB expr RB									# URLENCODE_fun2
	| URLDECODE LB expr RB									# URLDECODE_fun2
	| HTMLENCODE LB expr RB									# HTMLENCODE_fun2
	| HTMLDECODE LB expr RB									# HTMLDECODE_fun2
	| BASE64TOTEXT LB expr (COMMA expr)? RB						# BASE64TOTEXT_fun2
	| BASE64URLTOTEXT LB expr (COMMA expr)? RB					# BASE64URLTOTEXT_fun2
	| TEXTTOBASE64 LB expr (COMMA expr)? RB						# TEXTTOBASE64_fun2
	| TEXTTOBASE64URL LB expr (COMMA expr)? RB					# TEXTTOBASE64URL_fun2
	| REGEX LB expr COMMA expr RB								# REGEX_fun2
	| REGEXREPALCE LB expr COMMA expr COMMA expr RB				# REGEXREPALCE_fun2
	| ISREGEX LB expr COMMA expr RB								# ISREGEX_fun2
	| GUID LB RB												# GUID_fun2
	| MD5 LB expr (COMMA expr)? RB								# MD5_fun2
	| SHA1 LB expr (COMMA expr)? RB								# SHA1_fun2
	| SHA256 LB expr (COMMA expr)? RB							# SHA256_fun2
	| SHA512 LB expr (COMMA expr)? RB							# SHA512_fun2
	| CRC32 LB expr (COMMA expr)? RB							# CRC32_fun2
	| HMACMD5 LB expr COMMA expr (COMMA expr)? RB					# HMACMD5_fun2
	| HMACSHA1 LB expr COMMA expr (COMMA expr)? RB				# HMACSHA1_fun2
	| HMACSHA256 LB expr COMMA expr (COMMA expr)? RB				# HMACSHA256_fun2
	| HMACSHA512 LB expr COMMA expr (COMMA expr)? RB				# HMACSHA512_fun2
	| TRIMSTART LB expr (COMMA expr)? RB						# TRIMSTART_fun2
	| TRIMEND LB expr (COMMA expr)? RB							# TRIMEND_fun2
	| INDEXOF LB expr COMMA expr (COMMA expr (COMMA expr)?)? RB		# INDEXOF_fun2
	| LASTINDEXOF LB expr COMMA expr (COMMA expr (COMMA expr)?)? RB	# LASTINDEXOF_fun2
	| SPLIT LB expr COMMA expr RB								# SPLIT_fun2
	| JOIN LB expr (COMMA expr)+ RB								# JOIN_fun2
	| SUBSTRING LB expr COMMA expr (COMMA expr)? RB				# SUBSTRING_fun2
	| STARTSWITH LB expr COMMA expr (COMMA expr)? RB				# STARTSWITH_fun2
	| ENDSWITH LB expr COMMA expr (COMMA expr)? RB				# ENDSWITH_fun2
	| ISNULLOREMPTY LB expr RB								# ISNULLOREMPTY_fun2
	| ISNULLORWHITESPACE LB expr RB							# ISNULLORWHITESPACE_fun2
	| REMOVESTART LB expr (COMMA expr (COMMA expr)?)? RB			# REMOVESTART_fun2
	| REMOVEEND LB expr (COMMA expr (COMMA expr)?)? RB			# REMOVEEND_fun2
	| JSON LB expr RB											# JSON_fun2
	| VLOOKUP LB expr COMMA expr COMMA expr (COMMA expr)? RB		# VLOOKUP_fun2
	| LOOKUP LB expr COMMA expr COMMA expr RB						# LOOKUP_fun2
	| PARAMETER LB (expr (COMMA expr)*)? RB						# DiyFunction_fun2
	| '[' parameter ']'											# PARAMETER_fun2
	| PARAMETER													# PARAMETER_fun2
	| PARAMETER2												# PARAMETER_fun2
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
LB: '(' | '（';
RB: ')' | '）';
COMMA: ','| '，';
NUM: '0' ('.' [0-9]+)? | [1-9][0-9]* ('.' [0-9]+)?
	| ('0' ('.' [0-9]+)? | [1-9][0-9]* ('.' [0-9]+)?) [Ee] [+-]? [0-9][0-9]?
;
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
ARRAY:'ARRAY';

PARAMETER: ([A-Z_]| FullWidthLetter)([A-Z0-9_] | FullWidthLetter)*;
PARAMETER2: '{' (~('{'|'}'))+ '}'
			| '【' (~('【'|'】'))+ '】'
			| '#' (~('#'))+ '#'
			| '@' ([A-Z_]| FullWidthLetter)([A-Z0-9_] | FullWidthLetter)*
			;

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