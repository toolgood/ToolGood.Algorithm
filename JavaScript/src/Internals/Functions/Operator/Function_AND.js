import { Operand } from '../../../Operand.js';
import { Function_2 } from '../Function_2.js';

class Function_AND extends Function_2 {
  constructor(func1, func2) {
    super(func1, func2);
  }

  Evaluate(engine, tempParameter)  {
    // 程序 && and || or �?excel�? AND(x,y) OR(x,y) 有区�?
    // 在excel�?AND(x,y) OR(x,y) 先报错，
    // 在程序中�?& and  有true 直接返回true 就不会检测下一个会不会报错
    // 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
    let args1 = this.func1.Evaluate(engine, tempParameter);
    if (args1.IsNotBoolean) { args1 = args1.ToBoolean(); if (args1.IsError) { return args1; } }
    if (args1.BooleanValue === false) {
      let args2 = this.func2.Evaluate(engine, tempParameter).ToBoolean();
      if (args2.IsError) { return args2; }
      return Operand.False;
    }
    return this.func2.Evaluate(engine, tempParameter).ToBoolean();
  }
}

export { Function_AND };
