package toolgood.algorithm.internals.visitors;

import org.antlr.v4.runtime.tree.TerminalNode;

import toolgood.algorithm.math.mathBaseVisitor;
import toolgood.algorithm.math.mathParser.*;
import toolgood.algorithm.internals.DiyNameInfo;
import toolgood.algorithm.internals.DiyNameKeyInfo;

public final class DiyNameVisitor extends mathBaseVisitor<Object> {
    public DiyNameInfo diy = new DiyNameInfo();

    private void AddParameter(TerminalNode node) {
        if (node != null) {
            DiyNameKeyInfo keyInfo = new DiyNameKeyInfo();
            keyInfo.setName(node.getText());
            keyInfo.setStart(node.getSymbol().getStartIndex());
            keyInfo.setEnd(node.getSymbol().getStopIndex());
            diy.getParameters().add(keyInfo);
        }
    }

    @Override
    public Object visitPARAMETER_fun(PARAMETER_funContext context) {
        AddParameter(context.PARAMETER());
        return visitChildren(context);
    }

    @Override
    public Object visitGetJsonValue_fun(GetJsonValue_funContext context) {
        AddParameter(context.PARAMETER());
        return visitChildren(context);
    }

    @Override
    public Object visitDiyFunction_fun(DiyFunction_funContext context) {
        TerminalNode node = context.PARAMETER();
        DiyNameKeyInfo keyInfo = new DiyNameKeyInfo();
        keyInfo.setName(node.getText());
        keyInfo.setStart(node.getSymbol().getStartIndex());
        keyInfo.setEnd(node.getSymbol().getStopIndex());
        diy.getFunctions().add(keyInfo);
        return visitChildren(context);
    }
}
