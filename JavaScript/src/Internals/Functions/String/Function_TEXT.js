import { Function_2 } from '../Function_2.js';
import { Operand } from '../../../Operand.js';

class Function_TEXT extends Function_2 {
    get Name() {
        return "Text";
    }

    constructor(z) {
        super(z);
    }

    evaluate(work, tempParameter) {
        let args1 = this.a.evaluate(work, tempParameter);
        if (args1.IsError) {
            return args1;
        }
        let args2 = this.getText_2(work, tempParameter);
        if (args2.IsError) {
            return args2;
        }

        if (args1.IsText) {
            return args1;
        } else if (args1.IsBoolean) {
            return Operand.Create(args1.BooleanValue ? 'TRUE' : 'FALSE');
        } else if (args1.IsNumber) {
            try {
                return Operand.Create(this.formatNumber(args1.NumberValue, args2.TextValue));
            } catch (e) {
                // 非法格式字符串,与 C# FormatException 对应
                return this.parameterError(2);
            }
        } else if (args1.IsDate) {
            try {
                return Operand.Create(this.formatDate(args1.DateValue, args2.TextValue));
            } catch (e) {
                return this.parameterError(2);
            }
        }
        let args1Text = args1.ToText("Function '{0}' parameter {1} is error!", this.Name, 1);
        if (args1Text.IsError) {
            return args1Text;
        }
        return Operand.Create(args1Text.TextValue);
    }

    // 数字格式:标准格式(G/F/N/D/C/E/P/X)与自定义格式(0/#/.//,/%/科学计数)
    formatNumber(value, format) {
        // 标准数字格式
        let stdMatch = /^([GgFfNnDdCcEePpXx])(\d*)$/.exec(format);
        if (stdMatch) {
            return this.formatNumberStandard(value, stdMatch[1], stdMatch[2] ? parseInt(stdMatch[2], 10) : 0);
        }

        // 分节处理:正数;负数;零
        let sections = format.split(';');
        let section = sections[0];
        let isNegative = value < 0;
        if (isNegative && sections.length > 1) {
            section = sections[1];
            value = -value;
        } else if (value === 0 && sections.length > 2) {
            section = sections[2];
        }

        // 百分比
        let percentCount = (section.match(/%/g) || []).length;
        if (percentCount > 0) {
            value = value * Math.pow(100, percentCount);
            section = section.replace(/%/g, '');
        }

        // 科学计数法
        let expMatch = /[eE][+-]?\d+/.exec(section);
        if (expMatch) {
            return this.formatExponential(value, section, expMatch);
        }

        // 无占位符:整段视为字面量(如 "abc")
        if (!/[0#]/.test(section)) {
            return section;
        }

        // 整数部分与小数部分格式
        let dotIndex = section.indexOf('.');
        let intFormat = dotIndex >= 0 ? section.substring(0, dotIndex) : section;
        let fracFormat = dotIndex >= 0 ? section.substring(dotIndex + 1) : '';

        // 缩放:格式末尾的逗号每出现一次除以 1000
        let scale = 0;
        while (intFormat.endsWith(',')) {
            intFormat = intFormat.substring(0, intFormat.length - 1);
            scale++;
        }
        if (scale > 0) {
            value = value / Math.pow(1000, scale);
        }

        let useThousands = intFormat.includes(',');
        let intPlaceholder = intFormat.replace(/,/g, '');
        let intZeroCount = (intPlaceholder.match(/0/g) || []).length;
        let hasIntPlaceholder = /[0#]/.test(intPlaceholder);

        // 小数位占位符(0 保留尾零,# 可去掉尾零)
        let fracPositions = [];
        for (let i = 0; i < fracFormat.length; i++) {
            let c = fracFormat[i];
            fracPositions.push(c === '0' || c === '#' ? c : null);
        }
        let fracLen = fracPositions.filter(x => x !== null).length;

        // 四舍五入到 fracLen 位(AwayFromZero,加微小量修正二进制误差)
        let factor = Math.pow(10, fracLen);
        let scaled = Math.round(Math.abs(value) * factor + 1e-9);
        let intPart = Math.floor(scaled / factor);
        let fracScaled = scaled % factor;

        let intStr;
        if (hasIntPlaceholder) {
            intStr = intPart.toString();
            while (intStr.length < intZeroCount) {
                intStr = '0' + intStr;
            }
        } else {
            intStr = intPart === 0 ? '' : intPart.toString();
        }
        if (useThousands) {
            intStr = intStr.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        }

        let fracStr = '';
        if (fracLen > 0) {
            fracStr = fracScaled.toString().padStart(fracLen, '0');
            // 移除 # 对应的尾零
            for (let i = fracPositions.length - 1; i >= 0; i--) {
                if (fracPositions[i] === '#' && fracStr.endsWith('0')) {
                    fracStr = fracStr.substring(0, fracStr.length - 1);
                } else {
                    break;
                }
            }
        }

        let result = intStr + (fracStr.length > 0 ? '.' + fracStr : '');
        if (isNegative && !section.includes('-')) {
            result = '-' + result;
        }
        return result;
    }

    formatNumberStandard(value, code, digits) {
        switch (code) {
            case 'G': case 'g':
                return value.toString();
            case 'F': case 'f': {
                let d = digits > 0 ? digits : 2;
                return value.toFixed(d);
            }
            case 'N': case 'n': {
                let d = digits > 0 ? digits : 2;
                let parts = Math.abs(value).toFixed(d).split('.');
                parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                let s = parts.join('.');
                return value < 0 ? '-' + s : s;
            }
            case 'D': case 'd': {
                let s = Math.round(Math.abs(value)).toString().padStart(digits, '0');
                return value < 0 ? '-' + s : s;
            }
            case 'C': case 'c': {
                let d = digits > 0 ? digits : 2;
                let parts = Math.abs(value).toFixed(d).split('.');
                parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                let s = '¤' + parts.join('.');
                return value < 0 ? '-' + s : s;
            }
            case 'E': case 'e': {
                let d = digits > 0 ? digits : 6;
                let s = value.toExponential(d).replace(/e([+-])(\d+)/, (m, sign, exp) => 'E' + sign + exp.padStart(3, '0'));
                return s;
            }
            case 'P': case 'p': {
                let d = digits > 0 ? digits : 2;
                let parts = (Math.abs(value) * 100).toFixed(d).split('.');
                parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
                let s = parts.join('.') + '%';
                return value < 0 ? '-' + s : s;
            }
            case 'X': case 'x': {
                let s = Math.round(value).toString(16).toUpperCase();
                if (digits > 0) {
                    s = s.padStart(digits, '0');
                }
                return s;
            }
        }
        return value.toString();
    }

    formatExponential(value, section, expMatch) {
        let expPart = expMatch[0];
        let expDigits = parseInt(expPart.match(/\d+$/)[0].length, 10);
        let mantissaFmt = section.substring(0, expMatch.index);
        let mDot = mantissaFmt.indexOf('.');
        let mFrac = mDot >= 0 ? mantissaFmt.length - mDot - 1 : 0;
        let abs = Math.abs(value);
        let exp = abs === 0 ? 0 : Math.floor(Math.log10(abs));
        let mantissa = value / Math.pow(10, exp);
        let factor = Math.pow(10, mFrac);
        mantissa = Math.round(mantissa * factor + (mantissa >= 0 ? 1e-9 : -1e-9)) / factor;
        // 进位调整(如 9.99 -> 10.00)
        if (Math.abs(mantissa) >= 10) {
            mantissa /= 10;
            exp += 1;
        }
        let mantissaStr = mantissa.toFixed(mFrac);
        let expStr = Math.abs(exp).toString().padStart(expDigits, '0');
        let sign = '';
        if (expPart[1] === '+') {
            sign = exp >= 0 ? '+' : '-';
        } else if (expPart[1] === '-') {
            sign = exp < 0 ? '-' : '';
        } else {
            sign = exp < 0 ? '-' : '';
        }
        return mantissaStr + expPart[0] + sign + expStr;
    }

    // 日期格式:与 .NET 自定义日期格式串对齐(yyyy/MM/dd/HH/mm/ss/tt 等)
    formatDate(date, format) {
        const MONTH_NAMES = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        const DAY_NAMES = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

        let result = '';
        let i = 0;
        while (i < format.length) {
            let c = format[i];
            let j = i;
            while (j < format.length && format[j] === c) {
                j++;
            }
            let count = j - i;
            let token = '';
            if (c === 'y') {
                let year = date.getFullYear();
                token = count <= 2 ? year.toString().padStart(2, '0').slice(-2) : year.toString().padStart(count, '0');
            } else if (c === 'M') {
                let month = date.getMonth();
                if (count >= 4) token = MONTH_NAMES[month];
                else if (count === 3) token = MONTH_NAMES[month].substring(0, 3);
                else token = (month + 1).toString().padStart(count, '0');
            } else if (c === 'd') {
                let day = date.getDate();
                if (count >= 4) token = DAY_NAMES[date.getDay()];
                else if (count === 3) token = DAY_NAMES[date.getDay()].substring(0, 3);
                else token = day.toString().padStart(count, '0');
            } else if (c === 'H') {
                token = date.getHours().toString().padStart(count, '0');
            } else if (c === 'h') {
                let h = date.getHours() % 12;
                if (h === 0) h = 12;
                token = h.toString().padStart(count, '0');
            } else if (c === 'm') {
                token = date.getMinutes().toString().padStart(count, '0');
            } else if (c === 's') {
                token = date.getSeconds().toString().padStart(count, '0');
            } else if (c === 'f') {
                token = date.getMilliseconds().toString().padStart(3, '0').substring(0, count);
            } else if (c === 't') {
                token = date.getHours() < 12 ? 'AM' : 'PM';
                if (count === 1) token = token[0];
            } else if (c === "'") {
                // 单引号字面量
                let end = format.indexOf("'", i + 1);
                if (end < 0) end = format.length;
                token = format.substring(i + 1, end);
                result += token;
                i = end < format.length ? end + 1 : end;
                continue;
            } else {
                token = c.repeat(count);
            }
            result += token;
            i = j;
        }
        return result;
    }
}

export { Function_TEXT };
