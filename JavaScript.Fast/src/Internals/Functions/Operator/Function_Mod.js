import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_Mod extends Function_2 {
  get Name() {
    return '%';
  }

  constructor(z) {
    super(z);
  }

  evaluate(work, tempParameter) {
    let args1 = this.getNumber_1(work, tempParameter); if (args1.IsError) { return args1; }
    let args2 = this.getNumber_2(work, tempParameter); if (args2.IsError) { return args2; }

    if (args2.NumberValue === 0) { return this.div0Error(); }

    let number1 = args1.NumberValue;
    let number2 = args2.NumberValue;
    // Excel MOD 语义:结果符号随除数,即 n - d*INT(n/d);JS % 符号随被除数,需修正
    let r = number1 % number2;
    if (r != 0 && (r < 0 != number2 < 0)) {
        r += number2;
    }
    return Operand.Create(r);
  }

  toString2(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.append('(');
    this.a.toString2(stringBuilder, true);
    stringBuilder.append(' % ');
    this.b.toString2(stringBuilder, true);
    if (addBrackets) stringBuilder.append(')');
  }
}

export { Function_Mod };
