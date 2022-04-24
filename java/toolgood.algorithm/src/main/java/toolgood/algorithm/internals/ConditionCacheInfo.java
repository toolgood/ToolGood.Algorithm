package toolgood.algorithm.internals;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;

import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.ProgContext;

public class ConditionCacheInfo {
    public String CategoryName;

    public String Remark;

    public String ConditionString;

    private ProgContext _ConditionProg;

    public ProgContext GetConditionProg() {
        if (_ConditionProg == null && ConditionString != null && ConditionString.length() > 0 && LastError == null) {
            _ConditionProg = Parse(ConditionString);
        }
        return _ConditionProg;
    }

    public void SetConditionProg(ProgContext value) {
        _ConditionProg = value;
    }

    public String FormulaString;
    private ProgContext _FormulaProg;

    public ProgContext GetFormulaProg() {
        if (_FormulaProg == null && FormulaString != null && FormulaString.length() > 0 && LastError == null) {
            _FormulaProg = Parse(FormulaString);
        }
        return _FormulaProg;
    }

    public void SetFormulaProg(ProgContext value) {
        _FormulaProg = value;
    }

    public String LastError;

    public ProgContext Parse(final String exp) {
        if (exp == null || exp.equals("")) {
            LastError = "Parameter exp invalid !";
            return null;
        }
        try {
            final AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
            final mathLexer lexer = new mathLexer(stream);
            final CommonTokenStream tokens = new CommonTokenStream(lexer);
            final mathParser parser = new mathParser(tokens);
            final AntlrErrorListener antlrErrorListener = new AntlrErrorListener();
            parser.removeErrorListeners();
            parser.addErrorListener(antlrErrorListener);
            final ProgContext context = parser.prog();

            if (antlrErrorListener.IsError) {
                LastError = antlrErrorListener.ErrorMsg;
                return null;
            }
            return context;
        } catch (Exception ex) {
            LastError = ex.getMessage();
        }
        return null;
    }
}
