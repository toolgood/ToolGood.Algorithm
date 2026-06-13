package toolgood.algorithm.internals.visitors;

import org.antlr.v4.runtime.tree.AbstractParseTreeVisitor;
import org.antlr.v4.runtime.tree.TerminalNode;

import toolgood.algorithm.math.mathBaseVisitor;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.*;
import toolgood.algorithm.internals.DiyNameKeyInfo;

import java.util.ArrayList;
import java.util.List;

public class DiyNameVisitor extends mathBaseVisitor<Object> {
    public List<DiyNameKeyInfo> Parameters = new ArrayList<>();
    public List<DiyNameKeyInfo> Functions = new ArrayList<>();

    private void AddParameter(TerminalNode node) {
        if (node != null) {
            DiyNameKeyInfo info = new DiyNameKeyInfo();
            info.Name = node.getText();
            info.Start = node.getSymbol().getStartIndex();
            info.End = node.getSymbol().getStopIndex();
            Parameters.add(info);
        }
    }

    @Override
    public Object visitParameter2(mathParser.Parameter2Context ctx) {
        return visitChildren(ctx);
    }

    @Override
    public Object visitArrayJson(mathParser.ArrayJsonContext ctx) {
        return visitChildren(ctx);
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
    public Object visitFunction_fun(Function_funContext context) {
        TerminalNode node = context.PARAMETER();
        if (node != null) {
            DiyNameKeyInfo info = new DiyNameKeyInfo();
            info.Name = node.getText();
            info.Start = node.getSymbol().getStartIndex();
            info.End = node.getSymbol().getStopIndex();
            Functions.add(info);
        }
        return visitChildren(context);
    }
}
