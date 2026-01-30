import { Operand } from '../../../Operand.js';
import { Function_N } from '../Function_N.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_AND_N extends Function_N {
  constructor(funcs) {
    super(funcs);
  }

  Evaluate(engine, tempParameter)  {
    let index = 1;
    let b = true;
    for (let item of this.funcs) {
      let a = item.Evaluate(engine, tempParameter);
      if (a.IsNotBoolean) { a = a.ToBoolean(StringCache.Function_parameter_error2, 'AND', index++); if (a.IsError) { return a; } }
      if (a.BooleanValue === false) b = false;
    }
    return b ? Operand.True : Operand.False;
  }

  ToString(stringBuilder, addBrackets) {
    stringBuilder.push('AND(');
    for (let i = 0; i < this.funcs.length; i++) {
      if (i > 0) stringBuilder.push(', ');
      this.funcs[i].ToString(stringBuilder, false);
    }
    stringBuilder.push(')');
  }
}

export { Function_AND_N };