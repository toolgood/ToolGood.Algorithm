namespace ToolGood.Algorithm.WebAssembly
{
    using Microsoft.JSInterop;
    using ToolGood.Algorithm;
    using ToolGood.Algorithm.Enums;

    public class Program
    {
        private static void Main(string[] args)
        {
        }

        [JSInvokable]
        public static string TryEvaluateString(string exp, string def = null, string data = null, string option = null)
        {
            AlgorithmEngine ae;
            if (option == null) {
                ae = new AlgorithmEngine();
            } else {
                var ops = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(option);
                ae = new AlgorithmEngine(bool.Parse(ops["IgnoreCase"].ToString()));
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
            return ae.TryEvaluate(exp, def);
        }

        [JSInvokable]
        public static decimal TryEvaluateNumber(string exp, decimal def, string data = null, string option = null)
        {
            AlgorithmEngine ae;
            if (option == null) {
                ae = new AlgorithmEngine();
            } else {
                var ops = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(option);
                ae = new AlgorithmEngine(bool.Parse(ops["IgnoreCase"].ToString()));
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
            return ae.TryEvaluate(exp, def);
        }

        [JSInvokable]
        public static bool TryEvaluateBool(string exp, bool def, string data = null, string option = null)
        {
            AlgorithmEngine ae;
            if (option == null) {
                ae = new AlgorithmEngine();
            } else {
                var ops = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(option);
                ae = new AlgorithmEngine(bool.Parse(ops["IgnoreCase"].ToString()));
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
            return ae.TryEvaluate(exp, def);
        }

        [JSInvokable]
        public static DateTime TryEvaluateDateTime(string exp, DateTime def, string data = null, string option = null)
        {
            AlgorithmEngine ae;
            if (option == null) {
                ae = new AlgorithmEngine();
            } else {
                var ops = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(option);
                ae = new AlgorithmEngine(bool.Parse(ops["IgnoreCase"].ToString()));
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
            return ae.TryEvaluate(exp, def);
        }



    }
}
