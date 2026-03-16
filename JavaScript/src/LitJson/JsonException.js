class JsonException extends Error {
    constructor(message) {
        super(message);
        this.name = 'JsonException';
    }

    static createWithToken(token, innerException) {
        let message = `Invalid token '${token}' in input string`;
        let exception = new JsonException(message);
        if (innerException) {
            exception.innerException = innerException;
        }
        return exception;
    }

    static createWithChar(c) {
        let message = `Invalid character '${String.fromCharCode(c)}' in input string`;
        return new JsonException(message);
    }
}

export { JsonException };