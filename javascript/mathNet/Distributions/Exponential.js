

class Exponential{}


Exponential.PDF=function(rate, x)
{
    return x < 0.0 ? 0.0 : rate * Math.exp(-rate * x);
}

Exponential.CDF=function(rate, x)
{
    return x < 0.0 ? 0.0 : 1.0 - Math.exp(-rate * x);
}
module.exports = Exponential;
