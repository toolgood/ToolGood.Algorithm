import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

/**
 * Function_HAS
 */
export class Function_HAS extends Function_2 {
    /**
     * @param {FunctionBase[]} funcs
     */
    constructor(funcs) {
        super(funcs);
    }
    
    get Name() {
        return "Has";
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    Evaluate(engine, tempParameter) {
        let args1 = this.a.Evaluate(engine, tempParameter);
        if (args1.IsError) { return args1; }

        let args2 = this.GetText_2(engine, tempParameter);
        if (args2.IsError) { return args2; }

        if (args1.IsArrayJson) {
            return Operand.Create(args1.containsKey(args2));
        } else if (args1.IsJson) {
            let json = args1.JsonValue;
            if (Array.isArray(json)) {
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
        return this.ParameterError(1);
    }
    

}

