/**
 * MathSplitVisitor2.js
 * 访问者类，用于解析数学表达式并构建计算树
 */

import { CharUtil } from './CharUtil.js';
import { CalculateTreeType } from '../../Enums/CalculateTreeType.js';

export class MathSplitVisitor2 {
    VisitProg(context) {
        return context.expr().accept(this);
    }

    VisitBracket_fun(context) {
        return context.expr().accept(this);
    }

    VisitMulDiv_fun(context) {
        const tree = {
            nodes: []
        };
        const exprs = context.expr();
        const t = context.op.text;
        if (CharUtil.EqualsStrings(t, '*')) {
            tree.type = CalculateTreeType.Mul;
        } else if (CharUtil.EqualsStrings(t, '/')) {
            tree.type = CalculateTreeType.Div;
        } else {
            tree.type = CalculateTreeType.Mod;
        }
        tree.nodes.push(exprs[0].accept(this));
        tree.nodes.push(exprs[1].accept(this));
        tree.start = context.start.startIndex;
        tree.end = context.stop.stopIndex;
        tree.conditionString = context.getText();
        return tree;
    }

    VisitAddSub_fun(context) {
        const tree = {
            nodes: []
        };
        const exprs = context.expr();
        const t = context.op.text;
        if (CharUtil.EqualsStrings(t, '+')) {
            tree.type = CalculateTreeType.Add;
        } else if (CharUtil.EqualsStrings(t, '-')) {
            tree.type = CalculateTreeType.Sub;
        } else {
            tree.type = CalculateTreeType.Connect;
        }
        tree.nodes.push(exprs[0].accept(this));
        tree.nodes.push(exprs[1].accept(this));
        tree.start = context.start.startIndex;
        tree.end = context.stop.stopIndex;
        tree.conditionString = context.getText();
        return tree;
    }

    Visit_fun(context) {
        const tree = {
            start: context.start.startIndex,
            end: context.stop.stopIndex,
            conditionString: context.getText()
        };
        return tree;
    }

    VisitABS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitACOSH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitACOS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitADDDAYS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitADDHOURS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitADDMINUTES_fun(context) {
        return this.Visit_fun(context);
    }

    VisitADDMONTHS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitADDSECONDS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitADDYEARS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitAndOr_fun(context) {
        return this.Visit_fun(context);
    }

    VisitAND_fun(context) {
        return this.Visit_fun(context);
    }

    VisitArrayJson(context) {
        return this.Visit_fun(context);
    }

    VisitArrayJson_fun(context) {
        return this.Visit_fun(context);
    }

    VisitArray_fun(context) {
        return this.Visit_fun(context);
    }

    VisitASC_fun(context) {
        return this.Visit_fun(context);
    }

    VisitASINH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitASIN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitATAN2_fun(context) {
        return this.Visit_fun(context);
    }

    VisitATANH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitATAN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitAVEDEV_fun(context) {
        return this.Visit_fun(context);
    }

    VisitAVERAGEIF_fun(context) {
        return this.Visit_fun(context);
    }

    VisitAVERAGE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitBASE64TOTEXT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitBASE64URLTOTEXT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitBETADIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitBETAINV_fun(context) {
        return this.Visit_fun(context);
    }

    VisitBIN2DEC_fun(context) {
        return this.Visit_fun(context);
    }

    VisitBIN2HEX_fun(context) {
        return this.Visit_fun(context);
    }

    VisitBIN2OCT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitBINOMDIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCEILING_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCHAR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCLEAN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCODE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCOMBIN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCONCATENATE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCOSH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCOS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCOUNTIF_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCOUNT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCOVARIANCES_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCOVAR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitCRC32_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDATEDIF_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDATEVALUE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDATE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDAYS360_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDAY_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDEC2BIN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDEC2HEX_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDEC2OCT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDEGREES_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDEVSQ_fun(context) {
        return this.Visit_fun(context);
    }

    VisitDiyFunction_fun(context) {
        return this.Visit_fun(context);
    }

    VisitEDATE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitENDSWITH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitEOMONTH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitERROR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitEVEN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitEXACT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitEXPONDIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitEXP_fun(context) {
        return this.Visit_fun(context);
    }

    VisitE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitFACTDOUBLE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitFACT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitFALSE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitFDIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitFIND_fun(context) {
        return this.Visit_fun(context);
    }

    VisitFINV_fun(context) {
        return this.Visit_fun(context);
    }

    VisitFISHERINV_fun(context) {
        return this.Visit_fun(context);
    }

    VisitFISHER_fun(context) {
        return this.Visit_fun(context);
    }

    VisitFIXED_fun(context) {
        return this.Visit_fun(context);
    }

    VisitFLOOR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitGAMMADIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitGAMMAINV_fun(context) {
        return this.Visit_fun(context);
    }

    VisitGAMMALN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitGCD_fun(context) {
        return this.Visit_fun(context);
    }

    VisitGEOMEAN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitGetJsonValue_fun(context) {
        return this.Visit_fun(context);
    }

    VisitGUID_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHARMEAN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHASVALUE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHAS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHEX2BIN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHEX2DEC_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHEX2OCT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHMACMD5_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHMACSHA1_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHMACSHA256_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHMACSHA512_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHOUR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHTMLDECODE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHTMLENCODE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitHYPGEOMDIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitIFERROR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitIF_fun(context) {
        return this.Visit_fun(context);
    }

    VisitINDEXOF_fun(context) {
        return this.Visit_fun(context);
    }

    VisitINT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISERROR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISEVEN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISLOGICAL_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISNONTEXT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISNULLOREMPTY_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISNULLORERROR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISNULLORWHITESPACE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISNULL_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISNUMBER_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISODD_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISREGEX_fun(context) {
        return this.Visit_fun(context);
    }

    VisitISTEXT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitJIS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitJOIN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitJSON_fun(context) {
        return this.Visit_fun(context);
    }

    VisitJudge_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLARGE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLASTINDEXOF_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLCM_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLEFT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLEN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLOG10_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLOGINV_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLOGNORMDIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLOG_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLOWER_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMAX_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMD5_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMEDIAN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMID_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMINUTE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMIN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMODE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMOD_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMONTH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMROUND_fun(context) {
        return this.Visit_fun(context);
    }

    VisitMULTINOMIAL_fun(context) {
        return this.Visit_fun(context);
    }

    VisitNEGBINOMDIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitNETWORKDAYS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitNORMDIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitNORMINV_fun(context) {
        return this.Visit_fun(context);
    }

    VisitNORMSDIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitNORMSINV_fun(context) {
        return this.Visit_fun(context);
    }

    VisitNOT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitNOW_fun(context) {
        return this.Visit_fun(context);
    }

    VisitNULL_fun(context) {
        return this.Visit_fun(context);
    }

    VisitNum(context) {
        return this.Visit_fun(context);
    }

    VisitNUM_fun(context) {
        return this.Visit_fun(context);
    }

    VisitOCT2BIN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitOCT2DEC_fun(context) {
        return this.Visit_fun(context);
    }

    VisitOCT2HEX_fun(context) {
        return this.Visit_fun(context);
    }

    VisitODD_fun(context) {
        return this.Visit_fun(context);
    }

    VisitOR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitParameter2(context) {
        return this.Visit_fun(context);
    }

    VisitPARAMETER_fun(context) {
        return this.Visit_fun(context);
    }

    VisitPARAM_fun(context) {
        return this.Visit_fun(context);
    }

    VisitPercentage_fun(context) {
        return this.Visit_fun(context);
    }

    VisitPERCENTILE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitPERCENTRANK_fun(context) {
        return this.Visit_fun(context);
    }

    VisitPERMUT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitPI_fun(context) {
        return this.Visit_fun(context);
    }

    VisitPOISSON_fun(context) {
        return this.Visit_fun(context);
    }

    VisitPOWER_fun(context) {
        return this.Visit_fun(context);
    }

    VisitPRODUCT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitPROPER_fun(context) {
        return this.Visit_fun(context);
    }

    VisitQUARTILE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitQUOTIENT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitRADIANS_fun(context) {
        return this.Visit_fun(context);
    }

    VisitRANDBETWEEN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitRAND_fun(context) {
        return this.Visit_fun(context);
    }

    VisitREGEXREPALCE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitREGEX_fun(context) {
        return this.Visit_fun(context);
    }

    VisitREMOVEEND_fun(context) {
        return this.Visit_fun(context);
    }

    VisitREMOVESTART_fun(context) {
        return this.Visit_fun(context);
    }

    VisitREPLACE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitREPT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitRIGHT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitRMB_fun(context) {
        return this.Visit_fun(context);
    }

    VisitROUNDDOWN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitROUNDUP_fun(context) {
        return this.Visit_fun(context);
    }

    VisitROUND_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSEARCH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSECOND_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSHA1_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSHA256_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSHA512_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSIGN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSINH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSIN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSMALL_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSPLIT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSQRTPI_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSQRT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSTARTSWITH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSTDEVP_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSTDEV_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSTRING_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSUBSTITUTE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSUBSTRING_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSUMIF_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSUMSQ_fun(context) {
        return this.Visit_fun(context);
    }

    VisitSUM_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTANH_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTAN_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTDIST_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTEXTTOBASE64URL_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTEXTTOBASE64_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTEXT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTIMESTAMP_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTIMEVALUE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTIME_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTINV_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTODAY_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTRIMEND_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTRIMSTART_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTRIM_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTRUE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitTRUNC_fun(context) {
        return this.Visit_fun(context);
    }

    VisitT_fun(context) {
        return this.Visit_fun(context);
    }

    VisitUnit(context) {
        return this.Visit_fun(context);
    }

    VisitUPPER_fun(context) {
        return this.Visit_fun(context);
    }

    VisitURLDECODE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitURLENCODE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitVALUE_fun(context) {
        return this.Visit_fun(context);
    }

    VisitVARP_fun(context) {
        return this.Visit_fun(context);
    }

    VisitVAR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitVersion_fun(context) {
        return this.Visit_fun(context);
    }

    VisitWEEKDAY_fun(context) {
        return this.Visit_fun(context);
    }

    VisitWEEKNUM_fun(context) {
        return this.Visit_fun(context);
    }

    VisitWEIBULL_fun(context) {
        return this.Visit_fun(context);
    }

    VisitWORKDAY_fun(context) {
        return this.Visit_fun(context);
    }

    VisitYEAR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLOOKFLOOR_fun(context) {
        return this.Visit_fun(context);
    }

    VisitLOOKCEILING_fun(context) {
        return this.Visit_fun(context);
    }
}
