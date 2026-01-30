class Evaluate {
    /**
     * Evaluates a polynomial at a given point using Horner's method.
     * @param {number} z - The point at which to evaluate the polynomial.
     * @param {Array<number>} coefficients - The coefficients of the polynomial, in order from the highest degree to the lowest.
     * @returns {number} The value of the polynomial at the given point.
     */
    static Polynomial(z, coefficients) {
        if (!coefficients || coefficients.length === 0) {
            return 0;
        }

        let sum = coefficients[0];
        for (let i = 1; i < coefficients.length; ++i) {
            sum *= z;
            sum += coefficients[i];
        }

        return sum;
    }

    /**
     * Evaluates a series using Kahan summation.
     * @param {Function} nextSummand - A function that returns the next term in the series.
     * @returns {number} The sum of the series.
     */
    static Series(nextSummand) {
        let compensation = 0;
        const factor = 1 << 16;

        let sum = nextSummand();
        let current;

        do {
            // Kahan Summation
            current = nextSummand();
            const y = current - compensation;
            const t = sum + y;
            compensation = t - sum;
            compensation -= y;
            sum = t;
        } while (Math.abs(sum) < Math.abs(factor * current));

        return sum;
    }
}

export { Evaluate };