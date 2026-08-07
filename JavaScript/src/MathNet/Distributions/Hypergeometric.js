import { SpecialFunctions } from '../SpecialFunctions/SpecialFunctions.js';

class Hypergeometric {
    /**
     * Computes the probability mass (pmf) at k, i.e. P(X = k).
     * @param {number} population - The size of the population (N).
     * @param {number} successes - The number of successes in the population (K).
     * @param {number} draws - The number of draws (n).
     * @param {number} k - The number of successes in the draws.
     * @returns {number} The probability mass at k.
     */
    static pmf(population, successes, draws, k) {
        // Check if parameters are valid
        if (population < 0 || successes < 0 || draws < 0 || successes > population || draws > population) {
            return 0;
        }

        // Check domain of k
        if (k < 0 || k > draws || k > successes || k < draws + successes - population) {
            return 0;
        }

        // 对数域计算，避免大参数下组合数直接乘除溢出为 Infinity
        let logResult = SpecialFunctions.binomialLn(successes, k)
                      + SpecialFunctions.binomialLn(population - successes, draws - k)
                      - SpecialFunctions.binomialLn(population, draws);
        return Math.exp(logResult);
    }
}

export { Hypergeometric };