import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class Hypergeometric {
    /**
     * Computes the probability mass (PMF) at k, i.e. P(X = k).
     * @param {number} population - The size of the population (N).
     * @param {number} successes - The number of successes in the population (K).
     * @param {number} draws - The number of draws (n).
     * @param {number} k - The number of successes in the draws.
     * @returns {number} The probability mass at k.
     */
    static PMF(population, successes, draws, k) {
        // Check if parameters are valid
        if (population < 0 || successes < 0 || draws < 0 || successes > population || draws > population) {
            return 0;
        }

        // Calculate the probability mass function
        return SpecialFunctions.Binomial(successes, k) * SpecialFunctions.Binomial(population - successes, draws - k) / SpecialFunctions.Binomial(population, draws);
    }
}

export { Hypergeometric };