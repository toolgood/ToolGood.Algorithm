

class Statistics { }


Statistics.QuantileCustom = function (data, tau, definition) {
    return ArrayStatistics.QuantileCustomInplace(data, tau, definition);
}
Statistics.QuantileRank = function (data, x) {
    var array = Statistics.ShellSort(data);
    return SortedArrayStatistics.QuantileRank(array, x);
}

Statistics.ShellSort = function (array) {
    var len = array.size();
    var temp;
    var gap = len / 2;
    while (gap > 0) {
        for (var i = gap; i < len; i++) {
            temp = array.get(i);
            var preIndex = i - gap;
            while (preIndex >= 0 && array.get(preIndex) > temp) {
                array.set(preIndex + gap, array.get(preIndex));
                // array[preIndex + gap] = array[preIndex];
                preIndex -= gap;
            }
            array.set(preIndex + gap, temp);
            // array[preIndex + gap] = temp;
        }
        gap /= 2;
    }
    return array;
}





module.exports = Statistics;