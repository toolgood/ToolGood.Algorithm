import { Operand } from '../../../Operand';
import { Function_2 } from '../Function_2';

class Function_Connect extends Function_2 {
  constructor(func1, func2) {
    super(func1, func2);
  }

  Evaluate(engine) {
    const args1 = this._arg1.Evaluate(engine);
    if (args1.IsError) { return args1; }
    const args2 = this._arg2.Evaluate(engine);
    if (args2.IsError) { return args2; }

    if (args1.IsNull) {
      if (args2.IsNull) return args1;
      return args2.ToText('Function \'{0}\' parameter {1} is error!', '&', 2);
    } else if (args2.IsNull) {
      return args1.ToText('Function \'{0}\' parameter {1} is error!', '&', 1);
    }
    if (args1.IsNotText) { args1 = args1.ToText('Function \'{0}\' parameter {1} is error!', '&', 1); if (args1.IsError) { return args1; } }
    if (args2.IsNotText) { args2 = args2.ToText('Function \'{0}\' parameter {1} is error!', '&', 2); if (args2.IsError) { return args2; } }

    return Operand.Create(args1.TextValue + args2.TextValue);
  }

  ToString(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.push('(');
    this._arg1.ToString(stringBuilder, false);
    stringBuilder.push(' & ');
    this._arg2.ToString(stringBuilder, false);
    if (addBrackets) stringBuilder.push(')');
  }
}

export { Function_Connect };