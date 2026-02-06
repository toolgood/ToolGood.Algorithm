import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_Connect extends Function_2 {
  get Name() {
    return '&';
  }

  constructor(z) {
    super(z);
  }

  Evaluate(work, tempParameter) {
    let args1 = this.GetText_1(work, tempParameter); if (args1.IsError) { return args1; }
    let args2 = this.GetText_2(work, tempParameter); if (args2.IsError) { return args2; }

    return Operand.Create(args1.TextValue + args2.TextValue);
  }

  ToString(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.append('(');
    this.a.ToString(stringBuilder, false);
    stringBuilder.append(' & ');
    this.b.ToString(stringBuilder, false);
    if (addBrackets) stringBuilder.append(')');
  }
}

export { Function_Connect };
