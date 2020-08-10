package toolgood.algorithm.internals;

import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.misc.Interval;

public class CaseChangingCharStream implements CharStream {
	final CharStream stream;

	public CaseChangingCharStream(CharStream stream) {
		this.stream = stream;
	}

	@Override
	public String getText(Interval interval) {
		return stream.getText(interval);
	}

	@Override
	public void consume() {
		stream.consume();
	}

	@Override
	public int LA(int i) {
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
        return Character.toUpperCase(0);
	}

	@Override
	public int mark() {
		return stream.mark();
	}

	@Override
	public void release(int marker) {
		stream.release(marker);
	}

	@Override
	public int index() {
		return stream.index();
	}

	@Override
	public void seek(int index) {
		stream.seek(index);
	}

	@Override
	public int size() {
		return stream.size();
	}

	@Override
	public String getSourceName() {
		return stream.getSourceName();
	}
}