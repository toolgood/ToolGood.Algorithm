import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_HAS
 */
export class Function_HAS extends Function_2 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     */
    constructor(func1, func2) {
        super(func1, func2);
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        let args1 = this.func1.Evaluate(engine, tempParameter);
        if (args1.IsError) {
            return args1;
        }
        let args2 = this.func2.Evaluate(engine, tempParameter);
        if (args2.IsNotText) {
            args2.ToText('Function \'{0}\' parameter {1} is error!', 'Has', 2);
            if (args2.IsError) {
                return args2;
            }
        }
        
        if (args1.IsArrayJson) {
            return Operand.Create(args1.containsKey(args2));
        } else if (args1.IsJson) {
            let json = args1.JsonValue;
            if (Array.IsArray(json)) {
                for (let i = 0; i < json.length; i++) {
                    let v = json[i];
                    if (typeof v === 'string') {
                        if (v === args2.TextValue) {
                            return Operand.True;
                        }
                    } else if (typeof v === 'number') {
                        if (v.toString() === args2.TextValue) {
                            return Operand.True;
                        }
                    } else if (typeof v === 'boolean') {
                        if (v.toString().toLowerCase() === args2.TextValue.toLowerCase()) {
                            return Operand.True;
                        }
                    }
                }
            } else {
                if (json[args2.TextValue] !== undefined) {
                    return Operand.True;
                }
            }
            return Operand.False;
        } else if (args1.IsArray) {
            let ar = args1.ArrayValue;
            for (let item of ar) {
                let t = item.ToText();
                if (t.IsError) {
                    continue;
                }
                if (t.TextValue === args2.TextValue) {
                    return Operand.True;
                }
            }
            return Operand.False;
        }
        return Operand.error('Function \'{0}\' parameter {1} is error!', 'Has', 1);
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.AddFunction(stringBuilder, 'Has');
    }
}
