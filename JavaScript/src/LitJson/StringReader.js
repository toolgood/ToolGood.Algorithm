class StringReader {
    constructor(text) {
        this.text = text;
        this.position = 0;
        this.length = text.length;
    }

    Read() {
        if (this.position >= this.length) {
            return -1;
        }
        return this.text.charCodeAt(this.position++);
    }
}

export { StringReader };