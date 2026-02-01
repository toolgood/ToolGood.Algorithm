import { Operand } from './Operand.js';
import { DistanceUnitType, AreaUnitType, VolumeUnitType, MassUnitType } from './Enums/index.js';
import CommonTokenStream from './antlr4/CommonTokenStream.js';
import mathLexer from './math/mathLexer.js';
import mathParser from './math/mathParser.js';
import { AntlrErrorTextWriter } from './Internals/Visitors/AntlrErrorTextWriter.js';
import { MathFunctionVisitor } from './Internals/Visitors/MathFunctionVisitor.js';
import { AntlrCharStream } from './Internals/Visitors/AntlrCharStream.js';


class AlgorithmEngine {
  constructor() {
    this.ExcelIndex = 1;
    this.UseLocalTime = true;
    this.DistanceUnit = DistanceUnitType.M;
    this.AreaUnit = AreaUnitType.M2;
    this.VolumeUnit = VolumeUnitType.M3;
    this.MassUnit = MassUnitType.KG;
    this.LastError = null;
  }

  set UseExcelIndex(value) {
    this.ExcelIndex = value ? 1 : 0;
  }

  GetParameter(parameter) {
    return Operand.Error(`Parameter [${parameter}] is missing.`);
  }

  ExecuteDiyFunction(parameter, args) {
    return Operand.Error(`DiyFunction [${parameter}] is missing.`);
  }

  Parse(exp) {
    this.LastError = null;
    if (!exp || exp.trim() === '') {
      this.LastError = 'Parameter exp invalid !';
      throw new Error(this.LastError);
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
      this.LastError = antlrErrorTextWriter.ErrorMsg;
      throw new Error(this.LastError);
    }
    let visitor = new MathFunctionVisitor();
    return visitor.visit(context);
  }

  Evaluate(functionObj) {
    return functionObj.Evaluate(this);
  }

  TryEvaluate_Int(exp, def) {
    try {
      let functionObj = this.Parse(exp);
      let obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        let converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
        obj = converted;
      }
      return obj.IntValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_Double(exp, def) {
    try {
      let functionObj = this.Parse(exp);
      let obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        let converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
        obj = converted;
      }
      return obj.NumberValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_String(exp, def) {
    try {
      let functionObj = this.Parse(exp);
      let obj = functionObj.Evaluate(this);
      if (obj.IsNotText) {
        let converted = obj.ToText("It can't be converted to string!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
        return converted.TextValue;
      }
      return obj.TextValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_Boolean(exp, def) {
    try {
      let functionObj = this.Parse(exp);
      let obj = functionObj.Evaluate(this);
      if (obj.IsNotBoolean) {
        let converted = obj.ToBoolean("It can't be converted to bool!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.BooleanValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_DateTime(exp, def) {
    try {
      let functionObj = this.Parse(exp);
      let obj = functionObj.Evaluate(this);
      if (obj.IsNotDate) {
        let converted = obj.ToMyDate("It can't be converted to DateTime!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      if (this.UseLocalTime) {
        return obj.DateValue.ToDateTime(1);
      }
      return obj.DateValue.ToDateTime(0);
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_TimeSpan(exp, def) {
    try {
      let functionObj = this.Parse(exp);
      let obj = functionObj.Evaluate(this);
      if (obj.IsNotDate) {
        let converted = obj.ToMyDate("It can't be converted to DateTime!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.DateValue.ToTimeSpan();
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }
}

export { AlgorithmEngine };