import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';
import { StringCache } from '../../../Internals/StringCache.js';

class Function_NE extends Function_2 {
  constructor(func1, func2) {
    super(func1, func2);
  }

  Evaluate(engine, tempParameter) {
    let args1 = this.func1.Evaluate(engine, tempParameter);
    if (args1.IsError) { return args1; }
    let args2 = this.func2.Evaluate(engine, tempParameter);
    if (args2.IsError) { return args2; }

    if (args1.Type === args2.Type) {
      if (args1.IsNumber) {
        return Operand.Create(args1.NumberValue !== args2.NumberValue);
      } else if (args1.IsText) {
        return Operand.Create(args1.TextValue !== args2.TextValue);
      } else if (args1.IsBoolean) {
        return Operand.Create(args1.BooleanValue !== args2.BooleanValue);
      } else if (args1.IsDate) {
        let a1 = args1.ToNumber();
        let a2 = args2.ToNumber();
        return Operand.Create(a1.NumberValue !== a2.NumberValue);
      } else if (args1.IsJson) {
        let a1 = args1.ToText();
        let a2 = args2.ToText();
        return Operand.Create(a1.TextValue !== a2.TextValue);
      } else if (args1.IsNull) {
        return Operand.False;
      } else {
        return Operand.Error(StringCache.Function_compare_error, "!=");
      }
    } else if (args1.IsNull || args2.IsNull) {
      return Operand.True;
    } else if (args2.IsText) {
      if (args1.IsBoolean) {
        let a = args2.ToBoolean();
        if (!a.IsError) {
          return a.BooleanValue !== args1.BooleanValue ? Operand.True : Operand.False;
        }
        let a1 = args1.ToText();
        return Operand.Create(a1.TextValue !== args2.TextValue);
      } else if (args1.IsDate || args1.IsNumber || args1.IsJson) {
        let a1 = args1.ToText();
        return Operand.Create(a1.TextValue !== args2.TextValue);
      } else {
        return Operand.Error(StringCache.Function_compare_error, "!=");
      }
    } else if (args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
      return Operand.Error(StringCache.Function_compare_error, "!=");
    }
    if (args1.IsNotNumber) {
      let a1 = args1.ToNumber(StringCache.Function_parameter_error, "!=", 1);
      if (a1.IsError) { return a1; }
      args1 = a1;
    }
    if (args2.IsNotNumber) {
      let a2 = args2.ToNumber(StringCache.Function_parameter_error, "!=", 2);
      if (a2.IsError) { return a2; }
      args2 = a2;
    }

    return Operand.Create(args1.NumberValue !== args2.NumberValue);
  }
}

export { Function_NE };

