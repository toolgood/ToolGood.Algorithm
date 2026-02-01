import { Operand } from '../../../Operand.js';
import { Function_2 } from '../Function_2.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_Connect extends Function_2 {
  constructor(z) {
    super(z);
  }

  Evaluate(engine, tempParameter) {
    let args1 = this.a.Evaluate(engine, tempParameter);
    if (args1.IsError) { return args1; }
    let args2 = this.b.Evaluate(engine, tempParameter);
    if (args2.IsError) { return args2; }

    if (args1.IsNull) {
      if (args2.IsNull) return args1;
      return args2.ToText(StringCache.Function_parameter_error, '&', 2);
    } else if (args2.IsNull) {
      return args1.ToText(StringCache.Function_parameter_error, '&', 1);
    }
    args1 = args1.ToText(StringCache.Function_parameter_error, '&', 1); if (args1.IsError) { return args1; } 
    args2 = args2.ToText(StringCache.Function_parameter_error, '&', 2); if (args2.IsError) { return args2; } 

    return Operand.Create(args1.TextValue + args2.TextValue);
  }
}

export { Function_Connect };
