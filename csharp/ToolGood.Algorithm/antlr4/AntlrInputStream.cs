/* Copyright (c) 2012-2017 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
namespace Antlr4.Runtime
{
    internal abstract class BaseInputCharStream : ICharStream
    {
        public const int ReadBufferSize = 1024;
        public const int InitialBufferSize = 1024;
        protected internal int n;
        protected internal int p = 0;
        public string name;
        public virtual void Consume()
        {
            if (p >= n)
            {
                System.Diagnostics.Debug.Assert(LA(1) == IntStreamConstants.EOF);
                throw new InvalidOperationException("cannot consume EOF");
            }
            else
            {
                p++;
            }
        }
        public virtual int LA(int i)
        {
            if (i == 0)
            {
                return 0;
            }
            if (i < 0)
            {
                i++;
                if ((p + i - 1) < 0)
                {
                    return IntStreamConstants.EOF;
                }
            }
            if ((p + i - 1) >= n)
            {
                return IntStreamConstants.EOF;
            }
            return ValueAt(p + i - 1);
        }
        public virtual int Index
        {
            get
            {
                return p;
            }
        }
        public virtual int Size
        {
            get
            {
                return n;
            }
        }
        public virtual int Mark()
        {
            return -1;
        }
        public virtual void Release(int marker)
        {
        }
        public virtual void Seek(int index)
        {
            if (index <= p)
            {
                p = index;
                return;
            }
            index = Math.Min(index, n);
            while (p < index)
            {
                Consume();
            }
        }
        public virtual string GetText(Interval interval)
        {
            int start = interval.a;
            int stop = interval.b;
            if (stop >= n)
            {
                stop = n - 1;
            }
            int count = stop - start + 1;
            if (start >= n)
            {
                return string.Empty;
            }
            return ConvertDataToString(start, count);
        }
        protected abstract int ValueAt(int i);
        protected abstract string ConvertDataToString(int start, int count);
        public override sealed string ToString()
        {
            return ConvertDataToString(0, n);
        }
        public virtual string SourceName
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    return IntStreamConstants.UnknownSourceName;
                }
                return name;
            }
        }
    }
    internal class AntlrInputStream : BaseInputCharStream
    {
        protected internal char[] data;
        public AntlrInputStream()
        {
        }
        public AntlrInputStream(string input)
        {
            this.data = input.ToCharArray();
            this.n = input.Length;
        }
        protected override int ValueAt(int i)
        {
            return data[i];
        }
        protected override string ConvertDataToString(int start, int count)
        {
            return new string(data, start, count);
        }
    }
}
