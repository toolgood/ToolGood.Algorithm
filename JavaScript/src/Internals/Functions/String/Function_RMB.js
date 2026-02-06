import { Function_1 } from '../Function_1.js';
import { Operand } from '../../../Operand.js';

class Function_RMB extends Function_1 {
    get Name() {
        return "RMB";
    }

    constructor(a) {
        super(a);
    }

    Evaluate(work, tempParameter) {
        let args1 = this.GetNumber_1(work, tempParameter);
        if (args1.IsError) { return args1; }
        return Operand.Create(this.F_base_ToChineseRMB(args1.NumberValue));
    }

    F_base_ToChineseRMB(x) {
        if (x === 0) return "零元";
        
        let digits = ["零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖"];
        let units = ["", "拾", "佰", "仟"];
        let bigUnits = ["", "万", "亿", "兆"];
        let decimalUnits = ["角", "分"];
        
        let result = "";
        let integerPart = Math.floor(x);
        let decimalPart = Math.round((x - integerPart) * 100);
        
        // 处理整数部分
        if (integerPart < 0) {
            result = "负";
            integerPart = -integerPart;
        }
        
        if (integerPart === 0) {
            result += "零";
        } else {
            let unitIndex = 0;
            let bigUnitIndex = 0;
            
            while (integerPart > 0) {
                let section = integerPart % 10000;
                if (section > 0) {
                    let sectionStr = "";
                    let tempSection = section;
                    let sectionUnitIndex = 0;
                    
                    while (tempSection > 0) {
                        let digit = tempSection % 10;
                        if (digit > 0) {
                            sectionStr = digits[digit] + units[sectionUnitIndex] + sectionStr;
                        } else {
                            // 处理连续的零
                            if (sectionStr && !sectionStr.startsWith("零")) {
                                sectionStr = "零" + sectionStr;
                            }
                        }
                        tempSection = Math.floor(tempSection / 10);
                        sectionUnitIndex++;
                    }
                    
                    result = sectionStr + bigUnits[bigUnitIndex] + result;
                }
                
                integerPart = Math.floor(integerPart / 10000);
                bigUnitIndex++;
            }
        }
        
        result += "元";
        
        // 处理小数部分
        if (decimalPart === 0) {
            result += "整";
        } else {
            let jiao = Math.floor(decimalPart / 10);
            let fen = decimalPart % 10;
            
            if (jiao > 0) {
                result += digits[jiao] + decimalUnits[0];
            } else if (fen > 0) {
                result += "零";
            }
            
            if (fen > 0) {
                result += digits[fen] + decimalUnits[1];
            }
        }
        
        return result;
    }

   
}

export { Function_RMB };
