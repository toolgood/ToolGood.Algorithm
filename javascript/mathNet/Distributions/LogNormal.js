


class LogNormal{}


LogNormal.CDF=function(mu, sigma, x){
    return x < 0.0 ? 0.0 : 0.5 * (1.0 + SpecialFunctions.Erf((Math.log(x) - mu) / (sigma * Constants.Sqrt2)));
}

LogNormal.InvCDF=function(mu, sigma, p){
    return p <= 0.0 ? 0.0 : p >= 1.0 ? Infinity
        : Math.exp(mu - sigma * Constants.Sqrt2 * SpecialFunctions.ErfcInv(2.0 * p));
}



module.exports=LogNormal;