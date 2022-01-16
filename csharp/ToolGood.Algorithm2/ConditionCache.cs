using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Internals;
using static mathParser;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 条件缓存
    /// </summary>
    public class ConditionCache
    {
        private Dictionary<string, List<Internals.ConditionCache>> conditionCaches = new Dictionary<string, List<Internals.ConditionCache>>();
        /// <summary>
        /// 最后一个错误
        /// </summary>
        public string LastError;
        /// <summary>
        /// 是否开启延迟加载
        /// 开启后，不会立即调取Parse
        /// </summary>
        public bool LazyLoad = false;


        /// <summary>
        /// 添加公式缓存
        /// </summary>
        /// <param name="categoryName">分类名称</param>
        /// <param name="formula">公式 必埴</param>
        /// <param name="condition">条件 可空</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public bool AddFormula(string categoryName, string condition, string formula, string remark = null)
        {
            if (string.IsNullOrEmpty(categoryName)) {
                LastError = "Parameter categoryName is null or empty.";
                return false;
            }
            if (string.IsNullOrEmpty(formula)) {
                LastError = "Parameter formula is null or empty.";
                return false;
            }
            Internals.ConditionCache conditionCache = new Internals.ConditionCache() {
                CategoryName = categoryName,
                Remark = remark,
                ConditionString = condition,
                FormulaString = formula
            };

            if (LazyLoad == false) {
                if (string.IsNullOrEmpty(condition) == false) {
                    var conditionProg = Parse(condition);
                    if (conditionProg == null) {
                        return false;
                    }
                    conditionCache.ConditionProg = conditionProg;
                }
                var formulaProg = Parse(formula);
                if (formulaProg == null) {
                    return false;
                }
                conditionCache.FormulaProg = formulaProg;
            }

            List<Internals.ConditionCache> list;
            if (conditionCaches.TryGetValue(categoryName, out list) == false) {
                list = new List<Internals.ConditionCache>();
                conditionCaches[categoryName] = list;
            }

            list.Add(conditionCache);
            return true;
        }

        /// <summary>
        /// 添加 条件缓存，
        /// 有先后顺序的
        /// </summary>
        /// <param name="categoryName">分类名称</param>
        /// <param name="condition"></param>
        /// <param name="remark">备注 必埴</param>
        /// <returns></returns>
        public bool AddCondition(string categoryName, string condition, string remark)
        {
            if (string.IsNullOrEmpty(categoryName)) {
                LastError = "Parameter categoryName is null or empty.";
                return false;
            }
            if (string.IsNullOrEmpty(remark)) {
                LastError = "Parameter remark is null or empty.";
                return false;
            }
            Internals.ConditionCache conditionCache = new Internals.ConditionCache() {
                CategoryName = categoryName,
                Remark = remark,
                ConditionString = condition,
            };
            if (LazyLoad == false) {
                if (string.IsNullOrEmpty(condition) == false) {
                    var conditionProg = Parse(condition);
                    if (conditionProg == null) {
                        return false;
                    }
                    conditionCache.ConditionProg = conditionProg;
                }
            }
            List<Internals.ConditionCache> list;
            if (conditionCaches.TryGetValue(categoryName, out list) == false) {
                list = new List<Internals.ConditionCache>();
                conditionCaches[categoryName] = list;
            }

            list.Add(conditionCache);
            return true;
        }


        private ProgContext Parse(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp)) { return null; }
            try {
                var stream = new CaseChangingCharStream(new AntlrInputStream(exp));
                var lexer = new mathLexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new mathParser(tokens);
                var antlrErrorListener = new AntlrErrorListener();
                parser.RemoveErrorListeners();
                parser.AddErrorListener(antlrErrorListener);

                var context = parser.prog();
                var end = context.Stop.StopIndex;
                if (end + 1 < exp.Length) {
                    LastError = "Parameter exp invalid !";
                    return null;
                }
                if (antlrErrorListener.IsError) {
                    LastError = antlrErrorListener.ErrorMsg;
                    return null;
                }
                return context;
            } catch (Exception ex) {
                LastError = ex.Message;
            }
            return null;
        }

        internal List<Internals.ConditionCache> GetConditionCaches(string name)
        {
            List<Internals.ConditionCache> result;
            if (conditionCaches.TryGetValue(name, out result) == false) {
                result = new List<Internals.ConditionCache>();
            }
            return result;
        }

    }
}
