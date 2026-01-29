/**
 * Function utility class
 */
export var FunctionUtil = {
    StartDateUtc: new Date(Date.UTC(1970, 0, 1, 0, 0, 0, 0)),

    F_base_GetList: function(args, list) {
        for(var i = 0; i < args.length; i++) {
            var item = args[i];
            if(item.IsNumber) {
                list.push(item.NumberValue);
            } else if(item.IsArray) {
                var o = this.F_base_GetList(item.ArrayValue, list);
                if(!o) { return false; }
            } else if(item.IsJson) {
                var i = item.ToArray(null);
                if(i.IsError) { return false; }
                var o = this.F_base_GetList(i.ArrayValue, list);
                if(!o) { return false; }
            } else {
                var o = item.ToNumber(null);
                if(o.IsError) { return false; }
                list.push(o.NumberValue);
            }
        }
        return true;
    },

    F_base_compare: function(a, b) {
        if(a.IsNumber && b.IsNumber) {
            return a.NumberValue - b.NumberValue;
        } else if(a.IsText && b.IsText) {
            return a.TextValue.localeCompare(b.TextValue);
        } else if(a.IsBool && b.IsBool) {
            return a.BoolValue === b.BoolValue ? 0 : (a.BoolValue ? 1 : -1);
        } else if(a.IsDate && b.IsDate) {
            return a.DateValue - b.DateValue;
        }
        return 0;
    },

    F_base_gcd: function(a, b) {
        while(b !== 0) {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    },

    F_base_gcd_List: function(list) {
        list.sort(function(a, b) { return a - b; });
        var g = this.F_base_gcd(Math.floor(list[1]), Math.floor(list[0]));
        for(var i = 2; i < list.length; i++) {
            g = this.F_base_gcd(Math.floor(list[i]), g);
        }
        return g;
    },

    F_base_lgm: function(list) {
        list.sort(function(a, b) { return a - b; });
        list = list.filter(function(q) { return q > 1; });

        var a = Math.floor(list[0]);
        for(var i = 1; i < list.length; i++) {
            var b = Math.floor(list[i]);
            var g = b > a ? this.F_base_gcd(b, a) : this.F_base_gcd(a, b);
            a = a / g * b;
        }
        return a;
    },

    sumifMatch: function(s) {
        var c = s[0];
        if(c === '>' || c === '＞') {
            if(s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
                var d = parseFloat(s.substring(2).trim());
                if(!isNaN(d)) {
                    return { operator: ">=", value: d };
                }
            } else {
                var d = parseFloat(s.substring(1).trim());
                if(!isNaN(d)) {
                    return { operator: ">", value: d };
                }
            }
        } else if(c === '<' || c === '＜') {
            if(s.length > 1 && (s[1] === '=' || s[1] === '＝')) {
                var d = parseFloat(s.substring(2).trim());
                if(!isNaN(d)) {
                    return { operator: "<=", value: d };
                }
            } else {
                var d = parseFloat(s.substring(1).trim());
                if(!isNaN(d)) {
                    return { operator: "<", value: d };
                }
            }
        } else if((c === '=' || c === '＝' || c === ' ') && s.length > 1) {
            var d = parseFloat(s.substring(1).trim());
            if(!isNaN(d)) {
                return { operator: "=", value: d };
            }
        } else {
            var d = parseFloat(s.trim());
            if(!isNaN(d)) {
                return { operator: "=", value: d };
            }
        }
        return null;
    }
};
