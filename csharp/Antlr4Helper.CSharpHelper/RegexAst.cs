using System;
using System.Collections.Generic;

namespace Antlr4Helper.CSharpHelper.RegexEngine
{
    public abstract class RegexNode
    {
        public abstract void Accept(IRegexNodeVisitor visitor);
        public abstract RegexNode Clone();
    }

    public interface IRegexNodeVisitor
    {
        void Visit(CharNode node);
        void Visit(CharClassNode node);
        void Visit(AnyCharNode node);
        void Visit(ConcatNode node);
        void Visit(AlternationNode node);
        void Visit(QuantifierNode node);
        void Visit(GroupNode node);
        void Visit(AnchorNode node);
        void Visit(EmptyNode node);
    }

    public sealed class CharNode : RegexNode
    {
        public char Value { get; }
        public CharNode(char value) { Value = value; }
        public override void Accept(IRegexNodeVisitor visitor) => visitor.Visit(this);
        public override RegexNode Clone() => new CharNode(Value);
        public override string ToString() => Value == '.' || Value == '*' || Value == '+' || Value == '?' || Value == '|' || Value == '(' || Value == ')' || Value == '[' || Value == ']' || Value == '{' || Value == '}' || Value == '^' || Value == '$' || Value == '\\' || Value == '-'
            ? "\\" + Value
            : Value.ToString();
    }

    public sealed class CharClassNode : RegexNode
    {
        public List<(char Min, char Max)> Ranges { get; }
        public bool Negated { get; }
        public CharClassNode(List<(char Min, char Max)> ranges, bool negated)
        {
            Ranges = ranges;
            Negated = negated;
        }
        public override void Accept(IRegexNodeVisitor visitor) => visitor.Visit(this);
        public override RegexNode Clone() => new CharClassNode(new List<(char, char)>(Ranges), Negated);
        public override string ToString()
        {
            var result = Negated ? "[^" : "[";
            foreach (var range in Ranges)
            {
                if (range.Min == range.Max)
                    result += EscapeChar(range.Min);
                else
                    result += EscapeChar(range.Min) + "-" + EscapeChar(range.Max);
            }
            return result + "]";
        }
        private static string EscapeChar(char c)
        {
            return c == ']' || c == '\\' || c == '^' || c == '-' ? "\\" + c : c.ToString();
        }
    }

    public sealed class AnyCharNode : RegexNode
    {
        public override void Accept(IRegexNodeVisitor visitor) => visitor.Visit(this);
        public override RegexNode Clone() => new AnyCharNode();
        public override string ToString() => ".";
    }

    public sealed class ConcatNode : RegexNode
    {
        public List<RegexNode> Children { get; }
        public ConcatNode(List<RegexNode> children) { Children = children; }
        public override void Accept(IRegexNodeVisitor visitor) => visitor.Visit(this);
        public override RegexNode Clone()
        {
            var children = new List<RegexNode>();
            foreach (var child in Children)
                children.Add(child.Clone());
            return new ConcatNode(children);
        }
        public override string ToString() => string.Join("", Children);
    }

    public sealed class AlternationNode : RegexNode
    {
        public List<RegexNode> Alternatives { get; }
        public AlternationNode(List<RegexNode> alternatives) { Alternatives = alternatives; }
        public override void Accept(IRegexNodeVisitor visitor) => visitor.Visit(this);
        public override RegexNode Clone()
        {
            var alternatives = new List<RegexNode>();
            foreach (var alt in Alternatives)
                alternatives.Add(alt.Clone());
            return new AlternationNode(alternatives);
        }
        public override string ToString() => "(" + string.Join("|", Alternatives) + ")";
    }

    public sealed class QuantifierNode : RegexNode
    {
        public RegexNode Child { get; }
        public int MinCount { get; }
        public int? MaxCount { get; }
        public bool Greedy { get; }
        public QuantifierNode(RegexNode child, int minCount, int? maxCount, bool greedy = true)
        {
            Child = child;
            MinCount = minCount;
            MaxCount = maxCount;
            Greedy = greedy;
        }
        public override void Accept(IRegexNodeVisitor visitor) => visitor.Visit(this);
        public override RegexNode Clone() => new QuantifierNode(Child.Clone(), MinCount, MaxCount, Greedy);
        public override string ToString()
        {
            var quantifier = "";
            if (MinCount == 0 && MaxCount == null) quantifier = "*";
            else if (MinCount == 1 && MaxCount == null) quantifier = "+";
            else if (MinCount == 0 && MaxCount == 1) quantifier = "?";
            else if (MaxCount == null) quantifier = "{" + MinCount + ",}";
            else if (MinCount == MaxCount) quantifier = "{" + MinCount + "}";
            else quantifier = "{" + MinCount + "," + MaxCount + "}";
            if (!Greedy) quantifier += "?";
            return Child.ToString() + quantifier;
        }
    }

    public sealed class GroupNode : RegexNode
    {
        public RegexNode Child { get; }
        public int GroupIndex { get; set; }
        public bool IsCapturing { get; }
        public GroupNode(RegexNode child, int groupIndex, bool isCapturing = true)
        {
            Child = child;
            GroupIndex = groupIndex;
            IsCapturing = isCapturing;
        }
        public override void Accept(IRegexNodeVisitor visitor) => visitor.Visit(this);
        public override RegexNode Clone() => new GroupNode(Child.Clone(), GroupIndex, IsCapturing);
        public override string ToString() => IsCapturing ? "(" + Child + ")" : "(?:" + Child + ")";
    }

    public sealed class AnchorNode : RegexNode
    {
        public AnchorType Type { get; }
        public AnchorNode(AnchorType type) { Type = type; }
        public override void Accept(IRegexNodeVisitor visitor) => visitor.Visit(this);
        public override RegexNode Clone() => new AnchorNode(Type);
        public override string ToString() => Type == AnchorType.Start ? "^" : "$";
    }

    public sealed class EmptyNode : RegexNode
    {
        public override void Accept(IRegexNodeVisitor visitor) => visitor.Visit(this);
        public override RegexNode Clone() => new EmptyNode();
        public override string ToString() => "";
    }

    public enum AnchorType
    {
        Start,
        End
    }
}
