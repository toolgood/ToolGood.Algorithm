import { ArrayStatistics } from './ArrayStatistics.js';
import { SortedArrayStatistics } from './SortedArrayStatistics.js';

export var Statistics = {
    quantileCustom: function(data, tau, definition) {
        var array = data.slice();
        return ArrayStatistics.quantileCustomInplace(array, tau, definition);
    },

    quantileRank: function(data, x) {
        var array = data.slice();
        array.sort(function(a, b) { return a - b; });
        return SortedArrayStatistics.quantileRank(array, x);
    }
};
