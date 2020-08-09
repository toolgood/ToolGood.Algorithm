package toolgood.algorithm.internals;

public class CaseChangingCharStream extends ICharStream {
        private ICharStream stream;

        /// <summary>
        /// Constructs a new CaseChangingCharStream wrapping the given <paramref name="stream"/> forcing
        /// all characters to upper case or lower case.
        /// </summary>
        /// <param name="stream">The stream to wrap.</param>
        /// <param name="upper">If true force each symbol to upper case, otherwise force to lower.</param>
        public CaseChangingCharStream(ICharStream stream)
        {
            this.stream = stream;
        }

        public int Index {
            get {
                return stream.Index;
            }
        }

        public int Size {
            get {
                return stream.Size;
            }
        }

        public string SourceName {
            get {
                return stream.SourceName;
            }
        }

        public void Consume()
        {
            stream.Consume();
        }

        [return: NotNull]
        public string GetText(Interval interval)
        {
            return stream.GetText(interval);
        }

        public int LA(int i)
        {
            int c = stream.LA(i);

            if (c <= 0) {
                return c;
            }

            char o = (char)c;
            if (o == '（') {
                o = '(';
            } else if (o == '）') {
                o = ')';
            } else if (o == '，') {
                o = ',';
            } else if (o == '【') {
                o = '[';
            } else if (o == '】') {
                o = ']';
            } else if (o == '‘') {
                o = '\'';
            } else if (o == '’') {
                o = '\'';
            } else if (o == '“') {
                o = '"';
            } else if (o == '”') {
                o = '"';
            }

            return (int)char.ToUpperInvariant(o);

        }

        public int Mark()
        {
            return stream.Mark();
        }

        public void Release(int marker)
        {
            stream.Release(marker);
        }

        public void Seek(int index)
        {
            stream.Seek(index);
        }
}