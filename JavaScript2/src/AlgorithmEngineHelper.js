import { AntlrCharStream } from './AntlrCharStream.js';
import mathjsLexer from './math/mathjsLexer.js';
import CommonTokenStream from './antlr4/CommonTokenStream.js';
import mathjsParser from './math/mathjsParser.js';

export class AlgorithmEngineHelper {
    static ParseFormula(exp,errorListener) {
        if (!exp || exp.trim() === '') {
            throw new Error("Parameter exp invalid !");
        }
        if (!errorListener ) {
            throw new Error("Parameter errorListener invalid !");
        }
        let stream =new AntlrCharStream(exp);
        let lexer = new mathjsLexer(stream );
        lexer.removeErrorListeners();
        lexer.addErrorListener(errorListener);
        let tokens = new CommonTokenStream(lexer);
        let parser = new mathjsParser(tokens);
        parser.removeErrorListeners();
        parser.addErrorListener(errorListener);
        return parser.prog();
    }
}