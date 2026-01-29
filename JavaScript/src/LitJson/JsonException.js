class JsonException extends Error {
    constructor(message) {
        super(message);
        this.name = 'JsonException';
    }

    static createWithToken(token, innerException) {
        const message = `Invalid token '${token}' in input string`;
        const exception = new JsonException(message);
        if (innerException) {
            exception.innerException = innerException;
        }
        return exception;
    }

    static createWithChar(c) {
        const message = `Invalid character '${String.fromCharCode(c)}' in input string`;
        return new JsonException(message);
    }
}

export { JsonException };