namespace ToolGood.Algorithm.WebAssembly
{
    using Microsoft.JSInterop;
    using System.Text.Json;
    using ToolGood.Algorithm;
    using ToolGood.Algorithm.Enums;

    public class Program
    {
        private static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        [JSInvokable]
        public static string TryEvaluateString(string exp, string def = null, string data = null, string option = null)
        {
            AlgorithmEngineEx ae;
            if (option == null) {
                ae = new AlgorithmEngineEx();
            } else {
                var ops = JsonSerializer.Deserialize<Dictionary<string, object>>(option);
                ae = new AlgorithmEngineEx(bool.Parse(ops["IgnoreCase"].ToString()));
                ae.UseExcelIndex = bool.Parse(ops["UseExcelIndex"].ToString());
                ae.UseTempDict = bool.Parse(ops["UseTempDict"].ToString());
                ae.UseLocalTime = bool.Parse(ops["UseLocalTime"].ToString());
                ae.DistanceUnit = (DistanceUnitType)(int.Parse(ops["DistanceUnit"].ToString()));
                ae.AreaUnit = (AreaUnitType)(int.Parse(ops["AreaUnit"].ToString()));
                ae.VolumeUnit = (VolumeUnitType)(int.Parse(ops["VolumeUnit"].ToString()));
                ae.MassUnit = (MassUnitType)(int.Parse(ops["MassUnit"].ToString()));
            }
            if (data != null) {
                ae.AddParameterFromJson(data);
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["parse"] = false;
            result["useDef"] = false;
            result["error"] = null;
            try {
                var fun = ae.Parse(exp);
                result["parse"] = true;
                var obj = fun.Calculate(ae);
                if (obj.IsNull) {
                    result["result"] = null;
                    return JsonSerializer.Serialize(result);
                }
                if (obj.IsError) {
                    result["result"] = def;
                    result["useDef"] = true;
                    result["error"] = obj.ErrorMsg;
                    return JsonSerializer.Serialize(result);
                }
                if (obj.Type == OperandType.DATE) {
                    result["result"] = obj.DateValue.ToString();
                    result["error"] = ae.LastError;
                    return JsonSerializer.Serialize(result);
                }
                obj = obj.ToText("It can't be converted to string!");
                result["result"] = obj.TextValue;
                return JsonSerializer.Serialize(result);
            } catch (Exception ex) {
                result["error"] = ex.Message;
            }
            result["useDef"] = true;
            result["result"] = def;
            return JsonSerializer.Serialize(result);
        }

        [JSInvokable]
        public static string TryEvaluateNumber(string exp, decimal def, string data = null, string option = null)
        {
            AlgorithmEngineEx ae;
            if (option == null) {
                ae = new AlgorithmEngineEx();
            } else {
                var ops = JsonSerializer.Deserialize<Dictionary<string, object>>(option);
                ae = new AlgorithmEngineEx(bool.Parse(ops["IgnoreCase"].ToString()));
                ae.UseExcelIndex = bool.Parse(ops["UseExcelIndex"].ToString());
                ae.UseTempDict = bool.Parse(ops["UseTempDict"].ToString());
                ae.UseLocalTime = bool.Parse(ops["UseLocalTime"].ToString());
                ae.DistanceUnit = (DistanceUnitType)(int.Parse(ops["DistanceUnit"].ToString()));
                ae.AreaUnit = (AreaUnitType)(int.Parse(ops["AreaUnit"].ToString()));
                ae.VolumeUnit = (VolumeUnitType)(int.Parse(ops["VolumeUnit"].ToString()));
                ae.MassUnit = (MassUnitType)(int.Parse(ops["MassUnit"].ToString()));
            }
            if (data != null) {
                ae.AddParameterFromJson(data);
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["parse"] = false;
            result["useDef"] = false;
            result["error"] = null;
            try {
                var fun = ae.Parse(exp);
                result["parse"] = true;
                var obj = fun.Calculate(ae);
                obj = obj.ToNumber("It can't be converted to bool!");
                if (obj.IsError) {
                    result["result"] = def;
                    result["useDef"] = true;
                    result["error"] = obj.ErrorMsg;
                    return JsonSerializer.Serialize(result);
                }
                result["result"] = obj.NumberValue;
                return JsonSerializer.Serialize(result);
            } catch (Exception ex) {
                result["error"] = ex.Message;
            }
            result["useDef"] = true;
            result["result"] = def;
            return JsonSerializer.Serialize(result);
        }

        [JSInvokable]
        public static string TryEvaluateBool(string exp, bool def, string data = null, string option = null)
        {
            AlgorithmEngineEx ae;
            if (option == null) {
                ae = new AlgorithmEngineEx();
            } else {
                var ops = JsonSerializer.Deserialize<Dictionary<string, object>>(option);
                ae = new AlgorithmEngineEx(bool.Parse(ops["IgnoreCase"].ToString()));
                ae.UseExcelIndex = bool.Parse(ops["UseExcelIndex"].ToString());
                ae.UseTempDict = bool.Parse(ops["UseTempDict"].ToString());
                ae.UseLocalTime = bool.Parse(ops["UseLocalTime"].ToString());
                ae.DistanceUnit = (DistanceUnitType)(int.Parse(ops["DistanceUnit"].ToString()));
                ae.AreaUnit = (AreaUnitType)(int.Parse(ops["AreaUnit"].ToString()));
                ae.VolumeUnit = (VolumeUnitType)(int.Parse(ops["VolumeUnit"].ToString()));
                ae.MassUnit = (MassUnitType)(int.Parse(ops["MassUnit"].ToString()));
            }
            if (data != null) {
                ae.AddParameterFromJson(data);
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["parse"] = false;
            result["useDef"] = false;
            result["error"] = null;
            try {
                var fun = ae.Parse(exp);
                result["parse"] = true;
                var obj = fun.Calculate(ae);
                obj = obj.ToBoolean("It can't be converted to bool!");
                if (obj.IsError) {
                    result["result"] = def;
                    result["useDef"] = true;
                    result["error"] = obj.ErrorMsg;
                    return JsonSerializer.Serialize(result);
                }
                result["result"] = obj.BooleanValue;
                return JsonSerializer.Serialize(result);
            } catch (Exception ex) {
                result["error"] = ex.Message;
            }
            result["useDef"] = true;
            result["result"] = def;
            return JsonSerializer.Serialize(result);
        }

        [JSInvokable]
        public static string TryEvaluateDateTime(string exp, DateTime def, string data = null, string option = null)
        {
            AlgorithmEngineEx ae;
            if (option == null) {
                ae = new AlgorithmEngineEx();
            } else {
                var ops = JsonSerializer.Deserialize<Dictionary<string, object>>(option);
                ae = new AlgorithmEngineEx(bool.Parse(ops["IgnoreCase"].ToString()));
                ae.UseExcelIndex = bool.Parse(ops["UseExcelIndex"].ToString());
                ae.UseTempDict = bool.Parse(ops["UseTempDict"].ToString());
                ae.UseLocalTime = bool.Parse(ops["UseLocalTime"].ToString());
                ae.DistanceUnit = (DistanceUnitType)(int.Parse(ops["DistanceUnit"].ToString()));
                ae.AreaUnit = (AreaUnitType)(int.Parse(ops["AreaUnit"].ToString()));
                ae.VolumeUnit = (VolumeUnitType)(int.Parse(ops["VolumeUnit"].ToString()));
                ae.MassUnit = (MassUnitType)(int.Parse(ops["MassUnit"].ToString()));
            }
            if (data != null) {
                ae.AddParameterFromJson(data);
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["parse"] = false;
            result["useDef"] = false;
            result["error"] = null;
            try {
                var fun = ae.Parse(exp);
                result["parse"] = true;
                var obj = fun.Calculate(ae);
                obj = obj.ToMyDate("It can't be converted to datetime!");
                if (obj.IsError) {
                    result["result"] = def;
                    result["useDef"] = true;
                    result["error"] = ae.LastError;
                    return def.ToString("yyyy-MM-dd HH:mm:ss");
                }
                result["result"] = obj.DateValue.ToString();
                return JsonSerializer.Serialize(result);
            } catch (Exception ex) {
                result["error"] = ex.Message;
            }
            result["useDef"] = true;
            result["result"] = def.ToString("yyyy-MM-dd HH:mm:ss");
            return JsonSerializer.Serialize(result);
        }


        [JSInvokable]
        public static bool IsKeywords(string parameter)
        {
            return AlgorithmEngineHelper.IsKeywords(parameter);
        }

        [JSInvokable]
        public static string GetDiyNames(String exp)
        {
            return JsonSerializer.Serialize(AlgorithmEngineHelper.GetDiyNames(exp));
        }

        [JSInvokable]
        public static string UnitConversion(decimal src, string oldSrcUnit, string oldTarUnit, string name = null)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try {
                var r = AlgorithmEngineHelper.UnitConversion(src, oldSrcUnit, oldTarUnit, name);
                dic["code"] = 0;
                dic["result"] = r;
            } catch (Exception ex) {
                dic["code"] = 1;
                dic["error"] = ex.Message;
            }
            return JsonSerializer.Serialize(dic);
        }
    }
}