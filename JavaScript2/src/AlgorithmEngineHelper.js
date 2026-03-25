import { AntlrCharStream } from './AntlrCharStream.js';


// 导入ANTLR生成的文件
import mathLexer from './math/mathLexer.js';
import CommonTokenStream from './antlr4/CommonTokenStream.js';
import mathParser from './math/mathParser.js';
import mathVisitor from './math/mathVisitor.js';
import { AntlrErrorTextWriter } from './AntlrErrorTextWriter.js';



export class AlgorithmEngineHelper {
  
    /**
     * 编译公式
     */
    static ParseFormula(exp,errorListener) {
        if (!exp || exp.trim() === '') {
            throw new Error("Parameter exp invalid !");
        }
        let antlrErrorTextWriter = new AntlrErrorTextWriter();
        let stream =new AntlrCharStream(exp);
        let lexer = new mathLexer(stream, null, antlrErrorTextWriter);
        lexer.removeErrorListeners();
        lexer.addErrorListener(antlrErrorTextWriter);
        let tokens = new CommonTokenStream(lexer);
        let parser = new mathParser(tokens, null, antlrErrorTextWriter);
        parser.removeErrorListeners();
        parser.addErrorListener(antlrErrorTextWriter);
        if (errorListener) {
            parser.addErrorListener(errorListener);
            let context = parser.prog();
            let visitor = new mathVisitor();
            return visitor.visit(context);
        }
        let context = parser.prog();
        if (antlrErrorTextWriter.IsError) {
            throw new Error(antlrErrorTextWriter.ErrorMsg);
        }
        let visitor = new mathVisitor();
        return visitor.visit(context);
    }


}