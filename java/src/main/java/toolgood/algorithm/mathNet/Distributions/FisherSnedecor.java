package toolgood.algorithm.mathNet.Distributions;



import toolgood.algorithm.mathNet.SpecialFunctions;
import toolgood.algorithm.mathNet.RootFinding.Brent;

public class FisherSnedecor {
    public static double CDF(double d1, double d2, double x)
    {
        //if (d1 <= 0.0 || d2 <= 0.0) {
        //    throw new ArgumentException(Resources.InvalidDistributionParameters);
        //}

        return SpecialFunctions.BetaRegularized(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2));
    }

    public static double InvCDF(double d1, double d2, double p) throws Exception
    {
        //if (d1 <= 0.0 || d2 <= 0.0) {
        //    throw new ArgumentException(Resources.InvalidDistributionParameters);
        //}
        Function<Double,Double> f= x->{
            return  SpecialFunctions.BetaRegularized(d1 / 2.0, d2 / 2.0, d1 * x / (d1 * x + d2)) - p;
        };

        return Brent.FindRoot(f, 0, 1000,  1e-12);
    }
}