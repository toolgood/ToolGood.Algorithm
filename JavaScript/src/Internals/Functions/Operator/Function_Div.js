import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_Div extends Function_2 {
  get Name() {
    return '/';
  }

  constructor(z) {
    super(z);
  }

  Evaluate(work, tempParameter) {
    let args1 = this.GetNumber_1(work, tempParameter); if (args1.IsError) { return args1; }
    let args2 = this.GetNumber_2(work, tempParameter); if (args2.IsError) { return args2; }

    if (args2.NumberValue === 0) { return this.Div0Error(); }
    if (args2.NumberValue === 1) { return args1; }

    return Operand.Create(args1.NumberValue / args2.NumberValue);
  }

  ToString(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.append('(');
    this.a.ToString(stringBuilder, true);
    stringBuilder.append(' / ');
    this.b.ToString(stringBuilder, true);
    if (addBrackets) stringBuilder.append(')');
  }
}

export { Function_Div };
