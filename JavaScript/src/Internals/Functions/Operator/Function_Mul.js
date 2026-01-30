import { Operand } from '../../../Operand.js';
import { Function_2 } from '../Function_2.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_Mul extends Function_2 {
  constructor(func1, func2) {
    super(func1, func2);
  }

  Evaluate(engine, tempParameter) {
    let args1 = this.func1.Evaluate(engine, tempParameter);
    if (args1.IsError) { return args1; }
    let args2 = this.func2.Evaluate(engine, tempParameter);
    if (args2.IsError) { return args2; }

    if (args1.IsNumber && args2.IsNumber) { //  优化性能
      if (args1.NumberValue === 1) { return args2; }
      if (args2.NumberValue === 1) { return args1; }
      if (args1.NumberValue === 0 || args2.NumberValue === 0) { return Operand.Zero; }
      return Operand.Create(args1.NumberValue * args2.NumberValue);
    }
    if (args1.IsNull) { return Operand.Error('Function \'{0}\' parameter {1} is NULL!', '*', 1); }
    if (args2.IsNull) { return Operand.Error('Function \'{0}\' parameter {1} is NULL!', '*', 2); }

    if (args1.IsText) {
      let d = parseFloat(args1.TextValue);
      if (!isNaN(d)) {
        args1 = Operand.Create(d);
      } else {
        let result = [false];
        if (FunctionUtil.TryParseBoolean(args1.TextValue, result)) {
          let b = result[0];
          args1 = b ? Operand.One : Operand.Zero;
        } else {
          return Operand.Error('Two types cannot be multiplied ');
        }
      }
    }
    if (args2.IsText) {
      let d = parseFloat(args2.TextValue);
      if (!isNaN(d)) {
        args2 = Operand.Create(d);
      } else {
        let result = [false];
        if (FunctionUtil.TryParseBoolean(args2.TextValue, result)) {
          let b = result[0];
          args2 = b ? Operand.One : Operand.Zero;
        } else {
          return Operand.Error('Two types cannot be multiplied');
        }
      }
    }

    if (args1.IsNotNumber) { args1 = args1.ToNumber(StringCache.Function_parameter_error, '*', 1); if (args1.IsError) { return args1; } }
    if (args2.IsNotNumber) { args2 = args2.ToNumber(StringCache.Function_parameter_error, '*', 2); if (args2.IsError) { return args2; } }

    if (args1.NumberValue === 1) { return args2; }
    if (args2.NumberValue === 1) { return args1; }

    return Operand.Create(args1.NumberValue * args2.NumberValue);
  }

  ToString(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.push('(');
    this.func1.ToString(stringBuilder, true);
    stringBuilder.push(' * ');
    this.func2.ToString(stringBuilder, true);
    if (addBrackets) stringBuilder.push(')');
  }
}

export { Function_Mul };