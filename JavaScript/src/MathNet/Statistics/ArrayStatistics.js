import { QuantileDefinition } from './QuantileDefinition.js';

export var ArrayStatistics = {
    Minimum: function(data) {
        if (data.length == 0) {
            return Number.NaN;
        }

        var min = Number.POSITIVE_INFINITY;
        for (var i = 0; i < data.length; i++) {
            if (data[i] < min || Number.isNaN(data[i])) {
                min = data[i];
            }
        }

        return min;
    },

    Maximum: function(data) {
        if (data.length == 0) {
            return Number.NaN;
        }

        var max = Number.NEGATIVE_INFINITY;
        for (var i = 0; i < data.length; i++) {
            if (data[i] > max || Number.isNaN(data[i])) {
                max = data[i];
            }
        }

        return max;
    },

    QuantileCustomInplace: function(data, tau, definition) {
        if (tau < 0 || tau > 1 || data.length == 0) {
            return Number.NaN;
        }

        if (tau == 0 || data.length == 1) {
            return this.Minimum(data);
        }

        if (tau == 1) {
            return this.Maximum(data);
        }

        switch (definition) {
            case QuantileDefinition.R1: {
                var h = data.length * tau + 0.5;
                return this.SelectInplace(data, Math.ceil(h - 0.5) - 1);
            }

            case QuantileDefinition.R2: {
                var h = data.length * tau + 0.5;
                return (this.SelectInplace(data, Math.ceil(h - 0.5) - 1) + 
                    this.SelectInplace(data, Math.floor(h + 0.5) - 1)) * 0.5;
            }

            case QuantileDefinition.R3: {
                var h = data.length * tau;
                return this.SelectInplace(data, Math.round(h) - 1);
            }

            case QuantileDefinition.R4: {
                var h = data.length * tau;
                var hf = Math.floor(h);
                var lower = this.SelectInplace(data, hf - 1);
                var upper = this.SelectInplace(data, hf);
                return lower + (h - hf) * (upper - lower);
            }

            case QuantileDefinition.R5: {
                var h = data.length * tau + 0.5;
                var hf = Math.floor(h);
                var lower = this.SelectInplace(data, hf - 1);
                var upper = this.SelectInplace(data, hf);
                return lower + (h - hf) * (upper - lower);
            }

            case QuantileDefinition.R6: {
                var h = (data.length + 1) * tau;
                var hf = Math.floor(h);
                var lower = this.SelectInplace(data, hf - 1);
                var upper = this.SelectInplace(data, hf);
                return lower + (h - hf) * (upper - lower);
            }

            case QuantileDefinition.R7: {
                var h = (data.length - 1) * tau + 1;
                var hf = Math.floor(h);
                var lower = this.SelectInplace(data, hf - 1);
                var upper = this.SelectInplace(data, hf);
                return lower + (h - hf) * (upper - lower);
            }

            case QuantileDefinition.R8: {
                var h = (data.length + 1 / 3) * tau + 1 / 3;
                var hf = Math.floor(h);
                var lower = this.SelectInplace(data, hf - 1);
                var upper = this.SelectInplace(data, hf);
                return lower + (h - hf) * (upper - lower);
            }

            case QuantileDefinition.R9: {
                var h = (data.length + 0.25) * tau + 0.375;
                var hf = Math.floor(h);
                var lower = this.SelectInplace(data, hf - 1);
                var upper = this.SelectInplace(data, hf);
                return lower + (h - hf) * (upper - lower);
            }

            default:
                throw new Error('Not supported');
        }
    },

    SelectInplace: function(workingData, rank) {
        if (rank <= 0) {
            return this.Minimum(workingData);
        }

        if (rank >= workingData.length - 1) {
            return this.Maximum(workingData);
        }

        var a = workingData;
        var low = 0;
        var high = a.length - 1;

        while (true) {
            if (high <= low + 1) {
                if (high == low + 1 && a[high] < a[low]) {
                    var tmp = a[low];
                    a[low] = a[high];
                    a[high] = tmp;
                }

                return a[rank];
            }

            var middle = (low + high) >> 1;

            var tmp1 = a[middle];
            a[middle] = a[low + 1];
            a[low + 1] = tmp1;

            if (a[low] > a[high]) {
                var tmp = a[low];
                a[low] = a[high];
                a[high] = tmp;
            }

            if (a[low + 1] > a[high]) {
                var tmp = a[low + 1];
                a[low + 1] = a[high];
                a[high] = tmp;
            }

            if (a[low] > a[low + 1]) {
                var tmp = a[low];
                a[low] = a[low + 1];
                a[low + 1] = tmp;
            }

            var begin = low + 1;
            var end = high;
            var pivot = a[begin];

            while (true) {
                do {
                    begin++;
                }
                while (a[begin] < pivot);

                do {
                    end--;
                }
                while (a[end] > pivot);

                if (end < begin) {
                    break;
                }

                var tmp = a[begin];
                a[begin] = a[end];
                a[end] = tmp;
            }

            a[low + 1] = a[end];
            a[end] = pivot;

            if (end >= rank) {
                high = end - 1;
            }

            if (end <= rank) {
                low = begin;
            }
        }
    }
};
