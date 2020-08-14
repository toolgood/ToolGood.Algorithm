

class Weibull{}


 
Weibull.PDF=function(shape, scale, x){

    if (x >= 0.0) {
        if (x == 0.0 && shape == 1.0) {
            return shape / scale;
        }

        return shape
               * Math.pow(x / scale, shape - 1.0)
               * Math.exp(-Math.pow(x, shape) * Math.pow(scale, -shape))
               / scale;
    }

    return 0.0;
}
         
Weibull.CDF=function(shape, scale, x){
    if (x < 0.0) {
        return 0.0;
    }
    return -SpecialFunctions.ExponentialMinusOne(-Math.pow(x, shape) * Math.pow(scale, -shape));
}




module.exports=Weibull;