import { Operand } from '../../../Operand.js';
import { Function_2 } from '../Function_2.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_Div extends Function_2 {
  constructor(z) {
    super(z);
  }

  Evaluate(engine, tempParameter) {
    let args1 = this.a.Evaluate(engine, tempParameter);
    if (args1.IsError) { return args1; }
    let args2 = this.b.Evaluate(engine, tempParameter);
    if (args2.IsError) { return args2; }

    if (args1.IsNumber && args2.IsNumber) { //  优化性能
      if (args2.NumberValue === 1) { return args1; }
      if (args2.NumberValue === 0) { return Operand.Error(StringCache.Function_Div_0_error); }
      return Operand.Create(args1.NumberValue / args2.NumberValue);
    }
    if (args1.IsNull) { return Operand.Error(StringCache.Function_parameter_null, '/', 1); }
    if (args2.IsNull) { return Operand.Error(StringCache.Function_parameter_null, '/', 2); }

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
          return Operand.Error(StringCache.Function_modulo_error, 'divided');
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
          args2 = result[0] ? Operand.One : Operand.Zero;
        } else {
          return Operand.Error(StringCache.Function_modulo_error, 'divided');
        }
      }
    }
    args2 = args2.ToNumber(StringCache.Function_parameter_error, '/', 2); if (args2.IsError) { return args2; } 
    if (args2.NumberValue === 0) { return Operand.Error(StringCache.Function_Div_0_error); }
    if (args2.NumberValue === 1) { return args1; }

    args1 = args1.ToNumber(StringCache.Function_parameter_error, '/', 1); if (args1.IsError) { return args1; } 
    return Operand.Create(args1.NumberValue / args2.NumberValue);
  }
}

export { Function_Div };
