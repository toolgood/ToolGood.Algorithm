

class SortedArrayStatistics{}

SortedArrayStatistics.QuantileRank=function( data, x) {
    if (x < data[0]) {
        return 0.0;
    }

    if (x >= data[data.length - 1]) {
        return 1.0;
    }

    var right = binarySearch(data, x);
    if (right >= 0) {
        var left = right;

        while (left > 0 && data[left - 1] == data[left]) {
            left--;
        }

        while (right < data.length - 1 && data[right + 1] == data[right]) {
            right++;
        }

        return left /  (data.length - 1);

    } else {
        right = ~right;
        var left = right - 1;

        var a = left /  (data.length - 1);
        var b = right /   (data.length - 1);
        return ((data[right] - x) * a + (x - data[left]) * b) / (data[right] - data[left]);
    }
}
SortedArrayStatistics.binarySearch=function(array, key) {
    var low = 0;
    var high = array.length - 1;
    while (low <= high) {
        var mid = (low + high) >>> 1;
        if (key < array[mid]) {
            high = mid - 1;
        } else if (key > array[mid]) {
            low = mid + 1;
        } else {
            return mid;
        }
    }
    return -1;
}



module.exports=SortedArrayStatistics;