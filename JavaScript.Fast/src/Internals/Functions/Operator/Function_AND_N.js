import { Function_N } from '../Function_N.js';
import { Operand } from '../../../Operand.js';

class Function_AND_N extends Function_N {
  get Name() {
    return 'AndN';
  }

  constructor(z) {
    super(z);
  }

  evaluate(work, tempParameter) {
    let b = true;
    for (let i = 0; i < this.z.length; i++) {
      let a = this.getBoolean(work, tempParameter, i);
      if (a.IsErrorOrNone) { return a; }
      if (a.BooleanValue === false) {
        b = false;
        // 非严格模式短路,严格模式继续求值以检查后续错误
        if (work.UseStrictMode === false) { break; }
      }
    }
    return b ? Operand.True : Operand.False;
  }
}

export { Function_AND_N };
