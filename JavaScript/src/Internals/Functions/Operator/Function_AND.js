import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_AND extends Function_2 {
  get Name() {
    return 'And';
  }

  constructor(z) {
    super(z);
  }

  evaluate(work, tempParameter) {
    // 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
    // 在excel内 AND(x,y) OR(x,y) 先报错，
    // 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
    // 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
    let args1 = this.getBoolean_1(work, tempParameter);
    if (args1.IsError) { return args1; }
    if (args1.BooleanValue === false) {
      let args2 = this.getBoolean_2(work, tempParameter);
      if (args2.IsError) { return args2; }
      return Operand.False;
    }
    return this.getBoolean_2(work, tempParameter);
  }

  toString2(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.append('(');
    this.a.toString2(stringBuilder, false);
    stringBuilder.append(' && ');
    this.b.toString2(stringBuilder, false);
    if (addBrackets) stringBuilder.append(')');
  }
}

export { Function_AND };
