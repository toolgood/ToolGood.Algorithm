import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_LE extends Function_2 {
  constructor(func1, func2) {
    super(func1, func2);
  }

  Evaluate(engine) {
    const args1 = this.func1.Evaluate(engine);
    if (args1.IsError) { return args1; }
    const args2 = this.func2.Evaluate(engine);
    if (args2.IsError) { return args2; }

    if (args1.Type === args2.Type) {
      if (args1.IsNumber) {
        return Operand.Create(args1.NumberValue <= args2.NumberValue);
      } else if (args1.IsText) {
        const r = args1.TextValue.localeCompare(args2.TextValue, undefined, { numeric: true, sensitivity: 'base' });
        return r <= 0 ? Operand.True : Operand.False;
      } else if (args1.IsDate || args1.IsBoolean) {
        const a1 = args1.ToNumber();
        const a2 = args2.ToNumber();
        return Operand.Create(a1.NumberValue <= a2.NumberValue);
      } else if (args1.IsJson) {
        const a1 = args1.ToText();
        const a2 = args2.ToText();
        const r = a1.TextValue.localeCompare(a2.TextValue, undefined, { numeric: true, sensitivity: 'base' });
        return r <= 0 ? Operand.True : Operand.False;
      } else if (args1.IsNull) {
        return Operand.True;
      } else {
        return Operand.Error("Function '<=' compare is error.");
      }
    } else if (args1.IsNull || args2.IsNull) {
      return Operand.False;
    } else if (args2.IsText) {
      if (args1.IsBoolean) {
        const a = args2.ToBoolean();
        if (!a.IsError) {
          return a.BooleanValue !== args1.BooleanValue ? Operand.True : Operand.False;
        }
        const a1 = args1.ToText();
        const r = a1.TextValue.localeCompare(args2.TextValue, undefined, { numeric: true, sensitivity: 'base' });
        return r <= 0 ? Operand.True : Operand.False;
      } else if (args1.IsDate || args1.IsNumber || args1.IsJson) {
        const a1 = args1.ToText();
        const r = a1.TextValue.localeCompare(args2.TextValue, undefined, { numeric: true, sensitivity: 'base' });
        return r <= 0 ? Operand.True : Operand.False;
      } else {
        return Operand.Error("Function '<=' compare is error.");
      }
    } else if (args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
      return Operand.Error("Function '<=' compare is error.");
    }
    if (args1.IsNotNumber) {
      const a1 = args1.ToNumber(`Function '{"<="}' parameter {1} is error!`);
      if (a1.IsError) { return a1; }
      args1 = a1;
    }
    if (args2.IsNotNumber) {
      const a2 = args2.ToNumber(`Function '{"<="}' parameter {2} is error!`);
      if (a2.IsError) { return a2; }
      args2 = a2;
    }

    return Operand.Create(args1.NumberValue <= args2.NumberValue);
  }

  ToString(stringBuilder, addBrackets) {
    if (addBrackets) stringBuilder.push('(');
    this.func1.ToString(stringBuilder, false);
    stringBuilder.push(" <= ");
    this.func2.ToString(stringBuilder, false);
    if (addBrackets) stringBuilder.push(')');
  }
}

export { Function_LE };
