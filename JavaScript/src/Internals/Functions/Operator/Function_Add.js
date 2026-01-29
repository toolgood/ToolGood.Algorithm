import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';

class Function_Add extends Function_2 {
  constructor(func1, func2) {
    super(func1, func2);
  }

  Evaluate(engine) {
    const args1 = this._arg1.Evaluate(engine);
    if (args1.IsError) { return args1; }
    const args2 = this._arg2.Evaluate(engine);
    if (args2.IsError) { return args2; }

    if (args1.IsNumber && args2.IsNumber) {
      if (args1.NumberValue === 0) { return args2; }
      if (args2.NumberValue === 0) { return args1; }
      return Operand.Create(args1.NumberValue + args2.NumberValue);
    }
    if (args1.IsNull) { return Operand.Error(`Function '{"+"}' parameter {1} is NULL!`); }
    if (args2.IsNull) { return Operand.Error(`Function '{"+"}' parameter {2} is NULL!`); }

    if (args1.IsText) {
      const d = parseFloat(args1.TextValue);
      if (!isNaN(d)) {
        args1 = Operand.Create(d);
      } else {
        const boolValue = [false];
        if (FunctionUtil.TryParseBoolean(args1.TextValue, boolValue)) {
          args1 = boolValue[0] ? Operand.One : Operand.Zero;
        } else {
          const date = new Date(args1.TextValue);
          if (!isNaN(date.getTime())) {
            args1 = Operand.Create(new Date(date));
          } else {
            return Operand.Error("Function '+' is error");
          }
        }
      }
    }
    if (args2.IsText) {
      const d = parseFloat(args2.TextValue);
      if (!isNaN(d)) {
        args2 = Operand.Create(d);
      } else {
        const boolValue = [false];
        if (FunctionUtil.TryParseBoolean(args2.TextValue, boolValue)) {
          args2 = boolValue[0] ? Operand.One : Operand.Zero;
        } else {
          const date = new Date(args2.TextValue);
          if (!isNaN(date.getTime())) {
            args2 = Operand.Create(new Date(date));
          } else {
            return Operand.Error("Function '+' is error");
          }
        }
      }
    }
    if (args1.IsDate) {
      if (args2.IsDate) {
        return Operand.Create(args1.DateValue.ToNumber() + args2.DateValue.ToNumber());
      }
      if (args2.IsNotNumber) {
        const a2 = args2.ToNumber(`Function '{"+"}' parameter {2} is error!`);
        if (a2.IsError) { return a2; }
        args2 = a2;
      }
      if (args2.NumberValue === 0) { return args1; }
      return Operand.Create(args1.DateValue.ToNumber() + args2.NumberValue);
    } else if (args2.IsDate) {
      if (args1.IsNotNumber) {
        const a1 = args1.ToNumber(`Function '{"+"}' parameter {1} is error!`);
        if (a1.IsError) { return a1; }
        args1 = a1;
      }
      if (args1.NumberValue === 0) { return args2; }
      return Operand.Create(args2.DateValue.ToNumber() + args1.NumberValue);
    }
    if (args1.IsNotNumber) {
      const a1 = args1.ToNumber(`Function '{"+"}' parameter {1} is error!`);
      if (a1.IsError) { return a1; }
      args1 = a1;
    }
    if (args2.IsNotNumber) {
      const a2 = args2.ToNumber(`Function '{"+"}' parameter {2} is error!`);
      if (a2.IsError) { return a2; }
      args2 = a2;
    }

    if (args1.NumberValue === 0) { return args2; }
    if (args2.NumberValue === 0) { return args1; }

    return Operand.Create(args1.NumberValue + args2.NumberValue);
  }

  ToString(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.push('(');
    this._arg1.ToString(stringBuilder, false);
    stringBuilder.push(" + ");
    this._arg2.ToString(stringBuilder, false);
    if (addBrackets) stringBuilder.push(')');
  }
}

export { Function_Add };
