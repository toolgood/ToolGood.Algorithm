


class Poisson{}

Poisson.PMF=function(lambda, k){
    return Math.exp(-lambda + (k * Math.log(lambda)) - SpecialFunctions.FactorialLn(k));
}

Poisson.CDF=function(lambda, x){
    return 1.0 - SpecialFunctions.GammaLowerRegularized(x + 1, lambda);
}


module.exports=Poisson;