

class Gamma{}


Gamma.CDF=function(shape, rate, x) {
    if (isFinite(rate)==false) { return x >= shape ? 1.0 : 0.0; }

    if (shape == 0.0 && rate == 0.0) { return 0.0; }

    return SpecialFunctions.GammaLowerRegularized(shape, x * rate);
}

Gamma.PDF=function(shape, rate, x) {
    if (isFinite(rate)==false) {
        return x == shape ? Double.POSITIVE_INFINITY : 0.0;
    }

    if (shape == 0.0 && rate == 0.0) {
        return 0.0;
    }

    if (shape == 1.0) {
        return rate * Math.exp(-rate * x);
    }

    if (shape > 160.0) {
        return Math.exp(PDFLn(shape, rate, x));
    }

    return Math.pow(rate, shape) * Math.pow(x, shape - 1.0) * Math.exp(-rate * x) / SpecialFunctions.Gamma(shape);
}
Gamma.PDFLn=function(shape, rate, x) {
    if (isFinite(rate)==false) {
        return x == shape ? Double.POSITIVE_INFINITY : Double.NEGATIVE_INFINITY;
    }

    if (shape == 0.0 && rate == 0.0) {
        return Double.NEGATIVE_INFINITY;
    }

    if (shape == 1.0) {
        return Math.log(rate) - (rate * x);
    }

    return (shape * Math.log(rate)) + ((shape - 1.0) * Math.log(x)) - (rate * x) - SpecialFunctions.GammaLn(shape);
}

Gamma.InvCDF=function(shape, rate, p) {
    return SpecialFunctions.GammaLowerRegularizedInv(shape, p) / rate;
}

module.exports = Gamma;
