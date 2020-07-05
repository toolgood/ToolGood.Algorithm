// Generated from math.g4 by ANTLR 4.8
// jshint ignore: start
var antlr4 = require('antlr4/index');

// This class defines a complete generic visitor for a parse tree produced by mathParser.

function mathVisitor() {
	antlr4.tree.ParseTreeVisitor.call(this);
	return this;
}

mathVisitor.prototype = Object.create(antlr4.tree.ParseTreeVisitor.prototype);
mathVisitor.prototype.constructor = mathVisitor;

// Visit a parse tree produced by mathParser#prog.
mathVisitor.prototype.visitProg = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CRC8_fun.
mathVisitor.prototype.visitCRC8_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISLOGICAL_fun.
mathVisitor.prototype.visitISLOGICAL_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#INT_fun.
mathVisitor.prototype.visitINT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#URLDECODE_fun.
mathVisitor.prototype.visitURLDECODE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HEX2OCT_fun.
mathVisitor.prototype.visitHEX2OCT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REGEXREPALCE_fun.
mathVisitor.prototype.visitREGEXREPALCE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TEXT_fun.
mathVisitor.prototype.visitTEXT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#AddSub_fun.
mathVisitor.prototype.visitAddSub_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#YEAR_fun.
mathVisitor.prototype.visitYEAR_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BIN2HEX_fun.
mathVisitor.prototype.visitBIN2HEX_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CONCATENATE_fun.
mathVisitor.prototype.visitCONCATENATE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MONTH_fun.
mathVisitor.prototype.visitMONTH_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HTMLENCODE_fun.
mathVisitor.prototype.visitHTMLENCODE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BASE64URLTOTEXT_fun.
mathVisitor.prototype.visitBASE64URLTOTEXT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#URLENCODE_fun.
mathVisitor.prototype.visitURLENCODE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CRC16_fun.
mathVisitor.prototype.visitCRC16_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HMACSHA1_fun.
mathVisitor.prototype.visitHMACSHA1_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ENDSWITH_fun.
mathVisitor.prototype.visitENDSWITH_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#RIGHT_fun.
mathVisitor.prototype.visitRIGHT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#VALUE_fun.
mathVisitor.prototype.visitVALUE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DAY_fun.
mathVisitor.prototype.visitDAY_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISTEXT_fun.
mathVisitor.prototype.visitISTEXT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#expr2_fun.
mathVisitor.prototype.visitExpr2_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HMACSHA256_fun.
mathVisitor.prototype.visitHMACSHA256_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TIMEVALUE_fun.
mathVisitor.prototype.visitTIMEVALUE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#Judge_fun.
mathVisitor.prototype.visitJudge_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#OCT2BIN_fun.
mathVisitor.prototype.visitOCT2BIN_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#JSON_fun.
mathVisitor.prototype.visitJSON_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DEC2OCT_fun.
mathVisitor.prototype.visitDEC2OCT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#PROPER_fun.
mathVisitor.prototype.visitPROPER_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MINUTE_fun.
mathVisitor.prototype.visitMINUTE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#GetJsonValue_fun.
mathVisitor.prototype.visitGetJsonValue_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HEX2BIN_fun.
mathVisitor.prototype.visitHEX2BIN_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HOUR_fun.
mathVisitor.prototype.visitHOUR_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#INDEXOF_fun.
mathVisitor.prototype.visitINDEXOF_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LEN_fun.
mathVisitor.prototype.visitLEN_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#UPPER_fun.
mathVisitor.prototype.visitUPPER_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SHA512_fun.
mathVisitor.prototype.visitSHA512_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HTMLDECODE_fun.
mathVisitor.prototype.visitHTMLDECODE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISREGEX_fun.
mathVisitor.prototype.visitISREGEX_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISNULLORWHITESPACE_fun.
mathVisitor.prototype.visitISNULLORWHITESPACE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISNONTEXT_fun.
mathVisitor.prototype.visitISNONTEXT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DEC2BIN_fun.
mathVisitor.prototype.visitDEC2BIN_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#JOIN_fun.
mathVisitor.prototype.visitJOIN_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FIND_fun.
mathVisitor.prototype.visitFIND_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SUBSTITUTE_fun.
mathVisitor.prototype.visitSUBSTITUTE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REPT_fun.
mathVisitor.prototype.visitREPT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HEX2DEC_fun.
mathVisitor.prototype.visitHEX2DEC_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#OCT2DEC_fun.
mathVisitor.prototype.visitOCT2DEC_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SEARCH_fun.
mathVisitor.prototype.visitSEARCH_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SHA256_fun.
mathVisitor.prototype.visitSHA256_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CODE_fun.
mathVisitor.prototype.visitCODE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TEXTTOBASE64_fun.
mathVisitor.prototype.visitTEXTTOBASE64_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MulDiv_fun.
mathVisitor.prototype.visitMulDiv_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MID_fun.
mathVisitor.prototype.visitMID_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TRIMSTART_fun.
mathVisitor.prototype.visitTRIMSTART_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#RMB_fun.
mathVisitor.prototype.visitRMB_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REMOVESTART_fun.
mathVisitor.prototype.visitREMOVESTART_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SUBSTRING_fun.
mathVisitor.prototype.visitSUBSTRING_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISNUMBER_fun.
mathVisitor.prototype.visitISNUMBER_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#T_fun.
mathVisitor.prototype.visitT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DEC2HEX_fun.
mathVisitor.prototype.visitDEC2HEX_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CLEAN_fun.
mathVisitor.prototype.visitCLEAN_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LASTINDEXOF_fun.
mathVisitor.prototype.visitLASTINDEXOF_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LOWER_fun.
mathVisitor.prototype.visitLOWER_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BIN2OCT_fun.
mathVisitor.prototype.visitBIN2OCT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CHAR_fun.
mathVisitor.prototype.visitCHAR_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REGEX_fun.
mathVisitor.prototype.visitREGEX_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TEXTTOBASE64URL_fun.
mathVisitor.prototype.visitTEXTTOBASE64URL_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MD5_fun.
mathVisitor.prototype.visitMD5_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HMACSHA512_fun.
mathVisitor.prototype.visitHMACSHA512_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REPLACE_fun.
mathVisitor.prototype.visitREPLACE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LEFT_fun.
mathVisitor.prototype.visitLEFT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BASE64TOTEXT_fun.
mathVisitor.prototype.visitBASE64TOTEXT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISODD_fun.
mathVisitor.prototype.visitISODD_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISEVEN_fun.
mathVisitor.prototype.visitISEVEN_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DATEVALUE_fun.
mathVisitor.prototype.visitDATEVALUE_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#STARTSWITH_fun.
mathVisitor.prototype.visitSTARTSWITH_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ASC_fun.
mathVisitor.prototype.visitASC_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISNULLOREMPTY_fun.
mathVisitor.prototype.visitISNULLOREMPTY_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#AndOr_fun.
mathVisitor.prototype.visitAndOr_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISERROR_fun.
mathVisitor.prototype.visitISERROR_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BIN2DEC_fun.
mathVisitor.prototype.visitBIN2DEC_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#JIS_fun.
mathVisitor.prototype.visitJIS_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SHA1_fun.
mathVisitor.prototype.visitSHA1_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CRC32_fun.
mathVisitor.prototype.visitCRC32_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HMACMD5_fun.
mathVisitor.prototype.visitHMACMD5_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#EXACT_fun.
mathVisitor.prototype.visitEXACT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SECOND_fun.
mathVisitor.prototype.visitSECOND_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#OCT2HEX_fun.
mathVisitor.prototype.visitOCT2HEX_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TRIMEND_fun.
mathVisitor.prototype.visitTRIMEND_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REMOVEEND_fun.
mathVisitor.prototype.visitREMOVEEND_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TRIM_fun.
mathVisitor.prototype.visitTRIM_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SPLIT_fun.
mathVisitor.prototype.visitSPLIT_fun = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#Array_fun2.
mathVisitor.prototype.visitArray_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#Bracket_fun2.
mathVisitor.prototype.visitBracket_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#IF_fun2.
mathVisitor.prototype.visitIF_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#IFERROR_fun2.
mathVisitor.prototype.visitIFERROR_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISNUMBER_fun2.
mathVisitor.prototype.visitISNUMBER_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISTEXT_fun2.
mathVisitor.prototype.visitISTEXT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISERROR_fun2.
mathVisitor.prototype.visitISERROR_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISNONTEXT_fun2.
mathVisitor.prototype.visitISNONTEXT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISLOGICAL_fun2.
mathVisitor.prototype.visitISLOGICAL_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISEVEN_fun2.
mathVisitor.prototype.visitISEVEN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISODD_fun2.
mathVisitor.prototype.visitISODD_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#AND_fun2.
mathVisitor.prototype.visitAND_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#OR_fun2.
mathVisitor.prototype.visitOR_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#NOT_fun2.
mathVisitor.prototype.visitNOT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TRUE_fun2.
mathVisitor.prototype.visitTRUE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FALSE_fun2.
mathVisitor.prototype.visitFALSE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#E_fun2.
mathVisitor.prototype.visitE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#PI_fun2.
mathVisitor.prototype.visitPI_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DEC2BIN_fun2.
mathVisitor.prototype.visitDEC2BIN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DEC2HEX_fun2.
mathVisitor.prototype.visitDEC2HEX_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DEC2OCT_fun2.
mathVisitor.prototype.visitDEC2OCT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HEX2BIN_fun2.
mathVisitor.prototype.visitHEX2BIN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HEX2DEC_fun2.
mathVisitor.prototype.visitHEX2DEC_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HEX2OCT_fun2.
mathVisitor.prototype.visitHEX2OCT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#OCT2BIN_fun2.
mathVisitor.prototype.visitOCT2BIN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#OCT2DEC_fun2.
mathVisitor.prototype.visitOCT2DEC_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#OCT2HEX_fun2.
mathVisitor.prototype.visitOCT2HEX_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BIN2OCT_fun2.
mathVisitor.prototype.visitBIN2OCT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BIN2DEC_fun2.
mathVisitor.prototype.visitBIN2DEC_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BIN2HEX_fun2.
mathVisitor.prototype.visitBIN2HEX_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ABS_fun2.
mathVisitor.prototype.visitABS_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#QUOTIENT_fun2.
mathVisitor.prototype.visitQUOTIENT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MOD_fun2.
mathVisitor.prototype.visitMOD_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SIGN_fun2.
mathVisitor.prototype.visitSIGN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SQRT_fun2.
mathVisitor.prototype.visitSQRT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TRUNC_fun2.
mathVisitor.prototype.visitTRUNC_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#INT_fun2.
mathVisitor.prototype.visitINT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#GCD_fun2.
mathVisitor.prototype.visitGCD_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LCM_fun2.
mathVisitor.prototype.visitLCM_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#COMBIN_fun2.
mathVisitor.prototype.visitCOMBIN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#PERMUT_fun2.
mathVisitor.prototype.visitPERMUT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DEGREES_fun2.
mathVisitor.prototype.visitDEGREES_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#RADIANS_fun2.
mathVisitor.prototype.visitRADIANS_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#COS_fun2.
mathVisitor.prototype.visitCOS_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#COSH_fun2.
mathVisitor.prototype.visitCOSH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SIN_fun2.
mathVisitor.prototype.visitSIN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SINH_fun2.
mathVisitor.prototype.visitSINH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TAN_fun2.
mathVisitor.prototype.visitTAN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TANH_fun2.
mathVisitor.prototype.visitTANH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ACOS_fun2.
mathVisitor.prototype.visitACOS_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ACOSH_fun2.
mathVisitor.prototype.visitACOSH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ASIN_fun2.
mathVisitor.prototype.visitASIN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ASINH_fun2.
mathVisitor.prototype.visitASINH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ATAN_fun2.
mathVisitor.prototype.visitATAN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ATANH_fun2.
mathVisitor.prototype.visitATANH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ATAN2_fun2.
mathVisitor.prototype.visitATAN2_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ROUND_fun2.
mathVisitor.prototype.visitROUND_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ROUNDDOWN_fun2.
mathVisitor.prototype.visitROUNDDOWN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ROUNDUP_fun2.
mathVisitor.prototype.visitROUNDUP_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CEILING_fun2.
mathVisitor.prototype.visitCEILING_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FLOOR_fun2.
mathVisitor.prototype.visitFLOOR_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#EVEN_fun2.
mathVisitor.prototype.visitEVEN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ODD_fun2.
mathVisitor.prototype.visitODD_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MROUND_fun2.
mathVisitor.prototype.visitMROUND_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#RAND_fun2.
mathVisitor.prototype.visitRAND_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#RANDBETWEEN_fun2.
mathVisitor.prototype.visitRANDBETWEEN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FACT_fun2.
mathVisitor.prototype.visitFACT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FACTDOUBLE_fun2.
mathVisitor.prototype.visitFACTDOUBLE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#POWER_fun2.
mathVisitor.prototype.visitPOWER_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#EXP_fun2.
mathVisitor.prototype.visitEXP_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LN_fun2.
mathVisitor.prototype.visitLN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LOG_fun2.
mathVisitor.prototype.visitLOG_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LOG10_fun2.
mathVisitor.prototype.visitLOG10_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MULTINOMIAL_fun2.
mathVisitor.prototype.visitMULTINOMIAL_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#PRODUCT_fun2.
mathVisitor.prototype.visitPRODUCT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SQRTPI_fun2.
mathVisitor.prototype.visitSQRTPI_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SUMSQ_fun2.
mathVisitor.prototype.visitSUMSQ_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ASC_fun2.
mathVisitor.prototype.visitASC_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#JIS_fun2.
mathVisitor.prototype.visitJIS_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CHAR_fun2.
mathVisitor.prototype.visitCHAR_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CLEAN_fun2.
mathVisitor.prototype.visitCLEAN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CODE_fun2.
mathVisitor.prototype.visitCODE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CONCATENATE_fun2.
mathVisitor.prototype.visitCONCATENATE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#EXACT_fun2.
mathVisitor.prototype.visitEXACT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FIND_fun2.
mathVisitor.prototype.visitFIND_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FIXED_fun2.
mathVisitor.prototype.visitFIXED_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LEFT_fun2.
mathVisitor.prototype.visitLEFT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LEN_fun2.
mathVisitor.prototype.visitLEN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LOWER_fun2.
mathVisitor.prototype.visitLOWER_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MID_fun2.
mathVisitor.prototype.visitMID_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#PROPER_fun2.
mathVisitor.prototype.visitPROPER_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REPLACE_fun2.
mathVisitor.prototype.visitREPLACE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REPT_fun2.
mathVisitor.prototype.visitREPT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#RIGHT_fun2.
mathVisitor.prototype.visitRIGHT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#RMB_fun2.
mathVisitor.prototype.visitRMB_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SEARCH_fun2.
mathVisitor.prototype.visitSEARCH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SUBSTITUTE_fun2.
mathVisitor.prototype.visitSUBSTITUTE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#T_fun2.
mathVisitor.prototype.visitT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TEXT_fun2.
mathVisitor.prototype.visitTEXT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TRIM_fun2.
mathVisitor.prototype.visitTRIM_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#UPPER_fun2.
mathVisitor.prototype.visitUPPER_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#VALUE_fun2.
mathVisitor.prototype.visitVALUE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DATEVALUE_fun2.
mathVisitor.prototype.visitDATEVALUE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TIMEVALUE_fun2.
mathVisitor.prototype.visitTIMEVALUE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DATE_fun2.
mathVisitor.prototype.visitDATE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TIME_fun2.
mathVisitor.prototype.visitTIME_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#NOW_fun2.
mathVisitor.prototype.visitNOW_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TODAY_fun2.
mathVisitor.prototype.visitTODAY_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#YEAR_fun2.
mathVisitor.prototype.visitYEAR_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MONTH_fun2.
mathVisitor.prototype.visitMONTH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DAY_fun2.
mathVisitor.prototype.visitDAY_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HOUR_fun2.
mathVisitor.prototype.visitHOUR_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MINUTE_fun2.
mathVisitor.prototype.visitMINUTE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SECOND_fun2.
mathVisitor.prototype.visitSECOND_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#WEEKDAY_fun2.
mathVisitor.prototype.visitWEEKDAY_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DATEDIF_fun2.
mathVisitor.prototype.visitDATEDIF_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DAYS360_fun2.
mathVisitor.prototype.visitDAYS360_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#EDATE_fun2.
mathVisitor.prototype.visitEDATE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#EOMONTH_fun2.
mathVisitor.prototype.visitEOMONTH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#NETWORKDAYS_fun2.
mathVisitor.prototype.visitNETWORKDAYS_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#WORKDAY_fun2.
mathVisitor.prototype.visitWORKDAY_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#WEEKNUM_fun2.
mathVisitor.prototype.visitWEEKNUM_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MAX_fun2.
mathVisitor.prototype.visitMAX_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MEDIAN_fun2.
mathVisitor.prototype.visitMEDIAN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MIN_fun2.
mathVisitor.prototype.visitMIN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#QUARTILE_fun2.
mathVisitor.prototype.visitQUARTILE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MODE_fun2.
mathVisitor.prototype.visitMODE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LARGE_fun2.
mathVisitor.prototype.visitLARGE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SMALL_fun2.
mathVisitor.prototype.visitSMALL_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#PERCENTILE_fun2.
mathVisitor.prototype.visitPERCENTILE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#PERCENTRANK_fun2.
mathVisitor.prototype.visitPERCENTRANK_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#AVERAGE_fun2.
mathVisitor.prototype.visitAVERAGE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#AVERAGEIF_fun2.
mathVisitor.prototype.visitAVERAGEIF_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#GEOMEAN_fun2.
mathVisitor.prototype.visitGEOMEAN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HARMEAN_fun2.
mathVisitor.prototype.visitHARMEAN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#COUNT_fun2.
mathVisitor.prototype.visitCOUNT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#COUNTIF_fun2.
mathVisitor.prototype.visitCOUNTIF_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SUM_fun2.
mathVisitor.prototype.visitSUM_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SUMIF_fun2.
mathVisitor.prototype.visitSUMIF_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#AVEDEV_fun2.
mathVisitor.prototype.visitAVEDEV_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#STDEV_fun2.
mathVisitor.prototype.visitSTDEV_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#STDEVP_fun2.
mathVisitor.prototype.visitSTDEVP_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#DEVSQ_fun2.
mathVisitor.prototype.visitDEVSQ_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#VAR_fun2.
mathVisitor.prototype.visitVAR_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#VARP_fun2.
mathVisitor.prototype.visitVARP_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#NORMDIST_fun2.
mathVisitor.prototype.visitNORMDIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#NORMINV_fun2.
mathVisitor.prototype.visitNORMINV_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#NORMSDIST_fun2.
mathVisitor.prototype.visitNORMSDIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#NORMSINV_fun2.
mathVisitor.prototype.visitNORMSINV_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BETADIST_fun2.
mathVisitor.prototype.visitBETADIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BETAINV_fun2.
mathVisitor.prototype.visitBETAINV_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BINOMDIST_fun2.
mathVisitor.prototype.visitBINOMDIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#EXPONDIST_fun2.
mathVisitor.prototype.visitEXPONDIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FDIST_fun2.
mathVisitor.prototype.visitFDIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FINV_fun2.
mathVisitor.prototype.visitFINV_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FISHER_fun2.
mathVisitor.prototype.visitFISHER_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#FISHERINV_fun2.
mathVisitor.prototype.visitFISHERINV_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#GAMMADIST_fun2.
mathVisitor.prototype.visitGAMMADIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#GAMMAINV_fun2.
mathVisitor.prototype.visitGAMMAINV_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#GAMMALN_fun2.
mathVisitor.prototype.visitGAMMALN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HYPGEOMDIST_fun2.
mathVisitor.prototype.visitHYPGEOMDIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LOGINV_fun2.
mathVisitor.prototype.visitLOGINV_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LOGNORMDIST_fun2.
mathVisitor.prototype.visitLOGNORMDIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#NEGBINOMDIST_fun2.
mathVisitor.prototype.visitNEGBINOMDIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#POISSON_fun2.
mathVisitor.prototype.visitPOISSON_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TDIST_fun2.
mathVisitor.prototype.visitTDIST_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TINV_fun2.
mathVisitor.prototype.visitTINV_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#WEIBULL_fun2.
mathVisitor.prototype.visitWEIBULL_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#URLENCODE_fun2.
mathVisitor.prototype.visitURLENCODE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#URLDECODE_fun2.
mathVisitor.prototype.visitURLDECODE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HTMLENCODE_fun2.
mathVisitor.prototype.visitHTMLENCODE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HTMLDECODE_fun2.
mathVisitor.prototype.visitHTMLDECODE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BASE64TOTEXT_fun2.
mathVisitor.prototype.visitBASE64TOTEXT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#BASE64URLTOTEXT_fun2.
mathVisitor.prototype.visitBASE64URLTOTEXT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TEXTTOBASE64_fun2.
mathVisitor.prototype.visitTEXTTOBASE64_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TEXTTOBASE64URL_fun2.
mathVisitor.prototype.visitTEXTTOBASE64URL_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REGEX_fun2.
mathVisitor.prototype.visitREGEX_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REGEXREPALCE_fun2.
mathVisitor.prototype.visitREGEXREPALCE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISREGEX_fun2.
mathVisitor.prototype.visitISREGEX_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#GUID_fun2.
mathVisitor.prototype.visitGUID_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#MD5_fun2.
mathVisitor.prototype.visitMD5_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SHA1_fun2.
mathVisitor.prototype.visitSHA1_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SHA256_fun2.
mathVisitor.prototype.visitSHA256_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SHA512_fun2.
mathVisitor.prototype.visitSHA512_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CRC8_fun2.
mathVisitor.prototype.visitCRC8_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CRC16_fun2.
mathVisitor.prototype.visitCRC16_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#CRC32_fun2.
mathVisitor.prototype.visitCRC32_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HMACMD5_fun2.
mathVisitor.prototype.visitHMACMD5_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HMACSHA1_fun2.
mathVisitor.prototype.visitHMACSHA1_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HMACSHA256_fun2.
mathVisitor.prototype.visitHMACSHA256_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#HMACSHA512_fun2.
mathVisitor.prototype.visitHMACSHA512_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TRIMSTART_fun2.
mathVisitor.prototype.visitTRIMSTART_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#TRIMEND_fun2.
mathVisitor.prototype.visitTRIMEND_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#INDEXOF_fun2.
mathVisitor.prototype.visitINDEXOF_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#LASTINDEXOF_fun2.
mathVisitor.prototype.visitLASTINDEXOF_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SPLIT_fun2.
mathVisitor.prototype.visitSPLIT_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#JOIN_fun2.
mathVisitor.prototype.visitJOIN_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#SUBSTRING_fun2.
mathVisitor.prototype.visitSUBSTRING_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#STARTSWITH_fun2.
mathVisitor.prototype.visitSTARTSWITH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ENDSWITH_fun2.
mathVisitor.prototype.visitENDSWITH_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISNULLOREMPTY_fun2.
mathVisitor.prototype.visitISNULLOREMPTY_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#ISNULLORWHITESPACE_fun2.
mathVisitor.prototype.visitISNULLORWHITESPACE_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REMOVESTART_fun2.
mathVisitor.prototype.visitREMOVESTART_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#REMOVEEND_fun2.
mathVisitor.prototype.visitREMOVEEND_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#JSON_fun2.
mathVisitor.prototype.visitJSON_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#PARAMETER_fun2.
mathVisitor.prototype.visitPARAMETER_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#NUM_fun2.
mathVisitor.prototype.visitNUM_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#STRING_fun2.
mathVisitor.prototype.visitSTRING_fun2 = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#parameter.
mathVisitor.prototype.visitParameter = function(ctx) {
  return this.visitChildren(ctx);
};


// Visit a parse tree produced by mathParser#parameter2.
mathVisitor.prototype.visitParameter2 = function(ctx) {
  return this.visitChildren(ctx);
};



exports.mathVisitor = mathVisitor;