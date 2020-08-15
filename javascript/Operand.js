const {OperandType} = require('./OperandType');
const {MyDate} = require('./MyDate');

class Operand{}
Operand.prototype.IsNull=function(){return false;}
Operand.prototype.IsError=function(){return false;}
Operand.prototype.ErrorMsg=function() {return null;}
Operand.prototype.Type=function() {return OperandType.ERROR;}
Operand.prototype.NumberValue=function() {return 0.0;}
Operand.prototype.IntValue=function(){return 0;}
Operand.prototype.TextValue=function() {return null;}
Operand.prototype.BooleanValue=function() {return false;}
Operand.prototype.ArrayValue=function() {return null;}
Operand.prototype.JsonValue=function() {return null;}
Operand.prototype.DateValue=function() {return null;}

Operand.Create=function(value){
    if(value == null || value==undefined){
        return new OperandNull();
    }else if(value instanceof Boolean){
        return new OperandBoolean(value);
    }else if(value instanceof String){
        return new OperandString(value);
    }else if(value instanceof Date){
        return new OperandDate(new MyDate(value));
    }else if(value instanceof Number){
        return new OperandNumber(value);
    }else if(value instanceof Array){
        var array=[];
        for (let index = 0; index < value.length; index++) {
            const val = value[index];
            array.push(Operand.Create(val));
        }
        return new OperandArray(array);
    }else if(value instanceof Object){
        return new OperandJson(value);
    }
}
Operand.CreateJson=function(txt){
    if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]")))
    {
        try
        {
            return new OperandJson(JSON.parse(txt));
        } catch (e) { }
    }
    return Operand.Error("string to json is error!");
}
Operand.Error=function(errorMsg){
    return new OperandError(errorMsg);
}
Operand.prototype.ToNumber=function(errorMessage){
    if (this.Type() == OperandType.NUMBER) { return this; }
    if (this.IsError()) { return this; }
    if (this.Type() == OperandType.BOOLEAN) { return Create(this.BooleanValue() ? 1.0 : 0.0); }
    if (this.Type() == OperandType.DATE) { return Create(this.DateValue().ToNumber()); }
    if (this.Type() == OperandType.STRING)
    {
        try {
            var d= parseFloat(this.TextValue())
            return Operand.Create(d);
        } catch (e) { }
    }
    return Operand.Error(errorMessage);
}
Operand.prototype.ToBoolean=function(errorMessage){
    if (this.Type() == OperandType.BOOLEAN) {return this;}
    if (this.IsError()) {return this;}
    if (this.Type() == OperandType.NUMBER) {return Operand.Create(NumberValue() != 0);}
    if (this.Type() == OperandType.DATE) {return Operand.Create((this.DateValue().ToNumber()) != 0);}
    if (this.Type() == OperandType.STRING) {
        if (this.TextValue().toLowerCase()==("true")) { return Operand.Create(true); }
        if (this.TextValue().toLowerCase()==("false")) { return Operand.Create(false); }
        if (this.TextValue()==("1")) { return Operand.Create(true); }
        if (this.TextValue()==("0")) { return Operand.Create(false); }
    }
    return Operand.Error(errorMessage);
}
Operand.prototype.ToText=function(errorMessage){
    if (this.Type() == OperandType.STRING) { return this; }
    if (this.IsError()) { return this; }
    if (this.Type() == OperandType.NUMBER) {
        var str=(this.NumberValue()).toString();
        if(str.indexOf(".")!=-1){ str= str.replace(/(\\.)?0+$/,"") }
        return Operand.Create(str); 
    }
    if (this.Type() == OperandType.BOOLEAN) { return Operand.Create(this.BooleanValue() ? "TRUE" : "FALSE"); }
    if (this.Type() == OperandType.DATE) { return Operand.Create(this.DateValue().toString()); }

    return Operand.Error(errorMessage);
}
Operand.prototype.ToDate=function(errorMessage){
    if (this.Type() == OperandType.DATE) { return this; }
    if (this.IsError()) { return this; }
    if (this.Type() == OperandType.NUMBER) { return Operand.Create(new MyDate(NumberValue())); }
    if (this.Type() == OperandType.STRING)    {
        var date=MyDate.parse(TextValue());
        if (date != null){
            return Operand.Create(date);
        }
    }
    return Operand.Error(errorMessage);
}
Operand.prototype.ToJson=function(errorMessage){
    if (this.Type() == OperandType.JSON) { return this; }
    if (this.IsError()) { return this; }
    if (this.Type() == OperandType.STRING) {
        var txt = this.TextValue();
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                var json = JSON.parse(txt);
                return Operand.Create(json);
            } catch (e) { }
        }
    }
    return Operand.Error(errorMessage);
}
Operand.prototype.ToArray=function(errorMessage){
    if (this.Type() == OperandType.ARRARY) { return this; }
    if (this.IsError()) { return this; }
    if (this.Type() == OperandType.JSON) {
        var json=this.JsonValue();
        if (json instanceof Array){
            var list=[];
            for (let index = 0; index < json.length; index++) {
                const v = json[index];
                list.push(Operand.Create(v));
            }
            return Operand.Create(list);
        }
    }
    return Operand.Error(errorMessage);
}

class OperandT extends Operand {
    constructor(value) {
		this.value=value;
    }
}

class OperandArray extends OperandT{}
OperandArray.prototype.OperandArray =function(){return this.value;}
OperandArray.prototype.Type =function(){return OperandType.ARRARY;}

class OperandBoolean extends OperandT{}
OperandBoolean.prototype.BooleanValue =function(){return this.value;}
OperandBoolean.prototype.Type =function(){return OperandType.BOOLEAN;}

class OperandDate extends OperandT{}
OperandDate.prototype.DateValue =function(){return this.value;}
OperandDate.prototype.Type =function(){return OperandType.DATE;}

class OperandJson extends OperandT{}
OperandJson.prototype.JsonValue =function(){return this.value;}
OperandJson.prototype.Type =function(){return OperandType.JSON;}

class OperandNumber extends OperandT{}
OperandNumber.prototype.NumberValue =function(){return this.value;}
OperandNumber.prototype.Type =function(){return OperandType.NUMBER;}

class OperandString extends OperandT{}
OperandString.prototype.TextValue =function(){return this.value;}
OperandString.prototype.Type =function(){return OperandType.STRING;}

class OperandError extends Operand{
    constructor(errorMsg) {
		this.errorMsg=errorMsg;
    }
}
OperandError.prototype.IsError=function(){return true;}
OperandError.prototype.Type =function(){return OperandType.ERROR;}
OperandError.prototype.ErrorMsg =function(){return this.errorMsg;}

class OperandNull extends Operand{}
OperandNull.prototype.IsNull=function(){return true;}
OperandNull.prototype.Type =function(){return OperandType.NULL;}


String.prototype.startsWith = function(s) { 
    if (s == null || s == "" || this.length == 0 || s.length > this.length) 
        return false; 
    if (this.substr(0, s.length) == s) 
        return true;
    return false; 
}

String.prototype.endsWith = function(s) {
     if (s == null || s == "" || this.length == 0|| s.length > this.length)
          return false;
     if (this.substring(this.length - s.length) == s)
          return true;
    return false;
}



module.exports = Operand;
