/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    internal class BufferedTokenStream : ITokenStream
    {
        private ITokenSource _tokenSource;
        protected internal IList<IToken> tokens = new List<IToken>(100);
        protected internal int p = -1;
        protected internal bool fetchedEOF;
        public BufferedTokenStream(ITokenSource tokenSource)
        {
            if (tokenSource == null)
            {
                throw new ArgumentNullException("tokenSource cannot be null");
            }
            this._tokenSource = tokenSource;
        }
        public virtual ITokenSource TokenSource
        {
            get
            {
                return _tokenSource;
            }
        }
        public virtual int Index
        {
            get
            {
                return p;
            }
        }
        public virtual int Mark()
        {
            return 0;
        }
        public virtual void Release(int marker)
        {
        }

        public virtual void Seek(int index)
        {
            LazyInit();
            p = AdjustSeekIndex(index);
        }
        public virtual int Size
        {
            get
            {
                return tokens.Count;
            }
        }
        public virtual void Consume()
        {
            bool skipEofCheck;
            if (p >= 0)
            {
                if (fetchedEOF)
                {
                    skipEofCheck = p < tokens.Count - 1;
                }
                else
                {
                    skipEofCheck = p < tokens.Count;
                }
            }
            else
            {
                skipEofCheck = false;
            }
            if (!skipEofCheck && LA(1) == IntStreamConstants.EOF)
            {
                throw new InvalidOperationException("cannot consume EOF");
            }
            if (Sync(p + 1))
            {
                p = AdjustSeekIndex(p + 1);
            }
        }
        protected internal virtual bool Sync(int i)
        {
            System.Diagnostics.Debug.Assert(i >= 0);
            int n = i - tokens.Count + 1;
            if (n > 0)
            {
                int fetched = Fetch(n);
                return fetched >= n;
            }
            return true;
        }
        protected internal virtual int Fetch(int n)
        {
            if (fetchedEOF)
            {
                return 0;
            }
            for (int i = 0; i < n; i++)
            {
                IToken t = _tokenSource.NextToken();
                if (t is IWritableToken token)
                {
                    token.TokenIndex = tokens.Count;
                }
                tokens.Add(t);
                if (t.Type == TokenConstants.EOF)
                {
                    fetchedEOF = true;
                    return i + 1;
                }
            }
            return n;
        }
        public virtual IToken Get(int i)
        {
            if (i < 0 || i >= tokens.Count)
            {
                throw new ArgumentOutOfRangeException("token index " + i + " out of range 0.." + (tokens.Count - 1));
            }
            return tokens[i];
        }

        public virtual int LA(int i)
        {
            return LT(i).Type;
        }
        protected internal virtual IToken Lb(int k)
        {
            if ((p - k) < 0)
            {
                return null;
            }
            return tokens[p - k];
        }
        public virtual IToken LT(int k)
        {
            LazyInit();
            if (k == 0)
            {
                return null;
            }
            if (k < 0)
            {
                return Lb(-k);
            }
            int i = p + k - 1;
            Sync(i);
            if (i >= tokens.Count)
            {
                return tokens[tokens.Count - 1];
            }
            return tokens[i];
        }
        protected internal virtual int AdjustSeekIndex(int i)
        {
            return i;
        }
        protected internal void LazyInit()
        {
            if (p == -1)
            {
                Setup();
            }
        }
        protected internal virtual void Setup()
        {
            Sync(0);
            p = AdjustSeekIndex(0);
        }
        public virtual void SetTokenSource(ITokenSource tokenSource)
        {
            this._tokenSource = tokenSource;
            tokens.Clear();
            p = -1;
			this.fetchedEOF = false;
        }
        protected internal virtual int NextTokenOnChannel(int i, int channel)
        {
            Sync(i);
            if (i >= Size)
            {
                return Size - 1;
            }
            IToken token = tokens[i];
            while (token.Channel != channel)
            {
                if (token.Type == TokenConstants.EOF)
                {
                    return i;
                }
                i++;
                Sync(i);
                token = tokens[i];
            }
            return i;
        }
        protected internal virtual int PreviousTokenOnChannel(int i, int channel)
        {
            Sync(i);
            if (i >= Size)
            {
                return Size - 1;
            }
            while (i >= 0)
            {
                IToken token = tokens[i];
                if (token.Type == TokenConstants.EOF || token.Channel == channel)
                {
                    return i;
                }
                i--;
            }
            return i;
        }
        public virtual string SourceName
        {
            get
            {
                return _tokenSource.SourceName;
            }
        }
        public virtual string GetText()
        {
            Fill();
            return GetText(Interval.Of(0, Size - 1));
        }
        public virtual string GetText(Interval interval)
        {
            int start = interval.a;
            int stop = interval.b;
            if (start < 0 || stop < 0)
            {
                return string.Empty;
            }
            LazyInit();
            if (stop >= tokens.Count)
            {
                stop = tokens.Count - 1;
            }
            StringBuilder buf = new StringBuilder();
            for (int i = start; i <= stop; i++)
            {
                IToken t = tokens[i];
                if (t.Type == TokenConstants.EOF)
                {
                    break;
                }
                buf.Append(t.Text);
            }
            return buf.ToString();
        }
        public virtual string GetText(RuleContext ctx)
        {
            return GetText(ctx.SourceInterval);
        }
        public virtual string GetText(IToken start, IToken stop)
        {
            if (start != null && stop != null)
            {
                return GetText(Interval.Of(start.TokenIndex, stop.TokenIndex));
            }
            return string.Empty;
        }
        public virtual void Fill()
        {
            LazyInit();
            int blockSize = 1000;
            while (true)
            {
                int fetched = Fetch(blockSize);
                if (fetched < blockSize)
                {
                    return;
                }
            }
        }
    }
}
