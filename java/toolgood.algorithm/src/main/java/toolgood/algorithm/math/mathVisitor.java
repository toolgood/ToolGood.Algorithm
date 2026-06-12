// Generated from math.g4 by ANTLR 4.13.2
package toolgood.algorithm.math;
import org.antlr.v4.runtime.tree.ParseTreeVisitor;

/**
 * This interface defines a complete generic visitor for a parse tree produced
 * by {@link mathParser}.
 *
 * @param <T> The return type of the visit operation. Use {@link Void} for
 * operations with no return type.
 */
public interface mathVisitor<T> extends ParseTreeVisitor<T> {
	/**
	 * Visit a parse tree produced by {@link mathParser#prog}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitProg(mathParser.ProgContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Function_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitFunction_fun(mathParser.Function_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Or_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitOr_fun(mathParser.Or_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code IF_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitIF_fun(mathParser.IF_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Judge_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitJudge_fun(mathParser.Judge_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Percentage_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPercentage_fun(mathParser.Percentage_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code STRING_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitSTRING_fun(mathParser.STRING_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code AddSub_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAddSub_fun(mathParser.AddSub_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code ArrayJson_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitArrayJson_fun(mathParser.ArrayJson_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code GetJsonValue_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGetJsonValue_fun(mathParser.GetJsonValue_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code And_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAnd_fun(mathParser.And_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Array_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitArray_fun(mathParser.Array_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code MulDiv_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitMulDiv_fun(mathParser.MulDiv_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NOT_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNOT_fun(mathParser.NOT_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CONST2_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCONST2_fun(mathParser.CONST2_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code Bracket_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBracket_fun(mathParser.Bracket_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code CONST_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitCONST_fun(mathParser.CONST_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code NUM_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNUM_fun(mathParser.NUM_funContext ctx);
	/**
	 * Visit a parse tree produced by the {@code PARAMETER_fun}
	 * labeled alternative in {@link mathParser#expr}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitPARAMETER_fun(mathParser.PARAMETER_funContext ctx);
	/**
	 * Visit a parse tree produced by {@link mathParser#arrayJson}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitArrayJson(mathParser.ArrayJsonContext ctx);
	/**
	 * Visit a parse tree produced by {@link mathParser#parameter2}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParameter2(mathParser.Parameter2Context ctx);
}