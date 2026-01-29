export var SortedArrayStatistics = {
    QuantileRank: function(data, x) {
        if (x < data[0]) {
            return 0.0;
        }

        if (x >= data[data.length - 1]) {
            return 1.0;
        }

        var right = this.binarySearch(data, x);
        if (right >= 0) {
            var left = right;

            while (left > 0 && data[left - 1] == data[left]) {
                left--;
            }

            while (right < data.length - 1 && data[right + 1] == data[right]) {
                right++;
            }

            return left / (data.length - 1);
        } else {
            right = ~right;
            var left = right - 1;

            var a = left / (data.length - 1);
            var b = right / (data.length - 1);
            return ((data[right] - x) * a + (x - data[left]) * b) / (data[right] - data[left]);
        }
    },

    binarySearch: function(arr, target) {
        var left = 0;
        var right = arr.length - 1;

        while (left <= right) {
            var mid = Math.floor((left + right) / 2);

            if (arr[mid] === target) {
                return mid;
            } else if (arr[mid] < target) {
                left = mid + 1;
            } else {
                right = mid - 1;
            }
        }

        return ~left;
    }
};
