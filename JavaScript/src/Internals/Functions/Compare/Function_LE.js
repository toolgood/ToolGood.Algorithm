import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_LE extends Function_2 {
  constructor(funcs) {
    super(funcs);
  }

  get Name() {
    return "<=";
  }

  Evaluate(engine, tempParameter) {
    let args1 = this.a.Evaluate(engine, tempParameter);
    if (args1.IsError) { return args1; }
    let args2 = this.b.Evaluate(engine, tempParameter);
    if (args2.IsError) { return args2; }

    if (args1.Type === args2.Type) {
      if (args1.IsNumber) {
        return Operand.Create(args1.NumberValue <= args2.NumberValue);
      } else if (args1.IsText) {
        let r = args1.TextValue.localeCompare(args2.TextValue, undefined, { numeric: true, sensitivity: 'base' });
        return r <= 0 ? Operand.True : Operand.False;
      } else if (args1.IsDate) {
        return Operand.Create(args1.DateValue.ToLong() <= args2.DateValue.ToLong());
      } else if (args1.IsBoolean) {
        args1 = args1.ToNumber();
        args2 = args2.ToNumber();
        return Operand.Create(args1.NumberValue <= args2.NumberValue);
      } else if (args1.IsNull) {
        return Operand.True;
      } else {
        return this.CompareError();
      }
    } else if (args1.IsNull || args2.IsNull) {
      return Operand.False;
    } else if (args1.IsDate || args2.IsDate || args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
      return this.CompareError();
    }
    args1 = this.ConvertToNumber(args1, 1);
    if (args1.IsError) { return args1; }
    args2 = this.ConvertToNumber(args2, 2);
    if (args2.IsError) { return args2; }

    return Operand.Create(args1.NumberValue <= args2.NumberValue);
  }

  ToString(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.push('(');
    this.a.ToString(stringBuilder, false);
    stringBuilder.push(" <= ");
    this.b.ToString(stringBuilder, false);
    if (addBrackets) stringBuilder.push(')');
  }
}

export { Function_LE };

