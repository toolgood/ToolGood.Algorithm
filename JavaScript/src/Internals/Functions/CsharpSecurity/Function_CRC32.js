import { Function_2 } from '../Function_2';
import { Operand } from '../../../Operand';

/**
 * Function_CRC32
 */
export class Function_CRC32 extends Function_2 {
    /**
     * @param {FunctionBase} func1
     * @param {FunctionBase} func2
     */
    constructor(func1, func2) {
        super(func1, func2);
    }
    
    /**
     * @param {AlgorithmEngine} engine
     * @returns {Operand}
     */
    evaluate(engine) {
        const args1 = this._arg1.evaluate(engine);
        if (args1.isNotText) {
            args1.toText('Function \'{0}\' parameter {1} is error!', 'CRC32', 1);
            if (args1.isError) {
                return args1;
            }
        }
        
        try {
            let encoding = 'utf-8';
            if (this._arg2 !== null) {
                const args2 = this._arg2.evaluate(engine);
                if (args2.isNotText) {
                    args2.toText('Function \'{0}\' parameter {1} is error!', 'CRC32', 2);
                    if (args2.isError) {
                        return args2;
                    }
                }
                encoding = args2.textValue;
            }
            
            const encoder = new TextEncoder(encoding);
            const buffer = encoder.encode(args1.textValue);
            const t = this.getCrc32String(buffer);
            return Operand.create(t);
        } catch (ex) {
            return Operand.error('Function \'CRC32\'is error!' + ex.message);
        }
    }
    
    /**
     * @param {string[]} stringBuilder
     * @param {boolean} addBrackets
     */
    toString(stringBuilder, addBrackets) {
        this.addFunction(stringBuilder, 'CRC32');
    }
    
    /**
     * @param {Uint8Array} buffer
     * @returns {string}
     */
    getCrc32String(buffer) {
        const crc32 = new Crc32Hash();
        crc32.append(buffer);
        const retVal = crc32.finish();
        return Array.from(retVal).map(byte => byte.toString(16).padStart(2, '0')).join('');
    }
}

/**
 * Crc32Hash
 */
class Crc32Hash {
    /**
     * @constructor
     */
    constructor() {
        this.crc = 0;
        this.CrcSeed = 0xFFFFFFFF;
        this.CrcTable = [
            0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA, 0x076DC419,
            0x706AF48F, 0xE963A535, 0x9E6495A3, 0x0EDB8832, 0x79DCB8A4,
            0xE0D5E91E, 0x97D2D988, 0x09B64C2B, 0x7EB17CBD, 0xE7B82D07,
            0x90BF1D91, 0x1DB71064, 0x6AB020F2, 0xF3B97148, 0x84BE41DE,
            0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7, 0x136C9856,
            0x646BA8C0, 0xFD62F97A, 0x8A65C9EC, 0x14015C4F, 0x63066CD9,
            0xFA0F3D63, 0x83D385C7, 0x136C9856, 0x646BA8C0, 0xFD62F97A,
            0x8A65C9EC, 0x14015C4F, 0x63066CD9, 0xFA0F3D63, 0x8D080DF5,
            0x3B6E20C8, 0x4C69105E, 0xD56041E4, 0xA2677172, 0x3C03E4D1,
            0x4B04D447, 0xD20D85FD, 0xA50AB56B, 0x35B5A8FA, 0x42B2986C,
            0xDBBBC9D6, 0xACBCF940, 0x32D86CE3, 0x45DF5C75, 0xDCD60DCF,
            0xABD13D59, 0x26D930AC, 0x51DE003A, 0xC8D75180, 0xBFD06116,
            0x21B4F4B5, 0x56B3C423, 0xCFBA9599, 0xB8BDA50F, 0x2802B89E,
            0x5F058808, 0xC60CD9B2, 0xB10BE924, 0x2F6F7C87, 0x58684C11,
            0xC1611DAB, 0xB6662D3D, 0x76DC4190, 0x01DB7106, 0x98D220BC,
            0xEFD5102A, 0x71B18589, 0x06B6B51F, 0x9FBFE4A5, 0xE8B8D433,
            0x7807C9A2, 0x0F00F934, 0x9609A88E, 0xE10E9818, 0x7F6A0DBB,
            0x086D3D2D, 0x91646C97, 0xE6635C01, 0x6B6B51F4, 0x1C6C6162,
            0x856530D8, 0xF262004E, 0x6C0695ED, 0x1B01A57B, 0x8208F4C1,
            0xF50FC457, 0x65B0D9C6, 0x12B7E950, 0x8BBEB8EA, 0xFCB9887C,
            0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3, 0xFBD44C65, 0x4DB26158,
            0x3AB551CE, 0xA3BC0074, 0xD4BB30E2, 0x4ADFA541, 0x3DD895D7,
            0xA4D1C46D, 0xD3D6F4FB, 0x4369E96A, 0x346ED9FC, 0xAD678846,
            0xDA60B8D0, 0x44042D73, 0x33031DE5, 0xAA0A4C5F, 0xDD0D7CC9,
            0x5005713C, 0x270241AA, 0xBE0B1010, 0xC90C2086, 0x5768B525,
            0x206F85B3, 0xB966D409, 0xCE61E49F, 0x5EDEF90E, 0x29D9C998,
            0xB0D09822, 0xC7D7A8B4, 0x59B33D17, 0x2EB40D81, 0xB7BD5C3B,
            0xC0BA6CAD, 0xEDB88320, 0x9ABFB3B6, 0x03B6E20C, 0x74B1D29A,
            0xEAD54739, 0x9DD277AF, 0x04DB2615, 0x73DC1683, 0xE3630B12,
            0x94643B84, 0x0D6D6A3E, 0x7A6A5AA8, 0xE40ECF0B, 0x9309FF9D,
            0x0A00AE27, 0x7D079EB1, 0xF00F9344, 0x8708A3D2, 0x1E01F268,
            0x6906C2FE, 0xF762575D, 0x806567CB, 0x196C3671, 0x6E6B06E7,
            0xFED41B76, 0x89D32BE0, 0x10DA7A5A, 0x67DD4ACC, 0xF9B9DF6F,
            0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5, 0xD6D6A3E8, 0xA1D1937E,
            0x38D8C2C4, 0x4FDFF252, 0xD1BB67F1, 0xA6BC5767, 0x3FB506DD,
            0x48B2364B, 0xD80D2BDA, 0xAF0A1B4C, 0x36034AF6, 0x41047A60,
            0xDF60EFC3, 0xA867DF55, 0x316E8EEF, 0x4669BE79, 0xCB61B38C,
            0xBC66831A, 0x53B39330, 0x24B4A3A6, 0xBAD03605, 0xCDD70693,
            0x54DE5729, 0x23D967BF, 0xB3667A2E, 0xC4614AB8, 0x5D681B02,
            0x2A6F2B94, 0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B, 0x2D02EF8D
        ];
    }
    
    /**
     * @param {Uint8Array} buffer
     */
    append(buffer) {
        this.appendRange(buffer, 0, buffer.length);
    }
    
    /**
     * @param {Uint8Array} buffer
     * @param {number} offset
     * @param {number} count
     */
    appendRange(buffer, offset, count) {
        this.crc ^= this.CrcSeed;
        while (--count >= 0) {
            this.crc = this.CrcTable[(this.crc ^ buffer[offset++]) & 0xFF] ^ (this.crc >> 8);
        }
        this.crc ^= this.CrcSeed;
    }
    
    /**
     * @returns {Uint8Array}
     */
    finish() {
        const result = new Uint8Array(4);
        result[0] = (this.crc >> 24) & 0xFF;
        result[1] = (this.crc >> 16) & 0xFF;
        result[2] = (this.crc >> 8) & 0xFF;
        result[3] = this.crc & 0xFF;
        return result;
    }
}
