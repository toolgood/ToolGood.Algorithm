import { Operand } from './Operand.js';
import { DistanceUnitType, AreaUnitType, VolumeUnitType, MassUnitType } from './Enums/index.js';
import InputStream from './antlr4/InputStream.js';
import CommonTokenStream from './antlr4/CommonTokenStream.js';
import mathLexer from './math/mathLexer.js';
import mathParser from './math/mathParser.js';
import { AntlrErrorTextWriter } from './Internals/Visitors/AntlrErrorTextWriter.js';
import { MathFunctionVisitor } from './Internals/Visitors/MathFunctionVisitor.js';

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
    const antlrErrorTextWriter = new AntlrErrorTextWriter();
    const stream = new InputStream(exp);
    const lexer = new mathLexer(stream, null, antlrErrorTextWriter);
    const tokens = new CommonTokenStream(lexer);
    const parser = new mathParser(tokens, null, antlrErrorTextWriter);

    const context = parser.prog();
    if (antlrErrorTextWriter.IsError) {
      this.LastError = antlrErrorTextWriter.ErrorMsg;
      throw new Error(this.LastError);
    }
    const visitor = new MathFunctionVisitor();
    return visitor.Visit(context);
  }

  Evaluate(functionObj) {
    return functionObj.Evaluate(this);
  }

  TryEvaluate_UInt16(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        const converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.IntValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_UInt32(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        const converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.IntValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_UInt64(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        const converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.LongValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_Int16(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        const converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.IntValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_Int32(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        const converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.IntValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_Int64(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        const converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.LongValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_Single(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        const converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.DoubleValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_Double(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        const converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.DoubleValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_Decimal(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotNumber) {
        const converted = obj.ToNumber("It can't be converted to number!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.NumberValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_String(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotText) {
        const converted = obj.ToText("It can't be converted to string!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.TextValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_Boolean(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotBoolean) {
        const converted = obj.ToBoolean("It can't be converted to bool!");
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
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotDate) {
        const converted = obj.ToMyDate("It can't be converted to DateTime!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      if (this.UseLocalTime) {
        return obj.DateValue.ToDateTime(Date.prototype.constructor.prototype._Kind || 0);
      }
      return obj.DateValue.ToDateTime(0);
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }

  TryEvaluate_TimeSpan(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotDate) {
        const converted = obj.ToMyDate("It can't be converted to DateTime!");
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

  TryEvaluate_MyDate(exp, def) {
    try {
      const functionObj = this.Parse(exp);
      const obj = functionObj.Evaluate(this);
      if (obj.IsNotDate) {
        const converted = obj.ToMyDate("It can't be converted to DateTime!");
        if (converted.IsError) {
          this.LastError = converted.ErrorMsg;
          return def;
        }
      }
      return obj.DateValue;
    } catch (ex) {
      this.LastError = ex.message + '\n' + ex.stack;
    }
    return def;
  }
}

export { AlgorithmEngine };