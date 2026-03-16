import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_Sub extends Function_2 {
  get Name() {
    return '-';
  }

  constructor(z) {
    super(z);
  }

  evaluate(work, tempParameter) {
    let args1 = this.getNumber_1(work, tempParameter); if (args1.IsError) { return args1; }
    let args2 = this.getNumber_2(work, tempParameter); if (args2.IsError) { return args2; }

    if (args2.NumberValue === 0) { return args1; }

    return Operand.Create(args1.NumberValue - args2.NumberValue);
  }

  toString2(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.append('(');
    this.a.toString2(stringBuilder, false);
    stringBuilder.append(' - ');
    this.b.toString2(stringBuilder, false);
    if (addBrackets) stringBuilder.append(')');
  }
}

export { Function_Sub };

