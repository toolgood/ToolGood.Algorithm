import { Function_2 } from '../Function_2';
import { Operand } from '../../Operand';

class Function_TEXT extends Function_2 {
    constructor(func1, func2) {
        super(func1, func2);
    }

    evaluate(engine, tempParameter) {
        const args1 = this.func1.evaluate(engine, tempParameter);
        if (args1.isError) {
            return args1;
        }
        const args2 = this.func2.evaluate(engine, tempParameter);
        if (args2.isNotText) {
            args2.toText('Function {0} parameter {1} is error!', 'Text', 2);
            if (args2.isError) {
                return args2;
            }
        }

        if (args1.isText) {
            return args1;
        } else if (args1.isBoolean) {
            return Operand.create(args1.booleanValue ? 'TRUE' : 'FALSE');
        } else if (args1.isNumber) {
            // JavaScript的toLocaleString或其他格式化方法可能无法完全匹配C#的格式，但这里先使用基本的toString
            return Operand.create(args1.numberValue.toString());
        } else if (args1.isDate) {
            // 同样，日期格式化可能需要更复杂的处理
            return Operand.create(args1.dateValue.toString());
        }
        const args1Text = args1.toText('Function {0} parameter {1} is error!', 'Text', 1);
        if (args1Text.isError) {
            return args1Text;
        }
        return Operand.create(args1Text.textValue.toString());
    }

    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'Text');
    }
}

export { Function_TEXT };
