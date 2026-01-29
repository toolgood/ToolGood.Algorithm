/**
 * AntlrErrorTextWriter
 */
export class AntlrErrorTextWriter {
    /**
     * @param {Object} formatProvider
     */
    constructor(formatProvider = null) {
        this.IsError = false;
        this.ErrorMsg = null;
    }

    /**
     * Gets the encoding.
     * @returns {string}
     */
    get Encoding() {
        return 'UTF-8';
    }

    /**
     * Writes a line of text.
     * @param {string} value
     */
    WriteLine(value) {
        this.IsError = true;
        this.ErrorMsg = value;
    }
}
