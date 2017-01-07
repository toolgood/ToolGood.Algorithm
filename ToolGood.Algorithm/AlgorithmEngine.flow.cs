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
            addFunc("AND", AND);//如果所有参数均为TRUE，则返回TRUE
            addFunc("IF", IF);//指定要执行的逻辑检测
            addFunc("NOT", NOT);//对参数的逻辑值求反
            addFunc("OR", OR);//如果任一参数为TRUE，则返回TRUE
            addFunc("FALSE", FALSE);//返回逻辑值FALSE
            addFunc("TRUE", TRUE);//返回逻辑值TRUE
            addFunc("IFERROR", IFERROR);//指定要执行的逻辑检测
            addFunc("IFNUMBER", IFNUMBER);//指定要执行的逻辑检测
            addFunc("IFTEXT", IFTEXT);//指定要执行的逻辑检测


            addFunc("ISNUMBER", ISNUMBER);//指定要执行的逻辑检测
            addFunc("ISTEXT", ISTEXT);//指定要执行的逻辑检测


        }
        private Operand IFTEXT(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("ISSTRING中参数不足", new List<Operand>());
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


        private Operand IFNUMBER(List<Operand> arg)
        {
            if (arg.Count < 2) return throwError("IFNUMBER中参数不足", new List<Operand>());
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


        private Operand ISTEXT(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("ISSTRING中参数不足", new List<Operand>());
            if (arg[0].Type == OperandType.STRING) {
                return new Operand(OperandType.BOOLEAN, true);
            } else if (arg[0].Type == OperandType.DATE) {
                return new Operand(OperandType.BOOLEAN, arg[0].DateValue.srcText != null);
            }
            return new Operand(OperandType.BOOLEAN, false);
        }

        private Operand ISNUMBER(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("ISNUMBER中参数不足", new List<Operand>());
            if (arg[0].Type == OperandType.NUMBER) {
                return new Operand(OperandType.BOOLEAN, true);
            } else if (arg[0].Type == OperandType.DATE) {
                return new Operand(OperandType.BOOLEAN, true);
            }
            return new Operand(OperandType.BOOLEAN, false);
        }

        private Operand IFERROR(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("IFERROR中参数不足", new List<Operand>());
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

        private Operand IF(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("IF中参数不足", new List<Operand>());
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

        private Operand NOT(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("NOT中参数不足", new List<Operand>());
            if (arg[0].Type == OperandType.BOOLEAN) {
                return new Operand(OperandType.BOOLEAN, !arg[0].BooleanValue);
            } else if (arg[0].Type == OperandType.NUMBER) {
                return new Operand(OperandType.BOOLEAN, (arg[0].NumberValue == 0));

            }
            return new Operand(OperandType.BOOLEAN, false);
        }

        private Operand TRUE(List<Operand> arg)
        {
            return new Operand(OperandType.BOOLEAN, true);
        }

        private Operand FALSE(List<Operand> arg)
        {
            return new Operand(OperandType.BOOLEAN, false);
        }

        private Operand OR(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("OR中参数不足", new List<Operand>());
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

        private Operand AND(List<Operand> arg)
        {
            if (arg.Count < 1) return throwError("AND中参数不足", new List<Operand>());
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
