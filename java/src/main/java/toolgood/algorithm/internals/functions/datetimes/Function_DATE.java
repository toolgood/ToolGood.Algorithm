package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;



public class Function_DATE extends Function_N {
    public Function_DATE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = funcs[0].Evaluate(work, tempParameter);
        if (args1.isNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Date", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = funcs[1].Evaluate(work, tempParameter);
        if (args2.isNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Date", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = funcs[2].Evaluate(work, tempParameter);
        if (args3.isNotNumber()) {
            args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Date", 3);
            if (args3.IsError()) {
                return args3;
            }
        }

        toolgood.algorithm.internals.MyDate d;
        if (funcs.length == 3) {
            d = new toolgood.algorithm.internals.MyDate(args1.IntValue(), args2.IntValue(), args3.IntValue(), 0, 0, 0);
        } else if (funcs.length == 4) {
            Operand args4 = funcs[3].Evaluate(work, tempParameter);
            if (args4.isNotNumber()) {
                args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "Date", 4);
                if (args4.IsError()) {
                    return args4;
                }
            }
            d = new toolgood.algorithm.internals.MyDate(args1.IntValue(), args2.IntValue(), args3.IntValue(), args4.IntValue(), 0, 0);
        } else if (funcs.length == 5) {
            Operand args4 = funcs[3].Evaluate(work, tempParameter);
            if (args4.isNotNumber()) {
                args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "Date", 4);
                if (args4.IsError()) {
                    return args4;
                }
            }
            Operand args5 = funcs[4].Evaluate(work, tempParameter);
            if (args5.isNotNumber()) {
                args5 = args5.ToNumber("Function '{0}' parameter {1} is error!", "Date", 5);
                if (args5.IsError()) {
                    return args5;
                }
            }
            d = new toolgood.algorithm.internals.MyDate(args1.IntValue(), args2.IntValue(), args3.IntValue(), args4.IntValue(), args5.IntValue(), 0);
        } else {
            Operand args4 = funcs[3].Evaluate(work, tempParameter);
            if (args4.isNotNumber()) {
                args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "Date", 4);
                if (args4.IsError()) {
                    return args4;
                }
            }
            Operand args5 = funcs[4].Evaluate(work, tempParameter);
            if (args5.isNotNumber()) {
                args5 = args5.ToNumber("Function '{0}' parameter {1} is error!", "Date", 5);
                if (args5.IsError()) {
                    return args5;
                }
            }
            Operand args6 = funcs[5].Evaluate(work, tempParameter);
            if (args6.isNotNumber()) {
                args6 = args6.ToNumber("Function '{0}' parameter {1} is error!", "Date", 6);
                if (args6.IsError()) {
                    return args6;
                }
            }
            d = new toolgood.algorithm.internals.MyDate(args1.IntValue(), args2.IntValue(), args3.IntValue(), args4.IntValue(), args5.IntValue(), args6.IntValue());
        }
        return Operand.Create(d);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Date");
    }
}