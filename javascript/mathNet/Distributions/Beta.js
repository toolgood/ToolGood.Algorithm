const { SpecialFunctions } = require('./../SpecialFunctions');
const { Brent } = require('./../RootFinding/Brent');

class Beta { }

Beta.CDF = function (a, b, x) {
    if (x < 0.0) { return 0.0; }
    if (x >= 1.0) { return 1.0; }
    if (isFinite(a) == false && isFinite(b) == false) { return x < 0.5 ? 0.0 : 1.0; }
    if (isFinite(a) == false) { return x < 1.0 ? 0.0 : 1.0; }
    if (isFinite(b) == false) { return x >= 0.0 ? 1.0 : 0.0; }
    if (a == 0.0 && b == 0.0) {
        if (x >= 0.0 && x < 1.0) { return 0.5; }
        return 1.0;
    }
    if (a == 0.0) { return 1.0; }
    if (b == 0.0) { return x >= 1.0 ? 1.0 : 0.0; }
    if (a == 1.0 && b == 1.0) { return x; }
    return SpecialFunctions.BetaRegularized(a, b, x);
}

Beta.InvCDF = function (a, b, p) {
    return Brent.FindRoot(function (x) {
        return SpecialFunctions.BetaRegularized(a, b, x) - p;
    }, 0.0, 1.0, 1e-12);
}

module.exports = Beta;
