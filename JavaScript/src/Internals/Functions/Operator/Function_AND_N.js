import { Operand } from '../../../Operand.js';
import { Function_N } from '../Function_N.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_AND_N extends Function_N {
  constructor(z) {
    super(z);
  }

  Evaluate(engine, tempParameter)  {
    let index = 1;
    let b = true;
    for (let item of this.z) {
      let a = item.Evaluate(engine, tempParameter);
      a = a.ToBoolean(StringCache.Function_parameter_error, 'AND', index++); if (a.IsError) { return a; } 
      if (a.BooleanValue === false) b = false;
    }
    return b ? Operand.True : Operand.False;
  }
 
}

export { Function_AND_N };
