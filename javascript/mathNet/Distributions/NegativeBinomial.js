

class NegativeBinomial{}


NegativeBinomial.PMF=function(r, p, k) {
    return Math.exp(PMFLn(r, p, k));
}

NegativeBinomial.PMFLn=function(r, p, k) {
    return SpecialFunctions.GammaLn(r + k)
    - SpecialFunctions.GammaLn(r)
    - SpecialFunctions.GammaLn(k + 1.0)
    + (r * Math.log(p))
    + (k * Math.log(1.0 - p));
}


module.exports=NegativeBinomial;