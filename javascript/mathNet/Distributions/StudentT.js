const { SpecialFunctions } = require('./../SpecialFunctions');
const { Brent } = require('./../RootFinding/Brent');

class StudentT { }

StudentT.CDF = function (location, scale, freedom, x) {
    if (isFinite(freedom) == false) {
        return Normal.CDF(location, scale, x);
    }

    var k = (x - location) / scale;
    var h = freedom / (freedom + (k * k));
    var ib = 0.5 * SpecialFunctions.BetaRegularized(freedom / 2.0, 0.5, h);
    return x <= location ? ib : 1.0 - ib;
}
StudentT.InvCDF = function (location, scale, freedom, p) {

    if (isFinite(freedom) == false) {
        return Normal.InvCDF(location, scale, p);
    }

    if (p == 0.5) {
        return location;
    }
    return Brent.FindRoot(function (s) {
        var k = (x - location) / scale;
        var h = freedom / (freedom + (k * k));
        var ib = 0.5 * SpecialFunctions.BetaRegularized(freedom / 2.0, 0.5, h);
        return x <= location ? ib - p : 1.0 - ib - p;
    }, -800, 800, 1e-12);
}

module.exports = StudentT;