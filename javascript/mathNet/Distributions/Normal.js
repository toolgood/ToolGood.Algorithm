



class Normal{}

Normal.CDF=function(mean, stddev, x){
    return 0.5 * SpecialFunctions.Erfc((mean - x) / (stddev * Constants.Sqrt2));
}
Normal.InvCDF=function(mean, stddev, p){
    return mean - (stddev * Constants.Sqrt2 * SpecialFunctions.ErfcInv(2.0 * p));
}

Normal.PDF=function(mean, stddev, x){
    var d = (x - mean) / stddev;
    return Math.exp(-0.5 * d * d) / (Constants.Sqrt2Pi * stddev);
}


module.exports=Normal;