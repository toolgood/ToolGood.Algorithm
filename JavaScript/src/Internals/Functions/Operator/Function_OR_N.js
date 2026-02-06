import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_OR_N extends Function_N {
  get Name() {
    return 'OrN';
  }

  constructor(z) {
    super(z);
  }

  Evaluate(work, tempParameter) {
    let b = false;
    for (let i = 0; i < this.z.length; i++) {
      let a = this.GetBoolean(work, tempParameter, i);
      if (a.IsError) { return a; }
      if (a.BooleanValue) b = true;
    }
    return b ? Operand.True : Operand.False;
  }
}

export { Function_OR_N };
