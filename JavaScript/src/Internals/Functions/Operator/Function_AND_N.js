import { Operand } from '../../../Operand';
import { Function_N } from '../Function_N';

class Function_AND_N extends Function_N {
  constructor(funcs) {
    super(funcs);
  }

  Evaluate(engine) {
    let index = 1;
    let b = true;
    for (let item of this._args) {
      let a = item.Evaluate(engine);
      if (a.IsNotBoolean) { a = a.ToBoolean('Function \'{0}\' parameter {1} is error!', 'AND', index++); if (a.IsError) { return a; } }
      if (a.BooleanValue === false) b = false;
    }
    return b ? Operand.True : Operand.False;
  }

  ToString(stringBuilder, addBrackets) {
    stringBuilder.push('AND(');
    for (let i = 0; i < this._args.length; i++) {
      if (i > 0) stringBuilder.push(', ');
      this._args[i].ToString(stringBuilder, false);
    }
    stringBuilder.push(')');
  }
}

export { Function_AND_N };