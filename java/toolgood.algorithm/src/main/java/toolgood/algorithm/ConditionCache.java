package toolgood.algorithm;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;

import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser2.ProgContext;
import toolgood.algorithm.internals.AntlrErrorListener;
import toolgood.algorithm.internals.CaseChangingCharStream;
import toolgood.algorithm.internals.ConditionCacheInfo;

public class ConditionCache {
    private Map<String, ArrayList<ConditionCacheInfo>> conditionCaches = new HashMap<String, ArrayList<ConditionCacheInfo>>();
    /**
     * 最后一个错误
     */
    public String LastError;
    /**
     * 是否开启延迟加载
     * 开启后，不会立即调取Parse
     */
    public Boolean LazyLoad = false;

    /**
     * 添加公式缓存
     * 有先后顺序的
     * 
     * @param categoryName 分类名称
     * @param condition    条件 可空
     * @param formula      公式 必埴
     * @param remark       备注
     * @return
     */
    public Boolean AddFormula(String categoryName, String condition, String formula, String remark) {
        if (categoryName == null || categoryName.length() == 0) {
            LastError = "Parameter categoryName is null or empty.";
            return false;
        }
        if (formula == null || formula.length() == 0) {
            LastError = "Parameter formula is null or empty.";
            return false;
        }

        ConditionCacheInfo conditionCache = new ConditionCacheInfo();
        conditionCache.CategoryName = categoryName;
        conditionCache.Remark = remark;
        conditionCache.ConditionString = condition;
        conditionCache.FormulaString = formula;

        if (LazyLoad == false) {
            if (condition != null && condition.length() > 0) {
                ProgContext conditionProg = Parse(condition);
                if (conditionProg == null) {
                    return false;
                }
                conditionCache.SetConditionProg(conditionProg);
            }
            ProgContext formulaProg = Parse(formula);
            if (formulaProg == null) {
                return false;
            }
            conditionCache.SetFormulaProg(formulaProg);
        }
        if (conditionCaches.containsKey(categoryName)) {
            conditionCaches.get(categoryName).add(conditionCache);
        } else {
            ArrayList<ConditionCacheInfo> list = new ArrayList<ConditionCacheInfo>();
            list.add(conditionCache);
            conditionCaches.put(categoryName, list);
        }

        return true;
    }

    /**
     * 添加 条件缓存，
     * 有先后顺序的
     * @param categoryName 分类名称
     * @param condition 条件 可空
     * @param remark 备注 必埴
     * @return
     */
    public Boolean AddCondition(String categoryName, String condition, String remark) {
        if (categoryName == null || categoryName.length() == 0) {
            LastError = "Parameter categoryName is null or empty.";
            return false;
        }
        if (remark == null || remark.length() == 0) {
            LastError = "Parameter remark is null or empty.";
            return false;
        }
        ConditionCacheInfo conditionCache = new ConditionCacheInfo();
        conditionCache.CategoryName = categoryName;
        conditionCache.Remark = remark;
        conditionCache.ConditionString = condition;

        if (LazyLoad == false) {
            if (condition != null && condition.length() > 0) {
                ProgContext conditionProg = Parse(condition);
                if (conditionProg == null) {
                    return false;
                }
                conditionCache.SetConditionProg(conditionProg);
            }
        }
        if (conditionCaches.containsKey(categoryName)) {
            conditionCaches.get(categoryName).add(conditionCache);
        } else {
            ArrayList<ConditionCacheInfo> list = new ArrayList<ConditionCacheInfo>();
            list.add(conditionCache);
            conditionCaches.put(categoryName, list);
        }

        return true;
    }

    private ProgContext Parse(String exp) {
        if (exp == null || exp.equals("")) {
            LastError = "Parameter exp invalid !";
            return null;
        }
        try {
            final CaseChangingCharStream stream = new CaseChangingCharStream(CharStreams.fromString(exp));
            final mathLexer lexer = new mathLexer(stream);
            final CommonTokenStream tokens = new CommonTokenStream(lexer);
            final mathParser parser = new mathParser(tokens);
            final AntlrErrorListener antlrErrorListener = new AntlrErrorListener();
            parser.removeErrorListeners();
            parser.addErrorListener(antlrErrorListener);
            final ProgContext context = parser.prog();

            final int end = context.stop.getStopIndex();
            if (end + 1 < exp.length()) {
                LastError = "Parameter exp invalid !";
                return null;
            }
            if (antlrErrorListener.IsError) {
                LastError = antlrErrorListener.ErrorMsg;
                return null;
            }

        } catch (Exception ex) {
            LastError = ex.getMessage();
        }
        return null;
    }

    public ArrayList<ConditionCacheInfo> GetConditionCaches(String name) {

        if (conditionCaches.containsKey(name)) {
            return conditionCaches.get(name);
        }
        return new ArrayList<ConditionCacheInfo>();
    }

}
