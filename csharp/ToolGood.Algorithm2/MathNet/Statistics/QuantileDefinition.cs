namespace ToolGood.Algorithm.MathNet.Numerics.Statistics
{
    internal enum QuantileDefinition
    {
        R1 = 1,// SAS3 = 1, EmpiricalInvCDF = 1,
        R2 = 2,// SAS5 = 2, EmpiricalInvCDFAverage = 2,
        R3 = 3, //SAS2 = 3, Nearest = 3,
        R4 = 4, //SAS1 = 4, California = 4,
        R5 = 5, //Hydrology = 5, Hazen = 5,
        R6 = 6, //SAS4 = 6, Nist = 6, Weibull = 6, SPSS = 6,
        R7 = 7, Excel = 7,// Mode = 7, S = 7,
        R8 = 8, //Median = 8, Default = 8,
        R9 = 9, //Normal = 9,
    }
}