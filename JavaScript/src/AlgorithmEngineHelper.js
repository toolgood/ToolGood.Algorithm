import { CharUtil } from './Internals/Visitors/CharUtil.js';
import { AntlrCharStream } from './Internals/Visitors/AntlrCharStream.js';
import { AntlrErrorTextWriter } from './Internals/Visitors/AntlrErrorTextWriter.js';
import { DiyNameVisitor } from './Internals/Visitors/DiyNameVisitor.js';
import { MathFunctionVisitor } from './Internals/Visitors/MathFunctionVisitor.js';
import { MathSplitVisitor } from './Internals/Visitors/MathSplitVisitor.js';
import { MathSplitVisitor2 } from './Internals/Visitors/MathSplitVisitor2.js';
import { Function_AND } from './Internals/Functions/Operator/Function_AND.js';
import { Function_OR } from './Internals/Functions/Operator/Function_OR.js';
import { Function_Add } from './Internals/Functions/Operator/Function_Add.js';
import { Function_Sub } from './Internals/Functions/Operator/Function_Sub.js';
import { Function_Mul } from './Internals/Functions/Operator/Function_Mul.js';
import { Function_Div } from './Internals/Functions/Operator/Function_Div.js';
import { Function_Mod } from './Internals/Functions/Operator/Function_Mod.js';
import { Function_Connect } from './Internals/Functions/Operator/Function_Connect.js';
import { DistanceConverter } from './UnitConversion/DistanceConverter.js';
import { MassConverter } from './UnitConversion/MassConverter.js';
import { AreaConverter } from './UnitConversion/AreaConverter.js';
import { VolumeConverter } from './UnitConversion/VolumeConverter.js';
import { ConditionTreeType } from './Enums/ConditionTreeType.js';
import { CalculateTreeType } from './Enums/CalculateTreeType.js';

// 导入ANTLR生成的文件
import mathLexer from './math/mathLexer.js';
import CommonTokenStream from './antlr4/CommonTokenStream.js';
import mathParser from './math/mathParser.js';

/**
 * 算法引擎助手
 */
export class AlgorithmEngineHelper {
    static _lexerSet = null;
    static unitRegex = null;

    static GetLexerSet() {
        if (!this._lexerSet) {
            this._lexerSet = new Set([
                "NULL",
                "IF",
                "IFERROR",
                "ISNUMBER",
                "ISTEXT",
                "ISERROR",
                "ISNONTEXT",
                "ISLOGICAL",
                "ISEVEN",
                "ISODD",
                "ISNULL",
                "ISNULLORERROR",
                "AND",
                "OR",
                "NOT",
                "TRUE",
                "FALSE",
                "E",
                "PI",
                "DEC2BIN",
                "DEC2HEX",
                "DEC2OCT",
                "HEX2BIN",
                "HEX2DEC",
                "HEX2OCT",
                "OCT2BIN",
                "OCT2DEC",
                "OCT2HEX",
                "BIN2OCT",
                "BIN2DEC",
                "BIN2HEX",
                "ABS",
                "QUOTIENT",
                "MOD",
                "SIGN",
                "SQRT",
                "TRUNC",
                "INT",
                "GCD",
                "LCM",
                "COMBIN",
                "PERMUT",
                "DEGREES",
                "RADIANS",
                "COS",
                "COSH",
                "SIN",
                "SINH",
                "TAN",
                "TANH",
                "ACOS",
                "ACOSH",
                "ASIN",
                "ASINH",
                "ATAN",
                "ATANH",
                "ATAN2",
                "ROUND",
                "ROUNDDOWN",
                "ROUNDUP",
                "CEILING",
                "FLOOR",
                "EVEN",
                "ODD",
                "MROUND",
                "RAND",
                "RANDBETWEEN",
                "FACT",
                "FACTDOUBLE",
                "POWER",
                "EXP",
                "LN",
                "LOG",
                "LOG10",
                "MULTINOMIAL",
                "PRODUCT",
                "SQRTPI",
                "SUMSQ",
                "ASC",
                "JIS",
                "WIDECHAR",
                "CHAR",
                "CLEAN",
                "CODE",
                "CONCATENATE",
                "EXACT",
                "FIND",
                "FIXED",
                "LEFT",
                "LEN",
                "LOWER", "TOLOWER",
                "MID",
                "PROPER",
                "REPLACE",
                "REPT",
                "RIGHT",
                "RMB",
                "SEARCH",
                "SUBSTITUTE",
                "T",
                "TEXT",
                "TRIM",
                "UPPER", "TOUPPER",
                "VALUE",
                "DATEVALUE",
                "TIMEVALUE",
                "DATE",
                "TIME",
                "NOW",
                "TODAY",
                "YEAR",
                "MONTH",
                "DAY",
                "HOUR",
                "MINUTE",
                "SECOND",
                "WEEKDAY",
                "DATEDIF",
                "DAYS360",
                "EDATE",
                "EOMONTH",
                "NETWORKDAYS",
                "WORKDAY",
                "WEEKNUM",
                "MAX",
                "MEDIAN",
                "MIN",
                "QUARTILE",
                "MODE",
                "LARGE",
                "SMALL",
                "PERCENTILE", "PERCENTILE.INC",
                "PERCENTRANK", "PERCENTRANK.INC",
                "AVERAGE",
                "AVERAGEIF",
                "GEOMEAN",
                "HARMEAN",
                "COUNT",
                "COUNTIF",
                "SUM",
                "SUMIF",
                "AVEDEV",
                "STDEV", "STDEV.S",
                "STDEVP", "STDEV.P",
                "COVAR",
                "COVARIANCE.P",
                "COVARIANCE.S",
                "DEVSQ",
                "VAR", "VAR.S",
                "VARP", "VAR.P",
                "NORMDIST", "NORM.DIST",
                "NORMINV", "NORM.INV",
                "NORMSDIST", "NORM.S.DIST",
                "NORMSINV", "NORM.S.INV",
                "BETADIST", "BETA.DIST",
                "BETAINV", "BETA.INV",
                "BINOMDIST", "BINOM.DIST",
                "EXPONDIST", "EXPON.DIST",
                "FDIST", "F.DIST",
                "FINV", "F.INV",
                "FISHER",
                "FISHERINV",
                "GAMMADIST", "GAMMA.DIST",
                "GAMMAINV", "GAMMA.INV",
                "GAMMALN", "GAMMALN.PRECISE",
                "HYPGEOMDIST", "HYPGEOM.DIST",
                "LOGINV", "LOGNORM.INV",
                "LOGNORMDIST", "LOGNORM.DIST",
                "NEGBINOMDIST", "NEGBINOM.DIST",
                "POISSON", "POISSON.DIST",
                "TDIST", "T.DIST",
                "TINV", "T.INV",
                "WEIBULL",
                "URLENCODE",
                "URLDECODE",
                "HTMLENCODE",
                "HTMLDECODE",
                "BASE64TOTEXT",
                "BASE64URLTOTEXT",
                "TEXTTOBASE64",
                "TEXTTOBASE64URL",
                "REGEX",
                "REGEXREPALCE",
                "ISREGEX", "ISMATCH",
                "GUID",
                "MD5",
                "SHA1",
                "SHA256",
                "SHA512",
                "CRC32",
                "HMACMD5",
                "HMACSHA1",
                "HMACSHA256",
                "HMACSHA512",
                "TRIMSTART", "LTRIM",
                "TRIMEND", "RTRIM",
                "INDEXOF",
                "LASTINDEXOF",
                "SPLIT",
                "JOIN",
                "SUBSTRING",
                "STARTSWITH",
                "ENDSWITH",
                "ISNULLOREMPTY",
                "ISNULLORWHITESPACE",
                "REMOVESTART",
                "REMOVEEND",
                "JSON",
                "ARRAY",
                "LOOKFLOOR",
                "LOOKCEILING",
                "ADDYEARS",
                "ADDMONTHS",
                "ADDDAYS",
                "ADDHOURS",
                "ADDMINUTES",
                "ADDSECONDS",
                "TIMESTAMP",
                "HAS", "HASKEY",
                "CONTAINS", "CONTAINSKEY",
                "HASVALUE", "CONTAINSVALUE",
                "PARAM", "PARAMETER", "GetParameter",
                "ERROR"
            ]);
        }
        return this._lexerSet;
    }

    /**
     * 是否与内置关键字相同
     */
    static IsKeywords(parameter) {
        let lexerSet = this.GetLexerSet();
        return lexerSet.has(CharUtil.standardString(parameter));
    }

    /**
     * 获取 DIY 名称
     */
    static GetDiyNames(exp) {
        if (!exp || exp.trim() === '') {
            throw new Error("Parameter exp invalid !");
        }
        let antlrErrorTextWriter = new AntlrErrorTextWriter();
        let stream =new AntlrCharStream(exp);
        let lexer = new mathLexer(stream, null, antlrErrorTextWriter);
        lexer.removeErrorListeners();
        lexer.addErrorListener(antlrErrorTextWriter);
        let tokens = new CommonTokenStream(lexer);
        let parser = new mathParser(tokens, null, antlrErrorTextWriter);
        parser.removeErrorListeners();
        parser.addErrorListener(antlrErrorTextWriter);

        let context = parser.prog();
        if (antlrErrorTextWriter.IsError) {
            throw new Error(antlrErrorTextWriter.ErrorMsg);
        }
        let visitor = new DiyNameVisitor();
        visitor.visit(context);
        return visitor.diy;
    }

    /**
     * 单位转换
     */
    static UnitConversion(src, oldSrcUnit, oldTarUnit, name = null) {
        if (!oldSrcUnit || !oldTarUnit) { return src; }
        if (!this.unitRegex) {
            this.unitRegex = /[\s \(\)（）\[\]<>]/g;
        }
        oldSrcUnit = oldSrcUnit.replace(this.unitRegex, "");
        if (oldSrcUnit === oldTarUnit) { return src; }

        if (DistanceConverter.Exists(oldSrcUnit, oldTarUnit)) {
            let c = new DistanceConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (MassConverter.Exists(oldSrcUnit, oldTarUnit)) {
            let c = new MassConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (AreaConverter.Exists(oldSrcUnit, oldTarUnit)) {
            let c = new AreaConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (VolumeConverter.Exists(oldSrcUnit, oldTarUnit)) {
            let c = new VolumeConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (!name) {
            throw new Error(`The input item has different units and cannot be converted from [${oldSrcUnit}] to [${oldTarUnit}]`);
        }
        throw new Error(`The input item [${name}] has different units and cannot be converted from [${oldSrcUnit}] to [${oldTarUnit}]`);
    }

    /**
     * 编译公式
     */
    static ParseFormula(exp,errorListener) {
        if (!exp || exp.trim() === '') {
            throw new Error("Parameter exp invalid !");
        }
        let antlrErrorTextWriter = new AntlrErrorTextWriter();
        let stream =new AntlrCharStream(exp);
        let lexer = new mathLexer(stream, null, antlrErrorTextWriter);
        lexer.removeErrorListeners();
        lexer.addErrorListener(antlrErrorTextWriter);
        let tokens = new CommonTokenStream(lexer);
        let parser = new mathParser(tokens, null, antlrErrorTextWriter);
        parser.removeErrorListeners();
        parser.addErrorListener(antlrErrorTextWriter);
        if (errorListener) {
            parser.addErrorListener(errorListener);
            let context = parser.prog();
            let visitor = new MathFunctionVisitor();
            return visitor.visit(context);
        }
        let context = parser.prog();
        if (antlrErrorTextWriter.IsError) {
            throw new Error(antlrErrorTextWriter.ErrorMsg);
        }
        let visitor = new MathFunctionVisitor();
        return visitor.visit(context);
    }

    /**
     * 检查公式是否正确
     */
    static CheckFormula(exp) {
        if (!exp || exp.trim() === '') { return false; }
        let antlrErrorTextWriter = new AntlrErrorTextWriter();
        let stream =new AntlrCharStream(exp);
        let lexer = new mathLexer(stream, null, antlrErrorTextWriter);
        lexer.removeErrorListeners();
        lexer.addErrorListener(antlrErrorTextWriter);
        let tokens = new CommonTokenStream(lexer);
        let parser = new mathParser(tokens, null, antlrErrorTextWriter);
        parser.removeErrorListeners();
        parser.addErrorListener(antlrErrorTextWriter);

        let context = parser.prog();
        if (antlrErrorTextWriter.IsError) {
            return false;
        }
        return true;
    }

    /**
     * 解析条件
     */
    static ParseCondition(condition) {
        let tree = {
            Type: null,
            ErrorMessage: null
        };
        if (!condition || condition.trim() === '') {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = "condition is null";
            return tree;
        }
        try {
            let antlrErrorTextWriter = new AntlrErrorTextWriter();
            let stream =new AntlrCharStream(condition);
            let lexer = new mathLexer(stream, null, antlrErrorTextWriter);
            lexer.removeErrorListeners();
            lexer.addErrorListener(antlrErrorTextWriter);
            let tokens = new CommonTokenStream(lexer);
            let parser = new mathParser(tokens, null, antlrErrorTextWriter);
            parser.removeErrorListeners();
            parser.addErrorListener(antlrErrorTextWriter);

            let context = parser.prog();
            if (antlrErrorTextWriter.IsError) {
                tree.Type = ConditionTreeType.Error;
                tree.ErrorMessage = antlrErrorTextWriter.ErrorMsg;
                return tree;
            }
            let visitor = new MathSplitVisitor();
            return visitor.visit(context);
        } catch (ex) {
            tree.Type = ConditionTreeType.Error;
            tree.ErrorMessage = ex.message;
        }
        return tree;
    }

    /**
     * Creates a logical AND function that combines two specified functions.
     */
    static Condition_And(left, right) {
        return new Function_AND(left, right);
    }

    /**
     * Creates a logical OR function that combines two specified functions.
     */
    static Condition_Or(left, right) {
        return new Function_OR(left, right);
    }

    /**
     * 解析计算表达式
     */
    static ParseCalculate(exp) {
        let tree = {
            Type: null,
            ErrorMessage: null
        };
        if (!exp || exp.trim() === '') {
            tree.Type = CalculateTreeType.Error;
            tree.ErrorMessage = "exp is null";
            return tree;
        }
        try {
            let antlrErrorTextWriter = new AntlrErrorTextWriter();
            let stream =new AntlrCharStream(exp);
            let lexer = new mathLexer(stream, null, antlrErrorTextWriter);
            lexer.removeErrorListeners();
            lexer.addErrorListener(antlrErrorTextWriter);
            let tokens = new CommonTokenStream(lexer);
            let parser = new mathParser(tokens, null, antlrErrorTextWriter);
            parser.removeErrorListeners();
            parser.addErrorListener(antlrErrorTextWriter);

            let context = parser.prog();
            if (antlrErrorTextWriter.IsError) {
                tree.Type = CalculateTreeType.Error;
                tree.ErrorMessage = antlrErrorTextWriter.ErrorMsg;
                return tree;
            }
            let visitor = new MathSplitVisitor2();
            return visitor.visit(context);
        } catch (ex) {
            tree.Type = CalculateTreeType.Error;
            tree.ErrorMessage = ex.message;
        }
        return tree;
    }

    /**
     * Creates a function that represents the sum of two specified functions.
     */
    static Calculate_Add(left, right) {
        return new Function_Add(left, right);
    }

    /**
     * Creates a function that represents the subtraction of two functions.
     */
    static Calculate_Subtract(left, right) {
        return new Function_Sub(left, right);
    }

    /**
     * Creates a function that represents the multiplication of two functions.
     */
    static Calculate_Multiply(left, right) {
        return new Function_Mul(left, right);
    }

    /**
     * Creates a function that represents the division of two functions.
     */
    static Calculate_Divide(left, right) {
        return new Function_Div(left, right);
    }

    /**
     * Creates a function that computes the remainder after dividing the result of the left function by the result of the
     * right function.
     */
    static Calculate_Mod(left, right) {
        return new Function_Mod(left, right);
    }

    /**
     * Creates a new function that represents the connection of two functions.
     */
    static Calculate_Connect(left, right) {
        return new Function_Connect(left, right);
    }
}

// 浏览器支持
if (typeof window !== 'undefined') {
    window.AlgorithmEngineHelper = AlgorithmEngineHelper;
}