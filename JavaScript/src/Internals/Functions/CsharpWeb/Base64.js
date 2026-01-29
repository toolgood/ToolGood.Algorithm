// Modified Base64 for URL applications ('base64url' encoding)
//
// See http://tools.ietf.org/html/rfc4648
// For more information see http://en.wikipedia.org/wiki/Base64

let base64 = "===========================================+=+=/0123456789=======ABCDEFGHIJKLMNOPQRSTUVWXYZ====/=abcdefghijklmnopqrstuvwxyz=====";

class Base64 {
    static ToBase64String(input) {
        return btoa(String.fromCharCode(...input));
    }

    static FromBase64String(base64Str) {
        return Base64.FromBase64ForUrlString(base64Str);
    }

    // Modified Base64 for URL applications ('base64url' encoding)
    //
    // See http://tools.ietf.org/html/rfc4648
    // For more information see http://en.wikipedia.org/wiki/Base64
    //
    // @param {Uint8Array} input
    // @returns {string} Input byte array converted to a base64ForUrl encoded string
    static ToBase64ForUrlString(input) {
        let result = btoa(String.fromCharCode(...input)).replace(/=/g, '');
        result = result.replace(/\+/g, '-');
        result = result.replace(/\//g, '_');
        return result;
    }

    // Modified Base64 for URL applications ('base64url' encoding)
    //
    // See http://tools.ietf.org/html/rfc4648
    // For more information see http://en.wikipedia.org/wiki/Base64
    //
    // @param {string} base64ForUrlInput
    // @returns {Uint8Array} Input base64ForUrl encoded string as the original byte array
    static FromBase64ForUrlString(base64ForUrlInput) {
        let sb = [];
        for (let c of base64ForUrlInput) {
            if (c.charCodeAt(0) >= 128) continue;
            let k = base64[c.charCodeAt(0)];
            if (k === '=') continue;
            sb.push(k);
        }
        let len = sb.length;
        let padChars = len % 4 === 0 ? 0 : 4 - (len % 4);
        if (padChars > 0) {
            sb.push(...'='.repeat(padChars).split(''));
        }
        let base64Str = sb.join('');
        let binaryString = atob(base64Str);
        let bytes = new Uint8Array(binaryString.length);
        for (let i = 0; i < binaryString.length; i++) {
            bytes[i] = binaryString.charCodeAt(i);
        }
        return bytes;
    }
}

export { Base64 };
