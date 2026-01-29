import { Operand } from '../../../Operand';
import { Function_2 } from '../Function_2';

class Function_OR extends Function_2 {
  constructor(func1, func2) {
    super(func1, func2);
  }

  Evaluate(engine) {
    // 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
    // 在excel内 AND(x,y) OR(x,y) 先报错，
    // 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
    // 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
    let args1 = this._arg1.Evaluate(engine);
    if (args1.IsNotBoolean) { args1 = args1.ToBoolean(); if (args1.IsError) { return args1; } }
    if (args1.BooleanValue) {
      let args2 = this._arg2.Evaluate(engine).ToBoolean();
      if (args2.IsError) { return args2; }
      return Operand.True;
    }
    return this._arg2.Evaluate(engine).ToBoolean();
  }

  ToString(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.push('(');
    this._arg1.ToString(stringBuilder, false);
    stringBuilder.push(' || ');
    this._arg2.ToString(stringBuilder, false);
    if (addBrackets) stringBuilder.push(')');
  }
}

export { Function_OR };