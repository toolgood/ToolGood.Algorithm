/**
 * Function utility class
 */
export var FunctionUtil = {
    StartDateUtc: new Date(Date.UTC(1970, 0, 1, 0, 0, 0, 0)),

    FlattenToList: function(args, list) {
        if (args.IsArray) {
            args = args.ArrayValue;
        }

        for (var i = 0; i < args.length; i++) {
            var item = args[i];
            if (item.IsNumber) {
                list.push(item.NumberValue);
            } else if (item.IsArray) {
                var o = this.FlattenToList(item.ArrayValue, list);
                if (!o) { return false; }
            } else if (item.IsJson) {
                var array = item.ToArray(null);
                if (array.IsError) { return false; }
                var o = this.FlattenToList(array.ArrayValue, list);
                if (!o) { return false; }
            } else if (item.IsNull) {
                continue;
            } else {
                var o = item.ToNumber(null);
                if (o.IsError) { return false; }
                list.push(o.NumberValue);
            }
        }
        return true;
    },

    CompareValues: function(a, b) {
        if (a.IsNumber && b.IsNumber) {
            return a.NumberValue - b.NumberValue;
        } else if (a.IsText && b.IsText) {
            return a.TextValue.localeCompare(b.TextValue);
        } else if (a.IsBool && b.IsBool) {
            return a.BoolValue === b.BoolValue ? 0 : (a.BoolValue ? 1 : -1);
        } else if (a.IsDate && b.IsDate) {
            return a.DateValue - b.DateValue;
        }
        return 0;
    },

    GetGcd: function(a, b) {
        while (b !== 0) {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    },

    GetGcd_List: function(list) {
        list.sort(function(a, b) { return a - b; });
        var g = this.GetGcd(Math.floor(list[1]), Math.floor(list[0]));
        for (var i = 2; i < list.length; i++) {
            g = this.GetGcd(Math.floor(list[i]), g);
        }
        return g;
    },

    GetLcm: function(list) {
        list.sort(function(a, b) { return a - b; });
        list = list.filter(function(q) { return q > 1; });

        var a = Math.floor(list[0]);
        for (var i = 1; i < list.length; i++) {
            var b = Math.floor(list[i]);
            var g = b > a ? this.GetGcd(b, a) : this.GetGcd(a, b);
            a = a / g * b;
        }
        return a;
    },

    ParseSumIfMatch: function(s) {
        var c = s[0];
        if (c === '>' || c === '＞') {
            if (s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
                var d = parseFloat(s.substring(2).trim());
                if (!isNaN(d)) {
                    return { operator: ">=", value: d };
                }
            } else {
                var d = parseFloat(s.substring(1).trim());
                if (!isNaN(d)) {
                    return { operator: ">", value: d };
                }
            }
        } else if (c === '<' || c === '＜') {
            if (s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
                var d = parseFloat(s.substring(2).trim());
                if (!isNaN(d)) {
                    return { operator: "<=", value: d };
                }
            }
            else {
                var d = parseFloat(s.substring(1).trim());
                if (!isNaN(d)) {
                    return { operator: "<", value: d };
                }
            }
        } else if (c === '=' && s.length > 1 && s[1] === '=') {
            var d = parseFloat(s.substring(2).trim());
            if (!isNaN(d)) {
                return { operator: "=", value: d };
            }
        } else if ((c === '=' || c === '＝' || c === ' ') && s.length > 1) {
            var d = parseFloat(s.substring(1).trim());
            if (!isNaN(d)) {
                return { operator: "=", value: d };
            }
        } else if (c === '!' && s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
            var d = parseFloat(s.substring(2).trim());
            if (!isNaN(d)) {
                return { operator: "!=", value: d };
            }
        } else {
            var d = parseFloat(s.trim());
            if (!isNaN(d)) {
                return { operator: "=", value: d };
            }
        }
        return null;
    },

    TryParseBoolean: function(TextValue) {
        if (TextValue.toLowerCase() === "true" || TextValue.toLowerCase() === "yes") {
            return true;
        }
        if (TextValue.toLowerCase() === "false" || TextValue.toLowerCase() === "no") {
            return false;
        }
        if (TextValue === "1" || TextValue === "是" || TextValue === "有") {
            return true;
        }
        if (TextValue === "0" || TextValue === "否" || TextValue === "不是" || TextValue === "无" || TextValue === "没有") {
            return false;
        }
        return null;
    },

    GetCountIf: function(dbs, s, d) {
        let count = 0;
        for (let i = 0; i < dbs.length; i++) {
            let item = dbs[i];
            if (arguments.length === 2) {
                if (item === s) {
                    count++;
                }
            } else {
                if (this.CompareNumbers(item, d, s)) {
                    count++;
                }
            }
        }
        return count;
    },

    CompareNumbers: function(a, b, ss) {
        if (ss === '<') {
            return a < b;
        } else if (ss === '<=') {
            return a <= b;
        } else if (ss === '>') {
            return a > b;
        } else if (ss === '>=') {
            return a >= b;
        } else if (ss === '=') {
            return a === b;
        } else if (ss === '!=') {
            return a !== b;
        }
        return a !== b;
    },

    GetSumIf: function(list, arg1, arg2, arg3) {
        let sum = 0;
        for (let i = 0; i < list.length; i++) {
            if (arguments.length === 3) {
                let value = arg1;
                let sumdbs = arg2;
                if (list[i] === value) {
                    sum += sumdbs[i];
                }
            } else {
                let operator = arg1;
                let value = arg2;
                let sumdbs = arg3;
                if (this.CompareNumbers(list[i], value, operator)) {
                    sum += sumdbs[i];
                }
            }
        }
        return sum;
    }
};