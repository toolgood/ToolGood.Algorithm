import * as iconv from 'iconv-lite';

/**
 * 浏览器兼容的哈希和HMAC工具类
 */
export class CryptoUtils {
    /**
     * 将字符串转换为ArrayBuffer
     * @param {string} str - 要转换的字符串
     * @param {string} encoding - 编码格式
     * @returns {ArrayBuffer}
     */
    static stringToArrayBuffer(str, encoding) {
        let buffer;
        if (iconv.encodingExists(encoding)) {
            buffer = iconv.encode(str, encoding);
        } else {
            // 如果编码不支持，默认使用utf-8
            buffer = new TextEncoder().encode(str);
        }
        return buffer.buffer;
    }

    /**
     * 将ArrayBuffer转换为十六进制字符串
     * @param {ArrayBuffer} buffer - 要转换的ArrayBuffer
     * @returns {string}
     */
    static arrayBufferToHex(buffer) {
        const bytes = new Uint8Array(buffer);
        let hex = '';
        for (let i = 0; i < bytes.length; i++) {
            hex += bytes[i].toString(16).padStart(2, '0');
        }
        return hex.toUpperCase();
    }

    /**
     * 计算哈希值
     * @param {string} algorithm - 算法名称
     * @param {string} data - 要哈希的数据
     * @param {string} encoding - 编码格式
     * @returns {string} - 十六进制哈希值
     */
    static hash(algorithm, data, encoding) {
        let buffer;
        if (iconv.encodingExists(encoding)) {
            buffer = iconv.encode(data, encoding);
        } else {
            // 如果编码不支持，默认使用utf-8
            buffer = new TextEncoder().encode(data);
        }
        
        switch (algorithm) {
            case 'MD5':
                return this.md5ArrayBuffer(buffer.buffer);
            case 'SHA1':
                return this.sha1ArrayBuffer(buffer.buffer);
            case 'SHA256':
                return this.sha256ArrayBuffer(buffer.buffer);
            case 'SHA512':
                return this.sha512ArrayBuffer(buffer.buffer);
            default:
                throw new Error(`Unsupported algorithm: ${algorithm}`);
        }
    }

    /**
     * 计算HMAC值
     * @param {string} algorithm - 算法名称
     * @param {string} data - 要哈希的数据
     * @param {string} key - 密钥
     * @param {string} encoding - 编码格式
     * @returns {string} - 十六进制HMAC值
     */
    static hmac(algorithm, data, key, encoding) {
        const dataBuffer = iconv.encodingExists(encoding) ? iconv.encode(data, encoding) : new TextEncoder().encode(data);
        const keyBuffer = new TextEncoder().encode(key);
        
        switch (algorithm) {
            case 'MD5':
                return this.hmacMd5ArrayBuffer(dataBuffer.buffer, keyBuffer.buffer);
            case 'SHA1':
                return this.hmacSha1ArrayBuffer(dataBuffer.buffer, keyBuffer.buffer);
            case 'SHA256':
                return this.hmacSha256ArrayBuffer(dataBuffer.buffer, keyBuffer.buffer);
            case 'SHA512':
                return this.hmacSha512ArrayBuffer(dataBuffer.buffer, keyBuffer.buffer);
            default:
                throw new Error(`Unsupported algorithm: ${algorithm}`);
        }
    }

    /**
     * MD5算法实现
     * @param {string} data - 要哈希的数据
     * @param {string} encoding - 编码格式
     * @returns {string} - 十六进制MD5值
     */
    static md5(data, encoding) {
        const buffer = iconv.encodingExists(encoding) ? iconv.encode(data, encoding) : new TextEncoder().encode(data);
        return this.md5ArrayBuffer(buffer.buffer);
    }

    /**
     * HMAC-MD5算法实现
     * @param {string} data - 要哈希的数据
     * @param {string} key - 密钥
     * @param {string} encoding - 编码格式
     * @returns {string} - 十六进制HMAC-MD5值
     */
    static hmacMd5(data, key, encoding) {
        const dataBuffer = iconv.encodingExists(encoding) ? iconv.encode(data, encoding) : new TextEncoder().encode(data);
        const keyBuffer = new TextEncoder().encode(key);
        return this.hmacMd5ArrayBuffer(dataBuffer.buffer, keyBuffer.buffer);
    }

    /**
     * MD5算法核心实现
     * @param {ArrayBuffer} buffer - 要哈希的数据
     * @returns {string} - 十六进制MD5值
     */
    static md5ArrayBuffer(buffer) {
        const data = new Uint8Array(buffer);
        const len = data.length;
        
        // 初始化MD5状态
        let a = 0x67452301;
        let b = 0xefcdab89;
        let c = 0x98badcfe;
        let d = 0x10325476;
        
        // 常量K
        const K = [
            0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
            0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
            0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
            0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
            0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
            0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
            0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
            0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
            0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
            0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
            0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
            0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
            0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
            0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
            0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
            0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
        ];
        
        // 位移量
        const S = [
            7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
            5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20, 5, 9, 14, 20,
            4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
            6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
        ];
        
        // 填充数据
        const bitLen = len * 8;
        const padLen = (56 - (len + 1) % 64 + 64) % 64;
        const totalLen = len + 1 + padLen + 8;
        const paddedData = new Uint8Array(totalLen);
        paddedData.set(data, 0);
        paddedData[len] = 0x80;
        
        // 添加长度
        for (let i = 0; i < 8; i++) {
            paddedData[totalLen - 8 + i] = (bitLen >>> (i * 8)) & 0xFF;
        }
        
        // 处理数据块
        for (let i = 0; i < totalLen; i += 64) {
            const block = paddedData.subarray(i, i + 64);
            const M = new Array(16);
            
            // 将数据块转换为32位整数
            for (let j = 0; j < 16; j++) {
                M[j] = (
                    (block[j * 4] & 0xFF) |
                    ((block[j * 4 + 1] & 0xFF) << 8) |
                    ((block[j * 4 + 2] & 0xFF) << 16) |
                    ((block[j * 4 + 3] & 0xFF) << 24)
                );
            }
            
            // 保存当前状态
            let AA = a;
            let BB = b;
            let CC = c;
            let DD = d;
            
            // 主循环
            for (let j = 0; j < 64; j++) {
                let F, g;
                
                if (j < 16) {
                    F = (b & c) | (~b & d);
                    g = j;
                } else if (j < 32) {
                    F = (d & b) | (~d & c);
                    g = (5 * j + 1) % 16;
                } else if (j < 48) {
                    F = b ^ c ^ d;
                    g = (3 * j + 5) % 16;
                } else {
                    F = c ^ (b | ~d);
                    g = (7 * j) % 16;
                }
                
                const temp = d;
                d = c;
                c = b;
                b = this.add32(b, this.rotateLeft(this.add32(this.add32(this.add32(a, F), K[j]), M[g]), S[j]));
                a = temp;
            }
            
            // 更新状态
            a = this.add32(a, AA);
            b = this.add32(b, BB);
            c = this.add32(c, CC);
            d = this.add32(d, DD);
        }
        
        // 将结果转换为十六进制字符串
        return this.toHex(a) + this.toHex(b) + this.toHex(c) + this.toHex(d);
    }
    
    /**
     * 32位整数加法
     * @param {number} x - 第一个数
     * @param {number} y - 第二个数
     * @returns {number} - 结果
     */
    static add32(x, y) {
        return (x + y) >>> 0;
    }
    
    /**
     * 将32位整数转换为8位十六进制字符串
     * @param {number} n - 32位整数
     * @returns {string} - 十六进制字符串
     */
    static toHex(n) {
        let hex = '';
        for (let i = 0; i < 4; i++) {
            hex += ((n >>> (i * 8)) & 0xFF).toString(16).padStart(2, '0');
        }
        return hex;
    }

    /**
     * HMAC-MD5算法核心实现
     * @param {ArrayBuffer} data - 要哈希的数据
     * @param {ArrayBuffer} key - 密钥
     * @returns {string} - 十六进制HMAC-MD5值
     */
    static hmacMd5ArrayBuffer(data, key) {
        const blockSize = 64; // MD5的块大小
        let keyData = new Uint8Array(key);
        
        // 处理密钥
        if (keyData.length > blockSize) {
            // 如果密钥长度大于块大小，对密钥进行MD5哈希
            keyData = new Uint8Array(this.md5ArrayBufferToBuffer(this.md5ArrayBuffer(key)));
        }
        
        // 填充密钥到块大小
        const paddedKey = new Uint8Array(blockSize);
        paddedKey.set(keyData, 0);
        
        // 创建内部和外部填充
        const innerPad = new Uint8Array(blockSize);
        const outerPad = new Uint8Array(blockSize);
        for (let i = 0; i < blockSize; i++) {
            innerPad[i] = paddedKey[i] ^ 0x36;
            outerPad[i] = paddedKey[i] ^ 0x5C;
        }
        
        // 计算内部哈希
        const innerData = new Uint8Array(innerPad.length + data.byteLength);
        innerData.set(innerPad, 0);
        innerData.set(new Uint8Array(data), innerPad.length);
        const innerHash = this.md5ArrayBuffer(innerData.buffer);
        
        // 计算外部哈希
        const outerData = new Uint8Array(outerPad.length + 16); // 16是MD5哈希值的长度
        outerData.set(outerPad, 0);
        outerData.set(this.hexToUint8Array(innerHash), outerPad.length);
        const outerHash = this.md5ArrayBuffer(outerData.buffer);
        
        return outerHash;
    }

    /**
     * 左旋转函数
     * @param {number} value - 要旋转的值
     * @param {number} amount - 旋转的位数
     * @returns {number}
     */
    static rotateLeft(value, amount) {
        return ((value << amount) | (value >>> (32 - amount))) >>> 0;
    }

    /**
     * 将十六进制字符串转换为Uint8Array
     * @param {string} hex - 十六进制字符串
     * @returns {Uint8Array}
     */
    static hexToUint8Array(hex) {
        const result = new Uint8Array(hex.length / 2);
        for (let i = 0; i < hex.length; i += 2) {
            result[i / 2] = parseInt(hex.substr(i, 2), 16);
        }
        return result;
    }

    /**
     * 将MD5哈希结果转换为ArrayBuffer
     * @param {string} md5Hash - MD5哈希结果
     * @returns {ArrayBuffer}
     */
    static md5ArrayBufferToBuffer(md5Hash) {
        return this.hexToUint8Array(md5Hash).buffer;
    }

    /**
     * SHA1算法核心实现
     * @param {ArrayBuffer} buffer - 要哈希的数据
     * @returns {string} - 十六进制SHA1值
     */
    static sha1ArrayBuffer(buffer) {
        const data = new Uint8Array(buffer);
        const len = data.length;
        
        // 初始化哈希值
        let h0 = 0x67452301;
        let h1 = 0xEFCDAB89;
        let h2 = 0x98BADCFE;
        let h3 = 0x10325476;
        let h4 = 0xC3D2E1F0;
        
        // 填充
        const bitLen = len * 8;
        const padLen = (448 - (bitLen + 1)) % 512;
        const totalLen = len + 1 + (padLen + 7) / 8 + 8;
        const paddedData = new Uint8Array(totalLen);
        paddedData.set(data, 0);
        paddedData[len] = 0x80;
        
        // 添加长度
        for (let i = 0; i < 8; i++) {
            paddedData[totalLen - 8 + i] = (bitLen >>> (i * 8)) & 0xFF;
        }
        
        // 处理数据块
        for (let i = 0; i < totalLen; i += 64) {
            const block = paddedData.subarray(i, i + 64);
            const words = new Uint32Array(80);
            
            // 前16个词直接从数据块中获取
            for (let j = 0; j < 16; j++) {
                words[j] = (
                    (block[j * 4] & 0xFF) |
                    ((block[j * 4 + 1] & 0xFF) << 8) |
                    ((block[j * 4 + 2] & 0xFF) << 16) |
                    ((block[j * 4 + 3] & 0xFF) << 24)
                );
            }
            
            // 剩余的词通过循环左移计算
            for (let j = 16; j < 80; j++) {
                words[j] = this.rotateLeft(words[j - 3] ^ words[j - 8] ^ words[j - 14] ^ words[j - 16], 1);
            }
            
            // 初始化工作变量
            let a = h0;
            let b = h1;
            let c = h2;
            let d = h3;
            let e = h4;
            
            // 主循环
            for (let j = 0; j < 80; j++) {
                let f, k;
                if (j < 20) {
                    f = (b & c) | (~b & d);
                    k = 0x5A827999;
                } else if (j < 40) {
                    f = b ^ c ^ d;
                    k = 0x6ED9EBA1;
                } else if (j < 60) {
                    f = (b & c) | (b & d) | (c & d);
                    k = 0x8F1BBCDC;
                } else {
                    f = b ^ c ^ d;
                    k = 0xCA62C1D6;
                }
                
                const temp = (this.rotateLeft(a, 5) + f + e + k + words[j]) >>> 0;
                e = d;
                d = c;
                c = this.rotateLeft(b, 30);
                b = a;
                a = temp;
            }
            
            // 更新哈希值
            h0 = (h0 + a) >>> 0;
            h1 = (h1 + b) >>> 0;
            h2 = (h2 + c) >>> 0;
            h3 = (h3 + d) >>> 0;
            h4 = (h4 + e) >>> 0;
        }
        
        // 转换为十六进制字符串
        let hex = '';
        for (let h of [h0, h1, h2, h3, h4]) {
            hex += h.toString(16).padStart(8, '0');
        }
        
        return hex.toUpperCase();
    }

    /**
     * SHA256算法核心实现
     * @param {ArrayBuffer} buffer - 要哈希的数据
     * @returns {string} - 十六进制SHA256值
     */
    static sha256ArrayBuffer(buffer) {
        const data = new Uint8Array(buffer);
        const len = data.length;
        
        // 初始化哈希值
        const h = new Uint32Array([
            0x6A09E667, 0xBB67AE85, 0x3C6EF372, 0xA54FF53A,
            0x510E527F, 0x9B05688C, 0x1F83D9AB, 0x5BE0CD19
        ]);
        
        // 常量K
        const K = new Uint32Array([
            0x428A2F98, 0x71374491, 0xB5C0FBCF, 0xE9B5DBA5,
            0x3956C25B, 0x59F111F1, 0x923F82A4, 0xAB1C5ED5,
            0xD807AA98, 0x12835B01, 0x243185BE, 0x550C7DC3,
            0x72BE5D74, 0x80DEB1FE, 0x9BDC06A7, 0xC19BF174,
            0xE49B69C1, 0xEFBE4786, 0x0FC19DC6, 0x240CA1CC,
            0x2DE92C6F, 0x4A7484AA, 0x5CB0A9DC, 0x76F988DA,
            0x983E5152, 0xA831C66D, 0xB00327C8, 0xBF597FC7,
            0xC6E00BF3, 0xD5A79147, 0x06CA6351, 0x14292967,
            0x27B70A85, 0x2E1B2138, 0x4D2C6DFC, 0x53380D13,
            0x650A7354, 0x766A0ABB, 0x81C2C92E, 0x92722C85,
            0xA2BFE8A1, 0xA81A664B, 0xC24B8B70, 0xC76C51A3,
            0xD192E819, 0xD6990624, 0xF40E3585, 0x106AA070,
            0x19A4C116, 0x1E376C08, 0x2748774C, 0x34B0BCB5,
            0x391C0CB3, 0x4ED8AA4A, 0x5B9CCA4F, 0x682E6FF3,
            0x748F82EE, 0x78A5636F, 0x84C87814, 0x8CC70208,
            0x90BEFFFA, 0xA4506CEB, 0xBEF9A3F7, 0xC67178F2
        ]);
        
        // 填充
        const bitLen = len * 8;
        const padLen = (448 - (bitLen + 1)) % 512;
        const totalLen = len + 1 + (padLen + 7) / 8 + 8;
        const paddedData = new Uint8Array(totalLen);
        paddedData.set(data, 0);
        paddedData[len] = 0x80;
        
        // 添加长度
        for (let i = 0; i < 8; i++) {
            paddedData[totalLen - 8 + i] = (bitLen >>> (i * 8)) & 0xFF;
        }
        
        // 处理数据块
        for (let i = 0; i < totalLen; i += 64) {
            const block = paddedData.subarray(i, i + 64);
            const w = new Uint32Array(64);
            
            // 前16个词直接从数据块中获取
            for (let j = 0; j < 16; j++) {
                w[j] = (
                    (block[j * 4] & 0xFF) |
                    ((block[j * 4 + 1] & 0xFF) << 8) |
                    ((block[j * 4 + 2] & 0xFF) << 16) |
                    ((block[j * 4 + 3] & 0xFF) << 24)
                );
            }
            
            // 剩余的词通过计算得到
            for (let j = 16; j < 64; j++) {
                const s0 = (this.rotateRight(w[j - 15], 7) ^ this.rotateRight(w[j - 15], 18) ^ (w[j - 15] >>> 3));
                const s1 = (this.rotateRight(w[j - 2], 17) ^ this.rotateRight(w[j - 2], 19) ^ (w[j - 2] >>> 10));
                w[j] = (w[j - 16] + s0 + w[j - 7] + s1) >>> 0;
            }
            
            // 初始化工作变量
            let a = h[0], b = h[1], c = h[2], d = h[3];
            let e = h[4], f = h[5], g = h[6], hh = h[7];
            
            // 主循环
            for (let j = 0; j < 64; j++) {
                const S1 = (this.rotateRight(e, 6) ^ this.rotateRight(e, 11) ^ this.rotateRight(e, 25));
                const ch = ((e & f) ^ (~e & g));
                const temp1 = (hh + S1 + ch + K[j] + w[j]) >>> 0;
                const S0 = (this.rotateRight(a, 2) ^ this.rotateRight(a, 13) ^ this.rotateRight(a, 22));
                const maj = ((a & b) ^ (a & c) ^ (b & c));
                const temp2 = (S0 + maj) >>> 0;
                
                hh = g;
                g = f;
                f = e;
                e = (d + temp1) >>> 0;
                d = c;
                c = b;
                b = a;
                a = (temp1 + temp2) >>> 0;
            }
            
            // 更新哈希值
            h[0] = (h[0] + a) >>> 0;
            h[1] = (h[1] + b) >>> 0;
            h[2] = (h[2] + c) >>> 0;
            h[3] = (h[3] + d) >>> 0;
            h[4] = (h[4] + e) >>> 0;
            h[5] = (h[5] + f) >>> 0;
            h[6] = (h[6] + g) >>> 0;
            h[7] = (h[7] + hh) >>> 0;
        }
        
        // 转换为十六进制字符串
        let hex = '';
        for (let x of h) {
            hex += x.toString(16).padStart(8, '0');
        }
        
        return hex.toUpperCase();
    }

    /**
     * SHA512算法核心实现
     * @param {ArrayBuffer} buffer - 要哈希的数据
     * @returns {string} - 十六进制SHA512值
     */
    static sha512ArrayBuffer(buffer) {
        const data = new Uint8Array(buffer);
        const len = data.length;
        
        // 初始化哈希值
        const h = new BigUint64Array([
            0x6a09e667f3bcc908n, 0xbb67ae8584caa73bn, 0x3c6ef372fe94f82bn, 0xa54ff53a5f1d36f1n,
            0x510e527fade682d1n, 0x9b05688c2b3e6c1fn, 0x1f83d9abfb41bd6bn, 0x5be0cd19137e2179n
        ]);
        
        // 常量K
        const K = new BigUint64Array([
            0x428a2f98d728ae22n, 0x7137449123ef65cdn, 0xb5c0fbcfec4d3b2fn, 0xe9b5dba58189dbbc4n,
            0x3956c25bf348b538n, 0x59f111f1b605d019n, 0x923f82a4af194f9bn, 0xab1c5ed5da6d8118n,
            0xd807aa98a3030242n, 0x12835b0145706fb4n, 0x243185be4ee4b28cn, 0x550c7dc3d5ffb4e2n,
            0x72be5d74f27b896fn, 0x80deb1fe3b1696b1n, 0x9bdc06a725c71235n, 0xc19bf174cf692694n,
            0xe49b69c19ef14ad2n, 0xefbe4786384f25e3n, 0x0fc19dc68b8cd5b5n, 0x240ca1cc77ac9c65n,
            0x2de92c6f592b0275n, 0x4a7484aa6ea6e483n, 0x5cb0a9dcbd41fbd4n, 0x76f988da831153b5n,
            0x983e5152ee66dfabn, 0xa831c66d2db43210n, 0xb00327c898fb213fn, 0xbf597fc7beef0ee4n,
            0xc6e00bf33da88fc2n, 0xd5a79147930aa725n, 0x06ca6351e003826fn, 0x142929670a0e6e70n,
            0x27b70a8546d22ffcn, 0x2e1b21385c26c926n, 0x4d2c6dfc5ac42aedn, 0x53380d139d95b3dfn,
            0x650a73548baf63den, 0x766a0abb3c77b2a8n, 0x81c2c92e47edaee6n, 0x92722c851482353bn,
            0xa2bfe8a14cf10364n, 0xa81a664bbc423001n, 0xc24b8b70d0f89791n, 0xc76c51a30654be30n,
            0xd192e819d6ef5218n, 0xd69906245565a910n, 0xf40e35855771202an, 0x106aa07032bbd1b8n,
            0x19a4c116b8d2d0c8n, 0x1e376c085141ab53n, 0x2748774cdf8eeb99n, 0x34b0bcb5e19b48a8n,
            0x391c0cb3c5c95a63n, 0x4ed8aa4ae3418acbn, 0x5b9cca4f7763e373n, 0x682e6ff3d6b2b8a3n,
            0x748f82ee5defb2fcn, 0x78a5636f43172f60n, 0x84c87814a1f0ab72n, 0x8cc702081a6439ecn,
            0x90befffaa1f956e4n, 0xa4506cebde82bde9n, 0xbef9a3f7b2c67915n, 0xc67178f2e372532bn,
            0xca273eceea26619cn, 0xd186b8c721c0c207n, 0xeada7dd6cde0ebefn, 0xf57d4f7fee6ed178n,
            0x06f067aa72176fbcn, 0x0a637dc5a2c898a6n, 0x113f9804bef90daen, 0x1b710b35131c471bn,
            0x28db77f523047d84n, 0x32caab7b40c72493n, 0x3c9ebe0a15c9bebcn, 0x431d67c49c100d4cn,
            0x4cc5d4becb3e42b6n, 0x597f299cfc657e2an, 0x5fcb6fab3ad6faecn, 0x6c44198c4a475817n
        ]);
        
        // 填充
        const bitLen = BigInt(len) * 8n;
        const padLen = (448n - (bitLen + 1n)) % 512n;
        const totalLen = len + 1 + Number((padLen + 7n) / 8n) + 16;
        const paddedData = new Uint8Array(totalLen);
        paddedData.set(data, 0);
        paddedData[len] = 0x80;
        
        // 添加长度
        for (let i = 0; i < 16; i++) {
            paddedData[totalLen - 16 + i] = Number((bitLen >> BigInt(i * 8)) & 0xFFn);
        }
        
        // 处理数据块
        for (let i = 0; i < totalLen; i += 128) {
            const block = paddedData.subarray(i, i + 128);
            const w = new BigUint64Array(80);
            
            // 前16个词直接从数据块中获取
            for (let j = 0; j < 16; j++) {
                let val = 0n;
                for (let k = 0; k < 8; k++) {
                    val |= BigInt(block[j * 8 + k]) << BigInt(k * 8);
                }
                w[j] = val;
            }
            
            // 剩余的词通过计算得到
            for (let j = 16; j < 80; j++) {
                const s0 = (this.rotateRightBig(w[j - 15], 1n) ^ this.rotateRightBig(w[j - 15], 8n) ^ (w[j - 15] >> 7n));
                const s1 = (this.rotateRightBig(w[j - 2], 19n) ^ this.rotateRightBig(w[j - 2], 61n) ^ (w[j - 2] >> 6n));
                w[j] = (w[j - 16] + s0 + w[j - 7] + s1) & 0xffffffffffffffffn;
            }
            
            // 初始化工作变量
            let a = h[0], b = h[1], c = h[2], d = h[3];
            let e = h[4], f = h[5], g = h[6], hh = h[7];
            
            // 主循环
            for (let j = 0; j < 80; j++) {
                const S1 = (this.rotateRightBig(e, 14n) ^ this.rotateRightBig(e, 18n) ^ this.rotateRightBig(e, 41n));
                const ch = ((e & f) ^ (~e & g));
                const temp1 = (hh + S1 + ch + K[j] + w[j]) & 0xffffffffffffffffn;
                const S0 = (this.rotateRightBig(a, 28n) ^ this.rotateRightBig(a, 34n) ^ this.rotateRightBig(a, 39n));
                const maj = ((a & b) ^ (a & c) ^ (b & c));
                const temp2 = (S0 + maj) & 0xffffffffffffffffn;
                
                hh = g;
                g = f;
                f = e;
                e = (d + temp1) & 0xffffffffffffffffn;
                d = c;
                c = b;
                b = a;
                a = (temp1 + temp2) & 0xffffffffffffffffn;
            }
            
            // 更新哈希值
            h[0] = (h[0] + a) & 0xffffffffffffffffn;
            h[1] = (h[1] + b) & 0xffffffffffffffffn;
            h[2] = (h[2] + c) & 0xffffffffffffffffn;
            h[3] = (h[3] + d) & 0xffffffffffffffffn;
            h[4] = (h[4] + e) & 0xffffffffffffffffn;
            h[5] = (h[5] + f) & 0xffffffffffffffffn;
            h[6] = (h[6] + g) & 0xffffffffffffffffn;
            h[7] = (h[7] + hh) & 0xffffffffffffffffn;
        }
        
        // 转换为十六进制字符串
        let hex = '';
        for (let x of h) {
            hex += x.toString(16).padStart(16, '0');
        }
        
        return hex.toUpperCase();
    }

    /**
     * HMAC-SHA1算法核心实现
     * @param {ArrayBuffer} data - 要哈希的数据
     * @param {ArrayBuffer} key - 密钥
     * @returns {string} - 十六进制HMAC-SHA1值
     */
    static hmacSha1ArrayBuffer(data, key) {
        const blockSize = 64; // SHA1的块大小
        let keyData = new Uint8Array(key);
        
        // 处理密钥
        if (keyData.length > blockSize) {
            // 如果密钥长度大于块大小，对密钥进行SHA1哈希
            keyData = this.hexToUint8Array(this.sha1ArrayBuffer(key));
        }
        
        // 填充密钥到块大小
        const paddedKey = new Uint8Array(blockSize);
        paddedKey.set(keyData, 0);
        
        // 创建内部和外部填充
        const innerPad = new Uint8Array(blockSize);
        const outerPad = new Uint8Array(blockSize);
        for (let i = 0; i < blockSize; i++) {
            innerPad[i] = paddedKey[i] ^ 0x36;
            outerPad[i] = paddedKey[i] ^ 0x5C;
        }
        
        // 计算内部哈希
        const innerData = new Uint8Array(innerPad.length + data.byteLength);
        innerData.set(innerPad, 0);
        innerData.set(new Uint8Array(data), innerPad.length);
        const innerHash = this.sha1ArrayBuffer(innerData.buffer);
        
        // 计算外部哈希
        const outerData = new Uint8Array(outerPad.length + 20); // 20是SHA1哈希值的长度
        outerData.set(outerPad, 0);
        outerData.set(this.hexToUint8Array(innerHash), outerPad.length);
        const outerHash = this.sha1ArrayBuffer(outerData.buffer);
        
        return outerHash;
    }

    /**
     * HMAC-SHA256算法核心实现
     * @param {ArrayBuffer} data - 要哈希的数据
     * @param {ArrayBuffer} key - 密钥
     * @returns {string} - 十六进制HMAC-SHA256值
     */
    static hmacSha256ArrayBuffer(data, key) {
        const blockSize = 64; // SHA256的块大小
        let keyData = new Uint8Array(key);
        
        // 处理密钥
        if (keyData.length > blockSize) {
            // 如果密钥长度大于块大小，对密钥进行SHA256哈希
            keyData = this.hexToUint8Array(this.sha256ArrayBuffer(key));
        }
        
        // 填充密钥到块大小
        const paddedKey = new Uint8Array(blockSize);
        paddedKey.set(keyData, 0);
        
        // 创建内部和外部填充
        const innerPad = new Uint8Array(blockSize);
        const outerPad = new Uint8Array(blockSize);
        for (let i = 0; i < blockSize; i++) {
            innerPad[i] = paddedKey[i] ^ 0x36;
            outerPad[i] = paddedKey[i] ^ 0x5C;
        }
        
        // 计算内部哈希
        const innerData = new Uint8Array(innerPad.length + data.byteLength);
        innerData.set(innerPad, 0);
        innerData.set(new Uint8Array(data), innerPad.length);
        const innerHash = this.sha256ArrayBuffer(innerData.buffer);
        
        // 计算外部哈希
        const outerData = new Uint8Array(outerPad.length + 32); // 32是SHA256哈希值的长度
        outerData.set(outerPad, 0);
        outerData.set(this.hexToUint8Array(innerHash), outerPad.length);
        const outerHash = this.sha256ArrayBuffer(outerData.buffer);
        
        return outerHash;
    }

    /**
     * HMAC-SHA512算法核心实现
     * @param {ArrayBuffer} data - 要哈希的数据
     * @param {ArrayBuffer} key - 密钥
     * @returns {string} - 十六进制HMAC-SHA512值
     */
    static hmacSha512ArrayBuffer(data, key) {
        const blockSize = 128; // SHA512的块大小
        let keyData = new Uint8Array(key);
        
        // 处理密钥
        if (keyData.length > blockSize) {
            // 如果密钥长度大于块大小，对密钥进行SHA512哈希
            keyData = this.hexToUint8Array(this.sha512ArrayBuffer(key));
        }
        
        // 填充密钥到块大小
        const paddedKey = new Uint8Array(blockSize);
        paddedKey.set(keyData, 0);
        
        // 创建内部和外部填充
        const innerPad = new Uint8Array(blockSize);
        const outerPad = new Uint8Array(blockSize);
        for (let i = 0; i < blockSize; i++) {
            innerPad[i] = paddedKey[i] ^ 0x36;
            outerPad[i] = paddedKey[i] ^ 0x5C;
        }
        
        // 计算内部哈希
        const innerData = new Uint8Array(innerPad.length + data.byteLength);
        innerData.set(innerPad, 0);
        innerData.set(new Uint8Array(data), innerPad.length);
        const innerHash = this.sha512ArrayBuffer(innerData.buffer);
        
        // 计算外部哈希
        const outerData = new Uint8Array(outerPad.length + 64); // 64是SHA512哈希值的长度
        outerData.set(outerPad, 0);
        outerData.set(this.hexToUint8Array(innerHash), outerPad.length);
        const outerHash = this.sha512ArrayBuffer(outerData.buffer);
        
        return outerHash;
    }

    /**
     * 左旋转函数（32位）
     * @param {number} value - 要旋转的值
     * @param {number} amount - 旋转的位数
     * @returns {number}
     */
    static rotateLeft(value, amount) {
        return ((value << amount) | (value >>> (32 - amount))) >>> 0;
    }

    /**
     * 右旋转函数（32位）
     * @param {number} value - 要旋转的值
     * @param {number} amount - 旋转的位数
     * @returns {number}
     */
    static rotateRight(value, amount) {
        return ((value >>> amount) | (value << (32 - amount))) >>> 0;
    }

    /**
     * 右旋转函数（64位）
     * @param {bigint} value - 要旋转的值
     * @param {bigint} amount - 旋转的位数
     * @returns {bigint}
     */
    static rotateRightBig(value, amount) {
        return ((value >> amount) | (value << (64n - amount))) & 0xffffffffffffffffn;
    }

    /**
     * 将十六进制字符串转换为Uint8Array
     * @param {string} hex - 十六进制字符串
     * @returns {Uint8Array}
     */
    static hexToUint8Array(hex) {
        const result = new Uint8Array(hex.length / 2);
        for (let i = 0; i < hex.length; i += 2) {
            result[i / 2] = parseInt(hex.substr(i, 2), 16);
        }
        return result;
    }

    /**
     * 将MD5哈希结果转换为ArrayBuffer
     * @param {string} md5Hash - MD5哈希结果
     * @returns {ArrayBuffer}
     */
    static md5ArrayBufferToBuffer(md5Hash) {
        return this.hexToUint8Array(md5Hash).buffer;
    }
}