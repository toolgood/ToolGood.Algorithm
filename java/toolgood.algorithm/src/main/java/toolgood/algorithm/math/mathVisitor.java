// Generated from math.g4 by ANTLR 4.8
import org.antlr.v4.runtime.tree.ParseTreeVisitor;

/**
 * This interface defines a complete generic visitor for a parse tree produced
 * by {@link mathParser}.
 *
 * @param <T> The return type of the visit operation. Use {@link Void} for
 * operations with no return type.
 */
public interface mathVisitor<T> extends ParseTreeVisitor<T> {
	/**
	 * Visit a parse tree produced by {@link mathParser#prog}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitProg(mathParser.ProgContext ctx);
	/**
	 * Visit a parse tree produced by the {@code URLDECODE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitURLDECODE_fun(mathParser.URLDECODE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code REGEXREPALCE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code AddSub_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAddSub_fun(mathParser.AddSub_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULLORERROR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CRC16_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCRC16_fun(mathParser.CRC16_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code RIGHT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRIGHT_fun(mathParser.RIGHT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code VALUE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVALUE_fun(mathParser.VALUE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DAY_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDAY_fun(mathParser.DAY_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACSHA256_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACSHA256_fun(mathParser.HMACSHA256_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Judge_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJudge_fun(mathParser.Judge_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code OCT2BIN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOCT2BIN_fun(mathParser.OCT2BIN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MINUTE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMINUTE_fun(mathParser.MINUTE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code INDEXOF_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitINDEXOF_fun(mathParser.INDEXOF_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code UPPER_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitUPPER_fun(mathParser.UPPER_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HTMLDECODE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHTMLDECODE_fun(mathParser.HTMLDECODE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISREGEX_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISREGEX_fun(mathParser.ISREGEX_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code VLOOKUP_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVLOOKUP_fun(mathParser.VLOOKUP_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DEC2BIN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDEC2BIN_fun(mathParser.DEC2BIN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LOOKUP_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOOKUP_fun(mathParser.LOOKUP_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HEX2DEC_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHEX2DEC_fun(mathParser.HEX2DEC_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DiyFunction_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDiyFunction_fun(mathParser.DiyFunction_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SHA256_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSHA256_fun(mathParser.SHA256_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TEXTTOBASE64_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTEXTTOBASE64_fun(mathParser.TEXTTOBASE64_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MID_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMID_fun(mathParser.MID_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TRIMSTART_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRIMSTART_fun(mathParser.TRIMSTART_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code RMB_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRMB_fun(mathParser.RMB_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNUMBER_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNUMBER_fun(mathParser.ISNUMBER_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DEC2HEX_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDEC2HEX_fun(mathParser.DEC2HEX_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CLEAN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCLEAN_fun(mathParser.CLEAN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LASTINDEXOF_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LOWER_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOWER_fun(mathParser.LOWER_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CHAR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCHAR_fun(mathParser.CHAR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code REGEX_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREGEX_fun(mathParser.REGEX_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TEXTTOBASE64URL_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTEXTTOBASE64URL_fun(mathParser.TEXTTOBASE64URL_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MD5_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMD5_fun(mathParser.MD5_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code REPLACE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREPLACE_fun(mathParser.REPLACE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LEFT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLEFT_fun(mathParser.LEFT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISODD_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISODD_fun(mathParser.ISODD_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISEVEN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISEVEN_fun(mathParser.ISEVEN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ASC_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitASC_fun(mathParser.ASC_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISERROR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISERROR_fun(mathParser.ISERROR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BIN2DEC_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBIN2DEC_fun(mathParser.BIN2DEC_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code JIS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJIS_fun(mathParser.JIS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CRC32_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCRC32_fun(mathParser.CRC32_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACMD5_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACMD5_fun(mathParser.HMACMD5_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code EXACT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEXACT_fun(mathParser.EXACT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SECOND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSECOND_fun(mathParser.SECOND_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code OCT2HEX_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOCT2HEX_fun(mathParser.OCT2HEX_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TRIMEND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRIMEND_fun(mathParser.TRIMEND_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TRIM_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRIM_fun(mathParser.TRIM_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CRC8_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCRC8_fun(mathParser.CRC8_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISLOGICAL_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISLOGICAL_fun(mathParser.ISLOGICAL_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code INT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitINT_fun(mathParser.INT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HEX2OCT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHEX2OCT_fun(mathParser.HEX2OCT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TEXT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTEXT_fun(mathParser.TEXT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code YEAR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitYEAR_fun(mathParser.YEAR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BIN2HEX_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBIN2HEX_fun(mathParser.BIN2HEX_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CONCATENATE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCONCATENATE_fun(mathParser.CONCATENATE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MONTH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMONTH_fun(mathParser.MONTH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HTMLENCODE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHTMLENCODE_fun(mathParser.HTMLENCODE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BASE64URLTOTEXT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBASE64URLTOTEXT_fun(mathParser.BASE64URLTOTEXT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code URLENCODE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitURLENCODE_fun(mathParser.URLENCODE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACSHA1_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACSHA1_fun(mathParser.HMACSHA1_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ENDSWITH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitENDSWITH_fun(mathParser.ENDSWITH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISTEXT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISTEXT_fun(mathParser.ISTEXT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code expr2_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitExpr2_fun(mathParser.Expr2_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TIMEVALUE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code JSON_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJSON_fun(mathParser.JSON_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DEC2OCT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDEC2OCT_fun(mathParser.DEC2OCT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code PROPER_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPROPER_fun(mathParser.PROPER_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code GetJsonValue_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGetJsonValue_fun(mathParser.GetJsonValue_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HEX2BIN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHEX2BIN_fun(mathParser.HEX2BIN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HOUR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHOUR_fun(mathParser.HOUR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LEN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLEN_fun(mathParser.LEN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SHA512_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSHA512_fun(mathParser.SHA512_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULLORWHITESPACE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNONTEXT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNONTEXT_fun(mathParser.ISNONTEXT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code JOIN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJOIN_fun(mathParser.JOIN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FIND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFIND_fun(mathParser.FIND_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SUBSTITUTE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUBSTITUTE_fun(mathParser.SUBSTITUTE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code REPT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREPT_fun(mathParser.REPT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULL_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULL_fun(mathParser.ISNULL_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code OCT2DEC_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOCT2DEC_fun(mathParser.OCT2DEC_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SEARCH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSEARCH_fun(mathParser.SEARCH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CODE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCODE_fun(mathParser.CODE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MulDiv_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMulDiv_fun(mathParser.MulDiv_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code REMOVESTART_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREMOVESTART_fun(mathParser.REMOVESTART_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SUBSTRING_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUBSTRING_fun(mathParser.SUBSTRING_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code T_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitT_fun(mathParser.T_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BIN2OCT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBIN2OCT_fun(mathParser.BIN2OCT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACSHA512_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACSHA512_fun(mathParser.HMACSHA512_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BASE64TOTEXT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DATEVALUE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDATEVALUE_fun(mathParser.DATEVALUE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code STARTSWITH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSTARTSWITH_fun(mathParser.STARTSWITH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULLOREMPTY_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code AndOr_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAndOr_fun(mathParser.AndOr_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SHA1_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSHA1_fun(mathParser.SHA1_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code REMOVEEND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREMOVEEND_fun(mathParser.REMOVEEND_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SPLIT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSPLIT_fun(mathParser.SPLIT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Array_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitArray_fun2(mathParser.Array_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code Bracket_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBracket_fun2(mathParser.Bracket_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code IF_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIF_fun2(mathParser.IF_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNUMBER_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNUMBER_fun2(mathParser.ISNUMBER_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISTEXT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISTEXT_fun2(mathParser.ISTEXT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISERROR_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISERROR_fun2(mathParser.ISERROR_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNONTEXT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNONTEXT_fun2(mathParser.ISNONTEXT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISLOGICAL_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISLOGICAL_fun2(mathParser.ISLOGICAL_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISEVEN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISEVEN_fun2(mathParser.ISEVEN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISODD_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISODD_fun2(mathParser.ISODD_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code IFERROR_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIFERROR_fun2(mathParser.IFERROR_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULL_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULL_fun2(mathParser.ISNULL_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULLORERROR_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULLORERROR_fun2(mathParser.ISNULLORERROR_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code AND_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAND_fun2(mathParser.AND_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code OR_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOR_fun2(mathParser.OR_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code NOT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNOT_fun2(mathParser.NOT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TRUE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRUE_fun2(mathParser.TRUE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code FALSE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFALSE_fun2(mathParser.FALSE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code E_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitE_fun2(mathParser.E_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code PI_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPI_fun2(mathParser.PI_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DEC2BIN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDEC2BIN_fun2(mathParser.DEC2BIN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DEC2HEX_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDEC2HEX_fun2(mathParser.DEC2HEX_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DEC2OCT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDEC2OCT_fun2(mathParser.DEC2OCT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HEX2BIN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHEX2BIN_fun2(mathParser.HEX2BIN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HEX2DEC_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHEX2DEC_fun2(mathParser.HEX2DEC_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HEX2OCT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHEX2OCT_fun2(mathParser.HEX2OCT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code OCT2BIN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOCT2BIN_fun2(mathParser.OCT2BIN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code OCT2DEC_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOCT2DEC_fun2(mathParser.OCT2DEC_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code OCT2HEX_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOCT2HEX_fun2(mathParser.OCT2HEX_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code BIN2OCT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBIN2OCT_fun2(mathParser.BIN2OCT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code BIN2DEC_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBIN2DEC_fun2(mathParser.BIN2DEC_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code BIN2HEX_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBIN2HEX_fun2(mathParser.BIN2HEX_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ABS_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitABS_fun2(mathParser.ABS_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code QUOTIENT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitQUOTIENT_fun2(mathParser.QUOTIENT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MOD_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMOD_fun2(mathParser.MOD_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SIGN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSIGN_fun2(mathParser.SIGN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SQRT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSQRT_fun2(mathParser.SQRT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TRUNC_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRUNC_fun2(mathParser.TRUNC_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code INT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitINT_fun2(mathParser.INT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code GCD_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGCD_fun2(mathParser.GCD_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LCM_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLCM_fun2(mathParser.LCM_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code COMBIN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCOMBIN_fun2(mathParser.COMBIN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code PERMUT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPERMUT_fun2(mathParser.PERMUT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DEGREES_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDEGREES_fun2(mathParser.DEGREES_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code RADIANS_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRADIANS_fun2(mathParser.RADIANS_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code COS_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCOS_fun2(mathParser.COS_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code COSH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCOSH_fun2(mathParser.COSH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SIN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSIN_fun2(mathParser.SIN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SINH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSINH_fun2(mathParser.SINH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TAN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTAN_fun2(mathParser.TAN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TANH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTANH_fun2(mathParser.TANH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ACOS_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitACOS_fun2(mathParser.ACOS_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ACOSH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitACOSH_fun2(mathParser.ACOSH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ASIN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitASIN_fun2(mathParser.ASIN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ASINH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitASINH_fun2(mathParser.ASINH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ATAN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitATAN_fun2(mathParser.ATAN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ATANH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitATANH_fun2(mathParser.ATANH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ATAN2_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitATAN2_fun2(mathParser.ATAN2_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ROUND_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitROUND_fun2(mathParser.ROUND_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ROUNDDOWN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitROUNDDOWN_fun2(mathParser.ROUNDDOWN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ROUNDUP_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitROUNDUP_fun2(mathParser.ROUNDUP_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code CEILING_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCEILING_fun2(mathParser.CEILING_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code FLOOR_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFLOOR_fun2(mathParser.FLOOR_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code EVEN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEVEN_fun2(mathParser.EVEN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ODD_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitODD_fun2(mathParser.ODD_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MROUND_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMROUND_fun2(mathParser.MROUND_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code RAND_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRAND_fun2(mathParser.RAND_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code RANDBETWEEN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRANDBETWEEN_fun2(mathParser.RANDBETWEEN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code FACT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFACT_fun2(mathParser.FACT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code FACTDOUBLE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFACTDOUBLE_fun2(mathParser.FACTDOUBLE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code POWER_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPOWER_fun2(mathParser.POWER_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code EXP_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEXP_fun2(mathParser.EXP_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLN_fun2(mathParser.LN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LOG_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOG_fun2(mathParser.LOG_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LOG10_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOG10_fun2(mathParser.LOG10_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MULTINOMIAL_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMULTINOMIAL_fun2(mathParser.MULTINOMIAL_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code PRODUCT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPRODUCT_fun2(mathParser.PRODUCT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SQRTPI_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSQRTPI_fun2(mathParser.SQRTPI_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SUMSQ_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUMSQ_fun2(mathParser.SUMSQ_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ASC_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitASC_fun2(mathParser.ASC_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code JIS_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJIS_fun2(mathParser.JIS_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code CHAR_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCHAR_fun2(mathParser.CHAR_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code CLEAN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCLEAN_fun2(mathParser.CLEAN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code CODE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCODE_fun2(mathParser.CODE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code CONCATENATE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCONCATENATE_fun2(mathParser.CONCATENATE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code EXACT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEXACT_fun2(mathParser.EXACT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code FIND_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFIND_fun2(mathParser.FIND_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code FIXED_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFIXED_fun2(mathParser.FIXED_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LEFT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLEFT_fun2(mathParser.LEFT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LEN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLEN_fun2(mathParser.LEN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LOWER_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOWER_fun2(mathParser.LOWER_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MID_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMID_fun2(mathParser.MID_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code PROPER_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPROPER_fun2(mathParser.PROPER_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code REPLACE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREPLACE_fun2(mathParser.REPLACE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code REPT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREPT_fun2(mathParser.REPT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code RIGHT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRIGHT_fun2(mathParser.RIGHT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code RMB_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRMB_fun2(mathParser.RMB_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SEARCH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSEARCH_fun2(mathParser.SEARCH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SUBSTITUTE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUBSTITUTE_fun2(mathParser.SUBSTITUTE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code T_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitT_fun2(mathParser.T_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TEXT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTEXT_fun2(mathParser.TEXT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TRIM_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRIM_fun2(mathParser.TRIM_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code UPPER_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitUPPER_fun2(mathParser.UPPER_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code VALUE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVALUE_fun2(mathParser.VALUE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DATEVALUE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDATEVALUE_fun2(mathParser.DATEVALUE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TIMEVALUE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTIMEVALUE_fun2(mathParser.TIMEVALUE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DATE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDATE_fun2(mathParser.DATE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TIME_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTIME_fun2(mathParser.TIME_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code NOW_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNOW_fun2(mathParser.NOW_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TODAY_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTODAY_fun2(mathParser.TODAY_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code YEAR_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitYEAR_fun2(mathParser.YEAR_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MONTH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMONTH_fun2(mathParser.MONTH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DAY_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDAY_fun2(mathParser.DAY_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HOUR_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHOUR_fun2(mathParser.HOUR_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MINUTE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMINUTE_fun2(mathParser.MINUTE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SECOND_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSECOND_fun2(mathParser.SECOND_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code WEEKDAY_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWEEKDAY_fun2(mathParser.WEEKDAY_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DATEDIF_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDATEDIF_fun2(mathParser.DATEDIF_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DAYS360_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDAYS360_fun2(mathParser.DAYS360_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code EDATE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEDATE_fun2(mathParser.EDATE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code EOMONTH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEOMONTH_fun2(mathParser.EOMONTH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code NETWORKDAYS_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNETWORKDAYS_fun2(mathParser.NETWORKDAYS_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code WORKDAY_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWORKDAY_fun2(mathParser.WORKDAY_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code WEEKNUM_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWEEKNUM_fun2(mathParser.WEEKNUM_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MAX_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMAX_fun2(mathParser.MAX_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MEDIAN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMEDIAN_fun2(mathParser.MEDIAN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MIN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMIN_fun2(mathParser.MIN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code QUARTILE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitQUARTILE_fun2(mathParser.QUARTILE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MODE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMODE_fun2(mathParser.MODE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LARGE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLARGE_fun2(mathParser.LARGE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SMALL_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSMALL_fun2(mathParser.SMALL_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code PERCENTILE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPERCENTILE_fun2(mathParser.PERCENTILE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code PERCENTRANK_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPERCENTRANK_fun2(mathParser.PERCENTRANK_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code AVERAGE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAVERAGE_fun2(mathParser.AVERAGE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code AVERAGEIF_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAVERAGEIF_fun2(mathParser.AVERAGEIF_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code GEOMEAN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGEOMEAN_fun2(mathParser.GEOMEAN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HARMEAN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHARMEAN_fun2(mathParser.HARMEAN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code COUNT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCOUNT_fun2(mathParser.COUNT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code COUNTIF_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCOUNTIF_fun2(mathParser.COUNTIF_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SUM_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUM_fun2(mathParser.SUM_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SUMIF_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUMIF_fun2(mathParser.SUMIF_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code AVEDEV_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAVEDEV_fun2(mathParser.AVEDEV_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code STDEV_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSTDEV_fun2(mathParser.STDEV_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code STDEVP_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSTDEVP_fun2(mathParser.STDEVP_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DEVSQ_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDEVSQ_fun2(mathParser.DEVSQ_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code VAR_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVAR_fun2(mathParser.VAR_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code VARP_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVARP_fun2(mathParser.VARP_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code NORMDIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNORMDIST_fun2(mathParser.NORMDIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code NORMINV_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNORMINV_fun2(mathParser.NORMINV_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code NORMSDIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNORMSDIST_fun2(mathParser.NORMSDIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code NORMSINV_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNORMSINV_fun2(mathParser.NORMSINV_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code BETADIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBETADIST_fun2(mathParser.BETADIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code BETAINV_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBETAINV_fun2(mathParser.BETAINV_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code BINOMDIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBINOMDIST_fun2(mathParser.BINOMDIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code EXPONDIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEXPONDIST_fun2(mathParser.EXPONDIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code FDIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFDIST_fun2(mathParser.FDIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code FINV_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFINV_fun2(mathParser.FINV_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code FISHER_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFISHER_fun2(mathParser.FISHER_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code FISHERINV_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFISHERINV_fun2(mathParser.FISHERINV_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code GAMMADIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGAMMADIST_fun2(mathParser.GAMMADIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code GAMMAINV_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGAMMAINV_fun2(mathParser.GAMMAINV_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code GAMMALN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGAMMALN_fun2(mathParser.GAMMALN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HYPGEOMDIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHYPGEOMDIST_fun2(mathParser.HYPGEOMDIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LOGINV_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOGINV_fun2(mathParser.LOGINV_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LOGNORMDIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOGNORMDIST_fun2(mathParser.LOGNORMDIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code NEGBINOMDIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNEGBINOMDIST_fun2(mathParser.NEGBINOMDIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code POISSON_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPOISSON_fun2(mathParser.POISSON_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TDIST_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTDIST_fun2(mathParser.TDIST_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TINV_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTINV_fun2(mathParser.TINV_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code WEIBULL_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWEIBULL_fun2(mathParser.WEIBULL_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code URLENCODE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitURLENCODE_fun2(mathParser.URLENCODE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code URLDECODE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitURLDECODE_fun2(mathParser.URLDECODE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HTMLENCODE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHTMLENCODE_fun2(mathParser.HTMLENCODE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HTMLDECODE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHTMLDECODE_fun2(mathParser.HTMLDECODE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code BASE64TOTEXT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBASE64TOTEXT_fun2(mathParser.BASE64TOTEXT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code BASE64URLTOTEXT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBASE64URLTOTEXT_fun2(mathParser.BASE64URLTOTEXT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TEXTTOBASE64_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTEXTTOBASE64_fun2(mathParser.TEXTTOBASE64_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TEXTTOBASE64URL_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTEXTTOBASE64URL_fun2(mathParser.TEXTTOBASE64URL_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code REGEX_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREGEX_fun2(mathParser.REGEX_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code REGEXREPALCE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREGEXREPALCE_fun2(mathParser.REGEXREPALCE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISREGEX_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISREGEX_fun2(mathParser.ISREGEX_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code GUID_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGUID_fun2(mathParser.GUID_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code MD5_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMD5_fun2(mathParser.MD5_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SHA1_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSHA1_fun2(mathParser.SHA1_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SHA256_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSHA256_fun2(mathParser.SHA256_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SHA512_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSHA512_fun2(mathParser.SHA512_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code CRC8_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCRC8_fun2(mathParser.CRC8_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code CRC16_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCRC16_fun2(mathParser.CRC16_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code CRC32_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCRC32_fun2(mathParser.CRC32_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACMD5_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACMD5_fun2(mathParser.HMACMD5_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACSHA1_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACSHA1_fun2(mathParser.HMACSHA1_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACSHA256_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACSHA256_fun2(mathParser.HMACSHA256_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACSHA512_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACSHA512_fun2(mathParser.HMACSHA512_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TRIMSTART_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRIMSTART_fun2(mathParser.TRIMSTART_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code TRIMEND_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRIMEND_fun2(mathParser.TRIMEND_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code INDEXOF_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitINDEXOF_fun2(mathParser.INDEXOF_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LASTINDEXOF_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLASTINDEXOF_fun2(mathParser.LASTINDEXOF_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SPLIT_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSPLIT_fun2(mathParser.SPLIT_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code JOIN_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJOIN_fun2(mathParser.JOIN_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code SUBSTRING_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUBSTRING_fun2(mathParser.SUBSTRING_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code STARTSWITH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSTARTSWITH_fun2(mathParser.STARTSWITH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ENDSWITH_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitENDSWITH_fun2(mathParser.ENDSWITH_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULLOREMPTY_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULLOREMPTY_fun2(mathParser.ISNULLOREMPTY_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULLORWHITESPACE_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULLORWHITESPACE_fun2(mathParser.ISNULLORWHITESPACE_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code REMOVESTART_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREMOVESTART_fun2(mathParser.REMOVESTART_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code REMOVEEND_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREMOVEEND_fun2(mathParser.REMOVEEND_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code JSON_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJSON_fun2(mathParser.JSON_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code VLOOKUP_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVLOOKUP_fun2(mathParser.VLOOKUP_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code LOOKUP_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOOKUP_fun2(mathParser.LOOKUP_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code DiyFunction_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDiyFunction_fun2(mathParser.DiyFunction_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code PARAMETER_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPARAMETER_fun2(mathParser.PARAMETER_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code NUM_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNUM_fun2(mathParser.NUM_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code STRING_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSTRING_fun2(mathParser.STRING_fun2Context ctx);
	/**
	 * Visit a parse tree produced by the {@code NULL_fun2}
	 * labeled alternative in {@link mathParser#expr2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNULL_fun2(mathParser.NULL_fun2Context ctx);
	/**
	 * Visit a parse tree produced by {@link mathParser#parameter}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParameter(mathParser.ParameterContext ctx);
	/**
	 * Visit a parse tree produced by {@link mathParser#parameter2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParameter2(mathParser.Parameter2Context ctx);
}