package toolgood.algorithm.litJson;

public class JsonException extends RuntimeException {
    public JsonException(String message) {
        super(message);
    }

    public JsonException(ParserToken token, Exception innerException) {
        super("Invalid token '" + token + "' in input string", innerException);
    }

    public JsonException(int c) {
        super("Invalid character '" + (char) c + "' in input string");
    }
}
