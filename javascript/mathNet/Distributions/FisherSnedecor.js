

class FisherSnedecor{}


FisherSnedecor.CDF=function(d1, d2, x)
{
    return SpecialFunctions.BetaRegularized(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2));
}

FisherSnedecor.InvCDF=function(d1, d2, p) 
{
    return Brent.FindRoot(function(x){
        SpecialFunctions.BetaRegularized(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2)) - p;
    }, 0, 1000,  1e-12);
}

module.exports = FisherSnedecor;
