/**
 * Collection of functions equivalent to those provided by Microsoft Excel
 * but backed instead by Math.NET Numerics.
 * We do not recommend to use them except in an intermediate phase when
 * porting over solutions previously implemented in Excel.
 */
import { Normal } from './Distributions/Normal.js';
import { StudentT } from './Distributions/StudentT.js';
import { FisherSnedecor } from './Distributions/FisherSnedecor.js';
import { Beta } from './Distributions/Beta.js';
import { Gamma } from './Distributions/Gamma.js';
import { Exponential } from './Distributions/Exponential.js';
import { Hypergeometric } from './Distributions/Hypergeometric.js';
import { NegativeBinomial } from './Distributions/NegativeBinomial.js';
import { LogNormal } from './Distributions/LogNormal.js';
import { Binomial } from './Distributions/Binomial.js';
import { Poisson } from './Distributions/Poisson.js';
import { Weibull } from './Distributions/Weibull.js';
import { ArrayStatistics } from './Statistics/ArrayStatistics.js';
import { Statistics } from './Statistics/Statistics.js';
import { QuantileDefinition } from './Statistics/QuantileDefinition.js';
import { SpecialFunctions } from './SpecialFunctions/SpecialFunctions.js';

let ExcelFunctions = {
    /**
     * NormSDist
     * @param {number} z
     * @returns {number}
     */
    NormSDist: function(z) {
        return Normal.CDF(0, 1, z);
    },

    /**
     * NormSInv
     * @param {number} probability
     * @returns {number}
     */
    NormSInv: function(probability) {
        return Normal.InvCDF(0, 1, probability);
    },

    /**
     * NormDist
     * @param {number} x
     * @param {number} mean
     * @param {number} standardDev
     * @param {boolean} cumulative
     * @returns {number}
     */
    NormDist: function(x, mean, standardDev, cumulative) {
        return cumulative ? Normal.CDF(mean, standardDev, x) : Normal.PDF(mean, standardDev, x);
    },

    /**
     * NormInv
     * @param {number} probability
     * @param {number} mean
     * @param {number} standardDev
     * @returns {number}
     */
    NormInv: function(probability, mean, standardDev) {
        return Normal.InvCDF(mean, standardDev, probability);
    },

    /**
     * TDist
     * @param {number} x
     * @param {number} degreesFreedom
     * @param {number} tails
     * @returns {number}
     * @throws {Error}
     */
    TDist: function(x, degreesFreedom, tails) {
        switch (tails) {
            case 1:
                return 1 - StudentT.CDF(0, 1, degreesFreedom, x);

            case 2:
                return 1 - StudentT.CDF(0, 1, degreesFreedom, x) + StudentT.CDF(0, 1, degreesFreedom, -x);

            default:
                throw new Error('tails must be 1 or 2');
        }
    },

    /**
     * TInv
     * @param {number} probability
     * @param {number} degreesFreedom
     * @returns {number}
     */
    TInv: function(probability, degreesFreedom) {
        return -StudentT.InvCDF(0, 1, degreesFreedom, probability / 2);
    },

    /**
     * FDist
     * @param {number} x
     * @param {number} degreesFreedom1
     * @param {number} degreesFreedom2
     * @returns {number}
     */
    FDist: function(x, degreesFreedom1, degreesFreedom2) {
        return 1 - FisherSnedecor.CDF(degreesFreedom1, degreesFreedom2, x);
    },

    /**
     * FInv
     * @param {number} probability
     * @param {number} degreesFreedom1
     * @param {number} degreesFreedom2
     * @returns {number}
     */
    FInv: function(probability, degreesFreedom1, degreesFreedom2) {
        return FisherSnedecor.InvCDF(degreesFreedom1, degreesFreedom2, 1 - probability);
    },

    /**
     * BetaDist
     * @param {number} x
     * @param {number} alpha
     * @param {number} beta
     * @returns {number}
     */
    BetaDist: function(x, alpha, beta) {
        return Beta.CDF(alpha, beta, x);
    },

    /**
     * BetaInv
     * @param {number} probability
     * @param {number} alpha
     * @param {number} beta
     * @returns {number}
     */
    BetaInv: function(probability, alpha, beta) {
        return Beta.InvCDF(alpha, beta, probability);
    },

    /**
     * GammaDist
     * @param {number} x
     * @param {number} alpha
     * @param {number} beta
     * @param {boolean} cumulative
     * @returns {number}
     */
    GammaDist: function(x, alpha, beta, cumulative) {
        return cumulative ? Gamma.CDF(alpha, 1 / beta, x) : Gamma.PDF(alpha, 1 / beta, x);
    },

    /**
     * GammaInv
     * @param {number} probability
     * @param {number} alpha
     * @param {number} beta
     * @returns {number}
     */
    GammaInv: function(probability, alpha, beta) {
        return Gamma.InvCDF(alpha, 1 / beta, probability);
    },

    /**
     * Quartile
     * @param {Array<number>} array
     * @param {number} quant
     * @returns {number}
     * @throws {Error}
     */
    Quartile: function(array, quant) {
        switch (quant) {
            case 0:
                return ArrayStatistics.Minimum(array);

            case 1:
                return Statistics.QuantileCustom(array, 0.25, QuantileDefinition.R7);

            case 2:
                return Statistics.QuantileCustom(array, 0.5, QuantileDefinition.R7);

            case 3:
                return Statistics.QuantileCustom(array, 0.75, QuantileDefinition.R7);

            case 4:
                return ArrayStatistics.Maximum(array);

            default:
                throw new Error('quant must be between 0 and 4');
        }
    },

    /**
     * Percentile
     * @param {Array<number>} array
     * @param {number} k
     * @returns {number}
     */
    Percentile: function(array, k) {
        return Statistics.QuantileCustom(array, k, QuantileDefinition.R7);
    },

    /**
     * PercentRank
     * @param {Array<number>} array
     * @param {number} x
     * @returns {number}
     */
    PercentRank: function(array, x) {
        return Statistics.QuantileRank(array, x);
    },

    /**
     * GAMMALN
     * @param {number} z
     * @returns {number}
     */
    GAMMALN: function(z) {
        return SpecialFunctions.GammaLn(z);
    },

    /**
     * ExponDist
     * @param {number} x
     * @param {number} rate
     * @param {boolean} state
     * @returns {number}
     */
    ExponDist: function(x, rate, state) {
        if (state) {
            return Exponential.CDF(rate, x);
        }
        return Exponential.PDF(rate, x);
    },

    /**
     * HypgeomDist
     * @param {number} k
     * @param {number} draws
     * @param {number} success
     * @param {number} population
     * @returns {number}
     */
    HypgeomDist: function(k, draws, success, population) {
        return Hypergeometric.PMF(population, success, draws, k);
    },

    /**
     * NegbinomDist
     * @param {number} k
     * @param {number} r
     * @param {number} p
     * @returns {number}
     */
    NegbinomDist: function(k, r, p) {
        return NegativeBinomial.PMF(r, p, k);
    },

    /**
     * LognormDist
     * @param {number} x
     * @param {number} mu
     * @param {number} sigma
     * @returns {number}
     */
    LognormDist: function(x, mu, sigma) {
        return LogNormal.CDF(mu, sigma, x);
    },

    /**
     * LogInv
     * @param {number} p
     * @param {number} mu
     * @param {number} sigma
     * @returns {number}
     */
    LogInv: function(p, mu, sigma) {
        return LogNormal.InvCDF(mu, sigma, p);
    },

    /**
     * BinomDist
     * @param {number} k
     * @param {number} n
     * @param {number} p
     * @param {boolean} state
     * @returns {number}
     */
    BinomDist: function(k, n, p, state) {
        if (state === false) {
            return Binomial.PMF(p, n, k);
        }
        return Binomial.CDF(p, n, k);
    },

    /**
     * Poisson
     * @param {number} k
     * @param {number} lambda
     * @param {boolean} state
     * @returns {number}
     */
    Poisson: function(k, lambda, state) {
        if (state === false) {
            return Poisson.PMF(lambda, k);
        }
        return Poisson.CDF(lambda, k);
    },

    /**
     * Weibull
     * @param {number} x
     * @param {number} shape
     * @param {number} scale
     * @param {boolean} state
     * @returns {number}
     */
    Weibull: function(x, shape, scale, state) {
        if (state === false) {
            return Weibull.PDF(shape, scale, x);
        }
        return Weibull.CDF(shape, scale, x);
    }
};

export { ExcelFunctions };