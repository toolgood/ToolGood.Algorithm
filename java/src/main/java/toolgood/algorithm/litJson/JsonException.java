package toolgood.algorithm.litJson;

final class JsonException extends RuntimeException {
   public JsonException(ParserToken token, Exception inner_exception) {
        super("Invalid token '" + token + "' in input string", inner_exception);
    }

   public JsonException(int c) {
        super("Invalid character '" + (char) c + "' in input string");
    }
   public JsonException(String message, Exception inner_exception) {
        super(message);
    }
    public JsonException(String message) {
        super(message);
    }
}
