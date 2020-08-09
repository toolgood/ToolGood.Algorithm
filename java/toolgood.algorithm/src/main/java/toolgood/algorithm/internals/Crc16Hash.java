package toolgood.algorithm.internals;

public class Crc16Hash {
     
    /**
     * CRC16_CCITT：多项式x16+x12+x5+1（0x1021），初始值0x0000，低位在前，高位在后，结果与0x0000异或
     * 0x8408是0x1021按位颠倒后的结果。
     * @param buffer
     * @return
     */
    public static int CRC16_CCITT(byte[] buffer) {
        int wCRCin = 0x0000;
        int wCPoly = 0x8408;
        for (byte b : buffer) {
            wCRCin ^= ((int) b & 0x00ff);
            for (int j = 0; j < 8; j++) {
                if ((wCRCin & 0x0001) != 0) {
                    wCRCin >>= 1;
                    wCRCin ^= wCPoly;
                } else {
                    wCRCin >>= 1;
                }
            }
        }
//        wCRCin=(wCRCin<<8)|(wCRCin>>8);
//        wCRCin &= 0xffff;
        return wCRCin ^= 0x0000;
 
    }
 
    /**
     * CRC-CCITT (0xFFFF)
     * CRC16_CCITT_FALSE：多项式x16+x12+x5+1（0x1021），初始值0xFFFF，低位在后，高位在前，结果与0x0000异或
     *
     * @param buffer
     * @return
     */
    public static int CRC16_CCITT_FALSE(byte[] buffer) {
        int wCRCin = 0xffff;
        int wCPoly = 0x1021;
        for (byte b : buffer) {
            for (int i = 0; i < 8; i++) {
                boolean bit = ((b >> (7 - i) & 1) == 1);
                boolean c15 = ((wCRCin >> 15 & 1) == 1);
                wCRCin <<= 1;
                if (c15 ^ bit)
                    wCRCin ^= wCPoly;
            }
        }
        wCRCin &= 0xffff;
        return wCRCin ^= 0x0000;
    }
 
    /**
     * CRC-CCITT (XModem)
     * CRC16_XMODEM：多项式x16+x12+x5+1（0x1021），初始值0x0000，低位在后，高位在前，结果与0x0000异或
     *
     * @param buffer
     * @return
     */
    public static int CRC16_XMODEM(byte[] buffer) {
        int wCRCin = 0x0000; // initial value 65535
        int wCPoly = 0x1021; // 0001 0000 0010 0001 (0, 5, 12)
        for (byte b : buffer) {
            for (int i = 0; i < 8; i++) {
                boolean bit = ((b >> (7 - i) & 1) == 1);
                boolean c15 = ((wCRCin >> 15 & 1) == 1);
                wCRCin <<= 1;
                if (c15 ^ bit)
                    wCRCin ^= wCPoly;
            }
        }
        wCRCin &= 0xffff;
        return wCRCin ^= 0x0000;
    }
 
 
    /**
     * CRC16_X25：多项式x16+x12+x5+1（0x1021），初始值0xffff，低位在前，高位在后，结果与0xFFFF异或
     * 0x8408是0x1021按位颠倒后的结果。
     * @param buffer
     * @return
     */
    public static int CRC16_X25(byte[] buffer) {
        int wCRCin = 0xffff;
        int wCPoly = 0x8408;
        for (byte b : buffer) {
            wCRCin ^= ((int) b & 0x00ff);
            for (int j = 0; j < 8; j++) {
                if ((wCRCin & 0x0001) != 0) {
                    wCRCin >>= 1;
                    wCRCin ^= wCPoly;
                } else {
                    wCRCin >>= 1;
                }
            }
        }
        return wCRCin ^= 0xffff;
    }
 
    /**
     * CRC-16 (Modbus)
     * CRC16_MODBUS：多项式x16+x15+x2+1（0x8005），初始值0xFFFF，低位在前，高位在后，结果与0x0000异或
     * 0xA001是0x8005按位颠倒后的结果
     * @param buffer
     * @return
     */
    public static int CRC16_MODBUS(byte[] buffer) {
        int wCRCin = 0xffff;
        int POLYNOMIAL = 0xa001;
        for (byte b : buffer) {
            wCRCin ^= ((int) b & 0x00ff);
            for (int j = 0; j < 8; j++) {
                if ((wCRCin & 0x0001) != 0) {
                    wCRCin >>= 1;
                    wCRCin ^= POLYNOMIAL;
                } else {
                    wCRCin >>= 1;
                }
            }
        }
        return wCRCin ^= 0x0000;
    }
 
    /**
     * CRC-16
     * CRC16_IBM：多项式x16+x15+x2+1（0x8005），初始值0x0000，低位在前，高位在后，结果与0x0000异或
     * 0xA001是0x8005按位颠倒后的结果
     * @param buffer
     * @return
     */
    public static int CRC16_IBM(byte[] buffer) {
        int wCRCin = 0x0000;
        int wCPoly = 0xa001;
        for (byte b : buffer) {
            wCRCin ^= ((int) b & 0x00ff);
            for (int j = 0; j < 8; j++) {
                if ((wCRCin & 0x0001) != 0) {
                    wCRCin >>= 1;
                    wCRCin ^= wCPoly;
                } else {
                    wCRCin >>= 1;
                }
            }
        }
        return wCRCin ^= 0x0000;
    }
 
    /**
     * CRC16_MAXIM：多项式x16+x15+x2+1（0x8005），初始值0x0000，低位在前，高位在后，结果与0xFFFF异或
     * 0xA001是0x8005按位颠倒后的结果
     * @param buffer
     * @return
     */
    public static int CRC16_MAXIM(byte[] buffer) {
        int wCRCin = 0x0000;
        int wCPoly = 0xa001;
        for (byte b : buffer) {
            wCRCin ^= ((int) b & 0x00ff);
            for (int j = 0; j < 8; j++) {
                if ((wCRCin & 0x0001) != 0) {
                    wCRCin >>= 1;
                    wCRCin ^= wCPoly;
                } else {
                    wCRCin >>= 1;
                }
            }
        }
        return wCRCin ^= 0xffff;
    }
 
    /**
     * CRC16_USB：多项式x16+x15+x2+1（0x8005），初始值0xFFFF，低位在前，高位在后，结果与0xFFFF异或
     * 0xA001是0x8005按位颠倒后的结果
     * @param buffer
     * @return
     */
    public static int CRC16_USB(byte[] buffer) {
        int wCRCin = 0xFFFF;
        int wCPoly = 0xa001;
        for (byte b : buffer) {
            wCRCin ^= ((int) b & 0x00ff);
            for (int j = 0; j < 8; j++) {
                if ((wCRCin & 0x0001) != 0) {
                    wCRCin >>= 1;
                    wCRCin ^= wCPoly;
                } else {
                    wCRCin >>= 1;
                }
            }
        }
        return wCRCin ^= 0xffff;
    }
 
    /**
     * CRC16_DNP：多项式x16+x13+x12+x11+x10+x8+x6+x5+x2+1（0x3D65），初始值0x0000，低位在前，高位在后，结果与0xFFFF异或
     * 0xA6BC是0x3D65按位颠倒后的结果
     * @param buffer
     * @return
     */
    public static int CRC16_DNP(byte[] buffer) {
        int wCRCin = 0x0000;
        int wCPoly = 0xA6BC;
        for (byte b : buffer) {
            wCRCin ^= ((int) b & 0x00ff);
            for (int j = 0; j < 8; j++) {
                if ((wCRCin & 0x0001) != 0) {
                    wCRCin >>= 1;
                    wCRCin ^= wCPoly;
                } else {
                    wCRCin >>= 1;
                }
            }
        }
        return wCRCin ^= 0xffff;
    }
}