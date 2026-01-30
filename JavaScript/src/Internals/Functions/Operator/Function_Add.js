import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { FunctionUtil } from '../FunctionUtil.js';
import { MyDate } from '../../MyDate.js';

class Function_Add extends Function_2 {
  constructor(func1, func2) {
    super(func1, func2);
  }

  Evaluate(engine, tempParameter)  {
    let args1 = this.func1.Evaluate(engine, tempParameter);
    if (args1.IsError) { return args1; }
    let args2 = this.func2.Evaluate(engine, tempParameter);
    if (args2.IsError) { return args2; }

    if (args1.IsNumber && args2.IsNumber) {
      if (args1.NumberValue === 0) { return args2; }
      if (args2.NumberValue === 0) { return args1; }
      return Operand.Create(args1.NumberValue + args2.NumberValue);
    }
    if (args1.IsNull) { return Operand.Error(`Function '+' parameter {1} is NULL!`); }
    if (args2.IsNull) { return Operand.Error(`Function '+' parameter {2} is NULL!`); }

    if (args1.IsText) {
      let d = parseFloat(args1.TextValue);
      if (!isNaN(d)) {
        args1 = Operand.Create(d);
      } else {
        let boolValue = [false];
        if (FunctionUtil.TryParseBoolean(args1.TextValue, boolValue)) {
          args1 = boolValue[0] ? Operand.One : Operand.Zero;
        } else {
          let date = new Date(args1.TextValue);
          if (!isNaN(date.getTime())) {
            args1 = Operand.Create(new Date(date));
          } else {
            return Operand.Error("Function '+' is error");
          }
        }
      }
    }
    if (args2.IsText) {
      let d = parseFloat(args2.TextValue);
      if (!isNaN(d)) {
        args2 = Operand.Create(d);
      } else {
        let boolValue = [false];
        if (FunctionUtil.TryParseBoolean(args2.TextValue, boolValue)) {
          args2 = boolValue[0] ? Operand.One : Operand.Zero;
        } else {
          let date = new Date(args2.TextValue);
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
        // 两个日期相加，创建新�?MyDate 对象
        let totalValue = args1.ToNumber().NumberValue + args2.ToNumber().NumberValue;
        return Operand.Create(new MyDate(totalValue));
      }
      if (args2.IsNotNumber) {
        let a2 = args2.ToNumber(`Function '{"+"}' parameter {2} is error!`);
        if (a2.IsError) { return a2; }
        args2 = a2;
      }
      if (args2.NumberValue === 0) { return args1; }
      // 日期加数字，创建新的 MyDate 对象
      let totalValue = args1.DateValue.Value + args2.NumberValue;
      return Operand.Create(new MyDate(totalValue));
    } else if (args2.IsDate) {
      if (args1.IsNotNumber) {
        let a1 = args1.ToNumber(`Function '{"+"}' parameter {1} is error!`);
        if (a1.IsError) { return a1; }
        args1 = a1;
      }
      if (args1.NumberValue === 0) { return args2; }
      // 数字加日期，创建新的 MyDate 对象
      let totalValue = args1.NumberValue + args2.DateValue.Value;
      return Operand.Create(new MyDate(totalValue));
    }
    if (args1.IsNotNumber) {
      let a1 = args1.ToNumber(`Function '{"+"}' parameter {1} is error!`);
      if (a1.IsError) { return a1; }
      args1 = a1;
    }
    if (args2.IsNotNumber) {
      let a2 = args2.ToNumber(`Function '{"+"}' parameter {2} is error!`);
      if (a2.IsError) { return a2; }
      args2 = a2;
    }

    if (args1.NumberValue === 0) { return args2; }
    if (args2.NumberValue === 0) { return args1; }

    return Operand.Create(args1.NumberValue + args2.NumberValue);
  }
}

export { Function_Add };

