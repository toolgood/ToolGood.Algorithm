import { ArrayStatistics } from './ArrayStatistics.js';
import { SortedArrayStatistics } from './SortedArrayStatistics.js';
import { QuantileDefinition } from './QuantileDefinition.js';

export var Statistics = {
    QuantileCustom: function(data, tau, definition) {
        var array = data.slice();
        return ArrayStatistics.QuantileCustomInplace(array, tau, definition);
    },

    QuantileRank: function(data, x) {
        var array = data.slice();
        array.sort(function(a, b) { return a - b; });
        return SortedArrayStatistics.QuantileRank(array, x);
    }
};
