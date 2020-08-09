package toolgood.algorithm;

import java.util.List;

abstract class OperandT<T> extends Operand {
    public   T Value;
    public OperandT(final T obj)
    {
        Value = obj;
    }
    @Override
	public boolean IsNull() {
        throw new Exception();
	}

    @Override
    public boolean IsError() {
        return false;
    }

    @Override
    public String ErrorMsg() {
        return null;
    }
    @Override
    public double NumberValue() {
        throw new Exception();

    }

    @Override
    public int IntValue() {
        throw new Exception();

    }

    @Override
    public String TextValue() {
        throw new Exception();

    }

    @Override
    public boolean BooleanValue() {
        throw new Exception();

    }

    @Override
    public List<Operand> ArrayValue() {
        throw new Exception();

    }

    @Override
    JsonData JsonValue() {
        throw new Exception();
    }

    @Override
	public Date DateValue() {
        throw new Exception();
	}
}