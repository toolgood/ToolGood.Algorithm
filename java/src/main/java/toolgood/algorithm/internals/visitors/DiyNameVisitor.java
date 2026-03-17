package toolgood.algorithm.internals.visitors;

import org.antlr.v4.runtime.tree.TerminalNode;

import toolgood.algorithm.math.mathBaseVisitor;
import toolgood.algorithm.math.mathParser.*;
import toolgood.algorithm.internals.DiyNameInfo;

public final class DiyNameVisitor extends mathBaseVisitor<Object> {
    public DiyNameInfo diy = new DiyNameInfo();

    private void AddParameter(TerminalNode node) {
        if (node != null) {
            DiyNameInfo.KeyInfo keyInfo = diy.new KeyInfo();
            keyInfo.Name = node.getText();
            keyInfo.Start = node.getSymbol().getStartIndex();
            keyInfo.End = node.getSymbol().getStopIndex();
            diy.Parameters.add(keyInfo);
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
        DiyNameInfo.KeyInfo keyInfo = diy.new KeyInfo();
        keyInfo.Name = node.getText();
        keyInfo.Start = node.getSymbol().getStartIndex();
        keyInfo.End = node.getSymbol().getStopIndex();
        diy.Functions.add(keyInfo);
        return visitChildren(context);
    }
}
