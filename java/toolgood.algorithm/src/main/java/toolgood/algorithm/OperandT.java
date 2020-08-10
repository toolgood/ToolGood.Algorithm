package toolgood.algorithm;

import java.util.List;

abstract class OperandT<T> extends Operand {
    public T Value;
    public OperandT(final T obj)
    {
        Value = obj;
    }
 
}