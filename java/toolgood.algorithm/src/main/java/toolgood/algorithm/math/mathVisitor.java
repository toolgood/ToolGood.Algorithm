package toolgood.algorithm.math;// Generated from math.g4 by ANTLR 4.13.0
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
	 * Visit a parse tree produced by the {@code CEILING_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCEILING_fun(mathParser.CEILING_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FACT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFACT_fun(mathParser.FACT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code REGEXREPALCE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREGEXREPALCE_fun(mathParser.REGEXREPALCE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HASVALUE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHASVALUE_fun(mathParser.HASVALUE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code AddSub_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAddSub_fun(mathParser.AddSub_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code AVERAGEIF_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAVERAGEIF_fun(mathParser.AVERAGEIF_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code PARAM_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPARAM_fun(mathParser.PARAM_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULLORERROR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULLORERROR_fun(mathParser.ISNULLORERROR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code RIGHT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRIGHT_fun(mathParser.RIGHT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code OCT2BIN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOCT2BIN_fun(mathParser.OCT2BIN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code QUARTILE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitQUARTILE_fun(mathParser.QUARTILE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FINV_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFINV_fun(mathParser.FINV_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NOT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNOT_fun(mathParser.NOT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DAYS360_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDAYS360_fun(mathParser.DAYS360_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code WEEKNUM_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWEEKNUM_fun(mathParser.WEEKNUM_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code POISSON_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPOISSON_fun(mathParser.POISSON_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISREGEX_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISREGEX_fun(mathParser.ISREGEX_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code PERCENTILE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPERCENTILE_fun(mathParser.PERCENTILE_funContext ctx);
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
	 * Visit a parse tree produced by the {@code HAS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHAS_fun(mathParser.HAS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HYPGEOMDIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHYPGEOMDIST_fun(mathParser.HYPGEOMDIST_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code PERMUT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPERMUT_fun(mathParser.PERMUT_funContext ctx);
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
	 * Visit a parse tree produced by the {@code LOWER_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOWER_fun(mathParser.LOWER_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code OR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOR_fun(mathParser.OR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ADDMONTHS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitADDMONTHS_fun(mathParser.ADDMONTHS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NORMSINV_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNORMSINV_fun(mathParser.NORMSINV_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LEFT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLEFT_fun(mathParser.LEFT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISEVEN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISEVEN_fun(mathParser.ISEVEN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LOGINV_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOGINV_fun(mathParser.LOGINV_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code WORKDAY_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWORKDAY_fun(mathParser.WORKDAY_funContext ctx);
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
	 * Visit a parse tree produced by the {@code LCM_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLCM_fun(mathParser.LCM_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HARMEAN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHARMEAN_fun(mathParser.HARMEAN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NORMINV_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNORMINV_fun(mathParser.NORMINV_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code GAMMAINV_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGAMMAINV_fun(mathParser.GAMMAINV_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SQRT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSQRT_fun(mathParser.SQRT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DEGREES_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDEGREES_fun(mathParser.DEGREES_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MROUND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMROUND_fun(mathParser.MROUND_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DATEDIF_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDATEDIF_fun(mathParser.DATEDIF_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TRIMEND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRIMEND_fun(mathParser.TRIMEND_funContext ctx);
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
	 * Visit a parse tree produced by the {@code SUMIF_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUMIF_fun(mathParser.SUMIF_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HEX2OCT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHEX2OCT_fun(mathParser.HEX2OCT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code PI_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPI_fun(mathParser.PI_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code YEAR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitYEAR_fun(mathParser.YEAR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SQRTPI_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSQRTPI_fun(mathParser.SQRTPI_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CONCATENATE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCONCATENATE_fun(mathParser.CONCATENATE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code COUNT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCOUNT_fun(mathParser.COUNT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FALSE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFALSE_fun(mathParser.FALSE_funContext ctx);
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
	 * Visit a parse tree produced by the {@code LOG10_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOG10_fun(mathParser.LOG10_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISTEXT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISTEXT_fun(mathParser.ISTEXT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NEGBINOMDIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNEGBINOMDIST_fun(mathParser.NEGBINOMDIST_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NETWORKDAYS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNETWORKDAYS_fun(mathParser.NETWORKDAYS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FACTDOUBLE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFACTDOUBLE_fun(mathParser.FACTDOUBLE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TIMEVALUE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTIMEVALUE_fun(mathParser.TIMEVALUE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code AVEDEV_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAVEDEV_fun(mathParser.AVEDEV_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code GUID_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGUID_fun(mathParser.GUID_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code JSON_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJSON_fun(mathParser.JSON_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FIXED_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFIXED_fun(mathParser.FIXED_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code GetJsonValue_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGetJsonValue_fun(mathParser.GetJsonValue_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TINV_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTINV_fun(mathParser.TINV_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code EDATE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEDATE_fun(mathParser.EDATE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code GEOMEAN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGEOMEAN_fun(mathParser.GEOMEAN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code VAR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVAR_fun(mathParser.VAR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SIGN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSIGN_fun(mathParser.SIGN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code EOMONTH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEOMONTH_fun(mathParser.EOMONTH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FLOOR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFLOOR_fun(mathParser.FLOOR_funContext ctx);
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
	 * Visit a parse tree produced by the {@code ACOS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitACOS_fun(mathParser.ACOS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULLORWHITESPACE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULLORWHITESPACE_fun(mathParser.ISNULLORWHITESPACE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NUM_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNUM_fun(mathParser.NUM_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code COSH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCOSH_fun(mathParser.COSH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code QUOTIENT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitQUOTIENT_fun(mathParser.QUOTIENT_funContext ctx);
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
	 * Visit a parse tree produced by the {@code ROUNDUP_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitROUNDUP_fun(mathParser.ROUNDUP_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code COMBIN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCOMBIN_fun(mathParser.COMBIN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CODE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCODE_fun(mathParser.CODE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ASINH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitASINH_fun(mathParser.ASINH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SIN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSIN_fun(mathParser.SIN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SUBSTRING_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUBSTRING_fun(mathParser.SUBSTRING_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code RANDBETWEEN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRANDBETWEEN_fun(mathParser.RANDBETWEEN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code AVERAGE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAVERAGE_fun(mathParser.AVERAGE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LOG_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOG_fun(mathParser.LOG_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACSHA512_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACSHA512_fun(mathParser.HMACSHA512_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code AndOr_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAndOr_fun(mathParser.AndOr_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code STDEVP_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSTDEVP_fun(mathParser.STDEVP_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ADDYEARS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitADDYEARS_fun(mathParser.ADDYEARS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ADDSECONDS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitADDSECONDS_fun(mathParser.ADDSECONDS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Array_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitArray_fun(mathParser.Array_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ROUND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitROUND_fun(mathParser.ROUND_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code EXP_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEXP_fun(mathParser.EXP_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code COUNTIF_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCOUNTIF_fun(mathParser.COUNTIF_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code VARP_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVARP_fun(mathParser.VARP_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code REMOVEEND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitREMOVEEND_fun(mathParser.REMOVEEND_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DATE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDATE_fun(mathParser.DATE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code PARAMETER_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPARAMETER_fun(mathParser.PARAMETER_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SPLIT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSPLIT_fun(mathParser.SPLIT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code URLDECODE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitURLDECODE_fun(mathParser.URLDECODE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LARGE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLARGE_fun(mathParser.LARGE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TIMESTAMP_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTIMESTAMP_fun(mathParser.TIMESTAMP_funContext ctx);
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
	 * Visit a parse tree produced by the {@code WEIBULL_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWEIBULL_fun(mathParser.WEIBULL_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACSHA256_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACSHA256_fun(mathParser.HMACSHA256_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BINOMDIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBINOMDIST_fun(mathParser.BINOMDIST_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Judge_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJudge_fun(mathParser.Judge_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code DEVSQ_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDEVSQ_fun(mathParser.DEVSQ_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MODE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMODE_fun(mathParser.MODE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BETAINV_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBETAINV_fun(mathParser.BETAINV_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MAX_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMAX_fun(mathParser.MAX_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MINUTE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMINUTE_fun(mathParser.MINUTE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TAN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTAN_fun(mathParser.TAN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code IFERROR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIFERROR_fun(mathParser.IFERROR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FDIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFDIST_fun(mathParser.FDIST_funContext ctx);
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
	 * Visit a parse tree produced by the {@code EXPONDIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEXPONDIST_fun(mathParser.EXPONDIST_funContext ctx);
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
	 * Visit a parse tree produced by the {@code SMALL_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSMALL_fun(mathParser.SMALL_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ODD_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitODD_fun(mathParser.ODD_funContext ctx);
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
	 * Visit a parse tree produced by the {@code PERCENTRANK_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPERCENTRANK_fun(mathParser.PERCENTRANK_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code STDEV_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSTDEV_fun(mathParser.STDEV_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NORMSDIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNORMSDIST_fun(mathParser.NORMSDIST_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNUMBER_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNUMBER_fun(mathParser.ISNUMBER_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LASTINDEXOF_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLASTINDEXOF_fun(mathParser.LASTINDEXOF_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MOD_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMOD_fun(mathParser.MOD_funContext ctx);
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
	 * Visit a parse tree produced by the {@code ACOSH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitACOSH_fun(mathParser.ACOSH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISODD_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISODD_fun(mathParser.ISODD_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ASC_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitASC_fun(mathParser.ASC_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code COS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCOS_fun(mathParser.COS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLN_fun(mathParser.LN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code STRING_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSTRING_fun(mathParser.STRING_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HMACMD5_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHMACMD5_fun(mathParser.HMACMD5_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code PRODUCT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPRODUCT_fun(mathParser.PRODUCT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code EXACT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEXACT_fun(mathParser.EXACT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ADDMINUTES_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitADDMINUTES_fun(mathParser.ADDMINUTES_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SUMSQ_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUMSQ_fun(mathParser.SUMSQ_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SUM_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSUM_fun(mathParser.SUM_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SECOND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSECOND_fun(mathParser.SECOND_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code GAMMADIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGAMMADIST_fun(mathParser.GAMMADIST_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code OCT2HEX_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOCT2HEX_fun(mathParser.OCT2HEX_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TODAY_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTODAY_fun(mathParser.TODAY_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ERROR_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitERROR_fun(mathParser.ERROR_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ATAN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitATAN_fun(mathParser.ATAN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code E_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitE_fun(mathParser.E_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TRIM_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRIM_fun(mathParser.TRIM_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code RADIANS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRADIANS_fun(mathParser.RADIANS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code GAMMALN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGAMMALN_fun(mathParser.GAMMALN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TEXT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTEXT_fun(mathParser.TEXT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FISHER_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFISHER_fun(mathParser.FISHER_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code AND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAND_fun(mathParser.AND_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ArrayJson_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitArrayJson_fun(mathParser.ArrayJson_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BIN2HEX_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBIN2HEX_fun(mathParser.BIN2HEX_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MULTINOMIAL_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMULTINOMIAL_fun(mathParser.MULTINOMIAL_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MONTH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMONTH_fun(mathParser.MONTH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code URLENCODE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitURLENCODE_fun(mathParser.URLENCODE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NORMDIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNORMDIST_fun(mathParser.NORMDIST_funContext ctx);
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
	 * Visit a parse tree produced by the {@code Bracket_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBracket_fun(mathParser.Bracket_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BETADIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBETADIST_fun(mathParser.BETADIST_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ATANH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitATANH_fun(mathParser.ATANH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NOW_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNOW_fun(mathParser.NOW_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MEDIAN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMEDIAN_fun(mathParser.MEDIAN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code POWER_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPOWER_fun(mathParser.POWER_funContext ctx);
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
	 * Visit a parse tree produced by the {@code TRUNC_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRUNC_fun(mathParser.TRUNC_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code GCD_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGCD_fun(mathParser.GCD_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TANH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTANH_fun(mathParser.TANH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code HEX2BIN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHEX2BIN_fun(mathParser.HEX2BIN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SINH_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSINH_fun(mathParser.SINH_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SHA512_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSHA512_fun(mathParser.SHA512_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MIN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMIN_fun(mathParser.MIN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ADDDAYS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitADDDAYS_fun(mathParser.ADDDAYS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNONTEXT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNONTEXT_fun(mathParser.ISNONTEXT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ABS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitABS_fun(mathParser.ABS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ROUNDDOWN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitROUNDDOWN_fun(mathParser.ROUNDDOWN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code IF_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIF_fun(mathParser.IF_funContext ctx);
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
	 * Visit a parse tree produced by the {@code Percentage_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPercentage_fun(mathParser.Percentage_funContext ctx);
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
	 * Visit a parse tree produced by the {@code ASIN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitASIN_fun(mathParser.ASIN_funContext ctx);
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
	 * Visit a parse tree produced by the {@code T_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitT_fun(mathParser.T_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code WEEKDAY_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitWEEKDAY_fun(mathParser.WEEKDAY_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BIN2OCT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBIN2OCT_fun(mathParser.BIN2OCT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NULL_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNULL_fun(mathParser.NULL_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code BASE64TOTEXT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBASE64TOTEXT_fun(mathParser.BASE64TOTEXT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TDIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTDIST_fun(mathParser.TDIST_funContext ctx);
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
	 * Visit a parse tree produced by the {@code EVEN_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEVEN_fun(mathParser.EVEN_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code LOGNORMDIST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLOGNORMDIST_fun(mathParser.LOGNORMDIST_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ISNULLOREMPTY_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitISNULLOREMPTY_fun(mathParser.ISNULLOREMPTY_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TRUE_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTRUE_fun(mathParser.TRUE_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code FISHERINV_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFISHERINV_fun(mathParser.FISHERINV_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code SHA1_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSHA1_fun(mathParser.SHA1_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code TIME_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTIME_fun(mathParser.TIME_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ATAN2_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitATAN2_fun(mathParser.ATAN2_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ADDHOURS_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitADDHOURS_fun(mathParser.ADDHOURS_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code RAND_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitRAND_fun(mathParser.RAND_funContext ctx);
	/**
	 * Visit a parse tree produced by {@link mathParser#num}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNum(mathParser.NumContext ctx);
	/**
	 * Visit a parse tree produced by {@link mathParser#unit}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitUnit(mathParser.UnitContext ctx);
	/**
	 * Visit a parse tree produced by {@link mathParser#arrayJson}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitArrayJson(mathParser.ArrayJsonContext ctx);
	/**
	 * Visit a parse tree produced by {@link mathParser#parameter2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParameter2(mathParser.Parameter2Context ctx);
}