using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static mathParser;

namespace ToolGood.Algorithm.Internals
{
    internal class ConditionCache
    {
        public string CategoryName;

        public string Remark;

        public string ConditionString;
        public ProgContext Condition;

        public string FormulaString;
        public ProgContext Formula;
    }
}
