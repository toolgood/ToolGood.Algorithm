using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm
{
    partial class AlgorithmEngine
    {
        private void AddFlowFunction()
        {
            addFunc("AND", Func_And);//如果所有参数均为TRUE，则返回TRUE
            addFunc("IF", Func_If);//指定要执行的逻辑检测
            addFunc("NOT", Func_Not);//对参数的逻辑值求反
            addFunc("OR", Func_Or);//如果任一参数为TRUE，则返回TRUE
            addFunc("FALSE", Func_False);//返回逻辑值FALSE
            addFunc("TRUE", Func_True);//返回逻辑值TRUE
            addFunc("IFERROR", Func_IfError);//指定要执行的逻辑检测
            addFunc("IFNUMBER", Func_IfNumber);//指定要执行的逻辑检测
            addFunc("IFTEXT", Func_IfText);//指定要执行的逻辑检测


            addFunc("ISNUMBER", Func_IsNumber);//指定要执行的逻辑检测
            addFunc("ISTEXT", Func_IsText);//指定要执行的逻辑检测


        }
        private Operand Func_IfText(List<Operand> arg)
        {
            CheckArgsCount("IfText", arg, new OperandType[][] {
                new OperandType[] { OperandType.Any },
                new OperandType[] { OperandType.Any, OperandType.Any },
                new OperandType[] { OperandType.Any, OperandType.Any, OperandType.Any },
                 });

            if (arg.Count < 2) return ThrowError("ISSTRING 中参数不足", new List<Operand>());
            if (arg[0].Type == OperandType.STRING) {
                return arg[1];
            } else if (arg[0].Type == OperandType.DATE) {
                if (arg[0].DateValue.srcText != null) {
                    return arg[1];
                } else {
                    if (arg.Count == 3) return arg[2];
                }
            }
            if (arg.Count == 3) return arg[2];
            return new Operand(OperandType.BOOLEAN, false);
        }


        private Operand Func_IfNumber(List<Operand> arg)
        {
            CheckArgsCount("IFNUMBER", arg, new OperandType[][] {
                new OperandType[] { OperandType.Any },
                new OperandType[] { OperandType.Any, OperandType.Any },
                new OperandType[] { OperandType.Any, OperandType.Any, OperandType.Any },
                 });

            if (arg.Count < 2) return ThrowError("IFNUMBER 中参数不足", new List<Operand>());
            var b = false;
            if (arg[0].Type == OperandType.NUMBER) {
                b = arg[0].IntValue != 0;
            } else if (arg[0].Type == OperandType.DATE) {
                b = true;
            }

            if (b) {
                return arg[1];
            }
            if (arg.Count == 3) {
                return arg[2];
            }
            return new Operand(OperandType.BOOLEAN, b);
        }


        private Operand Func_IsText(List<Operand> arg)
        {
            CheckArgsCount("IsText", arg, new OperandType[][] {
                new OperandType[] { OperandType.STRING },
                new OperandType[] { OperandType.DATE },
                 });

            if (arg.Count < 1) return ThrowError("ISSTRING 中参数不足", new List<Operand>());
            if (arg[0].Type == OperandType.STRING) {
                return new Operand(OperandType.BOOLEAN, true);
            } else if (arg[0].Type == OperandType.DATE) {
                return new Operand(OperandType.BOOLEAN, arg[0].DateValue.srcText != null);
            }
            return new Operand(OperandType.BOOLEAN, false);
        }

        private Operand Func_IsNumber(List<Operand> arg)
        {
            CheckArgsCount("ISNUMBER", arg, new OperandType[][] {
                new OperandType[] { OperandType.NUMBER },
                new OperandType[] { OperandType.DATE },
                 });

            if (arg[0].Type == OperandType.NUMBER) {
                return new Operand(OperandType.BOOLEAN, true);
            } else if (arg[0].Type == OperandType.DATE) {
                return new Operand(OperandType.BOOLEAN, true);
            }
            return new Operand(OperandType.BOOLEAN, false);
        }

        private Operand Func_IfError(List<Operand> arg)
        {
            CheckArgsCount("IFERROR", arg, new OperandType[][] {
                new OperandType[] { OperandType.Any },
                new OperandType[] { OperandType.Any, OperandType.Any },
                new OperandType[] { OperandType.Any, OperandType.Any, OperandType.Any },
                 });

            var b = false;
            if (arg[0].Type == OperandType.ERROR) {
                b = true;
            }
            if (b) {
                if (arg.Count > 1) {
                    return arg[1];
                }
                return new Operand(OperandType.BOOLEAN, b);
            }
            if (arg.Count == 3) {
                return arg[2];
            }
            return new Operand(OperandType.BOOLEAN, b);
        }

        private Operand Func_If(List<Operand> arg)
        {
            CheckArgsCount("IF", arg, new OperandType[][] {
                new OperandType[] { OperandType.Any },
                new OperandType[] { OperandType.Any, OperandType.Any },
                new OperandType[] { OperandType.Any, OperandType.Any, OperandType.Any },
                 });

            var b = true;
            if (arg[0].Type == OperandType.BOOLEAN) {
                b = arg[0].BooleanValue;
            } else if (arg[0].Type == OperandType.NUMBER) {
                b = arg[0].IntValue != 0;
            }

            if (b) {
                if (arg.Count > 1) {
                    return arg[1];
                }
                return new Operand(OperandType.BOOLEAN, b);
            }
            if (arg.Count == 3) {
                return arg[2];
            }
            return new Operand(OperandType.BOOLEAN, b);

        }

        private Operand Func_Not(List<Operand> arg)
        {
            CheckArgsCount("NOT", arg, new OperandType[][] {
                new OperandType[] { OperandType.BOOLEAN },
                new OperandType[] { OperandType.NUMBER },
                 });

            if (arg.Count < 1) return ThrowError("NOT 中参数不足", new List<Operand>());
            if (arg[0].Type == OperandType.BOOLEAN) {
                return new Operand(OperandType.BOOLEAN, !arg[0].BooleanValue);
            } else if (arg[0].Type == OperandType.NUMBER) {
                return new Operand(OperandType.BOOLEAN, (arg[0].NumberValue == 0));

            }
            return new Operand(OperandType.BOOLEAN, false);
        }

        private Operand Func_True(List<Operand> arg)
        {
            return new Operand(OperandType.BOOLEAN, true);
        }

        private Operand Func_False(List<Operand> arg)
        {
            return new Operand(OperandType.BOOLEAN, false);
        }

        private Operand Func_Or(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("OR 中参数不足", new List<Operand>());
            var b = false;

            foreach (var item in arg) {
                if (item.Type == OperandType.BOOLEAN) {
                    if (item.BooleanValue == true) {
                        b = true;
                        break;
                    }
                } else if (item.Type == OperandType.NUMBER) {
                    if (item.IntValue != 0) {
                        b = true;
                        break;
                    }
                }
            }
            return new Operand(OperandType.BOOLEAN, b);
        }

        private Operand Func_And(List<Operand> arg)
        {
            if (arg.Count < 1) return ThrowError("AND 中参数不足", new List<Operand>());
            var b = true;

            foreach (var item in arg) {
                if (item.Type == OperandType.BOOLEAN) {
                    if (item.BooleanValue == false) {
                        b = false;
                        break;
                    }
                } else if (item.Type == OperandType.NUMBER) {
                    if (item.IntValue == 0) {
                        b = false;
                        break;
                    }
                }
            }
            return new Operand(OperandType.BOOLEAN, b);
        }
    }
}
