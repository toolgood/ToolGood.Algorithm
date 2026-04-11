/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    [System.Serializable]
    internal class CommonToken : IWritableToken
    {
        protected internal static readonly Tuple<ITokenSource, ICharStream> EmptySource = Tuple.Create<ITokenSource, ICharStream>(null, null);
        private int _type;
        private int _line;
        protected internal int charPositionInLine = -1;
        private int _channel = TokenConstants.DefaultChannel;
        protected internal Tuple<ITokenSource, ICharStream> source;
        private string _text;
        protected internal int index = -1;
        protected internal int start;
        protected internal int stop;
        public CommonToken(Tuple<ITokenSource, ICharStream> source, int type, int channel, int start, int stop)
        {
            this.source = source;
            this._type = type;
            this._channel = channel;
            this.start = start;
            this.stop = stop;
            if (source.Item1 != null)
            {
                this._line = source.Item1.Line;
                this.charPositionInLine = source.Item1.Column;
            }
        }
        public CommonToken(int type, string text)
        {
            this._type = type;
            this._channel = TokenConstants.DefaultChannel;
            this._text = text;
            this.source = EmptySource;
        }
        public virtual int Type
        {
            get
            {
                return _type;
            }
            set
            {
 				this._type = value;
            }
        }
        public virtual int Line
        {
            get
            {
                return _line;
            }
            set
            {
 				this._line = value;
            }
        }
        public virtual string Text
        {
            get
            {
                if (_text != null)
                {
                    return _text;
                }
                ICharStream input = InputStream;
                if (input == null)
                {
                    return null;
                }
                int n = input.Size;
                if (start < n && stop < n)
                {
                    return input.GetText(Interval.Of(start, stop));
                }
                else
                {
                    return "<EOF>";
                }
            }
            set
            {
 				this._text = value;
            }
        }
        public virtual int Column
        {
            get
            {
                return charPositionInLine;
            }
            set
            {
                int charPositionInLine = value;
                this.charPositionInLine = charPositionInLine;
            }
        }
        public virtual int Channel
        {
            get
            {
                return _channel;
            }
            set
            {
                this._channel = value;
            }
        }
        public virtual int StartIndex
        {
            get
            {
                return start;
            }
            set
            {
                int start = value;
                this.start = start;
            }
        }
        public virtual int StopIndex
        {
            get
            {
                return stop;
            }
            set
            {
                int stop = value;
                this.stop = stop;
            }
        }
        public virtual int TokenIndex
        {
            get
            {
                return index;
            }
            set
            {
                int index = value;
                this.index = index;
            }
        }
        public virtual ITokenSource TokenSource
        {
            get
            {
                return source.Item1;
            }
        }
        public virtual ICharStream InputStream
        {
            get
            {
                return source.Item2;
            }
        }
    }
}
