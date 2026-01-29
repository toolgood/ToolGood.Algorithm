/**
 * MathSplitvisitor2.js
 * 访问者类，用于解析数学表达式并构建计算树
 */
import mathVisitor from '../../math/mathVisitor.js';
import { CharUtil } from './CharUtil.js';
import { CalculateTreeType } from '../../Enums/CalculateTreeType.js';

export class MathSplitVisitor2 extends mathVisitor  {
    visitProg(context) {
        return context.expr().accept(this);
    }

    visitBracket_fun(context) {
        return context.expr().accept(this);
    }

    visitMulDiv_fun(context) {
        const tree = {
            nodes: []
        };
        const exprs = context.expr();
        const t = context.op.text;
        if (CharUtil.EqualsStrings(t, '*')) {
            tree.Type = CalculateTreeType.Mul;
        } else if (CharUtil.EqualsStrings(t, '/')) {
            tree.Type = CalculateTreeType.Div;
        } else {
            tree.Type = CalculateTreeType.Mod;
        }
        tree.nodes.push(exprs[0].accept(this));
        tree.nodes.push(exprs[1].accept(this));
        tree.start = context.start.startIndex;
        tree.end = context.stop.stopIndex;
        tree.conditionString = context.getText();
        return tree;
    }

    visitAddSub_fun(context) {
        const tree = {
            nodes: []
        };
        const exprs = context.expr();
        const t = context.op.text;
        if (CharUtil.EqualsStrings(t, '+')) {
            tree.Type = CalculateTreeType.Add;
        } else if (CharUtil.EqualsStrings(t, '-')) {
            tree.Type = CalculateTreeType.Sub;
        } else {
            tree.Type = CalculateTreeType.Connect;
        }
        tree.nodes.push(exprs[0].accept(this));
        tree.nodes.push(exprs[1].accept(this));
        tree.start = context.start.startIndex;
        tree.end = context.stop.stopIndex;
        tree.conditionString = context.getText();
        return tree;
    }

    visit_fun(context) {
        const tree = {
            start: context.start.startIndex,
            end: context.stop.stopIndex,
            conditionString: context.getText()
        };
        return tree;
    }

    visitABS_fun(context) {
        return this.visit_fun(context);
    }

    visitACOSH_fun(context) {
        return this.visit_fun(context);
    }

    visitACOS_fun(context) {
        return this.visit_fun(context);
    }

    visitADDDAYS_fun(context) {
        return this.visit_fun(context);
    }

    visitADDHOURS_fun(context) {
        return this.visit_fun(context);
    }

    visitADDMINUTES_fun(context) {
        return this.visit_fun(context);
    }

    visitADDMONTHS_fun(context) {
        return this.visit_fun(context);
    }

    visitADDSECONDS_fun(context) {
        return this.visit_fun(context);
    }

    visitADDYEARS_fun(context) {
        return this.visit_fun(context);
    }

    visitAndOr_fun(context) {
        return this.visit_fun(context);
    }

    visitAND_fun(context) {
        return this.visit_fun(context);
    }

    visitArrayJson(context) {
        return this.visit_fun(context);
    }

    visitArrayJson_fun(context) {
        return this.visit_fun(context);
    }

    visitArray_fun(context) {
        return this.visit_fun(context);
    }

    visitASC_fun(context) {
        return this.visit_fun(context);
    }

    visitASINH_fun(context) {
        return this.visit_fun(context);
    }

    visitASIN_fun(context) {
        return this.visit_fun(context);
    }

    visitATAN2_fun(context) {
        return this.visit_fun(context);
    }

    visitATANH_fun(context) {
        return this.visit_fun(context);
    }

    visitATAN_fun(context) {
        return this.visit_fun(context);
    }

    visitAVEDEV_fun(context) {
        return this.visit_fun(context);
    }

    visitAVERAGEIF_fun(context) {
        return this.visit_fun(context);
    }

    visitAVERAGE_fun(context) {
        return this.visit_fun(context);
    }

    visitBASE64TOTEXT_fun(context) {
        return this.visit_fun(context);
    }

    visitBASE64URLTOTEXT_fun(context) {
        return this.visit_fun(context);
    }

    visitBETADIST_fun(context) {
        return this.visit_fun(context);
    }

    visitBETAINV_fun(context) {
        return this.visit_fun(context);
    }

    visitBIN2DEC_fun(context) {
        return this.visit_fun(context);
    }

    visitBIN2HEX_fun(context) {
        return this.visit_fun(context);
    }

    visitBIN2OCT_fun(context) {
        return this.visit_fun(context);
    }

    visitBINOMDIST_fun(context) {
        return this.visit_fun(context);
    }

    visitCEILING_fun(context) {
        return this.visit_fun(context);
    }

    visitCHAR_fun(context) {
        return this.visit_fun(context);
    }

    visitCLEAN_fun(context) {
        return this.visit_fun(context);
    }

    visitCODE_fun(context) {
        return this.visit_fun(context);
    }

    visitCOMBIN_fun(context) {
        return this.visit_fun(context);
    }

    visitCONCATENATE_fun(context) {
        return this.visit_fun(context);
    }

    visitCOSH_fun(context) {
        return this.visit_fun(context);
    }

    visitCOS_fun(context) {
        return this.visit_fun(context);
    }

    visitCOUNTIF_fun(context) {
        return this.visit_fun(context);
    }

    visitCOUNT_fun(context) {
        return this.visit_fun(context);
    }

    visitCOVARIANCES_fun(context) {
        return this.visit_fun(context);
    }

    visitCOVAR_fun(context) {
        return this.visit_fun(context);
    }

    visitCRC32_fun(context) {
        return this.visit_fun(context);
    }

    visitDATEDIF_fun(context) {
        return this.visit_fun(context);
    }

    visitDATEVALUE_fun(context) {
        return this.visit_fun(context);
    }

    visitDATE_fun(context) {
        return this.visit_fun(context);
    }

    visitDAYS360_fun(context) {
        return this.visit_fun(context);
    }

    visitDAY_fun(context) {
        return this.visit_fun(context);
    }

    visitDEC2BIN_fun(context) {
        return this.visit_fun(context);
    }

    visitDEC2HEX_fun(context) {
        return this.visit_fun(context);
    }

    visitDEC2OCT_fun(context) {
        return this.visit_fun(context);
    }

    visitDEGREES_fun(context) {
        return this.visit_fun(context);
    }

    visitDEVSQ_fun(context) {
        return this.visit_fun(context);
    }

    visitDiyFunction_fun(context) {
        return this.visit_fun(context);
    }

    visitEDATE_fun(context) {
        return this.visit_fun(context);
    }

    visitENDSWITH_fun(context) {
        return this.visit_fun(context);
    }

    visitEOMONTH_fun(context) {
        return this.visit_fun(context);
    }

    visitERROR_fun(context) {
        return this.visit_fun(context);
    }

    visitEVEN_fun(context) {
        return this.visit_fun(context);
    }

    visitEXACT_fun(context) {
        return this.visit_fun(context);
    }

    visitEXPONDIST_fun(context) {
        return this.visit_fun(context);
    }

    visitEXP_fun(context) {
        return this.visit_fun(context);
    }

    visitE_fun(context) {
        return this.visit_fun(context);
    }

    visitFACTDOUBLE_fun(context) {
        return this.visit_fun(context);
    }

    visitFACT_fun(context) {
        return this.visit_fun(context);
    }

    visitFALSE_fun(context) {
        return this.visit_fun(context);
    }

    visitFDIST_fun(context) {
        return this.visit_fun(context);
    }

    visitFIND_fun(context) {
        return this.visit_fun(context);
    }

    visitFINV_fun(context) {
        return this.visit_fun(context);
    }

    visitFISHERINV_fun(context) {
        return this.visit_fun(context);
    }

    visitFISHER_fun(context) {
        return this.visit_fun(context);
    }

    visitFIXED_fun(context) {
        return this.visit_fun(context);
    }

    visitFLOOR_fun(context) {
        return this.visit_fun(context);
    }

    visitGAMMADIST_fun(context) {
        return this.visit_fun(context);
    }

    visitGAMMAINV_fun(context) {
        return this.visit_fun(context);
    }

    visitGAMMALN_fun(context) {
        return this.visit_fun(context);
    }

    visitGCD_fun(context) {
        return this.visit_fun(context);
    }

    visitGEOMEAN_fun(context) {
        return this.visit_fun(context);
    }

    visitGetJsonValue_fun(context) {
        return this.visit_fun(context);
    }

    visitGUID_fun(context) {
        return this.visit_fun(context);
    }

    visitHARMEAN_fun(context) {
        return this.visit_fun(context);
    }

    visitHASVALUE_fun(context) {
        return this.visit_fun(context);
    }

    visitHAS_fun(context) {
        return this.visit_fun(context);
    }

    visitHEX2BIN_fun(context) {
        return this.visit_fun(context);
    }

    visitHEX2DEC_fun(context) {
        return this.visit_fun(context);
    }

    visitHEX2OCT_fun(context) {
        return this.visit_fun(context);
    }

    visitHMACMD5_fun(context) {
        return this.visit_fun(context);
    }

    visitHMACSHA1_fun(context) {
        return this.visit_fun(context);
    }

    visitHMACSHA256_fun(context) {
        return this.visit_fun(context);
    }

    visitHMACSHA512_fun(context) {
        return this.visit_fun(context);
    }

    visitHOUR_fun(context) {
        return this.visit_fun(context);
    }

    visitHTMLDECODE_fun(context) {
        return this.visit_fun(context);
    }

    visitHTMLENCODE_fun(context) {
        return this.visit_fun(context);
    }

    visitHYPGEOMDIST_fun(context) {
        return this.visit_fun(context);
    }

    visitIFERROR_fun(context) {
        return this.visit_fun(context);
    }

    visitIF_fun(context) {
        return this.visit_fun(context);
    }

    visitINDEXOF_fun(context) {
        return this.visit_fun(context);
    }

    visitINT_fun(context) {
        return this.visit_fun(context);
    }

    visitISERROR_fun(context) {
        return this.visit_fun(context);
    }

    visitISEVEN_fun(context) {
        return this.visit_fun(context);
    }

    visitISLOGICAL_fun(context) {
        return this.visit_fun(context);
    }

    visitISNONTEXT_fun(context) {
        return this.visit_fun(context);
    }

    visitISNULLOREMPTY_fun(context) {
        return this.visit_fun(context);
    }

    visitISNULLORERROR_fun(context) {
        return this.visit_fun(context);
    }

    visitISNULLORWHITESPACE_fun(context) {
        return this.visit_fun(context);
    }

    visitISNULL_fun(context) {
        return this.visit_fun(context);
    }

    visitISNUMBER_fun(context) {
        return this.visit_fun(context);
    }

    visitISODD_fun(context) {
        return this.visit_fun(context);
    }

    visitISREGEX_fun(context) {
        return this.visit_fun(context);
    }

    visitISTEXT_fun(context) {
        return this.visit_fun(context);
    }

    visitJIS_fun(context) {
        return this.visit_fun(context);
    }

    visitJOIN_fun(context) {
        return this.visit_fun(context);
    }

    visitJSON_fun(context) {
        return this.visit_fun(context);
    }

    visitJudge_fun(context) {
        return this.visit_fun(context);
    }

    visitLARGE_fun(context) {
        return this.visit_fun(context);
    }

    visitLASTINDEXOF_fun(context) {
        return this.visit_fun(context);
    }

    visitLCM_fun(context) {
        return this.visit_fun(context);
    }

    visitLEFT_fun(context) {
        return this.visit_fun(context);
    }

    visitLEN_fun(context) {
        return this.visit_fun(context);
    }

    visitLN_fun(context) {
        return this.visit_fun(context);
    }

    visitLOG10_fun(context) {
        return this.visit_fun(context);
    }

    visitLOGINV_fun(context) {
        return this.visit_fun(context);
    }

    visitLOGNORMDIST_fun(context) {
        return this.visit_fun(context);
    }

    visitLOG_fun(context) {
        return this.visit_fun(context);
    }

    visitLOWER_fun(context) {
        return this.visit_fun(context);
    }

    visitMAX_fun(context) {
        return this.visit_fun(context);
    }

    visitMD5_fun(context) {
        return this.visit_fun(context);
    }

    visitMEDIAN_fun(context) {
        return this.visit_fun(context);
    }

    visitMID_fun(context) {
        return this.visit_fun(context);
    }

    visitMINUTE_fun(context) {
        return this.visit_fun(context);
    }

    visitMIN_fun(context) {
        return this.visit_fun(context);
    }

    visitMODE_fun(context) {
        return this.visit_fun(context);
    }

    visitMOD_fun(context) {
        return this.visit_fun(context);
    }

    visitMONTH_fun(context) {
        return this.visit_fun(context);
    }

    visitMROUND_fun(context) {
        return this.visit_fun(context);
    }

    visitMULTINOMIAL_fun(context) {
        return this.visit_fun(context);
    }

    visitNEGBINOMDIST_fun(context) {
        return this.visit_fun(context);
    }

    visitNETWORKDAYS_fun(context) {
        return this.visit_fun(context);
    }

    visitNORMDIST_fun(context) {
        return this.visit_fun(context);
    }

    visitNORMINV_fun(context) {
        return this.visit_fun(context);
    }

    visitNORMSDIST_fun(context) {
        return this.visit_fun(context);
    }

    visitNORMSINV_fun(context) {
        return this.visit_fun(context);
    }

    visitNOT_fun(context) {
        return this.visit_fun(context);
    }

    visitNOW_fun(context) {
        return this.visit_fun(context);
    }

    visitNULL_fun(context) {
        return this.visit_fun(context);
    }

    visitNum(context) {
        return this.visit_fun(context);
    }

    visitNUM_fun(context) {
        return this.visit_fun(context);
    }

    visitOCT2BIN_fun(context) {
        return this.visit_fun(context);
    }

    visitOCT2DEC_fun(context) {
        return this.visit_fun(context);
    }

    visitOCT2HEX_fun(context) {
        return this.visit_fun(context);
    }

    visitODD_fun(context) {
        return this.visit_fun(context);
    }

    visitOR_fun(context) {
        return this.visit_fun(context);
    }

    visitParameter2(context) {
        return this.visit_fun(context);
    }

    visitPARAMETER_fun(context) {
        return this.visit_fun(context);
    }

    visitPARAM_fun(context) {
        return this.visit_fun(context);
    }

    visitPercentage_fun(context) {
        return this.visit_fun(context);
    }

    visitPERCENTILE_fun(context) {
        return this.visit_fun(context);
    }

    visitPERCENTRANK_fun(context) {
        return this.visit_fun(context);
    }

    visitPERMUT_fun(context) {
        return this.visit_fun(context);
    }

    visitPI_fun(context) {
        return this.visit_fun(context);
    }

    visitPOISSON_fun(context) {
        return this.visit_fun(context);
    }

    visitPOWER_fun(context) {
        return this.visit_fun(context);
    }

    visitPRODUCT_fun(context) {
        return this.visit_fun(context);
    }

    visitPROPER_fun(context) {
        return this.visit_fun(context);
    }

    visitQUARTILE_fun(context) {
        return this.visit_fun(context);
    }

    visitQUOTIENT_fun(context) {
        return this.visit_fun(context);
    }

    visitRADIANS_fun(context) {
        return this.visit_fun(context);
    }

    visitRANDBETWEEN_fun(context) {
        return this.visit_fun(context);
    }

    visitRAND_fun(context) {
        return this.visit_fun(context);
    }

    visitREGEXREPALCE_fun(context) {
        return this.visit_fun(context);
    }

    visitREGEX_fun(context) {
        return this.visit_fun(context);
    }

    visitREMOVEEND_fun(context) {
        return this.visit_fun(context);
    }

    visitREMOVESTART_fun(context) {
        return this.visit_fun(context);
    }

    visitREPLACE_fun(context) {
        return this.visit_fun(context);
    }

    visitREPT_fun(context) {
        return this.visit_fun(context);
    }

    visitRIGHT_fun(context) {
        return this.visit_fun(context);
    }

    visitRMB_fun(context) {
        return this.visit_fun(context);
    }

    visitROUNDDOWN_fun(context) {
        return this.visit_fun(context);
    }

    visitROUNDUP_fun(context) {
        return this.visit_fun(context);
    }

    visitROUND_fun(context) {
        return this.visit_fun(context);
    }

    visitSEARCH_fun(context) {
        return this.visit_fun(context);
    }

    visitSECOND_fun(context) {
        return this.visit_fun(context);
    }

    visitSHA1_fun(context) {
        return this.visit_fun(context);
    }

    visitSHA256_fun(context) {
        return this.visit_fun(context);
    }

    visitSHA512_fun(context) {
        return this.visit_fun(context);
    }

    visitSIGN_fun(context) {
        return this.visit_fun(context);
    }

    visitSINH_fun(context) {
        return this.visit_fun(context);
    }

    visitSIN_fun(context) {
        return this.visit_fun(context);
    }

    visitSMALL_fun(context) {
        return this.visit_fun(context);
    }

    visitSPLIT_fun(context) {
        return this.visit_fun(context);
    }

    visitSQRTPI_fun(context) {
        return this.visit_fun(context);
    }

    visitSQRT_fun(context) {
        return this.visit_fun(context);
    }

    visitSTARTSWITH_fun(context) {
        return this.visit_fun(context);
    }

    visitSTDEVP_fun(context) {
        return this.visit_fun(context);
    }

    visitSTDEV_fun(context) {
        return this.visit_fun(context);
    }

    visitSTRING_fun(context) {
        return this.visit_fun(context);
    }

    visitSUBSTITUTE_fun(context) {
        return this.visit_fun(context);
    }

    visitSUBSTRING_fun(context) {
        return this.visit_fun(context);
    }

    visitSUMIF_fun(context) {
        return this.visit_fun(context);
    }

    visitSUMSQ_fun(context) {
        return this.visit_fun(context);
    }

    visitSUM_fun(context) {
        return this.visit_fun(context);
    }

    visitTANH_fun(context) {
        return this.visit_fun(context);
    }

    visitTAN_fun(context) {
        return this.visit_fun(context);
    }

    visitTDIST_fun(context) {
        return this.visit_fun(context);
    }

    visitTEXTTOBASE64URL_fun(context) {
        return this.visit_fun(context);
    }

    visitTEXTTOBASE64_fun(context) {
        return this.visit_fun(context);
    }

    visitTEXT_fun(context) {
        return this.visit_fun(context);
    }

    visitTIMESTAMP_fun(context) {
        return this.visit_fun(context);
    }

    visitTIMEVALUE_fun(context) {
        return this.visit_fun(context);
    }

    visitTIME_fun(context) {
        return this.visit_fun(context);
    }

    visitTINV_fun(context) {
        return this.visit_fun(context);
    }

    visitTODAY_fun(context) {
        return this.visit_fun(context);
    }

    visitTRIMEND_fun(context) {
        return this.visit_fun(context);
    }

    visitTRIMSTART_fun(context) {
        return this.visit_fun(context);
    }

    visitTRIM_fun(context) {
        return this.visit_fun(context);
    }

    visitTRUE_fun(context) {
        return this.visit_fun(context);
    }

    visitTRUNC_fun(context) {
        return this.visit_fun(context);
    }

    visitT_fun(context) {
        return this.visit_fun(context);
    }

    visitUnit(context) {
        return this.visit_fun(context);
    }

    visitUPPER_fun(context) {
        return this.visit_fun(context);
    }

    visitURLDECODE_fun(context) {
        return this.visit_fun(context);
    }

    visitURLENCODE_fun(context) {
        return this.visit_fun(context);
    }

    visitVALUE_fun(context) {
        return this.visit_fun(context);
    }

    visitVARP_fun(context) {
        return this.visit_fun(context);
    }

    visitVAR_fun(context) {
        return this.visit_fun(context);
    }

    visitVersion_fun(context) {
        return this.visit_fun(context);
    }

    visitWEEKDAY_fun(context) {
        return this.visit_fun(context);
    }

    visitWEEKNUM_fun(context) {
        return this.visit_fun(context);
    }

    visitWEIBULL_fun(context) {
        return this.visit_fun(context);
    }

    visitWORKDAY_fun(context) {
        return this.visit_fun(context);
    }

    visitYEAR_fun(context) {
        return this.visit_fun(context);
    }

    visitLOOKFLOOR_fun(context) {
        return this.visit_fun(context);
    }

    visitLOOKCEILING_fun(context) {
        return this.visit_fun(context);
    }
}
