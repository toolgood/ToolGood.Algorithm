/**
 * MD5
 */
export class MD5 {
    /**
     * @constructor
     */
    constructor() {
        // static state variables
        this.A = 0;
        this.B = 0;
        this.C = 0;
        this.D = 0;
        
        // number of bits to rotate in tranforming
        this.S11 = 7;
        this.S12 = 12;
        this.S13 = 17;
        this.S14 = 22;
        this.S21 = 5;
        this.S22 = 9;
        this.S23 = 14;
        this.S24 = 20;
        this.S31 = 4;
        this.S32 = 11;
        this.S33 = 16;
        this.S34 = 23;
        this.S41 = 6;
        this.S42 = 10;
        this.S43 = 15;
        this.S44 = 21;
    }
    
    /**
     * F(X,Y,Z) =(X&Y)|((~X)&Z)
     * @param {number} x
     * @param {number} y
     * @param {number} z
     * @returns {number}
     */
    F(x, y, z) {
        return (x & y) | ((~x) & z);
    }
    
    /**
     * G(X,Y,Z) =(X&Z)|(Y&(~Z))
     * @param {number} x
     * @param {number} y
     * @param {number} z
     * @returns {number}
     */
    G(x, y, z) {
        return (x & z) | (y & (~z));
    }
    
    /**
     * H(X,Y,Z) =X^Y^Z
     * @param {number} x
     * @param {number} y
     * @param {number} z
     * @returns {number}
     */
    H(x, y, z) {
        return x ^ y ^ z;
    }
    
    /**
     * I(X,Y,Z)=Y^(X|(~Z))
     * @param {number} x
     * @param {number} y
     * @param {number} z
     * @returns {number}
     */
    I(x, y, z) {
        return y ^ (x | (~z));
    }
    
    /**
     * FF transformation for round 1
     * @param {number} a
     * @param {number} b
     * @param {number} c
     * @param {number} d
     * @param {number} mj
     * @param {number} s
     * @param {number} ti
     * @returns {number}
     */
    FF(a, b, c, d, mj, s, ti) {
        a = a + this.F(b, c, d) + mj + ti;
        a = (a << s) | (a >>> (32 - s));
        a += b;
        return a;
    }
    
    /**
     * GG transformation for round 2
     * @param {number} a
     * @param {number} b
     * @param {number} c
     * @param {number} d
     * @param {number} mj
     * @param {number} s
     * @param {number} ti
     * @returns {number}
     */
    GG(a, b, c, d, mj, s, ti) {
        a = a + this.G(b, c, d) + mj + ti;
        a = (a << s) | (a >>> (32 - s));
        a += b;
        return a;
    }
    
    /**
     * HH transformation for round 3
     * @param {number} a
     * @param {number} b
     * @param {number} c
     * @param {number} d
     * @param {number} mj
     * @param {number} s
     * @param {number} ti
     * @returns {number}
     */
    HH(a, b, c, d, mj, s, ti) {
        a = a + this.H(b, c, d) + mj + ti;
        a = (a << s) | (a >>> (32 - s));
        a += b;
        return a;
    }
    
    /**
     * II transformation for round 4
     * @param {number} a
     * @param {number} b
     * @param {number} c
     * @param {number} d
     * @param {number} mj
     * @param {number} s
     * @param {number} ti
     * @returns {number}
     */
    II(a, b, c, d, mj, s, ti) {
        a = a + this.I(b, c, d) + mj + ti;
        a = (a << s) | (a >>> (32 - s));
        a += b;
        return a;
    }
    
    /**
     * MD5_Init
     */
    MD5_Init() {
        this.A = 0x67452301;  // in memory, this is 0x01234567
        this.B = 0xefcdab89;  // in memory, this is 0x89abcdef
        this.C = 0x98badcfe;  // in memory, this is 0xfedcba98
        this.D = 0x10325476;  // in memory, this is 0x76543210
    }
    
    /**
     * MD5_Append
     * @param {Uint8Array} input
     * @returns {number[]}
     */
    MD5_Append(input) {
        let zeros = 0;
        let ones = 1;
        let size = 0;
        let n = input.length;
        let m = n % 64;
        
        if (m < 56) {
            zeros = 55 - m;
            size = n - m + 64;
        } else if (m === 56) {
            zeros = 0;
            ones = 0;
            size = n + 8;
        } else {
            zeros = 63 - m + 56;
            size = n + 64 - m + 64;
        }
        
        let bs = [...input];
        if (ones === 1) {
            bs.push(0x80); // 0x80 = $10000000
        }
        for (let i = 0; i < zeros; i++) {
            bs.push(0);
        }
        
        let N = n * 8;
        bs.push(N & 0xFF);
        bs.push((N >> 8) & 0xFF);
        bs.push((N >> 16) & 0xFF);
        bs.push((N >> 24) & 0xFF);
        bs.push((N >> 32) & 0xFF);
        bs.push((N >> 40) & 0xFF);
        bs.push((N >> 48) & 0xFF);
        bs.push(N >> 56);
        
        // Decodes input (byte[]) into output (UInt32[]). Assumes len is a multiple of 4.
        let output = [];
        for (let i = 0, j = 0; i < size; j++, i += 4) {
            output[j] = bs[i] | (bs[i + 1] << 8) | (bs[i + 2] << 16) | (bs[i + 3] << 24);
        }
        return output;
    }
    
    /**
     * MD5_Trasform
     * @param {number[]} x
     * @returns {number[]}
     */
    MD5_Trasform(x) {
        let a, b, c, d;
        
        for (let k = 0; k < x.length; k += 16) {
            a = this.A;
            b = this.B;
            c = this.C;
            d = this.D;
            
            // Round 1
            a = this.FF(a, b, c, d, x[k + 0], this.S11, 0xd76aa478); /* 1 */
            d = this.FF(d, a, b, c, x[k + 1], this.S12, 0xe8c7b756); /* 2 */
            c = this.FF(c, d, a, b, x[k + 2], this.S13, 0x242070db); /* 3 */
            b = this.FF(b, c, d, a, x[k + 3], this.S14, 0xc1bdceee); /* 4 */
            a = this.FF(a, b, c, d, x[k + 4], this.S11, 0xf57c0faf); /* 5 */
            d = this.FF(d, a, b, c, x[k + 5], this.S12, 0x4787c62a); /* 6 */
            c = this.FF(c, d, a, b, x[k + 6], this.S13, 0xa8304613); /* 7 */
            b = this.FF(b, c, d, a, x[k + 7], this.S14, 0xfd469501); /* 8 */
            a = this.FF(a, b, c, d, x[k + 8], this.S11, 0x698098d8); /* 9 */
            d = this.FF(d, a, b, c, x[k + 9], this.S12, 0x8b44f7af); /* 10 */
            c = this.FF(c, d, a, b, x[k + 10], this.S13, 0xffff5bb1); /* 11 */
            b = this.FF(b, c, d, a, x[k + 11], this.S14, 0x895cd7be); /* 12 */
            a = this.FF(a, b, c, d, x[k + 12], this.S11, 0x6b901122); /* 13 */
            d = this.FF(d, a, b, c, x[k + 13], this.S12, 0xfd987193); /* 14 */
            c = this.FF(c, d, a, b, x[k + 14], this.S13, 0xa679438e); /* 15 */
            b = this.FF(b, c, d, a, x[k + 15], this.S14, 0x49b40821); /* 16 */
            
            // Round 2
            a = this.GG(a, b, c, d, x[k + 1], this.S21, 0xf61e2562); /* 17 */
            d = this.GG(d, a, b, c, x[k + 6], this.S22, 0xc040b340); /* 18 */
            c = this.GG(c, d, a, b, x[k + 11], this.S23, 0x265e5a51); /* 19 */
            b = this.GG(b, c, d, a, x[k + 0], this.S24, 0xe9b6c7aa); /* 20 */
            a = this.GG(a, b, c, d, x[k + 5], this.S21, 0xd62f105d); /* 21 */
            d = this.GG(d, a, b, c, x[k + 10], this.S22, 0x2441453); /* 22 */
            c = this.GG(c, d, a, b, x[k + 15], this.S23, 0xd8a1e681); /* 23 */
            b = this.GG(b, c, d, a, x[k + 4], this.S24, 0xe7d3fbc8); /* 24 */
            a = this.GG(a, b, c, d, x[k + 9], this.S21, 0x21e1cde6); /* 25 */
            d = this.GG(d, a, b, c, x[k + 14], this.S22, 0xc33707d6); /* 26 */
            c = this.GG(c, d, a, b, x[k + 3], this.S23, 0xf4d50d87); /* 27 */
            b = this.GG(b, c, d, a, x[k + 8], this.S24, 0x455a14ed); /* 28 */
            a = this.GG(a, b, c, d, x[k + 13], this.S21, 0xa9e3e905); /* 29 */
            d = this.GG(d, a, b, c, x[k + 2], this.S22, 0xfcefa3f8); /* 30 */
            c = this.GG(c, d, a, b, x[k + 7], this.S23, 0x676f02d9); /* 31 */
            b = this.GG(b, c, d, a, x[k + 12], this.S24, 0x8d2a4c8a); /* 32 */
            
            // Round 3
            a = this.HH(a, b, c, d, x[k + 5], this.S31, 0xfffa3942); /* 33 */
            d = this.HH(d, a, b, c, x[k + 8], this.S32, 0x8771f681); /* 34 */
            c = this.HH(c, d, a, b, x[k + 11], this.S33, 0x6d9d6122); /* 35 */
            b = this.HH(b, c, d, a, x[k + 14], this.S34, 0xfde5380c); /* 36 */
            a = this.HH(a, b, c, d, x[k + 1], this.S31, 0xa4beea44); /* 37 */
            d = this.HH(d, a, b, c, x[k + 4], this.S32, 0x4bdecfa9); /* 38 */
            c = this.HH(c, d, a, b, x[k + 7], this.S33, 0xf6bb4b60); /* 39 */
            b = this.HH(b, c, d, a, x[k + 10], this.S34, 0xbebfbc70); /* 40 */
            a = this.HH(a, b, c, d, x[k + 13], this.S31, 0x289b7ec6); /* 41 */
            d = this.HH(d, a, b, c, x[k + 0], this.S32, 0xeaa127fa); /* 42 */
            c = this.HH(c, d, a, b, x[k + 3], this.S33, 0xd4ef3085); /* 43 */
            b = this.HH(b, c, d, a, x[k + 6], this.S34, 0x4881d05); /* 44 */
            a = this.HH(a, b, c, d, x[k + 9], this.S31, 0xd9d4d039); /* 45 */
            d = this.HH(d, a, b, c, x[k + 12], this.S32, 0xe6db99e5); /* 46 */
            c = this.HH(c, d, a, b, x[k + 15], this.S33, 0x1fa27cf8); /* 47 */
            b = this.HH(b, c, d, a, x[k + 2], this.S34, 0xc4ac5665); /* 48 */
            
            // Round 4
            a = this.II(a, b, c, d, x[k + 0], this.S41, 0xf4292244); /* 49 */
            d = this.II(d, a, b, c, x[k + 7], this.S42, 0x432aff97); /* 50 */
            c = this.II(c, d, a, b, x[k + 14], this.S43, 0xab9423a7); /* 51 */
            b = this.II(b, c, d, a, x[k + 5], this.S44, 0xfc93a039); /* 52 */
            a = this.II(a, b, c, d, x[k + 12], this.S41, 0x655b59c3); /* 53 */
            d = this.II(d, a, b, c, x[k + 3], this.S42, 0x8f0ccc92); /* 54 */
            c = this.II(c, d, a, b, x[k + 10], this.S43, 0xffeff47d); /* 55 */
            b = this.II(b, c, d, a, x[k + 1], this.S44, 0x85845dd1); /* 56 */
            a = this.II(a, b, c, d, x[k + 8], this.S41, 0x6fa87e4f); /* 57 */
            d = this.II(d, a, b, c, x[k + 15], this.S42, 0xfe2ce6e0); /* 58 */
            c = this.II(c, d, a, b, x[k + 6], this.S43, 0xa3014314); /* 59 */
            b = this.II(b, c, d, a, x[k + 13], this.S44, 0x4e0811a1); /* 60 */
            a = this.II(a, b, c, d, x[k + 4], this.S41, 0xf7537e82); /* 61 */
            d = this.II(d, a, b, c, x[k + 11], this.S42, 0xbd3af235); /* 62 */
            c = this.II(c, d, a, b, x[k + 2], this.S43, 0x2ad7d2bb); /* 63 */
            b = this.II(b, c, d, a, x[k + 9], this.S44, 0xeb86d391); /* 64 */
            
            this.A += a;
            this.B += b;
            this.C += c;
            this.D += d;
        }
        return [this.A, this.B, this.C, this.D];
    }
    
    /**
     * MD5Array
     * @param {Uint8Array} input
     * @returns {Uint8Array}
     */
    MD5Array(input) {
        this.MD5_Init();
        let block = this.MD5_Append(input);
        let bits = this.MD5_Trasform(block);
        
        // Encodes bits (UInt32[]) into output (byte[]). Assumes len is a multiple of 4.
        let output = new Uint8Array(bits.length * 4);
        for (let i = 0, j = 0; i < bits.length; i++, j += 4) {
            output[j] = bits[i] & 0xff;
            output[j + 1] = (bits[i] >> 8) & 0xff;
            output[j + 2] = (bits[i] >> 16) & 0xff;
            output[j + 3] = (bits[i] >> 24) & 0xff;
        }
        return output;
    }
    
    /**
     * ArrayToHexString
     * @param {Uint8Array} array
     * @param {boolean} uppercase
     * @returns {string}
     */
    static ArrayToHexString(array, uppercase) {
        let hexString = '';
        let format = uppercase ? 'X2' : 'x2';
        for (let b of array) {
            hexString += b.toString(16).padStart(2, '0')[format === 'X2' ? 'toUpperCase' : 'toLowerCase']();
        }
        return hexString;
    }
    
    /**
     * MDString
     * @param {Uint8Array} c
     * @returns {string}
     */
    static MDString(c) {
        let md5 = new MD5();
        let digest = md5.MD5Array(c);
        return MD5.ArrayToHexString(digest, true);
    }
    
    /**
     * ComputeHash
     * @param {Uint8Array} c
     * @returns {Uint8Array}
     */
    static ComputeHash(c) {
        let md5 = new MD5();
        return md5.MD5Array(c);
    }
    
    /**
     * hmac_md5
     * @param {Uint8Array} b_tmp1
     * @param {Uint8Array} source
     * @returns {string}
     */
    static hmac_md5(b_tmp1, source) {
        let b_tmp;
        
        let digest = new Uint8Array(512);
        let k_ipad = new Uint8Array(64);
        let k_opad = new Uint8Array(64);
        
        for (let i = 0; i < 64; i++) {
            k_ipad[i] = 0 ^ 0x36;
            k_opad[i] = 0 ^ 0x5c;
        }
        
        if (source.length > 64) {
            source = MD5.ComputeHash(source);
        }
        
        for (let i = 0; i < source.length; i++) {
            k_ipad[i] = source[i] ^ 0x36;
            k_opad[i] = source[i] ^ 0x5c;
        }
        
        b_tmp = MD5.adding(k_ipad, b_tmp1);
        digest.set(MD5.ComputeHash(b_tmp));
        b_tmp = MD5.adding(k_opad, digest);
        digest.set(MD5.ComputeHash(b_tmp));
        
        return MD5.ArrayToHexString(digest.slice(0, 16), true);
    }
    
    /**
     * adding
     * @param {Uint8Array} a
     * @param {Uint8Array} b
     * @returns {Uint8Array}
     */
    static adding(a, b) {
        let c = new Uint8Array(a.length + b.length);
        c.set(a, 0);
        c.set(b, a.length);
        return c;
    }
}
