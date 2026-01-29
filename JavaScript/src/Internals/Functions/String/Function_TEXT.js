import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_TEXT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    Evaluate(engine, tempParameter) {
        const args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsError) {
            return args1;
        }
        const args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2.ToText('Function {0} parameter {1} is error!', 'Text', 2);
            if (args2.IsError) {
                return args2;
            }
        }

        if (args1.isText) {
            return args1;
        } else if (args1.isBoolean) {
            return Operand.Create(args1.BooleanValue ? 'TRUE' : 'FALSE');
        } else if (args1.IsNumber) {
            // JavaScript的toLocaleString或其他格式化方法可能无法完全匹配C#的格式，但这里先使用基本的toString
            return Operand.Create(args1.NumberValue.toString());
        } else if (args1.IsDate) {
            // 同样，日期格式化可能需要更复杂的处理
            return Operand.Create(args1.DateValue.toString());
        }
        const args1Text = args1.ToText('Function {0} parameter {1} is error!', 'Text', 1);
        if (args1Text.IsError) {
            return args1Text;
        }
        return Operand.Create(args1Text.TextValue.toString());
    }

    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Text');
    }
}

export { Function_TEXT };
