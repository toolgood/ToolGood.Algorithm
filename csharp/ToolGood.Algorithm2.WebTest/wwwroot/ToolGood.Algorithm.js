window.onload = async function () { await Blazor.start(); };
DistanceUnitType = new class { constructor() { this.MM = 1; this.CM = 2; this.DM = 3; this.M = 4; this.KM = 5; } }();
AreaUnitType = new class { constructor() { this.MM2 = 11; this.CM2 = 12; this.DM2 = 13; this.M2 = 14; this.KM2 = 15; } }();
VolumeUnitType = new class { constructor() { this.MM3 = 21; this.CM3 = 22; this.DM3 = 23; this.M3 = 24; this.KM3 = 25; } }();
MassUnitType = new class { constructor() { this.G = 31; this.KG = 32; this.T = 33; } }();
AlgorithmEngine = class {
    constructor() {
        this.UseExcelIndex = true;
        this.UseTempDict = false;
        this.UseLocalTime = false;
        this.DistanceUnit = DistanceUnitType.M;
        this.AreaUnit = AreaUnitType.M2;
        this.VolumeUnit = VolumeUnitType.M3;
        this.MassUnit = MassUnitType.KG;
        this.IgnoreCase = false;
    }
    __callResult = function (name, exp, def, data) {
        var json = DotNet.invokeMethod('ToolGood.Algorithm.WebAssembly', name, exp, def, JSON.stringify(data), JSON.stringify(this));
        var result = JSON.parse(json);
        this.LastParse = result["parse"];
        this.LastUseDef = result["useDef"];
        this.LastError = result["error"];
        this.LastResult = result["result"];
        return this.LastResult;
    }

    TryEvaluateString = function (exp, def, data) { return this.__callResult('TryEvaluateString', exp, def, data); }
    TryEvaluateNumber = function (exp, def, data) { return this.__callResult('TryEvaluateNumber', exp, def, data); }
    TryEvaluateBool = function (exp, def, data) { return this.__callResult('TryEvaluateBool', exp, def, data); }
    TryEvaluateDateTime = function (exp, def, data) { return this.__callResult('TryEvaluateDateTime', exp, def, data); }

    TryEvaluate = function (exp, def, data) {
        if (def == null) { return this.TryEvaluateString(exp, def, data); }
        if (typeof def == "number") { return this.TryEvaluateNumber(exp, def, data); }
        if (typeof def == "boolean") { return this.TryEvaluateBool(exp, def, data); }
        if (typeof def == "object" && def instanceof Date) { return this.TryEvaluateDateTime(exp, def, data); }
        return this.TryEvaluateString(exp, def, data);
    }

    GetSimplifiedFormula = function (exp, data) { return DotNet.invokeMethod('ToolGood.Algorithm.WebAssembly', 'GetSimplifiedFormula', exp, JSON.stringify(data), JSON.stringify(this)); }
    EvaluateFormula = function (exp, splitChars, data) { return DotNet.invokeMethod('ToolGood.Algorithm.WebAssembly', 'EvaluateFormula', exp, splitChars, JSON.stringify(data), JSON.stringify(this)); }
};
AlgorithmEngineHelper = new class {
    IsKeywords = function (parameter) { return DotNet.invokeMethod('ToolGood.Algorithm.WebAssembly', 'IsKeywords', parameter); }
    GetDiyNames = function (exp) { return JSON.parse(DotNet.invokeMethod('ToolGood.Algorithm.WebAssembly', 'GetDiyNames', exp)); }
    UnitConversion = function (src, oldSrcUnit, oldTarUnit, name) { return JSON.parse(DotNet.invokeMethod('ToolGood.Algorithm.WebAssembly', 'UnitConversion', src, oldSrcUnit, oldTarUnit, name)); }

}();


