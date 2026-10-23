class StringReader {
    constructor(text) {
        this.text = text;
        this.position = 0;
        this.length = text.length;
    }

    read() {
        if (this.position >= this.length) {
            return -1;
        }
        return this.text.charCodeAt(this.position++);
    }
}

export { StringReader };