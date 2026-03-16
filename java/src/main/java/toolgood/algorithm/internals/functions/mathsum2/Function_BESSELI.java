package toolgood.algorithm.internals.functions.mathsum2;

import java.math.BigDecimal;
import java.math.MathContext;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public class Function_BESSELI extends Function_2 {
    public Function_BESSELI(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    public Function_BESSELI(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "BesselI";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber(engine, tempParameter, 0);
        if (args1.IsError()) return args1;
        
        Operand args2 = GetNumber(engine, tempParameter, 1);
        if (args2.IsError()) return args2;

        BigDecimal x = args1.NumberValue();
        int n = (int) args2.DoubleValue();

        return Operand.Create(besselI(n, x));
    }

    private static BigDecimal besselI(int n, BigDecimal x) {
        // 处理 x < 0 的情�?
        if (x.compareTo(BigDecimal.ZERO) < 0) {
            int sign = (n % 2 == 0) ? 1 : -1;
            return besselI(n, x.negate()).multiply(BigDecimal.valueOf(sign));
        }
        // x == 0 的情�?
        if (x.compareTo(BigDecimal.ZERO) == 0) {
            return n == 0 ? BigDecimal.ONE : BigDecimal.ZERO;
        }

        BigDecimal ax = x.abs();
        BigDecimal epsilon = new BigDecimal("1e-10");
        if (ax.compareTo(epsilon) < 0) {
            return n == 0 ? BigDecimal.ONE : BigDecimal.ZERO;
        }

        if (n < 0) n = -n;

        // x > 700 时使用近似公�?
        if (ax.compareTo(new BigDecimal("700")) > 0) {
            BigDecimal exp = new BigDecimal(Math.exp(ax.doubleValue()));
            BigDecimal sqrtTerm = new BigDecimal(Math.sqrt(2 * Math.PI * ax.doubleValue()));
            BigDecimal factor = exp.divide(sqrtTerm, MathContext.DECIMAL128);
            BigDecimal correction = BigDecimal.ONE.subtract(
                new BigDecimal("4.0").multiply(new BigDecimal(n * n)).subtract(new BigDecimal("1.0"))
                    .divide(new BigDecimal("8.0").multiply(ax), MathContext.DECIMAL128)
            );
            return factor.multiply(correction);
        }

        if (n == 0) return besselI0(x);
        if (n == 1) return besselI1(x);

        BigDecimal I0 = besselI0(x);
        BigDecimal I1 = besselI1(x);
        BigDecimal In = BigDecimal.ZERO;

        for (int k = 1; k < n; k++) {
            In = I1.add(new BigDecimal("2.0").multiply(new BigDecimal(k)).divide(x, MathContext.DECIMAL128).multiply(I0));
            I0 = I1;
            I1 = In;
        }

        return I1;
    }

    private static BigDecimal besselI0(BigDecimal x) {
        BigDecimal ax = x.abs();
        BigDecimal threePoint75 = new BigDecimal("3.75");
        
        if (ax.compareTo(threePoint75) < 0) {
            BigDecimal y1 = x.divide(threePoint75, MathContext.DECIMAL128);
            y1 = y1.multiply(y1);
            return BigDecimal.ONE.add(y1.multiply(
                new BigDecimal("3.515622965380465").add(y1.multiply(
                    new BigDecimal("3.089942465562116").add(y1.multiply(
                        new BigDecimal("1.206749160761352").add(y1.multiply(
                            new BigDecimal("0.265973256598487").add(y1.multiply(
                                new BigDecimal("0.036076845538912").add(y1.multiply(new BigDecimal("0.004581327358717")))
                            ))
                        ))
                    ))
                ))
            ));
        }
        
        BigDecimal y2 = threePoint75.divide(ax, MathContext.DECIMAL128);
        BigDecimal exp = new BigDecimal(Math.exp(ax.doubleValue()));
        BigDecimal sqrtTerm = new BigDecimal(Math.sqrt(2.0 * Math.PI * ax.doubleValue()));
        BigDecimal factor = exp.divide(sqrtTerm, MathContext.DECIMAL128);
        
        return factor.multiply(
            new BigDecimal("0.398942280401433").add(y2.multiply(
                new BigDecimal("0.013285921344730").add(y2.multiply(
                    new BigDecimal("0.002253193626842").add(y2.multiply(
                        new BigDecimal("-0.001575649875251").add(y2.multiply(
                            new BigDecimal("0.009162816703917").add(y2.multiply(
                                new BigDecimal("-0.020577062932649").add(y2.multiply(
                                    new BigDecimal("0.026355373177924").add(y2.multiply(
                                        new BigDecimal("-0.016476329612910").add(y2.multiply(new BigDecimal("0.003923769605236")))
                                    ))
                                ))
                            ))
                        ))
                    ))
                ))
            ))
        );
    }

    private static BigDecimal besselI1(BigDecimal x) {
        BigDecimal ax = x.abs();
        BigDecimal threePoint75 = new BigDecimal("3.75");
        
        if (ax.compareTo(threePoint75) < 0) {
            BigDecimal y1 = x.divide(threePoint75, MathContext.DECIMAL128);
            y1 = y1.multiply(y1);
            return x.multiply(
                new BigDecimal("0.5").add(y1.multiply(
                    new BigDecimal("0.878905941521392").add(y1.multiply(
                        new BigDecimal("0.514988692842374").add(y1.multiply(
                            new BigDecimal("0.150849342225664").add(y1.multiply(
                                new BigDecimal("0.026587328231117").add(y1.multiply(
                                    new BigDecimal("0.003015319414231").add(y1.multiply(new BigDecimal("0.000324111013968")))
                                ))
                            ))
                        ))
                    ))
                ))
            );
        }
        
        BigDecimal y2 = threePoint75.divide(ax, MathContext.DECIMAL128);
        BigDecimal exp = new BigDecimal(Math.exp(ax.doubleValue()));
        BigDecimal sqrtTerm = new BigDecimal(Math.sqrt(2.0 * Math.PI * ax.doubleValue()));
        BigDecimal factor = exp.divide(sqrtTerm, MathContext.DECIMAL128);
        
        BigDecimal ans = factor.multiply(
            new BigDecimal("0.398942280401433").add(y2.multiply(
                new BigDecimal("-0.039880242337502").add(y2.multiply(
                    new BigDecimal("-0.003620182649157").add(y2.multiply(
                        new BigDecimal("0.001638105403528").add(y2.multiply(
                            new BigDecimal("-0.010315550635288").add(y2.multiply(
                                new BigDecimal("0.022829679456897").add(y2.multiply(
                                    new BigDecimal("-0.028953129286367").add(y2.multiply(
                                        new BigDecimal("0.017876545768998").subtract(y2.multiply(new BigDecimal("0.004200596567986")))
                                    ))
                                ))
                            ))
                        ))
                    ))
                ))
            ))
        );
        
        return x.compareTo(BigDecimal.ZERO) < 0 ? ans.negate() : ans;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void getParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].getParameterTypes(noneEngine, result, OperandType.NUMBER, null, null);
        funcs[1].getParameterTypes(noneEngine, result, OperandType.NUMBER, null, null);
    }
}
