package toolgood.algorithm.litJson;

public class JsonException extends Exception {
    /**
     *
     */
    private static final long serialVersionUID = 1L;

    public JsonException(ParserToken token, Exception inner_exception) {
        super();
        // base(String.Format("Invalid token '{0}' in input string", token),
        // inner_exception)
    }

    public JsonException(int c) {
        super();
        // base(String.Format("Invalid character '{0}' in input string", (char)c))
    }

    public JsonException(String message) {
        // : base(message)
    }
}