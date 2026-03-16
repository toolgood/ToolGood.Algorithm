/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
import standardEqualsFunction from "../utils/standardEqualsFunction.js";
import standardHashCodeFunction from "../utils/standardHashCodeFunction.js";

let DEFAULT_LOAD_FACTOR = 0.75;
let INITIAL_CAPACITY = 16

export default class HashMap {

    constructor(hashFunction, equalsFunction) {
        this.buckets = new Array(INITIAL_CAPACITY);
        this.threshold = Math.floor(INITIAL_CAPACITY * DEFAULT_LOAD_FACTOR);
        this.itemCount = 0;
        this.hashFunction = hashFunction || standardHashCodeFunction;
        this.equalsFunction = equalsFunction || standardEqualsFunction;
    }

    set(key, value) {
        this._expand();
        let slot = this._getSlot(key);
        let bucket = this.buckets[slot];
        if (!bucket) {
            bucket = [[key, value]];
            this.buckets[slot] = bucket;
            this.itemCount++;
            return value;
        }
        let existing = bucket.find(pair => this.equalsFunction(pair[0], key), this);
        if(existing) {
            let result = existing[1];
            existing[1] = value;
            return result;
        } else {
            bucket.push([key, value]);
            this.itemCount++;
            return value;
        }
    }

    containsKey(key) {
        let bucket = this._getBucket(key);
        if(!bucket) {
            return false;
        }
        let existing = bucket.find(pair => this.equalsFunction(pair[0], key), this);
        return !!existing;
    }

    get(key) {
        let bucket = this._getBucket(key);
        if(!bucket) {
            return null;
        }
        let existing = bucket.find(pair => this.equalsFunction(pair[0], key), this);
        return existing ? existing[1] : null;
    }

    entries() {
        return this.buckets.filter(b => b != null).flat(1);
    }

    getKeys() {
        return this.entries().map(pair => pair[0]);
    }

    getValues() {
        return this.entries().map(pair => pair[1]);
    }

    toString() {
        let ss = this.entries().map(e => '{' + e[0] + ':' + e[1] + '}');
        return '[' + ss.join(", ") + ']';
    }

    get length() {
        return this.itemCount;
    }

    _getSlot(key) {
        let hash = this.hashFunction(key);
        return hash & this.buckets.length - 1;
    }
    _getBucket(key) {
        return this.buckets[this._getSlot(key)];
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
            for (let pair of bucket) {
                let slot = this._getSlot(pair[0]);
                let newBucket = this.buckets[slot];
                if (!newBucket) {
                    newBucket = [];
                    this.buckets[slot] = newBucket;
                }
                newBucket.push(pair);
            }
        }
    }

}
