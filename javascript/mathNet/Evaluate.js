

class Evaluate{}

Evaluate.Polynomial=function(z, coefficients){
    var sum = coefficients[coefficients.length - 1];
    for (var i = coefficients.length - 2; i >= 0; --i) {
        sum *= z;
        sum += coefficients[i];
    }
    return sum;
}

module.exports = Evaluate;
