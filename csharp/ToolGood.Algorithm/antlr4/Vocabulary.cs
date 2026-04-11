/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    internal class Vocabulary : IVocabulary
    {
        private static readonly string[] EmptyNames = new string[0];
        public static readonly Vocabulary EmptyVocabulary = new Vocabulary(EmptyNames, EmptyNames, EmptyNames);
        private readonly string[] literalNames;
        private readonly string[] symbolicNames;
        private readonly string[] displayNames;
        public Vocabulary(string[] literalNames, string[] symbolicNames)
            : this(literalNames, symbolicNames, null)
        {
        }
        public Vocabulary(string[] literalNames, string[] symbolicNames, string[] displayNames)
        {
            this.literalNames = literalNames != null ? literalNames : EmptyNames;
            this.symbolicNames = symbolicNames != null ? symbolicNames : EmptyNames;
            this.displayNames = displayNames != null ? displayNames : EmptyNames;
        }
        public virtual string GetLiteralName(int tokenType)
        {
            if (tokenType >= 0 && tokenType < literalNames.Length)
            {
                return literalNames[tokenType];
            }
            return null;
        }
        public virtual string GetSymbolicName(int tokenType)
        {
            if (tokenType >= 0 && tokenType < symbolicNames.Length)
            {
                return symbolicNames[tokenType];
            }
            if (tokenType == TokenConstants.EOF)
            {
                return "EOF";
            }
            return null;
        }
        public virtual string GetDisplayName(int tokenType)
        {
            if (tokenType >= 0 && tokenType < displayNames.Length)
            {
                string displayName = displayNames[tokenType];
                if (displayName != null)
                {
                    return displayName;
                }
            }
            string literalName = GetLiteralName(tokenType);
            if (literalName != null)
            {
                return literalName;
            }
            string symbolicName = GetSymbolicName(tokenType);
            if (symbolicName != null)
            {
                return symbolicName;
            }
            return tokenType.ToString();
        }
    }
}
