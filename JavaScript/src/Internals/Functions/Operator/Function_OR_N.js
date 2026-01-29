import { Operand } from '../../../Operand.js';
import { Function_N } from '../Function_N.js';

class Function_OR_N extends Function_N {
  constructor(funcs) {
    super(funcs);
  }

  Evaluate(engine) {
    let index = 1;
    let b = false;
    for (let item of this._args) {
      let a = item.Evaluate(engine);
      if (a.IsError) { return a; }
      if (a.IsNotBoolean) { a = a.ToBoolean(`Function '{0}' parameter '{1}' is error!`, 'OR', index++); if (a.IsError) { return a; } }
      if (a.BooleanValue) { b = true; }
    }
    return b ? Operand.True : Operand.False;
  }

  ToString(stringBuilder, addBrackets) {
    stringBuilder.push('OR(');
    for (let i = 0; i < this._args.length; i++) {
      if (i > 0) { stringBuilder.push(', '); }
      this._args[i].ToString(stringBuilder, false);
    }
    stringBuilder.push(')');
  }
}

export { Function_OR_N };