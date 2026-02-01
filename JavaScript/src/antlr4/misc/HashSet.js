/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
import standardHashCodeFunction from "../utils/standardHashCodeFunction.js";
import standardEqualsFunction from "../utils/standardEqualsFunction.js";
import arrayToString from "../utils/arrayToString.js";

let DEFAULT_LOAD_FACTOR = 0.75;
let INITIAL_CAPACITY = 16

export default class HashSet {

    constructor(hashFunction, equalsFunction) {
        this.buckets = new Array(INITIAL_CAPACITY);
        this.threshold = Math.floor(INITIAL_CAPACITY * DEFAULT_LOAD_FACTOR);
        this.itemCount = 0;
        this.hashFunction = hashFunction || standardHashCodeFunction;
        this.equalsFunction = equalsFunction || standardEqualsFunction;
    }

    get(value) {
        if(value == null) {
            return value;
        }
        let bucket = this._getBucket(value)
        if (!bucket) {
            return null;
        }
        for (let e of bucket) {
            if (this.equalsFunction(e, value)) {
                return e;
            }
        }
        return null;
    }

    add(value) {
        let existing = this.getOrAdd(value);
        return existing === value;
    }

    getOrAdd(value) {
        this._expand();
        let slot = this._getSlot(value);
        let bucket = this.buckets[slot];
        if (!bucket) {
            bucket = [value];
            this.buckets[slot] = bucket;
            this.itemCount++;
            return value;
        }
        for (let existing of bucket) {
            if (this.equalsFunction(existing, value)) {
                return existing;
            }
        }
        bucket.push(value);
        this.itemCount++;
        return value;

    }

    has(value) {
        return this.get(value) != null;
    }


    values() {
        return this.buckets.filter(b => b != null).flat(1);
    }

    toString() {
        return arrayToString(this.values());
    }

    get length() {
        return this.itemCount;
    }

    _getSlot(value) {
        let hash = this.hashFunction(value);
        return hash & this.buckets.length - 1;
    }
    _getBucket(value) {
        return this.buckets[this._getSlot(value)];
    }

    _expand() {
        if (this.itemCount <= this.threshold) {
            return;
        }
        let old_buckets = this.buckets;
        let newCapacity = this.buckets.length * 2;
        this.buckets = new Array(newCapacity);
        this.threshold = Math.floor(newCapacity * DEFAULT_LOAD_FACTOR);
        for (let bucket of old_buckets) {
            if (!bucket) {
                continue;
            }
            for (let o of bucket) {
                let slot = this._getSlot(o);
                let newBucket = this.buckets[slot];
                if (!newBucket) {
                    newBucket = [];
                    this.buckets[slot] = newBucket;
                }
                newBucket.push(o);
            }
        }

    }
}
