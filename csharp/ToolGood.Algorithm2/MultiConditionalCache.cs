using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Internals;
using static mathParser;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 多条件缓存
    /// </summary>
    public class MultiConditionCache
    {
        private Dictionary<string, List<ConditionCache>> conditionCaches = new Dictionary<string, List<ConditionCache>>();
        /// <summary>
        /// 最后一个错误
        /// </summary>
        public string LastError;

        /// <summary>
        /// 添加公式缓存
        /// </summary>
        /// <param name="categoryName">分类名称</param>
        /// <param name="formula">公式</param>
        /// <param name="condition">条件</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public bool AddFormulaCache(string categoryName, string formula, string condition = null, string remark = null)
        {
            ConditionCache conditionCache = new ConditionCache() {
                CategoryName = categoryName,
                Remark = remark,
                ConditionString = condition,
                FormulaString = formula
            };
            if (condition != null) {
                var conditionProg = Parse(condition);
                if (conditionProg == null) {
                    return false;
                }
                conditionCache.Condition = conditionProg;
            }
            var formulaProg = Parse(formula);
            if (formulaProg == null) {
                return false;
            }
            conditionCache.Formula = formulaProg;

            List<ConditionCache> list;
            if (conditionCaches.TryGetValue(categoryName, out list) == false) {
                list = new List<ConditionCache>();
                conditionCaches[categoryName] = list;
            }

            list.Add(conditionCache);
            return true;
        }

        /// <summary>
        /// 添加 条件缓存，
        /// 有先后顺序的
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="condition"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddConditionCache(string categoryName, string condition, string remark)
        {
            ConditionCache conditionCache = new ConditionCache() {
                CategoryName = categoryName,
                Remark = remark,
                ConditionString = condition,
            };
            if (condition != null) {
                var conditionProg = Parse(condition);
                if (conditionProg == null) {
                    return false;
                }
                conditionCache.Condition = conditionProg;
            }
            List<ConditionCache> list;
            if (conditionCaches.TryGetValue(categoryName, out list) == false) {
                list = new List<ConditionCache>();
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

        internal List<ConditionCache> GetConditionCaches(string name)
        {
            List<ConditionCache> result;
            if (conditionCaches.TryGetValue(name, out result) == false) {
                result = new List<ConditionCache>();
            }
            return result;
        }

    }
}
