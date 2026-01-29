import { Operand } from '../../../Operand';
import { Function_2 } from '../Function_2';
import { FunctionUtil } from '../FunctionUtil';

class Function_Div extends Function_2 {
  constructor(func1, func2) {
    super(func1, func2);
  }

  Evaluate(engine) {
    const args1 = this._arg1.Evaluate(engine);
    if (args1.IsError) { return args1; }
    const args2 = this._arg2.Evaluate(engine);
    if (args2.IsError) { return args2; }

    if (args1.IsNumber && args2.IsNumber) { //  优化性能
      if (args2.NumberValue === 1) { return args1; }
      if (args2.NumberValue === 0) { return Operand.Error('Div 0 is error!'); }
      return Operand.Create(args1.NumberValue / args2.NumberValue);
    }
    if (args1.IsNull) { return Operand.Error('Function \'{0}\'' + ' parameter ' + '{1}' + ' is NULL!', '/', 1); }
    if (args2.IsNull) { return Operand.Error('Function \'{0}\'' + ' parameter ' + '{1}' + ' is NULL!', '/', 2); }

    if (args1.IsText) {
      const d = parseFloat(args1.TextValue);
      if (!isNaN(d)) {
        args1 = Operand.Create(d);
      } else {
        const result = [false];
        if (FunctionUtil.TryParseBoolean(args1.TextValue, result)) {
          const b = result[0];
          args1 = b ? Operand.One : Operand.Zero;
        } else {
          return Operand.Error('Two types cannot be divided ');
        }
      }
    }
    if (args2.IsText) {
      const d = parseFloat(args2.TextValue);
      if (!isNaN(d)) {
        args2 = Operand.Create(d);
      } else {
        const result = [false];
        if (FunctionUtil.TryParseBoolean(args2.TextValue, result)) {
          args2 = result[0] ? Operand.One : Operand.Zero;
        } else {
          return Operand.Error('Two types cannot be divided');
        }
      }
    }
    if (args2.IsNotNumber) { args2 = args2.ToNumber('Function \'{0}\'' + ' parameter ' + '{1}' + ' is error!', '/', 2); if (args2.IsError) { return args2; } }
    if (args2.NumberValue === 0) { return Operand.Error('Div 0 is error!'); }
    if (args2.NumberValue === 1) { return args1; }

    if (args1.IsNotNumber) { args1 = args1.ToNumber('Function \'{0}\'' + ' parameter ' + '{1}' + ' is error!', '/', 1); if (args1.IsError) { return args1; } }
    return Operand.Create(args1.NumberValue / args2.NumberValue);
  }

  ToString(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.push('(');
    this._arg1.ToString(stringBuilder, true);
    stringBuilder.push(' / ');
    this._arg2.ToString(stringBuilder, true);
    if (addBrackets) stringBuilder.push(')');
  }
}

export { Function_Div };