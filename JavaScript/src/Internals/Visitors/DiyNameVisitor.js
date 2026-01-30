/**
 * DiyNameVisitor - DIY 名称访问者
 */
import mathVisitor from '../../math/mathVisitor.js';
import { DiyNameInfo } from '../DiyNameInfo.js';

class DiyNameVisitor extends mathVisitor {
    constructor() {
        super();
        this.diy = new DiyNameInfo();
    }

    /**
     * 访问 PARAMETER 函数节点
     */
    visitPARAMETER_fun(context) {
        const node = context.PARAMETER();
        if (node) {
            this.diy.Parameters.push({
                Name: node.getText(),
                Start: node.symbol.start,
                End: node.symbol.stop
            });
        }
        return this.visitChildren(context);
    }

    /**
     * 访问 GetJsonValue 函数节点
     */
    visitGetJsonValue_fun(context) {
        const node = context.PARAMETER();
        if (node) {
            this.diy.Parameters.push({
                Name: node.getText(),
                Start: node.symbol.start,
                End: node.symbol.stop
            });
        }
        return this.visitChildren(context);
    }

    /**
     * 访问 DIY 函数节点
     */
    visitDiyFunction_fun(context) {
        const node = context.PARAMETER();
        this.diy.Functions.push({
            Name: node.getText(),
            Start: node.symbol.start,
            End: node.symbol.stop
        });
        return this.visitChildren(context);
    }

    /**
     * 访问 ABS 函数节点
     */
    visitABS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ACOSH 函数节点
     */
    visitACOSH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ACOS 函数节点
     */
    visitACOS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 AddSub 函数节点
     */
    visitAddSub_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 AndOr 函数节点
     */
    visitAndOr_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 AND 函数节点
     */
    visitAND_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 Array 函数节点
     */
    visitArray_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ASC 函数节点
     */
    visitASC_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ASINH 函数节点
     */
    visitASINH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ASIN 函数节点
     */
    visitASIN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ATAN2 函数节点
     */
    visitATAN2_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ATANH 函数节点
     */
    visitATANH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ATAN 函数节点
     */
    visitATAN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 AVEDEV 函数节点
     */
    visitAVEDEV_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 AVERAGEIF 函数节点
     */
    visitAVERAGEIF_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 AVERAGE 函数节点
     */
    visitAVERAGE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 BASE64TOTEXT 函数节点
     */
    visitBASE64TOTEXT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 BASE64URLTOTEXT 函数节点
     */
    visitBASE64URLTOTEXT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 BETADIST 函数节点
     */
    visitBETADIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 BETAINV 函数节点
     */
    visitBETAINV_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 BIN2DEC 函数节点
     */
    visitBIN2DEC_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 BIN2HEX 函数节点
     */
    visitBIN2HEX_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 BIN2OCT 函数节点
     */
    visitBIN2OCT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 BINOMDIST 函数节点
     */
    visitBINOMDIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 Bracket 函数节点
     */
    visitBracket_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 CEILING 函数节点
     */
    visitCEILING_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 CHAR 函数节点
     */
    visitCHAR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 CLEAN 函数节点
     */
    visitCLEAN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 CODE 函数节点
     */
    visitCODE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 COMBIN 函数节点
     */
    visitCOMBIN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 CONCATENATE 函数节点
     */
    visitCONCATENATE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 COSH 函数节点
     */
    visitCOSH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 COS 函数节点
     */
    visitCOS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 COUNTIF 函数节点
     */
    visitCOUNTIF_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 COUNT 函数节点
     */
    visitCOUNT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 CRC32 函数节点
     */
    visitCRC32_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 DATEDIF 函数节点
     */
    visitDATEDIF_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 DATEVALUE 函数节点
     */
    visitDATEVALUE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 DATE 函数节点
     */
    visitDATE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 DAYS360 函数节点
     */
    visitDAYS360_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 DAY 函数节点
     */
    visitDAY_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 DEC2BIN 函数节点
     */
    visitDEC2BIN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 DEC2HEX 函数节点
     */
    visitDEC2HEX_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 DEC2OCT 函数节点
     */
    visitDEC2OCT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 DEGREES 函数节点
     */
    visitDEGREES_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 DEVSQ 函数节点
     */
    visitDEVSQ_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 EDATE 函数节点
     */
    visitEDATE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ENDSWITH 函数节点
     */
    visitENDSWITH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 EOMONTH 函数节点
     */
    visitEOMONTH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 EVEN 函数节点
     */
    visitEVEN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 EXACT 函数节点
     */
    visitEXACT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 EXPONDIST 函数节点
     */
    visitEXPONDIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 EXP 函数节点
     */
    visitEXP_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 E 函数节点
     */
    visitE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 FACTDOUBLE 函数节点
     */
    visitFACTDOUBLE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 FACT 函数节点
     */
    visitFACT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 FALSE 函数节点
     */
    visitFALSE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 FDIST 函数节点
     */
    visitFDIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 FIND 函数节点
     */
    visitFIND_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 FINV 函数节点
     */
    visitFINV_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 FISHERINV 函数节点
     */
    visitFISHERINV_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 FISHER 函数节点
     */
    visitFISHER_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 FIXED 函数节点
     */
    visitFIXED_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 FLOOR 函数节点
     */
    visitFLOOR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 GAMMADIST 函数节点
     */
    visitGAMMADIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 GAMMAINV 函数节点
     */
    visitGAMMAINV_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 GAMMALN 函数节点
     */
    visitGAMMALN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 GCD 函数节点
     */
    visitGCD_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 GEOMEAN 函数节点
     */
    visitGEOMEAN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 GUID 函数节点
     */
    visitGUID_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HARMEAN 函数节点
     */
    visitHARMEAN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HEX2BIN 函数节点
     */
    visitHEX2BIN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HEX2DEC 函数节点
     */
    visitHEX2DEC_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HEX2OCT 函数节点
     */
    visitHEX2OCT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HMACMD5 函数节点
     */
    visitHMACMD5_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HMACSHA1 函数节点
     */
    visitHMACSHA1_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HMACSHA256 函数节点
     */
    visitHMACSHA256_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HMACSHA512 函数节点
     */
    visitHMACSHA512_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HOUR 函数节点
     */
    visitHOUR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HTMLDECODE 函数节点
     */
    visitHTMLDECODE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HTMLENCODE 函数节点
     */
    visitHTMLENCODE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HYPGEOMDIST 函数节点
     */
    visitHYPGEOMDIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 IFERROR 函数节点
     */
    visitIFERROR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 IF 函数节点
     */
    visitIF_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 INDEXOF 函数节点
     */
    visitINDEXOF_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 INT 函数节点
     */
    visitINT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISERROR 函数节点
     */
    visitISERROR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISEVEN 函数节点
     */
    visitISEVEN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISLOGICAL 函数节点
     */
    visitISLOGICAL_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISNONTEXT 函数节点
     */
    visitISNONTEXT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISNULLOREMPTY 函数节点
     */
    visitISNULLOREMPTY_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISNULLORERROR 函数节点
     */
    visitISNULLORERROR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISNULLORWHITESPACE 函数节点
     */
    visitISNULLORWHITESPACE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISNULL 函数节点
     */
    visitISNULL_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISNUMBER 函数节点
     */
    visitISNUMBER_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISODD 函数节点
     */
    visitISODD_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISREGEX 函数节点
     */
    visitISREGEX_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ISTEXT 函数节点
     */
    visitISTEXT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 JIS 函数节点
     */
    visitJIS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 JOIN 函数节点
     */
    visitJOIN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 JSON 函数节点
     */
    visitJSON_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 Judge 函数节点
     */
    visitJudge_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LARGE 函数节点
     */
    visitLARGE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LASTINDEXOF 函数节点
     */
    visitLASTINDEXOF_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LCM 函数节点
     */
    visitLCM_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LEFT 函数节点
     */
    visitLEFT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LEN 函数节点
     */
    visitLEN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LN 函数节点
     */
    visitLN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LOG10 函数节点
     */
    visitLOG10_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LOGINV 函数节点
     */
    visitLOGINV_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LOGNORMDIST 函数节点
     */
    visitLOGNORMDIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LOG 函数节点
     */
    visitLOG_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LOWER 函数节点
     */
    visitLOWER_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MAX 函数节点
     */
    visitMAX_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MD5 函数节点
     */
    visitMD5_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MEDIAN 函数节点
     */
    visitMEDIAN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MID 函数节点
     */
    visitMID_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MINUTE 函数节点
     */
    visitMINUTE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MIN 函数节点
     */
    visitMIN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MODE 函数节点
     */
    visitMODE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MOD 函数节点
     */
    visitMOD_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MONTH 函数节点
     */
    visitMONTH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MROUND 函数节点
     */
    visitMROUND_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MulDiv 函数节点
     */
    visitMulDiv_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 MULTINOMIAL 函数节点
     */
    visitMULTINOMIAL_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 NEGBINOMDIST 函数节点
     */
    visitNEGBINOMDIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 NETWORKDAYS 函数节点
     */
    visitNETWORKDAYS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 NORMDIST 函数节点
     */
    visitNORMDIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 NORMINV 函数节点
     */
    visitNORMINV_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 NORMSDIST 函数节点
     */
    visitNORMSDIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 NORMSINV 函数节点
     */
    visitNORMSINV_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 NOT 函数节点
     */
    visitNOT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 NOW 函数节点
     */
    visitNOW_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 NULL 函数节点
     */
    visitNULL_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 NUM 函数节点
     */
    visitNUM_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 OCT2BIN 函数节点
     */
    visitOCT2BIN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 OCT2DEC 函数节点
     */
    visitOCT2DEC_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 OCT2HEX 函数节点
     */
    visitOCT2HEX_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ODD 函数节点
     */
    visitODD_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 OR 函数节点
     */
    visitOR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 Parameter2 节点
     */
    visitParameter2(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 Percentage 函数节点
     */
    visitPercentage_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 PERCENTILE 函数节点
     */
    visitPERCENTILE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 PERCENTRANK 函数节点
     */
    visitPERCENTRANK_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 PERMUT 函数节点
     */
    visitPERMUT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 PI 函数节点
     */
    visitPI_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 POISSON 函数节点
     */
    visitPOISSON_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 POWER 函数节点
     */
    visitPOWER_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 PRODUCT 函数节点
     */
    visitPRODUCT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 Prog 节点
     */
    visitProg(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 PROPER 函数节点
     */
    visitPROPER_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 QUARTILE 函数节点
     */
    visitQUARTILE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 QUOTIENT 函数节点
     */
    visitQUOTIENT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 RADIANS 函数节点
     */
    visitRADIANS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 RANDBETWEEN 函数节点
     */
    visitRANDBETWEEN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 RAND 函数节点
     */
    visitRAND_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 REGEXREPALCE 函数节点
     */
    visitREGEXREPALCE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 REGEX 函数节点
     */
    visitREGEX_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 REMOVEEND 函数节点
     */
    visitREMOVEEND_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 REMOVESTART 函数节点
     */
    visitREMOVESTART_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 REPLACE 函数节点
     */
    visitREPLACE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 REPT 函数节点
     */
    visitREPT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 RIGHT 函数节点
     */
    visitRIGHT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 RMB 函数节点
     */
    visitRMB_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ROUNDDOWN 函数节点
     */
    visitROUNDDOWN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ROUNDUP 函数节点
     */
    visitROUNDUP_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ROUND 函数节点
     */
    visitROUND_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SEARCH 函数节点
     */
    visitSEARCH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SECOND 函数节点
     */
    visitSECOND_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SHA1 函数节点
     */
    visitSHA1_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SHA256 函数节点
     */
    visitSHA256_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SHA512 函数节点
     */
    visitSHA512_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SIGN 函数节点
     */
    visitSIGN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SINH 函数节点
     */
    visitSINH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SIN 函数节点
     */
    visitSIN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SMALL 函数节点
     */
    visitSMALL_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SPLIT 函数节点
     */
    visitSPLIT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SQRTPI 函数节点
     */
    visitSQRTPI_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SQRT 函数节点
     */
    visitSQRT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 STARTSWITH 函数节点
     */
    visitSTARTSWITH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 STDEVP 函数节点
     */
    visitSTDEVP_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 STDEV 函数节点
     */
    visitSTDEV_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 STRING 节点
     */
    visitSTRING_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SUBSTITUTE 函数节点
     */
    visitSUBSTITUTE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SUBSTRING 函数节点
     */
    visitSUBSTRING_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SUMIF 函数节点
     */
    visitSUMIF_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SUMSQ 函数节点
     */
    visitSUMSQ_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 SUM 函数节点
     */
    visitSUM_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TANH 函数节点
     */
    visitTANH_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TAN 函数节点
     */
    visitTAN_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TDIST 函数节点
     */
    visitTDIST_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TEXTTOBASE64URL 函数节点
     */
    visitTEXTTOBASE64URL_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TEXTTOBASE64 函数节点
     */
    visitTEXTTOBASE64_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TEXT 函数节点
     */
    visitTEXT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TIMEVALUE 函数节点
     */
    visitTIMEVALUE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TIME 函数节点
     */
    visitTIME_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TINV 函数节点
     */
    visitTINV_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TODAY 函数节点
     */
    visitTODAY_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TRIMEND 函数节点
     */
    visitTRIMEND_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TRIMSTART 函数节点
     */
    visitTRIMSTART_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TRIM 函数节点
     */
    visitTRIM_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TRUE 函数节点
     */
    visitTRUE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TRUNC 函数节点
     */
    visitTRUNC_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 T 函数节点
     */
    visitT_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 UPPER 函数节点
     */
    visitUPPER_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 URLDECODE 函数节点
     */
    visitURLDECODE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 URLENCODE 函数节点
     */
    visitURLENCODE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 VALUE 函数节点
     */
    visitVALUE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 VARP 函数节点
     */
    visitVARP_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 VAR 函数节点
     */
    visitVAR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 WEEKDAY 函数节点
     */
    visitWEEKDAY_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 WEEKNUM 函数节点
     */
    visitWEEKNUM_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 WEIBULL 函数节点
     */
    visitWEIBULL_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 WORKDAY 函数节点
     */
    visitWORKDAY_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 YEAR 函数节点
     */
    visitYEAR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ADDMONTHS 函数节点
     */
    visitADDMONTHS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ADDYEARS 函数节点
     */
    visitADDYEARS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ADDSECONDS 函数节点
     */
    visitADDSECONDS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ADDMINUTES 函数节点
     */
    visitADDMINUTES_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ADDDAYS 函数节点
     */
    visitADDDAYS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ADDHOURS 函数节点
     */
    visitADDHOURS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 TIMESTAMP 函数节点
     */
    visitTIMESTAMP_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 PARAM 函数节点
     */
    visitPARAM_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HAS 函数节点
     */
    visitHAS_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ArrayJson 函数节点
     */
    visitArrayJson_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 Num 节点
     */
    visitNum(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 Unit 节点
     */
    visitUnit(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ArrayJson 节点
     */
    visitArrayJson(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 ERROR 函数节点
     */
    visitERROR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 HASVALUE 函数节点
     */
    visitHASVALUE_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 COVARIANCES 函数节点
     */
    visitCOVARIANCES_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 COVAR 函数节点
     */
    visitCOVAR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 Version 函数节点
     */
    visitVersion_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LOOKFLOOR 函数节点
     */
    visitLOOKFLOOR_fun(context) {
        return this.visitChildren(context);
    }

    /**
     * 访问 LOOKCEILING 函数节点
     */
    visitLOOKCEILING_fun(context) {
        return this.visitChildren(context);
    }
}

export { DiyNameVisitor };
