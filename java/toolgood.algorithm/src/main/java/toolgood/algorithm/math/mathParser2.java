package toolgood.algorithm.math;

import java.util.List;
import org.antlr.v4.runtime.ParserRuleContext;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.tree.ParseTreeVisitor;
import org.antlr.v4.runtime.tree.TerminalNode;

public class mathParser2{
	public static final int RULE_prog = 0, RULE_expr = 1, RULE_parameter2 = 2;

    public static class ProgContext extends ParserRuleContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ProgContext(final ParserRuleContext parent, final int invokingState) {
            super(parent, invokingState);
        }
        @Override
        public int getRuleIndex() {
            return RULE_prog;
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitProg(this);
        }
    }
    public static class ExprContext extends ParserRuleContext {
        public ExprContext(final ParserRuleContext parent, final int invokingState) {
            super(parent, invokingState);
        }
        @Override
        public int getRuleIndex() {
            return RULE_expr;
        }
        public ExprContext() {
        }
        public void copyFrom(final ExprContext ctx) {
            super.copyFrom(ctx);
        }
    }
    public static class URLDECODE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public URLDECODE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitURLDECODE_fun(this);
        }
    }
    public static class REGEXREPALCE_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public REGEXREPALCE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitREGEXREPALCE_fun(this);
        }
    }
    public static class AddSub_funContext extends ExprContext {
        public Token op;
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public AddSub_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitAddSub_fun(this);
        }
    }
    public static class ISNULLORERROR_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public ISNULLORERROR_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISNULLORERROR_fun(this);
        }
    }
    public static class RIGHT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public RIGHT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitRIGHT_fun(this);
        }
    }
    public static class VALUE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public VALUE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitVALUE_fun(this);
        }
    }
    public static class DAY_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public DAY_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitDAY_fun(this);
        }
    }
    public static class HMACSHA256_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public HMACSHA256_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHMACSHA256_fun(this);
        }
    }
    public static class Judge_funContext extends ExprContext {
        public Token op;
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public Judge_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitJudge_fun(this);
        }
    }
    public static class OCT2BIN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public OCT2BIN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitOCT2BIN_fun(this);
        }
    }
    public static class MINUTE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public MINUTE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMINUTE_fun(this);
        }
    }
    public static class INDEXOF_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public INDEXOF_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitINDEXOF_fun(this);
        }
    }
    public static class UPPER_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public UPPER_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitUPPER_fun(this);
        }
    }
    public static class HTMLDECODE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public HTMLDECODE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHTMLDECODE_fun(this);
        }
    }
    public static class ISREGEX_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public ISREGEX_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISREGEX_fun(this);
        }
    }
    public static class VLOOKUP_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public VLOOKUP_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitVLOOKUP_fun(this);
        }
    }
    public static class DEC2BIN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public DEC2BIN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitDEC2BIN_fun(this);
        }
    }
    public static class LOOKUP_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public LOOKUP_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLOOKUP_fun(this);
        }
    }
    public static class HEX2DEC_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public HEX2DEC_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHEX2DEC_fun(this);
        }
    }

    public static class SHA256_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SHA256_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSHA256_fun(this);
        }
    }
    public static class TEXTTOBASE64_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public TEXTTOBASE64_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTEXTTOBASE64_fun(this);
        }
    }
    public static class MID_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public MID_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMID_fun(this);
        }
    }
    public static class TRIMSTART_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public TRIMSTART_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTRIMSTART_fun(this);
        }
    }
    public static class RMB_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public RMB_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitRMB_fun(this);
        }
    }
    public static class ISNUMBER_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ISNUMBER_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISNUMBER_fun(this);
        }
    }
    public static class DEC2HEX_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public DEC2HEX_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitDEC2HEX_fun(this);
        }
    }
    public static class CLEAN_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public CLEAN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCLEAN_fun(this);
        }
    }
    public static class LASTINDEXOF_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public LASTINDEXOF_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLASTINDEXOF_fun(this);
        }
    }
    public static class LOWER_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public LOWER_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLOWER_fun(this);
        }
    }
    public static class CHAR_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public CHAR_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCHAR_fun(this);
        }
    }
    public static class REGEX_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public REGEX_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitREGEX_fun(this);
        }
    }
    public static class TEXTTOBASE64URL_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public TEXTTOBASE64URL_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTEXTTOBASE64URL_fun(this);
        }
    }
    public static class MD5_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public MD5_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMD5_fun(this);
        }
    }
    public static class REPLACE_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public REPLACE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitREPLACE_fun(this);
        }
    }
    public static class LEFT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public LEFT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLEFT_fun(this);
        }
    }
    public static class ISODD_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ISODD_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISODD_fun(this);
        }
    }
    public static class ISEVEN_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ISEVEN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISEVEN_fun(this);
        }
    }
    public static class ASC_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ASC_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitASC_fun(this);
        }
    }
    public static class ISERROR_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public ISERROR_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISERROR_fun(this);
        }
    }
    public static class BIN2DEC_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public BIN2DEC_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitBIN2DEC_fun(this);
        }
    }
    public static class JIS_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public JIS_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitJIS_fun(this);
        }
    }
    public static class CRC32_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public CRC32_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCRC32_fun(this);
        }
    }
    public static class HMACMD5_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public HMACMD5_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHMACMD5_fun(this);
        }
    }
    public static class EXACT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public EXACT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitEXACT_fun(this);
        }
    }
    public static class SECOND_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public SECOND_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSECOND_fun(this);
        }
    }
    public static class OCT2HEX_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public OCT2HEX_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitOCT2HEX_fun(this);
        }
    }
    public static class TRIMEND_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public TRIMEND_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTRIMEND_fun(this);
        }
    }
    public static class TRIM_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public TRIM_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTRIM_fun(this);
        }
    }
    public static class ISLOGICAL_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ISLOGICAL_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISLOGICAL_fun(this);
        }
    }
    public static class INT_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public INT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitINT_fun(this);
        }
    }
    public static class HEX2OCT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public HEX2OCT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHEX2OCT_fun(this);
        }
    }
    public static class TEXT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public TEXT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTEXT_fun(this);
        }
    }
    public static class YEAR_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public YEAR_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitYEAR_fun(this);
        }
    }
    public static class BIN2HEX_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public BIN2HEX_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitBIN2HEX_fun(this);
        }
    }
    public static class CONCATENATE_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public CONCATENATE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCONCATENATE_fun(this);
        }
    }
    public static class HTMLENCODE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public HTMLENCODE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHTMLENCODE_fun(this);
        }
    }
    public static class BASE64URLTOTEXT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public BASE64URLTOTEXT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitBASE64URLTOTEXT_fun(this);
        }
    }
    public static class URLENCODE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public URLENCODE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitURLENCODE_fun(this);
        }
    }
    public static class HMACSHA1_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public HMACSHA1_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHMACSHA1_fun(this);
        }
    }
    public static class ENDSWITH_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public ENDSWITH_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitENDSWITH_fun(this);
        }
    }
    public static class ISTEXT_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ISTEXT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISTEXT_fun(this);
        }
    }
    public static class Expr2_funContext extends ExprContext {
        public ExprContext expr2() {
            return getRuleContext(ExprContext.class, 0);
        }
        public Expr2_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitExpr2_fun(this);
        }
    }
    public static class TIMEVALUE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public TIMEVALUE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTIMEVALUE_fun(this);
        }
    }
    public static class JSON_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public JSON_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitJSON_fun(this);
        }
    }
    public static class DEC2OCT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public DEC2OCT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitDEC2OCT_fun(this);
        }
    }
    public static class PROPER_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public PROPER_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitPROPER_fun(this);
        }
    }
	public static class GetJsonValue_funContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
		public Parameter2Context parameter2() {
			return getRuleContext(Parameter2Context.class,0);
		}
		public GetJsonValue_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			return ((mathVisitor<? extends T>)visitor).visitGetJsonValue_fun(this);
		}
	}
    public static class HEX2BIN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public HEX2BIN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHEX2BIN_fun(this);
        }
    }
    public static class HOUR_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public HOUR_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHOUR_fun(this);
        }
    }
    public static class LEN_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public LEN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLEN_fun(this);
        }
    }
    public static class SHA512_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SHA512_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSHA512_fun(this);
        }
    }
    public static class ISNULLORWHITESPACE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ISNULLORWHITESPACE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISNULLORWHITESPACE_fun(this);
        }
    }
    public static class ISNONTEXT_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ISNONTEXT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISNONTEXT_fun(this);
        }
    }
    public static class JOIN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public JOIN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitJOIN_fun(this);
        }
    }
    public static class FIND_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public FIND_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitFIND_fun(this);
        }
    }
    public static class SUBSTITUTE_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SUBSTITUTE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSUBSTITUTE_fun(this);
        }
    }
    public static class REPT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public REPT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitREPT_fun(this);
        }
    }
    public static class ISNULL_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public ISNULL_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISNULL_fun(this);
        }
    }
    public static class OCT2DEC_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public OCT2DEC_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitOCT2DEC_fun(this);
        }
    }
    public static class SEARCH_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SEARCH_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSEARCH_fun(this);
        }
    }
    public static class MulDiv_funContext extends ExprContext {
        public Token op;
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public MulDiv_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMulDiv_fun(this);
        }
    }
    public static class SUBSTRING_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SUBSTRING_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSUBSTRING_fun(this);
        }
    }
    public static class T_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public T_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitT_fun(this);
        }
    }
    public static class BIN2OCT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public BIN2OCT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitBIN2OCT_fun(this);
        }
    }
    public static class HMACSHA512_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public HMACSHA512_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHMACSHA512_fun(this);
        }
    }
    public static class BASE64TOTEXT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public BASE64TOTEXT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitBASE64TOTEXT_fun(this);
        }
    }
    public static class DATEVALUE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public DATEVALUE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitDATEVALUE_fun(this);
        }
    }
    public static class STARTSWITH_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public STARTSWITH_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSTARTSWITH_fun(this);
        }
    }
    public static class ISNULLOREMPTY_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ISNULLOREMPTY_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitISNULLOREMPTY_fun(this);
        }
    }
    public static class AndOr_funContext extends ExprContext {
        public Token op;
        public ExprContext expr(int i) {
			return getRuleContext(ExprContext.class,i);
		}
        public AndOr_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitAndOr_fun(this);
        }
    }
    public static class SHA1_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SHA1_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSHA1_fun(this);
        }
    }
    public static class REMOVEEND_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public REMOVEEND_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitREMOVEEND_fun(this);
        }
    }
    public static class SPLIT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SPLIT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSPLIT_fun(this);
        }
    }
    public static class DAYS360_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public DAYS360_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitDAYS360_fun(this);
        }
    }
    public static class ATAN2_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public ATAN2_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitATAN2_fun(this);
        }
    }
    public static class SIGN_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public SIGN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSIGN_fun(this);
        }
    }


    public static class FIXED_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public FIXED_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitFIXED_fun(this);
        }
    }
    public static class MROUND_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public MROUND_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMROUND_fun(this);
        }
    }
    public static class COMBIN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public COMBIN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCOMBIN_fun(this);
        }
    }
    public static class SUMIF_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SUMIF_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSUMIF_fun(this);
        }
    }
    public static class EOMONTH_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public EOMONTH_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitEOMONTH_fun(this);
        }
    }
    public static class EVEN_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public EVEN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitEVEN_fun(this);
        }
    }
    public static class TODAY_funContext extends ExprContext {
        public TODAY_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTODAY_fun(this);
        }
    }
    public static class BINOMDIST_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public BINOMDIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitBINOMDIST_fun(this);
        }
    }
    public static class VARP_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public VARP_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitVARP_fun(this);
        }
    }
    public static class BETAINV_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public BETAINV_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitBETAINV_fun(this);
        }
    }
    public static class FLOOR_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public FLOOR_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitFLOOR_fun(this);
        }
    }
    public static class AVERAGE_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public AVERAGE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitAVERAGE_fun(this);
        }
    }
    public static class TAN_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public TAN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTAN_fun(this);
        }
    }
    public static class FALSE_funContext extends ExprContext {
        public FALSE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitFALSE_fun(this);
        }
    }
    public static class PERCENTILE_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public PERCENTILE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitPERCENTILE_fun(this);
        }
    }
    public static class NULL_funContext extends ExprContext {
        public NULL_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitNULL_fun(this);
        }
    }
    public static class MEDIAN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public MEDIAN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMEDIAN_fun(this);
        }
    }
    public static class PERMUT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public PERMUT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitPERMUT_fun(this);
        }
    }
    public static class Array_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public Array_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitArray_fun(this);
        }
    }
    public static class PRODUCT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public PRODUCT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitPRODUCT_fun(this);
        }
    }
    public static class COS_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public COS_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCOS_fun(this);
        }
    }
    public static class MIN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public MIN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMIN_fun(this);
        }
    }
    public static class MAX_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public MAX_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMAX_fun(this);
        }
    }
    public static class FISHERINV_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public FISHERINV_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitFISHERINV_fun(this);
        }
    }
    public static class ROUNDUP_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public ROUNDUP_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitROUNDUP_fun(this);
        }
    }
    public static class ROUNDDOWN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public ROUNDDOWN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitROUNDDOWN_fun(this);
        }
    }
    public static class LOGINV_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public LOGINV_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLOGINV_fun(this);
        }
    }
    public static class AVEDEV_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public AVEDEV_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitAVEDEV_fun(this);
        }
    }
    public static class ATANH_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ATANH_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitATANH_fun(this);
        }
    }
    public static class TRUE_funContext extends ExprContext {
        public TRUE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTRUE_fun(this);
        }
    }
    public static class WORKDAY_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public WORKDAY_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitWORKDAY_fun(this);
        }
    }
    public static class AVERAGEIF_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public AVERAGEIF_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitAVERAGEIF_fun(this);
        }
    }
    public static class LCM_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public LCM_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLCM_fun(this);
        }
    }
    public static class HYPGEOMDIST_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public HYPGEOMDIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHYPGEOMDIST_fun(this);
        }
    }
    public static class SQRT_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public SQRT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSQRT_fun(this);
        }
    }
    public static class GCD_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public GCD_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitGCD_fun(this);
        }
    }
    public static class CEILING_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public CEILING_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCEILING_fun(this);
        }
    }
    public static class NORMDIST_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public NORMDIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitNORMDIST_fun(this);
        }
    }
    public static class REMOVESTART_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public REMOVESTART_funContext(ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitREMOVESTART_fun(this);
        }
    }
    public static class LOG_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public LOG_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLOG_fun(this);
        }
    }
    public static class MONTH_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public MONTH_funContext(ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMONTH_fun(this);
        }
    }
    public static class BETADIST_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public BETADIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitBETADIST_fun(this);
        }
    }
    public static class HARMEAN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public HARMEAN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitHARMEAN_fun(this);
        }
    }
    public static class FINV_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public FINV_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitFINV_fun(this);
        }
    }
    public static class SUMSQ_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SUMSQ_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSUMSQ_fun(this);
        }
    }
    public static class ACOS_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ACOS_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitACOS_fun(this);
        }
    }
    public static class ROUND_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public ROUND_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitROUND_fun(this);
        }
    }
    public static class NETWORKDAYS_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public NETWORKDAYS_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitNETWORKDAYS_fun(this);
        }
    }
    public static class TRUNC_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public TRUNC_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTRUNC_fun(this);
        }
    }
    public static class ACOSH_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ACOSH_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitACOSH_fun(this);
        }
    }
    public static class LOG10_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public LOG10_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLOG10_fun(this);
        }
    }
    public static class LOGNORMDIST_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public LOGNORMDIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLOGNORMDIST_fun(this);
        }
    }
    public static class MODE_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public MODE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMODE_fun(this);
        }
    }

    public static class Bracket_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public Bracket_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitBracket_fun(this);
        }
    }
    public static class TIME_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public TIME_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTIME_fun(this);
        }
    }
    public static class SQRTPI_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public SQRTPI_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSQRTPI_fun(this);
        }
    }
    public static class FDIST_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public FDIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitFDIST_fun(this);
        }
    }
    public static class ODD_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ODD_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitODD_fun(this);
        }
    }
    public static class COUNT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public COUNT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCOUNT_fun(this);
        }
    }
    public static class MOD_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public MOD_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMOD_fun(this);
        }
    }
    public static class SMALL_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SMALL_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSMALL_fun(this);
        }
    }
    public static class IF_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public IF_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitIF_fun(this);
        }
    }
    public static class GAMMAINV_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public GAMMAINV_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitGAMMAINV_fun(this);
        }
    }
    public static class ABS_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ABS_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitABS_fun(this);
        }
    }

    public static class PI_funContext extends ExprContext {
        public PI_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitPI_fun(this);
        }
    }
    public static class WEEKDAY_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public WEEKDAY_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitWEEKDAY_fun(this);
        }
    }
    public static class COSH_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public COSH_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCOSH_fun(this);
        }
    }
    public static class EDATE_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public EDATE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitEDATE_fun(this);
        }
    }
    public static class IFERROR_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public IFERROR_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitIFERROR_fun(this);
        }
    }
    public static class OR_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public OR_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitOR_fun(this);
        }
    }
    public static class CODE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public CODE_funContext(ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCODE_fun(this);
        }
    }
    public static class COUNTIF_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public COUNTIF_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitCOUNTIF_fun(this);
        }
    }
    public static class NORMSDIST_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public NORMSDIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitNORMSDIST_fun(this);
        }
    }
    public static class PERCENTRANK_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public PERCENTRANK_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitPERCENTRANK_fun(this);
        }
    }
    public static class DEGREES_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public DEGREES_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitDEGREES_fun(this);
        }
    }
    public static class RADIANS_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public RADIANS_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitRADIANS_fun(this);
        }
    }
    public static class GUID_funContext extends ExprContext {
        public GUID_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitGUID_fun(this);
        }
    }
    public static class EXP_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public EXP_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitEXP_fun(this);
        }
    }
    public static class POWER_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public POWER_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitPOWER_fun(this);
        }
    }
    public static class MULTINOMIAL_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public MULTINOMIAL_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitMULTINOMIAL_fun(this);
        }
    }
    public static class TDIST_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public TDIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTDIST_fun(this);
        }
    }
    public static class FACT_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public FACT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitFACT_fun(this);
        }
    }
    public static class EXPONDIST_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public EXPONDIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitEXPONDIST_fun(this);
        }
    }
    public static class POISSON_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public POISSON_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitPOISSON_fun(this);
        }
    }
    public static class E_funContext extends ExprContext {
        public E_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitE_fun(this);
        }
    }
    public static class SUM_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public SUM_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSUM_fun(this);
        }
    }
    public static class LARGE_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public LARGE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLARGE_fun(this);
        }
    }
    public static class TINV_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public TINV_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTINV_fun(this);
        }
    }
    public static class GEOMEAN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public GEOMEAN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitGEOMEAN_fun(this);
        }
    }
    public static class STDEVP_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public STDEVP_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSTDEVP_fun(this);
        }
    }
    public static class ASIN_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ASIN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitASIN_fun(this);
        }
    }
    public static class WEEKNUM_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public WEEKNUM_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitWEEKNUM_fun(this);
        }
    }
    public static class DATEDIF_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public DATEDIF_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitDATEDIF_fun(this);
        }
    }
    public static class NORMSINV_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public NORMSINV_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitNORMSINV_fun(this);
        }
    }
    public static class GAMMADIST_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public GAMMADIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitGAMMADIST_fun(this);
        }
    }
    public static class NEGBINOMDIST_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public NEGBINOMDIST_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitNEGBINOMDIST_fun(this);
        }
    }
    public static class NORMINV_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public NORMINV_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitNORMINV_fun(this);
        }
    }
    public static class STDEV_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public STDEV_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSTDEV_fun(this);
        }
    }
    public static class WEIBULL_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public WEIBULL_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitWEIBULL_fun(this);
        }
    }
    public static class ASINH_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ASINH_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitASINH_fun(this);
        }
    }
    public static class VAR_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public VAR_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitVAR_fun(this);
        }
    }
    public static class FACTDOUBLE_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public FACTDOUBLE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitFACTDOUBLE_fun(this);
        }
    }
    public static class GAMMALN_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public GAMMALN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitGAMMALN_fun(this);
        }
    }
    public static class QUOTIENT_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public QUOTIENT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitQUOTIENT_fun(this);
        }
    }
    public static class DEVSQ_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public DEVSQ_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitDEVSQ_fun(this);
        }
    }
    public static class RAND_funContext extends ExprContext {
        public RAND_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitRAND_fun(this);
        }
    }
    public static class TANH_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public TANH_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitTANH_fun(this);
        }
    }
    public static class FISHER_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public FISHER_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitFISHER_fun(this);
        }
    }
    public static class ATAN_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public ATAN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitATAN_fun(this);
        }
    }
    public static class LN_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public LN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitLN_fun(this);
        }
    }
    public static class RANDBETWEEN_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public RANDBETWEEN_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitRANDBETWEEN_fun(this);
        }
    }
    public static class NOW_funContext extends ExprContext {
        public NOW_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitNOW_fun(this);
        }
    }
    public static class QUARTILE_funContext extends ExprContext {
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public QUARTILE_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitQUARTILE_fun(this);
        }
    }
    public static class Parameter2Context extends ParserRuleContext {
        public Parameter2Context(final ParserRuleContext parent, final int invokingState) {
            super(parent, invokingState);
        }
        @Override
        public int getRuleIndex() {
            return RULE_parameter2;
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
        return ((mathVisitor<? extends T>)visitor).visitParameter2(this);
        }
    }
    public static class SIN_funContext extends ExprContext {
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public SIN_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			return ((mathVisitor<? extends T>)visitor).visitSIN_fun(this);
		}
    }
	public static class DATE_funContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public DATE_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			return ((mathVisitor<? extends T>)visitor).visitDATE_fun(this);
		}
    }
    public static class AND_funContext extends ExprContext {
		public List<ExprContext> expr() {
			return getRuleContexts(ExprContext.class);
		}
		public AND_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			return ((mathVisitor<? extends T>)visitor).visitAND_fun(this);
		}
    }
    public static class SINH_funContext extends ExprContext {
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public SINH_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			return ((mathVisitor<? extends T>)visitor).visitSINH_fun(this);
		}
	}

	public static class Percentage_funContext extends ExprContext {
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public Percentage_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			return ((mathVisitor<? extends T>)visitor).visitPercentage_fun(this);
		}
	}
    public static class NOT_funContext extends ExprContext {
        public ExprContext expr() {
            return getRuleContext(ExprContext.class, 0);
        }
        public NOT_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitNOT_fun(this);
        }
    }
 
    public static class STRING_funContext extends ExprContext {
        public TerminalNode STRING() { return getToken(mathParser.STRING, 0); }
        public STRING_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitSTRING_fun(this);
        }
    }
 
    public static class NUM_funContext extends ExprContext {
        public TerminalNode NUM() { return getToken(mathParser.NUM, 0); }
		public TerminalNode SUB() { return getToken(mathParser.SUB, 0); }
        public NUM_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitNUM_fun(this);
        }
    }
	public static class PARAMETER_funContext extends ExprContext {
		public TerminalNode PARAMETER() { return getToken(mathParser.PARAMETER, 0); }
		public ExprContext expr() {
			return getRuleContext(ExprContext.class,0);
		}
		public TerminalNode PARAMETER2() { return getToken(mathParser.PARAMETER2, 0); }
		public PARAMETER_funContext(ExprContext ctx) { copyFrom(ctx); }
		@Override
		public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
			return ((mathVisitor<? extends T>)visitor).visitPARAMETER_fun(this);
		}
	}
 
    public static class DiyFunction_funContext extends ExprContext {
        public TerminalNode PARAMETER() { return getToken(mathParser.PARAMETER, 0); }
        public List<ExprContext> expr() {
            return getRuleContexts(ExprContext.class);
        }
        public DiyFunction_funContext(final ExprContext ctx) {
            copyFrom(ctx);
        }
        @Override
        public <T> T accept(final ParseTreeVisitor<? extends T> visitor) {
            return ((mathVisitor<? extends T>) visitor).visitDiyFunction_fun(this);
        }
    }
     
}