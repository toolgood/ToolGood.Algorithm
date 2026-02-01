import { Operand } from '../../../Operand.js';
import { Function_2 } from '../Function_2.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_OR extends Function_2 {
  constructor(funcs) {
    super(funcs);
  }

  Evaluate(engine, tempParameter) {
    // 程序 && and || or �?excel�? AND(x,y) OR(x,y) 有区�?
    // 在excel�?AND(x,y) OR(x,y) 先报错，
    // 在程序中�?& and  有true 直接返回true 就不会检测下一个会不会报错
    // 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
    let args1 = this.func1.Evaluate(engine, tempParameter);
    if (args1.IsNotBoolean) { args1 = args1.ToBoolean(StringCache.Function_parameter_error, 'OR', 1); if (args1.IsError) { return args1; } }
    if (args1.BooleanValue) {
      let args2 = this.func2.Evaluate(engine, tempParameter).ToBoolean(StringCache.Function_parameter_error, 'OR', 2);
      if (args2.IsError) { return args2; }
      return Operand.True;
    }
    return this.func2.Evaluate(engine, tempParameter).ToBoolean(StringCache.Function_parameter_error, 'OR', 2);
  }
}

export { Function_OR };
