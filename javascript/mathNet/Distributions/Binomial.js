
class Binomial{}

Binomial.PMF=function(p, n, k) {
    if (k < 0 || k > n) { return 0.0; }
    if (p == 0.0) { return k == 0 ? 1.0 : 0.0; }
    if (p == 1.0) { return k == n ? 1.0 : 0.0; }
    return Math.exp(SpecialFunctions.BinomialLn(n, k) + (k * Math.log(p)) + ((n - k) * Math.log(1.0 - p)));
}

Binomial.CDF=function(p, n, x) {
    if (x < 0.0) { return 0.0; }
    if (x > n) { return 1.0; }
    var k = Math.floor(x);
    return SpecialFunctions.BetaRegularized(n - k, k + 1, 1 - p);
}


module.exports = Binomial;
