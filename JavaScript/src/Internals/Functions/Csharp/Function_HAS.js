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
    Evaluate(engine) {
        const args1 = this.func1.Evaluate(engine);
        if (args1.isError) {
            return args1;
        }
        const args2 = this.func2.Evaluate(engine);
        if (args2.isNotText) {
            args2.toText('Function \'{0}\' parameter {1} is error!', 'Has', 2);
            if (args2.isError) {
                return args2;
            }
        }
        
        if (args1.isArrayJson) {
            return Operand.create(args1.containsKey(args2));
        } else if (args1.isJson) {
            const json = args1.jsonValue;
            if (Array.isArray(json)) {
                for (let i = 0; i < json.length; i++) {
                    const v = json[i];
                    if (typeof v === 'string') {
                        if (v === args2.textValue) {
                            return Operand.True;
                        }
                    } else if (typeof v === 'number') {
                        if (v.toString() === args2.textValue) {
                            return Operand.True;
                        }
                    } else if (typeof v === 'boolean') {
                        if (v.toString().toLowerCase() === args2.textValue.toLowerCase()) {
                            return Operand.True;
                        }
                    }
                }
            } else {
                if (json[args2.textValue] !== undefined) {
                    return Operand.True;
                }
            }
            return Operand.False;
        } else if (args1.isArray) {
            const ar = args1.arrayValue;
            for (let item of ar) {
                const t = item.toText();
                if (t.isError) {
                    continue;
                }
                if (t.textValue === args2.textValue) {
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
        this.addFunction(stringBuilder, 'Has');
    }
}
