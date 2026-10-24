// CsharpSecurity.cpp - 对应 csharp/.../Internals/Functions/CsharpSecurity/（8 个哈希 / HMAC 函数）
// 说明：C++ 无内置密码学原语，这里按 RFC 1321(MD5)、RFC 3174(SHA-1)、FIPS 180-4(SHA-2)、
//       RFC 2104(HMAC) 自行实现；输入按 UTF-8 字节处理，输出为小写十六进制字符串。
#include "CsharpSecurity.h"

#include <cstddef>
#include <cstdint>
#include <string>
#include <utility>
#include <vector>

#include "../../../AlgorithmEngine.h"
#include "../../../Operands/Operand.h"

namespace toolgood {

namespace {

using Bytes = std::vector<uint8_t>;

// ---------------------------------------------------------------------------
// 基础工具
// ---------------------------------------------------------------------------

uint32_t RotateLeft32(uint32_t v, int bits) { return (v << bits) | (v >> (32 - bits)); }

uint32_t RotateRight32(uint32_t v, int bits) { return (v >> bits) | (v << (32 - bits)); }

uint64_t RotateRight64(uint64_t v, int bits) { return (v >> bits) | (v << (64 - bits)); }

uint32_t LoadBE32(const uint8_t* p) {
    return (static_cast<uint32_t>(p[0]) << 24) | (static_cast<uint32_t>(p[1]) << 16) |
           (static_cast<uint32_t>(p[2]) << 8) | static_cast<uint32_t>(p[3]);
}

uint32_t LoadLE32(const uint8_t* p) {
    return static_cast<uint32_t>(p[0]) | (static_cast<uint32_t>(p[1]) << 8) |
           (static_cast<uint32_t>(p[2]) << 16) | (static_cast<uint32_t>(p[3]) << 24);
}

void StoreBE32(uint8_t* p, uint32_t v) {
    p[0] = static_cast<uint8_t>((v >> 24) & 0xFF);
    p[1] = static_cast<uint8_t>((v >> 16) & 0xFF);
    p[2] = static_cast<uint8_t>((v >> 8) & 0xFF);
    p[3] = static_cast<uint8_t>(v & 0xFF);
}

void StoreLE32(uint8_t* p, uint32_t v) {
    p[0] = static_cast<uint8_t>(v & 0xFF);
    p[1] = static_cast<uint8_t>((v >> 8) & 0xFF);
    p[2] = static_cast<uint8_t>((v >> 16) & 0xFF);
    p[3] = static_cast<uint8_t>((v >> 24) & 0xFF);
}

uint64_t LoadBE64(const uint8_t* p) {
    uint64_t v = 0;
    for (int i = 0; i < 8; i++) v = (v << 8) | static_cast<uint64_t>(p[i]);
    return v;
}

void StoreBE64(uint8_t* p, uint64_t v) {
    for (int i = 7; i >= 0; i--) {
        p[i] = static_cast<uint8_t>(v & 0xFF);
        v >>= 8;
    }
}

/// std::string（UTF-8 字节）转字节数组
Bytes ToBytes(const std::string& text) { return Bytes(text.begin(), text.end()); }

/// 小写十六进制（对应 C# BitConverter.ToString(...).Replace("-", "").ToLower()）
std::string ToHexUpper(const Bytes& data) {
    static const char kDigits[] = "0123456789ABCDEF";
    std::string result;
    result.reserve(data.size() * 2);
    for (uint8_t b : data) {
        result.push_back(kDigits[b >> 4]);
        result.push_back(kDigits[b & 0x0F]);
    }
    return result;
}

/// 消息填充：追加 0x80，补零到 blockSize-lengthBytes 字节，末尾写入比特长度
/// （MD5 的长度字段为小端，SHA-1/SHA-2 为大端；SHA-512 为 128 位长度）
Bytes PadMessage(const Bytes& data, size_t blockSize, size_t lengthBytes, bool lengthLittleEndian) {
    Bytes msg = data;
    const uint64_t bitLen = static_cast<uint64_t>(data.size()) * 8;
    msg.push_back(0x80);
    while (msg.size() % blockSize != blockSize - lengthBytes) msg.push_back(0);
    if (lengthBytes == 16) {
        for (int i = 0; i < 8; i++) msg.push_back(0);
    }
    if (lengthLittleEndian) {
        for (int i = 0; i < 8; i++) msg.push_back(static_cast<uint8_t>((bitLen >> (8 * i)) & 0xFF));
    } else {
        for (int i = 7; i >= 0; i--) msg.push_back(static_cast<uint8_t>((bitLen >> (8 * i)) & 0xFF));
    }
    return msg;
}

// ---------------------------------------------------------------------------
// MD5（RFC 1321）
// ---------------------------------------------------------------------------

Bytes Md5(const Bytes& data) {
    static const uint32_t kTable[64] = {
        0xd76aa478u, 0xe8c7b756u, 0x242070dbu, 0xc1bdceeeu, 0xf57c0fafu, 0x4787c62au, 0xa8304613u,
        0xfd469501u, 0x698098d8u, 0x8b44f7afu, 0xffff5bb1u, 0x895cd7beu, 0x6b901122u, 0xfd987193u,
        0xa679438eu, 0x49b40821u, 0xf61e2562u, 0xc040b340u, 0x265e5a51u, 0xe9b6c7aau, 0xd62f105du,
        0x02441453u, 0xd8a1e681u, 0xe7d3fbc8u, 0x21e1cde6u, 0xc33707d6u, 0xf4d50d87u, 0x455a14edu,
        0xa9e3e905u, 0xfcefa3f8u, 0x676f02d9u, 0x8d2a4c8au, 0xfffa3942u, 0x8771f681u, 0x6d9d6122u,
        0xfde5380cu, 0xa4beea44u, 0x4bdecfa9u, 0xf6bb4b60u, 0xbebfbc70u, 0x289b7ec6u, 0xeaa127fau,
        0xd4ef3085u, 0x04881d05u, 0xd9d4d039u, 0xe6db99e5u, 0x1fa27cf8u, 0xc4ac5665u, 0xf4292244u,
        0x432aff97u, 0xab9423a7u, 0xfc93a039u, 0x655b59c3u, 0x8f0ccc92u, 0xffeff47du, 0x85845dd1u,
        0x6fa87e4fu, 0xfe2ce6e0u, 0xa3014314u, 0x4e0811a1u, 0xf7537e82u, 0xbd3af235u, 0x2ad7d2bbu,
        0xeb86d391u};
    static const int kShift[64] = {7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
                                   5, 9,  14, 20, 5, 9,  14, 20, 5, 9,  14, 20, 5, 9,  14, 20,
                                   4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
                                   6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21};

    uint32_t a0 = 0x67452301u;
    uint32_t b0 = 0xefcdab89u;
    uint32_t c0 = 0x98badcfeu;
    uint32_t d0 = 0x10325476u;

    const Bytes msg = PadMessage(data, 64, 8, true);
    for (size_t off = 0; off < msg.size(); off += 64) {
        uint32_t m[16];
        for (int i = 0; i < 16; i++) m[i] = LoadLE32(&msg[off + static_cast<size_t>(i) * 4]);

        uint32_t a = a0, b = b0, c = c0, d = d0;
        for (int i = 0; i < 64; i++) {
            uint32_t f = 0;
            int g = 0;
            if (i < 16) {
                f = (b & c) | (~b & d);
                g = i;
            } else if (i < 32) {
                f = (d & b) | (~d & c);
                g = (5 * i + 1) % 16;
            } else if (i < 48) {
                f = b ^ c ^ d;
                g = (3 * i + 5) % 16;
            } else {
                f = c ^ (b | ~d);
                g = (7 * i) % 16;
            }
            const uint32_t tmp = d;
            d = c;
            c = b;
            b = b + RotateLeft32(a + f + kTable[i] + m[g], kShift[i]);
            a = tmp;
        }
        a0 += a;
        b0 += b;
        c0 += c;
        d0 += d;
    }

    Bytes out(16);
    StoreLE32(&out[0], a0);
    StoreLE32(&out[4], b0);
    StoreLE32(&out[8], c0);
    StoreLE32(&out[12], d0);
    return out;
}

// ---------------------------------------------------------------------------
// SHA-1（RFC 3174）
// ---------------------------------------------------------------------------

Bytes Sha1(const Bytes& data) {
    uint32_t h0 = 0x67452301u;
    uint32_t h1 = 0xEFCDAB89u;
    uint32_t h2 = 0x98BADCFEu;
    uint32_t h3 = 0x10325476u;
    uint32_t h4 = 0xC3D2E1F0u;

    const Bytes msg = PadMessage(data, 64, 8, false);
    for (size_t off = 0; off < msg.size(); off += 64) {
        uint32_t w[80];
        for (int i = 0; i < 16; i++) w[i] = LoadBE32(&msg[off + static_cast<size_t>(i) * 4]);
        for (int i = 16; i < 80; i++) {
            w[i] = RotateLeft32(w[i - 3] ^ w[i - 8] ^ w[i - 14] ^ w[i - 16], 1);
        }

        uint32_t a = h0, b = h1, c = h2, d = h3, e = h4;
        for (int i = 0; i < 80; i++) {
            uint32_t f = 0;
            uint32_t k = 0;
            if (i < 20) {
                f = (b & c) | (~b & d);
                k = 0x5A827999u;
            } else if (i < 40) {
                f = b ^ c ^ d;
                k = 0x6ED9EBA1u;
            } else if (i < 60) {
                f = (b & c) | (b & d) | (c & d);
                k = 0x8F1BBCDCu;
            } else {
                f = b ^ c ^ d;
                k = 0xCA62C1D6u;
            }
            const uint32_t temp = RotateLeft32(a, 5) + f + e + k + w[i];
            e = d;
            d = c;
            c = RotateLeft32(b, 30);
            b = a;
            a = temp;
        }
        h0 += a;
        h1 += b;
        h2 += c;
        h3 += d;
        h4 += e;
    }

    Bytes out(20);
    StoreBE32(&out[0], h0);
    StoreBE32(&out[4], h1);
    StoreBE32(&out[8], h2);
    StoreBE32(&out[12], h3);
    StoreBE32(&out[16], h4);
    return out;
}

// ---------------------------------------------------------------------------
// SHA-256（FIPS 180-4）
// ---------------------------------------------------------------------------

Bytes Sha256(const Bytes& data) {
    static const uint32_t kTable[64] = {
        0x428a2f98u, 0x71374491u, 0xb5c0fbcfu, 0xe9b5dba5u, 0x3956c25bu, 0x59f111f1u, 0x923f82a4u,
        0xab1c5ed5u, 0xd807aa98u, 0x12835b01u, 0x243185beu, 0x550c7dc3u, 0x72be5d74u, 0x80deb1feu,
        0x9bdc06a7u, 0xc19bf174u, 0xe49b69c1u, 0xefbe4786u, 0x0fc19dc6u, 0x240ca1ccu, 0x2de92c6fu,
        0x4a7484aau, 0x5cb0a9dcu, 0x76f988dau, 0x983e5152u, 0xa831c66du, 0xb00327c8u, 0xbf597fc7u,
        0xc6e00bf3u, 0xd5a79147u, 0x06ca6351u, 0x14292967u, 0x27b70a85u, 0x2e1b2138u, 0x4d2c6dfcu,
        0x53380d13u, 0x650a7354u, 0x766a0abbu, 0x81c2c92eu, 0x92722c85u, 0xa2bfe8a1u, 0xa81a664bu,
        0xc24b8b70u, 0xc76c51a3u, 0xd192e819u, 0xd6990624u, 0xf40e3585u, 0x106aa070u, 0x19a4c116u,
        0x1e376c08u, 0x2748774cu, 0x34b0bcb5u, 0x391c0cb3u, 0x4ed8aa4au, 0x5b9cca4fu, 0x682e6ff3u,
        0x748f82eeu, 0x78a5636fu, 0x84c87814u, 0x8cc70208u, 0x90befffau, 0xa4506cebu, 0xbef9a3f7u,
        0xc67178f2u};

    uint32_t h[8] = {0x6a09e667u, 0xbb67ae85u, 0x3c6ef372u, 0xa54ff53au,
                     0x510e527fu, 0x9b05688cu, 0x1f83d9abu, 0x5be0cd19u};

    const Bytes msg = PadMessage(data, 64, 8, false);
    for (size_t off = 0; off < msg.size(); off += 64) {
        uint32_t w[64];
        for (int i = 0; i < 16; i++) w[i] = LoadBE32(&msg[off + static_cast<size_t>(i) * 4]);
        for (int i = 16; i < 64; i++) {
            const uint32_t s0 =
                RotateRight32(w[i - 15], 7) ^ RotateRight32(w[i - 15], 18) ^ (w[i - 15] >> 3);
            const uint32_t s1 =
                RotateRight32(w[i - 2], 17) ^ RotateRight32(w[i - 2], 19) ^ (w[i - 2] >> 10);
            w[i] = w[i - 16] + s0 + w[i - 7] + s1;
        }

        uint32_t a = h[0], b = h[1], c = h[2], d = h[3];
        uint32_t e = h[4], f = h[5], g = h[6], hh = h[7];
        for (int i = 0; i < 64; i++) {
            const uint32_t s1 = RotateRight32(e, 6) ^ RotateRight32(e, 11) ^ RotateRight32(e, 25);
            const uint32_t ch = (e & f) ^ (~e & g);
            const uint32_t temp1 = hh + s1 + ch + kTable[i] + w[i];
            const uint32_t s0 = RotateRight32(a, 2) ^ RotateRight32(a, 13) ^ RotateRight32(a, 22);
            const uint32_t maj = (a & b) ^ (a & c) ^ (b & c);
            const uint32_t temp2 = s0 + maj;
            hh = g;
            g = f;
            f = e;
            e = d + temp1;
            d = c;
            c = b;
            b = a;
            a = temp1 + temp2;
        }
        h[0] += a;
        h[1] += b;
        h[2] += c;
        h[3] += d;
        h[4] += e;
        h[5] += f;
        h[6] += g;
        h[7] += hh;
    }

    Bytes out(32);
    for (int i = 0; i < 8; i++) StoreBE32(&out[static_cast<size_t>(i) * 4], h[i]);
    return out;
}

// ---------------------------------------------------------------------------
// SHA-512（FIPS 180-4）
// ---------------------------------------------------------------------------

Bytes Sha512(const Bytes& data) {
    static const uint64_t kTable[80] = {
        0x428a2f98d728ae22ull, 0x7137449123ef65cdull, 0xb5c0fbcfec4d3b2full, 0xe9b5dba58189dbbcull,
        0x3956c25bf348b538ull, 0x59f111f1b605d019ull, 0x923f82a4af194f9bull, 0xab1c5ed5da6d8118ull,
        0xd807aa98a3030242ull, 0x12835b0145706fbeull, 0x243185be4ee4b28cull, 0x550c7dc3d5ffb4e2ull,
        0x72be5d74f27b896full, 0x80deb1fe3b1696b1ull, 0x9bdc06a725c71235ull, 0xc19bf174cf692694ull,
        0xe49b69c19ef14ad2ull, 0xefbe4786384f25e3ull, 0x0fc19dc68b8cd5b5ull, 0x240ca1cc77ac9c65ull,
        0x2de92c6f592b0275ull, 0x4a7484aa6ea6e483ull, 0x5cb0a9dcbd41fbd4ull, 0x76f988da831153b5ull,
        0x983e5152ee66dfabull, 0xa831c66d2db43210ull, 0xb00327c898fb213full, 0xbf597fc7beef0ee4ull,
        0xc6e00bf33da88fc2ull, 0xd5a79147930aa725ull, 0x06ca6351e003826full, 0x142929670a0e6e70ull,
        0x27b70a8546d22ffcull, 0x2e1b21385c26c926ull, 0x4d2c6dfc5ac42aedull, 0x53380d139d95b3dfull,
        0x650a73548baf63deull, 0x766a0abb3c77b2a8ull, 0x81c2c92e47edaee6ull, 0x92722c851482353bull,
        0xa2bfe8a14cf10364ull, 0xa81a664bbc423001ull, 0xc24b8b70d0f89791ull, 0xc76c51a30654be30ull,
        0xd192e819d6ef5218ull, 0xd69906245565a910ull, 0xf40e35855771202aull, 0x106aa07032bbd1b8ull,
        0x19a4c116b8d2d0c8ull, 0x1e376c085141ab53ull, 0x2748774cdf8eeb99ull, 0x34b0bcb5e19b48a8ull,
        0x391c0cb3c5c95a63ull, 0x4ed8aa4ae3418acbull, 0x5b9cca4f7763e373ull, 0x682e6ff3d6b2b8a3ull,
        0x748f82ee5defb2fcull, 0x78a5636f43172f60ull, 0x84c87814a1f0ab72ull, 0x8cc702081a6439ecull,
        0x90befffa23631e28ull, 0xa4506cebde82bde9ull, 0xbef9a3f7b2c67915ull, 0xc67178f2e372532bull,
        0xca273eceea26619cull, 0xd186b8c721c0c207ull, 0xeada7dd6cde0eb1eull, 0xf57d4f7fee6ed178ull,
        0x06f067aa72176fbaull, 0x0a637dc5a2c898a6ull, 0x113f9804bef90daeull, 0x1b710b35131c471bull,
        0x28db77f523047d84ull, 0x32caab7b40c72493ull, 0x3c9ebe0a15c9bebcull, 0x431d67c49c100d4cull,
        0x4cc5d4becb3e42b6ull, 0x597f299cfc657e2aull, 0x5fcb6fab3ad6faecull, 0x6c44198c4a475817ull};

    uint64_t h[8] = {0x6a09e667f3bcc908ull, 0xbb67ae8584caa73bull, 0x3c6ef372fe94f82bull,
                     0xa54ff53a5f1d36f1ull, 0x510e527fade682d1ull, 0x9b05688c2b3e6c1full,
                     0x1f83d9abfb41bd6bull, 0x5be0cd19137e2179ull};

    const Bytes msg = PadMessage(data, 128, 16, false);
    for (size_t off = 0; off < msg.size(); off += 128) {
        uint64_t w[80];
        for (int i = 0; i < 16; i++) w[i] = LoadBE64(&msg[off + static_cast<size_t>(i) * 8]);
        for (int i = 16; i < 80; i++) {
            const uint64_t s0 =
                RotateRight64(w[i - 15], 1) ^ RotateRight64(w[i - 15], 8) ^ (w[i - 15] >> 7);
            const uint64_t s1 =
                RotateRight64(w[i - 2], 19) ^ RotateRight64(w[i - 2], 61) ^ (w[i - 2] >> 6);
            w[i] = w[i - 16] + s0 + w[i - 7] + s1;
        }

        uint64_t a = h[0], b = h[1], c = h[2], d = h[3];
        uint64_t e = h[4], f = h[5], g = h[6], hh = h[7];
        for (int i = 0; i < 80; i++) {
            const uint64_t s1 = RotateRight64(e, 14) ^ RotateRight64(e, 18) ^ RotateRight64(e, 41);
            const uint64_t ch = (e & f) ^ (~e & g);
            const uint64_t temp1 = hh + s1 + ch + kTable[i] + w[i];
            const uint64_t s0 = RotateRight64(a, 28) ^ RotateRight64(a, 34) ^ RotateRight64(a, 39);
            const uint64_t maj = (a & b) ^ (a & c) ^ (b & c);
            const uint64_t temp2 = s0 + maj;
            hh = g;
            g = f;
            f = e;
            e = d + temp1;
            d = c;
            c = b;
            b = a;
            a = temp1 + temp2;
        }
        h[0] += a;
        h[1] += b;
        h[2] += c;
        h[3] += d;
        h[4] += e;
        h[5] += f;
        h[6] += g;
        h[7] += hh;
    }

    Bytes out(64);
    for (int i = 0; i < 8; i++) StoreBE64(&out[static_cast<size_t>(i) * 8], h[i]);
    return out;
}

// ---------------------------------------------------------------------------
// HMAC（RFC 2104）
// ---------------------------------------------------------------------------

using HashFunc = Bytes (*)(const Bytes&);

/// 密钥规范化：超过分组长度先哈希；不足则右侧补 0（RFC 2104 第 2 节）
Bytes Hmac(HashFunc hash, size_t blockSize, const Bytes& key, const Bytes& message) {
    Bytes k = key;
    if (k.size() > blockSize) k = hash(k);
    k.resize(blockSize, 0);

    Bytes inner;
    inner.reserve(blockSize + message.size());
    for (size_t i = 0; i < blockSize; i++) {
        inner.push_back(static_cast<uint8_t>(k[i] ^ 0x36));
    }
    inner.insert(inner.end(), message.begin(), message.end());

    Bytes outer;
    outer.reserve(blockSize + 64);
    for (size_t i = 0; i < blockSize; i++) {
        outer.push_back(static_cast<uint8_t>(k[i] ^ 0x5C));
    }
    const Bytes innerHash = hash(inner);
    outer.insert(outer.end(), innerHash.begin(), innerHash.end());
    return hash(outer);
}

/// HMAC 求值：文本按 UTF-8 转字节，密钥为第二个参数
std::string HmacHex(HashFunc hash, size_t blockSize, const std::string& message,
                    const std::string& secret) {
    return ToHexUpper(Hmac(hash, blockSize, ToBytes(secret), ToBytes(message)));
}

// ---------------------------------------------------------------------------
// 函数实现
// ---------------------------------------------------------------------------

class Function_MD5 : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_MD5(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "MD5", 1, 1);
    }

    std::string Name() const override { return "MD5"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(ToHexUpper(Md5(ToBytes(args1.TextValue()))));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_SHA1 : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_SHA1(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SHA1", 1, 1);
    }

    std::string Name() const override { return "SHA1"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(ToHexUpper(Sha1(ToBytes(args1.TextValue()))));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_SHA256 : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_SHA256(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SHA256", 1, 1);
    }

    std::string Name() const override { return "SHA256"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(ToHexUpper(Sha256(ToBytes(args1.TextValue()))));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_SHA512 : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_SHA512(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SHA512", 1, 1);
    }

    std::string Name() const override { return "SHA512"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(ToHexUpper(Sha512(ToBytes(args1.TextValue()))));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_HMACMD5 : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_HMACMD5(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "HmacMD5", 2, 2);
    }

    std::string Name() const override { return "HmacMD5"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        return Operand::Create(HmacHex(Md5, 64, args1.TextValue(), args2.TextValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_HMACSHA1 : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_HMACSHA1(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "HmacSHA1", 2, 2);
    }

    std::string Name() const override { return "HmacSHA1"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        return Operand::Create(HmacHex(Sha1, 64, args1.TextValue(), args2.TextValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_HMACSHA256 : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_HMACSHA256(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "HmacSHA256", 2, 2);
    }

    std::string Name() const override { return "HmacSHA256"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        return Operand::Create(HmacHex(Sha256, 64, args1.TextValue(), args2.TextValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_HMACSHA512 : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_HMACSHA512(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "HmacSHA512", 2, 2);
    }

    std::string Name() const override { return "HmacSHA512"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        return Operand::Create(HmacHex(Sha512, 128, args1.TextValue(), args2.TextValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

}  // namespace

Function NewFunction_MD5(std::vector<Function> args) {
    return std::make_shared<Function_MD5>(std::move(args));
}

Function NewFunction_SHA1(std::vector<Function> args) {
    return std::make_shared<Function_SHA1>(std::move(args));
}

Function NewFunction_SHA256(std::vector<Function> args) {
    return std::make_shared<Function_SHA256>(std::move(args));
}

Function NewFunction_SHA512(std::vector<Function> args) {
    return std::make_shared<Function_SHA512>(std::move(args));
}

Function NewFunction_HMACMD5(std::vector<Function> args) {
    return std::make_shared<Function_HMACMD5>(std::move(args));
}

Function NewFunction_HMACSHA1(std::vector<Function> args) {
    return std::make_shared<Function_HMACSHA1>(std::move(args));
}

Function NewFunction_HMACSHA256(std::vector<Function> args) {
    return std::make_shared<Function_HMACSHA256>(std::move(args));
}

Function NewFunction_HMACSHA512(std::vector<Function> args) {
    return std::make_shared<Function_HMACSHA512>(std::move(args));
}

}  // namespace toolgood
