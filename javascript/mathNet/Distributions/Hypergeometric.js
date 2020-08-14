

class Hypergeometric{}


Hypergeometric.PMF=function(population, success, draws, k){
    return SpecialFunctions.Binomial(success, k) * SpecialFunctions.Binomial(population - success, draws - k) / SpecialFunctions.Binomial(population, draws);
}


module.exports=Hypergeometric;