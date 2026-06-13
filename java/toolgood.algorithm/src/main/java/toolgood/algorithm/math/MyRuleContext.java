package toolgood.algorithm.math;

import org.antlr.v4.runtime.ParserRuleContext;

abstract class MyRuleContext extends ParserRuleContext {
    public MyRuleContext() {
        super();
    }

    public MyRuleContext(ParserRuleContext parent, int invokingState) {
        super(parent, invokingState);
    }
}
