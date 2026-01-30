import { Operand } from '../../../Operand.js';
import { Function_N } from '../Function_N.js';

class Function_OR_N extends Function_N {
  constructor(funcs) {
    super(funcs);
  }

  Evaluate(engine, tempParameter) {
    let index = 1;
    let b = false;
    for (let item of this.funcs) {
      let a = item.Evaluate(engine, tempParameter);
      if (a.IsError) { return a; }
      if (a.IsNotBoolean) { a = a.ToBoolean(`Function '{0}' parameter '{1}' is error!`, 'OR', index++); if (a.IsError) { return a; } }
      if (a.BooleanValue) { b = true; }
    }
    return b ? Operand.True : Operand.False;
  }
 
}

export { Function_OR_N };
