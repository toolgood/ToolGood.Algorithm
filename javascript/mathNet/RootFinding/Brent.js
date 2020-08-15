const { Precision } = require('./../Precision');

class Brent { }

Brent.FindRoot = function (f, lowerBound, upperBound, accuracy, maxIterations) {
    var root = new RootNumber();
    if (TryFindRoot(f, lowerBound, upperBound, accuracy, maxIterations, root)) {
        return root.root;
    }
    throw new Exception("RootFindingFailed");
}

Brent.TryFindRoot = function (f, lowerBound, upperBound, accuracy, maxIterations, root) {
    var fmin = f.apply(lowerBound);
    var fmax = f.apply(upperBound);
    var froot = fmax;
    var d = 0.0, e = 0.0;

    root.root = upperBound;
    var xMid = Double.NaN;

    // Root must be bracketed.
    if (sign(fmin) == sign(fmax)) {
        return false;
    }

    for (var i = 0; i <= maxIterations; i++) {
        // adjust bounds
        if (sign(froot) == sign(fmax)) {
            upperBound = lowerBound;
            fmax = fmin;
            e = d = root.root - lowerBound;
        }

        if (Math.abs(fmax) < Math.abs(froot)) {
            lowerBound = root.root;
            root.root = upperBound;
            upperBound = lowerBound;
            fmin = froot;
            froot = fmax;
            fmax = fmin;
        }

        // convergence check
        var xAcc1 = Precision.PositiveDoublePrecision * Math.abs(root.root) + 0.5 * accuracy;
        var xMidOld = xMid;
        xMid = (upperBound - root.root) / 2.0;

        if (Math.abs(xMid) <= xAcc1 || Precision.AlmostEqualNormRelative(froot, 0, froot, accuracy)) {
            return true;
        }

        if (xMid == xMidOld) {
            // accuracy not sufficient, but cannot be improved further
            return false;
        }

        if (Math.abs(e) >= xAcc1 && Math.abs(fmin) > Math.abs(froot)) {
            // Attempt inverse quadratic interpolation
            var s = froot / fmin;
            var p;
            var q;
            if (Precision.AlmostEqualRelative(lowerBound, upperBound)) {
                p = 2.0 * xMid * s;
                q = 1.0 - s;
            } else {
                q = fmin / fmax;
                var r = froot / fmax;
                p = s * (2.0 * xMid * q * (q - r) - (root.root - lowerBound) * (r - 1.0));
                q = (q - 1.0) * (r - 1.0) * (s - 1.0);
            }

            if (p > 0.0) {
                // Check whether in bounds
                q = -q;
            }

            p = Math.abs(p);
            if (2.0 * p < Math.min(3.0 * xMid * q - Math.abs(xAcc1 * q), Math.abs(e * q))) {
                // Accept interpolation
                e = d;
                d = p / q;
            } else {
                // Interpolation failed, use bisection
                d = xMid;
                e = d;
            }
        } else {
            // Bounds decreasing too slowly, use bisection
            d = xMid;
            e = d;
        }

        lowerBound = root.root;
        fmin = froot;
        if (Math.abs(d) > xAcc1) {
            root.root += d;
        } else {
            root.root += Sign(xAcc1, xMid);
        }

        froot = f.apply(root.root);
    }

    return false;
}
sign = function (a) {
    if (a == 0.0) { return 0; }
    if (a < 0) { return -1; }
    return 1;
}

Sign = function (a, b) {
    return b >= 0 ? (a >= 0 ? a : -a) : (a >= 0 ? -a : a);
}

function RootNumber() {
    this.root = 0.0;
}


module.exports = Brent;