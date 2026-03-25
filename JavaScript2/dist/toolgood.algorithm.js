/*!
 * # ToolGood Noncommercial License 1.0.0
 * 
 * ## Acceptance
 * 
 * In order to get any license under these terms, you must agree
 * to them as both strict obligations and conditions to all
 * your licenses.
 * 
 * ## Copyright License
 * 
 * The licensor grants you a copyright license for the
 * software to do everything you might do with the software
 * that would otherwise infringe the licensor's copyright
 * in it for any permitted purpose.  However, you may
 * only distribute the software according to [Distribution
 * License](#distribution-license) and make changes or new works
 * based on the software according to [Changes and New Works
 * License](#changes-and-new-works-license).
 * 
 * ## Distribution License
 * 
 * The licensor grants you an additional copyright license
 * to distribute copies of the software.  Your license
 * to distribute covers distributing the software with
 * changes and new works permitted by [Changes and New Works
 * License](#changes-and-new-works-license).
 * 
 * ## Notices
 * 
 * You must ensure that anyone who gets a copy of any part of
 * the software from you also gets a copy of these terms or the
 * URL for them above, as well as copies of any plain-text lines
 * beginning with `Required Notice:` that the licensor provided
 * with the software.  For example:
 * 
 * > Required Notice: Copyright ToolGood, Inc. (https://ToolGood.com)
 * 
 * ## Changes and New Works License
 * 
 * The licensor grants you an additional copyright license to
 * make changes and new works based on the software for any
 * permitted purpose.
 * 
 * ## Patent License
 * 
 * The licensor grants you a patent license for the software that
 * covers patent claims the licensor can license, or becomes able
 * to license, that you would infringe by using the software.
 * 
 * ## Noncommercial Purposes
 * 
 * Any noncommercial purpose is a permitted purpose.
 * 
 * ## Personal Uses
 * 
 * Personal use for research, experiment, and testing for
 * the benefit of public knowledge, personal study, private
 * entertainment, hobby projects, amateur pursuits, or religious
 * observance, without any anticipated commercial application,
 * is use for a permitted purpose.
 * 
 * ## Noncommercial Organizations
 * 
 * Use by any charitable organization, educational institution,
 * public research organization, public safety or health
 * organization, environmental protection organization,
 * or government institution is use for a permitted purpose
 * regardless of the source of funding or obligations resulting
 * from the funding.
 * 
 * ## Fair Use
 * 
 * You may have "fair use" rights for the software under the
 * law. These terms do not limit them.
 * 
 * ## No Other Rights
 * 
 * These terms do not allow you to sublicense or transfer any of
 * your licenses to anyone else, or prevent the licensor from
 * granting licenses to anyone else.  These terms do not imply
 * any other licenses.
 * 
 * ## Patent Defense
 * 
 * If you make any written claim that the software infringes or
 * contributes to infringement of any patent, your patent license
 * for the software granted under these terms ends immediately. If
 * your company makes such a claim, your patent license ends
 * immediately for work on behalf of your company.
 * 
 * ## Violations
 * 
 * The first time you are notified in writing that you have
 * violated any of these terms, or done anything with the software
 * not covered by your licenses, your licenses can nonetheless
 * continue if you come into full compliance with these terms,
 * and take practical steps to correct past violations, within
 * 32 days of receiving notice.  Otherwise, all your licenses
 * end immediately.
 * 
 * ## No Liability
 * 
 * ***As far as the law allows, the software comes as is, without
 * any warranty or condition, and the licensor will not be liable
 * to you for any damages arising out of these terms or the use
 * or nature of the software, under any kind of legal claim.***
 * 
 * ## Definitions
 * 
 * The **licensor** is the individual or entity offering these
 * terms, and the **software** is the software the licensor makes
 * available under these terms.
 * 
 * **You** refers to the individual or entity agreeing to these
 * terms.
 * 
 * **Your company** is any legal entity, sole proprietorship,
 * or other kind of organization that you work for, plus all
 * organizations that have control over, are under the control of,
 * or are under common control with that organization.  **Control**
 * means ownership of substantially all the assets of an entity,
 * or the power to direct its management and policies by vote,
 * contract, or otherwise.  Control can be direct or indirect.
 * 
 * **Your licenses** are all the licenses granted to you for the
 * software under these terms.
 * 
 * **Use** means anything you do with the software requiring one
 * of your licenses.
 */
(function webpackUniversalModuleDefinition(root, factory) {
	if(typeof exports === 'object' && typeof module === 'object')
		module.exports = factory();
	else if(typeof define === 'function' && define.amd)
		define([], factory);
	else if(typeof exports === 'object')
		exports["ToolGood.Algorithm"] = factory();
	else
		root["ToolGood.Algorithm"] = factory();
})(this, () => {
return /******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	// The require scope
/******/ 	var __webpack_require__ = {};
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};

// EXPORTS
__webpack_require__.d(__webpack_exports__, {
  "default": () => (/* binding */ src)
});

// UNUSED EXPORTS: AlgorithmEngineHelper

;// ./src/CharUtil.js
/**
 * CharUtil
 */
class CharUtil {
  /**
   * Standardizes a character by converting it to uppercase and handling special characters.
   * @param {string} o - The character to standardize.
   * @returns {string} The standardized character.
   */
  static standardChar(o) {
    if (typeof o !== 'string' || o.length !== 1) return o;
    let charCode = o.charCodeAt(0);
    if (charCode >= 65 && charCode <= 90) return o;
    if (charCode <= 127) return o.toUpperCase();
    if (charCode <= 65280) {
      if (o == '×') return '*'; //215
      if (o == '÷') return '/'; //247
      if (o == '‘') return '\''; //8216
      if (o == '’') return '\''; //8217
      if (o == '"') return '"'; //8220
      if (o == '"') return '"'; //8221
      if (charCode == 12288) return String.fromCharCode(32);
      if (o == '【') return '['; //12304
      if (o == '】') return ']'; //12305
      if (o == '〔') return '('; //12308
      if (o == '〕') return ')'; //12309
      return o;
    } else if (charCode < 65375) {
      o = String.fromCharCode(charCode - 65248);
    }
    return o.toUpperCase();
  }

  /**
   * Standardizes a string by converting each character to uppercase and handling special characters.
   * @param {string} s - The string to standardize.
   * @returns {string} The standardized string.
   */
  static standardString(s) {
    var sb = [];
    for (var i = 0; i < s.length; i++) {
      sb.push(this.standardChar(s[i]));
    }
    return sb.join('');
  }

  /**
   * Compares a string with a character for equality after standardization.
   * @param {string} left - The string to compare.
   * @param {string} right - The character to compare with.
   * @returns {boolean} True if the string and character are equal after standardization, false otherwise.
   */
  static equals(left, right) {
    if (left.length != right.length) return false;
    for (var i = 0; i < left.length; i++) {
      if (left[i] != right[i]) {
        var a = this.standardChar(left[i]);
        if (a != right[i]) return false;
      }
    }
    return true;
  }

  /**
   * Compares a string with two other strings for equality after standardization.
   * @param {string} left - The string to compare.
   * @param {string} arg1 - The first string to compare with.
   * @param {string} arg2 - The second string to compare with.
   * @returns {boolean} True if the string is equal to either of the other strings after standardization, false otherwise.
   */
  static equals3(left, arg1, arg2) {
    if (this.equals(left, arg1)) return true;
    if (this.equals(left, arg2)) return true;
    return false;
  }

  /**
   * Compares a string with three other strings for equality after standardization.
   * @param {string} left - The string to compare.
   * @param {string} arg1 - The first string to compare with.
   * @param {string} arg2 - The second string to compare with.
   * @param {string} arg3 - The third string to compare with.
   * @returns {boolean} True if the string is equal to any of the other strings after standardization, false otherwise.
   */
  static equals4(left, arg1, arg2, arg3) {
    if (this.equals(left, arg1)) return true;
    if (this.equals(left, arg2)) return true;
    if (this.equals(left, arg3)) return true;
    return false;
  }
}
;// ./src/antlr4/Token.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

/**
 * A token has properties: text, type, line, character position in the line
 * (so we can ignore tabs), token channel, index, and source from which
 * we obtained this token.
 */
class Token {
  constructor() {
    this.source = null;
    this.type = null; // token type of the token
    this.channel = null; // The parser ignores everything not on DEFAULT_CHANNEL
    this.start = null; // optional; return -1 if FIXME.
    this.stop = null; // optional; return -1 if FIXME.
    this.tokenIndex = null; // from 0..n-1 of the token object in the input stream
    this.line = null; // line=1..n of the 1st character
    this.column = null; // beginning of the line at which it occurs, 0..n-1
    this._text = null; // text of the token.
  }
  getTokenSource() {
    return this.source[0];
  }
  getInputStream() {
    return this.source[1];
  }
  get text() {
    return this._text;
  }
  set text(text) {
    this._text = text;
  }
}
Token.INVALID_TYPE = 0;

/**
 * During lookahead operations, this "token" signifies we hit rule end ATN state
 * and did not follow it despite needing to.
 */
Token.EPSILON = -2;
Token.MIN_USER_TOKEN_TYPE = 1;
Token.EOF = -1;

/**
 * All tokens go to the parser (unless skip() is called in that rule)
 * on a particular "channel". The parser tunes to a particular channel
 * so that whitespace etc... can go to the parser on a "hidden" channel.
 */
Token.DEFAULT_CHANNEL = 0;

/**
 * Anything on different channel than DEFAULT_CHANNEL is not parsed
 * by parser.
 */
Token.HIDDEN_CHANNEL = 1;
;// ./src/antlr4/CharStream.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * If decodeToUnicodeCodePoints is true, the input is treated
 * as a series of Unicode code points.
 *
 * Otherwise, the input is treated as a series of 16-bit UTF-16 code
 * units.
 */
class CharStream {
  constructor(data, decodeToUnicodeCodePoints) {
    this.name = "<empty>";
    this.strdata = data;
    this.decodeToUnicodeCodePoints = decodeToUnicodeCodePoints || false;
    // _loadString - Vacuum all input from a string and then treat it like a buffer.
    this._index = 0;
    this.data = [];
    if (this.decodeToUnicodeCodePoints) {
      for (let i = 0; i < this.strdata.length;) {
        let codePoint = this.strdata.codePointAt(i);
        this.data.push(codePoint);
        i += codePoint <= 0xFFFF ? 1 : 2;
      }
    } else {
      this.data = new Array(this.strdata.length);
      for (let i = 0; i < this.strdata.length; i++) {
        this.data[i] = this.strdata.charCodeAt(i);
      }
    }
    this._size = this.data.length;
  }

  /**
   * Reset the stream so that it's in the same state it was
   * when the object was created *except* the data array is not
   * touched.
   */
  reset() {
    this._index = 0;
  }
  consume() {
    if (this._index >= this._size) {
      // assert this.LA(1) == Token.EOF
      throw "cannot consume EOF";
    }
    this._index += 1;
  }
  LA(offset) {
    if (offset === 0) {
      return 0; // undefined
    }
    if (offset < 0) {
      offset += 1; // e.g., translate LA(-1) to use offset=0
    }
    let pos = this._index + offset - 1;
    if (pos < 0 || pos >= this._size) {
      // invalid
      return Token.EOF;
    }
    return this.data[pos];
  }
  LT(offset) {
    return this.LA(offset);
  }

  // mark/release do nothing; we have entire buffer
  mark() {
    return -1;
  }
  release(marker) {}

  /**
   * consume() ahead until p==_index; can't just set p=_index as we must
   * update line and column. If we seek backwards, just set p
   */
  seek(_index) {
    if (_index <= this._index) {
      this._index = _index; // just jump; don't update stream state (line,
      // ...)
      return;
    }
    // seek forward
    this._index = Math.min(_index, this._size);
  }
  getText(start, stop) {
    if (stop >= this._size) {
      stop = this._size - 1;
    }
    if (start >= this._size) {
      return "";
    } else {
      if (this.decodeToUnicodeCodePoints) {
        let result = "";
        for (let i = start; i <= stop; i++) {
          result += String.fromCodePoint(this.data[i]);
        }
        return result;
      } else {
        return this.strdata.slice(start, stop + 1);
      }
    }
  }
  toString() {
    return this.strdata;
  }
  get index() {
    return this._index;
  }
  get size() {
    return this._size;
  }
}
;// ./src/AntlrCharStream.js




/**
 * This class supports case-insensitive lexing by wrapping an existing
 * char stream and forcing the lexer to see either upper or
 * lowercase characters. Grammar literals should then be either upper or
 * lower case such as 'BEGIN' or 'begin'. The text of the character
 * stream is unaffected. Example: input 'BeGiN' would match lexer rule
 * 'BEGIN' if constructor parameter upper=true but getText() would return
 * 'BeGiN'.
 */
class AntlrCharStream extends CharStream {
  constructor(data, decodeToUnicodeCodePoints) {
    super(data, decodeToUnicodeCodePoints);
  }
  LA(offset) {
    if (offset === 0) {
      return 0; // undefined
    }
    if (offset < 0) {
      offset += 1; // e.g., translate LA(-1) to use offset=0
    }
    let pos = this._index + offset - 1;
    if (pos < 0 || pos >= this._size) {
      // invalid
      return Token.EOF;
    }
    let c = this.data[pos];
    return CharUtil.standardChar(String.fromCharCode(c)).charCodeAt(0);
  }
}
;// ./src/antlr4/utils/equalArrays.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
function equalArrays(a, b) {
  if (!Array.isArray(a) || !Array.isArray(b)) return false;
  if (a === b) return true;
  if (a.length !== b.length) return false;
  for (let i = 0; i < a.length; i++) {
    if (a[i] === b[i]) continue;
    if (!a[i].equals || !a[i].equals(b[i])) return false;
  }
  return true;
}
;// ./src/antlr4/utils/stringHashCode.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

let StringSeedHashCode = Math.round(Math.random() * Math.pow(2, 32));
function stringHashCode(value) {
  if (!value) {
    return 0;
  }
  let type = typeof value;
  let key = type === 'string' ? value : type === 'object' && value.toString ? value.toString() : false;
  if (!key) {
    return 0;
  }
  let h1b, k1;
  let remainder = key.length & 3; // key.length % 4
  let bytes = key.length - remainder;
  let h1 = StringSeedHashCode;
  let c1 = 0xcc9e2d51;
  let c2 = 0x1b873593;
  let i = 0;
  while (i < bytes) {
    k1 = key.charCodeAt(i) & 0xff | (key.charCodeAt(++i) & 0xff) << 8 | (key.charCodeAt(++i) & 0xff) << 16 | (key.charCodeAt(++i) & 0xff) << 24;
    ++i;
    k1 = (k1 & 0xffff) * c1 + (((k1 >>> 16) * c1 & 0xffff) << 16) & 0xffffffff;
    k1 = k1 << 15 | k1 >>> 17;
    k1 = (k1 & 0xffff) * c2 + (((k1 >>> 16) * c2 & 0xffff) << 16) & 0xffffffff;
    h1 ^= k1;
    h1 = h1 << 13 | h1 >>> 19;
    h1b = (h1 & 0xffff) * 5 + (((h1 >>> 16) * 5 & 0xffff) << 16) & 0xffffffff;
    h1 = (h1b & 0xffff) + 0x6b64 + (((h1b >>> 16) + 0xe654 & 0xffff) << 16);
  }
  k1 = 0;
  switch (remainder) {
    case 3:
      k1 ^= (key.charCodeAt(i + 2) & 0xff) << 16;
    // no-break
    case 2:
      k1 ^= (key.charCodeAt(i + 1) & 0xff) << 8;
    // no-break
    case 1:
      k1 ^= key.charCodeAt(i) & 0xff;
      k1 = (k1 & 0xffff) * c1 + (((k1 >>> 16) * c1 & 0xffff) << 16) & 0xffffffff;
      k1 = k1 << 15 | k1 >>> 17;
      k1 = (k1 & 0xffff) * c2 + (((k1 >>> 16) * c2 & 0xffff) << 16) & 0xffffffff;
      h1 ^= k1;
  }
  h1 ^= key.length;
  h1 ^= h1 >>> 16;
  h1 = (h1 & 0xffff) * 0x85ebca6b + (((h1 >>> 16) * 0x85ebca6b & 0xffff) << 16) & 0xffffffff;
  h1 ^= h1 >>> 13;
  h1 = (h1 & 0xffff) * 0xc2b2ae35 + (((h1 >>> 16) * 0xc2b2ae35 & 0xffff) << 16) & 0xffffffff;
  h1 ^= h1 >>> 16;
  return h1 >>> 0;
}
;// ./src/antlr4/misc/HashCode.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class HashCode {
  constructor() {
    this.count = 0;
    this.hash = 0;
  }
  update() {
    for (let i = 0; i < arguments.length; i++) {
      let value = arguments[i];
      if (value == null) continue;
      if (Array.isArray(value)) this.update.apply(this, value);else {
        let k = 0;
        switch (typeof value) {
          case 'undefined':
          case 'function':
            continue;
          case 'number':
          case 'boolean':
            k = value;
            break;
          case 'string':
            k = stringHashCode(value);
            break;
          default:
            if (value.updateHashCode) value.updateHashCode(this);else console.log("No updateHashCode for " + value.toString());
            continue;
        }
        k = k * 0xCC9E2D51;
        k = k << 15 | k >>> 32 - 15;
        k = k * 0x1B873593;
        this.count = this.count + 1;
        let hash = this.hash ^ k;
        hash = hash << 13 | hash >>> 32 - 13;
        hash = hash * 5 + 0xE6546B64;
        this.hash = hash;
      }
    }
  }
  finish() {
    let hash = this.hash ^ this.count * 4;
    hash = hash ^ hash >>> 16;
    hash = hash * 0x85EBCA6B;
    hash = hash ^ hash >>> 13;
    hash = hash * 0xC2B2AE35;
    hash = hash ^ hash >>> 16;
    return hash;
  }
  static hashStuff() {
    let hash = new HashCode();
    hash.update.apply(hash, arguments);
    return hash.finish();
  }
}
;// ./src/antlr4/utils/standardHashCodeFunction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

function standardHashCodeFunction(a) {
  return a ? typeof a === 'string' ? stringHashCode(a) : a.hashCode() : -1;
}
;// ./src/antlr4/utils/standardEqualsFunction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
function standardEqualsFunction(a, b) {
  return a && a.equals ? a.equals(b) : a === b;
}
;// ./src/antlr4/utils/valueToString.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
function valueToString(v) {
  return v === null ? "null" : v;
}
;// ./src/antlr4/utils/arrayToString.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

function arrayToString(a) {
  return Array.isArray(a) ? "[" + a.map(valueToString).join(", ") + "]" : "null";
}
;// ./src/antlr4/misc/HashSet.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



let DEFAULT_LOAD_FACTOR = 0.75;
let INITIAL_CAPACITY = 16;
class HashSet {
  constructor(hashFunction, equalsFunction) {
    this.buckets = new Array(INITIAL_CAPACITY);
    this.threshold = Math.floor(INITIAL_CAPACITY * DEFAULT_LOAD_FACTOR);
    this.itemCount = 0;
    this.hashFunction = hashFunction || standardHashCodeFunction;
    this.equalsFunction = equalsFunction || standardEqualsFunction;
  }
  get(value) {
    if (value == null) {
      return value;
    }
    let bucket = this._getBucket(value);
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
;// ./src/antlr4/atn/SemanticContext.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */





/**
 * A tree structure used to record the semantic context in which
 * an ATN configuration is valid.  It's either a single predicate,
 * a conjunction {@code p1&&p2}, or a sum of products {@code p1||p2}.
 *
 * <p>I have scoped the {@link AND}, {@link OR}, and {@link Predicate} subclasses of
 * {@link SemanticContext} within the scope of this outer class.</p>
 */
class SemanticContext {
  hashCode() {
    let hash = new HashCode();
    this.updateHashCode(hash);
    return hash.finish();
  }

  /**
   * For context independent predicates, we evaluate them without a local
   * context (i.e., null context). That way, we can evaluate them without
   * having to create proper rule-specific context during prediction (as
   * opposed to the parser, which creates them naturally). In a practical
   * sense, this avoids a cast exception from RuleContext to myruleContext.
   *
   * <p>For context dependent predicates, we must pass in a local context so that
   * references such as $arg evaluate properly as _localctx.arg. We only
   * capture context dependent predicates in the context in which we begin
   * prediction, so we passed in the outer context here in case of context
   * dependent predicate evaluation.</p>
   */
  evaluate(parser, outerContext) {}

  /**
   * Evaluate the precedence predicates for the context and reduce the result.
   *
   * @param parser The parser instance.
   * @param outerContext The current parser context object.
   * @return The simplified semantic context after precedence predicates are
   * evaluated, which will be one of the following values.
   * <ul>
   * <li>{@link //NONE}: if the predicate simplifies to {@code true} after
   * precedence predicates are evaluated.</li>
   * <li>{@code null}: if the predicate simplifies to {@code false} after
   * precedence predicates are evaluated.</li>
   * <li>{@code this}: if the semantic context is not changed as a result of
   * precedence predicate evaluation.</li>
   * <li>A non-{@code null} {@link SemanticContext}: the new simplified
   * semantic context after precedence predicates are evaluated.</li>
   * </ul>
   */
  evalPrecedence(parser, outerContext) {
    return this;
  }
  static andContext(a, b) {
    if (a === null || a === SemanticContext.NONE) {
      return b;
    }
    if (b === null || b === SemanticContext.NONE) {
      return a;
    }
    let result = new AND(a, b);
    if (result.opnds.length === 1) {
      return result.opnds[0];
    } else {
      return result;
    }
  }
  static orContext(a, b) {
    if (a === null) {
      return b;
    }
    if (b === null) {
      return a;
    }
    if (a === SemanticContext.NONE || b === SemanticContext.NONE) {
      return SemanticContext.NONE;
    }
    let result = new OR(a, b);
    if (result.opnds.length === 1) {
      return result.opnds[0];
    } else {
      return result;
    }
  }
}
class AND extends SemanticContext {
  /**
   * A semantic context which is true whenever none of the contained contexts
   * is false
   */
  constructor(a, b) {
    super();
    let operands = new HashSet();
    if (a instanceof AND) {
      a.opnds.map(function (o) {
        operands.add(o);
      });
    } else {
      operands.add(a);
    }
    if (b instanceof AND) {
      b.opnds.map(function (o) {
        operands.add(o);
      });
    } else {
      operands.add(b);
    }
    let precedencePredicates = filterPrecedencePredicates(operands);
    if (precedencePredicates.length > 0) {
      // interested in the transition with the lowest precedence
      let reduced = null;
      precedencePredicates.map(function (p) {
        if (reduced === null || p.precedence < reduced.precedence) {
          reduced = p;
        }
      });
      operands.add(reduced);
    }
    this.opnds = Array.from(operands.values());
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof AND)) {
      return false;
    } else {
      return equalArrays(this.opnds, other.opnds);
    }
  }
  updateHashCode(hash) {
    hash.update(this.opnds, "AND");
  }

  /**
   * {@inheritDoc}
   *
   * <p>
   * The evaluation of predicates by this context is short-circuiting, but
   * unordered.</p>
   */
  evaluate(parser, outerContext) {
    for (let i = 0; i < this.opnds.length; i++) {
      if (!this.opnds[i].evaluate(parser, outerContext)) {
        return false;
      }
    }
    return true;
  }
  evalPrecedence(parser, outerContext) {
    let differs = false;
    let operands = [];
    for (let i = 0; i < this.opnds.length; i++) {
      let context = this.opnds[i];
      let evaluated = context.evalPrecedence(parser, outerContext);
      differs |= evaluated !== context;
      if (evaluated === null) {
        // The AND context is false if any element is false
        return null;
      } else if (evaluated !== SemanticContext.NONE) {
        // Reduce the result by skipping true elements
        operands.push(evaluated);
      }
    }
    if (!differs) {
      return this;
    }
    if (operands.length === 0) {
      // all elements were true, so the AND context is true
      return SemanticContext.NONE;
    }
    let result = null;
    operands.map(function (o) {
      result = result === null ? o : SemanticContext.andContext(result, o);
    });
    return result;
  }
  toString() {
    let s = this.opnds.map(o => o.toString());
    return (s.length > 3 ? s.slice(3) : s).join("&&");
  }
}
class OR extends SemanticContext {
  /**
   * A semantic context which is true whenever at least one of the contained
   * contexts is true
   */
  constructor(a, b) {
    super();
    let operands = new HashSet();
    if (a instanceof OR) {
      a.opnds.map(function (o) {
        operands.add(o);
      });
    } else {
      operands.add(a);
    }
    if (b instanceof OR) {
      b.opnds.map(function (o) {
        operands.add(o);
      });
    } else {
      operands.add(b);
    }
    let precedencePredicates = filterPrecedencePredicates(operands);
    if (precedencePredicates.length > 0) {
      // interested in the transition with the highest precedence
      let s = precedencePredicates.sort(function (a, b) {
        return a.compareTo(b);
      });
      let reduced = s[s.length - 1];
      operands.add(reduced);
    }
    this.opnds = Array.from(operands.values());
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof OR)) {
      return false;
    } else {
      return equalArrays(this.opnds, other.opnds);
    }
  }
  updateHashCode(hash) {
    hash.update(this.opnds, "OR");
  }

  /**
   * <p>
   * The evaluation of predicates by this context is short-circuiting, but
   * unordered.</p>
   */
  evaluate(parser, outerContext) {
    for (let i = 0; i < this.opnds.length; i++) {
      if (this.opnds[i].evaluate(parser, outerContext)) {
        return true;
      }
    }
    return false;
  }
  evalPrecedence(parser, outerContext) {
    let differs = false;
    let operands = [];
    for (let i = 0; i < this.opnds.length; i++) {
      let context = this.opnds[i];
      let evaluated = context.evalPrecedence(parser, outerContext);
      differs |= evaluated !== context;
      if (evaluated === SemanticContext.NONE) {
        // The OR context is true if any element is true
        return SemanticContext.NONE;
      } else if (evaluated !== null) {
        // Reduce the result by skipping false elements
        operands.push(evaluated);
      }
    }
    if (!differs) {
      return this;
    }
    if (operands.length === 0) {
      // all elements were false, so the OR context is false
      return null;
    }
    let result = null;
    operands.map(function (o) {
      return result === null ? o : SemanticContext.orContext(result, o);
    });
    return result;
  }
  toString() {
    let s = this.opnds.map(o => o.toString());
    return (s.length > 3 ? s.slice(3) : s).join("||");
  }
}
function filterPrecedencePredicates(set) {
  let result = [];
  set.values().map(function (context) {
    if (context instanceof SemanticContext.PrecedencePredicate) {
      result.push(context);
    }
  });
  return result;
}
;// ./src/antlr4/atn/ATNConfig.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



function checkParams(params, isCfg) {
  if (params === null) {
    let result = {
      state: null,
      alt: null,
      context: null,
      semanticContext: null
    };
    if (isCfg) {
      result.reachesIntoOuterContext = 0;
    }
    return result;
  } else {
    let props = {};
    props.state = params.state || null;
    props.alt = params.alt === undefined ? null : params.alt;
    props.context = params.context || null;
    props.semanticContext = params.semanticContext || null;
    if (isCfg) {
      props.reachesIntoOuterContext = params.reachesIntoOuterContext || 0;
      props.precedenceFilterSuppressed = params.precedenceFilterSuppressed || false;
    }
    return props;
  }
}
class ATNConfig {
  /**
   * @param {Object} params A tuple: (ATN state, predicted alt, syntactic, semantic context).
   * The syntactic context is a graph-structured stack node whose
   * path(s) to the root is the rule invocation(s)
   * chain used to arrive at the state.  The semantic context is
   * the tree of semantic predicates encountered before reaching
   * an ATN state
   */
  constructor(params, config) {
    this.checkContext(params, config);
    params = checkParams(params);
    config = checkParams(config, true);
    // The ATN state associated with this configuration///
    this.state = params.state !== null ? params.state : config.state;
    // What alt (or lexer rule) is predicted by this configuration///
    this.alt = params.alt !== null ? params.alt : config.alt;
    /**
     * The stack of invoking states leading to the rule/states associated
     * with this config.  We track only those contexts pushed during
     * execution of the ATN simulator
     */
    this.context = params.context !== null ? params.context : config.context;
    this.semanticContext = params.semanticContext !== null ? params.semanticContext : config.semanticContext !== null ? config.semanticContext : SemanticContext.NONE;
    // TODO: make it a boolean then
    /**
     * We cannot execute predicates dependent upon local context unless
     * we know for sure we are in the correct context. Because there is
     * no way to do this efficiently, we simply cannot evaluate
     * dependent predicates unless we are in the rule that initially
     * invokes the ATN simulator.
     * closure() tracks the depth of how far we dip into the
     * outer context: depth &gt; 0.  Note that it may not be totally
     * accurate depth since I don't ever decrement
     */
    this.reachesIntoOuterContext = config.reachesIntoOuterContext;
    this.precedenceFilterSuppressed = config.precedenceFilterSuppressed;
  }
  checkContext(params, config) {
    if ((params.context === null || params.context === undefined) && (config === null || config.context === null || config.context === undefined)) {
      this.context = null;
    }
  }
  hashCode() {
    let hash = new HashCode();
    this.updateHashCode(hash);
    return hash.finish();
  }
  updateHashCode(hash) {
    hash.update(this.state.stateNumber, this.alt, this.context, this.semanticContext);
  }

  /**
   * An ATN configuration is equal to another if both have
   * the same state, they predict the same alternative, and
   * syntactic/semantic contexts are the same
   */
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof ATNConfig)) {
      return false;
    } else {
      return this.state.stateNumber === other.state.stateNumber && this.alt === other.alt && (this.context === null ? other.context === null : this.context.equals(other.context)) && this.semanticContext.equals(other.semanticContext) && this.precedenceFilterSuppressed === other.precedenceFilterSuppressed;
    }
  }
  hashCodeForConfigSet() {
    let hash = new HashCode();
    hash.update(this.state.stateNumber, this.alt, this.semanticContext);
    return hash.finish();
  }
  equalsForConfigSet(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof ATNConfig)) {
      return false;
    } else {
      return this.state.stateNumber === other.state.stateNumber && this.alt === other.alt && this.semanticContext.equals(other.semanticContext);
    }
  }
  toString() {
    return "(" + this.state + "," + this.alt + (this.context !== null ? ",[" + this.context.toString() + "]" : "") + (this.semanticContext !== SemanticContext.NONE ? "," + this.semanticContext.toString() : "") + (this.reachesIntoOuterContext > 0 ? ",up=" + this.reachesIntoOuterContext : "") + ")";
  }
}
;// ./src/antlr4/misc/Interval.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
/* stop is not included! */
class Interval {
  constructor(start, stop) {
    this.start = start;
    this.stop = stop;
  }
  clone() {
    return new Interval(this.start, this.stop);
  }
  contains(item) {
    return item >= this.start && item < this.stop;
  }
  toString() {
    if (this.start === this.stop - 1) {
      return this.start.toString();
    } else {
      return this.start.toString() + ".." + (this.stop - 1).toString();
    }
  }
  get length() {
    return this.stop - this.start;
  }
}
Interval.INVALID_INTERVAL = new Interval(-1, -2);
;// ./src/antlr4/misc/IntervalSet.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



class IntervalSet {
  constructor() {
    this.intervals = null;
    this.readOnly = false;
  }
  first(v) {
    if (this.intervals === null || this.intervals.length === 0) {
      return Token.INVALID_TYPE;
    } else {
      return this.intervals[0].start;
    }
  }
  addOne(v) {
    this.addInterval(new Interval(v, v + 1));
  }
  addRange(l, h) {
    this.addInterval(new Interval(l, h + 1));
  }
  addInterval(toAdd) {
    if (this.intervals === null) {
      this.intervals = [];
      this.intervals.push(toAdd.clone());
    } else {
      // find insert pos
      for (let pos = 0; pos < this.intervals.length; pos++) {
        let existing = this.intervals[pos];
        // distinct range -> insert
        if (toAdd.stop < existing.start) {
          this.intervals.splice(pos, 0, toAdd);
          return;
        }
        // contiguous range -> adjust
        else if (toAdd.stop === existing.start) {
          this.intervals[pos] = new Interval(toAdd.start, existing.stop);
          return;
        }
        // overlapping range -> adjust and reduce
        else if (toAdd.start <= existing.stop) {
          this.intervals[pos] = new Interval(Math.min(existing.start, toAdd.start), Math.max(existing.stop, toAdd.stop));
          this.reduce(pos);
          return;
        }
      }
      // greater than any existing
      this.intervals.push(toAdd.clone());
    }
  }
  addSet(other) {
    if (other.intervals !== null) {
      other.intervals.forEach(toAdd => this.addInterval(toAdd), this);
    }
    return this;
  }
  reduce(pos) {
    // only need to reduce if pos is not the last
    if (pos < this.intervals.length - 1) {
      let current = this.intervals[pos];
      let next = this.intervals[pos + 1];
      // if next contained in current
      if (current.stop >= next.stop) {
        this.intervals.splice(pos + 1, 1);
        this.reduce(pos);
      } else if (current.stop >= next.start) {
        this.intervals[pos] = new Interval(current.start, next.stop);
        this.intervals.splice(pos + 1, 1);
      }
    }
  }
  complement(start, stop) {
    let result = new IntervalSet();
    result.addInterval(new Interval(start, stop + 1));
    if (this.intervals !== null) this.intervals.forEach(toRemove => result.removeRange(toRemove));
    return result;
  }
  contains(item) {
    if (this.intervals === null) {
      return false;
    } else {
      for (let k = 0; k < this.intervals.length; k++) {
        if (this.intervals[k].contains(item)) {
          return true;
        }
      }
      return false;
    }
  }
  removeRange(toRemove) {
    if (toRemove.start === toRemove.stop - 1) {
      this.removeOne(toRemove.start);
    } else if (this.intervals !== null) {
      let pos = 0;
      for (let n = 0; n < this.intervals.length; n++) {
        let existing = this.intervals[pos];
        // intervals are ordered
        if (toRemove.stop <= existing.start) {
          return;
        }
        // check for including range, split it
        else if (toRemove.start > existing.start && toRemove.stop < existing.stop) {
          this.intervals[pos] = new Interval(existing.start, toRemove.start);
          let x = new Interval(toRemove.stop, existing.stop);
          this.intervals.splice(pos, 0, x);
          return;
        }
        // check for included range, remove it
        else if (toRemove.start <= existing.start && toRemove.stop >= existing.stop) {
          this.intervals.splice(pos, 1);
          pos = pos - 1; // need another pass
        }
        // check for lower boundary
        else if (toRemove.start < existing.stop) {
          this.intervals[pos] = new Interval(existing.start, toRemove.start);
        }
        // check for upper boundary
        else if (toRemove.stop < existing.stop) {
          this.intervals[pos] = new Interval(toRemove.stop, existing.stop);
        }
        pos += 1;
      }
    }
  }
  removeOne(value) {
    if (this.intervals !== null) {
      for (let i = 0; i < this.intervals.length; i++) {
        let existing = this.intervals[i];
        // intervals are ordered
        if (value < existing.start) {
          return;
        }
        // check for single value range
        else if (value === existing.start && value === existing.stop - 1) {
          this.intervals.splice(i, 1);
          return;
        }
        // check for lower boundary
        else if (value === existing.start) {
          this.intervals[i] = new Interval(existing.start + 1, existing.stop);
          return;
        }
        // check for upper boundary
        else if (value === existing.stop - 1) {
          this.intervals[i] = new Interval(existing.start, existing.stop - 1);
          return;
        }
        // split existing range
        else if (value < existing.stop - 1) {
          let replace = new Interval(existing.start, value);
          existing.start = value + 1;
          this.intervals.splice(i, 0, replace);
          return;
        }
      }
    }
  }
  toString(literalNames, symbolicNames, elemsAreChar) {
    literalNames = literalNames || null;
    symbolicNames = symbolicNames || null;
    elemsAreChar = elemsAreChar || false;
    if (this.intervals === null) {
      return "{}";
    } else if (literalNames !== null || symbolicNames !== null) {
      return this.toTokenString(literalNames, symbolicNames);
    } else if (elemsAreChar) {
      return this.toCharString();
    } else {
      return this.toIndexString();
    }
  }
  toCharString() {
    let names = [];
    for (let i = 0; i < this.intervals.length; i++) {
      let existing = this.intervals[i];
      if (existing.stop === existing.start + 1) {
        if (existing.start === Token.EOF) {
          names.push("<EOF>");
        } else {
          names.push("'" + String.fromCharCode(existing.start) + "'");
        }
      } else {
        names.push("'" + String.fromCharCode(existing.start) + "'..'" + String.fromCharCode(existing.stop - 1) + "'");
      }
    }
    if (names.length > 1) {
      return "{" + names.join(", ") + "}";
    } else {
      return names[0];
    }
  }
  toIndexString() {
    let names = [];
    for (let i = 0; i < this.intervals.length; i++) {
      let existing = this.intervals[i];
      if (existing.stop === existing.start + 1) {
        if (existing.start === Token.EOF) {
          names.push("<EOF>");
        } else {
          names.push(existing.start.toString());
        }
      } else {
        names.push(existing.start.toString() + ".." + (existing.stop - 1).toString());
      }
    }
    if (names.length > 1) {
      return "{" + names.join(", ") + "}";
    } else {
      return names[0];
    }
  }
  toTokenString(literalNames, symbolicNames) {
    let names = [];
    for (let i = 0; i < this.intervals.length; i++) {
      let existing = this.intervals[i];
      for (let j = existing.start; j < existing.stop; j++) {
        names.push(this.elementName(literalNames, symbolicNames, j));
      }
    }
    if (names.length > 1) {
      return "{" + names.join(", ") + "}";
    } else {
      return names[0];
    }
  }
  elementName(literalNames, symbolicNames, token) {
    if (token === Token.EOF) {
      return "<EOF>";
    } else if (token === Token.EPSILON) {
      return "<EPSILON>";
    } else {
      return literalNames[token] || symbolicNames[token];
    }
  }
  get length() {
    return this.intervals.map(interval => interval.length).reduce((acc, val) => acc + val);
  }
}
;// ./src/antlr4/state/ATNState.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

/**
 * The following images show the relation of states and
 * {@link ATNState//transitions} for various grammar constructs.
 *
 * <ul>
 *
 * <li>Solid edges marked with an &//0949; indicate a required
 * {@link EpsilonTransition}.</li>
 *
 * <li>Dashed edges indicate locations where any transition derived from
 * {@link Transition} might appear.</li>
 *
 * <li>Dashed nodes are place holders for either a sequence of linked
 * {@link BasicState} states or the inclusion of a block representing a nested
 * construct in one of the forms below.</li>
 *
 * <li>Nodes showing multiple outgoing alternatives with a {@code ...} support
 * any number of alternatives (one or more). Nodes without the {@code ...} only
 * support the exact number of alternatives shown in the diagram.</li>
 *
 * </ul>
 *
 * <h2>Basic Blocks</h2>
 *
 * <h3>Rule</h3>
 *
 * <embed src="images/Rule.svg" type="image/svg+xml"/>
 *
 * <h3>Block of 1 or more alternatives</h3>
 *
 * <embed src="images/Block.svg" type="image/svg+xml"/>
 *
 * <h2>Greedy Loops</h2>
 *
 * <h3>Greedy Closure: {@code (...)*}</h3>
 *
 * <embed src="images/ClosureGreedy.svg" type="image/svg+xml"/>
 *
 * <h3>Greedy Positive Closure: {@code (...)+}</h3>
 *
 * <embed src="images/PositiveClosureGreedy.svg" type="image/svg+xml"/>
 *
 * <h3>Greedy Optional: {@code (...)?}</h3>
 *
 * <embed src="images/OptionalGreedy.svg" type="image/svg+xml"/>
 *
 * <h2>Non-Greedy Loops</h2>
 *
 * <h3>Non-Greedy Closure: {@code (...)*?}</h3>
 *
 * <embed src="images/ClosureNonGreedy.svg" type="image/svg+xml"/>
 *
 * <h3>Non-Greedy Positive Closure: {@code (...)+?}</h3>
 *
 * <embed src="images/PositiveClosureNonGreedy.svg" type="image/svg+xml"/>
 *
 * <h3>Non-Greedy Optional: {@code (...)??}</h3>
 *
 * <embed src="images/OptionalNonGreedy.svg" type="image/svg+xml"/>
 */
class ATNState {
  constructor() {
    // Which ATN are we in?
    this.atn = null;
    this.stateNumber = ATNState.INVALID_STATE_NUMBER;
    this.stateType = null;
    this.ruleIndex = 0; // at runtime, we don't have Rule objects
    this.epsilonOnlyTransitions = false;
    // Track the transitions emanating from this ATN state.
    this.transitions = [];
    // Used to cache lookahead during parsing, not used during construction
    this.nextTokenWithinRule = null;
  }
  toString() {
    return this.stateNumber;
  }
  equals(other) {
    if (other instanceof ATNState) {
      return this.stateNumber === other.stateNumber;
    } else {
      return false;
    }
  }
  isNonGreedyExitState() {
    return false;
  }
  addTransition(trans, index) {
    if (index === undefined) {
      index = -1;
    }
    if (this.transitions.length === 0) {
      this.epsilonOnlyTransitions = trans.isEpsilon;
    } else if (this.epsilonOnlyTransitions !== trans.isEpsilon) {
      this.epsilonOnlyTransitions = false;
    }
    if (index === -1) {
      this.transitions.push(trans);
    } else {
      this.transitions.splice(index, 1, trans);
    }
  }
}

// constants for serialization
ATNState.INVALID_TYPE = 0;
ATNState.BASIC = 1;
ATNState.RULE_START = 2;
ATNState.BLOCK_START = 3;
ATNState.PLUS_BLOCK_START = 4;
ATNState.STAR_BLOCK_START = 5;
ATNState.TOKEN_START = 6;
ATNState.RULE_STOP = 7;
ATNState.BLOCK_END = 8;
ATNState.STAR_LOOP_BACK = 9;
ATNState.STAR_LOOP_ENTRY = 10;
ATNState.PLUS_LOOP_BACK = 11;
ATNState.LOOP_END = 12;
ATNState.serializationNames = ["INVALID", "BASIC", "RULE_START", "BLOCK_START", "PLUS_BLOCK_START", "STAR_BLOCK_START", "TOKEN_START", "RULE_STOP", "BLOCK_END", "STAR_LOOP_BACK", "STAR_LOOP_ENTRY", "PLUS_LOOP_BACK", "LOOP_END"];
ATNState.INVALID_STATE_NUMBER = -1;
;// ./src/antlr4/state/RuleStopState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


/**
 * The last node in the ATN for a rule, unless that rule is the start symbol.
 * In that case, there is one transition to EOF. Later, we might encode
 * references to all calls to this rule to compute FOLLOW sets for
 * error handling
 */
class RuleStopState extends ATNState {
  constructor() {
    super();
    this.stateType = ATNState.RULE_STOP;
    return this;
  }
}
;// ./src/antlr4/transition/Transition.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

/**
 * An ATN transition between any two ATN states.  Subclasses define
 * atom, set, epsilon, action, predicate, rule transitions.
 *
 * <p>This is a one way link.  It emanates from a state (usually via a list of
 * transitions) and has a target state.</p>
 *
 * <p>Since we never have to change the ATN transitions once we construct it,
 * we can fix these transitions as specific classes. The DFA transitions
 * on the other hand need to update the labels as it adds transitions to
 * the states. We'll use the term Edge for the DFA to distinguish them from
 * ATN transitions.</p>
 */
class Transition {
  constructor(target) {
    // The target of this transition.
    if (target === undefined || target === null) {
      throw "target cannot be null.";
    }
    this.target = target;
    // Are we epsilon, action, sempred?
    this.isEpsilon = false;
    this.label = null;
  }
}

// constants for serialization

Transition.EPSILON = 1;
Transition.RANGE = 2;
Transition.RULE = 3;
// e.g., {isType(input.LT(1))}?
Transition.PREDICATE = 4;
Transition.ATOM = 5;
Transition.ACTION = 6;
// ~(A|B) or ~atom, wildcard, which convert to next 2
Transition.SET = 7;
Transition.NOT_SET = 8;
Transition.WILDCARD = 9;
Transition.PRECEDENCE = 10;
Transition.serializationNames = ["INVALID", "EPSILON", "RANGE", "RULE", "PREDICATE", "ATOM", "ACTION", "SET", "NOT_SET", "WILDCARD", "PRECEDENCE"];
Transition.serializationTypes = {
  EpsilonTransition: Transition.EPSILON,
  RangeTransition: Transition.RANGE,
  RuleTransition: Transition.RULE,
  PredicateTransition: Transition.PREDICATE,
  AtomTransition: Transition.ATOM,
  ActionTransition: Transition.ACTION,
  SetTransition: Transition.SET,
  NotSetTransition: Transition.NOT_SET,
  WildcardTransition: Transition.WILDCARD,
  PrecedencePredicateTransition: Transition.PRECEDENCE
};
;// ./src/antlr4/transition/RuleTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class RuleTransition extends Transition {
  constructor(ruleStart, ruleIndex, precedence, followState) {
    super(ruleStart);
    // ptr to the rule definition object for this rule ref
    this.ruleIndex = ruleIndex;
    this.precedence = precedence;
    // what node to begin computations following ref to rule
    this.followState = followState;
    this.serializationType = Transition.RULE;
    this.isEpsilon = true;
  }
  matches(symbol, minVocabSymbol, maxVocabSymbol) {
    return false;
  }
}
;// ./src/antlr4/transition/SetTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
// A transition containing a set of values.



class SetTransition extends Transition {
  constructor(target, set) {
    super(target);
    this.serializationType = Transition.SET;
    if (set !== undefined && set !== null) {
      this.label = set;
    } else {
      this.label = new IntervalSet();
      this.label.addOne(Token.INVALID_TYPE);
    }
  }
  matches(symbol, minVocabSymbol, maxVocabSymbol) {
    return this.label.contains(symbol);
  }
  toString() {
    return this.label.toString();
  }
}
;// ./src/antlr4/transition/NotSetTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class NotSetTransition extends SetTransition {
  constructor(target, set) {
    super(target, set);
    this.serializationType = Transition.NOT_SET;
  }
  matches(symbol, minVocabSymbol, maxVocabSymbol) {
    return symbol >= minVocabSymbol && symbol <= maxVocabSymbol && !super.matches(symbol, minVocabSymbol, maxVocabSymbol);
  }
  toString() {
    return '~' + super.toString();
  }
}
;// ./src/antlr4/transition/WildcardTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class WildcardTransition extends Transition {
  constructor(target) {
    super(target);
    this.serializationType = Transition.WILDCARD;
  }
  matches(symbol, minVocabSymbol, maxVocabSymbol) {
    return symbol >= minVocabSymbol && symbol <= maxVocabSymbol;
  }
  toString() {
    return ".";
  }
}
;// ./src/antlr4/atn/AbstractPredicateTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class AbstractPredicateTransition extends Transition {
  constructor(target) {
    super(target);
  }
}
;// ./src/antlr4/tree/Tree.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

/**
 * The basic notion of a tree has a parent, a payload, and a list of children.
 * It is the most abstract interface for all the trees used by ANTLR.
 */
class Tree {}
;// ./src/antlr4/tree/SyntaxTree.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class SyntaxTree extends Tree {}
;// ./src/antlr4/tree/ParseTree.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class ParseTree extends SyntaxTree {}
;// ./src/antlr4/tree/RuleNode.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class RuleNode extends ParseTree {
  get ruleContext() {
    throw new Error("missing interface implementation");
  }
}
;// ./src/antlr4/tree/TerminalNode.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class TerminalNode extends ParseTree {}
;// ./src/antlr4/tree/ErrorNode.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class ErrorNode extends TerminalNode {}
;// ./src/antlr4/utils/escapeWhitespace.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
function escapeWhitespace(s, escapeSpaces) {
  s = s.replace(/\t/g, "\\t").replace(/\n/g, "\\n").replace(/\r/g, "\\r");
  if (escapeSpaces) {
    s = s.replace(/ /g, "\u00B7");
  }
  return s;
}
;// ./src/antlr4/tree/Trees.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */







/** A set of utility routines useful for all kinds of ANTLR trees. */
let Trees = {
  /**
   * Print out a whole tree in LISP form. {@link //getNodeText} is used on the
   *  node payloads to get the text for the nodes.  Detect
   *  parse trees and extract data appropriately.
   */
  toStringTree: function (tree, ruleNames, recog) {
    ruleNames = ruleNames || null;
    recog = recog || null;
    if (recog !== null) {
      ruleNames = recog.ruleNames;
    }
    let s = Trees.getNodeText(tree, ruleNames);
    s = escapeWhitespace(s, false);
    let c = tree.getChildCount();
    if (c === 0) {
      return s;
    }
    let res = "(" + s + ' ';
    if (c > 0) {
      s = Trees.toStringTree(tree.getChild(0), ruleNames);
      res = res.concat(s);
    }
    for (let i = 1; i < c; i++) {
      s = Trees.toStringTree(tree.getChild(i), ruleNames);
      res = res.concat(' ' + s);
    }
    res = res.concat(")");
    return res;
  },
  getNodeText: function (t, ruleNames, recog) {
    ruleNames = ruleNames || null;
    recog = recog || null;
    if (recog !== null) {
      ruleNames = recog.ruleNames;
    }
    if (ruleNames !== null) {
      if (t instanceof RuleNode) {
        let context = t.ruleContext;
        let altNumber = context.getAltNumber();
        // use let value of ATN.INVALID_ALT_NUMBER to avoid circular dependency
        if (altNumber != 0) {
          return ruleNames[t.ruleIndex] + ":" + altNumber;
        }
        return ruleNames[t.ruleIndex];
      } else if (t instanceof ErrorNode) {
        return t.toString();
      } else if (t instanceof TerminalNode) {
        if (t.symbol !== null) {
          return t.symbol.text;
        }
      }
    }
    // no recog for rule names
    let payload = t.getPayload();
    if (payload instanceof Token) {
      return payload.text;
    }
    return t.getPayload().toString();
  },
  /**
   * Return ordered list of all children of this node
   */
  getChildren: function (t) {
    let list = [];
    for (let i = 0; i < t.getChildCount(); i++) {
      list.push(t.getChild(i));
    }
    return list;
  },
  /**
   * Return a list of all ancestors of this node.  The first node of
   * list is the root and the last is the parent of this node.
   */
  getAncestors: function (t) {
    let ancestors = [];
    t = t.getParent();
    while (t !== null) {
      ancestors = [t].concat(ancestors);
      t = t.getParent();
    }
    return ancestors;
  },
  findAllTokenNodes: function (t, ttype) {
    return Trees.findAllNodes(t, ttype, true);
  },
  findAllRuleNodes: function (t, ruleIndex) {
    return Trees.findAllNodes(t, ruleIndex, false);
  },
  findAllNodes: function (t, index, findTokens) {
    let nodes = [];
    Trees._findAllNodes(t, index, findTokens, nodes);
    return nodes;
  },
  _findAllNodes: function (t, index, findTokens, nodes) {
    // check this node (the root) first
    if (findTokens && t instanceof TerminalNode) {
      if (t.symbol.type === index) {
        nodes.push(t);
      }
    } else if (!findTokens && t instanceof RuleNode) {
      if (t.ruleIndex === index) {
        nodes.push(t);
      }
    }
    // check children
    for (let i = 0; i < t.getChildCount(); i++) {
      Trees._findAllNodes(t.getChild(i), index, findTokens, nodes);
    }
  },
  descendants: function (t) {
    let nodes = [t];
    for (let i = 0; i < t.getChildCount(); i++) {
      nodes = nodes.concat(Trees.descendants(t.getChild(i)));
    }
    return nodes;
  }
};
/* harmony default export */ const tree_Trees = (Trees);
;// ./src/antlr4/context/RuleContext.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */




class RuleContext extends RuleNode {
  /** A rule context is a record of a single rule invocation. It knows
   * which context invoked it, if any. If there is no parent context, then
   * naturally the invoking state is not valid.  The parent link
   * provides a chain upwards from the current rule invocation to the root
   * of the invocation tree, forming a stack. We actually carry no
   * information about the rule associated with this context (except
   * when parsing). We keep only the state number of the invoking state from
   * the ATN submachine that invoked this. Contrast this with the s
   * pointer inside ParserRuleContext that tracks the current state
   * being "executed" for the current rule.
   *
   * The parent contexts are useful for computing lookahead sets and
   * getting error information.
   *
   * These objects are used during parsing and prediction.
   * For the special case of parsers, we use the subclass
   * ParserRuleContext.
   *
   * @see ParserRuleContext
   */
  constructor(parent, invokingState) {
    // What context invoked this rule?
    super();
    this.parentCtx = parent || null;
    /**
     * What state invoked the rule associated with this context?
     * The "return address" is the followState of invokingState
     * If parent is null, this should be -1.
     */
    this.invokingState = invokingState || -1;
  }
  depth() {
    let n = 0;
    let p = this;
    while (p !== null) {
      p = p.parentCtx;
      n += 1;
    }
    return n;
  }

  /**
   * A context is empty if there is no invoking state; meaning nobody call
   * current context.
   */
  isEmpty() {
    return this.invokingState === -1;
  }

  // satisfy the ParseTree / SyntaxTree interface
  getSourceInterval() {
    return Interval.INVALID_INTERVAL;
  }
  get ruleContext() {
    return this;
  }
  getPayload() {
    return this;
  }

  /**
   * Return the combined text of all child nodes. This method only considers
   * tokens which have been added to the parse tree.
   * <p>
   * Since tokens on hidden channels (e.g. whitespace or comments) are not
   * added to the parse trees, they will not appear in the output of this
   * method.
   */
  getText() {
    if (this.getChildCount() === 0) {
      return "";
    } else {
      return this.children.map(function (child) {
        return child.getText();
      }).join("");
    }
  }

  /**
   * For rule associated with this parse tree internal node, return
   * the outer alternative number used to match the input. Default
   * implementation does not compute nor store this alt num. Create
   * a subclass of ParserRuleContext with backing field and set
   * option contextSuperClass.
   * to set it.
   */
  getAltNumber() {
    // use constant value of ATN.INVALID_ALT_NUMBER to avoid circular dependency
    return 0;
  }

  /**
   * Set the outer alternative number for this context node. Default
   * implementation does nothing to avoid backing field overhead for
   * trees that don't need it.  Create
   * a subclass of ParserRuleContext with backing field and set
   * option contextSuperClass.
   */
  setAltNumber(altNumber) {}
  getChild(i) {
    return null;
  }
  getChildCount() {
    return 0;
  }
  accept(visitor) {
    return visitor.visitChildren(this);
  }

  /**
   * Print out a whole tree, not just a node, in LISP format
   * (root child1 .. childN). Print just a node if this is a leaf.
   */
  toStringTree(ruleNames, recog) {
    return tree_Trees.toStringTree(this, ruleNames, recog);
  }
  toString(ruleNames, stop) {
    ruleNames = ruleNames || null;
    stop = stop || null;
    let p = this;
    let s = "[";
    while (p !== null && p !== stop) {
      if (ruleNames === null) {
        if (!p.isEmpty()) {
          s += p.invokingState;
        }
      } else {
        let ri = p.ruleIndex;
        let ruleName = ri >= 0 && ri < ruleNames.length ? ruleNames[ri] : "" + ri;
        s += ruleName;
      }
      if (p.parentCtx !== null && (ruleNames !== null || !p.parentCtx.isEmpty())) {
        s += " ";
      }
      p = p.parentCtx;
    }
    s += "]";
    return s;
  }
}
;// ./src/antlr4/context/PredictionContext.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class PredictionContext {
  constructor(cachedHashCode) {
    this.cachedHashCode = cachedHashCode;
  }

  /**
   * Stores the computed hash code of this {@link PredictionContext}. The hash
   * code is computed in parts to match the following reference algorithm.
   *
   * <pre>
   * private int referenceHashCode() {
   * int hash = {@link MurmurHash//initialize MurmurHash.initialize}({@link
   * //INITIAL_HASH});
   *
   * for (int i = 0; i &lt; {@link //size()}; i++) {
   * hash = {@link MurmurHash//update MurmurHash.update}(hash, {@link //getParent
   * getParent}(i));
   * }
   *
   * for (int i = 0; i &lt; {@link //size()}; i++) {
   * hash = {@link MurmurHash//update MurmurHash.update}(hash, {@link
   * //getReturnState getReturnState}(i));
   * }
   *
   * hash = {@link MurmurHash//finish MurmurHash.finish}(hash, 2// {@link
   * //size()});
   * return hash;
   * }
   * </pre>
   * This means only the {@link //EMPTY} context is in set.
   */
  isEmpty() {
    return this === PredictionContext.EMPTY;
  }
  hasEmptyPath() {
    return this.getReturnState(this.length - 1) === PredictionContext.EMPTY_RETURN_STATE;
  }
  hashCode() {
    return this.cachedHashCode;
  }
  updateHashCode(hash) {
    hash.update(this.cachedHashCode);
  }
}

/**
 * Represents {@code $} in local context prediction, which means wildcard.
 * {@code//+x =//}.
 */
PredictionContext.EMPTY = null;

/**
 * Represents {@code $} in an array in full context mode, when {@code $}
 * doesn't mean wildcard: {@code $ + x = [$,x]}. Here,
 * {@code $} = {@link //EMPTY_RETURN_STATE}.
 */
PredictionContext.EMPTY_RETURN_STATE = 0x7FFFFFFF;
PredictionContext.globalNodeCount = 1;
PredictionContext.id = PredictionContext.globalNodeCount;
PredictionContext.trace_atn_sim = false;
;// ./src/antlr4/context/ArrayPredictionContext.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



class ArrayPredictionContext extends PredictionContext {
  constructor(parents, returnStates) {
    /**
     * Parent can be null only if full ctx mode and we make an array
     * from {@link //EMPTY} and non-empty. We merge {@link //EMPTY} by using
     * null parent and
     * returnState == {@link //EMPTY_RETURN_STATE}.
     */
    let h = new HashCode();
    h.update(parents, returnStates);
    let hashCode = h.finish();
    super(hashCode);
    this.parents = parents;
    this.returnStates = returnStates;
    return this;
  }
  isEmpty() {
    // since EMPTY_RETURN_STATE can only appear in the last position, we
    // don't need to verify that size==1
    return this.returnStates[0] === PredictionContext.EMPTY_RETURN_STATE;
  }
  getParent(index) {
    return this.parents[index];
  }
  getReturnState(index) {
    return this.returnStates[index];
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof ArrayPredictionContext)) {
      return false;
    } else if (this.hashCode() !== other.hashCode()) {
      return false; // can't be same if hash is different
    } else {
      return equalArrays(this.returnStates, other.returnStates) && equalArrays(this.parents, other.parents);
    }
  }
  toString() {
    if (this.isEmpty()) {
      return "[]";
    } else {
      let s = "[";
      for (let i = 0; i < this.returnStates.length; i++) {
        if (i > 0) {
          s = s + ", ";
        }
        if (this.returnStates[i] === PredictionContext.EMPTY_RETURN_STATE) {
          s = s + "$";
          continue;
        }
        s = s + this.returnStates[i];
        if (this.parents[i] !== null) {
          s = s + " " + this.parents[i];
        } else {
          s = s + "null";
        }
      }
      return s + "]";
    }
  }
  get length() {
    return this.returnStates.length;
  }
}
;// ./src/antlr4/context/SingletonPredictionContext.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class SingletonPredictionContext extends PredictionContext {
  constructor(parent, returnState) {
    let hashCode = 0;
    let hash = new HashCode();
    if (parent !== null) {
      hash.update(parent, returnState);
    } else {
      hash.update(1);
    }
    hashCode = hash.finish();
    super(hashCode);
    this.parentCtx = parent;
    this.returnState = returnState;
  }
  getParent(index) {
    return this.parentCtx;
  }
  getReturnState(index) {
    return this.returnState;
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof SingletonPredictionContext)) {
      return false;
    } else if (this.hashCode() !== other.hashCode()) {
      return false; // can't be same if hash is different
    } else {
      if (this.returnState !== other.returnState) return false;else if (this.parentCtx == null) return other.parentCtx == null;else return this.parentCtx.equals(other.parentCtx);
    }
  }
  toString() {
    let up = this.parentCtx === null ? "" : this.parentCtx.toString();
    if (up.length === 0) {
      if (this.returnState === PredictionContext.EMPTY_RETURN_STATE) {
        return "$";
      } else {
        return "" + this.returnState;
      }
    } else {
      return "" + this.returnState + " " + up;
    }
  }
  get length() {
    return 1;
  }
  static create(parent, returnState) {
    if (returnState === PredictionContext.EMPTY_RETURN_STATE && parent === null) {
      // someone can pass in the bits of an array ctx that mean $
      return PredictionContext.EMPTY;
    } else {
      return new SingletonPredictionContext(parent, returnState);
    }
  }
}
;// ./src/antlr4/context/EmptyPredictionContext.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class EmptyPredictionContext extends SingletonPredictionContext {
  constructor() {
    super(null, PredictionContext.EMPTY_RETURN_STATE);
  }
  isEmpty() {
    return true;
  }
  getParent(index) {
    return null;
  }
  getReturnState(index) {
    return this.returnState;
  }
  equals(other) {
    return this === other;
  }
  toString() {
    return "$";
  }
}
PredictionContext.EMPTY = new EmptyPredictionContext();
;// ./src/antlr4/misc/HashMap.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


let HashMap_DEFAULT_LOAD_FACTOR = 0.75;
let HashMap_INITIAL_CAPACITY = 16;
class HashMap_HashMap {
  constructor(hashFunction, equalsFunction) {
    this.buckets = new Array(HashMap_INITIAL_CAPACITY);
    this.threshold = Math.floor(HashMap_INITIAL_CAPACITY * HashMap_DEFAULT_LOAD_FACTOR);
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
    if (existing) {
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
    if (!bucket) {
      return false;
    }
    let existing = bucket.find(pair => this.equalsFunction(pair[0], key), this);
    return !!existing;
  }
  get(key) {
    let bucket = this._getBucket(key);
    if (!bucket) {
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
    this.threshold = Math.floor(newCapacity * HashMap_DEFAULT_LOAD_FACTOR);
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
;// ./src/antlr4/context/PredictionContextUtils.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */







/**
 * Convert a {@link RuleContext} tree to a {@link PredictionContext} graph.
 * Return {@link //EMPTY} if {@code outerContext} is empty or null.
 */
function predictionContextFromRuleContext(atn, outerContext) {
  if (outerContext === undefined || outerContext === null) {
    outerContext = RuleContext.EMPTY;
  }
  // if we are in RuleContext of start rule, s, then PredictionContext
  // is EMPTY. Nobody called us. (if we are empty, return empty)
  if (outerContext.parentCtx === null || outerContext === RuleContext.EMPTY) {
    return PredictionContext.EMPTY;
  }
  // If we have a parent, convert it to a PredictionContext graph
  let parent = predictionContextFromRuleContext(atn, outerContext.parentCtx);
  let state = atn.states[outerContext.invokingState];
  let transition = state.transitions[0];
  return SingletonPredictionContext.create(parent, transition.followState.stateNumber);
}
function getCachedPredictionContext(context, contextCache, visited) {
  if (context.isEmpty()) {
    return context;
  }
  let existing = visited.get(context) || null;
  if (existing !== null) {
    return existing;
  }
  existing = contextCache.get(context);
  if (existing !== null) {
    visited.set(context, existing);
    return existing;
  }
  let changed = false;
  let parents = [];
  for (let i = 0; i < parents.length; i++) {
    let parent = getCachedPredictionContext(context.getParent(i), contextCache, visited);
    if (changed || parent !== context.getParent(i)) {
      if (!changed) {
        parents = [];
        for (let j = 0; j < context.length; j++) {
          parents[j] = context.getParent(j);
        }
        changed = true;
      }
      parents[i] = parent;
    }
  }
  if (!changed) {
    contextCache.add(context);
    visited.set(context, context);
    return context;
  }
  let updated = null;
  if (parents.length === 0) {
    updated = PredictionContext.EMPTY;
  } else if (parents.length === 1) {
    updated = SingletonPredictionContext.create(parents[0], context.getReturnState(0));
  } else {
    updated = new ArrayPredictionContext(parents, context.returnStates);
  }
  contextCache.add(updated);
  visited.set(updated, updated);
  visited.set(context, updated);
  return updated;
}
function merge(a, b, rootIsWildcard, mergeCache) {
  // share same graph if both same
  if (a === b) {
    return a;
  }
  if (a instanceof SingletonPredictionContext && b instanceof SingletonPredictionContext) {
    return mergeSingletons(a, b, rootIsWildcard, mergeCache);
  }
  // At least one of a or b is array
  // If one is $ and rootIsWildcard, return $ as * wildcard
  if (rootIsWildcard) {
    if (a instanceof EmptyPredictionContext) {
      return a;
    }
    if (b instanceof EmptyPredictionContext) {
      return b;
    }
  }
  // convert singleton so both are arrays to normalize
  if (a instanceof SingletonPredictionContext) {
    a = new ArrayPredictionContext([a.getParent()], [a.returnState]);
  }
  if (b instanceof SingletonPredictionContext) {
    b = new ArrayPredictionContext([b.getParent()], [b.returnState]);
  }
  return mergeArrays(a, b, rootIsWildcard, mergeCache);
}

/**
 * Merge two {@link ArrayPredictionContext} instances.
 *
 * <p>Different tops, different parents.<br>
 * <embed src="images/ArrayMerge_DiffTopDiffPar.svg" type="image/svg+xml"/></p>
 *
 * <p>Shared top, same parents.<br>
 * <embed src="images/ArrayMerge_ShareTopSamePar.svg" type="image/svg+xml"/></p>
 *
 * <p>Shared top, different parents.<br>
 * <embed src="images/ArrayMerge_ShareTopDiffPar.svg" type="image/svg+xml"/></p>
 *
 * <p>Shared top, all shared parents.<br>
 * <embed src="images/ArrayMerge_ShareTopSharePar.svg"
 * type="image/svg+xml"/></p>
 *
 * <p>Equal tops, merge parents and reduce top to
 * {@link SingletonPredictionContext}.<br>
 * <embed src="images/ArrayMerge_EqualTop.svg" type="image/svg+xml"/></p>
 */
function mergeArrays(a, b, rootIsWildcard, mergeCache) {
  if (mergeCache !== null) {
    let previous = mergeCache.get(a, b);
    if (previous !== null) {
      if (PredictionContext.trace_atn_sim) console.log("mergeArrays a=" + a + ",b=" + b + " -> previous");
      return previous;
    }
    previous = mergeCache.get(b, a);
    if (previous !== null) {
      if (PredictionContext.trace_atn_sim) console.log("mergeArrays a=" + a + ",b=" + b + " -> previous");
      return previous;
    }
  }
  // merge sorted payloads a + b => M
  let i = 0; // walks a
  let j = 0; // walks b
  let k = 0; // walks target M array

  let mergedReturnStates = new Array(a.returnStates.length + b.returnStates.length).fill(0);
  let mergedParents = new Array(a.returnStates.length + b.returnStates.length).fill(null);
  // walk and merge to yield mergedParents, mergedReturnStates
  while (i < a.returnStates.length && j < b.returnStates.length) {
    let a_parent = a.parents[i];
    let b_parent = b.parents[j];
    if (a.returnStates[i] === b.returnStates[j]) {
      // same payload (stack tops are equal), must yield merged singleton
      let payload = a.returnStates[i];
      // $+$ = $
      let bothDollars = payload === PredictionContext.EMPTY_RETURN_STATE && a_parent === null && b_parent === null;
      let ax_ax = a_parent !== null && b_parent !== null && a_parent === b_parent; // ax+ax
      // ->
      // ax
      if (bothDollars || ax_ax) {
        mergedParents[k] = a_parent; // choose left
        mergedReturnStates[k] = payload;
      } else {
        // ax+ay -> a'[x,y]
        mergedParents[k] = merge(a_parent, b_parent, rootIsWildcard, mergeCache);
        mergedReturnStates[k] = payload;
      }
      i += 1; // hop over left one as usual
      j += 1; // but also skip one in right side since we merge
    } else if (a.returnStates[i] < b.returnStates[j]) {
      // copy a[i] to M
      mergedParents[k] = a_parent;
      mergedReturnStates[k] = a.returnStates[i];
      i += 1;
    } else {
      // b > a, copy b[j] to M
      mergedParents[k] = b_parent;
      mergedReturnStates[k] = b.returnStates[j];
      j += 1;
    }
    k += 1;
  }
  // copy over any payloads remaining in either array
  if (i < a.returnStates.length) {
    for (let p = i; p < a.returnStates.length; p++) {
      mergedParents[k] = a.parents[p];
      mergedReturnStates[k] = a.returnStates[p];
      k += 1;
    }
  } else {
    for (let p = j; p < b.returnStates.length; p++) {
      mergedParents[k] = b.parents[p];
      mergedReturnStates[k] = b.returnStates[p];
      k += 1;
    }
  }
  // trim merged if we combined a few that had same stack tops
  if (k < mergedParents.length) {
    // write index < last position; trim
    if (k === 1) {
      // for just one merged element, return singleton top
      let a_ = SingletonPredictionContext.create(mergedParents[0], mergedReturnStates[0]);
      if (mergeCache !== null) {
        mergeCache.set(a, b, a_);
      }
      return a_;
    }
    mergedParents = mergedParents.slice(0, k);
    mergedReturnStates = mergedReturnStates.slice(0, k);
  }
  let M = new ArrayPredictionContext(mergedParents, mergedReturnStates);

  // if we created same array as a or b, return that instead
  // TODO: track whether this is possible above during merge sort for speed
  if (M.equals(a)) {
    if (mergeCache !== null) {
      mergeCache.set(a, b, a);
    }
    if (PredictionContext.trace_atn_sim) console.log("mergeArrays a=" + a + ",b=" + b + " -> a");
    return a;
  }
  if (M.equals(b)) {
    if (mergeCache !== null) {
      mergeCache.set(a, b, b);
    }
    if (PredictionContext.trace_atn_sim) console.log("mergeArrays a=" + a + ",b=" + b + " -> b");
    return b;
  }
  combineCommonParents(mergedParents);
  if (mergeCache !== null) {
    mergeCache.set(a, b, M);
  }
  if (PredictionContext.trace_atn_sim) console.log("mergeArrays a=" + a + ",b=" + b + " -> " + M);
  return M;
}

/**
 * Make pass over all <em>M</em> {@code parents}; merge any {@code equals()}
 * ones.
 */
function combineCommonParents(parents) {
  let uniqueParents = new HashMap_HashMap();
  for (let p = 0; p < parents.length; p++) {
    let parent = parents[p];
    if (!uniqueParents.containsKey(parent)) {
      uniqueParents.set(parent, parent);
    }
  }
  for (let q = 0; q < parents.length; q++) {
    parents[q] = uniqueParents.get(parents[q]);
  }
}

/**
 * Merge two {@link SingletonPredictionContext} instances.
 *
 * <p>Stack tops equal, parents merge is same; return left graph.<br>
 * <embed src="images/SingletonMerge_SameRootSamePar.svg"
 * type="image/svg+xml"/></p>
 *
 * <p>Same stack top, parents differ; merge parents giving array node, then
 * remainders of those graphs. A new root node is created to point to the
 * merged parents.<br>
 * <embed src="images/SingletonMerge_SameRootDiffPar.svg"
 * type="image/svg+xml"/></p>
 *
 * <p>Different stack tops pointing to same parent. Make array node for the
 * root where both element in the root point to the same (original)
 * parent.<br>
 * <embed src="images/SingletonMerge_DiffRootSamePar.svg"
 * type="image/svg+xml"/></p>
 *
 * <p>Different stack tops pointing to different parents. Make array node for
 * the root where each element points to the corresponding original
 * parent.<br>
 * <embed src="images/SingletonMerge_DiffRootDiffPar.svg"
 * type="image/svg+xml"/></p>
 *
 * @param a the first {@link SingletonPredictionContext}
 * @param b the second {@link SingletonPredictionContext}
 * @param rootIsWildcard {@code true} if this is a local-context merge,
 * otherwise false to indicate a full-context merge
 * @param mergeCache
 */
function mergeSingletons(a, b, rootIsWildcard, mergeCache) {
  if (mergeCache !== null) {
    let previous = mergeCache.get(a, b);
    if (previous !== null) {
      return previous;
    }
    previous = mergeCache.get(b, a);
    if (previous !== null) {
      return previous;
    }
  }
  let rootMerge = mergeRoot(a, b, rootIsWildcard);
  if (rootMerge !== null) {
    if (mergeCache !== null) {
      mergeCache.set(a, b, rootMerge);
    }
    return rootMerge;
  }
  if (a.returnState === b.returnState) {
    let parent = merge(a.parentCtx, b.parentCtx, rootIsWildcard, mergeCache);
    // if parent is same as existing a or b parent or reduced to a parent,
    // return it
    if (parent === a.parentCtx) {
      return a; // ax + bx = ax, if a=b
    }
    if (parent === b.parentCtx) {
      return b; // ax + bx = bx, if a=b
    }
    // else: ax + ay = a'[x,y]
    // merge parents x and y, giving array node with x,y then remainders
    // of those graphs. dup a, a' points at merged array
    // new joined parent so create new singleton pointing to it, a'
    let spc = SingletonPredictionContext.create(parent, a.returnState);
    if (mergeCache !== null) {
      mergeCache.set(a, b, spc);
    }
    return spc;
  } else {
    // a != b payloads differ
    // see if we can collapse parents due to $+x parents if local ctx
    let singleParent = null;
    if (a === b || a.parentCtx !== null && a.parentCtx === b.parentCtx) {
      // ax +
      // bx =
      // [a,b]x
      singleParent = a.parentCtx;
    }
    if (singleParent !== null) {
      // parents are same
      // sort payloads and use same parent
      let payloads = [a.returnState, b.returnState];
      if (a.returnState > b.returnState) {
        payloads[0] = b.returnState;
        payloads[1] = a.returnState;
      }
      let parents = [singleParent, singleParent];
      let apc = new ArrayPredictionContext(parents, payloads);
      if (mergeCache !== null) {
        mergeCache.set(a, b, apc);
      }
      return apc;
    }
    // parents differ and can't merge them. Just pack together
    // into array; can't merge.
    // ax + by = [ax,by]
    let payloads = [a.returnState, b.returnState];
    let parents = [a.parentCtx, b.parentCtx];
    if (a.returnState > b.returnState) {
      // sort by payload
      payloads[0] = b.returnState;
      payloads[1] = a.returnState;
      parents = [b.parentCtx, a.parentCtx];
    }
    let a_ = new ArrayPredictionContext(parents, payloads);
    if (mergeCache !== null) {
      mergeCache.set(a, b, a_);
    }
    return a_;
  }
}

/**
 * Handle case where at least one of {@code a} or {@code b} is
 * {@link //EMPTY}. In the following diagrams, the symbol {@code $} is used
 * to represent {@link //EMPTY}.
 *
 * <h2>Local-Context Merges</h2>
 *
 * <p>These local-context merge operations are used when {@code rootIsWildcard}
 * is true.</p>
 *
 * <p>{@link //EMPTY} is superset of any graph; return {@link //EMPTY}.<br>
 * <embed src="images/LocalMerge_EmptyRoot.svg" type="image/svg+xml"/></p>
 *
 * <p>{@link //EMPTY} and anything is {@code //EMPTY}, so merged parent is
 * {@code //EMPTY}; return left graph.<br>
 * <embed src="images/LocalMerge_EmptyParent.svg" type="image/svg+xml"/></p>
 *
 * <p>Special case of last merge if local context.<br>
 * <embed src="images/LocalMerge_DiffRoots.svg" type="image/svg+xml"/></p>
 *
 * <h2>Full-Context Merges</h2>
 *
 * <p>These full-context merge operations are used when {@code rootIsWildcard}
 * is false.</p>
 *
 * <p><embed src="images/FullMerge_EmptyRoots.svg" type="image/svg+xml"/></p>
 *
 * <p>Must keep all contexts; {@link //EMPTY} in array is a special value (and
 * null parent).<br>
 * <embed src="images/FullMerge_EmptyRoot.svg" type="image/svg+xml"/></p>
 *
 * <p><embed src="images/FullMerge_SameRoot.svg" type="image/svg+xml"/></p>
 *
 * @param a the first {@link SingletonPredictionContext}
 * @param b the second {@link SingletonPredictionContext}
 * @param rootIsWildcard {@code true} if this is a local-context merge,
 * otherwise false to indicate a full-context merge
 */
function mergeRoot(a, b, rootIsWildcard) {
  if (rootIsWildcard) {
    if (a === PredictionContext.EMPTY) {
      return PredictionContext.EMPTY; // // + b =//
    }
    if (b === PredictionContext.EMPTY) {
      return PredictionContext.EMPTY; // a +// =//
    }
  } else {
    if (a === PredictionContext.EMPTY && b === PredictionContext.EMPTY) {
      return PredictionContext.EMPTY; // $ + $ = $
    } else if (a === PredictionContext.EMPTY) {
      // $ + x = [$,x]
      let payloads = [b.returnState, PredictionContext.EMPTY_RETURN_STATE];
      let parents = [b.parentCtx, null];
      return new ArrayPredictionContext(parents, payloads);
    } else if (b === PredictionContext.EMPTY) {
      // x + $ = [$,x] ($ is always first if present)
      let payloads = [a.returnState, PredictionContext.EMPTY_RETURN_STATE];
      let parents = [a.parentCtx, null];
      return new ArrayPredictionContext(parents, payloads);
    }
  }
  return null;
}

// ter's recursive version of Sam's getAllNodes()
function getAllContextNodes(context, nodes, visited) {
  if (nodes === null) {
    nodes = [];
    return getAllContextNodes(context, nodes, visited);
  } else if (visited === null) {
    visited = new HashMap();
    return getAllContextNodes(context, nodes, visited);
  } else {
    if (context === null || visited.containsKey(context)) {
      return nodes;
    }
    visited.set(context, context);
    nodes.push(context);
    for (let i = 0; i < context.length; i++) {
      getAllContextNodes(context.getParent(i), nodes, visited);
    }
    return nodes;
  }
}
;// ./src/antlr4/misc/BitSet.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class BitSet {
  constructor() {
    this.data = new Uint32Array(1);
  }
  set(index) {
    BitSet._checkIndex(index);
    this._resize(index);
    this.data[index >>> 5] |= 1 << index % 32;
  }
  get(index) {
    BitSet._checkIndex(index);
    let slot = index >>> 5;
    if (slot >= this.data.length) {
      return false;
    }
    return (this.data[slot] & 1 << index % 32) !== 0;
  }
  clear(index) {
    BitSet._checkIndex(index);
    let slot = index >>> 5;
    if (slot < this.data.length) {
      this.data[slot] &= ~(1 << index);
    }
  }
  or(set) {
    let minCount = Math.min(this.data.length, set.data.length);
    for (let k = 0; k < minCount; ++k) {
      this.data[k] |= set.data[k];
    }
    if (this.data.length < set.data.length) {
      this._resize((set.data.length << 5) - 1);
      let c = set.data.length;
      for (let k = minCount; k < c; ++k) {
        this.data[k] = set.data[k];
      }
    }
  }
  values() {
    let result = new Array(this.length);
    let pos = 0;
    let length = this.data.length;
    for (let k = 0; k < length; ++k) {
      let l = this.data[k];
      while (l !== 0) {
        let t = l & -l;
        result[pos++] = (k << 5) + BitSet._bitCount(t - 1);
        l ^= t;
      }
    }
    return result;
  }
  minValue() {
    for (let k = 0; k < this.data.length; ++k) {
      let l = this.data[k];
      if (l !== 0) {
        let result = 0;
        while ((l & 1) === 0) {
          result++;
          l >>= 1;
        }
        return result + 32 * k;
      }
    }
    return 0;
  }
  hashCode() {
    return HashCode.hashStuff(this.values());
  }
  equals(other) {
    return other instanceof BitSet && equalArrays(this.data, other.data);
  }
  toString() {
    return "{" + this.values().join(", ") + "}";
  }
  get length() {
    return this.data.map(l => BitSet._bitCount(l)).reduce((s, v) => s + v, 0);
  }
  _resize(index) {
    let count = index + 32 >>> 5;
    if (count <= this.data.length) {
      return;
    }
    let data = new Uint32Array(count);
    data.set(this.data);
    data.fill(0, this.data.length);
    this.data = data;
  }
  static _checkIndex(index) {
    if (index < 0) throw new RangeError("index cannot be negative");
  }
  static _bitCount(l) {
    // see https://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel
    let count = 0;
    l = l - (l >> 1 & 0x55555555);
    l = (l & 0x33333333) + (l >> 2 & 0x33333333);
    l = l + (l >> 4) & 0x0f0f0f0f;
    l = l + (l >> 8);
    l = l + (l >> 16);
    return count + l & 0x3f;
  }
}
;// ./src/antlr4/atn/LL1Analyzer.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */














class LL1Analyzer {
  constructor(atn) {
    this.atn = atn;
  }

  /**
   * Calculates the SLL(1) expected lookahead set for each outgoing transition
   * of an {@link ATNState}. The returned array has one element for each
   * outgoing transition in {@code s}. If the closure from transition
   * <em>i</em> leads to a semantic predicate before matching a symbol, the
   * element at index <em>i</em> of the result will be {@code null}.
   *
   * @param s the ATN state
   * @return the expected symbols for each outgoing transition of {@code s}.
   */
  getDecisionLookahead(s) {
    if (s === null) {
      return null;
    }
    let count = s.transitions.length;
    let look = [];
    for (let alt = 0; alt < count; alt++) {
      look[alt] = new IntervalSet();
      let lookBusy = new HashSet();
      let seeThruPreds = false; // fail to get lookahead upon pred
      this._LOOK(s.transition(alt).target, null, PredictionContext.EMPTY, look[alt], lookBusy, new BitSet(), seeThruPreds, false);
      // Wipe out lookahead for this alternative if we found nothing
      // or we had a predicate when we !seeThruPreds
      if (look[alt].length === 0 || look[alt].contains(LL1Analyzer.HIT_PRED)) {
        look[alt] = null;
      }
    }
    return look;
  }

  /**
   * Compute set of tokens that can follow {@code s} in the ATN in the
   * specified {@code ctx}.
   *
   * <p>If {@code ctx} is {@code null} and the end of the rule containing
   * {@code s} is reached, {@link Token//EPSILON} is added to the result set.
   * If {@code ctx} is not {@code null} and the end of the outermost rule is
   * reached, {@link Token//EOF} is added to the result set.</p>
   *
   * @param s the ATN state
   * @param stopState the ATN state to stop at. This can be a
   * {@link BlockEndState} to detect epsilon paths through a closure.
   * @param ctx the complete parser context, or {@code null} if the context
   * should be ignored
   *
   * @return The set of tokens that can follow {@code s} in the ATN in the
   * specified {@code ctx}.
   */
  LOOK(s, stopState, ctx) {
    let r = new IntervalSet();
    let seeThruPreds = true; // ignore preds; get all lookahead
    ctx = ctx || null;
    let lookContext = ctx !== null ? predictionContextFromRuleContext(s.atn, ctx) : null;
    this._LOOK(s, stopState, lookContext, r, new HashSet(), new BitSet(), seeThruPreds, true);
    return r;
  }

  /**
   * Compute set of tokens that can follow {@code s} in the ATN in the
   * specified {@code ctx}.
   *
   * <p>If {@code ctx} is {@code null} and {@code stopState} or the end of the
   * rule containing {@code s} is reached, {@link Token//EPSILON} is added to
   * the result set. If {@code ctx} is not {@code null} and {@code addEOF} is
   * {@code true} and {@code stopState} or the end of the outermost rule is
   * reached, {@link Token//EOF} is added to the result set.</p>
   *
   * @param s the ATN state.
   * @param stopState the ATN state to stop at. This can be a
   * {@link BlockEndState} to detect epsilon paths through a closure.
   * @param ctx The outer context, or {@code null} if the outer context should
   * not be used.
   * @param look The result lookahead set.
   * @param lookBusy A set used for preventing epsilon closures in the ATN
   * from causing a stack overflow. Outside code should pass
   * {@code new CustomizedSet<ATNConfig>} for this argument.
   * @param calledRuleStack A set used for preventing left recursion in the
   * ATN from causing a stack overflow. Outside code should pass
   * {@code new BitSet()} for this argument.
   * @param seeThruPreds {@code true} to true semantic predicates as
   * implicitly {@code true} and "see through them", otherwise {@code false}
   * to treat semantic predicates as opaque and add {@link //HIT_PRED} to the
   * result if one is encountered.
   * @param addEOF Add {@link Token//EOF} to the result if the end of the
   * outermost context is reached. This parameter has no effect if {@code ctx}
   * is {@code null}.
   */
  _LOOK(s, stopState, ctx, look, lookBusy, calledRuleStack, seeThruPreds, addEOF) {
    let c = new ATNConfig({
      state: s,
      alt: 0,
      context: ctx
    }, null);
    if (lookBusy.has(c)) {
      return;
    }
    lookBusy.add(c);
    if (s === stopState) {
      if (ctx === null) {
        look.addOne(Token.EPSILON);
        return;
      } else if (ctx.isEmpty() && addEOF) {
        look.addOne(Token.EOF);
        return;
      }
    }
    if (s instanceof RuleStopState) {
      if (ctx === null) {
        look.addOne(Token.EPSILON);
        return;
      } else if (ctx.isEmpty() && addEOF) {
        look.addOne(Token.EOF);
        return;
      }
      if (ctx !== PredictionContext.EMPTY) {
        let removed = calledRuleStack.get(s.ruleIndex);
        try {
          calledRuleStack.clear(s.ruleIndex);
          // run thru all possible stack tops in ctx
          for (let i = 0; i < ctx.length; i++) {
            let returnState = this.atn.states[ctx.getReturnState(i)];
            this._LOOK(returnState, stopState, ctx.getParent(i), look, lookBusy, calledRuleStack, seeThruPreds, addEOF);
          }
        } finally {
          if (removed) {
            calledRuleStack.set(s.ruleIndex);
          }
        }
        return;
      }
    }
    for (let j = 0; j < s.transitions.length; j++) {
      let t = s.transitions[j];
      if (t.constructor === RuleTransition) {
        if (calledRuleStack.get(t.target.ruleIndex)) {
          continue;
        }
        let newContext = SingletonPredictionContext.create(ctx, t.followState.stateNumber);
        try {
          calledRuleStack.set(t.target.ruleIndex);
          this._LOOK(t.target, stopState, newContext, look, lookBusy, calledRuleStack, seeThruPreds, addEOF);
        } finally {
          calledRuleStack.clear(t.target.ruleIndex);
        }
      } else if (t instanceof AbstractPredicateTransition) {
        if (seeThruPreds) {
          this._LOOK(t.target, stopState, ctx, look, lookBusy, calledRuleStack, seeThruPreds, addEOF);
        } else {
          look.addOne(LL1Analyzer.HIT_PRED);
        }
      } else if (t.isEpsilon) {
        this._LOOK(t.target, stopState, ctx, look, lookBusy, calledRuleStack, seeThruPreds, addEOF);
      } else if (t.constructor === WildcardTransition) {
        look.addRange(Token.MIN_USER_TOKEN_TYPE, this.atn.maxTokenType);
      } else {
        let set = t.label;
        if (set !== null) {
          if (t instanceof NotSetTransition) {
            set = set.complement(Token.MIN_USER_TOKEN_TYPE, this.atn.maxTokenType);
          }
          look.addSet(set);
        }
      }
    }
  }
}

/**
 * Special value added to the lookahead sets to indicate that we hit
 * a predicate during analysis if {@code seeThruPreds==false}.
 */
LL1Analyzer.HIT_PRED = Token.INVALID_TYPE;
;// ./src/antlr4/atn/ATN.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */




class ATN {
  constructor(grammarType, maxTokenType) {
    /**
     * Used for runtime deserialization of ATNs from strings
     * The type of the ATN.
    */
    this.grammarType = grammarType;
    // The maximum value for any symbol recognized by a transition in the ATN.
    this.maxTokenType = maxTokenType;
    this.states = [];
    /**
     * Each subrule/rule is a decision point and we must track them so we
     * can go back later and build DFA predictors for them.  This includes
     * all the rules, subrules, optional blocks, ()+, ()* etc...
     */
    this.decisionToState = [];
    // Maps from rule index to starting state number.
    this.ruleToStartState = [];
    // Maps from rule index to stop state number.
    this.ruleToStopState = null;
    this.modeNameToStartState = {};
    /**
     * For lexer ATNs, this maps the rule index to the resulting token type.
     * For parser ATNs, this maps the rule index to the generated bypass token
     * type if the {@link ATNDeserializationOptions//isGenerateRuleBypassTransitions}
     * deserialization option was specified; otherwise, this is {@code null}
     */
    this.ruleToTokenType = null;
    /**
     * For lexer ATNs, this is an array of {@link LexerAction} objects which may
     * be referenced by action transitions in the ATN
     */
    this.lexerActions = null;
    this.modeToStartState = [];
  }

  /**
   * Compute the set of valid tokens that can occur starting in state {@code s}.
   * If {@code ctx} is null, the set of tokens will not include what can follow
   * the rule surrounding {@code s}. In other words, the set will be
   * restricted to tokens reachable staying within {@code s}'s rule
   */
  nextTokensInContext(s, ctx) {
    let anal = new LL1Analyzer(this);
    return anal.LOOK(s, null, ctx);
  }

  /**
   * Compute the set of valid tokens that can occur starting in {@code s} and
   * staying in same rule. {@link Token//EPSILON} is in set if we reach end of
   * rule
   */
  nextTokensNoContext(s) {
    if (s.nextTokenWithinRule !== null) {
      return s.nextTokenWithinRule;
    }
    s.nextTokenWithinRule = this.nextTokensInContext(s, null);
    s.nextTokenWithinRule.readOnly = true;
    return s.nextTokenWithinRule;
  }
  nextTokens(s, ctx) {
    if (ctx === undefined) {
      return this.nextTokensNoContext(s);
    } else {
      return this.nextTokensInContext(s, ctx);
    }
  }
  addState(state) {
    if (state !== null) {
      state.atn = this;
      state.stateNumber = this.states.length;
    }
    this.states.push(state);
  }
  removeState(state) {
    this.states[state.stateNumber] = null; // just free mem, don't shift states in list
  }
  defineDecisionState(s) {
    this.decisionToState.push(s);
    s.decision = this.decisionToState.length - 1;
    return s.decision;
  }
  getDecisionState(decision) {
    if (this.decisionToState.length === 0) {
      return null;
    } else {
      return this.decisionToState[decision];
    }
  }

  /**
   * Computes the set of input symbols which could follow ATN state number
   * {@code stateNumber} in the specified full {@code context}. This method
   * considers the complete parser context, but does not evaluate semantic
   * predicates (i.e. all predicates encountered during the calculation are
   * assumed true). If a path in the ATN exists from the starting state to the
   * {@link RuleStopState} of the outermost context without matching any
   * symbols, {@link Token//EOF} is added to the returned set.
   *
   * <p>If {@code context} is {@code null}, it is treated as
   * {@link ParserRuleContext//EMPTY}.</p>
   *
   * @param stateNumber the ATN state number
   * @param ctx the full parse context
   *
   * @return {IntervalSet} The set of potentially valid input symbols which could follow the
   * specified state in the specified context.
   *
   * @throws IllegalArgumentException if the ATN does not contain a state with
   * number {@code stateNumber}
   */
  getExpectedTokens(stateNumber, ctx) {
    if (stateNumber < 0 || stateNumber >= this.states.length) {
      throw "Invalid state number.";
    }
    let s = this.states[stateNumber];
    let following = this.nextTokens(s);
    if (!following.contains(Token.EPSILON)) {
      return following;
    }
    let expected = new IntervalSet();
    expected.addSet(following);
    expected.removeOne(Token.EPSILON);
    while (ctx !== null && ctx.invokingState >= 0 && following.contains(Token.EPSILON)) {
      let invokingState = this.states[ctx.invokingState];
      let rt = invokingState.transitions[0];
      following = this.nextTokens(rt.followState);
      expected.addSet(following);
      expected.removeOne(Token.EPSILON);
      ctx = ctx.parentCtx;
    }
    if (following.contains(Token.EPSILON)) {
      expected.addOne(Token.EOF);
    }
    return expected;
  }
}
ATN.INVALID_ALT_NUMBER = 0;
;// ./src/antlr4/atn/ATNType.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

/**
 * Represents the type of recognizer an ATN applies to
 */
/* harmony default export */ const ATNType = ({
  LEXER: 0,
  PARSER: 1
});
;// ./src/antlr4/state/BasicState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class BasicState extends ATNState {
  constructor() {
    super();
    this.stateType = ATNState.BASIC;
  }
}
;// ./src/antlr4/state/DecisionState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class DecisionState extends ATNState {
  constructor() {
    super();
    this.decision = -1;
    this.nonGreedy = false;
    return this;
  }
}
;// ./src/antlr4/state/BlockStartState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


/**
 *  The start of a regular {@code (...)} block
 */
class BlockStartState extends DecisionState {
  constructor() {
    super();
    this.endState = null;
    return this;
  }
}
;// ./src/antlr4/state/BlockEndState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


/**
 * Terminal node of a simple {@code (a|b|c)} block
 */
class BlockEndState extends ATNState {
  constructor() {
    super();
    this.stateType = ATNState.BLOCK_END;
    this.startState = null;
    return this;
  }
}
;// ./src/antlr4/state/LoopEndState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


/**
 * Mark the end of a * or + loop
 */
class LoopEndState extends ATNState {
  constructor() {
    super();
    this.stateType = ATNState.LOOP_END;
    this.loopBackState = null;
    return this;
  }
}
;// ./src/antlr4/state/RuleStartState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class RuleStartState extends ATNState {
  constructor() {
    super();
    this.stateType = ATNState.RULE_START;
    this.stopState = null;
    this.isPrecedenceRule = false;
    return this;
  }
}
;// ./src/antlr4/state/TokensStartState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * The Tokens rule start state linking to each lexer rule start state
 */
class TokensStartState extends DecisionState {
  constructor() {
    super();
    this.stateType = ATNState.TOKEN_START;
    return this;
  }
}
;// ./src/antlr4/state/PlusLoopbackState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Decision state for {@code A+} and {@code (A|B)+}.  It has two transitions:
 * one to the loop back to start of the block and one to exit.
 */
class PlusLoopbackState extends DecisionState {
  constructor() {
    super();
    this.stateType = ATNState.PLUS_LOOP_BACK;
    return this;
  }
}
;// ./src/antlr4/state/StarLoopbackState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class StarLoopbackState extends ATNState {
  constructor() {
    super();
    this.stateType = ATNState.STAR_LOOP_BACK;
    return this;
  }
}
;// ./src/antlr4/state/StarLoopEntryState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class StarLoopEntryState extends DecisionState {
  constructor() {
    super();
    this.stateType = ATNState.STAR_LOOP_ENTRY;
    this.loopBackState = null;
    // Indicates whether this state can benefit from a precedence DFA during SLL decision making.
    this.isPrecedenceDecision = null;
    return this;
  }
}
;// ./src/antlr4/state/PlusBlockStartState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Start of {@code (A|B|...)+} loop. Technically a decision state, but
 * we don't use for code generation; somebody might need it, so I'm defining
 * it for completeness. In reality, the {@link PlusLoopbackState} node is the
 * real decision-making note for {@code A+}
 */
class PlusBlockStartState extends BlockStartState {
  constructor() {
    super();
    this.stateType = ATNState.PLUS_BLOCK_START;
    this.loopBackState = null;
    return this;
  }
}
;// ./src/antlr4/state/StarBlockStartState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * The block that begins a closure loop
 */
class StarBlockStartState extends BlockStartState {
  constructor() {
    super();
    this.stateType = ATNState.STAR_BLOCK_START;
    return this;
  }
}
;// ./src/antlr4/state/BasicBlockStartState.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class BasicBlockStartState extends BlockStartState {
  constructor() {
    super();
    this.stateType = ATNState.BLOCK_START;
    return this;
  }
}
;// ./src/antlr4/transition/AtomTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class AtomTransition extends Transition {
  constructor(target, label) {
    super(target);
    // The token type or character value; or, signifies special label.
    this.label_ = label;
    this.label = this.makeLabel();
    this.serializationType = Transition.ATOM;
  }
  makeLabel() {
    let s = new IntervalSet();
    s.addOne(this.label_);
    return s;
  }
  matches(symbol, minVocabSymbol, maxVocabSymbol) {
    return this.label_ === symbol;
  }
  toString() {
    return this.label_;
  }
}
;// ./src/antlr4/transition/RangeTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class RangeTransition extends Transition {
  constructor(target, start, stop) {
    super(target);
    this.serializationType = Transition.RANGE;
    this.start = start;
    this.stop = stop;
    this.label = this.makeLabel();
  }
  makeLabel() {
    let s = new IntervalSet();
    s.addRange(this.start, this.stop);
    return s;
  }
  matches(symbol, minVocabSymbol, maxVocabSymbol) {
    return symbol >= this.start && symbol <= this.stop;
  }
  toString() {
    return "'" + String.fromCharCode(this.start) + "'..'" + String.fromCharCode(this.stop) + "'";
  }
}
;// ./src/antlr4/transition/ActionTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class ActionTransition extends Transition {
  constructor(target, ruleIndex, actionIndex, isCtxDependent) {
    super(target);
    this.serializationType = Transition.ACTION;
    this.ruleIndex = ruleIndex;
    this.actionIndex = actionIndex === undefined ? -1 : actionIndex;
    this.isCtxDependent = isCtxDependent === undefined ? false : isCtxDependent; // e.g., $i ref in pred
    this.isEpsilon = true;
  }
  matches(symbol, minVocabSymbol, maxVocabSymbol) {
    return false;
  }
  toString() {
    return "action_" + this.ruleIndex + ":" + this.actionIndex;
  }
}
;// ./src/antlr4/transition/EpsilonTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class EpsilonTransition extends Transition {
  constructor(target, outermostPrecedenceReturn) {
    super(target);
    this.serializationType = Transition.EPSILON;
    this.isEpsilon = true;
    this.outermostPrecedenceReturn = outermostPrecedenceReturn;
  }
  matches(symbol, minVocabSymbol, maxVocabSymbol) {
    return false;
  }
  toString() {
    return "epsilon";
  }
}
;// ./src/antlr4/atn/Predicate.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class Predicate extends SemanticContext {
  constructor(ruleIndex, predIndex, isCtxDependent) {
    super();
    this.ruleIndex = ruleIndex === undefined ? -1 : ruleIndex;
    this.predIndex = predIndex === undefined ? -1 : predIndex;
    this.isCtxDependent = isCtxDependent === undefined ? false : isCtxDependent; // e.g., $i ref in pred
  }
  evaluate(parser, outerContext) {
    let localctx = this.isCtxDependent ? outerContext : null;
    return parser.sempred(localctx, this.ruleIndex, this.predIndex);
  }
  updateHashCode(hash) {
    hash.update(this.ruleIndex, this.predIndex, this.isCtxDependent);
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof Predicate)) {
      return false;
    } else {
      return this.ruleIndex === other.ruleIndex && this.predIndex === other.predIndex && this.isCtxDependent === other.isCtxDependent;
    }
  }
  toString() {
    return "{" + this.ruleIndex + ":" + this.predIndex + "}?";
  }
}

/**
 * The default {@link SemanticContext}, which is semantically equivalent to
 * a predicate of the form {@code {true}?}
 */
SemanticContext.NONE = new Predicate();
;// ./src/antlr4/transition/PredicateTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



class PredicateTransition extends AbstractPredicateTransition {
  constructor(target, ruleIndex, predIndex, isCtxDependent) {
    super(target);
    this.serializationType = Transition.PREDICATE;
    this.ruleIndex = ruleIndex;
    this.predIndex = predIndex;
    this.isCtxDependent = isCtxDependent; // e.g., $i ref in pred
    this.isEpsilon = true;
  }
  matches(symbol, minVocabSymbol, maxVocabSymbol) {
    return false;
  }
  getPredicate() {
    return new Predicate(this.ruleIndex, this.predIndex, this.isCtxDependent);
  }
  toString() {
    return "pred_" + this.ruleIndex + ":" + this.predIndex;
  }
}
;// ./src/antlr4/atn/PrecedencePredicate.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class PrecedencePredicate extends SemanticContext {
  constructor(precedence) {
    super();
    this.precedence = precedence === undefined ? 0 : precedence;
  }
  evaluate(parser, outerContext) {
    return parser.precpred(outerContext, this.precedence);
  }
  evalPrecedence(parser, outerContext) {
    if (parser.precpred(outerContext, this.precedence)) {
      return SemanticContext.NONE;
    } else {
      return null;
    }
  }
  compareTo(other) {
    return this.precedence - other.precedence;
  }
  updateHashCode(hash) {
    hash.update(this.precedence);
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof PrecedencePredicate)) {
      return false;
    } else {
      return this.precedence === other.precedence;
    }
  }
  toString() {
    return "{" + this.precedence + ">=prec}?";
  }
}

// HORRIBLE workaround circular import, avoiding dynamic import
SemanticContext.PrecedencePredicate = PrecedencePredicate;
;// ./src/antlr4/transition/PrecedencePredicateTransition.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



class PrecedencePredicateTransition extends AbstractPredicateTransition {
  constructor(target, precedence) {
    super(target);
    this.serializationType = Transition.PRECEDENCE;
    this.precedence = precedence;
    this.isEpsilon = true;
  }
  matches(symbol, minVocabSymbol, maxVocabSymbol) {
    return false;
  }
  getPredicate() {
    return new PrecedencePredicate(this.precedence);
  }
  toString() {
    return this.precedence + " >= _p";
  }
}
;// ./src/antlr4/atn/ATNDeserializationOptions.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class ATNDeserializationOptions {
  constructor(copyFrom) {
    if (copyFrom === undefined) {
      copyFrom = null;
    }
    this.readOnly = false;
    this.verifyATN = copyFrom === null ? true : copyFrom.verifyATN;
    this.generateRuleBypassTransitions = copyFrom === null ? false : copyFrom.generateRuleBypassTransitions;
  }
}
ATNDeserializationOptions.defaultOptions = new ATNDeserializationOptions();
ATNDeserializationOptions.defaultOptions.readOnly = true;
;// ./src/antlr4/atn/LexerActionType.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
/* harmony default export */ const LexerActionType = ({
  // The type of a {@link LexerChannelAction} action.
  CHANNEL: 0,
  // The type of a {@link LexerCustomAction} action
  CUSTOM: 1,
  // The type of a {@link LexerModeAction} action.
  MODE: 2,
  //The type of a {@link LexerMoreAction} action.
  MORE: 3,
  //The type of a {@link LexerPopModeAction} action.
  POP_MODE: 4,
  //The type of a {@link LexerPushModeAction} action.
  PUSH_MODE: 5,
  //The type of a {@link LexerSkipAction} action.
  SKIP: 6,
  //The type of a {@link LexerTypeAction} action.
  TYPE: 7
});
;// ./src/antlr4/action/LexerAction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class LexerAction {
  constructor(action) {
    this.actionType = action;
    this.isPositionDependent = false;
  }
  hashCode() {
    let hash = new HashCode();
    this.updateHashCode(hash);
    return hash.finish();
  }
  updateHashCode(hash) {
    hash.update(this.actionType);
  }
  equals(other) {
    return this === other;
  }
}
;// ./src/antlr4/action/LexerSkipAction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Implements the {@code skip} lexer action by calling {@link Lexer//skip}.
 *
 * <p>The {@code skip} command does not have any parameters, so this action is
 * implemented as a singleton instance exposed by {@link //INSTANCE}.</p>
 */
class LexerSkipAction extends LexerAction {
  constructor() {
    super(LexerActionType.SKIP);
  }
  execute(lexer) {
    lexer.skip();
  }
  toString() {
    return "skip";
  }
}

// Provides a singleton instance of this parameterless lexer action.
LexerSkipAction.INSTANCE = new LexerSkipAction();
;// ./src/antlr4/action/LexerChannelAction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Implements the {@code channel} lexer action by calling
 * {@link Lexer//setChannel} with the assigned channel.
 * Constructs a new {@code channel} action with the specified channel value.
 * @param channel The channel value to pass to {@link Lexer//setChannel}
 */
class LexerChannelAction extends LexerAction {
  constructor(channel) {
    super(LexerActionType.CHANNEL);
    this.channel = channel;
  }

  /**
   * <p>This action is implemented by calling {@link Lexer//setChannel} with the
   * value provided by {@link //getChannel}.</p>
   */
  execute(lexer) {
    lexer._channel = this.channel;
  }
  updateHashCode(hash) {
    hash.update(this.actionType, this.channel);
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof LexerChannelAction)) {
      return false;
    } else {
      return this.channel === other.channel;
    }
  }
  toString() {
    return "channel(" + this.channel + ")";
  }
}
;// ./src/antlr4/action/LexerCustomAction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Executes a custom lexer action by calling {@link Recognizer//action} with the
 * rule and action indexes assigned to the custom action. The implementation of
 * a custom action is added to the generated code for the lexer in an override
 * of {@link Recognizer//action} when the grammar is compiled.
 *
 * <p>This class may represent embedded actions created with the <code>{...}</code>
 * syntax in ANTLR 4, as well as actions created for lexer commands where the
 * command argument could not be evaluated when the grammar was compiled.</p>
 */
class LexerCustomAction extends LexerAction {
  /**
   * Constructs a custom lexer action with the specified rule and action
   * indexes.
   *
   * @param ruleIndex The rule index to use for calls to
   * {@link Recognizer//action}.
   * @param actionIndex The action index to use for calls to
   * {@link Recognizer//action}.
   */
  constructor(ruleIndex, actionIndex) {
    super(LexerActionType.CUSTOM);
    this.ruleIndex = ruleIndex;
    this.actionIndex = actionIndex;
    this.isPositionDependent = true;
  }

  /**
   * <p>Custom actions are implemented by calling {@link Lexer//action} with the
   * appropriate rule and action indexes.</p>
   */
  execute(lexer) {
    lexer.action(null, this.ruleIndex, this.actionIndex);
  }
  updateHashCode(hash) {
    hash.update(this.actionType, this.ruleIndex, this.actionIndex);
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof LexerCustomAction)) {
      return false;
    } else {
      return this.ruleIndex === other.ruleIndex && this.actionIndex === other.actionIndex;
    }
  }
}
;// ./src/antlr4/action/LexerMoreAction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Implements the {@code more} lexer action by calling {@link Lexer//more}.
 *
 * <p>The {@code more} command does not have any parameters, so this action is
 * implemented as a singleton instance exposed by {@link //INSTANCE}.</p>
 */
class LexerMoreAction extends LexerAction {
  constructor() {
    super(LexerActionType.MORE);
  }

  /**
   * <p>This action is implemented by calling {@link Lexer//popMode}.</p>
   */
  execute(lexer) {
    lexer.more();
  }
  toString() {
    return "more";
  }
}
LexerMoreAction.INSTANCE = new LexerMoreAction();
;// ./src/antlr4/action/LexerTypeAction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Implements the {@code type} lexer action by calling {@link Lexer//setType}
 * with the assigned type
 */

class LexerTypeAction extends LexerAction {
  constructor(type) {
    super(LexerActionType.TYPE);
    this.type = type;
  }
  execute(lexer) {
    lexer.type = this.type;
  }
  updateHashCode(hash) {
    hash.update(this.actionType, this.type);
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof LexerTypeAction)) {
      return false;
    } else {
      return this.type === other.type;
    }
  }
  toString() {
    return "type(" + this.type + ")";
  }
}
;// ./src/antlr4/action/LexerPushModeAction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Implements the {@code pushMode} lexer action by calling
 * {@link Lexer//pushMode} with the assigned mode
 */
class LexerPushModeAction extends LexerAction {
  constructor(mode) {
    super(LexerActionType.PUSH_MODE);
    this.mode = mode;
  }

  /**
   * <p>This action is implemented by calling {@link Lexer//pushMode} with the
   * value provided by {@link //getMode}.</p>
   */
  execute(lexer) {
    lexer.pushMode(this.mode);
  }
  updateHashCode(hash) {
    hash.update(this.actionType, this.mode);
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof LexerPushModeAction)) {
      return false;
    } else {
      return this.mode === other.mode;
    }
  }
  toString() {
    return "pushMode(" + this.mode + ")";
  }
}
;// ./src/antlr4/action/LexerPopModeAction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Implements the {@code popMode} lexer action by calling {@link Lexer//popMode}.
 *
 * <p>The {@code popMode} command does not have any parameters, so this action is
 * implemented as a singleton instance exposed by {@link //INSTANCE}.</p>
 */
class LexerPopModeAction extends LexerAction {
  constructor() {
    super(LexerActionType.POP_MODE);
  }

  /**
   * <p>This action is implemented by calling {@link Lexer//popMode}.</p>
   */
  execute(lexer) {
    lexer.popMode();
  }
  toString() {
    return "popMode";
  }
}
LexerPopModeAction.INSTANCE = new LexerPopModeAction();
;// ./src/antlr4/action/LexerModeAction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Implements the {@code mode} lexer action by calling {@link Lexer//mode} with
 * the assigned mode
 */
class LexerModeAction extends LexerAction {
  constructor(mode) {
    super(LexerActionType.MODE);
    this.mode = mode;
  }

  /**
   * <p>This action is implemented by calling {@link Lexer//mode} with the
   * value provided by {@link //getMode}.</p>
   */
  execute(lexer) {
    lexer.setMode(this.mode);
  }
  updateHashCode(hash) {
    hash.update(this.actionType, this.mode);
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof LexerModeAction)) {
      return false;
    } else {
      return this.mode === other.mode;
    }
  }
  toString() {
    return "mode(" + this.mode + ")";
  }
}
;// ./src/antlr4/atn/ATNDeserializer.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */









































let SERIALIZED_VERSION = 4;
function initArray(length, value) {
  let tmp = [];
  tmp[length - 1] = value;
  return tmp.map(function (i) {
    return value;
  });
}
class ATNDeserializer {
  constructor(options) {
    if (options === undefined || options === null) {
      options = ATNDeserializationOptions.defaultOptions;
    }
    this.deserializationOptions = options;
    this.stateFactories = null;
    this.actionFactories = null;
  }
  deserialize(data) {
    let legacy = this.reset(data);
    this.checkVersion(legacy);
    if (legacy) this.skipUUID();
    let atn = this.readATN();
    this.readStates(atn, legacy);
    this.readRules(atn, legacy);
    this.readModes(atn);
    let sets = [];
    this.readSets(atn, sets, this.readInt.bind(this));
    if (legacy) this.readSets(atn, sets, this.readInt32.bind(this));
    this.readEdges(atn, sets);
    this.readDecisions(atn);
    this.readLexerActions(atn, legacy);
    this.markPrecedenceDecisions(atn);
    this.verifyATN(atn);
    if (this.deserializationOptions.generateRuleBypassTransitions && atn.grammarType === ATNType.PARSER) {
      this.generateRuleBypassTransitions(atn);
      // re-verify after modification
      this.verifyATN(atn);
    }
    return atn;
  }
  reset(data) {
    let version = data.charCodeAt ? data.charCodeAt(0) : data[0];
    if (version === SERIALIZED_VERSION - 1) {
      let adjust = function (c) {
        let v = c.charCodeAt(0);
        return v > 1 ? v - 2 : v + 65534;
      };
      let temp = data.split("").map(adjust);
      // don't adjust the first value since that's the version number
      temp[0] = data.charCodeAt(0);
      this.data = temp;
      this.pos = 0;
      return true;
    } else {
      this.data = data;
      this.pos = 0;
      return false;
    }
  }
  skipUUID() {
    let count = 0;
    while (count++ < 8) this.readInt();
  }
  checkVersion(legacy) {
    let version = this.readInt();
    if (!legacy && version !== SERIALIZED_VERSION) {
      throw "Could not deserialize ATN with version " + version + " (expected " + SERIALIZED_VERSION + ").";
    }
  }
  readATN() {
    let grammarType = this.readInt();
    let maxTokenType = this.readInt();
    return new ATN(grammarType, maxTokenType);
  }
  readStates(atn, legacy) {
    let j, pair, stateNumber;
    let loopBackStateNumbers = [];
    let endStateNumbers = [];
    let nstates = this.readInt();
    for (let i = 0; i < nstates; i++) {
      let stype = this.readInt();
      // ignore bad type of states
      if (stype === ATNState.INVALID_TYPE) {
        atn.addState(null);
        continue;
      }
      let ruleIndex = this.readInt();
      if (legacy && ruleIndex === 0xFFFF) {
        ruleIndex = -1;
      }
      let s = this.stateFactory(stype, ruleIndex);
      if (stype === ATNState.LOOP_END) {
        // special case
        let loopBackStateNumber = this.readInt();
        loopBackStateNumbers.push([s, loopBackStateNumber]);
      } else if (s instanceof BlockStartState) {
        let endStateNumber = this.readInt();
        endStateNumbers.push([s, endStateNumber]);
      }
      atn.addState(s);
    }
    // delay the assignment of loop back and end states until we know all the
    // state instances have been initialized
    for (j = 0; j < loopBackStateNumbers.length; j++) {
      pair = loopBackStateNumbers[j];
      pair[0].loopBackState = atn.states[pair[1]];
    }
    for (j = 0; j < endStateNumbers.length; j++) {
      pair = endStateNumbers[j];
      pair[0].endState = atn.states[pair[1]];
    }
    let numNonGreedyStates = this.readInt();
    for (j = 0; j < numNonGreedyStates; j++) {
      stateNumber = this.readInt();
      atn.states[stateNumber].nonGreedy = true;
    }
    let numPrecedenceStates = this.readInt();
    for (j = 0; j < numPrecedenceStates; j++) {
      stateNumber = this.readInt();
      atn.states[stateNumber].isPrecedenceRule = true;
    }
  }
  readRules(atn, legacy) {
    let i;
    let nrules = this.readInt();
    if (atn.grammarType === ATNType.LEXER) {
      atn.ruleToTokenType = initArray(nrules, 0);
    }
    atn.ruleToStartState = initArray(nrules, 0);
    for (i = 0; i < nrules; i++) {
      let s = this.readInt();
      atn.ruleToStartState[i] = atn.states[s];
      if (atn.grammarType === ATNType.LEXER) {
        let tokenType = this.readInt();
        if (legacy && tokenType === 0xFFFF) {
          tokenType = Token.EOF;
        }
        atn.ruleToTokenType[i] = tokenType;
      }
    }
    atn.ruleToStopState = initArray(nrules, 0);
    for (i = 0; i < atn.states.length; i++) {
      let state = atn.states[i];
      if (!(state instanceof RuleStopState)) {
        continue;
      }
      atn.ruleToStopState[state.ruleIndex] = state;
      atn.ruleToStartState[state.ruleIndex].stopState = state;
    }
  }
  readModes(atn) {
    let nmodes = this.readInt();
    for (let i = 0; i < nmodes; i++) {
      let s = this.readInt();
      atn.modeToStartState.push(atn.states[s]);
    }
  }
  readSets(atn, sets, reader) {
    let m = this.readInt();
    for (let i = 0; i < m; i++) {
      let iset = new IntervalSet();
      sets.push(iset);
      let n = this.readInt();
      let containsEof = this.readInt();
      if (containsEof !== 0) {
        iset.addOne(-1);
      }
      for (let j = 0; j < n; j++) {
        let i1 = reader();
        let i2 = reader();
        iset.addRange(i1, i2);
      }
    }
  }
  readEdges(atn, sets) {
    let i, j, state, trans, target;
    let nedges = this.readInt();
    for (i = 0; i < nedges; i++) {
      let src = this.readInt();
      let trg = this.readInt();
      let ttype = this.readInt();
      let arg1 = this.readInt();
      let arg2 = this.readInt();
      let arg3 = this.readInt();
      trans = this.edgeFactory(atn, ttype, src, trg, arg1, arg2, arg3, sets);
      let srcState = atn.states[src];
      srcState.addTransition(trans);
    }
    // edges for rule stop states can be derived, so they aren't serialized
    for (i = 0; i < atn.states.length; i++) {
      state = atn.states[i];
      for (j = 0; j < state.transitions.length; j++) {
        let t = state.transitions[j];
        if (!(t instanceof RuleTransition)) {
          continue;
        }
        let outermostPrecedenceReturn = -1;
        if (atn.ruleToStartState[t.target.ruleIndex].isPrecedenceRule) {
          if (t.precedence === 0) {
            outermostPrecedenceReturn = t.target.ruleIndex;
          }
        }
        trans = new EpsilonTransition(t.followState, outermostPrecedenceReturn);
        atn.ruleToStopState[t.target.ruleIndex].addTransition(trans);
      }
    }
    for (i = 0; i < atn.states.length; i++) {
      state = atn.states[i];
      if (state instanceof BlockStartState) {
        // we need to know the end state to set its start state
        if (state.endState === null) {
          throw "IllegalState";
        }
        // block end states can only be associated to a single block start
        // state
        if (state.endState.startState !== null) {
          throw "IllegalState";
        }
        state.endState.startState = state;
      }
      if (state instanceof PlusLoopbackState) {
        for (j = 0; j < state.transitions.length; j++) {
          target = state.transitions[j].target;
          if (target instanceof PlusBlockStartState) {
            target.loopBackState = state;
          }
        }
      } else if (state instanceof StarLoopbackState) {
        for (j = 0; j < state.transitions.length; j++) {
          target = state.transitions[j].target;
          if (target instanceof StarLoopEntryState) {
            target.loopBackState = state;
          }
        }
      }
    }
  }
  readDecisions(atn) {
    let ndecisions = this.readInt();
    for (let i = 0; i < ndecisions; i++) {
      let s = this.readInt();
      let decState = atn.states[s];
      atn.decisionToState.push(decState);
      decState.decision = i;
    }
  }
  readLexerActions(atn, legacy) {
    if (atn.grammarType === ATNType.LEXER) {
      let count = this.readInt();
      atn.lexerActions = initArray(count, null);
      for (let i = 0; i < count; i++) {
        let actionType = this.readInt();
        let data1 = this.readInt();
        if (legacy && data1 === 0xFFFF) {
          data1 = -1;
        }
        let data2 = this.readInt();
        if (legacy && data2 === 0xFFFF) {
          data2 = -1;
        }
        atn.lexerActions[i] = this.lexerActionFactory(actionType, data1, data2);
      }
    }
  }
  generateRuleBypassTransitions(atn) {
    let i;
    let count = atn.ruleToStartState.length;
    for (i = 0; i < count; i++) {
      atn.ruleToTokenType[i] = atn.maxTokenType + i + 1;
    }
    for (i = 0; i < count; i++) {
      this.generateRuleBypassTransition(atn, i);
    }
  }
  generateRuleBypassTransition(atn, idx) {
    let i, state;
    let bypassStart = new BasicBlockStartState();
    bypassStart.ruleIndex = idx;
    atn.addState(bypassStart);
    let bypassStop = new BlockEndState();
    bypassStop.ruleIndex = idx;
    atn.addState(bypassStop);
    bypassStart.endState = bypassStop;
    atn.defineDecisionState(bypassStart);
    bypassStop.startState = bypassStart;
    let excludeTransition = null;
    let endState = null;
    if (atn.ruleToStartState[idx].isPrecedenceRule) {
      // wrap from the beginning of the rule to the StarLoopEntryState
      endState = null;
      for (i = 0; i < atn.states.length; i++) {
        state = atn.states[i];
        if (this.stateIsEndStateFor(state, idx)) {
          endState = state;
          excludeTransition = state.loopBackState.transitions[0];
          break;
        }
      }
      if (excludeTransition === null) {
        throw "Couldn't identify final state of the precedence rule prefix section.";
      }
    } else {
      endState = atn.ruleToStopState[idx];
    }

    // all non-excluded transitions that currently target end state need to
    // target blockEnd instead
    for (i = 0; i < atn.states.length; i++) {
      state = atn.states[i];
      for (let j = 0; j < state.transitions.length; j++) {
        let transition = state.transitions[j];
        if (transition === excludeTransition) {
          continue;
        }
        if (transition.target === endState) {
          transition.target = bypassStop;
        }
      }
    }

    // all transitions leaving the rule start state need to leave blockStart
    // instead
    let ruleToStartState = atn.ruleToStartState[idx];
    let count = ruleToStartState.transitions.length;
    while (count > 0) {
      bypassStart.addTransition(ruleToStartState.transitions[count - 1]);
      ruleToStartState.transitions = ruleToStartState.transitions.slice(-1);
    }
    // link the new states
    atn.ruleToStartState[idx].addTransition(new EpsilonTransition(bypassStart));
    bypassStop.addTransition(new EpsilonTransition(endState));
    let matchState = new BasicState();
    atn.addState(matchState);
    matchState.addTransition(new AtomTransition(bypassStop, atn.ruleToTokenType[idx]));
    bypassStart.addTransition(new EpsilonTransition(matchState));
  }
  stateIsEndStateFor(state, idx) {
    if (state.ruleIndex !== idx) {
      return null;
    }
    if (!(state instanceof StarLoopEntryState)) {
      return null;
    }
    let maybeLoopEndState = state.transitions[state.transitions.length - 1].target;
    if (!(maybeLoopEndState instanceof LoopEndState)) {
      return null;
    }
    if (maybeLoopEndState.epsilonOnlyTransitions && maybeLoopEndState.transitions[0].target instanceof RuleStopState) {
      return state;
    } else {
      return null;
    }
  }

  /**
   * Analyze the {@link StarLoopEntryState} states in the specified ATN to set
   * the {@link StarLoopEntryState//isPrecedenceDecision} field to the
   * correct value.
   * @param atn The ATN.
   */
  markPrecedenceDecisions(atn) {
    for (let i = 0; i < atn.states.length; i++) {
      let state = atn.states[i];
      if (!(state instanceof StarLoopEntryState)) {
        continue;
      }
      // We analyze the ATN to determine if this ATN decision state is the
      // decision for the closure block that determines whether a
      // precedence rule should continue or complete.
      if (atn.ruleToStartState[state.ruleIndex].isPrecedenceRule) {
        let maybeLoopEndState = state.transitions[state.transitions.length - 1].target;
        if (maybeLoopEndState instanceof LoopEndState) {
          if (maybeLoopEndState.epsilonOnlyTransitions && maybeLoopEndState.transitions[0].target instanceof RuleStopState) {
            state.isPrecedenceDecision = true;
          }
        }
      }
    }
  }
  verifyATN(atn) {
    if (!this.deserializationOptions.verifyATN) {
      return;
    }
    // verify assumptions
    for (let i = 0; i < atn.states.length; i++) {
      let state = atn.states[i];
      if (state === null) {
        continue;
      }
      this.checkCondition(state.epsilonOnlyTransitions || state.transitions.length <= 1);
      if (state instanceof PlusBlockStartState) {
        this.checkCondition(state.loopBackState !== null);
      } else if (state instanceof StarLoopEntryState) {
        this.checkCondition(state.loopBackState !== null);
        this.checkCondition(state.transitions.length === 2);
        if (state.transitions[0].target instanceof StarBlockStartState) {
          this.checkCondition(state.transitions[1].target instanceof LoopEndState);
          this.checkCondition(!state.nonGreedy);
        } else if (state.transitions[0].target instanceof LoopEndState) {
          this.checkCondition(state.transitions[1].target instanceof StarBlockStartState);
          this.checkCondition(state.nonGreedy);
        } else {
          throw "IllegalState";
        }
      } else if (state instanceof StarLoopbackState) {
        this.checkCondition(state.transitions.length === 1);
        this.checkCondition(state.transitions[0].target instanceof StarLoopEntryState);
      } else if (state instanceof LoopEndState) {
        this.checkCondition(state.loopBackState !== null);
      } else if (state instanceof RuleStartState) {
        this.checkCondition(state.stopState !== null);
      } else if (state instanceof BlockStartState) {
        this.checkCondition(state.endState !== null);
      } else if (state instanceof BlockEndState) {
        this.checkCondition(state.startState !== null);
      } else if (state instanceof DecisionState) {
        this.checkCondition(state.transitions.length <= 1 || state.decision >= 0);
      } else {
        this.checkCondition(state.transitions.length <= 1 || state instanceof RuleStopState);
      }
    }
  }
  checkCondition(condition, message) {
    if (!condition) {
      if (message === undefined || message === null) {
        message = "IllegalState";
      }
      throw message;
    }
  }
  readInt() {
    return this.data[this.pos++];
  }
  readInt32() {
    let low = this.readInt();
    let high = this.readInt();
    return low | high << 16;
  }
  edgeFactory(atn, type, src, trg, arg1, arg2, arg3, sets) {
    let target = atn.states[trg];
    switch (type) {
      case Transition.EPSILON:
        return new EpsilonTransition(target);
      case Transition.RANGE:
        return arg3 !== 0 ? new RangeTransition(target, Token.EOF, arg2) : new RangeTransition(target, arg1, arg2);
      case Transition.RULE:
        return new RuleTransition(atn.states[arg1], arg2, arg3, target);
      case Transition.PREDICATE:
        return new PredicateTransition(target, arg1, arg2, arg3 !== 0);
      case Transition.PRECEDENCE:
        return new PrecedencePredicateTransition(target, arg1);
      case Transition.ATOM:
        return arg3 !== 0 ? new AtomTransition(target, Token.EOF) : new AtomTransition(target, arg1);
      case Transition.ACTION:
        return new ActionTransition(target, arg1, arg2, arg3 !== 0);
      case Transition.SET:
        return new SetTransition(target, sets[arg1]);
      case Transition.NOT_SET:
        return new NotSetTransition(target, sets[arg1]);
      case Transition.WILDCARD:
        return new WildcardTransition(target);
      default:
        throw "The specified transition type: " + type + " is not valid.";
    }
  }
  stateFactory(type, ruleIndex) {
    if (this.stateFactories === null) {
      let sf = [];
      sf[ATNState.INVALID_TYPE] = null;
      sf[ATNState.BASIC] = () => new BasicState();
      sf[ATNState.RULE_START] = () => new RuleStartState();
      sf[ATNState.BLOCK_START] = () => new BasicBlockStartState();
      sf[ATNState.PLUS_BLOCK_START] = () => new PlusBlockStartState();
      sf[ATNState.STAR_BLOCK_START] = () => new StarBlockStartState();
      sf[ATNState.TOKEN_START] = () => new TokensStartState();
      sf[ATNState.RULE_STOP] = () => new RuleStopState();
      sf[ATNState.BLOCK_END] = () => new BlockEndState();
      sf[ATNState.STAR_LOOP_BACK] = () => new StarLoopbackState();
      sf[ATNState.STAR_LOOP_ENTRY] = () => new StarLoopEntryState();
      sf[ATNState.PLUS_LOOP_BACK] = () => new PlusLoopbackState();
      sf[ATNState.LOOP_END] = () => new LoopEndState();
      this.stateFactories = sf;
    }
    if (type > this.stateFactories.length || this.stateFactories[type] === null) {
      throw "The specified state type " + type + " is not valid.";
    } else {
      let s = this.stateFactories[type]();
      if (s !== null) {
        s.ruleIndex = ruleIndex;
        return s;
      }
    }
  }
  lexerActionFactory(type, data1, data2) {
    if (this.actionFactories === null) {
      let af = [];
      af[LexerActionType.CHANNEL] = (data1, data2) => new LexerChannelAction(data1);
      af[LexerActionType.CUSTOM] = (data1, data2) => new LexerCustomAction(data1, data2);
      af[LexerActionType.MODE] = (data1, data2) => new LexerModeAction(data1);
      af[LexerActionType.MORE] = (data1, data2) => LexerMoreAction.INSTANCE;
      af[LexerActionType.POP_MODE] = (data1, data2) => LexerPopModeAction.INSTANCE;
      af[LexerActionType.PUSH_MODE] = (data1, data2) => new LexerPushModeAction(data1);
      af[LexerActionType.SKIP] = (data1, data2) => LexerSkipAction.INSTANCE;
      af[LexerActionType.TYPE] = (data1, data2) => new LexerTypeAction(data1);
      this.actionFactories = af;
    }
    if (type > this.actionFactories.length || this.actionFactories[type] === null) {
      throw "The specified lexer action type " + type + " is not valid.";
    } else {
      return this.actionFactories[type](data1, data2);
    }
  }
}
;// ./src/antlr4/error/ErrorListener.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

/**
 * Provides an empty default implementation of {@link ANTLRErrorListener}. The
 * default implementation of each method does nothing, but can be overridden as
 * necessary.
 */
class ErrorListener {
  syntaxError(recognizer, offendingSymbol, line, column, msg, e) {}
  reportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs) {}
  reportAttemptingFullContext(recognizer, dfa, startIndex, stopIndex, conflictingAlts, configs) {}
  reportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, configs) {}
}
;// ./src/antlr4/error/ConsoleErrorListener.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


/**
 * {@inheritDoc}
 *
 * <p>
 * This implementation prints messages to {@link System//err} containing the
 * values of {@code line}, {@code charPositionInLine}, and {@code msg} using
 * the following format.</p>
 *
 * <pre>
 * line <em>line</em>:<em>charPositionInLine</em> <em>msg</em>
 * </pre>
 *
 */
class ConsoleErrorListener extends ErrorListener {
  constructor() {
    super();
  }
  syntaxError(recognizer, offendingSymbol, line, column, msg, e) {
    console.error("line " + line + ":" + column + " " + msg);
  }
}

/**
 * Provides a default instance of {@link ConsoleErrorListener}.
 */
ConsoleErrorListener.INSTANCE = new ConsoleErrorListener();
;// ./src/antlr4/error/ProxyErrorListener.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class ProxyErrorListener extends ErrorListener {
  constructor(delegates) {
    super();
    if (delegates === null) {
      throw "delegates";
    }
    this.delegates = delegates;
    return this;
  }
  syntaxError(recognizer, offendingSymbol, line, column, msg, e) {
    this.delegates.map(d => d.syntaxError(recognizer, offendingSymbol, line, column, msg, e));
  }
  reportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs) {
    this.delegates.map(d => d.reportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs));
  }
  reportAttemptingFullContext(recognizer, dfa, startIndex, stopIndex, conflictingAlts, configs) {
    this.delegates.map(d => d.reportAttemptingFullContext(recognizer, dfa, startIndex, stopIndex, conflictingAlts, configs));
  }
  reportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, configs) {
    this.delegates.map(d => d.reportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, configs));
  }
}
;// ./src/antlr4/Recognizer.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */




class Recognizer {
  constructor() {
    this._listeners = [ConsoleErrorListener.INSTANCE];
    this._interp = null;
    this._stateNumber = -1;
  }
  checkVersion(toolVersion) {
    let runtimeVersion = "4.13.2";
    if (runtimeVersion !== toolVersion) {
      console.log("ANTLR runtime and generated code versions disagree: " + runtimeVersion + "!=" + toolVersion);
    }
  }
  addErrorListener(listener) {
    this._listeners.push(listener);
  }
  removeErrorListeners() {
    this._listeners = [];
  }
  getLiteralNames() {
    return Object.getPrototypeOf(this).constructor.literalNames || [];
  }
  getSymbolicNames() {
    return Object.getPrototypeOf(this).constructor.symbolicNames || [];
  }
  getTokenNames() {
    if (!this.tokenNames) {
      let literalNames = this.getLiteralNames();
      let symbolicNames = this.getSymbolicNames();
      let length = literalNames.length > symbolicNames.length ? literalNames.length : symbolicNames.length;
      this.tokenNames = [];
      for (let i = 0; i < length; i++) {
        this.tokenNames[i] = literalNames[i] || symbolicNames[i] || "<INVALID";
      }
    }
    return this.tokenNames;
  }
  getTokenTypeMap() {
    let tokenNames = this.getTokenNames();
    if (tokenNames === null) {
      throw "The current recognizer does not provide a list of token names.";
    }
    let result = this.tokenTypeMapCache[tokenNames];
    if (result === undefined) {
      result = tokenNames.reduce(function (o, k, i) {
        o[k] = i;
      });
      result.EOF = Token.EOF;
      this.tokenTypeMapCache[tokenNames] = result;
    }
    return result;
  }

  /**
   * Get a map from rule names to rule indexes.
   * <p>Used for XPath and tree pattern compilation.</p>
   */
  getRuleIndexMap() {
    let ruleNames = this.ruleNames;
    if (ruleNames === null) {
      throw "The current recognizer does not provide a list of rule names.";
    }
    let result = this.ruleIndexMapCache[ruleNames]; // todo: should it be Recognizer.ruleIndexMapCache ?
    if (result === undefined) {
      result = ruleNames.reduce(function (o, k, i) {
        o[k] = i;
      });
      this.ruleIndexMapCache[ruleNames] = result;
    }
    return result;
  }
  getTokenType(tokenName) {
    let ttype = this.getTokenTypeMap()[tokenName];
    if (ttype !== undefined) {
      return ttype;
    } else {
      return Token.INVALID_TYPE;
    }
  }

  // What is the error header, normally line/character position information?
  getErrorHeader(e) {
    let line = e.getOffendingToken().line;
    let column = e.getOffendingToken().column;
    return "line " + line + ":" + column;
  }

  /**
   * How should a token be displayed in an error message? The default
   * is to display just the text, but during development you might
   * want to have a lot of information spit out.  Override in that case
   * to use t.toString() (which, for CommonToken, dumps everything about
   * the token). This is better than forcing you to override a method in
   * your token objects because you don't have to go modify your lexer
   * so that it creates a new Java type.
   *
   * @deprecated This method is not called by the ANTLR 4 Runtime. Specific
   * implementations of {@link ANTLRErrorStrategy} may provide a similar
   * feature when necessary. For example, see
   * {@link DefaultErrorStrategy//getTokenErrorDisplay}.*/
  getTokenErrorDisplay(t) {
    if (t === null) {
      return "<no token>";
    }
    let s = t.text;
    if (s === null) {
      if (t.type === Token.EOF) {
        s = "<EOF>";
      } else {
        s = "<" + t.type + ">";
      }
    }
    s = s.replace("\n", "\\n").replace("\r", "\\r").replace("\t", "\\t");
    return "'" + s + "'";
  }

  /**
   * @deprecated since ANTLR 4.13.2; use getErrorListener instead
   */
  getErrorListenerDispatch() {
    console.warn("Calling deprecated method in Recognizer class: getErrorListenerDispatch()");
    return this.getErrorListener();
  }
  getErrorListener() {
    return new ProxyErrorListener(this._listeners);
  }

  /**
   * subclass needs to override these if there are sempreds or actions
   * that the ATN interp needs to execute
   */
  sempred(localctx, ruleIndex, actionIndex) {
    return true;
  }
  precpred(localctx, precedence) {
    return true;
  }
  get atn() {
    return this._interp.atn;
  }
  get state() {
    return this._stateNumber;
  }
  set state(state) {
    this._stateNumber = state;
  }
}
Recognizer.tokenTypeMapCache = {};
Recognizer.ruleIndexMapCache = {};
;// ./src/antlr4/CommonToken.js

class CommonToken extends Token {
  constructor(source, type, channel, start, stop) {
    super();
    this.source = source !== undefined ? source : CommonToken.EMPTY_SOURCE;
    this.type = type !== undefined ? type : null;
    this.channel = channel !== undefined ? channel : Token.DEFAULT_CHANNEL;
    this.start = start !== undefined ? start : -1;
    this.stop = stop !== undefined ? stop : -1;
    this.tokenIndex = -1;
    if (this.source[0] !== null) {
      this.line = source[0].line;
      this.column = source[0].column;
    } else {
      this.column = -1;
    }
  }

  /**
   * Constructs a new {@link CommonToken} as a copy of another {@link Token}.
   *
   * <p>
   * If {@code oldToken} is also a {@link CommonToken} instance, the newly
   * constructed token will share a reference to the {@link //text} field and
   * the {@link Pair} stored in {@link //source}. Otherwise, {@link //text} will
   * be assigned the result of calling {@link //getText}, and {@link //source}
   * will be constructed from the result of {@link Token//getTokenSource} and
   * {@link Token//getInputStream}.</p>
   *
   * @param oldToken The token to copy.
   */
  clone() {
    let t = new CommonToken(this.source, this.type, this.channel, this.start, this.stop);
    t.tokenIndex = this.tokenIndex;
    t.line = this.line;
    t.column = this.column;
    t.text = this.text;
    return t;
  }
  cloneWithType(type) {
    let t = new CommonToken(this.source, type, this.channel, this.start, this.stop);
    t.tokenIndex = this.tokenIndex;
    t.line = this.line;
    t.column = this.column;
    if (type === Token.EOF) t.text = "";
    return t;
  }
  toString() {
    let txt = this.text;
    if (txt !== null) {
      txt = txt.replace(/\n/g, "\\n").replace(/\r/g, "\\r").replace(/\t/g, "\\t");
    } else {
      txt = "<no text>";
    }
    return "[@" + this.tokenIndex + "," + this.start + ":" + this.stop + "='" + txt + "',<" + this.type + ">" + (this.channel > 0 ? ",channel=" + this.channel : "") + "," + this.line + ":" + this.column + "]";
  }
  get text() {
    if (this._text !== null) {
      return this._text;
    }
    let input = this.getInputStream();
    if (input === null) {
      return null;
    }
    let n = input.size;
    if (this.start < n && this.stop < n) {
      return input.getText(this.start, this.stop);
    } else {
      return "<EOF>";
    }
  }
  set text(text) {
    this._text = text;
  }
}

/**
 * An empty {@link Pair} which is used as the default value of
 * {@link //source} for tokens that do not have a source.
 */
CommonToken.EMPTY_SOURCE = [null, null];
;// ./src/antlr4/CommonTokenFactory.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class TokenFactory {}

/**
 * This default implementation of {@link TokenFactory} creates
 * {@link CommonToken} objects.
 */
class CommonTokenFactory extends TokenFactory {
  constructor(copyText) {
    super();
    /**
     * Indicates whether {@link CommonToken//setText} should be called after
     * constructing tokens to explicitly set the text. This is useful for cases
     * where the input stream might not be able to provide arbitrary substrings
     * of text from the input after the lexer creates a token (e.g. the
     * implementation of {@link CharStream//getText} in
     * {@link UnbufferedCharStream} throws an
     * {@link UnsupportedOperationException}). Explicitly setting the token text
     * allows {@link Token//getText} to be called at any time regardless of the
     * input stream implementation.
     *
     * <p>
     * The default value is {@code false} to avoid the performance and memory
     * overhead of copying text for every token unless explicitly requested.</p>
     */
    this.copyText = copyText === undefined ? false : copyText;
  }
  create(source, type, text, channel, start, stop, line, column) {
    let t = new CommonToken(source, type, channel, start, stop);
    t.line = line;
    t.column = column;
    if (text !== null) {
      t.text = text;
    } else if (this.copyText && source[1] !== null) {
      t.text = source[1].getText(start, stop);
    }
    return t;
  }
  createThin(type, text) {
    let t = new CommonToken(null, type);
    t.text = text;
    return t;
  }
}

/**
 * The default {@link CommonTokenFactory} instance.
 *
 * <p>
 * This token factory does not explicitly copy token text when constructing
 * tokens.</p>
 */
CommonTokenFactory.DEFAULT = new CommonTokenFactory();
;// ./src/antlr4/error/RecognitionException.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

/**
 * The root of the ANTLR exception hierarchy. In general, ANTLR tracks just
 *  3 kinds of errors: prediction errors, failed predicate errors, and
 *  mismatched input errors. In each case, the parser knows where it is
 *  in the input, where it is in the ATN, the rule invocation stack,
 *  and what kind of problem occurred.
 */

class RecognitionException extends Error {
  constructor(params) {
    super(params.message);
    if (Error.captureStackTrace) Error.captureStackTrace(this, RecognitionException);
    this.message = params.message;
    this.recognizer = params.recognizer;
    this.input = params.input;
    this.ctx = params.ctx;
    /**
     * The current {@link Token} when an error occurred. Since not all streams
     * support accessing symbols by index, we have to track the {@link Token}
     * instance itself
    */
    this.offendingToken = null;
    /**
     * Get the ATN state number the parser was in at the time the error
     * occurred. For {@link NoViableAltException} and
     * {@link LexerNoViableAltException} exceptions, this is the
     * {@link DecisionState} number. For others, it is the state whose outgoing
     * edge we couldn't match.
     */
    this.offendingState = -1;
    if (this.recognizer !== null) {
      this.offendingState = this.recognizer.state;
    }
  }

  /**
   * Gets the set of input symbols which could potentially follow the
   * previously matched symbol at the time this exception was thrown.
   *
   * <p>If the set of expected tokens is not known and could not be computed,
   * this method returns {@code null}.</p>
   *
   * @return The set of token types that could potentially follow the current
   * state in the ATN, or {@code null} if the information is not available.
   */
  getExpectedTokens() {
    if (this.recognizer !== null) {
      return this.recognizer.atn.getExpectedTokens(this.offendingState, this.ctx);
    } else {
      return null;
    }
  }

  // <p>If the state number is not known, this method returns -1.</p>
  toString() {
    return this.message;
  }
}
;// ./src/antlr4/error/LexerNoViableAltException.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class LexerNoViableAltException extends RecognitionException {
  constructor(lexer, input, startIndex, deadEndConfigs) {
    super({
      message: "",
      recognizer: lexer,
      input: input,
      ctx: null
    });
    this.startIndex = startIndex;
    this.deadEndConfigs = deadEndConfigs;
  }
  toString() {
    let symbol = "";
    if (this.startIndex >= 0 && this.startIndex < this.input.size) {
      symbol = this.input.getText(new Interval(this.startIndex, this.startIndex));
    }
    return "LexerNoViableAltException" + symbol;
  }
}
;// ./src/antlr4/Lexer.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */







/**
 * A lexer is recognizer that draws input symbols from a character stream.
 * lexer grammars result in a subclass of this object. A Lexer object
 * uses simplified match() and error recovery mechanisms in the interest of speed.
 */
class Lexer extends Recognizer {
  constructor(input) {
    super();
    this._input = input;
    this._factory = CommonTokenFactory.DEFAULT;
    this._tokenFactorySourcePair = [this, input];
    this._interp = null; // child classes must populate this

    /**
     * The goal of all lexer rules/methods is to create a token object.
     * this is an instance variable as multiple rules may collaborate to
     * create a single token. nextToken will return this object after
     * matching lexer rule(s). If you subclass to allow multiple token
     * emissions, then set this to the last token to be matched or
     * something nonnull so that the auto token emit mechanism will not
     * emit another token.
     */
    this._token = null;

    /**
     * What character index in the stream did the current token start at?
     * Needed, for example, to get the text for current token. Set at
     * the start of nextToken.
     */
    this._tokenStartCharIndex = -1;

    // The line on which the first character of the token resides///
    this._tokenStartLine = -1;

    // The character position of first character within the line///
    this._tokenStartColumn = -1;

    // Once we see EOF on char stream, next token will be EOF.
    // If you have DONE : EOF ; then you see DONE EOF.
    this._hitEOF = false;

    // The channel number for the current token///
    this._channel = Token.DEFAULT_CHANNEL;

    // The token type for the current token///
    this._type = Token.INVALID_TYPE;
    this._modeStack = [];
    this._mode = Lexer.DEFAULT_MODE;

    /**
     * You can set the text for the current token to override what is in
     * the input char buffer. Use setText() or can set this instance var.
     */
    this._text = null;
  }
  reset() {
    // wack Lexer state variables
    if (this._input !== null) {
      this._input.seek(0); // rewind the input
    }
    this._token = null;
    this._type = Token.INVALID_TYPE;
    this._channel = Token.DEFAULT_CHANNEL;
    this._tokenStartCharIndex = -1;
    this._tokenStartColumn = -1;
    this._tokenStartLine = -1;
    this._text = null;
    this._hitEOF = false;
    this._mode = Lexer.DEFAULT_MODE;
    this._modeStack = [];
    this._interp.reset();
  }

  // Return a token from this source; i.e., match a token on the char stream.
  nextToken() {
    if (this._input === null) {
      throw "nextToken requires a non-null input stream.";
    }

    /**
     * Mark start location in char stream so unbuffered streams are
     * guaranteed at least have text of current token
     */
    let tokenStartMarker = this._input.mark();
    try {
      for (;;) {
        if (this._hitEOF) {
          this.emitEOF();
          return this._token;
        }
        this._token = null;
        this._channel = Token.DEFAULT_CHANNEL;
        this._tokenStartCharIndex = this._input.index;
        this._tokenStartColumn = this._interp.column;
        this._tokenStartLine = this._interp.line;
        this._text = null;
        let continueOuter = false;
        for (;;) {
          this._type = Token.INVALID_TYPE;
          let ttype = Lexer.SKIP;
          try {
            ttype = this._interp.match(this._input, this._mode);
          } catch (e) {
            if (e instanceof RecognitionException) {
              this.notifyListeners(e); // report error
              this.recover(e);
            } else {
              console.log(e.stack);
              throw e;
            }
          }
          if (this._input.LA(1) === Token.EOF) {
            this._hitEOF = true;
          }
          if (this._type === Token.INVALID_TYPE) {
            this._type = ttype;
          }
          if (this._type === Lexer.SKIP) {
            continueOuter = true;
            break;
          }
          if (this._type !== Lexer.MORE) {
            break;
          }
        }
        if (continueOuter) {
          continue;
        }
        if (this._token === null) {
          this.emit();
        }
        return this._token;
      }
    } finally {
      // make sure we release marker after match or
      // unbuffered char stream will keep buffering
      this._input.release(tokenStartMarker);
    }
  }

  /**
   * Instruct the lexer to skip creating a token for current lexer rule
   * and look for another token. nextToken() knows to keep looking when
   * a lexer rule finishes with token set to SKIP_TOKEN. Recall that
   * if token==null at end of any token rule, it creates one for you
   * and emits it.
   */
  skip() {
    this._type = Lexer.SKIP;
  }
  more() {
    this._type = Lexer.MORE;
  }

  /**
   * @deprecated since ANTLR 4.13.2; use setMode instead
   */
  mode(m) {
    console.warn("Calling deprecated method in Lexer class: mode(...)");
    this.setMode(m);
  }
  setMode(m) {
    this._mode = m;
  }
  getMode() {
    return this._mode;
  }
  getModeStack() {
    return this._modeStack;
  }
  pushMode(m) {
    if (this._interp.debug) {
      console.log("pushMode " + m);
    }
    this._modeStack.push(this._mode);
    this.setMode(m);
  }
  popMode() {
    if (this._modeStack.length === 0) {
      throw "Empty Stack";
    }
    if (this._interp.debug) {
      console.log("popMode back to " + this._modeStack.slice(0, -1));
    }
    this.setMode(this._modeStack.pop());
    return this._mode;
  }

  /**
   * By default does not support multiple emits per nextToken invocation
   * for efficiency reasons. Subclass and override this method, nextToken,
   * and getToken (to push tokens into a list and pull from that list
   * rather than a single variable as this implementation does).
   */
  emitToken(token) {
    this._token = token;
  }

  /**
   * The standard method called to automatically emit a token at the
   * outermost lexical rule. The token object should point into the
   * char buffer start..stop. If there is a text override in 'text',
   * use that to set the token's text. Override this method to emit
   * custom Token objects or provide a new factory.
   */
  emit() {
    let t = this._factory.create(this._tokenFactorySourcePair, this._type, this._text, this._channel, this._tokenStartCharIndex, this.getCharIndex() - 1, this._tokenStartLine, this._tokenStartColumn);
    this.emitToken(t);
    return t;
  }
  emitEOF() {
    let cpos = this.column;
    let lpos = this.line;
    let eof = this._factory.create(this._tokenFactorySourcePair, Token.EOF, null, Token.DEFAULT_CHANNEL, this._input.index, this._input.index - 1, lpos, cpos);
    this.emitToken(eof);
    return eof;
  }

  // What is the index of the current character of lookahead?///
  getCharIndex() {
    return this._input.index;
  }

  /**
   * Return a list of all Token objects in input char stream.
   * Forces load of all tokens. Does not include EOF token.
   */
  getAllTokens() {
    let tokens = [];
    let t = this.nextToken();
    while (t.type !== Token.EOF) {
      tokens.push(t);
      t = this.nextToken();
    }
    return tokens;
  }
  notifyListeners(e) {
    let start = this._tokenStartCharIndex;
    let stop = this._input.index;
    let text = this._input.getText(start, stop);
    let msg = "token recognition error at: '" + this.getErrorDisplay(text) + "'";
    let listener = this.getErrorListener();
    listener.syntaxError(this, null, this._tokenStartLine, this._tokenStartColumn, msg, e);
  }
  getErrorDisplay(s) {
    let d = [];
    for (let i = 0; i < s.length; i++) {
      d.push(s[i]);
    }
    return d.join('');
  }
  getErrorDisplayForChar(c) {
    if (c.charCodeAt(0) === Token.EOF) {
      return "<EOF>";
    } else if (c === '\n') {
      return "\\n";
    } else if (c === '\t') {
      return "\\t";
    } else if (c === '\r') {
      return "\\r";
    } else {
      return c;
    }
  }
  getCharErrorDisplay(c) {
    return "'" + this.getErrorDisplayForChar(c) + "'";
  }

  /**
   * Lexers can normally match any char in it's vocabulary after matching
   * a token, so do the easy thing and just kill a character and hope
   * it all works out. You can instead use the rule invocation stack
   * to do sophisticated error recovery if you are in a fragment rule.
   */
  recover(re) {
    if (this._input.LA(1) !== Token.EOF) {
      if (re instanceof LexerNoViableAltException) {
        // skip a char and try again
        this._interp.consume(this._input);
      } else {
        // TODO: Do we lose character or line position information?
        this._input.consume();
      }
    }
  }
  get inputStream() {
    return this._input;
  }
  set inputStream(input) {
    this._input = null;
    this._tokenFactorySourcePair = [this, this._input];
    this.reset();
    this._input = input;
    this._tokenFactorySourcePair = [this, this._input];
  }
  get sourceName() {
    return this._input.sourceName;
  }
  get type() {
    return this._type;
  }
  set type(type) {
    this._type = type;
  }
  get line() {
    return this._interp.line;
  }
  set line(line) {
    this._interp.line = line;
  }
  get column() {
    return this._interp.column;
  }
  set column(column) {
    this._interp.column = column;
  }
  get text() {
    if (this._text !== null) {
      return this._text;
    } else {
      return this._interp.getText(this._input);
    }
  }
  set text(text) {
    this._text = text;
  }
}
Lexer.DEFAULT_MODE = 0;
Lexer.MORE = -2;
Lexer.SKIP = -3;
Lexer.DEFAULT_TOKEN_CHANNEL = Token.DEFAULT_CHANNEL;
Lexer.HIDDEN = Token.HIDDEN_CHANNEL;
Lexer.MIN_CHAR_VALUE = 0x0000;
Lexer.MAX_CHAR_VALUE = 0x10FFFF;
;// ./src/antlr4/atn/ATNConfigSet.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */








function hashATNConfig(c) {
  return c.hashCodeForConfigSet();
}
function equalATNConfigs(a, b) {
  if (a === b) {
    return true;
  } else if (a === null || b === null) {
    return false;
  } else return a.equalsForConfigSet(b);
}

/**
 * Specialized {@link Set}{@code <}{@link ATNConfig}{@code >} that can track
 * info about the set, with support for combining similar configurations using a
 * graph-structured stack
 */
class ATNConfigSet {
  constructor(fullCtx) {
    /**
     * The reason that we need this is because we don't want the hash map to use
     * the standard hash code and equals. We need all configurations with the
     * same
     * {@code (s,i,_,semctx)} to be equal. Unfortunately, this key effectively
     * doubles
     * the number of objects associated with ATNConfigs. The other solution is
     * to
     * use a hash table that lets us specify the equals/hashcode operation.
     * All configs but hashed by (s, i, _, pi) not including context. Wiped out
     * when we go readonly as this set becomes a DFA state
     */
    this.configLookup = new HashSet(hashATNConfig, equalATNConfigs);
    /**
     * Indicates that this configuration set is part of a full context
     * LL prediction. It will be used to determine how to merge $. With SLL
     * it's a wildcard whereas it is not for LL context merge
     */
    this.fullCtx = fullCtx === undefined ? true : fullCtx;
    /**
     * Indicates that the set of configurations is read-only. Do not
     * allow any code to manipulate the set; DFA states will point at
     * the sets and they must not change. This does not protect the other
     * fields; in particular, conflictingAlts is set after
     * we've made this readonly
     */
    this.readOnly = false;
    // Track the elements as they are added to the set; supports get(i)///
    this.configs = [];

    // TODO: these fields make me pretty uncomfortable but nice to pack up info
    // together, saves recomputation
    // TODO: can we track conflicts as they are added to save scanning configs
    // later?
    this.uniqueAlt = 0;
    this.conflictingAlts = null;

    /**
     * Used in parser and lexer. In lexer, it indicates we hit a pred
     * while computing a closure operation. Don't make a DFA state from this
     */
    this.hasSemanticContext = false;
    this.dipsIntoOuterContext = false;
    this.cachedHashCode = -1;
  }

  /**
   * Adding a new config means merging contexts with existing configs for
   * {@code (s, i, pi, _)}, where {@code s} is the
   * {@link ATNConfig//state}, {@code i} is the {@link ATNConfig//alt}, and
   * {@code pi} is the {@link ATNConfig//semanticContext}. We use
   * {@code (s,i,pi)} as key.
   *
   * <p>This method updates {@link //dipsIntoOuterContext} and
   * {@link //hasSemanticContext} when necessary.</p>
   */
  add(config, mergeCache) {
    if (mergeCache === undefined) {
      mergeCache = null;
    }
    if (this.readOnly) {
      throw "This set is readonly";
    }
    if (config.semanticContext !== SemanticContext.NONE) {
      this.hasSemanticContext = true;
    }
    if (config.reachesIntoOuterContext > 0) {
      this.dipsIntoOuterContext = true;
    }
    let existing = this.configLookup.getOrAdd(config);
    if (existing === config) {
      this.cachedHashCode = -1;
      this.configs.push(config); // track order here
      return true;
    }
    // a previous (s,i,pi,_), merge with it and save result
    let rootIsWildcard = !this.fullCtx;
    let merged = merge(existing.context, config.context, rootIsWildcard, mergeCache);
    /**
     * no need to check for existing.context, config.context in cache
     * since only way to create new graphs is "call rule" and here. We
     * cache at both places
     */
    existing.reachesIntoOuterContext = Math.max(existing.reachesIntoOuterContext, config.reachesIntoOuterContext);
    // make sure to preserve the precedence filter suppression during the merge
    if (config.precedenceFilterSuppressed) {
      existing.precedenceFilterSuppressed = true;
    }
    existing.context = merged; // replace context; no need to alt mapping
    return true;
  }
  getStates() {
    let states = new HashSet();
    for (let i = 0; i < this.configs.length; i++) {
      states.add(this.configs[i].state);
    }
    return states;
  }
  getPredicates() {
    let preds = [];
    for (let i = 0; i < this.configs.length; i++) {
      let c = this.configs[i].semanticContext;
      if (c !== SemanticContext.NONE) {
        preds.push(c.semanticContext);
      }
    }
    return preds;
  }
  optimizeConfigs(interpreter) {
    if (this.readOnly) {
      throw "This set is readonly";
    }
    if (this.configLookup.length === 0) {
      return;
    }
    for (let i = 0; i < this.configs.length; i++) {
      let config = this.configs[i];
      config.context = interpreter.getCachedContext(config.context);
    }
  }
  addAll(coll) {
    for (let i = 0; i < coll.length; i++) {
      this.add(coll[i]);
    }
    return false;
  }
  equals(other) {
    return this === other || other instanceof ATNConfigSet && equalArrays(this.configs, other.configs) && this.fullCtx === other.fullCtx && this.uniqueAlt === other.uniqueAlt && this.conflictingAlts === other.conflictingAlts && this.hasSemanticContext === other.hasSemanticContext && this.dipsIntoOuterContext === other.dipsIntoOuterContext;
  }
  hashCode() {
    let hash = new HashCode();
    hash.update(this.configs);
    return hash.finish();
  }
  updateHashCode(hash) {
    if (this.readOnly) {
      if (this.cachedHashCode === -1) {
        this.cachedHashCode = this.hashCode();
      }
      hash.update(this.cachedHashCode);
    } else {
      hash.update(this.hashCode());
    }
  }
  isEmpty() {
    return this.configs.length === 0;
  }
  contains(item) {
    if (this.configLookup === null) {
      throw "This method is not implemented for readonly sets.";
    }
    return this.configLookup.contains(item);
  }
  containsFast(item) {
    if (this.configLookup === null) {
      throw "This method is not implemented for readonly sets.";
    }
    return this.configLookup.containsFast(item);
  }
  clear() {
    if (this.readOnly) {
      throw "This set is readonly";
    }
    this.configs = [];
    this.cachedHashCode = -1;
    this.configLookup = new HashSet();
  }
  setReadonly(readOnly) {
    this.readOnly = readOnly;
    if (readOnly) {
      this.configLookup = null; // can't mod, no need for lookup cache
    }
  }
  toString() {
    return arrayToString(this.configs) + (this.hasSemanticContext ? ",hasSemanticContext=" + this.hasSemanticContext : "") + (this.uniqueAlt !== ATN.INVALID_ALT_NUMBER ? ",uniqueAlt=" + this.uniqueAlt : "") + (this.conflictingAlts !== null ? ",conflictingAlts=" + this.conflictingAlts : "") + (this.dipsIntoOuterContext ? ",dipsIntoOuterContext" : "");
  }
  get items() {
    return this.configs;
  }
  get length() {
    return this.configs.length;
  }
}
;// ./src/antlr4/dfa/DFAState.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */





/**
 * A DFA state represents a set of possible ATN configurations.
 * As Aho, Sethi, Ullman p. 117 says "The DFA uses its state
 * to keep track of all possible states the ATN can be in after
 * reading each input symbol. That is to say, after reading
 * input a1a2..an, the DFA is in a state that represents the
 * subset T of the states of the ATN that are reachable from the
 * ATN's start state along some path labeled a1a2..an."
 * In conventional NFA&rarr;DFA conversion, therefore, the subset T
 * would be a bitset representing the set of states the
 * ATN could be in. We need to track the alt predicted by each
 * state as well, however. More importantly, we need to maintain
 * a stack of states, tracking the closure operations as they
 * jump from rule to rule, emulating rule invocations (method calls).
 * I have to add a stack to simulate the proper lookahead sequences for
 * the underlying LL grammar from which the ATN was derived.
 *
 * <p>I use a set of ATNConfig objects not simple states. An ATNConfig
 * is both a state (ala normal conversion) and a RuleContext describing
 * the chain of rules (if any) followed to arrive at that state.</p>
 *
 * <p>A DFA state may have multiple references to a particular state,
 * but with different ATN contexts (with same or different alts)
 * meaning that state was reached via a different set of rule invocations.</p>
 */
class DFAState {
  constructor(stateNumber, configs) {
    if (stateNumber === null) {
      stateNumber = -1;
    }
    if (configs === null) {
      configs = new ATNConfigSet();
    }
    this.stateNumber = stateNumber;
    this.configs = configs;
    /**
     * {@code edges[symbol]} points to target of symbol. Shift up by 1 so (-1)
     * {@link Token//EOF} maps to {@code edges[0]}.
     */
    this.edges = null;
    this.isAcceptState = false;
    /**
     * if accept state, what ttype do we match or alt do we predict?
     * This is set to {@link ATN//INVALID_ALT_NUMBER} when {@link//predicates}
     * {@code !=null} or {@link //requiresFullContext}.
     */
    this.prediction = 0;
    this.lexerActionExecutor = null;
    /**
     * Indicates that this state was created during SLL prediction that
     * discovered a conflict between the configurations in the state. Future
     * {@link ParserATNSimulator//execATN} invocations immediately jumped doing
     * full context prediction if this field is true.
     */
    this.requiresFullContext = false;
    /**
     * During SLL parsing, this is a list of predicates associated with the
     * ATN configurations of the DFA state. When we have predicates,
     * {@link //requiresFullContext} is {@code false} since full context
     * prediction evaluates predicates
     * on-the-fly. If this is not null, then {@link //prediction} is
     * {@link ATN//INVALID_ALT_NUMBER}.
     *
     * <p>We only use these for non-{@link //requiresFullContext} but
     * conflicting states. That
     * means we know from the context (it's $ or we don't dip into outer
     * context) that it's an ambiguity not a conflict.</p>
     *
     * <p>This list is computed by {@link
     * ParserATNSimulator//predicateDFAState}.</p>
     */
    this.predicates = null;
    return this;
  }

  /**
   * Get the set of all alts mentioned by all ATN configurations in this
   * DFA state.
   */
  getAltSet() {
    let alts = new HashSet();
    if (this.configs !== null) {
      for (let i = 0; i < this.configs.length; i++) {
        let c = this.configs[i];
        alts.add(c.alt);
      }
    }
    if (alts.length === 0) {
      return null;
    } else {
      return alts;
    }
  }

  /**
   * Two {@link DFAState} instances are equal if their ATN configuration sets
   * are the same. This method is used to see if a state already exists.
   *
   * <p>Because the number of alternatives and number of ATN configurations are
   * finite, there is a finite number of DFA states that can be processed.
   * This is necessary to show that the algorithm terminates.</p>
   *
   * <p>Cannot test the DFA state numbers here because in
   * {@link ParserATNSimulator//addDFAState} we need to know if any other state
   * exists that has this exact set of ATN configurations. The
   * {@link //stateNumber} is irrelevant.</p>
   */
  equals(other) {
    // compare set of ATN configurations in this set with other
    return this === other || other instanceof DFAState && this.configs.equals(other.configs);
  }
  toString() {
    let s = "" + this.stateNumber + ":" + this.configs;
    if (this.isAcceptState) {
      s = s + "=>";
      if (this.predicates !== null) s = s + this.predicates;else s = s + this.prediction;
    }
    return s;
  }
  hashCode() {
    let hash = new HashCode();
    hash.update(this.configs);
    return hash.finish();
  }
}
;// ./src/antlr4/atn/ATNSimulator.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */





class ATNSimulator {
  constructor(atn, sharedContextCache) {
    /**
     * The context cache maps all PredictionContext objects that are ==
     * to a single cached copy. This cache is shared across all contexts
     * in all ATNConfigs in all DFA states.  We rebuild each ATNConfigSet
     * to use only cached nodes/graphs in addDFAState(). We don't want to
     * fill this during closure() since there are lots of contexts that
     * pop up but are not used ever again. It also greatly slows down closure().
     *
     * <p>This cache makes a huge difference in memory and a little bit in speed.
     * For the Java grammar on java.*, it dropped the memory requirements
     * at the end from 25M to 16M. We don't store any of the full context
     * graphs in the DFA because they are limited to local context only,
     * but apparently there's a lot of repetition there as well. We optimize
     * the config contexts before storing the config set in the DFA states
     * by literally rebuilding them with cached subgraphs only.</p>
     *
     * <p>I tried a cache for use during closure operations, that was
     * whacked after each adaptivePredict(). It cost a little bit
     * more time I think and doesn't save on the overall footprint
     * so it's not worth the complexity.</p>
     */
    this.atn = atn;
    this.sharedContextCache = sharedContextCache;
    return this;
  }
  getCachedContext(context) {
    if (this.sharedContextCache === null) {
      return context;
    }
    let visited = new HashMap_HashMap();
    return getCachedPredictionContext(context, this.sharedContextCache, visited);
  }
}

// Must distinguish between missing edge and edge we know leads nowhere///
ATNSimulator.ERROR = new DFAState(0x7FFFFFFF, new ATNConfigSet());
;// ./src/antlr4/atn/OrderedATNConfigSet.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class OrderedATNConfigSet extends ATNConfigSet {
  constructor() {
    super();
    this.configLookup = new HashSet();
  }
}
;// ./src/antlr4/atn/LexerATNConfig.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class LexerATNConfig extends ATNConfig {
  constructor(params, config) {
    super(params, config);

    // This is the backing field for {@link //getLexerActionExecutor}.
    let lexerActionExecutor = params.lexerActionExecutor || null;
    this.lexerActionExecutor = lexerActionExecutor || (config !== null ? config.lexerActionExecutor : null);
    this.passedThroughNonGreedyDecision = config !== null ? this.checkNonGreedyDecision(config, this.state) : false;
    this.hashCodeForConfigSet = LexerATNConfig.prototype.hashCode;
    this.equalsForConfigSet = LexerATNConfig.prototype.equals;
    return this;
  }
  updateHashCode(hash) {
    hash.update(this.state.stateNumber, this.alt, this.context, this.semanticContext, this.passedThroughNonGreedyDecision, this.lexerActionExecutor);
  }
  equals(other) {
    return this === other || other instanceof LexerATNConfig && this.passedThroughNonGreedyDecision === other.passedThroughNonGreedyDecision && (this.lexerActionExecutor ? this.lexerActionExecutor.equals(other.lexerActionExecutor) : !other.lexerActionExecutor) && super.equals(other);
  }
  checkNonGreedyDecision(source, target) {
    return source.passedThroughNonGreedyDecision || target instanceof DecisionState && target.nonGreedy;
  }
}
;// ./src/antlr4/action/LexerIndexedCustomAction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
/**
 * This implementation of {@link LexerAction} is used for tracking input offsets
 * for position-dependent actions within a {@link LexerActionExecutor}.
 *
 * <p>This action is not serialized as part of the ATN, and is only required for
 * position-dependent lexer actions which appear at a location other than the
 * end of a rule. For more information about DFA optimizations employed for
 * lexer actions, see {@link LexerActionExecutor//append} and
 * {@link LexerActionExecutor//fixOffsetBeforeMatch}.</p>
 *
 * Constructs a new indexed custom action by associating a character offset
 * with a {@link LexerAction}.
 *
 * <p>Note: This class is only required for lexer actions for which
 * {@link LexerAction//isPositionDependent} returns {@code true}.</p>
 *
 * @param offset The offset into the input {@link CharStream}, relative to
 * the token start index, at which the specified lexer action should be
 * executed.
 * @param action The lexer action to execute at a particular offset in the
 * input {@link CharStream}.
 */

class LexerIndexedCustomAction extends LexerAction {
  constructor(offset, action) {
    super(action.actionType);
    this.offset = offset;
    this.action = action;
    this.isPositionDependent = true;
  }

  /**
   * <p>This method calls {@link //execute} on the result of {@link //getAction}
   * using the provided {@code lexer}.</p>
   */
  execute(lexer) {
    // assume the input stream position was properly set by the calling code
    this.action.execute(lexer);
  }
  updateHashCode(hash) {
    hash.update(this.actionType, this.offset, this.action);
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof LexerIndexedCustomAction)) {
      return false;
    } else {
      return this.offset === other.offset && this.action === other.action;
    }
  }
}
;// ./src/antlr4/atn/LexerActionExecutor.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



class LexerActionExecutor {
  /**
   * Represents an executor for a sequence of lexer actions which traversed during
   * the matching operation of a lexer rule (token).
   *
   * <p>The executor tracks position information for position-dependent lexer actions
   * efficiently, ensuring that actions appearing only at the end of the rule do
   * not cause bloating of the {@link DFA} created for the lexer.</p>
   */
  constructor(lexerActions) {
    this.lexerActions = lexerActions === null ? [] : lexerActions;
    /**
     * Caches the result of {@link //hashCode} since the hash code is an element
     * of the performance-critical {@link LexerATNConfig//hashCode} operation
     */
    this.cachedHashCode = HashCode.hashStuff(lexerActions); // "".join([str(la) for la in
    // lexerActions]))
    return this;
  }

  /**
   * Creates a {@link LexerActionExecutor} which encodes the current offset
   * for position-dependent lexer actions.
   *
   * <p>Normally, when the executor encounters lexer actions where
   * {@link LexerAction//isPositionDependent} returns {@code true}, it calls
   * {@link IntStream//seek} on the input {@link CharStream} to set the input
   * position to the <em>end</em> of the current token. This behavior provides
   * for efficient DFA representation of lexer actions which appear at the end
   * of a lexer rule, even when the lexer rule matches a variable number of
   * characters.</p>
   *
   * <p>Prior to traversing a match transition in the ATN, the current offset
   * from the token start index is assigned to all position-dependent lexer
   * actions which have not already been assigned a fixed offset. By storing
   * the offsets relative to the token start index, the DFA representation of
   * lexer actions which appear in the middle of tokens remains efficient due
   * to sharing among tokens of the same length, regardless of their absolute
   * position in the input stream.</p>
   *
   * <p>If the current executor already has offsets assigned to all
   * position-dependent lexer actions, the method returns {@code this}.</p>
   *
   * @param offset The current offset to assign to all position-dependent
   * lexer actions which do not already have offsets assigned.
   *
   * @return {LexerActionExecutor} A {@link LexerActionExecutor} which stores input stream offsets
   * for all position-dependent lexer actions.
   */
  fixOffsetBeforeMatch(offset) {
    let updatedLexerActions = null;
    for (let i = 0; i < this.lexerActions.length; i++) {
      if (this.lexerActions[i].isPositionDependent && !(this.lexerActions[i] instanceof LexerIndexedCustomAction)) {
        if (updatedLexerActions === null) {
          updatedLexerActions = this.lexerActions.concat([]);
        }
        updatedLexerActions[i] = new LexerIndexedCustomAction(offset, this.lexerActions[i]);
      }
    }
    if (updatedLexerActions === null) {
      return this;
    } else {
      return new LexerActionExecutor(updatedLexerActions);
    }
  }

  /**
   * Execute the actions encapsulated by this executor within the context of a
   * particular {@link Lexer}.
   *
   * <p>This method calls {@link IntStream//seek} to set the position of the
   * {@code input} {@link CharStream} prior to calling
   * {@link LexerAction//execute} on a position-dependent action. Before the
   * method returns, the input position will be restored to the same position
   * it was in when the method was invoked.</p>
   *
   * @param lexer The lexer instance.
   * @param input The input stream which is the source for the current token.
   * When this method is called, the current {@link IntStream//index} for
   * {@code input} should be the start of the following token, i.e. 1
   * character past the end of the current token.
   * @param startIndex The token start index. This value may be passed to
   * {@link IntStream//seek} to set the {@code input} position to the beginning
   * of the token.
   */
  execute(lexer, input, startIndex) {
    let requiresSeek = false;
    let stopIndex = input.index;
    try {
      for (let i = 0; i < this.lexerActions.length; i++) {
        let lexerAction = this.lexerActions[i];
        if (lexerAction instanceof LexerIndexedCustomAction) {
          let offset = lexerAction.offset;
          input.seek(startIndex + offset);
          lexerAction = lexerAction.action;
          requiresSeek = startIndex + offset !== stopIndex;
        } else if (lexerAction.isPositionDependent) {
          input.seek(stopIndex);
          requiresSeek = false;
        }
        lexerAction.execute(lexer);
      }
    } finally {
      if (requiresSeek) {
        input.seek(stopIndex);
      }
    }
  }
  hashCode() {
    return this.cachedHashCode;
  }
  updateHashCode(hash) {
    hash.update(this.cachedHashCode);
  }
  equals(other) {
    if (this === other) {
      return true;
    } else if (!(other instanceof LexerActionExecutor)) {
      return false;
    } else if (this.cachedHashCode != other.cachedHashCode) {
      return false;
    } else if (this.lexerActions.length != other.lexerActions.length) {
      return false;
    } else {
      let numActions = this.lexerActions.length;
      for (let idx = 0; idx < numActions; ++idx) {
        if (!this.lexerActions[idx].equals(other.lexerActions[idx])) {
          return false;
        }
      }
      return true;
    }
  }

  /**
   * Creates a {@link LexerActionExecutor} which executes the actions for
   * the input {@code lexerActionExecutor} followed by a specified
   * {@code lexerAction}.
   *
   * @param lexerActionExecutor The executor for actions already traversed by
   * the lexer while matching a token within a particular
   * {@link LexerATNConfig}. If this is {@code null}, the method behaves as
   * though it were an empty executor.
   * @param lexerAction The lexer action to execute after the actions
   * specified in {@code lexerActionExecutor}.
   *
   * @return {LexerActionExecutor} A {@link LexerActionExecutor} for executing the combine actions
   * of {@code lexerActionExecutor} and {@code lexerAction}.
   */
  static append(lexerActionExecutor, lexerAction) {
    if (lexerActionExecutor === null) {
      return new LexerActionExecutor([lexerAction]);
    }
    let lexerActions = lexerActionExecutor.lexerActions.concat([lexerAction]);
    return new LexerActionExecutor(lexerActions);
  }
}
;// ./src/antlr4/atn/LexerATNSimulator.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */














function resetSimState(sim) {
  sim.index = -1;
  sim.line = 0;
  sim.column = -1;
  sim.dfaState = null;
}
class SimState {
  constructor() {
    resetSimState(this);
  }
  reset() {
    resetSimState(this);
  }
}
class LexerATNSimulator extends ATNSimulator {
  /**
   * When we hit an accept state in either the DFA or the ATN, we
   * have to notify the character stream to start buffering characters
   * via {@link IntStream//mark} and record the current state. The current sim state
   * includes the current index into the input, the current line,
   * and current character position in that line. Note that the Lexer is
   * tracking the starting line and characterization of the token. These
   * variables track the "state" of the simulator when it hits an accept state.
   *
   * <p>We track these variables separately for the DFA and ATN simulation
   * because the DFA simulation often has to fail over to the ATN
   * simulation. If the ATN simulation fails, we need the DFA to fall
   * back to its previously accepted state, if any. If the ATN succeeds,
   * then the ATN does the accept and the DFA simulator that invoked it
   * can simply return the predicted token type.</p>
   */
  constructor(recog, atn, decisionToDFA, sharedContextCache) {
    super(atn, sharedContextCache);
    this.decisionToDFA = decisionToDFA;
    this.recog = recog;
    /**
     * The current token's starting index into the character stream.
     * Shared across DFA to ATN simulation in case the ATN fails and the
     * DFA did not have a previous accept state. In this case, we use the
     * ATN-generated exception object
     */
    this.startIndex = -1;
    // line number 1..n within the input///
    this.line = 1;
    /**
     * The index of the character relative to the beginning of the line
     * 0..n-1
     */
    this.column = 0;
    this.mode = Lexer.DEFAULT_MODE;
    /**
     * Used during DFA/ATN exec to record the most recent accept configuration
     * info
     */
    this.prevAccept = new SimState();
  }
  copyState(simulator) {
    this.column = simulator.column;
    this.line = simulator.line;
    this.mode = simulator.mode;
    this.startIndex = simulator.startIndex;
  }
  match(input, mode) {
    this.mode = mode;
    let mark = input.mark();
    try {
      this.startIndex = input.index;
      this.prevAccept.reset();
      let dfa = this.decisionToDFA[mode];
      if (dfa.s0 === null) {
        return this.matchATN(input);
      } else {
        return this.execATN(input, dfa.s0);
      }
    } finally {
      input.release(mark);
    }
  }
  reset() {
    this.prevAccept.reset();
    this.startIndex = -1;
    this.line = 1;
    this.column = 0;
    this.mode = Lexer.DEFAULT_MODE;
  }
  matchATN(input) {
    let startState = this.atn.modeToStartState[this.mode];
    if (LexerATNSimulator.debug) {
      console.log("matchATN mode " + this.mode + " start: " + startState);
    }
    let old_mode = this.mode;
    let s0_closure = this.computeStartState(input, startState);
    let suppressEdge = s0_closure.hasSemanticContext;
    s0_closure.hasSemanticContext = false;
    let next = this.addDFAState(s0_closure);
    if (!suppressEdge) {
      this.decisionToDFA[this.mode].s0 = next;
    }
    let predict = this.execATN(input, next);
    if (LexerATNSimulator.debug) {
      console.log("DFA after matchATN: " + this.decisionToDFA[old_mode].toLexerString());
    }
    return predict;
  }
  execATN(input, ds0) {
    if (LexerATNSimulator.debug) {
      console.log("start state closure=" + ds0.configs);
    }
    if (ds0.isAcceptState) {
      // allow zero-length tokens
      this.captureSimState(this.prevAccept, input, ds0);
    }
    let t = input.LA(1);
    let s = ds0; // s is current/from DFA state

    for (;;) {
      // while more work
      if (LexerATNSimulator.debug) {
        console.log("execATN loop starting closure: " + s.configs);
      }

      /**
       * As we move src->trg, src->trg, we keep track of the previous trg to
       * avoid looking up the DFA state again, which is expensive.
       * If the previous target was already part of the DFA, we might
       * be able to avoid doing a reach operation upon t. If s!=null,
       * it means that semantic predicates didn't prevent us from
       * creating a DFA state. Once we know s!=null, we check to see if
       * the DFA state has an edge already for t. If so, we can just reuse
       * it's configuration set; there's no point in re-computing it.
       * This is kind of like doing DFA simulation within the ATN
       * simulation because DFA simulation is really just a way to avoid
       * computing reach/closure sets. Technically, once we know that
       * we have a previously added DFA state, we could jump over to
       * the DFA simulator. But, that would mean popping back and forth
       * a lot and making things more complicated algorithmically.
       * This optimization makes a lot of sense for loops within DFA.
       * A character will take us back to an existing DFA state
       * that already has lots of edges out of it. e.g., .* in comments.
       * print("Target for:" + str(s) + " and:" + str(t))
       */
      let target = this.getExistingTargetState(s, t);
      // print("Existing:" + str(target))
      if (target === null) {
        target = this.computeTargetState(input, s, t);
        // print("Computed:" + str(target))
      }
      if (target === ATNSimulator.ERROR) {
        break;
      }
      // If this is a consumable input element, make sure to consume before
      // capturing the accept state so the input index, line, and char
      // position accurately reflect the state of the interpreter at the
      // end of the token.
      if (t !== Token.EOF) {
        this.consume(input);
      }
      if (target.isAcceptState) {
        this.captureSimState(this.prevAccept, input, target);
        if (t === Token.EOF) {
          break;
        }
      }
      t = input.LA(1);
      s = target; // flip; current DFA target becomes new src/from state
    }
    return this.failOrAccept(this.prevAccept, input, s.configs, t);
  }

  /**
   * Get an existing target state for an edge in the DFA. If the target state
   * for the edge has not yet been computed or is otherwise not available,
   * this method returns {@code null}.
   *
   * @param s The current DFA state
   * @param t The next input symbol
   * @return The existing target DFA state for the given input symbol
   * {@code t}, or {@code null} if the target state for this edge is not
   * already cached
   */
  getExistingTargetState(s, t) {
    if (s.edges === null || t < LexerATNSimulator.MIN_DFA_EDGE || t > LexerATNSimulator.MAX_DFA_EDGE) {
      return null;
    }
    let target = s.edges[t - LexerATNSimulator.MIN_DFA_EDGE];
    if (target === undefined) {
      target = null;
    }
    if (LexerATNSimulator.debug && target !== null) {
      console.log("reuse state " + s.stateNumber + " edge to " + target.stateNumber);
    }
    return target;
  }

  /**
   * Compute a target state for an edge in the DFA, and attempt to add the
   * computed state and corresponding edge to the DFA.
   *
   * @param input The input stream
   * @param s The current DFA state
   * @param t The next input symbol
   *
   * @return The computed target DFA state for the given input symbol
   * {@code t}. If {@code t} does not lead to a valid DFA state, this method
   * returns {@link //ERROR}.
   */
  computeTargetState(input, s, t) {
    let reach = new OrderedATNConfigSet();
    // if we don't find an existing DFA state
    // Fill reach starting from closure, following t transitions
    this.getReachableConfigSet(input, s.configs, reach, t);
    if (reach.items.length === 0) {
      // we got nowhere on t from s
      if (!reach.hasSemanticContext) {
        // we got nowhere on t, don't throw out this knowledge; it'd
        // cause a failover from DFA later.
        this.addDFAEdge(s, t, ATNSimulator.ERROR);
      }
      // stop when we can't match any more char
      return ATNSimulator.ERROR;
    }
    // Add an edge from s to target DFA found/created for reach
    return this.addDFAEdge(s, t, null, reach);
  }
  failOrAccept(prevAccept, input, reach, t) {
    if (this.prevAccept.dfaState !== null) {
      let lexerActionExecutor = prevAccept.dfaState.lexerActionExecutor;
      this.accept(input, lexerActionExecutor, this.startIndex, prevAccept.index, prevAccept.line, prevAccept.column);
      return prevAccept.dfaState.prediction;
    } else {
      // if no accept and EOF is first char, return EOF
      if (t === Token.EOF && input.index === this.startIndex) {
        return Token.EOF;
      }
      throw new LexerNoViableAltException(this.recog, input, this.startIndex, reach);
    }
  }

  /**
   * Given a starting configuration set, figure out all ATN configurations
   * we can reach upon input {@code t}. Parameter {@code reach} is a return
   * parameter.
   */
  getReachableConfigSet(input, closure, reach, t) {
    // this is used to skip processing for configs which have a lower priority
    // than a config that already reached an accept state for the same rule
    let skipAlt = ATN.INVALID_ALT_NUMBER;
    for (let i = 0; i < closure.items.length; i++) {
      let cfg = closure.items[i];
      let currentAltReachedAcceptState = cfg.alt === skipAlt;
      if (currentAltReachedAcceptState && cfg.passedThroughNonGreedyDecision) {
        continue;
      }
      if (LexerATNSimulator.debug) {
        console.log("testing %s at %s\n", this.getTokenName(t), cfg.toString(this.recog, true));
      }
      for (let j = 0; j < cfg.state.transitions.length; j++) {
        let trans = cfg.state.transitions[j]; // for each transition
        let target = this.getReachableTarget(trans, t);
        if (target !== null) {
          let lexerActionExecutor = cfg.lexerActionExecutor;
          if (lexerActionExecutor !== null) {
            lexerActionExecutor = lexerActionExecutor.fixOffsetBeforeMatch(input.index - this.startIndex);
          }
          let treatEofAsEpsilon = t === Token.EOF;
          let config = new LexerATNConfig({
            state: target,
            lexerActionExecutor: lexerActionExecutor
          }, cfg);
          if (this.closure(input, config, reach, currentAltReachedAcceptState, true, treatEofAsEpsilon)) {
            // any remaining configs for this alt have a lower priority
            // than the one that just reached an accept state.
            skipAlt = cfg.alt;
          }
        }
      }
    }
  }
  accept(input, lexerActionExecutor, startIndex, index, line, charPos) {
    if (LexerATNSimulator.debug) {
      console.log("ACTION %s\n", lexerActionExecutor);
    }
    // seek to after last char in token
    input.seek(index);
    this.line = line;
    this.column = charPos;
    if (lexerActionExecutor !== null && this.recog !== null) {
      lexerActionExecutor.execute(this.recog, input, startIndex);
    }
  }
  getReachableTarget(trans, t) {
    if (trans.matches(t, 0, Lexer.MAX_CHAR_VALUE)) {
      return trans.target;
    } else {
      return null;
    }
  }
  computeStartState(input, p) {
    let initialContext = PredictionContext.EMPTY;
    let configs = new OrderedATNConfigSet();
    for (let i = 0; i < p.transitions.length; i++) {
      let target = p.transitions[i].target;
      let cfg = new LexerATNConfig({
        state: target,
        alt: i + 1,
        context: initialContext
      }, null);
      this.closure(input, cfg, configs, false, false, false);
    }
    return configs;
  }

  /**
   * Since the alternatives within any lexer decision are ordered by
   * preference, this method stops pursuing the closure as soon as an accept
   * state is reached. After the first accept state is reached by depth-first
   * search from {@code config}, all other (potentially reachable) states for
   * this rule would have a lower priority.
   *
   * @return {Boolean} {@code true} if an accept state is reached, otherwise
   * {@code false}.
   */
  closure(input, config, configs, currentAltReachedAcceptState, speculative, treatEofAsEpsilon) {
    let cfg = null;
    if (LexerATNSimulator.debug) {
      console.log("closure(" + config.toString(this.recog, true) + ")");
    }
    if (config.state instanceof RuleStopState) {
      if (LexerATNSimulator.debug) {
        if (this.recog !== null) {
          console.log("closure at %s rule stop %s\n", this.recog.ruleNames[config.state.ruleIndex], config);
        } else {
          console.log("closure at rule stop %s\n", config);
        }
      }
      if (config.context === null || config.context.hasEmptyPath()) {
        if (config.context === null || config.context.isEmpty()) {
          configs.add(config);
          return true;
        } else {
          configs.add(new LexerATNConfig({
            state: config.state,
            context: PredictionContext.EMPTY
          }, config));
          currentAltReachedAcceptState = true;
        }
      }
      if (config.context !== null && !config.context.isEmpty()) {
        for (let i = 0; i < config.context.length; i++) {
          if (config.context.getReturnState(i) !== PredictionContext.EMPTY_RETURN_STATE) {
            let newContext = config.context.getParent(i); // "pop" return state
            let returnState = this.atn.states[config.context.getReturnState(i)];
            cfg = new LexerATNConfig({
              state: returnState,
              context: newContext
            }, config);
            currentAltReachedAcceptState = this.closure(input, cfg, configs, currentAltReachedAcceptState, speculative, treatEofAsEpsilon);
          }
        }
      }
      return currentAltReachedAcceptState;
    }
    // optimization
    if (!config.state.epsilonOnlyTransitions) {
      if (!currentAltReachedAcceptState || !config.passedThroughNonGreedyDecision) {
        configs.add(config);
      }
    }
    for (let j = 0; j < config.state.transitions.length; j++) {
      let trans = config.state.transitions[j];
      cfg = this.getEpsilonTarget(input, config, trans, configs, speculative, treatEofAsEpsilon);
      if (cfg !== null) {
        currentAltReachedAcceptState = this.closure(input, cfg, configs, currentAltReachedAcceptState, speculative, treatEofAsEpsilon);
      }
    }
    return currentAltReachedAcceptState;
  }

  // side-effect: can alter configs.hasSemanticContext
  getEpsilonTarget(input, config, trans, configs, speculative, treatEofAsEpsilon) {
    let cfg = null;
    if (trans.serializationType === Transition.RULE) {
      let newContext = SingletonPredictionContext.create(config.context, trans.followState.stateNumber);
      cfg = new LexerATNConfig({
        state: trans.target,
        context: newContext
      }, config);
    } else if (trans.serializationType === Transition.PRECEDENCE) {
      throw "Precedence predicates are not supported in lexers.";
    } else if (trans.serializationType === Transition.PREDICATE) {
      // Track traversing semantic predicates. If we traverse,
      // we cannot add a DFA state for this "reach" computation
      // because the DFA would not test the predicate again in the
      // future. Rather than creating collections of semantic predicates
      // like v3 and testing them on prediction, v4 will test them on the
      // fly all the time using the ATN not the DFA. This is slower but
      // semantically it's not used that often. One of the key elements to
      // this predicate mechanism is not adding DFA states that see
      // predicates immediately afterwards in the ATN. For example,

      // a : ID {p1}? | ID {p2}? ;

      // should create the start state for rule 'a' (to save start state
      // competition), but should not create target of ID state. The
      // collection of ATN states the following ID references includes
      // states reached by traversing predicates. Since this is when we
      // test them, we cannot cash the DFA state target of ID.

      if (LexerATNSimulator.debug) {
        console.log("EVAL rule " + trans.ruleIndex + ":" + trans.predIndex);
      }
      configs.hasSemanticContext = true;
      if (this.evaluatePredicate(input, trans.ruleIndex, trans.predIndex, speculative)) {
        cfg = new LexerATNConfig({
          state: trans.target
        }, config);
      }
    } else if (trans.serializationType === Transition.ACTION) {
      if (config.context === null || config.context.hasEmptyPath()) {
        // execute actions anywhere in the start rule for a token.
        //
        // TODO: if the entry rule is invoked recursively, some
        // actions may be executed during the recursive call. The
        // problem can appear when hasEmptyPath() is true but
        // isEmpty() is false. In this case, the config needs to be
        // split into two contexts - one with just the empty path
        // and another with everything but the empty path.
        // Unfortunately, the current algorithm does not allow
        // getEpsilonTarget to return two configurations, so
        // additional modifications are needed before we can support
        // the split operation.
        let lexerActionExecutor = LexerActionExecutor.append(config.lexerActionExecutor, this.atn.lexerActions[trans.actionIndex]);
        cfg = new LexerATNConfig({
          state: trans.target,
          lexerActionExecutor: lexerActionExecutor
        }, config);
      } else {
        // ignore actions in referenced rules
        cfg = new LexerATNConfig({
          state: trans.target
        }, config);
      }
    } else if (trans.serializationType === Transition.EPSILON) {
      cfg = new LexerATNConfig({
        state: trans.target
      }, config);
    } else if (trans.serializationType === Transition.ATOM || trans.serializationType === Transition.RANGE || trans.serializationType === Transition.SET) {
      if (treatEofAsEpsilon) {
        if (trans.matches(Token.EOF, 0, Lexer.MAX_CHAR_VALUE)) {
          cfg = new LexerATNConfig({
            state: trans.target
          }, config);
        }
      }
    }
    return cfg;
  }

  /**
   * Evaluate a predicate specified in the lexer.
   *
   * <p>If {@code speculative} is {@code true}, this method was called before
   * {@link //consume} for the matched character. This method should call
   * {@link //consume} before evaluating the predicate to ensure position
   * sensitive values, including {@link Lexer//getText}, {@link Lexer//getLine},
   * and {@link Lexer//getcolumn}, properly reflect the current
   * lexer state. This method should restore {@code input} and the simulator
   * to the original state before returning (i.e. undo the actions made by the
   * call to {@link //consume}.</p>
   *
   * @param input The input stream.
   * @param ruleIndex The rule containing the predicate.
   * @param predIndex The index of the predicate within the rule.
   * @param speculative {@code true} if the current index in {@code input} is
   * one character before the predicate's location.
   *
   * @return {@code true} if the specified predicate evaluates to
   * {@code true}.
   */
  evaluatePredicate(input, ruleIndex, predIndex, speculative) {
    // assume true if no recognizer was provided
    if (this.recog === null) {
      return true;
    }
    if (!speculative) {
      return this.recog.sempred(null, ruleIndex, predIndex);
    }
    let savedcolumn = this.column;
    let savedLine = this.line;
    let index = input.index;
    let marker = input.mark();
    try {
      this.consume(input);
      return this.recog.sempred(null, ruleIndex, predIndex);
    } finally {
      this.column = savedcolumn;
      this.line = savedLine;
      input.seek(index);
      input.release(marker);
    }
  }
  captureSimState(settings, input, dfaState) {
    settings.index = input.index;
    settings.line = this.line;
    settings.column = this.column;
    settings.dfaState = dfaState;
  }
  addDFAEdge(from_, tk, to, cfgs) {
    if (to === undefined) {
      to = null;
    }
    if (cfgs === undefined) {
      cfgs = null;
    }
    if (to === null && cfgs !== null) {
      // leading to this call, ATNConfigSet.hasSemanticContext is used as a
      // marker indicating dynamic predicate evaluation makes this edge
      // dependent on the specific input sequence, so the static edge in the
      // DFA should be omitted. The target DFAState is still created since
      // execATN has the ability to resynchronize with the DFA state cache
      // following the predicate evaluation step.
      //
      // TJP notes: next time through the DFA, we see a pred again and eval.
      // If that gets us to a previously created (but dangling) DFA
      // state, we can continue in pure DFA mode from there.
      // /
      let suppressEdge = cfgs.hasSemanticContext;
      cfgs.hasSemanticContext = false;
      to = this.addDFAState(cfgs);
      if (suppressEdge) {
        return to;
      }
    }
    // add the edge
    if (tk < LexerATNSimulator.MIN_DFA_EDGE || tk > LexerATNSimulator.MAX_DFA_EDGE) {
      // Only track edges within the DFA bounds
      return to;
    }
    if (LexerATNSimulator.debug) {
      console.log("EDGE " + from_ + " -> " + to + " upon " + tk);
    }
    if (from_.edges === null) {
      // make room for tokens 1..n and -1 masquerading as index 0
      from_.edges = [];
    }
    from_.edges[tk - LexerATNSimulator.MIN_DFA_EDGE] = to; // connect

    return to;
  }

  /**
   * Add a new DFA state if there isn't one with this set of
   * configurations already. This method also detects the first
   * configuration containing an ATN rule stop state. Later, when
   * traversing the DFA, we will know which rule to accept.
   */
  addDFAState(configs) {
    let proposed = new DFAState(null, configs);
    let firstConfigWithRuleStopState = null;
    for (let i = 0; i < configs.items.length; i++) {
      let cfg = configs.items[i];
      if (cfg.state instanceof RuleStopState) {
        firstConfigWithRuleStopState = cfg;
        break;
      }
    }
    if (firstConfigWithRuleStopState !== null) {
      proposed.isAcceptState = true;
      proposed.lexerActionExecutor = firstConfigWithRuleStopState.lexerActionExecutor;
      proposed.prediction = this.atn.ruleToTokenType[firstConfigWithRuleStopState.state.ruleIndex];
    }
    let dfa = this.decisionToDFA[this.mode];
    let existing = dfa.states.get(proposed);
    if (existing !== null) {
      return existing;
    }
    let newState = proposed;
    newState.stateNumber = dfa.states.length;
    configs.setReadonly(true);
    newState.configs = configs;
    dfa.states.add(newState);
    return newState;
  }
  getDFA(mode) {
    return this.decisionToDFA[mode];
  }

  // Get the text matched so far for the current token.
  getText(input) {
    // index is first lookahead char, don't include.
    return input.getText(this.startIndex, input.index - 1);
  }
  consume(input) {
    let curChar = input.LA(1);
    if (curChar === "\n".charCodeAt(0)) {
      this.line += 1;
      this.column = 0;
    } else {
      this.column += 1;
    }
    input.consume();
  }
  getTokenName(tt) {
    if (tt === -1) {
      return "EOF";
    } else {
      return "'" + String.fromCharCode(tt) + "'";
    }
  }
}
LexerATNSimulator.debug = false;
LexerATNSimulator.dfa_debug = false;
LexerATNSimulator.MIN_DFA_EDGE = 0;
LexerATNSimulator.MAX_DFA_EDGE = 127; // forces unicode to stay in ATN
;// ./src/antlr4/dfa/PredPrediction.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
/**
 * Map a predicate to a predicted alternative.
 */
class PredPrediction {
  constructor(pred, alt) {
    this.alt = alt;
    this.pred = pred;
  }
  toString() {
    return "(" + this.pred + ", " + this.alt + ")";
  }
}
;// ./src/antlr4/misc/AltDict.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
class AltDict {
  constructor() {
    this.data = {};
  }
  get(key) {
    return this.data["k-" + key] || null;
  }
  set(key, value) {
    this.data["k-" + key] = value;
  }
  values() {
    return Object.keys(this.data).filter(key => key.startsWith("k-")).map(key => this.data[key], this);
  }
}
;// ./src/antlr4/atn/PredictionMode.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */











/**
 * This enumeration defines the prediction modes available in ANTLR 4 along with
 * utility methods for analyzing configuration sets for conflicts and/or
 * ambiguities.
 */
let PredictionMode = {
  /**
   * The SLL(*) prediction mode. This prediction mode ignores the current
   * parser context when making predictions. This is the fastest prediction
   * mode, and provides correct results for many grammars. This prediction
   * mode is more powerful than the prediction mode provided by ANTLR 3, but
   * may result in syntax errors for grammar and input combinations which are
   * not SLL.
   *
   * <p>
   * When using this prediction mode, the parser will either return a correct
   * parse tree (i.e. the same parse tree that would be returned with the
   * {@link //LL} prediction mode), or it will report a syntax error. If a
   * syntax error is encountered when using the {@link //SLL} prediction mode,
   * it may be due to either an actual syntax error in the input or indicate
   * that the particular combination of grammar and input requires the more
   * powerful {@link //LL} prediction abilities to complete successfully.</p>
   *
   * <p>
   * This prediction mode does not provide any guarantees for prediction
   * behavior for syntactically-incorrect inputs.</p>
   */
  SLL: 0,
  /**
   * The LL(*) prediction mode. This prediction mode allows the current parser
   * context to be used for resolving SLL conflicts that occur during
   * prediction. This is the fastest prediction mode that guarantees correct
   * parse results for all combinations of grammars with syntactically correct
   * inputs.
   *
   * <p>
   * When using this prediction mode, the parser will make correct decisions
   * for all syntactically-correct grammar and input combinations. However, in
   * cases where the grammar is truly ambiguous this prediction mode might not
   * report a precise answer for <em>exactly which</em> alternatives are
   * ambiguous.</p>
   *
   * <p>
   * This prediction mode does not provide any guarantees for prediction
   * behavior for syntactically-incorrect inputs.</p>
   */
  LL: 1,
  /**
   *
   * The LL(*) prediction mode with exact ambiguity detection. In addition to
   * the correctness guarantees provided by the {@link //LL} prediction mode,
   * this prediction mode instructs the prediction algorithm to determine the
   * complete and exact set of ambiguous alternatives for every ambiguous
   * decision encountered while parsing.
   *
   * <p>
   * This prediction mode may be used for diagnosing ambiguities during
   * grammar development. Due to the performance overhead of calculating sets
   * of ambiguous alternatives, this prediction mode should be avoided when
   * the exact results are not necessary.</p>
   *
   * <p>
   * This prediction mode does not provide any guarantees for prediction
   * behavior for syntactically-incorrect inputs.</p>
   */
  LL_EXACT_AMBIG_DETECTION: 2,
  /**
   *
   * Computes the SLL prediction termination condition.
   *
   * <p>
   * This method computes the SLL prediction termination condition for both of
   * the following cases.</p>
   *
   * <ul>
   * <li>The usual SLL+LL fallback upon SLL conflict</li>
   * <li>Pure SLL without LL fallback</li>
   * </ul>
   *
   * <p><strong>COMBINED SLL+LL PARSING</strong></p>
   *
   * <p>When LL-fallback is enabled upon SLL conflict, correct predictions are
   * ensured regardless of how the termination condition is computed by this
   * method. Due to the substantially higher cost of LL prediction, the
   * prediction should only fall back to LL when the additional lookahead
   * cannot lead to a unique SLL prediction.</p>
   *
   * <p>Assuming combined SLL+LL parsing, an SLL configuration set with only
   * conflicting subsets should fall back to full LL, even if the
   * configuration sets don't resolve to the same alternative (e.g.
   * {@code {1,2}} and {@code {3,4}}. If there is at least one non-conflicting
   * configuration, SLL could continue with the hopes that more lookahead will
   * resolve via one of those non-conflicting configurations.</p>
   *
   * <p>Here's the prediction termination rule them: SLL (for SLL+LL parsing)
   * stops when it sees only conflicting configuration subsets. In contrast,
   * full LL keeps going when there is uncertainty.</p>
   *
   * <p><strong>HEURISTIC</strong></p>
   *
   * <p>As a heuristic, we stop prediction when we see any conflicting subset
   * unless we see a state that only has one alternative associated with it.
   * The single-alt-state thing lets prediction continue upon rules like
   * (otherwise, it would admit defeat too soon):</p>
   *
   * <p>{@code [12|1|[], 6|2|[], 12|2|[]]. s : (ID | ID ID?) ';' ;}</p>
   *
   * <p>When the ATN simulation reaches the state before {@code ';'}, it has a
   * DFA state that looks like: {@code [12|1|[], 6|2|[], 12|2|[]]}. Naturally
   * {@code 12|1|[]} and {@code 12|2|[]} conflict, but we cannot stop
   * processing this node because alternative to has another way to continue,
   * via {@code [6|2|[]]}.</p>
   *
   * <p>It also let's us continue for this rule:</p>
   *
   * <p>{@code [1|1|[], 1|2|[], 8|3|[]] a : A | A | A B ;}</p>
   *
   * <p>After matching input A, we reach the stop state for rule A, state 1.
   * State 8 is the state right before B. Clearly alternatives 1 and 2
   * conflict and no amount of further lookahead will separate the two.
   * However, alternative 3 will be able to continue and so we do not stop
   * working on this state. In the previous example, we're concerned with
   * states associated with the conflicting alternatives. Here alt 3 is not
   * associated with the conflicting configs, but since we can continue
   * looking for input reasonably, don't declare the state done.</p>
   *
   * <p><strong>PURE SLL PARSING</strong></p>
   *
   * <p>To handle pure SLL parsing, all we have to do is make sure that we
   * combine stack contexts for configurations that differ only by semantic
   * predicate. From there, we can do the usual SLL termination heuristic.</p>
   *
   * <p><strong>PREDICATES IN SLL+LL PARSING</strong></p>
   *
   * <p>SLL decisions don't evaluate predicates until after they reach DFA stop
   * states because they need to create the DFA cache that works in all
   * semantic situations. In contrast, full LL evaluates predicates collected
   * during start state computation so it can ignore predicates thereafter.
   * This means that SLL termination detection can totally ignore semantic
   * predicates.</p>
   *
   * <p>Implementation-wise, {@link ATNConfigSet} combines stack contexts but not
   * semantic predicate contexts so we might see two configurations like the
   * following.</p>
   *
   * <p>{@code (s, 1, x, {}), (s, 1, x', {p})}</p>
   *
   * <p>Before testing these configurations against others, we have to merge
   * {@code x} and {@code x'} (without modifying the existing configurations).
   * For example, we test {@code (x+x')==x''} when looking for conflicts in
   * the following configurations.</p>
   *
   * <p>{@code (s, 1, x, {}), (s, 1, x', {p}), (s, 2, x'', {})}</p>
   *
   * <p>If the configuration set has predicates (as indicated by
   * {@link ATNConfigSet//hasSemanticContext}), this algorithm makes a copy of
   * the configurations to strip out all of the predicates so that a standard
   * {@link ATNConfigSet} will merge everything ignoring predicates.</p>
   */
  hasSLLConflictTerminatingPrediction: function (mode, configs) {
    // Configs in rule stop states indicate reaching the end of the decision
    // rule (local context) or end of start rule (full context). If all
    // configs meet this condition, then none of the configurations is able
    // to match additional input so we terminate prediction.
    //
    if (PredictionMode.allConfigsInRuleStopStates(configs)) {
      return true;
    }
    // pure SLL mode parsing
    if (mode === PredictionMode.SLL) {
      // Don't bother with combining configs from different semantic
      // contexts if we can fail over to full LL; costs more time
      // since we'll often fail over anyway.
      if (configs.hasSemanticContext) {
        // dup configs, tossing out semantic predicates
        let dup = new ATNConfigSet();
        for (let i = 0; i < configs.items.length; i++) {
          let c = configs.items[i];
          c = new ATNConfig({
            semanticContext: SemanticContext.NONE
          }, c);
          dup.add(c);
        }
        configs = dup;
      }
      // now we have combined contexts for configs with dissimilar preds
    }
    // pure SLL or combined SLL+LL mode parsing
    let altsets = PredictionMode.getConflictingAltSubsets(configs);
    return PredictionMode.hasConflictingAltSet(altsets) && !PredictionMode.hasStateAssociatedWithOneAlt(configs);
  },
  /**
   * Checks if any configuration in {@code configs} is in a
   * {@link RuleStopState}. Configurations meeting this condition have reached
   * the end of the decision rule (local context) or end of start rule (full
   * context).
   *
   * @param configs the configuration set to test
   * @return {@code true} if any configuration in {@code configs} is in a
   * {@link RuleStopState}, otherwise {@code false}
   */
  hasConfigInRuleStopState: function (configs) {
    for (let i = 0; i < configs.items.length; i++) {
      let c = configs.items[i];
      if (c.state instanceof RuleStopState) {
        return true;
      }
    }
    return false;
  },
  /**
   * Checks if all configurations in {@code configs} are in a
   * {@link RuleStopState}. Configurations meeting this condition have reached
   * the end of the decision rule (local context) or end of start rule (full
   * context).
   *
   * @param configs the configuration set to test
   * @return {@code true} if all configurations in {@code configs} are in a
   * {@link RuleStopState}, otherwise {@code false}
   */
  allConfigsInRuleStopStates: function (configs) {
    for (let i = 0; i < configs.items.length; i++) {
      let c = configs.items[i];
      if (!(c.state instanceof RuleStopState)) {
        return false;
      }
    }
    return true;
  },
  /**
   *
   * Full LL prediction termination.
   *
   * <p>Can we stop looking ahead during ATN simulation or is there some
   * uncertainty as to which alternative we will ultimately pick, after
   * consuming more input? Even if there are partial conflicts, we might know
   * that everything is going to resolve to the same minimum alternative. That
   * means we can stop since no more lookahead will change that fact. On the
   * other hand, there might be multiple conflicts that resolve to different
   * minimums. That means we need more look ahead to decide which of those
   * alternatives we should predict.</p>
   *
   * <p>The basic idea is to split the set of configurations {@code C}, into
   * conflicting subsets {@code (s, _, ctx, _)} and singleton subsets with
   * non-conflicting configurations. Two configurations conflict if they have
   * identical {@link ATNConfig//state} and {@link ATNConfig//context} values
   * but different {@link ATNConfig//alt} value, e.g. {@code (s, i, ctx, _)}
   * and {@code (s, j, ctx, _)} for {@code i!=j}.</p>
   *
   * <p>Reduce these configuration subsets to the set of possible alternatives.
   * You can compute the alternative subsets in one pass as follows:</p>
   *
   * <p>{@code A_s,ctx = {i | (s, i, ctx, _)}} for each configuration in
   * {@code C} holding {@code s} and {@code ctx} fixed.</p>
   *
   * <p>Or in pseudo-code, for each configuration {@code c} in {@code C}:</p>
   *
   * <pre>
   * map[c] U= c.{@link ATNConfig//alt alt} // map hash/equals uses s and x, not
   * alt and not pred
   * </pre>
   *
   * <p>The values in {@code map} are the set of {@code A_s,ctx} sets.</p>
   *
   * <p>If {@code |A_s,ctx|=1} then there is no conflict associated with
   * {@code s} and {@code ctx}.</p>
   *
   * <p>Reduce the subsets to singletons by choosing a minimum of each subset. If
   * the union of these alternative subsets is a singleton, then no amount of
   * more lookahead will help us. We will always pick that alternative. If,
   * however, there is more than one alternative, then we are uncertain which
   * alternative to predict and must continue looking for resolution. We may
   * or may not discover an ambiguity in the future, even if there are no
   * conflicting subsets this round.</p>
   *
   * <p>The biggest sin is to terminate early because it means we've made a
   * decision but were uncertain as to the eventual outcome. We haven't used
   * enough lookahead. On the other hand, announcing a conflict too late is no
   * big deal; you will still have the conflict. It's just inefficient. It
   * might even look until the end of file.</p>
   *
   * <p>No special consideration for semantic predicates is required because
   * predicates are evaluated on-the-fly for full LL prediction, ensuring that
   * no configuration contains a semantic context during the termination
   * check.</p>
   *
   * <p><strong>CONFLICTING CONFIGS</strong></p>
   *
   * <p>Two configurations {@code (s, i, x)} and {@code (s, j, x')}, conflict
   * when {@code i!=j} but {@code x=x'}. Because we merge all
   * {@code (s, i, _)} configurations together, that means that there are at
   * most {@code n} configurations associated with state {@code s} for
   * {@code n} possible alternatives in the decision. The merged stacks
   * complicate the comparison of configuration contexts {@code x} and
   * {@code x'}. Sam checks to see if one is a subset of the other by calling
   * merge and checking to see if the merged result is either {@code x} or
   * {@code x'}. If the {@code x} associated with lowest alternative {@code i}
   * is the superset, then {@code i} is the only possible prediction since the
   * others resolve to {@code min(i)} as well. However, if {@code x} is
   * associated with {@code j>i} then at least one stack configuration for
   * {@code j} is not in conflict with alternative {@code i}. The algorithm
   * should keep going, looking for more lookahead due to the uncertainty.</p>
   *
   * <p>For simplicity, I'm doing a equality check between {@code x} and
   * {@code x'} that lets the algorithm continue to consume lookahead longer
   * than necessary. The reason I like the equality is of course the
   * simplicity but also because that is the test you need to detect the
   * alternatives that are actually in conflict.</p>
   *
   * <p><strong>CONTINUE/STOP RULE</strong></p>
   *
   * <p>Continue if union of resolved alternative sets from non-conflicting and
   * conflicting alternative subsets has more than one alternative. We are
   * uncertain about which alternative to predict.</p>
   *
   * <p>The complete set of alternatives, {@code [i for (_,i,_)]}, tells us which
   * alternatives are still in the running for the amount of input we've
   * consumed at this point. The conflicting sets let us to strip away
   * configurations that won't lead to more states because we resolve
   * conflicts to the configuration with a minimum alternate for the
   * conflicting set.</p>
   *
   * <p><strong>CASES</strong></p>
   *
   * <ul>
   *
   * <li>no conflicts and more than 1 alternative in set =&gt; continue</li>
   *
   * <li> {@code (s, 1, x)}, {@code (s, 2, x)}, {@code (s, 3, z)},
   * {@code (s', 1, y)}, {@code (s', 2, y)} yields non-conflicting set
   * {@code {3}} U conflicting sets {@code min({1,2})} U {@code min({1,2})} =
   * {@code {1,3}} =&gt; continue
   * </li>
   *
   * <li>{@code (s, 1, x)}, {@code (s, 2, x)}, {@code (s', 1, y)},
   * {@code (s', 2, y)}, {@code (s'', 1, z)} yields non-conflicting set
   * {@code {1}} U conflicting sets {@code min({1,2})} U {@code min({1,2})} =
   * {@code {1}} =&gt; stop and predict 1</li>
   *
   * <li>{@code (s, 1, x)}, {@code (s, 2, x)}, {@code (s', 1, y)},
   * {@code (s', 2, y)} yields conflicting, reduced sets {@code {1}} U
   * {@code {1}} = {@code {1}} =&gt; stop and predict 1, can announce
   * ambiguity {@code {1,2}}</li>
   *
   * <li>{@code (s, 1, x)}, {@code (s, 2, x)}, {@code (s', 2, y)},
   * {@code (s', 3, y)} yields conflicting, reduced sets {@code {1}} U
   * {@code {2}} = {@code {1,2}} =&gt; continue</li>
   *
   * <li>{@code (s, 1, x)}, {@code (s, 2, x)}, {@code (s', 3, y)},
   * {@code (s', 4, y)} yields conflicting, reduced sets {@code {1}} U
   * {@code {3}} = {@code {1,3}} =&gt; continue</li>
   *
   * </ul>
   *
   * <p><strong>EXACT AMBIGUITY DETECTION</strong></p>
   *
   * <p>If all states report the same conflicting set of alternatives, then we
   * know we have the exact ambiguity set.</p>
   *
   * <p><code>|A_<em>i</em>|&gt;1</code> and
   * <code>A_<em>i</em> = A_<em>j</em></code> for all <em>i</em>, <em>j</em>.</p>
   *
   * <p>In other words, we continue examining lookahead until all {@code A_i}
   * have more than one alternative and all {@code A_i} are the same. If
   * {@code A={{1,2}, {1,3}}}, then regular LL prediction would terminate
   * because the resolved set is {@code {1}}. To determine what the real
   * ambiguity is, we have to know whether the ambiguity is between one and
   * two or one and three so we keep going. We can only stop prediction when
   * we need exact ambiguity detection when the sets look like
   * {@code A={{1,2}}} or {@code {{1,2},{1,2}}}, etc...</p>
   */
  resolvesToJustOneViableAlt: function (altsets) {
    return PredictionMode.getSingleViableAlt(altsets);
  },
  /**
   * Determines if every alternative subset in {@code altsets} contains more
   * than one alternative.
   *
   * @param altsets a collection of alternative subsets
   * @return {@code true} if every {@link BitSet} in {@code altsets} has
   * {@link BitSet//cardinality cardinality} &gt; 1, otherwise {@code false}
   */
  allSubsetsConflict: function (altsets) {
    return !PredictionMode.hasNonConflictingAltSet(altsets);
  },
  /**
   * Determines if any single alternative subset in {@code altsets} contains
   * exactly one alternative.
   *
   * @param altsets a collection of alternative subsets
   * @return {@code true} if {@code altsets} contains a {@link BitSet} with
   * {@link BitSet//cardinality cardinality} 1, otherwise {@code false}
   */
  hasNonConflictingAltSet: function (altsets) {
    for (let i = 0; i < altsets.length; i++) {
      let alts = altsets[i];
      if (alts.length === 1) {
        return true;
      }
    }
    return false;
  },
  /**
   * Determines if any single alternative subset in {@code altsets} contains
   * more than one alternative.
   *
   * @param altsets a collection of alternative subsets
   * @return {@code true} if {@code altsets} contains a {@link BitSet} with
   * {@link BitSet//cardinality cardinality} &gt; 1, otherwise {@code false}
   */
  hasConflictingAltSet: function (altsets) {
    for (let i = 0; i < altsets.length; i++) {
      let alts = altsets[i];
      if (alts.length > 1) {
        return true;
      }
    }
    return false;
  },
  /**
   * Determines if every alternative subset in {@code altsets} is equivalent.
   *
   * @param altsets a collection of alternative subsets
   * @return {@code true} if every member of {@code altsets} is equal to the
   * others, otherwise {@code false}
   */
  allSubsetsEqual: function (altsets) {
    let first = null;
    for (let i = 0; i < altsets.length; i++) {
      let alts = altsets[i];
      if (first === null) {
        first = alts;
      } else if (alts !== first) {
        return false;
      }
    }
    return true;
  },
  /**
   * Returns the unique alternative predicted by all alternative subsets in
   * {@code altsets}. If no such alternative exists, this method returns
   * {@link ATN//INVALID_ALT_NUMBER}.
   *
   * @param altsets a collection of alternative subsets
   */
  getUniqueAlt: function (altsets) {
    let all = PredictionMode.getAlts(altsets);
    if (all.length === 1) {
      return all.minValue();
    } else {
      return ATN.INVALID_ALT_NUMBER;
    }
  },
  /**
   * Gets the complete set of represented alternatives for a collection of
   * alternative subsets. This method returns the union of each {@link BitSet}
   * in {@code altsets}.
   *
   * @param altsets a collection of alternative subsets
   * @return the set of represented alternatives in {@code altsets}
   */
  getAlts: function (altsets) {
    let all = new BitSet();
    altsets.map(function (alts) {
      all.or(alts);
    });
    return all;
  },
  /**
   * This function gets the conflicting alt subsets from a configuration set.
   * For each configuration {@code c} in {@code configs}:
   *
   * <pre>
   * map[c] U= c.{@link ATNConfig//alt alt} // map hash/equals uses s and x, not
   * alt and not pred
   * </pre>
   */
  getConflictingAltSubsets: function (configs) {
    let configToAlts = new HashMap_HashMap();
    configToAlts.hashFunction = function (cfg) {
      HashCode.hashStuff(cfg.state.stateNumber, cfg.context);
    };
    configToAlts.equalsFunction = function (c1, c2) {
      return c1.state.stateNumber === c2.state.stateNumber && c1.context.equals(c2.context);
    };
    configs.items.map(function (cfg) {
      let alts = configToAlts.get(cfg);
      if (alts === null) {
        alts = new BitSet();
        configToAlts.set(cfg, alts);
      }
      alts.set(cfg.alt);
    });
    return configToAlts.getValues();
  },
  /**
   * Get a map from state to alt subset from a configuration set. For each
   * configuration {@code c} in {@code configs}:
   *
   * <pre>
   * map[c.{@link ATNConfig//state state}] U= c.{@link ATNConfig//alt alt}
   * </pre>
   */
  getStateToAltMap: function (configs) {
    let m = new AltDict();
    configs.items.map(function (c) {
      let alts = m.get(c.state);
      if (alts === null) {
        alts = new BitSet();
        m.set(c.state, alts);
      }
      alts.set(c.alt);
    });
    return m;
  },
  hasStateAssociatedWithOneAlt: function (configs) {
    let values = PredictionMode.getStateToAltMap(configs).values();
    for (let i = 0; i < values.length; i++) {
      if (values[i].length === 1) {
        return true;
      }
    }
    return false;
  },
  getSingleViableAlt: function (altsets) {
    let result = null;
    for (let i = 0; i < altsets.length; i++) {
      let alts = altsets[i];
      let minAlt = alts.minValue();
      if (result === null) {
        result = minAlt;
      } else if (result !== minAlt) {
        // more than 1 viable alt
        return ATN.INVALID_ALT_NUMBER;
      }
    }
    return result;
  }
};
/* harmony default export */ const atn_PredictionMode = (PredictionMode);
;// ./src/antlr4/error/NoViableAltException.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


/**
 * Indicates that the parser could not decide which of two or more paths
 * to take based upon the remaining input. It tracks the starting token
 * of the offending input and also knows where the parser was
 * in the various paths when the error. Reported by reportNoViableAlternative()
 */

class NoViableAltException extends RecognitionException {
  constructor(recognizer, input, startToken, offendingToken, deadEndConfigs, ctx) {
    ctx = ctx || recognizer._ctx;
    offendingToken = offendingToken || recognizer.getCurrentToken();
    startToken = startToken || recognizer.getCurrentToken();
    input = input || recognizer.getInputStream();
    super({
      message: "",
      recognizer: recognizer,
      input: input,
      ctx: ctx
    });
    // Which configurations did we try at input.index() that couldn't match
    // input.LT(1)?//
    this.deadEndConfigs = deadEndConfigs;
    // The token object at the start index; the input stream might
    // not be buffering tokens so get a reference to it. (At the
    // time the error occurred, of course the stream needs to keep a
    // buffer all of the tokens but later we might not have access to those.)
    this.startToken = startToken;
    this.offendingToken = offendingToken;
  }
}
;// ./src/antlr4/utils/DoubleDict.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class DoubleDict {
  constructor(defaultMapCtor) {
    this.defaultMapCtor = defaultMapCtor || HashMap_HashMap;
    this.cacheMap = new this.defaultMapCtor();
  }
  get(a, b) {
    let d = this.cacheMap.get(a) || null;
    return d === null ? null : d.get(b) || null;
  }
  set(a, b, o) {
    let d = this.cacheMap.get(a) || null;
    if (d === null) {
      d = new this.defaultMapCtor();
      this.cacheMap.set(a, d);
    }
    d.set(b, o);
  }
}
;// ./src/antlr4/atn/ParserATNSimulator.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */





























/**
 * The embodiment of the adaptive LL(*), ALL(*), parsing strategy.
 *
 * <p>
 * The basic complexity of the adaptive strategy makes it harder to understand.
 * We begin with ATN simulation to build paths in a DFA. Subsequent prediction
 * requests go through the DFA first. If they reach a state without an edge for
 * the current symbol, the algorithm fails over to the ATN simulation to
 * complete the DFA path for the current input (until it finds a conflict state
 * or uniquely predicting state).</p>
 *
 * <p>
 * All of that is done without using the outer context because we want to create
 * a DFA that is not dependent upon the rule invocation stack when we do a
 * prediction. One DFA works in all contexts. We avoid using context not
 * necessarily because it's slower, although it can be, but because of the DFA
 * caching problem. The closure routine only considers the rule invocation stack
 * created during prediction beginning in the decision rule. For example, if
 * prediction occurs without invoking another rule's ATN, there are no context
 * stacks in the configurations. When lack of context leads to a conflict, we
 * don't know if it's an ambiguity or a weakness in the strong LL(*) parsing
 * strategy (versus full LL(*)).</p>
 *
 * <p>
 * When SLL yields a configuration set with conflict, we rewind the input and
 * retry the ATN simulation, this time using full outer context without adding
 * to the DFA. Configuration context stacks will be the full invocation stacks
 * from the start rule. If we get a conflict using full context, then we can
 * definitively say we have a true ambiguity for that input sequence. If we
 * don't get a conflict, it implies that the decision is sensitive to the outer
 * context. (It is not context-sensitive in the sense of context-sensitive
 * grammars.)</p>
 *
 * <p>
 * The next time we reach this DFA state with an SLL conflict, through DFA
 * simulation, we will again retry the ATN simulation using full context mode.
 * This is slow because we can't save the results and have to "interpret" the
 * ATN each time we get that input.</p>
 *
 * <p>
 * <strong>CACHING FULL CONTEXT PREDICTIONS</strong></p>
 *
 * <p>
 * We could cache results from full context to predicted alternative easily and
 * that saves a lot of time but doesn't work in presence of predicates. The set
 * of visible predicates from the ATN start state changes depending on the
 * context, because closure can fall off the end of a rule. I tried to cache
 * tuples (stack context, semantic context, predicted alt) but it was slower
 * than interpreting and much more complicated. Also required a huge amount of
 * memory. The goal is not to create the world's fastest parser anyway. I'd like
 * to keep this algorithm simple. By launching multiple threads, we can improve
 * the speed of parsing across a large number of files.</p>
 *
 * <p>
 * There is no strict ordering between the amount of input used by SLL vs LL,
 * which makes it really hard to build a cache for full context. Let's say that
 * we have input A B C that leads to an SLL conflict with full context X. That
 * implies that using X we might only use A B but we could also use A B C D to
 * resolve conflict. Input A B C D could predict alternative 1 in one position
 * in the input and A B C E could predict alternative 2 in another position in
 * input. The conflicting SLL configurations could still be non-unique in the
 * full context prediction, which would lead us to requiring more input than the
 * original A B C.	To make a	prediction cache work, we have to track	the exact
 * input	used during the previous prediction. That amounts to a cache that maps
 * X to a specific DFA for that context.</p>
 *
 * <p>
 * Something should be done for left-recursive expression predictions. They are
 * likely LL(1) + pred eval. Easier to do the whole SLL unless error and retry
 * with full LL thing Sam does.</p>
 *
 * <p>
 * <strong>AVOIDING FULL CONTEXT PREDICTION</strong></p>
 *
 * <p>
 * We avoid doing full context retry when the outer context is empty, we did not
 * dip into the outer context by falling off the end of the decision state rule,
 * or when we force SLL mode.</p>
 *
 * <p>
 * As an example of the not dip into outer context case, consider as super
 * constructor calls versus function calls. One grammar might look like
 * this:</p>
 *
 * <pre>
 * ctorBody
 *   : '{' superCall? stat* '}'
 *   ;
 * </pre>
 *
 * <p>
 * Or, you might see something like</p>
 *
 * <pre>
 * stat
 *   : superCall ';'
 *   | expression ';'
 *   | ...
 *   ;
 * </pre>
 *
 * <p>
 * In both cases I believe that no closure operations will dip into the outer
 * context. In the first case ctorBody in the worst case will stop at the '}'.
 * In the 2nd case it should stop at the ';'. Both cases should stay within the
 * entry rule and not dip into the outer context.</p>
 *
 * <p>
 * <strong>PREDICATES</strong></p>
 *
 * <p>
 * Predicates are always evaluated if present in either SLL or LL both. SLL and
 * LL simulation deals with predicates differently. SLL collects predicates as
 * it performs closure operations like ANTLR v3 did. It delays predicate
 * evaluation until it reaches and accept state. This allows us to cache the SLL
 * ATN simulation whereas, if we had evaluated predicates on-the-fly during
 * closure, the DFA state configuration sets would be different and we couldn't
 * build up a suitable DFA.</p>
 *
 * <p>
 * When building a DFA accept state during ATN simulation, we evaluate any
 * predicates and return the sole semantically valid alternative. If there is
 * more than 1 alternative, we report an ambiguity. If there are 0 alternatives,
 * we throw an exception. Alternatives without predicates act like they have
 * true predicates. The simple way to think about it is to strip away all
 * alternatives with false predicates and choose the minimum alternative that
 * remains.</p>
 *
 * <p>
 * When we start in the DFA and reach an accept state that's predicated, we test
 * those and return the minimum semantically viable alternative. If no
 * alternatives are viable, we throw an exception.</p>
 *
 * <p>
 * During full LL ATN simulation, closure always evaluates predicates and
 * on-the-fly. This is crucial to reducing the configuration set size during
 * closure. It hits a landmine when parsing with the Java grammar, for example,
 * without this on-the-fly evaluation.</p>
 *
 * <p>
 * <strong>SHARING DFA</strong></p>
 *
 * <p>
 * All instances of the same parser share the same decision DFAs through a
 * static field. Each instance gets its own ATN simulator but they share the
 * same {@link //decisionToDFA} field. They also share a
 * {@link PredictionContextCache} object that makes sure that all
 * {@link PredictionContext} objects are shared among the DFA states. This makes
 * a big size difference.</p>
 *
 * <p>
 * <strong>THREAD SAFETY</strong></p>
 *
 * <p>
 * The {@link ParserATNSimulator} locks on the {@link //decisionToDFA} field when
 * it adds a new DFA object to that array. {@link //addDFAEdge}
 * locks on the DFA for the current decision when setting the
 * {@link DFAState//edges} field. {@link //addDFAState} locks on
 * the DFA for the current decision when looking up a DFA state to see if it
 * already exists. We must make sure that all requests to add DFA states that
 * are equivalent result in the same shared DFA object. This is because lots of
 * threads will be trying to update the DFA at once. The
 * {@link //addDFAState} method also locks inside the DFA lock
 * but this time on the shared context cache when it rebuilds the
 * configurations' {@link PredictionContext} objects using cached
 * subgraphs/nodes. No other locking occurs, even during DFA simulation. This is
 * safe as long as we can guarantee that all threads referencing
 * {@code s.edge[t]} get the same physical target {@link DFAState}, or
 * {@code null}. Once into the DFA, the DFA simulation does not reference the
 * {@link DFA//states} map. It follows the {@link DFAState//edges} field to new
 * targets. The DFA simulator will either find {@link DFAState//edges} to be
 * {@code null}, to be non-{@code null} and {@code dfa.edges[t]} null, or
 * {@code dfa.edges[t]} to be non-null. The
 * {@link //addDFAEdge} method could be racing to set the field
 * but in either case the DFA simulator works; if {@code null}, and requests ATN
 * simulation. It could also race trying to get {@code dfa.edges[t]}, but either
 * way it will work because it's not doing a test and set operation.</p>
 *
 * <p>
 * <strong>Starting with SLL then failing to combined SLL/LL (Two-Stage
 * Parsing)</strong></p>
 *
 * <p>
 * Sam pointed out that if SLL does not give a syntax error, then there is no
 * point in doing full LL, which is slower. We only have to try LL if we get a
 * syntax error. For maximum speed, Sam starts the parser set to pure SLL
 * mode with the {@link BailErrorStrategy}:</p>
 *
 * <pre>
 * parser.{@link Parser//getInterpreter() getInterpreter()}.{@link //setPredictionMode setPredictionMode}{@code (}{@link PredictionMode//SLL}{@code )};
 * parser.{@link Parser//setErrorHandler setErrorHandler}(new {@link BailErrorStrategy}());
 * </pre>
 *
 * <p>
 * If it does not get a syntax error, then we're done. If it does get a syntax
 * error, we need to retry with the combined SLL/LL strategy.</p>
 *
 * <p>
 * The reason this works is as follows. If there are no SLL conflicts, then the
 * grammar is SLL (at least for that input set). If there is an SLL conflict,
 * the full LL analysis must yield a set of viable alternatives which is a
 * subset of the alternatives reported by SLL. If the LL set is a singleton,
 * then the grammar is LL but not SLL. If the LL set is the same size as the SLL
 * set, the decision is SLL. If the LL set has size &gt; 1, then that decision
 * is truly ambiguous on the current input. If the LL set is smaller, then the
 * SLL conflict resolution might choose an alternative that the full LL would
 * rule out as a possibility based upon better context information. If that's
 * the case, then the SLL parse will definitely get an error because the full LL
 * analysis says it's not viable. If SLL conflict resolution chooses an
 * alternative within the LL set, them both SLL and LL would choose the same
 * alternative because they both choose the minimum of multiple conflicting
 * alternatives.</p>
 *
 * <p>
 * Let's say we have a set of SLL conflicting alternatives {@code {1, 2, 3}} and
 * a smaller LL set called <em>s</em>. If <em>s</em> is {@code {2, 3}}, then SLL
 * parsing will get an error because SLL will pursue alternative 1. If
 * <em>s</em> is {@code {1, 2}} or {@code {1, 3}} then both SLL and LL will
 * choose the same alternative because alternative one is the minimum of either
 * set. If <em>s</em> is {@code {2}} or {@code {3}} then SLL will get a syntax
 * error. If <em>s</em> is {@code {1}} then SLL will succeed.</p>
 *
 * <p>
 * Of course, if the input is invalid, then we will get an error for sure in
 * both SLL and LL parsing. Erroneous input will therefore require 2 passes over
 * the input.</p>
 */
class ParserATNSimulator extends ATNSimulator {
  constructor(parser, atn, decisionToDFA, sharedContextCache) {
    super(atn, sharedContextCache);
    this.parser = parser;
    this.decisionToDFA = decisionToDFA;
    // SLL, LL, or LL + exact ambig detection?//
    this.predictionMode = atn_PredictionMode.LL;
    // LAME globals to avoid parameters!!!!! I need these down deep in predTransition
    this._input = null;
    this._startIndex = 0;
    this._outerContext = null;
    this._dfa = null;
    /**
     * Each prediction operation uses a cache for merge of prediction contexts.
     *  Don't keep around as it wastes huge amounts of memory. DoubleKeyMap
     *  isn't synchronized but we're ok since two threads shouldn't reuse same
     *  parser/atnsim object because it can only handle one input at a time.
     *  This maps graphs a and b to merged result c. (a,b)&rarr;c. We can avoid
     *  the merge if we ever see a and b again.  Note that (b,a)&rarr;c should
     *  also be examined during cache lookup.
     */
    this.mergeCache = null;
    this.debug = false;
    this.debug_closure = false;
    this.debug_add = false;
    this.trace_atn_sim = false;
    this.dfa_debug = false;
    this.retry_debug = false;
  }
  reset() {}
  adaptivePredict(input, decision, outerContext) {
    if (this.debug || this.trace_atn_sim) {
      console.log("adaptivePredict decision " + decision + " exec LA(1)==" + this.getLookaheadName(input) + " line " + input.LT(1).line + ":" + input.LT(1).column);
    }
    this._input = input;
    this._startIndex = input.index;
    this._outerContext = outerContext;
    let dfa = this.decisionToDFA[decision];
    this._dfa = dfa;
    let m = input.mark();
    let index = input.index;

    // Now we are certain to have a specific decision's DFA
    // But, do we still need an initial state?
    try {
      let s0;
      if (dfa.precedenceDfa) {
        // the start state for a precedence DFA depends on the current
        // parser precedence, and is provided by a DFA method.
        s0 = dfa.getPrecedenceStartState(this.parser.getPrecedence());
      } else {
        // the start state for a "regular" DFA is just s0
        s0 = dfa.s0;
      }
      if (s0 === null) {
        if (outerContext === null) {
          outerContext = RuleContext.EMPTY;
        }
        if (this.debug) {
          console.log("predictATN decision " + dfa.decision + " exec LA(1)==" + this.getLookaheadName(input) + ", outerContext=" + outerContext.toString(this.parser.ruleNames));
        }
        let fullCtx = false;
        let s0_closure = this.computeStartState(dfa.atnStartState, RuleContext.EMPTY, fullCtx);
        if (dfa.precedenceDfa) {
          // If this is a precedence DFA, we use applyPrecedenceFilter
          // to convert the computed start state to a precedence start
          // state. We then use DFA.setPrecedenceStartState to set the
          // appropriate start state for the precedence level rather
          // than simply setting DFA.s0.
          //
          dfa.s0.configs = s0_closure; // not used for prediction but useful to know start configs anyway
          s0_closure = this.applyPrecedenceFilter(s0_closure);
          s0 = this.addDFAState(dfa, new DFAState(null, s0_closure));
          dfa.setPrecedenceStartState(this.parser.getPrecedence(), s0);
        } else {
          s0 = this.addDFAState(dfa, new DFAState(null, s0_closure));
          dfa.s0 = s0;
        }
      }
      let alt = this.execATN(dfa, s0, input, index, outerContext);
      if (this.debug) {
        console.log("DFA after predictATN: " + dfa.toString(this.parser.literalNames, this.parser.symbolicNames));
      }
      return alt;
    } finally {
      this._dfa = null;
      this.mergeCache = null; // wack cache after each prediction
      input.seek(index);
      input.release(m);
    }
  }

  /**
   * Performs ATN simulation to compute a predicted alternative based
   *  upon the remaining input, but also updates the DFA cache to avoid
   *  having to traverse the ATN again for the same input sequence.
   *
   * There are some key conditions we're looking for after computing a new
   * set of ATN configs (proposed DFA state):
   *       if the set is empty, there is no viable alternative for current symbol
   *       does the state uniquely predict an alternative?
   *       does the state have a conflict that would prevent us from
   *         putting it on the work list?
   *
   * We also have some key operations to do:
   *       add an edge from previous DFA state to potentially new DFA state, D,
   *         upon current symbol but only if adding to work list, which means in all
   *         cases except no viable alternative (and possibly non-greedy decisions?)
   *       collecting predicates and adding semantic context to DFA accept states
   *       adding rule context to context-sensitive DFA accept states
   *       consuming an input symbol
   *       reporting a conflict
   *       reporting an ambiguity
   *       reporting a context sensitivity
   *       reporting insufficient predicates
   *
   * cover these cases:
   *    dead end
   *    single alt
   *    single alt + preds
   *    conflict
   *    conflict + preds
   *
   */
  execATN(dfa, s0, input, startIndex, outerContext) {
    if (this.debug || this.trace_atn_sim) {
      console.log("execATN decision " + dfa.decision + ", DFA state " + s0 + ", LA(1)==" + this.getLookaheadName(input) + " line " + input.LT(1).line + ":" + input.LT(1).column);
    }
    let alt;
    let previousD = s0;
    if (this.debug) {
      console.log("s0 = " + s0);
    }
    let t = input.LA(1);
    for (;;) {
      // while more work
      let D = this.getExistingTargetState(previousD, t);
      if (D === null) {
        D = this.computeTargetState(dfa, previousD, t);
      }
      if (D === ATNSimulator.ERROR) {
        // if any configs in previous dipped into outer context, that
        // means that input up to t actually finished entry rule
        // at least for SLL decision. Full LL doesn't dip into outer
        // so don't need special case.
        // We will get an error no matter what so delay until after
        // decision; better error message. Also, no reachable target
        // ATN states in SLL implies LL will also get nowhere.
        // If conflict in states that dip out, choose min since we
        // will get error no matter what.
        let e = this.noViableAlt(input, outerContext, previousD.configs, startIndex);
        input.seek(startIndex);
        alt = this.getSynValidOrSemInvalidAltThatFinishedDecisionEntryRule(previousD.configs, outerContext);
        if (alt !== ATN.INVALID_ALT_NUMBER) {
          return alt;
        } else {
          throw e;
        }
      }
      if (D.requiresFullContext && this.predictionMode !== atn_PredictionMode.SLL) {
        // IF PREDS, MIGHT RESOLVE TO SINGLE ALT => SLL (or syntax error)
        let conflictingAlts = null;
        if (D.predicates !== null) {
          if (this.debug) {
            console.log("DFA state has preds in DFA sim LL failover");
          }
          let conflictIndex = input.index;
          if (conflictIndex !== startIndex) {
            input.seek(startIndex);
          }
          conflictingAlts = this.evalSemanticContext(D.predicates, outerContext, true);
          if (conflictingAlts.length === 1) {
            if (this.debug) {
              console.log("Full LL avoided");
            }
            return conflictingAlts.minValue();
          }
          if (conflictIndex !== startIndex) {
            // restore the index so reporting the fallback to full
            // context occurs with the index at the correct spot
            input.seek(conflictIndex);
          }
        }
        if (this.dfa_debug) {
          console.log("ctx sensitive state " + outerContext + " in " + D);
        }
        let fullCtx = true;
        let s0_closure = this.computeStartState(dfa.atnStartState, outerContext, fullCtx);
        this.reportAttemptingFullContext(dfa, conflictingAlts, D.configs, startIndex, input.index);
        alt = this.execATNWithFullContext(dfa, D, s0_closure, input, startIndex, outerContext);
        return alt;
      }
      if (D.isAcceptState) {
        if (D.predicates === null) {
          return D.prediction;
        }
        let stopIndex = input.index;
        input.seek(startIndex);
        let alts = this.evalSemanticContext(D.predicates, outerContext, true);
        if (alts.length === 0) {
          throw this.noViableAlt(input, outerContext, D.configs, startIndex);
        } else if (alts.length === 1) {
          return alts.minValue();
        } else {
          // report ambiguity after predicate evaluation to make sure the correct set of ambig alts is reported.
          this.reportAmbiguity(dfa, D, startIndex, stopIndex, false, alts, D.configs);
          return alts.minValue();
        }
      }
      previousD = D;
      if (t !== Token.EOF) {
        input.consume();
        t = input.LA(1);
      }
    }
  }

  /**
   * Get an existing target state for an edge in the DFA. If the target state
   * for the edge has not yet been computed or is otherwise not available,
   * this method returns {@code null}.
   *
   * @param previousD The current DFA state
   * @param t The next input symbol
   * @return The existing target DFA state for the given input symbol
   * {@code t}, or {@code null} if the target state for this edge is not
   * already cached
   */
  getExistingTargetState(previousD, t) {
    let edges = previousD.edges;
    if (edges === null) {
      return null;
    } else {
      return edges[t + 1] || null;
    }
  }

  /**
   * Compute a target state for an edge in the DFA, and attempt to add the
   * computed state and corresponding edge to the DFA.
   *
   * @param dfa The DFA
   * @param previousD The current DFA state
   * @param t The next input symbol
   *
   * @return The computed target DFA state for the given input symbol
   * {@code t}. If {@code t} does not lead to a valid DFA state, this method
   * returns {@link //ERROR
   */
  computeTargetState(dfa, previousD, t) {
    let reach = this.computeReachSet(previousD.configs, t, false);
    if (reach === null) {
      this.addDFAEdge(dfa, previousD, t, ATNSimulator.ERROR);
      return ATNSimulator.ERROR;
    }
    // create new target state; we'll add to DFA after it's complete
    let D = new DFAState(null, reach);
    let predictedAlt = this.getUniqueAlt(reach);
    if (this.debug) {
      let altSubSets = atn_PredictionMode.getConflictingAltSubsets(reach);
      console.log("SLL altSubSets=" + arrayToString(altSubSets) + /*", previous=" + previousD.configs + */
      ", configs=" + reach + ", predict=" + predictedAlt + ", allSubsetsConflict=" + atn_PredictionMode.allSubsetsConflict(altSubSets) + ", conflictingAlts=" + this.getConflictingAlts(reach));
    }
    if (predictedAlt !== ATN.INVALID_ALT_NUMBER) {
      // NO CONFLICT, UNIQUELY PREDICTED ALT
      D.isAcceptState = true;
      D.configs.uniqueAlt = predictedAlt;
      D.prediction = predictedAlt;
    } else if (atn_PredictionMode.hasSLLConflictTerminatingPrediction(this.predictionMode, reach)) {
      // MORE THAN ONE VIABLE ALTERNATIVE
      D.configs.conflictingAlts = this.getConflictingAlts(reach);
      D.requiresFullContext = true;
      // in SLL-only mode, we will stop at this state and return the minimum alt
      D.isAcceptState = true;
      D.prediction = D.configs.conflictingAlts.minValue();
    }
    if (D.isAcceptState && D.configs.hasSemanticContext) {
      this.predicateDFAState(D, this.atn.getDecisionState(dfa.decision));
      if (D.predicates !== null) {
        D.prediction = ATN.INVALID_ALT_NUMBER;
      }
    }
    // all adds to dfa are done after we've created full D state
    D = this.addDFAEdge(dfa, previousD, t, D);
    return D;
  }
  predicateDFAState(dfaState, decisionState) {
    // We need to test all predicates, even in DFA states that
    // uniquely predict alternative.
    let nalts = decisionState.transitions.length;
    // Update DFA so reach becomes accept state with (predicate,alt)
    // pairs if preds found for conflicting alts
    let altsToCollectPredsFrom = this.getConflictingAltsOrUniqueAlt(dfaState.configs);
    let altToPred = this.getPredsForAmbigAlts(altsToCollectPredsFrom, dfaState.configs, nalts);
    if (altToPred !== null) {
      dfaState.predicates = this.getPredicatePredictions(altsToCollectPredsFrom, altToPred);
      dfaState.prediction = ATN.INVALID_ALT_NUMBER; // make sure we use preds
    } else {
      // There are preds in configs but they might go away
      // when OR'd together like {p}? || NONE == NONE. If neither
      // alt has preds, resolve to min alt
      dfaState.prediction = altsToCollectPredsFrom.minValue();
    }
  }

  // comes back with reach.uniqueAlt set to a valid alt
  execATNWithFullContext(dfa, D,
  // how far we got before failing over
  s0, input, startIndex, outerContext) {
    if (this.debug || this.trace_atn_sim) {
      console.log("execATNWithFullContext " + s0);
    }
    let fullCtx = true;
    let foundExactAmbig = false;
    let reach;
    let previous = s0;
    input.seek(startIndex);
    let t = input.LA(1);
    let predictedAlt = -1;
    for (;;) {
      // while more work
      reach = this.computeReachSet(previous, t, fullCtx);
      if (reach === null) {
        // if any configs in previous dipped into outer context, that
        // means that input up to t actually finished entry rule
        // at least for LL decision. Full LL doesn't dip into outer
        // so don't need special case.
        // We will get an error no matter what so delay until after
        // decision; better error message. Also, no reachable target
        // ATN states in SLL implies LL will also get nowhere.
        // If conflict in states that dip out, choose min since we
        // will get error no matter what.
        let e = this.noViableAlt(input, outerContext, previous, startIndex);
        input.seek(startIndex);
        let alt = this.getSynValidOrSemInvalidAltThatFinishedDecisionEntryRule(previous, outerContext);
        if (alt !== ATN.INVALID_ALT_NUMBER) {
          return alt;
        } else {
          throw e;
        }
      }
      let altSubSets = atn_PredictionMode.getConflictingAltSubsets(reach);
      if (this.debug) {
        console.log("LL altSubSets=" + altSubSets + ", predict=" + atn_PredictionMode.getUniqueAlt(altSubSets) + ", resolvesToJustOneViableAlt=" + atn_PredictionMode.resolvesToJustOneViableAlt(altSubSets));
      }
      reach.uniqueAlt = this.getUniqueAlt(reach);
      // unique prediction?
      if (reach.uniqueAlt !== ATN.INVALID_ALT_NUMBER) {
        predictedAlt = reach.uniqueAlt;
        break;
      } else if (this.predictionMode !== atn_PredictionMode.LL_EXACT_AMBIG_DETECTION) {
        predictedAlt = atn_PredictionMode.resolvesToJustOneViableAlt(altSubSets);
        if (predictedAlt !== ATN.INVALID_ALT_NUMBER) {
          break;
        }
      } else {
        // In exact ambiguity mode, we never try to terminate early.
        // Just keeps scarfing until we know what the conflict is
        if (atn_PredictionMode.allSubsetsConflict(altSubSets) && atn_PredictionMode.allSubsetsEqual(altSubSets)) {
          foundExactAmbig = true;
          predictedAlt = atn_PredictionMode.getSingleViableAlt(altSubSets);
          break;
        }
        // else there are multiple non-conflicting subsets or
        // we're not sure what the ambiguity is yet.
        // So, keep going.
      }
      previous = reach;
      if (t !== Token.EOF) {
        input.consume();
        t = input.LA(1);
      }
    }
    // If the configuration set uniquely predicts an alternative,
    // without conflict, then we know that it's a full LL decision
    // not SLL.
    if (reach.uniqueAlt !== ATN.INVALID_ALT_NUMBER) {
      this.reportContextSensitivity(dfa, predictedAlt, reach, startIndex, input.index);
      return predictedAlt;
    }
    // We do not check predicates here because we have checked them
    // on-the-fly when doing full context prediction.

    //
    // In non-exact ambiguity detection mode, we might	actually be able to
    // detect an exact ambiguity, but I'm not going to spend the cycles
    // needed to check. We only emit ambiguity warnings in exact ambiguity
    // mode.
    //
    // For example, we might know that we have conflicting configurations.
    // But, that does not mean that there is no way forward without a
    // conflict. It's possible to have nonconflicting alt subsets as in:

    // altSubSets=[{1, 2}, {1, 2}, {1}, {1, 2}]

    // from
    //
    //    [(17,1,[5 $]), (13,1,[5 10 $]), (21,1,[5 10 $]), (11,1,[$]),
    //     (13,2,[5 10 $]), (21,2,[5 10 $]), (11,2,[$])]
    //
    // In this case, (17,1,[5 $]) indicates there is some next sequence that
    // would resolve this without conflict to alternative 1. Any other viable
    // next sequence, however, is associated with a conflict.  We stop
    // looking for input because no amount of further lookahead will alter
    // the fact that we should predict alternative 1.  We just can't say for
    // sure that there is an ambiguity without looking further.

    this.reportAmbiguity(dfa, D, startIndex, input.index, foundExactAmbig, null, reach);
    return predictedAlt;
  }
  computeReachSet(closure, t, fullCtx) {
    if (this.debug) {
      console.log("in computeReachSet, starting closure: " + closure);
    }
    if (this.mergeCache === null) {
      this.mergeCache = new DoubleDict();
    }
    let intermediate = new ATNConfigSet(fullCtx);

    // Configurations already in a rule stop state indicate reaching the end
    // of the decision rule (local context) or end of the start rule (full
    // context). Once reached, these configurations are never updated by a
    // closure operation, so they are handled separately for the performance
    // advantage of having a smaller intermediate set when calling closure.
    //
    // For full-context reach operations, separate handling is required to
    // ensure that the alternative matching the longest overall sequence is
    // chosen when multiple such configurations can match the input.

    let skippedStopStates = null;

    // First figure out where we can reach on input t
    for (let i = 0; i < closure.items.length; i++) {
      let c = closure.items[i];
      if (this.debug) {
        console.log("testing " + this.getTokenName(t) + " at " + c);
      }
      if (c.state instanceof RuleStopState) {
        if (fullCtx || t === Token.EOF) {
          if (skippedStopStates === null) {
            skippedStopStates = [];
          }
          skippedStopStates.push(c);
          if (this.debug_add) {
            console.log("added " + c + " to skippedStopStates");
          }
        }
        continue;
      }
      for (let j = 0; j < c.state.transitions.length; j++) {
        let trans = c.state.transitions[j];
        let target = this.getReachableTarget(trans, t);
        if (target !== null) {
          let cfg = new ATNConfig({
            state: target
          }, c);
          intermediate.add(cfg, this.mergeCache);
          if (this.debug_add) {
            console.log("added " + cfg + " to intermediate");
          }
        }
      }
    }
    // Now figure out where the reach operation can take us...
    let reach = null;

    // This block optimizes the reach operation for intermediate sets which
    // trivially indicate a termination state for the overall
    // adaptivePredict operation.
    //
    // The conditions assume that intermediate
    // contains all configurations relevant to the reach set, but this
    // condition is not true when one or more configurations have been
    // withheld in skippedStopStates, or when the current symbol is EOF.
    //
    if (skippedStopStates === null && t !== Token.EOF) {
      if (intermediate.items.length === 1) {
        // Don't pursue the closure if there is just one state.
        // It can only have one alternative; just add to result
        // Also don't pursue the closure if there is unique alternative
        // among the configurations.
        reach = intermediate;
      } else if (this.getUniqueAlt(intermediate) !== ATN.INVALID_ALT_NUMBER) {
        // Also don't pursue the closure if there is unique alternative
        // among the configurations.
        reach = intermediate;
      }
    }
    // If the reach set could not be trivially determined, perform a closure
    // operation on the intermediate set to compute its initial value.
    //
    if (reach === null) {
      reach = new ATNConfigSet(fullCtx);
      let closureBusy = new HashSet();
      let treatEofAsEpsilon = t === Token.EOF;
      for (let k = 0; k < intermediate.items.length; k++) {
        this.closure(intermediate.items[k], reach, closureBusy, false, fullCtx, treatEofAsEpsilon);
      }
    }
    if (t === Token.EOF) {
      // After consuming EOF no additional input is possible, so we are
      // only interested in configurations which reached the end of the
      // decision rule (local context) or end of the start rule (full
      // context). Update reach to contain only these configurations. This
      // handles both explicit EOF transitions in the grammar and implicit
      // EOF transitions following the end of the decision or start rule.
      //
      // When reach==intermediate, no closure operation was performed. In
      // this case, removeAllConfigsNotInRuleStopState needs to check for
      // reachable rule stop states as well as configurations already in
      // a rule stop state.
      //
      // This is handled before the configurations in skippedStopStates,
      // because any configurations potentially added from that list are
      // already guaranteed to meet this condition whether or not it's
      // required.
      //
      reach = this.removeAllConfigsNotInRuleStopState(reach, reach === intermediate);
    }
    // If skippedStopStates!==null, then it contains at least one
    // configuration. For full-context reach operations, these
    // configurations reached the end of the start rule, in which case we
    // only add them back to reach if no configuration during the current
    // closure operation reached such a state. This ensures adaptivePredict
    // chooses an alternative matching the longest overall sequence when
    // multiple alternatives are viable.
    //
    if (skippedStopStates !== null && (!fullCtx || !atn_PredictionMode.hasConfigInRuleStopState(reach))) {
      for (let l = 0; l < skippedStopStates.length; l++) {
        reach.add(skippedStopStates[l], this.mergeCache);
      }
    }
    if (this.trace_atn_sim) {
      console.log("computeReachSet " + closure + " -> " + reach);
    }
    if (reach.items.length === 0) {
      return null;
    } else {
      return reach;
    }
  }

  /**
   * Return a configuration set containing only the configurations from
   * {@code configs} which are in a {@link RuleStopState}. If all
   * configurations in {@code configs} are already in a rule stop state, this
   * method simply returns {@code configs}.
   *
   * <p>When {@code lookToEndOfRule} is true, this method uses
   * {@link ATN//nextTokens} for each configuration in {@code configs} which is
   * not already in a rule stop state to see if a rule stop state is reachable
   * from the configuration via epsilon-only transitions.</p>
   *
   * @param configs the configuration set to update
   * @param lookToEndOfRule when true, this method checks for rule stop states
   * reachable by epsilon-only transitions from each configuration in
   * {@code configs}.
   *
   * @return {@code configs} if all configurations in {@code configs} are in a
   * rule stop state, otherwise return a new configuration set containing only
   * the configurations from {@code configs} which are in a rule stop state
   */
  removeAllConfigsNotInRuleStopState(configs, lookToEndOfRule) {
    if (atn_PredictionMode.allConfigsInRuleStopStates(configs)) {
      return configs;
    }
    let result = new ATNConfigSet(configs.fullCtx);
    for (let i = 0; i < configs.items.length; i++) {
      let config = configs.items[i];
      if (config.state instanceof RuleStopState) {
        result.add(config, this.mergeCache);
        continue;
      }
      if (lookToEndOfRule && config.state.epsilonOnlyTransitions) {
        let nextTokens = this.atn.nextTokens(config.state);
        if (nextTokens.contains(Token.EPSILON)) {
          let endOfRuleState = this.atn.ruleToStopState[config.state.ruleIndex];
          result.add(new ATNConfig({
            state: endOfRuleState
          }, config), this.mergeCache);
        }
      }
    }
    return result;
  }
  computeStartState(p, ctx, fullCtx) {
    // always at least the implicit call to start rule
    let initialContext = predictionContextFromRuleContext(this.atn, ctx);
    let configs = new ATNConfigSet(fullCtx);
    if (this.trace_atn_sim) {
      console.log("computeStartState from ATN state " + p + " initialContext=" + initialContext.toString(this.parser));
    }
    for (let i = 0; i < p.transitions.length; i++) {
      let target = p.transitions[i].target;
      let c = new ATNConfig({
        state: target,
        alt: i + 1,
        context: initialContext
      }, null);
      let closureBusy = new HashSet();
      this.closure(c, configs, closureBusy, true, fullCtx, false);
    }
    return configs;
  }

  /**
   * This method transforms the start state computed by
   * {@link //computeStartState} to the special start state used by a
   * precedence DFA for a particular precedence value. The transformation
   * process applies the following changes to the start state's configuration
   * set.
   *
   * <ol>
   * <li>Evaluate the precedence predicates for each configuration using
   * {@link SemanticContext//evalPrecedence}.</li>
   * <li>Remove all configurations which predict an alternative greater than
   * 1, for which another configuration that predicts alternative 1 is in the
   * same ATN state with the same prediction context. This transformation is
   * valid for the following reasons:
   * <ul>
   * <li>The closure block cannot contain any epsilon transitions which bypass
   * the body of the closure, so all states reachable via alternative 1 are
   * part of the precedence alternatives of the transformed left-recursive
   * rule.</li>
   * <li>The "primary" portion of a left recursive rule cannot contain an
   * epsilon transition, so the only way an alternative other than 1 can exist
   * in a state that is also reachable via alternative 1 is by nesting calls
   * to the left-recursive rule, with the outer calls not being at the
   * preferred precedence level.</li>
   * </ul>
   * </li>
   * </ol>
   *
   * <p>
   * The prediction context must be considered by this filter to address
   * situations like the following.
   * </p>
   * <code>
   * <pre>
   * grammar TA;
   * prog: statement* EOF;
   * statement: letterA | statement letterA 'b' ;
   * letterA: 'a';
   * </pre>
   * </code>
   * <p>
   * If the above grammar, the ATN state immediately before the token
   * reference {@code 'a'} in {@code letterA} is reachable from the left edge
   * of both the primary and closure blocks of the left-recursive rule
   * {@code statement}. The prediction context associated with each of these
   * configurations distinguishes between them, and prevents the alternative
   * which stepped out to {@code prog} (and then back in to {@code statement}
   * from being eliminated by the filter.
   * </p>
   *
   * @param configs The configuration set computed by
   * {@link //computeStartState} as the start state for the DFA.
   * @return The transformed configuration set representing the start state
   * for a precedence DFA at a particular precedence level (determined by
   * calling {@link Parser//getPrecedence})
   */
  applyPrecedenceFilter(configs) {
    let config;
    let statesFromAlt1 = [];
    let configSet = new ATNConfigSet(configs.fullCtx);
    for (let i = 0; i < configs.items.length; i++) {
      config = configs.items[i];
      // handle alt 1 first
      if (config.alt !== 1) {
        continue;
      }
      let updatedContext = config.semanticContext.evalPrecedence(this.parser, this._outerContext);
      if (updatedContext === null) {
        // the configuration was eliminated
        continue;
      }
      statesFromAlt1[config.state.stateNumber] = config.context;
      if (updatedContext !== config.semanticContext) {
        configSet.add(new ATNConfig({
          semanticContext: updatedContext
        }, config), this.mergeCache);
      } else {
        configSet.add(config, this.mergeCache);
      }
    }
    for (let i = 0; i < configs.items.length; i++) {
      config = configs.items[i];
      if (config.alt === 1) {
        // already handled
        continue;
      }
      // In the future, this elimination step could be updated to also
      // filter the prediction context for alternatives predicting alt>1
      // (basically a graph subtraction algorithm).
      if (!config.precedenceFilterSuppressed) {
        let context = statesFromAlt1[config.state.stateNumber] || null;
        if (context !== null && context.equals(config.context)) {
          // eliminated
          continue;
        }
      }
      configSet.add(config, this.mergeCache);
    }
    return configSet;
  }
  getReachableTarget(trans, ttype) {
    if (trans.matches(ttype, 0, this.atn.maxTokenType)) {
      return trans.target;
    } else {
      return null;
    }
  }
  getPredsForAmbigAlts(ambigAlts, configs, nalts) {
    // REACH=[1|1|[]|0:0, 1|2|[]|0:1]
    // altToPred starts as an array of all null contexts. The entry at index i
    // corresponds to alternative i. altToPred[i] may have one of three values:
    //   1. null: no ATNConfig c is found such that c.alt==i
    //   2. SemanticContext.NONE: At least one ATNConfig c exists such that
    //      c.alt==i and c.semanticContext==SemanticContext.NONE. In other words,
    //      alt i has at least one unpredicated config.
    //   3. Non-NONE Semantic Context: There exists at least one, and for all
    //      ATNConfig c such that c.alt==i, c.semanticContext!=SemanticContext.NONE.
    //
    // From this, it is clear that NONE||anything==NONE.
    //
    let altToPred = [];
    for (let i = 0; i < configs.items.length; i++) {
      let c = configs.items[i];
      if (ambigAlts.get(c.alt)) {
        altToPred[c.alt] = SemanticContext.orContext(altToPred[c.alt] || null, c.semanticContext);
      }
    }
    let nPredAlts = 0;
    for (let i = 1; i < nalts + 1; i++) {
      let pred = altToPred[i] || null;
      if (pred === null) {
        altToPred[i] = SemanticContext.NONE;
      } else if (pred !== SemanticContext.NONE) {
        nPredAlts += 1;
      }
    }
    // nonambig alts are null in altToPred
    if (nPredAlts === 0) {
      altToPred = null;
    }
    if (this.debug) {
      console.log("getPredsForAmbigAlts result " + arrayToString(altToPred));
    }
    return altToPred;
  }
  getPredicatePredictions(ambigAlts, altToPred) {
    let pairs = [];
    let containsPredicate = false;
    for (let i = 1; i < altToPred.length; i++) {
      let pred = altToPred[i];
      // unpredicated is indicated by SemanticContext.NONE
      if (ambigAlts !== null && ambigAlts.get(i)) {
        pairs.push(new PredPrediction(pred, i));
      }
      if (pred !== SemanticContext.NONE) {
        containsPredicate = true;
      }
    }
    if (!containsPredicate) {
      return null;
    }
    return pairs;
  }

  /**
   * This method is used to improve the localization of error messages by
   * choosing an alternative rather than throwing a
   * {@link NoViableAltException} in particular prediction scenarios where the
   * {@link //ERROR} state was reached during ATN simulation.
   *
   * <p>
   * The default implementation of this method uses the following
   * algorithm to identify an ATN configuration which successfully parsed the
   * decision entry rule. Choosing such an alternative ensures that the
   * {@link ParserRuleContext} returned by the calling rule will be complete
   * and valid, and the syntax error will be reported later at a more
   * localized location.</p>
   *
   * <ul>
   * <li>If a syntactically valid path or paths reach the end of the decision rule and
   * they are semantically valid if predicated, return the min associated alt.</li>
   * <li>Else, if a semantically invalid but syntactically valid path exist
   * or paths exist, return the minimum associated alt.
   * </li>
   * <li>Otherwise, return {@link ATN//INVALID_ALT_NUMBER}.</li>
   * </ul>
   *
   * <p>
   * In some scenarios, the algorithm described above could predict an
   * alternative which will result in a {@link FailedPredicateException} in
   * the parser. Specifically, this could occur if the <em>only</em> configuration
   * capable of successfully parsing to the end of the decision rule is
   * blocked by a semantic predicate. By choosing this alternative within
   * {@link //adaptivePredict} instead of throwing a
   * {@link NoViableAltException}, the resulting
   * {@link FailedPredicateException} in the parser will identify the specific
   * predicate which is preventing the parser from successfully parsing the
   * decision rule, which helps developers identify and correct logic errors
   * in semantic predicates.
   * </p>
   *
   * @param configs The ATN configurations which were valid immediately before
   * the {@link //ERROR} state was reached
   * @param outerContext The is the \gamma_0 initial parser context from the paper
   * or the parser stack at the instant before prediction commences.
   *
   * @return The value to return from {@link //adaptivePredict}, or
   * {@link ATN//INVALID_ALT_NUMBER} if a suitable alternative was not
   * identified and {@link //adaptivePredict} should report an error instead
   */
  getSynValidOrSemInvalidAltThatFinishedDecisionEntryRule(configs, outerContext) {
    let cfgs = this.splitAccordingToSemanticValidity(configs, outerContext);
    let semValidConfigs = cfgs[0];
    let semInvalidConfigs = cfgs[1];
    let alt = this.getAltThatFinishedDecisionEntryRule(semValidConfigs);
    if (alt !== ATN.INVALID_ALT_NUMBER) {
      // semantically/syntactically viable path exists
      return alt;
    }
    // Is there a syntactically valid path with a failed pred?
    if (semInvalidConfigs.items.length > 0) {
      alt = this.getAltThatFinishedDecisionEntryRule(semInvalidConfigs);
      if (alt !== ATN.INVALID_ALT_NUMBER) {
        // syntactically viable path exists
        return alt;
      }
    }
    return ATN.INVALID_ALT_NUMBER;
  }
  getAltThatFinishedDecisionEntryRule(configs) {
    let alts = [];
    for (let i = 0; i < configs.items.length; i++) {
      let c = configs.items[i];
      if (c.reachesIntoOuterContext > 0 || c.state instanceof RuleStopState && c.context.hasEmptyPath()) {
        if (alts.indexOf(c.alt) < 0) {
          alts.push(c.alt);
        }
      }
    }
    if (alts.length === 0) {
      return ATN.INVALID_ALT_NUMBER;
    } else {
      return Math.min.apply(null, alts);
    }
  }

  /**
   * Walk the list of configurations and split them according to
   * those that have preds evaluating to true/false.  If no pred, assume
   * true pred and include in succeeded set.  Returns Pair of sets.
   *
   * Create a new set so as not to alter the incoming parameter.
   *
   * Assumption: the input stream has been restored to the starting point
   * prediction, which is where predicates need to evaluate.*/
  splitAccordingToSemanticValidity(configs, outerContext) {
    let succeeded = new ATNConfigSet(configs.fullCtx);
    let failed = new ATNConfigSet(configs.fullCtx);
    for (let i = 0; i < configs.items.length; i++) {
      let c = configs.items[i];
      if (c.semanticContext !== SemanticContext.NONE) {
        let predicateEvaluationResult = c.semanticContext.evaluate(this.parser, outerContext);
        if (predicateEvaluationResult) {
          succeeded.add(c);
        } else {
          failed.add(c);
        }
      } else {
        succeeded.add(c);
      }
    }
    return [succeeded, failed];
  }

  /**
   * Look through a list of predicate/alt pairs, returning alts for the
   * pairs that win. A {@code NONE} predicate indicates an alt containing an
   * unpredicated config which behaves as "always true." If !complete
   * then we stop at the first predicate that evaluates to true. This
   * includes pairs with null predicates.
   */
  evalSemanticContext(predPredictions, outerContext, complete) {
    let predictions = new BitSet();
    for (let i = 0; i < predPredictions.length; i++) {
      let pair = predPredictions[i];
      if (pair.pred === SemanticContext.NONE) {
        predictions.set(pair.alt);
        if (!complete) {
          break;
        }
        continue;
      }
      let predicateEvaluationResult = pair.pred.evaluate(this.parser, outerContext);
      if (this.debug || this.dfa_debug) {
        console.log("eval pred " + pair + "=" + predicateEvaluationResult);
      }
      if (predicateEvaluationResult) {
        if (this.debug || this.dfa_debug) {
          console.log("PREDICT " + pair.alt);
        }
        predictions.set(pair.alt);
        if (!complete) {
          break;
        }
      }
    }
    return predictions;
  }

  // TODO: If we are doing predicates, there is no point in pursuing
  //     closure operations if we reach a DFA state that uniquely predicts
  //     alternative. We will not be caching that DFA state and it is a
  //     waste to pursue the closure. Might have to advance when we do
  //     ambig detection thought :(
  //
  closure(config, configs, closureBusy, collectPredicates, fullCtx, treatEofAsEpsilon) {
    let initialDepth = 0;
    this.closureCheckingStopState(config, configs, closureBusy, collectPredicates, fullCtx, initialDepth, treatEofAsEpsilon);
  }
  closureCheckingStopState(config, configs, closureBusy, collectPredicates, fullCtx, depth, treatEofAsEpsilon) {
    if (this.trace_atn_sim || this.debug_closure) {
      console.log("closure(" + config.toString(this.parser, true) + ")");
    }
    if (config.state instanceof RuleStopState) {
      // We hit rule end. If we have context info, use it
      // run thru all possible stack tops in ctx
      if (!config.context.isEmpty()) {
        for (let i = 0; i < config.context.length; i++) {
          if (config.context.getReturnState(i) === PredictionContext.EMPTY_RETURN_STATE) {
            if (fullCtx) {
              configs.add(new ATNConfig({
                state: config.state,
                context: PredictionContext.EMPTY
              }, config), this.mergeCache);
              continue;
            } else {
              // we have no context info, just chase follow links (if greedy)
              if (this.debug) {
                console.log("FALLING off rule " + this.getRuleName(config.state.ruleIndex));
              }
              this.closure_(config, configs, closureBusy, collectPredicates, fullCtx, depth, treatEofAsEpsilon);
            }
            continue;
          }
          let returnState = this.atn.states[config.context.getReturnState(i)];
          let newContext = config.context.getParent(i); // "pop" return state
          let parms = {
            state: returnState,
            alt: config.alt,
            context: newContext,
            semanticContext: config.semanticContext
          };
          let c = new ATNConfig(parms, null);
          // While we have context to pop back from, we may have
          // gotten that context AFTER having falling off a rule.
          // Make sure we track that we are now out of context.
          c.reachesIntoOuterContext = config.reachesIntoOuterContext;
          this.closureCheckingStopState(c, configs, closureBusy, collectPredicates, fullCtx, depth - 1, treatEofAsEpsilon);
        }
        return;
      } else if (fullCtx) {
        // reached end of start rule
        configs.add(config, this.mergeCache);
        return;
      } else {
        // else if we have no context info, just chase follow links (if greedy)
        if (this.debug) {
          console.log("FALLING off rule " + this.getRuleName(config.state.ruleIndex));
        }
      }
    }
    this.closure_(config, configs, closureBusy, collectPredicates, fullCtx, depth, treatEofAsEpsilon);
  }

  // Do the actual work of walking epsilon edges//
  closure_(config, configs, closureBusy, collectPredicates, fullCtx, depth, treatEofAsEpsilon) {
    let p = config.state;
    // optimization
    if (!p.epsilonOnlyTransitions) {
      configs.add(config, this.mergeCache);
      // make sure to not return here, because EOF transitions can act as
      // both epsilon transitions and non-epsilon transitions.
    }
    for (let i = 0; i < p.transitions.length; i++) {
      if (i === 0 && this.canDropLoopEntryEdgeInLeftRecursiveRule(config)) continue;
      let t = p.transitions[i];
      let continueCollecting = collectPredicates && !(t instanceof ActionTransition);
      let c = this.getEpsilonTarget(config, t, continueCollecting, depth === 0, fullCtx, treatEofAsEpsilon);
      if (c !== null) {
        let newDepth = depth;
        if (config.state instanceof RuleStopState) {
          // target fell off end of rule; mark resulting c as having dipped into outer context
          // We can't get here if incoming config was rule stop and we had context
          // track how far we dip into outer context.  Might
          // come in handy and we avoid evaluating context dependent
          // preds if this is > 0.
          if (this._dfa !== null && this._dfa.precedenceDfa) {
            if (t.outermostPrecedenceReturn === this._dfa.atnStartState.ruleIndex) {
              c.precedenceFilterSuppressed = true;
            }
          }
          c.reachesIntoOuterContext += 1;
          if (closureBusy.getOrAdd(c) !== c) {
            // avoid infinite recursion for right-recursive rules
            continue;
          }
          configs.dipsIntoOuterContext = true; // TODO: can remove? only care when we add to set per middle of this method
          newDepth -= 1;
          if (this.debug) {
            console.log("dips into outer ctx: " + c);
          }
        } else {
          if (!t.isEpsilon && closureBusy.getOrAdd(c) !== c) {
            // avoid infinite recursion for EOF* and EOF+
            continue;
          }
          if (t instanceof RuleTransition) {
            // latch when newDepth goes negative - once we step out of the entry context we can't return
            if (newDepth >= 0) {
              newDepth += 1;
            }
          }
        }
        this.closureCheckingStopState(c, configs, closureBusy, continueCollecting, fullCtx, newDepth, treatEofAsEpsilon);
      }
    }
  }
  canDropLoopEntryEdgeInLeftRecursiveRule(config) {
    // return False
    let p = config.state;
    // First check to see if we are in StarLoopEntryState generated during
    // left-recursion elimination. For efficiency, also check if
    // the context has an empty stack case. If so, it would mean
    // global FOLLOW so we can't perform optimization
    // Are we the special loop entry/exit state? or SLL wildcard
    if (p.stateType !== ATNState.STAR_LOOP_ENTRY) return false;
    if (p.stateType !== ATNState.STAR_LOOP_ENTRY || !p.isPrecedenceDecision || config.context.isEmpty() || config.context.hasEmptyPath()) return false;

    // Require all return states to return back to the same rule that p is in.
    let numCtxs = config.context.length;
    for (let i = 0; i < numCtxs; i++) {
      // for each stack context
      let returnState = this.atn.states[config.context.getReturnState(i)];
      if (returnState.ruleIndex !== p.ruleIndex) return false;
    }
    let decisionStartState = p.transitions[0].target;
    let blockEndStateNum = decisionStartState.endState.stateNumber;
    let blockEndState = this.atn.states[blockEndStateNum];

    // Verify that the top of each stack context leads to loop entry/exit
    // state through epsilon edges and w/o leaving rule.
    for (let i = 0; i < numCtxs; i++) {
      // for each stack context
      let returnStateNumber = config.context.getReturnState(i);
      let returnState = this.atn.states[returnStateNumber];
      // all states must have single outgoing epsilon edge
      if (returnState.transitions.length !== 1 || !returnState.transitions[0].isEpsilon) return false;

      // Look for prefix op case like 'not expr', (' type ')' expr
      let returnStateTarget = returnState.transitions[0].target;
      if (returnState.stateType === ATNState.BLOCK_END && returnStateTarget === p) continue;

      // Look for 'expr op expr' or case where expr's return state is block end
      // of (...)* internal block; the block end points to loop back
      // which points to p but we don't need to check that
      if (returnState === blockEndState) continue;

      // Look for ternary expr ? expr : expr. The return state points at block end,
      // which points at loop entry state
      if (returnStateTarget === blockEndState) continue;

      // Look for complex prefix 'between expr and expr' case where 2nd expr's
      // return state points at block end state of (...)* internal block
      if (returnStateTarget.stateType === ATNState.BLOCK_END && returnStateTarget.transitions.length === 1 && returnStateTarget.transitions[0].isEpsilon && returnStateTarget.transitions[0].target === p) continue;

      // anything else ain't conforming
      return false;
    }
    return true;
  }
  getRuleName(index) {
    if (this.parser !== null && index >= 0) {
      return this.parser.ruleNames[index];
    } else {
      return "<rule " + index + ">";
    }
  }
  getEpsilonTarget(config, t, collectPredicates, inContext, fullCtx, treatEofAsEpsilon) {
    switch (t.serializationType) {
      case Transition.RULE:
        return this.ruleTransition(config, t);
      case Transition.PRECEDENCE:
        return this.precedenceTransition(config, t, collectPredicates, inContext, fullCtx);
      case Transition.PREDICATE:
        return this.predTransition(config, t, collectPredicates, inContext, fullCtx);
      case Transition.ACTION:
        return this.actionTransition(config, t);
      case Transition.EPSILON:
        return new ATNConfig({
          state: t.target
        }, config);
      case Transition.ATOM:
      case Transition.RANGE:
      case Transition.SET:
        // EOF transitions act like epsilon transitions after the first EOF
        // transition is traversed
        if (treatEofAsEpsilon) {
          if (t.matches(Token.EOF, 0, 1)) {
            return new ATNConfig({
              state: t.target
            }, config);
          }
        }
        return null;
      default:
        return null;
    }
  }
  actionTransition(config, t) {
    if (this.debug) {
      let index = t.actionIndex === -1 ? 65535 : t.actionIndex;
      console.log("ACTION edge " + t.ruleIndex + ":" + index);
    }
    return new ATNConfig({
      state: t.target
    }, config);
  }
  precedenceTransition(config, pt, collectPredicates, inContext, fullCtx) {
    if (this.debug) {
      console.log("PRED (collectPredicates=" + collectPredicates + ") " + pt.precedence + ">=_p, ctx dependent=true");
      if (this.parser !== null) {
        console.log("context surrounding pred is " + arrayToString(this.parser.getRuleInvocationStack()));
      }
    }
    let c = null;
    if (collectPredicates && inContext) {
      if (fullCtx) {
        // In full context mode, we can evaluate predicates on-the-fly
        // during closure, which dramatically reduces the size of
        // the config sets. It also obviates the need to test predicates
        // later during conflict resolution.
        let currentPosition = this._input.index;
        this._input.seek(this._startIndex);
        let predSucceeds = pt.getPredicate().evaluate(this.parser, this._outerContext);
        this._input.seek(currentPosition);
        if (predSucceeds) {
          c = new ATNConfig({
            state: pt.target
          }, config); // no pred context
        }
      } else {
        let newSemCtx = SemanticContext.andContext(config.semanticContext, pt.getPredicate());
        c = new ATNConfig({
          state: pt.target,
          semanticContext: newSemCtx
        }, config);
      }
    } else {
      c = new ATNConfig({
        state: pt.target
      }, config);
    }
    if (this.debug) {
      console.log("config from pred transition=" + c);
    }
    return c;
  }
  predTransition(config, pt, collectPredicates, inContext, fullCtx) {
    if (this.debug) {
      console.log("PRED (collectPredicates=" + collectPredicates + ") " + pt.ruleIndex + ":" + pt.predIndex + ", ctx dependent=" + pt.isCtxDependent);
      if (this.parser !== null) {
        console.log("context surrounding pred is " + arrayToString(this.parser.getRuleInvocationStack()));
      }
    }
    let c = null;
    if (collectPredicates && (pt.isCtxDependent && inContext || !pt.isCtxDependent)) {
      if (fullCtx) {
        // In full context mode, we can evaluate predicates on-the-fly
        // during closure, which dramatically reduces the size of
        // the config sets. It also obviates the need to test predicates
        // later during conflict resolution.
        let currentPosition = this._input.index;
        this._input.seek(this._startIndex);
        let predSucceeds = pt.getPredicate().evaluate(this.parser, this._outerContext);
        this._input.seek(currentPosition);
        if (predSucceeds) {
          c = new ATNConfig({
            state: pt.target
          }, config); // no pred context
        }
      } else {
        let newSemCtx = SemanticContext.andContext(config.semanticContext, pt.getPredicate());
        c = new ATNConfig({
          state: pt.target,
          semanticContext: newSemCtx
        }, config);
      }
    } else {
      c = new ATNConfig({
        state: pt.target
      }, config);
    }
    if (this.debug) {
      console.log("config from pred transition=" + c);
    }
    return c;
  }
  ruleTransition(config, t) {
    if (this.debug) {
      console.log("CALL rule " + this.getRuleName(t.target.ruleIndex) + ", ctx=" + config.context);
    }
    let returnState = t.followState;
    let newContext = SingletonPredictionContext.create(config.context, returnState.stateNumber);
    return new ATNConfig({
      state: t.target,
      context: newContext
    }, config);
  }
  getConflictingAlts(configs) {
    let altsets = atn_PredictionMode.getConflictingAltSubsets(configs);
    return atn_PredictionMode.getAlts(altsets);
  }

  /**
   * Sam pointed out a problem with the previous definition, v3, of
   * ambiguous states. If we have another state associated with conflicting
   * alternatives, we should keep going. For example, the following grammar
   *
   * s : (ID | ID ID?) ';' ;
   *
   * When the ATN simulation reaches the state before ';', it has a DFA
   * state that looks like: [12|1|[], 6|2|[], 12|2|[]]. Naturally
   * 12|1|[] and 12|2|[] conflict, but we cannot stop processing this node
   * because alternative to has another way to continue, via [6|2|[]].
   * The key is that we have a single state that has config's only associated
   * with a single alternative, 2, and crucially the state transitions
   * among the configurations are all non-epsilon transitions. That means
   * we don't consider any conflicts that include alternative 2. So, we
   * ignore the conflict between alts 1 and 2. We ignore a set of
   * conflicting alts when there is an intersection with an alternative
   * associated with a single alt state in the state&rarr;config-list map.
   *
   * It's also the case that we might have two conflicting configurations but
   * also a 3rd nonconflicting configuration for a different alternative:
   * [1|1|[], 1|2|[], 8|3|[]]. This can come about from grammar:
   *
   * a : A | A | A B ;
   *
   * After matching input A, we reach the stop state for rule A, state 1.
   * State 8 is the state right before B. Clearly alternatives 1 and 2
   * conflict and no amount of further lookahead will separate the two.
   * However, alternative 3 will be able to continue and so we do not
   * stop working on this state. In the previous example, we're concerned
   * with states associated with the conflicting alternatives. Here alt
   * 3 is not associated with the conflicting configs, but since we can continue
   * looking for input reasonably, I don't declare the state done. We
   * ignore a set of conflicting alts when we have an alternative
   * that we still need to pursue
   */
  getConflictingAltsOrUniqueAlt(configs) {
    let conflictingAlts = null;
    if (configs.uniqueAlt !== ATN.INVALID_ALT_NUMBER) {
      conflictingAlts = new BitSet();
      conflictingAlts.set(configs.uniqueAlt);
    } else {
      conflictingAlts = configs.conflictingAlts;
    }
    return conflictingAlts;
  }
  getTokenName(t) {
    if (t === Token.EOF) {
      return "EOF";
    }
    if (this.parser !== null && this.parser.literalNames !== null) {
      if (t >= this.parser.literalNames.length && t >= this.parser.symbolicNames.length) {
        console.log("" + t + " ttype out of range: " + this.parser.literalNames);
        console.log("" + this.parser.getInputStream().getTokens());
      } else {
        let name = this.parser.literalNames[t] || this.parser.symbolicNames[t];
        return name + "<" + t + ">";
      }
    }
    return "" + t;
  }
  getLookaheadName(input) {
    return this.getTokenName(input.LA(1));
  }

  /**
   * Used for debugging in adaptivePredict around execATN but I cut
   * it out for clarity now that alg. works well. We can leave this
   * "dead" code for a bit
   */
  dumpDeadEndConfigs(nvae) {
    console.log("dead end configs: ");
    let decs = nvae.getDeadEndConfigs();
    for (let i = 0; i < decs.length; i++) {
      let c = decs[i];
      let trans = "no edges";
      if (c.state.transitions.length > 0) {
        let t = c.state.transitions[0];
        if (t instanceof AtomTransition) {
          trans = "Atom " + this.getTokenName(t.label);
        } else if (t instanceof SetTransition) {
          let neg = t instanceof NotSetTransition;
          trans = (neg ? "~" : "") + "Set " + t.set;
        }
      }
      console.error(c.toString(this.parser, true) + ":" + trans);
    }
  }
  noViableAlt(input, outerContext, configs, startIndex) {
    return new NoViableAltException(this.parser, input, input.get(startIndex), input.LT(1), configs, outerContext);
  }
  getUniqueAlt(configs) {
    let alt = ATN.INVALID_ALT_NUMBER;
    for (let i = 0; i < configs.items.length; i++) {
      let c = configs.items[i];
      if (alt === ATN.INVALID_ALT_NUMBER) {
        alt = c.alt; // found first alt
      } else if (c.alt !== alt) {
        return ATN.INVALID_ALT_NUMBER;
      }
    }
    return alt;
  }

  /**
   * Add an edge to the DFA, if possible. This method calls
   * {@link //addDFAState} to ensure the {@code to} state is present in the
   * DFA. If {@code from} is {@code null}, or if {@code t} is outside the
   * range of edges that can be represented in the DFA tables, this method
   * returns without adding the edge to the DFA.
   *
   * <p>If {@code to} is {@code null}, this method returns {@code null}.
   * Otherwise, this method returns the {@link DFAState} returned by calling
   * {@link //addDFAState} for the {@code to} state.</p>
   *
   * @param dfa The DFA
   * @param from_ The source state for the edge
   * @param t The input symbol
   * @param to The target state for the edge
   *
   * @return If {@code to} is {@code null}, this method returns {@code null};
   * otherwise this method returns the result of calling {@link //addDFAState}
   * on {@code to}
   */
  addDFAEdge(dfa, from_, t, to) {
    if (this.debug) {
      console.log("EDGE " + from_ + " -> " + to + " upon " + this.getTokenName(t));
    }
    if (to === null) {
      return null;
    }
    to = this.addDFAState(dfa, to); // used existing if possible not incoming
    if (from_ === null || t < -1 || t > this.atn.maxTokenType) {
      return to;
    }
    if (from_.edges === null) {
      from_.edges = [];
    }
    from_.edges[t + 1] = to; // connect

    if (this.debug) {
      let literalNames = this.parser === null ? null : this.parser.literalNames;
      let symbolicNames = this.parser === null ? null : this.parser.symbolicNames;
      console.log("DFA=\n" + dfa.toString(literalNames, symbolicNames));
    }
    return to;
  }

  /**
   * Add state {@code D} to the DFA if it is not already present, and return
   * the actual instance stored in the DFA. If a state equivalent to {@code D}
   * is already in the DFA, the existing state is returned. Otherwise this
   * method returns {@code D} after adding it to the DFA.
   *
   * <p>If {@code D} is {@link //ERROR}, this method returns {@link //ERROR} and
   * does not change the DFA.</p>
   *
   * @param dfa The dfa
   * @param D The DFA state to add
   * @return The state stored in the DFA. This will be either the existing
   * state if {@code D} is already in the DFA, or {@code D} itself if the
   * state was not already present
   */
  addDFAState(dfa, D) {
    if (D === ATNSimulator.ERROR) {
      return D;
    }
    let existing = dfa.states.get(D);
    if (existing !== null) {
      if (this.trace_atn_sim) console.log("addDFAState " + D + " exists");
      return existing;
    }
    D.stateNumber = dfa.states.length;
    if (!D.configs.readOnly) {
      D.configs.optimizeConfigs(this);
      D.configs.setReadonly(true);
    }
    if (this.trace_atn_sim) console.log("addDFAState new " + D);
    dfa.states.add(D);
    if (this.debug) {
      console.log("adding new DFA state: " + D);
    }
    return D;
  }
  reportAttemptingFullContext(dfa, conflictingAlts, configs, startIndex, stopIndex) {
    if (this.debug || this.retry_debug) {
      let interval = new Interval(startIndex, stopIndex + 1);
      console.log("reportAttemptingFullContext decision=" + dfa.decision + ":" + configs + ", input=" + this.parser.getTokenStream().getText(interval));
    }
    if (this.parser !== null) {
      this.parser.getErrorListener().reportAttemptingFullContext(this.parser, dfa, startIndex, stopIndex, conflictingAlts, configs);
    }
  }
  reportContextSensitivity(dfa, prediction, configs, startIndex, stopIndex) {
    if (this.debug || this.retry_debug) {
      let interval = new Interval(startIndex, stopIndex + 1);
      console.log("reportContextSensitivity decision=" + dfa.decision + ":" + configs + ", input=" + this.parser.getTokenStream().getText(interval));
    }
    if (this.parser !== null) {
      this.parser.getErrorListener().reportContextSensitivity(this.parser, dfa, startIndex, stopIndex, prediction, configs);
    }
  }

  // If context sensitive parsing, we know it's ambiguity not conflict//
  reportAmbiguity(dfa, D, startIndex, stopIndex, exact, ambigAlts, configs) {
    if (this.debug || this.retry_debug) {
      let interval = new Interval(startIndex, stopIndex + 1);
      console.log("reportAmbiguity " + ambigAlts + ":" + configs + ", input=" + this.parser.getTokenStream().getText(interval));
    }
    if (this.parser !== null) {
      this.parser.getErrorListener().reportAmbiguity(this.parser, dfa, startIndex, stopIndex, exact, ambigAlts, configs);
    }
  }
}
;// ./src/antlr4/atn/PredictionContextCache.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Used to cache {@link PredictionContext} objects. Its used for the shared
 * context cash associated with contexts in DFA states. This cache
 * can be used for both lexers and parsers.
 */
class PredictionContextCache {
  constructor() {
    this.cache = new HashMap_HashMap();
  }

  /**
   * Add a context to the cache and return it. If the context already exists,
   * return that one instead and do not add a new context to the cache.
   * Protect shared cache from unsafe thread access.
   */
  add(ctx) {
    if (ctx === PredictionContext.EMPTY) {
      return PredictionContext.EMPTY;
    }
    let existing = this.cache.get(ctx) || null;
    if (existing !== null) {
      return existing;
    }
    this.cache.set(ctx, ctx);
    return ctx;
  }
  get(ctx) {
    return this.cache.get(ctx) || null;
  }
  get length() {
    return this.cache.length;
  }
}
;// ./src/antlr4/atn/index.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */







/* harmony default export */ const atn = ({
  ATN: ATN,
  ATNDeserializer: ATNDeserializer,
  LexerATNSimulator: LexerATNSimulator,
  ParserATNSimulator: ParserATNSimulator,
  PredictionMode: atn_PredictionMode,
  PredictionContextCache: PredictionContextCache
});
;// ./src/antlr4/dfa/DFASerializer.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * A DFA walker that knows how to dump them to serialized strings.
 */
class DFASerializer {
  constructor(dfa, literalNames, symbolicNames) {
    this.dfa = dfa;
    this.literalNames = literalNames || [];
    this.symbolicNames = symbolicNames || [];
  }
  toString() {
    if (this.dfa.s0 === null) {
      return null;
    }
    let buf = "";
    let states = this.dfa.sortedStates();
    for (let i = 0; i < states.length; i++) {
      let s = states[i];
      if (s.edges !== null) {
        let n = s.edges.length;
        for (let j = 0; j < n; j++) {
          let t = s.edges[j] || null;
          if (t !== null && t.stateNumber !== 0x7FFFFFFF) {
            buf = buf.concat(this.getStateString(s));
            buf = buf.concat("-");
            buf = buf.concat(this.getEdgeLabel(j));
            buf = buf.concat("->");
            buf = buf.concat(this.getStateString(t));
            buf = buf.concat('\n');
          }
        }
      }
    }
    return buf.length === 0 ? null : buf;
  }
  getEdgeLabel(i) {
    if (i === 0) {
      return "EOF";
    } else if (this.literalNames !== null || this.symbolicNames !== null) {
      return this.literalNames[i - 1] || this.symbolicNames[i - 1];
    } else {
      return String.fromCharCode(i - 1);
    }
  }
  getStateString(s) {
    let baseStateStr = (s.isAcceptState ? ":" : "") + "s" + s.stateNumber + (s.requiresFullContext ? "^" : "");
    if (s.isAcceptState) {
      if (s.predicates !== null) {
        return baseStateStr + "=>" + arrayToString(s.predicates);
      } else {
        return baseStateStr + "=>" + s.prediction.toString();
      }
    } else {
      return baseStateStr;
    }
  }
}
;// ./src/antlr4/dfa/LexerDFASerializer.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class LexerDFASerializer extends DFASerializer {
  constructor(dfa) {
    super(dfa, null);
  }
  getEdgeLabel(i) {
    return "'" + String.fromCharCode(i) + "'";
  }
}
;// ./src/antlr4/dfa/DFA.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */







class DFA {
  constructor(atnStartState, decision) {
    if (decision === undefined) {
      decision = 0;
    }
    /**
     * From which ATN state did we create this DFA?
     */
    this.atnStartState = atnStartState;
    this.decision = decision;
    /**
     * A set of all DFA states. Use {@link Map} so we can get old state back
     * ({@link Set} only allows you to see if it's there).
     */
    this._states = new HashSet();
    this.s0 = null;
    /**
     * {@code true} if this DFA is for a precedence decision; otherwise,
     * {@code false}. This is the backing field for {@link //isPrecedenceDfa},
     * {@link //setPrecedenceDfa}
     */
    this.precedenceDfa = false;
    if (atnStartState instanceof StarLoopEntryState) {
      if (atnStartState.isPrecedenceDecision) {
        this.precedenceDfa = true;
        let precedenceState = new DFAState(null, new ATNConfigSet());
        precedenceState.edges = [];
        precedenceState.isAcceptState = false;
        precedenceState.requiresFullContext = false;
        this.s0 = precedenceState;
      }
    }
  }

  /**
   * Get the start state for a specific precedence value.
   *
   * @param precedence The current precedence.
   * @return The start state corresponding to the specified precedence, or
   * {@code null} if no start state exists for the specified precedence.
   *
   * @throws IllegalStateException if this is not a precedence DFA.
   * @see //isPrecedenceDfa()
   */
  getPrecedenceStartState(precedence) {
    if (!this.precedenceDfa) {
      throw "Only precedence DFAs may contain a precedence start state.";
    }
    // s0.edges is never null for a precedence DFA
    if (precedence < 0 || precedence >= this.s0.edges.length) {
      return null;
    }
    return this.s0.edges[precedence] || null;
  }

  /**
   * Set the start state for a specific precedence value.
   *
   * @param precedence The current precedence.
   * @param startState The start state corresponding to the specified
   * precedence.
   *
   * @throws IllegalStateException if this is not a precedence DFA.
   * @see //isPrecedenceDfa()
   */
  setPrecedenceStartState(precedence, startState) {
    if (!this.precedenceDfa) {
      throw "Only precedence DFAs may contain a precedence start state.";
    }
    if (precedence < 0) {
      return;
    }

    /**
     * synchronization on s0 here is ok. when the DFA is turned into a
     * precedence DFA, s0 will be initialized once and not updated again
     * s0.edges is never null for a precedence DFA
     */
    this.s0.edges[precedence] = startState;
  }

  /**
   * Sets whether this is a precedence DFA. If the specified value differs
   * from the current DFA configuration, the following actions are taken;
   * otherwise no changes are made to the current DFA.
   *
   * <ul>
   * <li>The {@link //states} map is cleared</li>
   * <li>If {@code precedenceDfa} is {@code false}, the initial state
   * {@link //s0} is set to {@code null}; otherwise, it is initialized to a new
   * {@link DFAState} with an empty outgoing {@link DFAState//edges} array to
   * store the start states for individual precedence values.</li>
   * <li>The {@link //precedenceDfa} field is updated</li>
   * </ul>
   *
   * @param precedenceDfa {@code true} if this is a precedence DFA; otherwise,
   * {@code false}
   */
  setPrecedenceDfa(precedenceDfa) {
    if (this.precedenceDfa !== precedenceDfa) {
      this._states = new HashSet();
      if (precedenceDfa) {
        let precedenceState = new DFAState(null, new ATNConfigSet());
        precedenceState.edges = [];
        precedenceState.isAcceptState = false;
        precedenceState.requiresFullContext = false;
        this.s0 = precedenceState;
      } else {
        this.s0 = null;
      }
      this.precedenceDfa = precedenceDfa;
    }
  }

  /**
   * Return a list of all states in this DFA, ordered by state number.
   */
  sortedStates() {
    let list = this._states.values();
    return list.sort(function (a, b) {
      return a.stateNumber - b.stateNumber;
    });
  }
  toString(literalNames, symbolicNames) {
    literalNames = literalNames || null;
    symbolicNames = symbolicNames || null;
    if (this.s0 === null) {
      return "";
    }
    let serializer = new DFASerializer(this, literalNames, symbolicNames);
    return serializer.toString();
  }
  toLexerString() {
    if (this.s0 === null) {
      return "";
    }
    let serializer = new LexerDFASerializer(this);
    return serializer.toString();
  }
  get states() {
    return this._states;
  }
}
;// ./src/antlr4/dfa/index.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */





/* harmony default export */ const dfa = ({
  DFA: DFA,
  DFASerializer: DFASerializer,
  LexerDFASerializer: LexerDFASerializer,
  PredPrediction: PredPrediction
});
;// ./src/antlr4/context/index.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

/* harmony default export */ const context = ({
  PredictionContext: PredictionContext
});
;// ./src/antlr4/misc/index.js



/* harmony default export */ const misc = ({
  BitSet: BitSet,
  Interval: Interval,
  IntervalSet: IntervalSet
});
;// ./src/antlr4/tree/ParseTreeListener.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
class ParseTreeListener {
  visitTerminal(node) {}
  visitErrorNode(node) {}
  enterEveryRule(node) {}
  exitEveryRule(node) {}
}
;// ./src/antlr4/tree/ParseTreeVisitor.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
class ParseTreeVisitor {
  visit(ctx) {
    if (Array.isArray(ctx)) {
      return ctx.map(function (child) {
        return child.accept(this);
      }, this);
    } else {
      return ctx.accept(this);
    }
  }
  visitChildren(ctx) {
    if (ctx.children) {
      return this.visit(ctx.children);
    } else {
      return null;
    }
  }
  visitTerminal(node) {}
  visitErrorNode(node) {}
}
;// ./src/antlr4/tree/ParseTreeWalker.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


class ParseTreeWalker {
  /**
   * Performs a walk on the given parse tree starting at the root and going down recursively
   * with depth-first search. On each node, {@link ParseTreeWalker//enterRule} is called before
   * recursively walking down into child nodes, then
   * {@link ParseTreeWalker//exitRule} is called after the recursive call to wind up.
   * @param listener The listener used by the walker to process grammar rules
   * @param t The parse tree to be walked on
   */
  walk(listener, t) {
    let errorNode = t instanceof ErrorNode || t.isErrorNode !== undefined && t.isErrorNode();
    if (errorNode) {
      listener.visitErrorNode(t);
    } else if (t instanceof TerminalNode) {
      listener.visitTerminal(t);
    } else {
      this.enterRule(listener, t);
      for (let i = 0; i < t.getChildCount(); i++) {
        let child = t.getChild(i);
        this.walk(listener, child);
      }
      this.exitRule(listener, t);
    }
  }

  /**
   * Enters a grammar rule by first triggering the generic event {@link ParseTreeListener//enterEveryRule}
   * then by triggering the event specific to the given parse tree node
   * @param listener The listener responding to the trigger events
   * @param r The grammar rule containing the rule context
   */
  enterRule(listener, r) {
    let ctx = r.ruleContext;
    listener.enterEveryRule(ctx);
    ctx.enterRule(listener);
  }

  /**
   * Exits a grammar rule by first triggering the event specific to the given parse tree node
   * then by triggering the generic event {@link ParseTreeListener//exitEveryRule}
   * @param listener The listener responding to the trigger events
   * @param r The grammar rule containing the rule context
   */
  exitRule(listener, r) {
    let ctx = r.ruleContext;
    ctx.exitRule(listener);
    listener.exitEveryRule(ctx);
  }
}
ParseTreeWalker.DEFAULT = new ParseTreeWalker();
;// ./src/antlr4/tree/index.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */








/* harmony default export */ const tree = ({
  Trees: tree_Trees,
  RuleNode: RuleNode,
  ErrorNode: ErrorNode,
  TerminalNode: TerminalNode,
  ParseTreeListener: ParseTreeListener,
  ParseTreeVisitor: ParseTreeVisitor,
  ParseTreeWalker: ParseTreeWalker
});
;// ./src/antlr4/error/InputMismatchException.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


/**
 * This signifies any kind of mismatched input exceptions such as
 * when the current input does not match the expected token.
 */
class InputMismatchException extends RecognitionException {
  constructor(recognizer) {
    super({
      message: "",
      recognizer: recognizer,
      input: recognizer.getInputStream(),
      ctx: recognizer._ctx
    });
    this.offendingToken = recognizer.getCurrentToken();
  }
}
;// ./src/antlr4/error/FailedPredicateException.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * A semantic predicate failed during validation. Validation of predicates
 * occurs when normally parsing the alternative just like matching a token.
 * Disambiguating predicate evaluation occurs when we test a predicate during
 * prediction.
 */
class FailedPredicateException extends RecognitionException {
  constructor(recognizer, predicate, message) {
    super({
      message: formatMessage(predicate, message || null),
      recognizer: recognizer,
      input: recognizer.getInputStream(),
      ctx: recognizer._ctx
    });
    let s = recognizer._interp.atn.states[recognizer.state];
    let trans = s.transitions[0];
    if (trans instanceof PredicateTransition) {
      this.ruleIndex = trans.ruleIndex;
      this.predicateIndex = trans.predIndex;
    } else {
      this.ruleIndex = 0;
      this.predicateIndex = 0;
    }
    this.predicate = predicate;
    this.offendingToken = recognizer.getCurrentToken();
  }
}
function formatMessage(predicate, message) {
  if (message !== null) {
    return message;
  } else {
    return "failed predicate: {" + predicate + "}?";
  }
}
;// ./src/antlr4/error/DiagnosticErrorListener.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */





/**
 * This implementation of {@link ANTLRErrorListener} can be used to identify
 *  certain potential correctness and performance problems in grammars. "Reports"
 *  are made by calling {@link Parser//notifyErrorListeners} with the appropriate
 *  message.
 *
 *  <ul>
 *  <li><b>Ambiguities</b>: These are cases where more than one path through the
 *  grammar can match the input.</li>
 *  <li><b>Weak context sensitivity</b>: These are cases where full-context
 *  prediction resolved an SLL conflict to a unique alternative which equaled the
 *  minimum alternative of the SLL conflict.</li>
 *  <li><b>Strong (forced) context sensitivity</b>: These are cases where the
 *  full-context prediction resolved an SLL conflict to a unique alternative,
 *  <em>and</em> the minimum alternative of the SLL conflict was found to not be
 *  a truly viable alternative. Two-stage parsing cannot be used for inputs where
 *  this situation occurs.</li>
 *  </ul>
 */
class DiagnosticErrorListener extends ErrorListener {
  constructor(exactOnly) {
    super();
    exactOnly = exactOnly || true;
    // whether all ambiguities or only exact ambiguities are reported.
    this.exactOnly = exactOnly;
  }
  reportAmbiguity(recognizer, dfa, startIndex, stopIndex, exact, ambigAlts, configs) {
    if (this.exactOnly && !exact) {
      return;
    }
    let msg = "reportAmbiguity d=" + this.getDecisionDescription(recognizer, dfa) + ": ambigAlts=" + this.getConflictingAlts(ambigAlts, configs) + ", input='" + recognizer.getTokenStream().getText(new Interval(startIndex, stopIndex)) + "'";
    recognizer.notifyErrorListeners(msg);
  }
  reportAttemptingFullContext(recognizer, dfa, startIndex, stopIndex, conflictingAlts, configs) {
    let msg = "reportAttemptingFullContext d=" + this.getDecisionDescription(recognizer, dfa) + ", input='" + recognizer.getTokenStream().getText(new Interval(startIndex, stopIndex)) + "'";
    recognizer.notifyErrorListeners(msg);
  }
  reportContextSensitivity(recognizer, dfa, startIndex, stopIndex, prediction, configs) {
    let msg = "reportContextSensitivity d=" + this.getDecisionDescription(recognizer, dfa) + ", input='" + recognizer.getTokenStream().getText(new Interval(startIndex, stopIndex)) + "'";
    recognizer.notifyErrorListeners(msg);
  }
  getDecisionDescription(recognizer, dfa) {
    let decision = dfa.decision;
    let ruleIndex = dfa.atnStartState.ruleIndex;
    let ruleNames = recognizer.ruleNames;
    if (ruleIndex < 0 || ruleIndex >= ruleNames.length) {
      return "" + decision;
    }
    let ruleName = ruleNames[ruleIndex] || null;
    if (ruleName === null || ruleName.length === 0) {
      return "" + decision;
    }
    return `${decision} (${ruleName})`;
  }

  /**
   * Computes the set of conflicting or ambiguous alternatives from a
   * configuration set, if that information was not already provided by the
   * parser.
   *
   * @param reportedAlts The set of conflicting or ambiguous alternatives, as
   * reported by the parser.
   * @param configs The conflicting or ambiguous configuration set.
   * @return Returns {@code reportedAlts} if it is not {@code null}, otherwise
   * returns the set of alternatives represented in {@code configs}.
      */
  getConflictingAlts(reportedAlts, configs) {
    if (reportedAlts !== null) {
      return reportedAlts;
    }
    let result = new BitSet();
    for (let i = 0; i < configs.items.length; i++) {
      result.set(configs.items[i].alt);
    }
    return `{${result.values().join(", ")}}`;
  }
}
;// ./src/antlr4/error/ParseCancellationException.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
class ParseCancellationException extends Error {
  constructor() {
    super();
    Error.captureStackTrace(this, ParseCancellationException);
  }
}
;// ./src/antlr4/error/ErrorStrategy.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class ErrorStrategy {
  reset(recognizer) {}
  recoverInline(recognizer) {}
  recover(recognizer, e) {}
  sync(recognizer) {}
  inErrorRecoveryMode(recognizer) {}
  reportError(recognizer) {}
}
;// ./src/antlr4/error/DefaultErrorStrategy.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */









/**
 * This is the default implementation of {@link ANTLRErrorStrategy} used for
 * error reporting and recovery in ANTLR parsers.
 */
class DefaultErrorStrategy extends ErrorStrategy {
  constructor() {
    super();
    /**
     * Indicates whether the error strategy is currently "recovering from an
     * error". This is used to suppress reporting multiple error messages while
     * attempting to recover from a detected syntax error.
     *
     * @see //inErrorRecoveryMode
     */
    this.errorRecoveryMode = false;

    /**
     * The index into the input stream where the last error occurred.
     * This is used to prevent infinite loops where an error is found
     * but no token is consumed during recovery...another error is found,
     * ad nauseum. This is a failsafe mechanism to guarantee that at least
     * one token/tree node is consumed for two errors.
     */
    this.lastErrorIndex = -1;
    this.lastErrorStates = null;
    this.nextTokensContext = null;
    this.nextTokenState = 0;
  }

  /**
   * <p>The default implementation simply calls {@link //endErrorCondition} to
   * ensure that the handler is not in error recovery mode.</p>
   */
  reset(recognizer) {
    this.endErrorCondition(recognizer);
  }

  /**
   * This method is called to enter error recovery mode when a recognition
   * exception is reported.
   *
   * @param recognizer the parser instance
   */
  beginErrorCondition(recognizer) {
    this.errorRecoveryMode = true;
  }
  inErrorRecoveryMode(recognizer) {
    return this.errorRecoveryMode;
  }

  /**
   * This method is called to leave error recovery mode after recovering from
   * a recognition exception.
   * @param recognizer
   */
  endErrorCondition(recognizer) {
    this.errorRecoveryMode = false;
    this.lastErrorStates = null;
    this.lastErrorIndex = -1;
  }

  /**
   * {@inheritDoc}
   * <p>The default implementation simply calls {@link //endErrorCondition}.</p>
   */
  reportMatch(recognizer) {
    this.endErrorCondition(recognizer);
  }

  /**
   * {@inheritDoc}
   *
   * <p>The default implementation returns immediately if the handler is already
   * in error recovery mode. Otherwise, it calls {@link //beginErrorCondition}
   * and dispatches the reporting task based on the runtime type of {@code e}
   * according to the following table.</p>
   *
   * <ul>
   * <li>{@link NoViableAltException}: Dispatches the call to
   * {@link //reportNoViableAlternative}</li>
   * <li>{@link InputMismatchException}: Dispatches the call to
   * {@link //reportInputMismatch}</li>
   * <li>{@link FailedPredicateException}: Dispatches the call to
   * {@link //reportFailedPredicate}</li>
   * <li>All other types: calls {@link Parser//notifyErrorListeners} to report
   * the exception</li>
   * </ul>
   */
  reportError(recognizer, e) {
    // if we've already reported an error and have not matched a token
    // yet successfully, don't report any errors.
    if (this.inErrorRecoveryMode(recognizer)) {
      return; // don't report spurious errors
    }
    this.beginErrorCondition(recognizer);
    if (e instanceof NoViableAltException) {
      this.reportNoViableAlternative(recognizer, e);
    } else if (e instanceof InputMismatchException) {
      this.reportInputMismatch(recognizer, e);
    } else if (e instanceof FailedPredicateException) {
      this.reportFailedPredicate(recognizer, e);
    } else {
      console.log("unknown recognition error type: " + e.constructor.name);
      console.log(e.stack);
      recognizer.notifyErrorListeners(e.getOffendingToken(), e.getMessage(), e);
    }
  }

  /**
   *
   * {@inheritDoc}
   *
   * <p>The default implementation resynchronizes the parser by consuming tokens
   * until we find one in the resynchronization set--loosely the set of tokens
   * that can follow the current rule.</p>
   *
   */
  recover(recognizer, e) {
    if (this.lastErrorIndex === recognizer.getInputStream().index && this.lastErrorStates !== null && this.lastErrorStates.indexOf(recognizer.state) >= 0) {
      // uh oh, another error at same token index and previously-visited
      // state in ATN; must be a case where LT(1) is in the recovery
      // token set so nothing got consumed. Consume a single token
      // at least to prevent an infinite loop; this is a failsafe.
      recognizer.consume();
    }
    this.lastErrorIndex = recognizer._input.index;
    if (this.lastErrorStates === null) {
      this.lastErrorStates = [];
    }
    this.lastErrorStates.push(recognizer.state);
    let followSet = this.getErrorRecoverySet(recognizer);
    this.consumeUntil(recognizer, followSet);
  }

  /**
   * The default implementation of {@link ANTLRErrorStrategy//sync} makes sure
   * that the current lookahead symbol is consistent with what were expecting
   * at this point in the ATN. You can call this anytime but ANTLR only
   * generates code to check before subrules/loops and each iteration.
   *
   * <p>Implements Jim Idle's magic sync mechanism in closures and optional
   * subrules. E.g.,</p>
   *
   * <pre>
   * a : sync ( stuff sync )* ;
   * sync : {consume to what can follow sync} ;
   * </pre>
   *
   * At the start of a sub rule upon error, {@link //sync} performs single
   * token deletion, if possible. If it can't do that, it bails on the current
   * rule and uses the default error recovery, which consumes until the
   * resynchronization set of the current rule.
   *
   * <p>If the sub rule is optional ({@code (...)?}, {@code (...)*}, or block
   * with an empty alternative), then the expected set includes what follows
   * the subrule.</p>
   *
   * <p>During loop iteration, it consumes until it sees a token that can start a
   * sub rule or what follows loop. Yes, that is pretty aggressive. We opt to
   * stay in the loop as long as possible.</p>
   *
   * <p><strong>ORIGINS</strong></p>
   *
   * <p>Previous versions of ANTLR did a poor job of their recovery within loops.
   * A single mismatch token or missing token would force the parser to bail
   * out of the entire rules surrounding the loop. So, for rule</p>
   *
   * <pre>
   * classDef : 'class' ID '{' member* '}'
   * </pre>
   *
   * input with an extra token between members would force the parser to
   * consume until it found the next class definition rather than the next
   * member definition of the current class.
   *
   * <p>This functionality cost a little bit of effort because the parser has to
   * compare token set at the start of the loop and at each iteration. If for
   * some reason speed is suffering for you, you can turn off this
   * functionality by simply overriding this method as a blank { }.</p>
   *
   */
  sync(recognizer) {
    // If already recovering, don't try to sync
    if (this.inErrorRecoveryMode(recognizer)) {
      return;
    }
    let s = recognizer._interp.atn.states[recognizer.state];
    let la = recognizer.getTokenStream().LA(1);
    // try cheaper subset first; might get lucky. seems to shave a wee bit off
    let nextTokens = recognizer.atn.nextTokens(s);
    if (nextTokens.contains(la)) {
      this.nextTokensContext = null;
      this.nextTokenState = ATNState.INVALID_STATE_NUMBER;
      return;
    } else if (nextTokens.contains(Token.EPSILON)) {
      if (this.nextTokensContext === null) {
        // It's possible the next token won't match information tracked
        // by sync is restricted for performance.
        this.nextTokensContext = recognizer._ctx;
        this.nextTokensState = recognizer._stateNumber;
      }
      return;
    }
    switch (s.stateType) {
      case ATNState.BLOCK_START:
      case ATNState.STAR_BLOCK_START:
      case ATNState.PLUS_BLOCK_START:
      case ATNState.STAR_LOOP_ENTRY:
        // report error and recover if possible
        if (this.singleTokenDeletion(recognizer) !== null) {
          return;
        } else {
          throw new InputMismatchException(recognizer);
        }
      case ATNState.PLUS_LOOP_BACK:
      case ATNState.STAR_LOOP_BACK:
        {
          this.reportUnwantedToken(recognizer);
          let expecting = new IntervalSet();
          expecting.addSet(recognizer.getExpectedTokens());
          let whatFollowsLoopIterationOrRule = expecting.addSet(this.getErrorRecoverySet(recognizer));
          this.consumeUntil(recognizer, whatFollowsLoopIterationOrRule);
        }
        break;
      default:
      // do nothing if we can't identify the exact kind of ATN state
    }
  }

  /**
   * This is called by {@link //reportError} when the exception is a
   * {@link NoViableAltException}.
   *
   * @see //reportError
   *
   * @param recognizer the parser instance
   * @param e the recognition exception
   */
  reportNoViableAlternative(recognizer, e) {
    let tokens = recognizer.getTokenStream();
    let input;
    if (tokens !== null) {
      if (e.startToken.type === Token.EOF) {
        input = "<EOF>";
      } else {
        input = tokens.getText(new Interval(e.startToken.tokenIndex, e.offendingToken.tokenIndex));
      }
    } else {
      input = "<unknown input>";
    }
    let msg = "no viable alternative at input " + this.escapeWSAndQuote(input);
    recognizer.notifyErrorListeners(msg, e.offendingToken, e);
  }

  /**
   * This is called by {@link //reportError} when the exception is an
   * {@link InputMismatchException}.
   *
   * @see //reportError
   *
   * @param recognizer the parser instance
   * @param e the recognition exception
   */
  reportInputMismatch(recognizer, e) {
    let msg = "mismatched input " + this.getTokenErrorDisplay(e.offendingToken) + " expecting " + e.getExpectedTokens().toString(recognizer.literalNames, recognizer.symbolicNames);
    recognizer.notifyErrorListeners(msg, e.offendingToken, e);
  }

  /**
   * This is called by {@link //reportError} when the exception is a
   * {@link FailedPredicateException}.
   *
   * @see //reportError
   *
   * @param recognizer the parser instance
   * @param e the recognition exception
   */
  reportFailedPredicate(recognizer, e) {
    let ruleName = recognizer.ruleNames[recognizer._ctx.ruleIndex];
    let msg = "rule " + ruleName + " " + e.message;
    recognizer.notifyErrorListeners(msg, e.offendingToken, e);
  }

  /**
   * This method is called to report a syntax error which requires the removal
   * of a token from the input stream. At the time this method is called, the
   * erroneous symbol is current {@code LT(1)} symbol and has not yet been
   * removed from the input stream. When this method returns,
   * {@code recognizer} is in error recovery mode.
   *
   * <p>This method is called when {@link //singleTokenDeletion} identifies
   * single-token deletion as a viable recovery strategy for a mismatched
   * input error.</p>
   *
   * <p>The default implementation simply returns if the handler is already in
   * error recovery mode. Otherwise, it calls {@link //beginErrorCondition} to
   * enter error recovery mode, followed by calling
   * {@link Parser//notifyErrorListeners}.</p>
   *
   * @param recognizer the parser instance
   *
   */
  reportUnwantedToken(recognizer) {
    if (this.inErrorRecoveryMode(recognizer)) {
      return;
    }
    this.beginErrorCondition(recognizer);
    let t = recognizer.getCurrentToken();
    let tokenName = this.getTokenErrorDisplay(t);
    let expecting = this.getExpectedTokens(recognizer);
    let msg = "extraneous input " + tokenName + " expecting " + expecting.toString(recognizer.literalNames, recognizer.symbolicNames);
    recognizer.notifyErrorListeners(msg, t, null);
  }

  /**
   * This method is called to report a syntax error which requires the
   * insertion of a missing token into the input stream. At the time this
   * method is called, the missing token has not yet been inserted. When this
   * method returns, {@code recognizer} is in error recovery mode.
   *
   * <p>This method is called when {@link //singleTokenInsertion} identifies
   * single-token insertion as a viable recovery strategy for a mismatched
   * input error.</p>
   *
   * <p>The default implementation simply returns if the handler is already in
   * error recovery mode. Otherwise, it calls {@link //beginErrorCondition} to
   * enter error recovery mode, followed by calling
   * {@link Parser//notifyErrorListeners}.</p>
   *
   * @param recognizer the parser instance
   */
  reportMissingToken(recognizer) {
    if (this.inErrorRecoveryMode(recognizer)) {
      return;
    }
    this.beginErrorCondition(recognizer);
    let t = recognizer.getCurrentToken();
    let expecting = this.getExpectedTokens(recognizer);
    let msg = "missing " + expecting.toString(recognizer.literalNames, recognizer.symbolicNames) + " at " + this.getTokenErrorDisplay(t);
    recognizer.notifyErrorListeners(msg, t, null);
  }

  /**
   * <p>The default implementation attempts to recover from the mismatched input
   * by using single token insertion and deletion as described below. If the
   * recovery attempt fails, this method throws an
   * {@link InputMismatchException}.</p>
   *
   * <p><strong>EXTRA TOKEN</strong> (single token deletion)</p>
   *
   * <p>{@code LA(1)} is not what we are looking for. If {@code LA(2)} has the
   * right token, however, then assume {@code LA(1)} is some extra spurious
   * token and delete it. Then consume and return the next token (which was
   * the {@code LA(2)} token) as the successful result of the match operation.</p>
   *
   * <p>This recovery strategy is implemented by {@link
      * //singleTokenDeletion}.</p>
   *
   * <p><strong>MISSING TOKEN</strong> (single token insertion)</p>
   *
   * <p>If current token (at {@code LA(1)}) is consistent with what could come
   * after the expected {@code LA(1)} token, then assume the token is missing
   * and use the parser's {@link TokenFactory} to create it on the fly. The
   * "insertion" is performed by returning the created token as the successful
   * result of the match operation.</p>
   *
   * <p>This recovery strategy is implemented by {@link
      * //singleTokenInsertion}.</p>
   *
   * <p><strong>EXAMPLE</strong></p>
   *
   * <p>For example, Input {@code i=(3;} is clearly missing the {@code ')'}. When
   * the parser returns from the nested call to {@code expr}, it will have
   * call chain:</p>
   *
   * <pre>
   * stat &rarr; expr &rarr; atom
   * </pre>
   *
   * and it will be trying to match the {@code ')'} at this point in the
   * derivation:
   *
   * <pre>
   * =&gt; ID '=' '(' INT ')' ('+' atom)* ';'
   * ^
   * </pre>
   *
   * The attempt to match {@code ')'} will fail when it sees {@code ';'} and
   * call {@link //recoverInline}. To recover, it sees that {@code LA(1)==';'}
   * is in the set of tokens that can follow the {@code ')'} token reference
   * in rule {@code atom}. It can assume that you forgot the {@code ')'}.
   */
  recoverInline(recognizer) {
    // SINGLE TOKEN DELETION
    let matchedSymbol = this.singleTokenDeletion(recognizer);
    if (matchedSymbol !== null) {
      // we have deleted the extra token.
      // now, move past ttype token as if all were ok
      recognizer.consume();
      return matchedSymbol;
    }
    // SINGLE TOKEN INSERTION
    if (this.singleTokenInsertion(recognizer)) {
      return this.getMissingSymbol(recognizer);
    }
    // even that didn't work; must throw the exception
    throw new InputMismatchException(recognizer);
  }

  /**
   * This method implements the single-token insertion inline error recovery
   * strategy. It is called by {@link //recoverInline} if the single-token
   * deletion strategy fails to recover from the mismatched input. If this
   * method returns {@code true}, {@code recognizer} will be in error recovery
   * mode.
   *
   * <p>This method determines whether or not single-token insertion is viable by
   * checking if the {@code LA(1)} input symbol could be successfully matched
   * if it were instead the {@code LA(2)} symbol. If this method returns
   * {@code true}, the caller is responsible for creating and inserting a
   * token with the correct type to produce this behavior.</p>
   *
   * @param recognizer the parser instance
   * @return {@code true} if single-token insertion is a viable recovery
   * strategy for the current mismatched input, otherwise {@code false}
   */
  singleTokenInsertion(recognizer) {
    let currentSymbolType = recognizer.getTokenStream().LA(1);
    // if current token is consistent with what could come after current
    // ATN state, then we know we're missing a token; error recovery
    // is free to conjure up and insert the missing token
    let atn = recognizer._interp.atn;
    let currentState = atn.states[recognizer.state];
    let next = currentState.transitions[0].target;
    let expectingAtLL2 = atn.nextTokens(next, recognizer._ctx);
    if (expectingAtLL2.contains(currentSymbolType)) {
      this.reportMissingToken(recognizer);
      return true;
    } else {
      return false;
    }
  }

  /**
   * This method implements the single-token deletion inline error recovery
   * strategy. It is called by {@link //recoverInline} to attempt to recover
   * from mismatched input. If this method returns null, the parser and error
   * handler state will not have changed. If this method returns non-null,
   * {@code recognizer} will <em>not</em> be in error recovery mode since the
   * returned token was a successful match.
   *
   * <p>If the single-token deletion is successful, this method calls
   * {@link //reportUnwantedToken} to report the error, followed by
   * {@link Parser//consume} to actually "delete" the extraneous token. Then,
   * before returning {@link //reportMatch} is called to signal a successful
   * match.</p>
   *
   * @param recognizer the parser instance
   * @return the successfully matched {@link Token} instance if single-token
   * deletion successfully recovers from the mismatched input, otherwise
   * {@code null}
   */
  singleTokenDeletion(recognizer) {
    let nextTokenType = recognizer.getTokenStream().LA(2);
    let expecting = this.getExpectedTokens(recognizer);
    if (expecting.contains(nextTokenType)) {
      this.reportUnwantedToken(recognizer);
      // print("recoverFromMismatchedToken deleting " \
      // + str(recognizer.getTokenStream().LT(1)) \
      // + " since " + str(recognizer.getTokenStream().LT(2)) \
      // + " is what we want", file=sys.stderr)
      recognizer.consume(); // simply delete extra token
      // we want to return the token we're actually matching
      let matchedSymbol = recognizer.getCurrentToken();
      this.reportMatch(recognizer); // we know current token is correct
      return matchedSymbol;
    } else {
      return null;
    }
  }

  /**
   * Conjure up a missing token during error recovery.
   *
   * The recognizer attempts to recover from single missing
   * symbols. But, actions might refer to that missing symbol.
   * For example, x=ID {f($x);}. The action clearly assumes
   * that there has been an identifier matched previously and that
   * $x points at that token. If that token is missing, but
   * the next token in the stream is what we want we assume that
   * this token is missing and we keep going. Because we
   * have to return some token to replace the missing token,
   * we have to conjure one up. This method gives the user control
   * over the tokens returned for missing tokens. Mostly,
   * you will want to create something special for identifier
   * tokens. For literals such as '{' and ',', the default
   * action in the parser or tree parser works. It simply creates
   * a CommonToken of the appropriate type. The text will be the token.
   * If you change what tokens must be created by the lexer,
   * override this method to create the appropriate tokens.
   *
   */
  getMissingSymbol(recognizer) {
    let currentSymbol = recognizer.getCurrentToken();
    let expecting = this.getExpectedTokens(recognizer);
    let expectedTokenType = expecting.first(); // get any element
    let tokenText;
    if (expectedTokenType === Token.EOF) {
      tokenText = "<missing EOF>";
    } else {
      tokenText = "<missing " + recognizer.literalNames[expectedTokenType] + ">";
    }
    let current = currentSymbol;
    let lookback = recognizer.getTokenStream().LT(-1);
    if (current.type === Token.EOF && lookback !== null) {
      current = lookback;
    }
    return recognizer.getTokenFactory().create(current.source, expectedTokenType, tokenText, Token.DEFAULT_CHANNEL, -1, -1, current.line, current.column);
  }
  getExpectedTokens(recognizer) {
    return recognizer.getExpectedTokens();
  }

  /**
   * How should a token be displayed in an error message? The default
   * is to display just the text, but during development you might
   * want to have a lot of information spit out. Override in that case
   * to use t.toString() (which, for CommonToken, dumps everything about
   * the token). This is better than forcing you to override a method in
   * your token objects because you don't have to go modify your lexer
   * so that it creates a new Java type.
   */
  getTokenErrorDisplay(t) {
    if (t === null) {
      return "<no token>";
    }
    let s = t.text;
    if (s === null) {
      if (t.type === Token.EOF) {
        s = "<EOF>";
      } else {
        s = "<" + t.type + ">";
      }
    }
    return this.escapeWSAndQuote(s);
  }
  escapeWSAndQuote(s) {
    s = s.replace(/\n/g, "\\n");
    s = s.replace(/\r/g, "\\r");
    s = s.replace(/\t/g, "\\t");
    return "'" + s + "'";
  }

  /**
   * Compute the error recovery set for the current rule. During
   * rule invocation, the parser pushes the set of tokens that can
   * follow that rule reference on the stack; this amounts to
   * computing FIRST of what follows the rule reference in the
   * enclosing rule. See LinearApproximator.FIRST().
   * This local follow set only includes tokens
   * from within the rule; i.e., the FIRST computation done by
   * ANTLR stops at the end of a rule.
   *
   * EXAMPLE
   *
   * When you find a "no viable alt exception", the input is not
   * consistent with any of the alternatives for rule r. The best
   * thing to do is to consume tokens until you see something that
   * can legally follow a call to r//or* any rule that called r.
   * You don't want the exact set of viable next tokens because the
   * input might just be missing a token--you might consume the
   * rest of the input looking for one of the missing tokens.
   *
   * Consider grammar:
   *
   * a : '[' b ']'
   * | '(' b ')'
   * ;
   * b : c '^' INT ;
   * c : ID
   * | INT
   * ;
   *
   * At each rule invocation, the set of tokens that could follow
   * that rule is pushed on a stack. Here are the various
   * context-sensitive follow sets:
   *
   * FOLLOW(b1_in_a) = FIRST(']') = ']'
   * FOLLOW(b2_in_a) = FIRST(')') = ')'
   * FOLLOW(c_in_b) = FIRST('^') = '^'
   *
   * Upon erroneous input "[]", the call chain is
   *
   * a -> b -> c
   *
   * and, hence, the follow context stack is:
   *
   * depth follow set start of rule execution
   * 0 <EOF> a (from main())
   * 1 ']' b
   * 2 '^' c
   *
   * Notice that ')' is not included, because b would have to have
   * been called from a different context in rule a for ')' to be
   * included.
   *
   * For error recovery, we cannot consider FOLLOW(c)
   * (context-sensitive or otherwise). We need the combined set of
   * all context-sensitive FOLLOW sets--the set of all tokens that
   * could follow any reference in the call chain. We need to
   * resync to one of those tokens. Note that FOLLOW(c)='^' and if
   * we resync'd to that token, we'd consume until EOF. We need to
   * sync to context-sensitive FOLLOWs for a, b, and c: {']','^'}.
   * In this case, for input "[]", LA(1) is ']' and in the set, so we would
   * not consume anything. After printing an error, rule c would
   * return normally. Rule b would not find the required '^' though.
   * At this point, it gets a mismatched token error and throws an
   * exception (since LA(1) is not in the viable following token
   * set). The rule exception handler tries to recover, but finds
   * the same recovery set and doesn't consume anything. Rule b
   * exits normally returning to rule a. Now it finds the ']' (and
   * with the successful match exits errorRecovery mode).
   *
   * So, you can see that the parser walks up the call chain looking
   * for the token that was a member of the recovery set.
   *
   * Errors are not generated in errorRecovery mode.
   *
   * ANTLR's error recovery mechanism is based upon original ideas:
   *
   * "Algorithms + Data Structures = Programs" by Niklaus Wirth
   *
   * and
   *
   * "A note on error recovery in recursive descent parsers":
   * http://portal.acm.org/citation.cfm?id=947902.947905
   *
   * Later, Josef Grosch had some good ideas:
   *
   * "Efficient and Comfortable Error Recovery in Recursive Descent
   * Parsers":
   * ftp://www.cocolab.com/products/cocktail/doca4.ps/ell.ps.zip
   *
   * Like Grosch I implement context-sensitive FOLLOW sets that are combined
   * at run-time upon error to avoid overhead during parsing.
   */
  getErrorRecoverySet(recognizer) {
    let atn = recognizer._interp.atn;
    let ctx = recognizer._ctx;
    let recoverSet = new IntervalSet();
    while (ctx !== null && ctx.invokingState >= 0) {
      // compute what follows who invoked us
      let invokingState = atn.states[ctx.invokingState];
      let rt = invokingState.transitions[0];
      let follow = atn.nextTokens(rt.followState);
      recoverSet.addSet(follow);
      ctx = ctx.parentCtx;
    }
    recoverSet.removeOne(Token.EPSILON);
    return recoverSet;
  }

  // Consume tokens until one matches the given token set.//
  consumeUntil(recognizer, set) {
    let ttype = recognizer.getTokenStream().LA(1);
    while (ttype !== Token.EOF && !set.contains(ttype)) {
      recognizer.consume();
      ttype = recognizer.getTokenStream().LA(1);
    }
  }
}
;// ./src/antlr4/error/BailErrorStrategy.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */




/**
 * This implementation of {@link ANTLRErrorStrategy} responds to syntax errors
 * by immediately canceling the parse operation with a
 * {@link ParseCancellationException}. The implementation ensures that the
 * {@link ParserRuleContext//exception} field is set for all parse tree nodes
 * that were not completed prior to encountering the error.
 *
 * <p>
 * This error strategy is useful in the following scenarios.</p>
 *
 * <ul>
 * <li><strong>Two-stage parsing:</strong> This error strategy allows the first
 * stage of two-stage parsing to immediately terminate if an error is
 * encountered, and immediately fall back to the second stage. In addition to
 * avoiding wasted work by attempting to recover from errors here, the empty
 * implementation of {@link BailErrorStrategy//sync} improves the performance of
 * the first stage.</li>
 * <li><strong>Silent validation:</strong> When syntax errors are not being
 * reported or logged, and the parse result is simply ignored if errors occur,
 * the {@link BailErrorStrategy} avoids wasting work on recovering from errors
 * when the result will be ignored either way.</li>
 * </ul>
 *
 * <p>
 * {@code myparser.setErrorHandler(new BailErrorStrategy());}</p>
 *
 * @see Parser//setErrorHandler(ANTLRErrorStrategy)
 * */
class BailErrorStrategy extends DefaultErrorStrategy {
  constructor() {
    super();
  }

  /**
   * Instead of recovering from exception {@code e}, re-throw it wrapped
   * in a {@link ParseCancellationException} so it is not caught by the
   * rule function catches. Use {@link Exception//getCause()} to get the
   * original {@link RecognitionException}.
   */
  recover(recognizer, e) {
    let context = recognizer._ctx;
    while (context !== null) {
      context.exception = e;
      context = context.parentCtx;
    }
    throw new ParseCancellationException(e);
  }

  /**
   * Make sure we don't attempt to recover inline; if the parser
   * successfully recovers, it won't throw an exception.
   */
  recoverInline(recognizer) {
    this.recover(recognizer, new InputMismatchException(recognizer));
  }

  // Make sure we don't attempt to recover from problems in subrules.//
  sync(recognizer) {
    // pass
  }
}
;// ./src/antlr4/error/index.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */










/* harmony default export */ const error = ({
  RecognitionException: RecognitionException,
  NoViableAltException: NoViableAltException,
  LexerNoViableAltException: LexerNoViableAltException,
  InputMismatchException: InputMismatchException,
  FailedPredicateException: FailedPredicateException,
  DiagnosticErrorListener: DiagnosticErrorListener,
  BailErrorStrategy: BailErrorStrategy,
  DefaultErrorStrategy: DefaultErrorStrategy,
  ErrorListener: ErrorListener
});
;// ./src/antlr4/CharStreams.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * Utility functions to create InputStreams from various sources.
 *
 * All returned InputStreams support the full range of Unicode
 * up to U+10FFFF (the default behavior of InputStream only supports
 * code points up to U+FFFF).
 */
/* harmony default export */ const CharStreams = ({
  // Creates an InputStream from a string.
  fromString: function (str) {
    return new CharStream(str, true);
  },
  /**
   * Asynchronously creates an InputStream from a blob given the
   * encoding of the bytes in that blob (defaults to 'utf8' if
   * encoding is null).
   *
   * Invokes onLoad(result) on success, onError(error) on
   * failure.
   */
  fromBlob: function (blob, encoding, onLoad, onError) {
    let reader = new window.FileReader();
    reader.onload = function (e) {
      let is = new CharStream(e.target.result, true);
      onLoad(is);
    };
    reader.onerror = onError;
    reader.readAsText(blob, encoding);
  },
  /**
   * Creates an InputStream from a Buffer given the
   * encoding of the bytes in that buffer (defaults to 'utf8' if
   * encoding is null).
   */
  fromBuffer: function (buffer, encoding) {
    return new CharStream(buffer.toString(encoding), true);
  },
  /** Asynchronously creates an InputStream from a file on disk given
   * the encoding of the bytes in that file (defaults to 'utf8' if
   * encoding is null).
   *
   * Invokes callback(error, result) on completion.
   */
  fromPath: function (path, encoding, callback) {
    throw new Error("fromPath is not available in browser environment!");
  },
  /**
   * Synchronously creates an InputStream given a path to a file
   * on disk and the encoding of the bytes in that file (defaults to
   * 'utf8' if encoding is null).
   */
  fromPathSync: function (path, encoding) {
    throw new Error("fromPathSync is not available in browser environment!");
  }
});
;// ./src/antlr4/utils/stringToCharArray.js
function stringToCharArray(str) {
  let result = new Uint16Array(str.length);
  for (let i = 0; i < str.length; i++) {
    result[i] = str.charCodeAt(i);
  }
  return result;
}
;// ./src/antlr4/utils/index.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */


/* harmony default export */ const utils = ({
  arrayToString: arrayToString,
  stringToCharArray: stringToCharArray
});
;// ./src/antlr4/InputStream.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



/**
 * @deprecated Use CharStream instead
*/
class InputStream extends CharStream {
  constructor(data, decodeToUnicodeCodePoints) {
    super(data, decodeToUnicodeCodePoints);
  }
}
;// ./src/antlr4/TokenStream.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
// this is just to keep meaningful parameter types to Parser
class TokenStream {}
;// ./src/antlr4/BufferedTokenStream.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */






/**
 * This implementation of {@link TokenStream} loads tokens from a
 * {@link TokenSource} on-demand, and places the tokens in a buffer to provide
 * access to any previous token by index.
 *
 * <p>
 * This token stream ignores the value of {@link Token//getChannel}. If your
 * parser requires the token stream filter tokens to only those on a particular
 * channel, such as {@link Token//DEFAULT_CHANNEL} or
 * {@link Token//HIDDEN_CHANNEL}, use a filtering token stream such a
 * {@link CommonTokenStream}.</p>
 */
class BufferedTokenStream extends TokenStream {
  constructor(tokenSource) {
    super();
    // The {@link TokenSource} from which tokens for this stream are fetched.
    this.tokenSource = tokenSource;
    /**
     * A collection of all tokens fetched from the token source. The list is
     * considered a complete view of the input once {@link //fetchedEOF} is set
     * to {@code true}.
     */
    this.tokens = [];

    /**
     * The index into {@link //tokens} of the current token (next token to
     * {@link //consume}). {@link //tokens}{@code [}{@link //p}{@code ]} should
     * be
     * {@link //LT LT(1)}.
     *
     * <p>This field is set to -1 when the stream is first constructed or when
     * {@link //setTokenSource} is called, indicating that the first token has
     * not yet been fetched from the token source. For additional information,
     * see the documentation of {@link IntStream} for a description of
     * Initializing Methods.</p>
     */
    this.index = -1;

    /**
     * Indicates whether the {@link Token//EOF} token has been fetched from
     * {@link //tokenSource} and added to {@link //tokens}. This field improves
     * performance for the following cases:
     *
     * <ul>
     * <li>{@link //consume}: The lookahead check in {@link //consume} to
     * prevent
     * consuming the EOF symbol is optimized by checking the values of
     * {@link //fetchedEOF} and {@link //p} instead of calling {@link
     * //LA}.</li>
     * <li>{@link //fetch}: The check to prevent adding multiple EOF symbols
     * into
     * {@link //tokens} is trivial with this field.</li>
     * <ul>
     */
    this.fetchedEOF = false;
  }
  mark() {
    return 0;
  }
  release(marker) {
    // no resources to release
  }
  reset() {
    this.seek(0);
  }
  seek(index) {
    this.lazyInit();
    this.index = this.adjustSeekIndex(index);
  }
  get size() {
    return this.tokens.length;
  }
  get(index) {
    this.lazyInit();
    return this.tokens[index];
  }
  consume() {
    let skipEofCheck = false;
    if (this.index >= 0) {
      if (this.fetchedEOF) {
        // the last token in tokens is EOF. skip check if p indexes any
        // fetched token except the last.
        skipEofCheck = this.index < this.tokens.length - 1;
      } else {
        // no EOF token in tokens. skip check if p indexes a fetched token.
        skipEofCheck = this.index < this.tokens.length;
      }
    } else {
      // not yet initialized
      skipEofCheck = false;
    }
    if (!skipEofCheck && this.LA(1) === Token.EOF) {
      throw "cannot consume EOF";
    }
    if (this.sync(this.index + 1)) {
      this.index = this.adjustSeekIndex(this.index + 1);
    }
  }

  /**
   * Make sure index {@code i} in tokens has a token.
   *
   * @return {Boolean} {@code true} if a token is located at index {@code i}, otherwise
   * {@code false}.
   * @see //get(int i)
   */
  sync(i) {
    let n = i - this.tokens.length + 1; // how many more elements we need?
    if (n > 0) {
      let fetched = this.fetch(n);
      return fetched >= n;
    }
    return true;
  }

  /**
   * Add {@code n} elements to buffer.
   *
   * @return {Number} The actual number of elements added to the buffer.
   */
  fetch(n) {
    if (this.fetchedEOF) {
      return 0;
    }
    for (let i = 0; i < n; i++) {
      let t = this.tokenSource.nextToken();
      t.tokenIndex = this.tokens.length;
      this.tokens.push(t);
      if (t.type === Token.EOF) {
        this.fetchedEOF = true;
        return i + 1;
      }
    }
    return n;
  }

  // Get all tokens from start..stop inclusively///
  getTokens(start, stop, types) {
    if (types === undefined) {
      types = null;
    }
    if (start < 0 || stop < 0) {
      return null;
    }
    this.lazyInit();
    let subset = [];
    if (stop >= this.tokens.length) {
      stop = this.tokens.length - 1;
    }
    for (let i = start; i < stop; i++) {
      let t = this.tokens[i];
      if (t.type === Token.EOF) {
        break;
      }
      if (types === null || types.contains(t.type)) {
        subset.push(t);
      }
    }
    return subset;
  }
  LA(i) {
    return this.LT(i).type;
  }
  LB(k) {
    if (this.index - k < 0) {
      return null;
    }
    return this.tokens[this.index - k];
  }
  LT(k) {
    this.lazyInit();
    if (k === 0) {
      return null;
    }
    if (k < 0) {
      return this.LB(-k);
    }
    let i = this.index + k - 1;
    this.sync(i);
    if (i >= this.tokens.length) {
      // return EOF token
      // EOF must be last token
      return this.tokens[this.tokens.length - 1];
    }
    return this.tokens[i];
  }

  /**
   * Allowed derived classes to modify the behavior of operations which change
   * the current stream position by adjusting the target token index of a seek
   * operation. The default implementation simply returns {@code i}. If an
   * exception is thrown in this method, the current stream index should not be
   * changed.
   *
   * <p>For example, {@link CommonTokenStream} overrides this method to ensure
   * that
   * the seek target is always an on-channel token.</p>
   *
   * @param {Number} i The target token index.
   * @return {Number} The adjusted target token index.
   */
  adjustSeekIndex(i) {
    return i;
  }
  lazyInit() {
    if (this.index === -1) {
      this.setup();
    }
  }
  setup() {
    this.sync(0);
    this.index = this.adjustSeekIndex(0);
  }

  // Reset this token stream by setting its token source.///
  setTokenSource(tokenSource) {
    this.tokenSource = tokenSource;
    this.tokens = [];
    this.index = -1;
    this.fetchedEOF = false;
  }

  /**
   * Given a starting index, return the index of the next token on channel.
   * Return i if tokens[i] is on channel. Return -1 if there are no tokens
   * on channel between i and EOF.
   */
  nextTokenOnChannel(i, channel) {
    this.sync(i);
    if (i >= this.tokens.length) {
      return -1;
    }
    let token = this.tokens[i];
    while (token.channel !== channel) {
      if (token.type === Token.EOF) {
        return -1;
      }
      i += 1;
      this.sync(i);
      token = this.tokens[i];
    }
    return i;
  }

  /**
   * Given a starting index, return the index of the previous token on channel.
   * Return i if tokens[i] is on channel. Return -1 if there are no tokens
   * on channel between i and 0.
   */
  previousTokenOnChannel(i, channel) {
    while (i >= 0 && this.tokens[i].channel !== channel) {
      i -= 1;
    }
    return i;
  }

  /**
   * Collect all tokens on specified channel to the right of
   * the current token up until we see a token on DEFAULT_TOKEN_CHANNEL or
   * EOF. If channel is -1, find any non default channel token.
   */
  getHiddenTokensToRight(tokenIndex, channel) {
    if (channel === undefined) {
      channel = -1;
    }
    this.lazyInit();
    if (tokenIndex < 0 || tokenIndex >= this.tokens.length) {
      throw "" + tokenIndex + " not in 0.." + this.tokens.length - 1;
    }
    let nextOnChannel = this.nextTokenOnChannel(tokenIndex + 1, Lexer.DEFAULT_TOKEN_CHANNEL);
    let from_ = tokenIndex + 1;
    // if none onchannel to right, nextOnChannel=-1 so set to = last token
    let to = nextOnChannel === -1 ? this.tokens.length - 1 : nextOnChannel;
    return this.filterForChannel(from_, to, channel);
  }

  /**
   * Collect all tokens on specified channel to the left of
   * the current token up until we see a token on DEFAULT_TOKEN_CHANNEL.
   * If channel is -1, find any non default channel token.
   */
  getHiddenTokensToLeft(tokenIndex, channel) {
    if (channel === undefined) {
      channel = -1;
    }
    this.lazyInit();
    if (tokenIndex < 0 || tokenIndex >= this.tokens.length) {
      throw "" + tokenIndex + " not in 0.." + this.tokens.length - 1;
    }
    let prevOnChannel = this.previousTokenOnChannel(tokenIndex - 1, Lexer.DEFAULT_TOKEN_CHANNEL);
    if (prevOnChannel === tokenIndex - 1) {
      return null;
    }
    // if none on channel to left, prevOnChannel=-1 then from=0
    let from_ = prevOnChannel + 1;
    let to = tokenIndex - 1;
    return this.filterForChannel(from_, to, channel);
  }
  filterForChannel(left, right, channel) {
    let hidden = [];
    for (let i = left; i < right + 1; i++) {
      let t = this.tokens[i];
      if (channel === -1) {
        if (t.channel !== Lexer.DEFAULT_TOKEN_CHANNEL) {
          hidden.push(t);
        }
      } else if (t.channel === channel) {
        hidden.push(t);
      }
    }
    if (hidden.length === 0) {
      return null;
    }
    return hidden;
  }
  getSourceName() {
    return this.tokenSource.getSourceName();
  }

  // Get the text of all tokens in this buffer.///
  getText(interval) {
    this.lazyInit();
    this.fill();
    if (!interval) {
      interval = new Interval(0, this.tokens.length - 1);
    }
    let start = interval.start;
    if (start instanceof Token) {
      start = start.tokenIndex;
    }
    let stop = interval.stop;
    if (stop instanceof Token) {
      stop = stop.tokenIndex;
    }
    if (start === null || stop === null || start < 0 || stop < 0) {
      return "";
    }
    if (stop >= this.tokens.length) {
      stop = this.tokens.length - 1;
    }
    let s = "";
    for (let i = start; i < stop + 1; i++) {
      let t = this.tokens[i];
      if (t.type === Token.EOF) {
        break;
      }
      s = s + t.text;
    }
    return s;
  }

  // Get all tokens from lexer until EOF///
  fill() {
    this.lazyInit();
    // noinspection StatementWithEmptyBodyJS
    while (this.fetch(1000) === 1000);
  }
}
Object.defineProperty(BufferedTokenStream, "size", {
  get: function () {
    return this.tokens.length;
  }
});
;// ./src/antlr4/CommonTokenStream.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */




/**
 * This class extends {@link BufferedTokenStream} with functionality to filter
 * token streams to tokens on a particular channel (tokens where
 * {@link Token//getChannel} returns a particular value).
 *
 * <p>
 * This token stream provides access to all tokens by index or when calling
 * methods like {@link //getText}. The channel filtering is only used for code
 * accessing tokens via the lookahead methods {@link //LA}, {@link //LT}, and
 * {@link //LB}.</p>
 *
 * <p>
 * By default, tokens are placed on the default channel
 * ({@link Token//DEFAULT_CHANNEL}), but may be reassigned by using the
 * {@code ->channel(HIDDEN)} lexer command, or by using an embedded action to
 * call {@link Lexer//setChannel}.
 * </p>
 *
 * <p>
 * Note: lexer rules which use the {@code ->skip} lexer command or call
 * {@link Lexer//skip} do not produce tokens at all, so input text matched by
 * such a rule will not be available as part of the token stream, regardless of
 * channel.</p>
 */
class CommonTokenStream extends BufferedTokenStream {
  constructor(lexer, channel) {
    super(lexer);
    this.channel = channel === undefined ? Token.DEFAULT_CHANNEL : channel;
  }
  adjustSeekIndex(i) {
    return this.nextTokenOnChannel(i, this.channel);
  }
  LB(k) {
    if (k === 0 || this.index - k < 0) {
      return null;
    }
    let i = this.index;
    let n = 1;
    // find k good tokens looking backwards
    while (n <= k) {
      // skip off-channel tokens
      i = this.previousTokenOnChannel(i - 1, this.channel);
      n += 1;
    }
    if (i < 0) {
      return null;
    }
    return this.tokens[i];
  }
  LT(k) {
    this.lazyInit();
    if (k === 0) {
      return null;
    }
    if (k < 0) {
      return this.LB(-k);
    }
    let i = this.index;
    let n = 1; // we know tokens[pos] is a good one
    // find k good tokens
    while (n < k) {
      // skip off-channel tokens, but make sure to not look past EOF
      if (this.sync(i + 1)) {
        i = this.nextTokenOnChannel(i + 1, this.channel);
      }
      n += 1;
    }
    return this.tokens[i];
  }

  // Count EOF just once.
  getNumberOfOnChannelTokens() {
    let n = 0;
    this.fill();
    for (let i = 0; i < this.tokens.length; i++) {
      let t = this.tokens[i];
      if (t.channel === this.channel) {
        n += 1;
      }
      if (t.type === Token.EOF) {
        break;
      }
    }
    return n;
  }
}
;// ./src/antlr4/TraceListener.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

class TraceListener extends ParseTreeListener {
  constructor(parser) {
    super();
    this.parser = parser;
  }
  enterEveryRule(ctx) {
    console.log("enter   " + this.parser.ruleNames[ctx.ruleIndex] + ", LT(1)=" + this.parser._input.LT(1).text);
  }
  visitTerminal(node) {
    console.log("consume " + node.symbol + " rule " + this.parser.ruleNames[this.parser._ctx.ruleIndex]);
  }
  exitEveryRule(ctx) {
    console.log("exit    " + this.parser.ruleNames[ctx.ruleIndex] + ", LT(1)=" + this.parser._input.LT(1).text);
  }
}
;// ./src/antlr4/Parser.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */









class Parser extends Recognizer {
  /**
   * this is all the parsing support code essentially; most of it is error
   * recovery stuff.
   */
  constructor(input) {
    super();
    // The input stream.
    this._input = null;
    /**
     * The error handling strategy for the parser. The default value is a new
     * instance of {@link DefaultErrorStrategy}.
     */
    this._errHandler = new DefaultErrorStrategy();
    this._precedenceStack = [];
    this._precedenceStack.push(0);
    /**
     * The {@link ParserRuleContext} object for the currently executing rule.
     * this is always non-null during the parsing process.
     */
    this._ctx = null;
    /**
     * Specifies whether or not the parser should construct a parse tree during
     * the parsing process. The default value is {@code true}.
     */
    this.buildParseTrees = true;
    /**
     * When {@link //setTrace}{@code (true)} is called, a reference to the
     * {@link TraceListener} is stored here so it can be easily removed in a
     * later call to {@link //setTrace}{@code (false)}. The listener itself is
     * implemented as a parser listener so this field is not directly used by
     * other parser methods.
     */
    this._tracer = null;
    /**
     * The list of {@link ParseTreeListener} listeners registered to receive
     * events during the parse.
     */
    this._parseListeners = null;
    /**
     * The number of syntax errors reported during parsing. this value is
     * incremented each time {@link //notifyErrorListeners} is called.
     */
    this._syntaxErrors = 0;
    this.setInputStream(input);
  }

  // reset the parser's state
  reset() {
    if (this._input !== null) {
      this._input.seek(0);
    }
    this._errHandler.reset(this);
    this._ctx = null;
    this._syntaxErrors = 0;
    this.setTrace(false);
    this._precedenceStack = [];
    this._precedenceStack.push(0);
    if (this._interp !== null) {
      this._interp.reset();
    }
  }

  /**
   * Match current input symbol against {@code ttype}. If the symbol type
   * matches, {@link ANTLRErrorStrategy//reportMatch} and {@link //consume} are
   * called to complete the match process.
   *
   * <p>If the symbol type does not match,
   * {@link ANTLRErrorStrategy//recoverInline} is called on the current error
   * strategy to attempt recovery. If {@link //buildParseTree} is
   * {@code true} and the token index of the symbol returned by
   * {@link ANTLRErrorStrategy//recoverInline} is -1, the symbol is added to
   * the parse tree by calling {@link ParserRuleContext//addErrorNode}.</p>
   *
   * @param ttype the token type to match
   * @return the matched symbol
   * @throws RecognitionException if the current input symbol did not match
   * {@code ttype} and the error strategy could not recover from the
   * mismatched symbol
   */
  match(ttype) {
    let t = this.getCurrentToken();
    if (t.type === ttype) {
      this._errHandler.reportMatch(this);
      this.consume();
    } else {
      t = this._errHandler.recoverInline(this);
      if (this.buildParseTrees && t.tokenIndex === -1) {
        // we must have conjured up a new token during single token
        // insertion
        // if it's not the current symbol
        this._ctx.addErrorNode(t);
      }
    }
    return t;
  }

  /**
   * Match current input symbol as a wildcard. If the symbol type matches
   * (i.e. has a value greater than 0), {@link ANTLRErrorStrategy//reportMatch}
   * and {@link //consume} are called to complete the match process.
   *
   * <p>If the symbol type does not match,
   * {@link ANTLRErrorStrategy//recoverInline} is called on the current error
   * strategy to attempt recovery. If {@link //buildParseTree} is
   * {@code true} and the token index of the symbol returned by
   * {@link ANTLRErrorStrategy//recoverInline} is -1, the symbol is added to
   * the parse tree by calling {@link ParserRuleContext//addErrorNode}.</p>
   *
   * @return the matched symbol
   * @throws RecognitionException if the current input symbol did not match
   * a wildcard and the error strategy could not recover from the mismatched
   * symbol
   */
  matchWildcard() {
    let t = this.getCurrentToken();
    if (t.type > 0) {
      this._errHandler.reportMatch(this);
      this.consume();
    } else {
      t = this._errHandler.recoverInline(this);
      if (this.buildParseTrees && t.tokenIndex === -1) {
        // we must have conjured up a new token during single token
        // insertion
        // if it's not the current symbol
        this._ctx.addErrorNode(t);
      }
    }
    return t;
  }
  getParseListeners() {
    return this._parseListeners || [];
  }

  /**
   * Registers {@code listener} to receive events during the parsing process.
   *
   * <p>To support output-preserving grammar transformations (including but not
   * limited to left-recursion removal, automated left-factoring, and
   * optimized code generation), calls to listener methods during the parse
   * may differ substantially from calls made by
   * {@link ParseTreeWalker//DEFAULT} used after the parse is complete. In
   * particular, rule entry and exit events may occur in a different order
   * during the parse than after the parser. In addition, calls to certain
   * rule entry methods may be omitted.</p>
   *
   * <p>With the following specific exceptions, calls to listener events are
   * <em>deterministic</em>, i.e. for identical input the calls to listener
   * methods will be the same.</p>
   *
   * <ul>
   * <li>Alterations to the grammar used to generate code may change the
   * behavior of the listener calls.</li>
   * <li>Alterations to the command line options passed to ANTLR 4 when
   * generating the parser may change the behavior of the listener calls.</li>
   * <li>Changing the version of the ANTLR Tool used to generate the parser
   * may change the behavior of the listener calls.</li>
   * </ul>
   *
   * @param listener the listener to add
   *
   * @throws NullPointerException if {@code} listener is {@code null}
   */
  addParseListener(listener) {
    if (listener === null) {
      throw "listener";
    }
    if (this._parseListeners === null) {
      this._parseListeners = [];
    }
    this._parseListeners.push(listener);
  }

  /**
   * Remove {@code listener} from the list of parse listeners.
   *
   * <p>If {@code listener} is {@code null} or has not been added as a parse
   * listener, this method does nothing.</p>
   * @param listener the listener to remove
   */
  removeParseListener(listener) {
    if (this._parseListeners !== null) {
      let idx = this._parseListeners.indexOf(listener);
      if (idx >= 0) {
        this._parseListeners.splice(idx, 1);
      }
      if (this._parseListeners.length === 0) {
        this._parseListeners = null;
      }
    }
  }

  // Remove all parse listeners.
  removeParseListeners() {
    this._parseListeners = null;
  }

  // Notify any parse listeners of an enter rule event.
  triggerEnterRuleEvent() {
    if (this._parseListeners !== null) {
      let ctx = this._ctx;
      this._parseListeners.forEach(function (listener) {
        listener.enterEveryRule(ctx);
        ctx.enterRule(listener);
      });
    }
  }

  /**
   * Notify any parse listeners of an exit rule event.
   * @see //addParseListener
   */
  triggerExitRuleEvent() {
    if (this._parseListeners !== null) {
      // reverse order walk of listeners
      let ctx = this._ctx;
      this._parseListeners.slice(0).reverse().forEach(function (listener) {
        ctx.exitRule(listener);
        listener.exitEveryRule(ctx);
      });
    }
  }
  getTokenFactory() {
    return this._input.tokenSource._factory;
  }

  // Tell our token source and error strategy about a new way to create tokens.
  setTokenFactory(factory) {
    this._input.tokenSource._factory = factory;
  }

  /**
   * The ATN with bypass alternatives is expensive to create so we create it
   * lazily.
   *
   * @throws UnsupportedOperationException if the current parser does not
   * implement the {@link //getSerializedATN()} method.
   */
  getATNWithBypassAlts() {
    let serializedAtn = this.getSerializedATN();
    if (serializedAtn === null) {
      throw "The current parser does not support an ATN with bypass alternatives.";
    }
    let result = this.bypassAltsAtnCache[serializedAtn];
    if (result === null) {
      let deserializationOptions = new ATNDeserializationOptions();
      deserializationOptions.generateRuleBypassTransitions = true;
      result = new ATNDeserializer(deserializationOptions).deserialize(serializedAtn);
      this.bypassAltsAtnCache[serializedAtn] = result;
    }
    return result;
  }
  getInputStream() {
    return this.getTokenStream();
  }
  setInputStream(input) {
    this.setTokenStream(input);
  }
  getTokenStream() {
    return this._input;
  }

  // Set the token stream and reset the parser.
  setTokenStream(input) {
    this._input = null;
    this.reset();
    this._input = input;
  }

  /**
  * Gets the number of syntax errors reported during parsing. This value is
  * incremented each time {@link //notifyErrorListeners} is called.	 
  */
  get syntaxErrorsCount() {
    return this._syntaxErrors;
  }

  /**
   * Match needs to return the current input symbol, which gets put
   * into the label for the associated token ref; e.g., x=ID.
   */
  getCurrentToken() {
    return this._input.LT(1);
  }
  notifyErrorListeners(msg, offendingToken, err) {
    offendingToken = offendingToken || null;
    err = err || null;
    if (offendingToken === null) {
      offendingToken = this.getCurrentToken();
    }
    this._syntaxErrors += 1;
    let line = offendingToken.line;
    let column = offendingToken.column;
    let listener = this.getErrorListener();
    listener.syntaxError(this, offendingToken, line, column, msg, err);
  }

  /**
   * Consume and return the {@linkplain //getCurrentToken current symbol}.
   *
   * <p>E.g., given the following input with {@code A} being the current
   * lookahead symbol, this function moves the cursor to {@code B} and returns
   * {@code A}.</p>
   *
   * <pre>
   * A B
   * ^
   * </pre>
   *
   * If the parser is not in error recovery mode, the consumed symbol is added
   * to the parse tree using {@link ParserRuleContext//addChild(Token)}, and
   * {@link ParseTreeListener//visitTerminal} is called on any parse listeners.
   * If the parser <em>is</em> in error recovery mode, the consumed symbol is
   * added to the parse tree using
   * {@link ParserRuleContext//addErrorNode(Token)}, and
   * {@link ParseTreeListener//visitErrorNode} is called on any parse
   * listeners.
   */
  consume() {
    let o = this.getCurrentToken();
    if (o.type !== Token.EOF) {
      this.getInputStream().consume();
    }
    let hasListener = this._parseListeners !== null && this._parseListeners.length > 0;
    if (this.buildParseTrees || hasListener) {
      let node;
      if (this._errHandler.inErrorRecoveryMode(this)) {
        node = this._ctx.addErrorNode(o);
      } else {
        node = this._ctx.addTokenNode(o);
      }
      node.invokingState = this.state;
      if (hasListener) {
        this._parseListeners.forEach(function (listener) {
          if (node instanceof ErrorNode || node.isErrorNode !== undefined && node.isErrorNode()) {
            listener.visitErrorNode(node);
          } else if (node instanceof TerminalNode) {
            listener.visitTerminal(node);
          }
        });
      }
    }
    return o;
  }
  addContextToParseTree() {
    // add current context to parent if we have a parent
    if (this._ctx.parentCtx !== null) {
      this._ctx.parentCtx.addChild(this._ctx);
    }
  }

  /**
   * Always called by generated parsers upon entry to a rule. Access field
   * {@link //_ctx} get the current context.
   */
  enterRule(localctx, state, ruleIndex) {
    this.state = state;
    this._ctx = localctx;
    this._ctx.start = this._input.LT(1);
    if (this.buildParseTrees) {
      this.addContextToParseTree();
    }
    this.triggerEnterRuleEvent();
  }
  exitRule() {
    this._ctx.stop = this._input.LT(-1);
    // trigger event on _ctx, before it reverts to parent
    this.triggerExitRuleEvent();
    this.state = this._ctx.invokingState;
    this._ctx = this._ctx.parentCtx;
  }
  enterOuterAlt(localctx, altNum) {
    localctx.setAltNumber(altNum);
    // if we have new localctx, make sure we replace existing ctx
    // that is previous child of parse tree
    if (this.buildParseTrees && this._ctx !== localctx) {
      if (this._ctx.parentCtx !== null) {
        this._ctx.parentCtx.removeLastChild();
        this._ctx.parentCtx.addChild(localctx);
      }
    }
    this._ctx = localctx;
  }

  /**
   * Get the precedence level for the top-most precedence rule.
   *
   * @return The precedence level for the top-most precedence rule, or -1 if
   * the parser context is not nested within a precedence rule.
   */
  getPrecedence() {
    if (this._precedenceStack.length === 0) {
      return -1;
    } else {
      return this._precedenceStack[this._precedenceStack.length - 1];
    }
  }
  enterRecursionRule(localctx, state, ruleIndex, precedence) {
    this.state = state;
    this._precedenceStack.push(precedence);
    this._ctx = localctx;
    this._ctx.start = this._input.LT(1);
    this.triggerEnterRuleEvent(); // simulates rule entry for left-recursive rules
  }

  // Like {@link //enterRule} but for recursive rules.
  pushNewRecursionContext(localctx, state, ruleIndex) {
    let previous = this._ctx;
    previous.parentCtx = localctx;
    previous.invokingState = state;
    previous.stop = this._input.LT(-1);
    this._ctx = localctx;
    this._ctx.start = previous.start;
    if (this.buildParseTrees) {
      this._ctx.addChild(previous);
    }
    this.triggerEnterRuleEvent(); // simulates rule entry for left-recursive rules
  }
  unrollRecursionContexts(parentCtx) {
    this._precedenceStack.pop();
    this._ctx.stop = this._input.LT(-1);
    let retCtx = this._ctx; // save current ctx (return value)
    // unroll so _ctx is as it was before call to recursive method
    let parseListeners = this.getParseListeners();
    if (parseListeners !== null && parseListeners.length > 0) {
      while (this._ctx !== parentCtx) {
        this.triggerExitRuleEvent();
        this._ctx = this._ctx.parentCtx;
      }
    } else {
      this._ctx = parentCtx;
    }
    // hook into tree
    retCtx.parentCtx = parentCtx;
    if (this.buildParseTrees && parentCtx !== null) {
      // add return ctx into invoking rule's tree
      parentCtx.addChild(retCtx);
    }
  }
  getInvokingContext(ruleIndex) {
    let ctx = this._ctx;
    while (ctx !== null) {
      if (ctx.ruleIndex === ruleIndex) {
        return ctx;
      }
      ctx = ctx.parentCtx;
    }
    return null;
  }
  precpred(localctx, precedence) {
    return precedence >= this._precedenceStack[this._precedenceStack.length - 1];
  }
  inContext(context) {
    // TODO: useful in parser?
    return false;
  }

  /**
   * Checks whether or not {@code symbol} can follow the current state in the
   * ATN. The behavior of this method is equivalent to the following, but is
   * implemented such that the complete context-sensitive follow set does not
   * need to be explicitly constructed.
   *
   * <pre>
   * return getExpectedTokens().contains(symbol);
   * </pre>
   *
   * @param symbol the symbol type to check
   * @return {@code true} if {@code symbol} can follow the current state in
   * the ATN, otherwise {@code false}.
   */
  isExpectedToken(symbol) {
    let atn = this._interp.atn;
    let ctx = this._ctx;
    let s = atn.states[this.state];
    let following = atn.nextTokens(s);
    if (following.contains(symbol)) {
      return true;
    }
    if (!following.contains(Token.EPSILON)) {
      return false;
    }
    while (ctx !== null && ctx.invokingState >= 0 && following.contains(Token.EPSILON)) {
      let invokingState = atn.states[ctx.invokingState];
      let rt = invokingState.transitions[0];
      following = atn.nextTokens(rt.followState);
      if (following.contains(symbol)) {
        return true;
      }
      ctx = ctx.parentCtx;
    }
    if (following.contains(Token.EPSILON) && symbol === Token.EOF) {
      return true;
    } else {
      return false;
    }
  }

  /**
   * Computes the set of input symbols which could follow the current parser
   * state and context, as given by {@link //getState} and {@link //getContext},
   * respectively.
   *
   * @see ATN//getExpectedTokens(int, RuleContext)
   */
  getExpectedTokens() {
    return this._interp.atn.getExpectedTokens(this.state, this._ctx);
  }
  getExpectedTokensWithinCurrentRule() {
    let atn = this._interp.atn;
    let s = atn.states[this.state];
    return atn.nextTokens(s);
  }

  // Get a rule's index (i.e., {@code RULE_ruleName} field) or -1 if not found.
  getRuleIndex(ruleName) {
    let ruleIndex = this.getRuleIndexMap()[ruleName];
    if (ruleIndex !== null) {
      return ruleIndex;
    } else {
      return -1;
    }
  }

  /**
   * Return List&lt;String&gt; of the rule names in your parser instance
   * leading up to a call to the current rule. You could override if
   * you want more details such as the file/line info of where
   * in the ATN a rule is invoked.
   *
   * this is very useful for error messages.
   */
  getRuleInvocationStack(p) {
    p = p || null;
    if (p === null) {
      p = this._ctx;
    }
    let stack = [];
    while (p !== null) {
      // compute what follows who invoked us
      let ruleIndex = p.ruleIndex;
      if (ruleIndex < 0) {
        stack.push("n/a");
      } else {
        stack.push(this.ruleNames[ruleIndex]);
      }
      p = p.parentCtx;
    }
    return stack;
  }

  // For debugging and other purposes.
  getDFAStrings() {
    return this._interp.decisionToDFA.toString();
  }

  // For debugging and other purposes.
  dumpDFA() {
    let seenOne = false;
    for (let i = 0; i < this._interp.decisionToDFA.length; i++) {
      let dfa = this._interp.decisionToDFA[i];
      if (dfa.states.length > 0) {
        if (seenOne) {
          console.log();
        }
        this.printer.println("Decision " + dfa.decision + ":");
        this.printer.print(dfa.toString(this.literalNames, this.symbolicNames));
        seenOne = true;
      }
    }
  }
  getSourceName() {
    return this._input.getSourceName();
  }

  /**
   * During a parse is sometimes useful to listen in on the rule entry and exit
   * events as well as token matches. this is for quick and dirty debugging.
   */
  setTrace(trace) {
    if (!trace) {
      this.removeParseListener(this._tracer);
      this._tracer = null;
    } else {
      if (this._tracer !== null) {
        this.removeParseListener(this._tracer);
      }
      this._tracer = new TraceListener(this);
      this.addParseListener(this._tracer);
    }
  }
}

/**
 * this field maps from the serialized ATN string to the deserialized {@link
    * ATN} with
 * bypass alternatives.
 *
 * @see ATNDeserializationOptions//isGenerateRuleBypassTransitions()
 */
Parser.bypassAltsAtnCache = {};
;// ./src/antlr4/tree/TerminalNodeImpl.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */



class TerminalNodeImpl extends TerminalNode {
  constructor(symbol) {
    super();
    this.parentCtx = null;
    this.symbol = symbol;
  }
  getChild(i) {
    return null;
  }
  getSymbol() {
    return this.symbol;
  }
  getParent() {
    return this.parentCtx;
  }
  getPayload() {
    return this.symbol;
  }
  getSourceInterval() {
    if (this.symbol === null) {
      return Interval.INVALID_INTERVAL;
    }
    let tokenIndex = this.symbol.tokenIndex;
    return new Interval(tokenIndex, tokenIndex);
  }
  getChildCount() {
    return 0;
  }
  accept(visitor) {
    return visitor.visitTerminal(this);
  }
  getText() {
    return this.symbol.text;
  }
  toString() {
    if (this.symbol.type === Token.EOF) {
      return "<EOF>";
    } else {
      return this.symbol.text;
    }
  }
}
;// ./src/antlr4/tree/ErrorNodeImpl.js
/* Copyright (c) 2012-2022 The ANTLR Project Contributors. All rights reserved.
 * Use is of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */
/**
 * Represents a token that was consumed during resynchronization
 * rather than during a valid match operation. For example,
 * we will create this kind of a node during single token insertion
 * and deletion as well as during "consume until error recovery set"
 * upon no viable alternative exceptions.
 */

class ErrorNodeImpl extends TerminalNodeImpl {
  constructor(token) {
    super(token);
  }
  isErrorNode() {
    return true;
  }
  accept(visitor) {
    return visitor.visitErrorNode(this);
  }
}
;// ./src/antlr4/context/ParserRuleContext.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */







/**
 * A rule invocation record for parsing.
 *
 *  Contains all of the information about the current rule not stored in the
 *  RuleContext. It handles parse tree children list, Any ATN state
 *  tracing, and the default values available for rule indications:
 *  start, stop, rule index, current alt number, current
 *  ATN state.
 *
 *  Subclasses made for each rule and grammar track the parameters,
 *  return values, locals, and labels specific to that rule. These
 *  are the objects that are returned from rules.
 *
 *  Note text is not an actual field of a rule return value; it is computed
 *  from start and stop using the input stream's toString() method.  I
 *  could add a ctor to this so that we can pass in and store the input
 *  stream, but I'm not sure we want to do that.  It would seem to be undefined
 *  to get the .text property anyway if the rule matches tokens from multiple
 *  input streams.
 *
 *  I do not use getters for fields of objects that are used simply to
 *  group values such as this aggregate.  The getters/setters are there to
 *  satisfy the superclass interface.
 */
class ParserRuleContext extends RuleContext {
  constructor(parent, invokingStateNumber) {
    super(parent, invokingStateNumber);
    /**
     * If we are debugging or building a parse tree for a visitor,
     * we need to track all of the tokens and rule invocations associated
     * with this rule's context. This is empty for parsing w/o tree constr.
     * operation because we don't the need to track the details about
     * how we parse this rule.
     */
    this.children = null;
    this.start = null;
    this.stop = null;
    /**
     * The exception that forced this rule to return. If the rule successfully
     * completed, this is {@code null}.
     */
    this.exception = null;
  }

  // COPY a ctx (I'm deliberately not using copy constructor)
  copyFrom(ctx) {
    // from RuleContext
    this.parentCtx = ctx.parentCtx;
    this.invokingState = ctx.invokingState;
    this.children = null;
    this.start = ctx.start;
    this.stop = ctx.stop;
    // copy any error nodes to alt label node
    if (ctx.children) {
      this.children = [];
      // reset parent pointer for any error nodes
      ctx.children.map(function (child) {
        if (child instanceof ErrorNodeImpl) {
          this.children.push(child);
          child.parentCtx = this;
        }
      }, this);
    }
  }

  // Double dispatch methods for listeners
  enterRule(listener) {}
  exitRule(listener) {}

  // Does not set parent link; other add methods do that
  addChild(child) {
    if (this.children === null) {
      this.children = [];
    }
    this.children.push(child);
    return child;
  }

  /** Used by enterOuterAlt to toss out a RuleContext previously added as
   * we entered a rule. If we have // label, we will need to remove
   * generic ruleContext object.
   */
  removeLastChild() {
    if (this.children !== null) {
      this.children.pop();
    }
  }
  addTokenNode(token) {
    let node = new TerminalNodeImpl(token);
    this.addChild(node);
    node.parentCtx = this;
    return node;
  }
  addErrorNode(badToken) {
    let node = new ErrorNodeImpl(badToken);
    this.addChild(node);
    node.parentCtx = this;
    return node;
  }
  getChild(i, type) {
    type = type || null;
    if (this.children === null || i < 0 || i >= this.children.length) {
      return null;
    }
    if (type === null) {
      return this.children[i];
    } else {
      for (let j = 0; j < this.children.length; j++) {
        let child = this.children[j];
        if (child instanceof type) {
          if (i === 0) {
            return child;
          } else {
            i -= 1;
          }
        }
      }
      return null;
    }
  }
  getToken(ttype, i) {
    if (this.children === null || i < 0 || i >= this.children.length) {
      return null;
    }
    for (let j = 0; j < this.children.length; j++) {
      let child = this.children[j];
      if (child instanceof TerminalNode) {
        if (child.symbol.type === ttype) {
          if (i === 0) {
            return child;
          } else {
            i -= 1;
          }
        }
      }
    }
    return null;
  }
  getTokens(ttype) {
    if (this.children === null) {
      return [];
    } else {
      let tokens = [];
      for (let j = 0; j < this.children.length; j++) {
        let child = this.children[j];
        if (child instanceof TerminalNode) {
          if (child.symbol.type === ttype) {
            tokens.push(child);
          }
        }
      }
      return tokens;
    }
  }
  getTypedRuleContext(ctxType, i) {
    return this.getChild(i, ctxType);
  }
  getTypedRuleContexts(ctxType) {
    if (this.children === null) {
      return [];
    } else {
      let contexts = [];
      for (let j = 0; j < this.children.length; j++) {
        let child = this.children[j];
        if (child instanceof ctxType) {
          contexts.push(child);
        }
      }
      return contexts;
    }
  }
  getChildCount() {
    if (this.children === null) {
      return 0;
    } else {
      return this.children.length;
    }
  }
  getSourceInterval() {
    if (this.start === null || this.stop === null) {
      return Interval.INVALID_INTERVAL;
    } else {
      return new Interval(this.start.tokenIndex, this.stop.tokenIndex);
    }
  }
}
RuleContext.EMPTY = new ParserRuleContext();
;// ./src/antlr4/TokenStreamRewriter.js



/**
 * @typedef {import("./CommonTokenStream").default} CommonTokenStream
 * @typedef {Array<RewriteOperation | undefined>} Rewrites
 * @typedef {unknown} Text
 */

class TokenStreamRewriter {
  // eslint-disable-next-line no-undef
  static DEFAULT_PROGRAM_NAME = "default";

  /**
   * @param {CommonTokenStream} tokens The token stream to modify
   */
  constructor(tokens) {
    this.tokens = tokens;
    /** @type {Map<string, Rewrites>} */
    this.programs = new Map();
  }

  /**
   * @returns {CommonTokenStream}
   */
  getTokenStream() {
    return this.tokens;
  }

  /**
   * Insert the supplied text after the specified token (or token index)
   * @param {Token | number} tokenOrIndex
   * @param {Text} text
   * @param {string} [programName]
   */
  insertAfter(tokenOrIndex, text, programName = TokenStreamRewriter.DEFAULT_PROGRAM_NAME) {
    /** @type {number} */
    let index;
    if (typeof tokenOrIndex === "number") {
      index = tokenOrIndex;
    } else {
      index = tokenOrIndex.tokenIndex;
    }

    // to insert after, just insert before next index (even if past end)
    let rewrites = this.getProgram(programName);
    let op = new InsertAfterOp(this.tokens, index, rewrites.length, text);
    rewrites.push(op);
  }

  /**
   * Insert the supplied text before the specified token (or token index)
   * @param {Token | number} tokenOrIndex
   * @param {Text} text
   * @param {string} [programName]
   */
  insertBefore(tokenOrIndex, text, programName = TokenStreamRewriter.DEFAULT_PROGRAM_NAME) {
    /** @type {number} */
    let index;
    if (typeof tokenOrIndex === "number") {
      index = tokenOrIndex;
    } else {
      index = tokenOrIndex.tokenIndex;
    }
    let rewrites = this.getProgram(programName);
    let op = new InsertBeforeOp(this.tokens, index, rewrites.length, text);
    rewrites.push(op);
  }

  /**
   * Replace the specified token with the supplied text
   * @param {Token | number} tokenOrIndex
   * @param {Text} text
   * @param {string} [programName]
   */
  replaceSingle(tokenOrIndex, text, programName = TokenStreamRewriter.DEFAULT_PROGRAM_NAME) {
    this.replace(tokenOrIndex, tokenOrIndex, text, programName);
  }

  /**
   * Replace the specified range of tokens with the supplied text
   * @param {Token | number} from
   * @param {Token | number} to
   * @param {Text} text
   * @param {string} [programName]
   */
  replace(from, to, text, programName = TokenStreamRewriter.DEFAULT_PROGRAM_NAME) {
    if (typeof from !== "number") {
      from = from.tokenIndex;
    }
    if (typeof to !== "number") {
      to = to.tokenIndex;
    }
    if (from > to || from < 0 || to < 0 || to >= this.tokens.size) {
      throw new RangeError(`replace: range invalid: ${from}..${to}(size=${this.tokens.size})`);
    }
    let rewrites = this.getProgram(programName);
    let op = new ReplaceOp(this.tokens, from, to, rewrites.length, text);
    rewrites.push(op);
  }

  /**
   * Delete the specified range of tokens
   * @param {number | Token} from
   * @param {number | Token} to
   * @param {string} [programName]
   */
  delete(from, to, programName = TokenStreamRewriter.DEFAULT_PROGRAM_NAME) {
    if (typeof to === "undefined") {
      to = from;
    }
    this.replace(from, to, null, programName);
  }

  /**
   * @param {string} name
   * @returns {Rewrites}
   */
  getProgram(name) {
    let is = this.programs.get(name);
    if (is == null) {
      is = this.initializeProgram(name);
    }
    return is;
  }

  /**
   * @param {string} name
   * @returns {Rewrites}
   */
  initializeProgram(name) {
    let is = [];
    this.programs.set(name, is);
    return is;
  }

  /**
   * Return the text from the original tokens altered per the instructions given to this rewriter
   * @param {Interval | string} [intervalOrProgram]
   * @param {string} [programName]
   * @returns {string}
   */
  getText(intervalOrProgram, programName = TokenStreamRewriter.DEFAULT_PROGRAM_NAME) {
    let interval;
    if (intervalOrProgram instanceof Interval) {
      interval = intervalOrProgram;
    } else {
      interval = new Interval(0, this.tokens.size - 1);
    }
    if (typeof intervalOrProgram === "string") {
      programName = intervalOrProgram;
    }
    let rewrites = this.programs.get(programName);
    let start = interval.start;
    let stop = interval.stop;

    // ensure start/end are in range
    if (stop > this.tokens.size - 1) {
      stop = this.tokens.size - 1;
    }
    if (start < 0) {
      start = 0;
    }
    if (rewrites == null || rewrites.length === 0) {
      return this.tokens.getText(new Interval(start, stop)); // no instructions to execute
    }
    let buf = [];

    // First, optimize instruction stream
    let indexToOp = this.reduceToSingleOperationPerIndex(rewrites);

    // Walk buffer, executing instructions and emitting tokens
    let i = start;
    while (i <= stop && i < this.tokens.size) {
      let op = indexToOp.get(i);
      indexToOp.delete(i); // remove so any left have index size-1
      let t = this.tokens.get(i);
      if (op == null) {
        // no operation at that index, just dump token
        if (t.type !== Token.EOF) {
          buf.push(String(t.text));
        }
        i++; // move to next token
      } else {
        i = op.execute(buf); // execute operation and skip
      }
    }

    // include stuff after end if it's last index in buffer
    // So, if they did an insertAfter(lastValidIndex, "foo"), include
    // foo if end==lastValidIndex.
    if (stop === this.tokens.size - 1) {
      // Scan any remaining operations after last token
      // should be included (they will be inserts).
      for (let op of indexToOp.values()) {
        if (op.index >= this.tokens.size - 1) {
          buf.push(op.text.toString());
        }
      }
    }
    return buf.join("");
  }

  /**
   * @param {Rewrites} rewrites
   * @returns {Map<number, RewriteOperation>} a map from token index to operation
   */
  reduceToSingleOperationPerIndex(rewrites) {
    // WALK REPLACES
    for (let i = 0; i < rewrites.length; i++) {
      let op = rewrites[i];
      if (op == null) {
        continue;
      }
      if (!(op instanceof ReplaceOp)) {
        continue;
      }
      let rop = op;
      // Wipe prior inserts within range
      let inserts = this.getKindOfOps(rewrites, InsertBeforeOp, i);
      for (let iop of inserts) {
        if (iop.index === rop.index) {
          // E.g., insert before 2, delete 2..2; update replace
          // text to include insert before, kill insert
          rewrites[iop.instructionIndex] = undefined;
          rop.text = iop.text.toString() + (rop.text != null ? rop.text.toString() : "");
        } else if (iop.index > rop.index && iop.index <= rop.lastIndex) {
          // delete insert as it's a no-op.
          rewrites[iop.instructionIndex] = undefined;
        }
      }
      // Drop any prior replaces contained within
      let prevReplaces = this.getKindOfOps(rewrites, ReplaceOp, i);
      for (let prevRop of prevReplaces) {
        if (prevRop.index >= rop.index && prevRop.lastIndex <= rop.lastIndex) {
          // delete replace as it's a no-op.
          rewrites[prevRop.instructionIndex] = undefined;
          continue;
        }
        // throw exception unless disjoint or identical
        let disjoint = prevRop.lastIndex < rop.index || prevRop.index > rop.lastIndex;
        // Delete special case of replace (text==null):
        // D.i-j.u D.x-y.v	| boundaries overlap	combine to max(min)..max(right)
        if (prevRop.text == null && rop.text == null && !disjoint) {
          rewrites[prevRop.instructionIndex] = undefined; // kill first delete
          rop.index = Math.min(prevRop.index, rop.index);
          rop.lastIndex = Math.max(prevRop.lastIndex, rop.lastIndex);
        } else if (!disjoint) {
          throw new Error(`replace op boundaries of ${rop} overlap with previous ${prevRop}`);
        }
      }
    }

    // WALK INSERTS
    for (let i = 0; i < rewrites.length; i++) {
      let op = rewrites[i];
      if (op == null) {
        continue;
      }
      if (!(op instanceof InsertBeforeOp)) {
        continue;
      }
      let iop = op;
      // combine current insert with prior if any at same index
      let prevInserts = this.getKindOfOps(rewrites, InsertBeforeOp, i);
      for (let prevIop of prevInserts) {
        if (prevIop.index === iop.index) {
          if (prevIop instanceof InsertAfterOp) {
            iop.text = this.catOpText(prevIop.text, iop.text);
            rewrites[prevIop.instructionIndex] = undefined;
          } else if (prevIop instanceof InsertBeforeOp) {
            // combine objects
            // convert to strings...we're in process of toString'ing
            // whole token buffer so no lazy eval issue with any templates
            iop.text = this.catOpText(iop.text, prevIop.text);
            // delete redundant prior insert
            rewrites[prevIop.instructionIndex] = undefined;
          }
        }
      }
      // look for replaces where iop.index is in range; error
      let prevReplaces = this.getKindOfOps(rewrites, ReplaceOp, i);
      for (let rop of prevReplaces) {
        if (iop.index === rop.index) {
          rop.text = this.catOpText(iop.text, rop.text);
          rewrites[i] = undefined; // delete current insert
          continue;
        }
        if (iop.index >= rop.index && iop.index <= rop.lastIndex) {
          throw new Error(`insert op ${iop} within boundaries of previous ${rop}`);
        }
      }
    }

    /** @type {Map<number, RewriteOperation>} */
    let m = new Map();
    for (let op of rewrites) {
      if (op == null) {
        // ignore deleted ops
        continue;
      }
      if (m.get(op.index) != null) {
        throw new Error("should only be one op per index");
      }
      m.set(op.index, op);
    }
    return m;
  }

  /**
   * @param {Text} a
   * @param {Text} b
   * @returns {string}
   */
  catOpText(a, b) {
    let x = "";
    let y = "";
    if (a != null) {
      x = a.toString();
    }
    if (b != null) {
      y = b.toString();
    }
    return x + y;
  }

  /**
   * Get all operations before an index of a particular kind
   * @param {Rewrites} rewrites
   * @param {any} kind
   * @param {number} before
   */
  getKindOfOps(rewrites, kind, before) {
    return rewrites.slice(0, before).filter(op => op && op instanceof kind);
  }
}
class RewriteOperation {
  /**
   * @param {CommonTokenStream} tokens
   * @param {number} index
   * @param {number} instructionIndex
   * @param {Text} text
   */
  constructor(tokens, index, instructionIndex, text) {
    this.tokens = tokens;
    this.instructionIndex = instructionIndex;
    this.index = index;
    this.text = text === undefined ? "" : text;
  }
  toString() {
    let opName = this.constructor.name;
    let $index = opName.indexOf("$");
    opName = opName.substring($index + 1, opName.length);
    return "<" + opName + "@" + this.tokens.get(this.index) + ":\"" + this.text + "\">";
  }
}
class InsertBeforeOp extends RewriteOperation {
  /**
   * @param {CommonTokenStream} tokens
   * @param {number} index
   * @param {number} instructionIndex
   * @param {Text} text
   */
  constructor(tokens, index, instructionIndex, text) {
    super(tokens, index, instructionIndex, text);
  }

  /**
   * @param {string[]} buf
   * @returns {number} the index of the next token to operate on
   */
  execute(buf) {
    if (this.text) {
      buf.push(this.text.toString());
    }
    if (this.tokens.get(this.index).type !== Token.EOF) {
      buf.push(String(this.tokens.get(this.index).text));
    }
    return this.index + 1;
  }
}
class InsertAfterOp extends InsertBeforeOp {
  /**
   * @param {CommonTokenStream} tokens
   * @param {number} index
   * @param {number} instructionIndex
   * @param {Text} text
   */
  constructor(tokens, index, instructionIndex, text) {
    super(tokens, index + 1, instructionIndex, text); // insert after is insert before index+1
  }
}
class ReplaceOp extends RewriteOperation {
  /**
   * @param {CommonTokenStream} tokens
   * @param {number} from
   * @param {number} to
   * @param {number} instructionIndex
   * @param {Text} text
   */
  constructor(tokens, from, to, instructionIndex, text) {
    super(tokens, from, instructionIndex, text);
    this.lastIndex = to;
  }

  /**
   * @param {string[]} buf
   * @returns {number} the index of the next token to operate on
   */
  execute(buf) {
    if (this.text) {
      buf.push(this.text.toString());
    }
    return this.lastIndex + 1;
  }
  toString() {
    if (this.text == null) {
      return "<DeleteOp@" + this.tokens.get(this.index) + ".." + this.tokens.get(this.lastIndex) + ">";
    }
    return "<ReplaceOp@" + this.tokens.get(this.index) + ".." + this.tokens.get(this.lastIndex) + ":\"" + this.text + "\">";
  }
}
;// ./src/antlr4/index.web.js
/* Copyright (c) 2012-2022 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */










































/* harmony default export */ const index_web = ({
  atn: atn,
  dfa: dfa,
  context: context,
  misc: misc,
  tree: tree,
  error: error,
  Token: Token,
  CommonToken: CommonToken,
  CharStreams: CharStreams,
  CharStream: CharStream,
  InputStream: InputStream,
  CommonTokenStream: CommonTokenStream,
  Lexer: Lexer,
  Parser: Parser,
  ParserRuleContext: ParserRuleContext,
  Interval: Interval,
  IntervalSet: IntervalSet,
  LL1Analyzer: LL1Analyzer,
  Utils: utils,
  TokenStreamRewriter: TokenStreamRewriter
});

;// ./src/math/mathjsLexer.js
// Generated from mathjs.g4 by ANTLR 4.13.2
// jshint ignore: start

const serializedATN = [4, 0, 308, 3229, 6, -1, 2, 0, 7, 0, 2, 1, 7, 1, 2, 2, 7, 2, 2, 3, 7, 3, 2, 4, 7, 4, 2, 5, 7, 5, 2, 6, 7, 6, 2, 7, 7, 7, 2, 8, 7, 8, 2, 9, 7, 9, 2, 10, 7, 10, 2, 11, 7, 11, 2, 12, 7, 12, 2, 13, 7, 13, 2, 14, 7, 14, 2, 15, 7, 15, 2, 16, 7, 16, 2, 17, 7, 17, 2, 18, 7, 18, 2, 19, 7, 19, 2, 20, 7, 20, 2, 21, 7, 21, 2, 22, 7, 22, 2, 23, 7, 23, 2, 24, 7, 24, 2, 25, 7, 25, 2, 26, 7, 26, 2, 27, 7, 27, 2, 28, 7, 28, 2, 29, 7, 29, 2, 30, 7, 30, 2, 31, 7, 31, 2, 32, 7, 32, 2, 33, 7, 33, 2, 34, 7, 34, 2, 35, 7, 35, 2, 36, 7, 36, 2, 37, 7, 37, 2, 38, 7, 38, 2, 39, 7, 39, 2, 40, 7, 40, 2, 41, 7, 41, 2, 42, 7, 42, 2, 43, 7, 43, 2, 44, 7, 44, 2, 45, 7, 45, 2, 46, 7, 46, 2, 47, 7, 47, 2, 48, 7, 48, 2, 49, 7, 49, 2, 50, 7, 50, 2, 51, 7, 51, 2, 52, 7, 52, 2, 53, 7, 53, 2, 54, 7, 54, 2, 55, 7, 55, 2, 56, 7, 56, 2, 57, 7, 57, 2, 58, 7, 58, 2, 59, 7, 59, 2, 60, 7, 60, 2, 61, 7, 61, 2, 62, 7, 62, 2, 63, 7, 63, 2, 64, 7, 64, 2, 65, 7, 65, 2, 66, 7, 66, 2, 67, 7, 67, 2, 68, 7, 68, 2, 69, 7, 69, 2, 70, 7, 70, 2, 71, 7, 71, 2, 72, 7, 72, 2, 73, 7, 73, 2, 74, 7, 74, 2, 75, 7, 75, 2, 76, 7, 76, 2, 77, 7, 77, 2, 78, 7, 78, 2, 79, 7, 79, 2, 80, 7, 80, 2, 81, 7, 81, 2, 82, 7, 82, 2, 83, 7, 83, 2, 84, 7, 84, 2, 85, 7, 85, 2, 86, 7, 86, 2, 87, 7, 87, 2, 88, 7, 88, 2, 89, 7, 89, 2, 90, 7, 90, 2, 91, 7, 91, 2, 92, 7, 92, 2, 93, 7, 93, 2, 94, 7, 94, 2, 95, 7, 95, 2, 96, 7, 96, 2, 97, 7, 97, 2, 98, 7, 98, 2, 99, 7, 99, 2, 100, 7, 100, 2, 101, 7, 101, 2, 102, 7, 102, 2, 103, 7, 103, 2, 104, 7, 104, 2, 105, 7, 105, 2, 106, 7, 106, 2, 107, 7, 107, 2, 108, 7, 108, 2, 109, 7, 109, 2, 110, 7, 110, 2, 111, 7, 111, 2, 112, 7, 112, 2, 113, 7, 113, 2, 114, 7, 114, 2, 115, 7, 115, 2, 116, 7, 116, 2, 117, 7, 117, 2, 118, 7, 118, 2, 119, 7, 119, 2, 120, 7, 120, 2, 121, 7, 121, 2, 122, 7, 122, 2, 123, 7, 123, 2, 124, 7, 124, 2, 125, 7, 125, 2, 126, 7, 126, 2, 127, 7, 127, 2, 128, 7, 128, 2, 129, 7, 129, 2, 130, 7, 130, 2, 131, 7, 131, 2, 132, 7, 132, 2, 133, 7, 133, 2, 134, 7, 134, 2, 135, 7, 135, 2, 136, 7, 136, 2, 137, 7, 137, 2, 138, 7, 138, 2, 139, 7, 139, 2, 140, 7, 140, 2, 141, 7, 141, 2, 142, 7, 142, 2, 143, 7, 143, 2, 144, 7, 144, 2, 145, 7, 145, 2, 146, 7, 146, 2, 147, 7, 147, 2, 148, 7, 148, 2, 149, 7, 149, 2, 150, 7, 150, 2, 151, 7, 151, 2, 152, 7, 152, 2, 153, 7, 153, 2, 154, 7, 154, 2, 155, 7, 155, 2, 156, 7, 156, 2, 157, 7, 157, 2, 158, 7, 158, 2, 159, 7, 159, 2, 160, 7, 160, 2, 161, 7, 161, 2, 162, 7, 162, 2, 163, 7, 163, 2, 164, 7, 164, 2, 165, 7, 165, 2, 166, 7, 166, 2, 167, 7, 167, 2, 168, 7, 168, 2, 169, 7, 169, 2, 170, 7, 170, 2, 171, 7, 171, 2, 172, 7, 172, 2, 173, 7, 173, 2, 174, 7, 174, 2, 175, 7, 175, 2, 176, 7, 176, 2, 177, 7, 177, 2, 178, 7, 178, 2, 179, 7, 179, 2, 180, 7, 180, 2, 181, 7, 181, 2, 182, 7, 182, 2, 183, 7, 183, 2, 184, 7, 184, 2, 185, 7, 185, 2, 186, 7, 186, 2, 187, 7, 187, 2, 188, 7, 188, 2, 189, 7, 189, 2, 190, 7, 190, 2, 191, 7, 191, 2, 192, 7, 192, 2, 193, 7, 193, 2, 194, 7, 194, 2, 195, 7, 195, 2, 196, 7, 196, 2, 197, 7, 197, 2, 198, 7, 198, 2, 199, 7, 199, 2, 200, 7, 200, 2, 201, 7, 201, 2, 202, 7, 202, 2, 203, 7, 203, 2, 204, 7, 204, 2, 205, 7, 205, 2, 206, 7, 206, 2, 207, 7, 207, 2, 208, 7, 208, 2, 209, 7, 209, 2, 210, 7, 210, 2, 211, 7, 211, 2, 212, 7, 212, 2, 213, 7, 213, 2, 214, 7, 214, 2, 215, 7, 215, 2, 216, 7, 216, 2, 217, 7, 217, 2, 218, 7, 218, 2, 219, 7, 219, 2, 220, 7, 220, 2, 221, 7, 221, 2, 222, 7, 222, 2, 223, 7, 223, 2, 224, 7, 224, 2, 225, 7, 225, 2, 226, 7, 226, 2, 227, 7, 227, 2, 228, 7, 228, 2, 229, 7, 229, 2, 230, 7, 230, 2, 231, 7, 231, 2, 232, 7, 232, 2, 233, 7, 233, 2, 234, 7, 234, 2, 235, 7, 235, 2, 236, 7, 236, 2, 237, 7, 237, 2, 238, 7, 238, 2, 239, 7, 239, 2, 240, 7, 240, 2, 241, 7, 241, 2, 242, 7, 242, 2, 243, 7, 243, 2, 244, 7, 244, 2, 245, 7, 245, 2, 246, 7, 246, 2, 247, 7, 247, 2, 248, 7, 248, 2, 249, 7, 249, 2, 250, 7, 250, 2, 251, 7, 251, 2, 252, 7, 252, 2, 253, 7, 253, 2, 254, 7, 254, 2, 255, 7, 255, 2, 256, 7, 256, 2, 257, 7, 257, 2, 258, 7, 258, 2, 259, 7, 259, 2, 260, 7, 260, 2, 261, 7, 261, 2, 262, 7, 262, 2, 263, 7, 263, 2, 264, 7, 264, 2, 265, 7, 265, 2, 266, 7, 266, 2, 267, 7, 267, 2, 268, 7, 268, 2, 269, 7, 269, 2, 270, 7, 270, 2, 271, 7, 271, 2, 272, 7, 272, 2, 273, 7, 273, 2, 274, 7, 274, 2, 275, 7, 275, 2, 276, 7, 276, 2, 277, 7, 277, 2, 278, 7, 278, 2, 279, 7, 279, 2, 280, 7, 280, 2, 281, 7, 281, 2, 282, 7, 282, 2, 283, 7, 283, 2, 284, 7, 284, 2, 285, 7, 285, 2, 286, 7, 286, 2, 287, 7, 287, 2, 288, 7, 288, 2, 289, 7, 289, 2, 290, 7, 290, 2, 291, 7, 291, 2, 292, 7, 292, 2, 293, 7, 293, 2, 294, 7, 294, 2, 295, 7, 295, 2, 296, 7, 296, 2, 297, 7, 297, 2, 298, 7, 298, 2, 299, 7, 299, 2, 300, 7, 300, 2, 301, 7, 301, 2, 302, 7, 302, 2, 303, 7, 303, 2, 304, 7, 304, 2, 305, 7, 305, 2, 306, 7, 306, 2, 307, 7, 307, 2, 308, 7, 308, 1, 0, 1, 0, 1, 1, 1, 1, 1, 2, 1, 2, 1, 3, 1, 3, 1, 4, 1, 4, 1, 5, 1, 5, 1, 6, 1, 6, 1, 7, 1, 7, 1, 8, 1, 8, 1, 9, 1, 9, 1, 10, 1, 10, 1, 11, 1, 11, 1, 12, 1, 12, 1, 13, 1, 13, 1, 13, 1, 14, 1, 14, 1, 15, 1, 15, 1, 15, 1, 16, 1, 16, 1, 17, 1, 17, 1, 17, 1, 18, 1, 18, 1, 18, 1, 18, 1, 19, 1, 19, 1, 19, 1, 19, 1, 20, 1, 20, 1, 20, 1, 21, 1, 21, 1, 21, 1, 22, 1, 22, 1, 22, 1, 23, 1, 23, 1, 23, 1, 24, 1, 24, 1, 25, 1, 25, 1, 26, 1, 26, 1, 27, 1, 27, 1, 28, 1, 28, 1, 29, 1, 29, 1, 29, 4, 29, 692, 8, 29, 11, 29, 12, 29, 693, 3, 29, 696, 8, 29, 1, 29, 1, 29, 5, 29, 700, 8, 29, 10, 29, 12, 29, 703, 9, 29, 1, 29, 1, 29, 4, 29, 707, 8, 29, 11, 29, 12, 29, 708, 3, 29, 711, 8, 29, 1, 29, 1, 29, 1, 29, 4, 29, 716, 8, 29, 11, 29, 12, 29, 717, 3, 29, 720, 8, 29, 1, 29, 1, 29, 5, 29, 724, 8, 29, 10, 29, 12, 29, 727, 9, 29, 1, 29, 1, 29, 4, 29, 731, 8, 29, 11, 29, 12, 29, 732, 3, 29, 735, 8, 29, 3, 29, 737, 8, 29, 1, 29, 1, 29, 3, 29, 741, 8, 29, 1, 29, 1, 29, 3, 29, 745, 8, 29, 3, 29, 747, 8, 29, 1, 30, 1, 30, 1, 30, 1, 30, 5, 30, 753, 8, 30, 10, 30, 12, 30, 756, 9, 30, 1, 30, 1, 30, 1, 30, 1, 30, 1, 30, 5, 30, 763, 8, 30, 10, 30, 12, 30, 766, 9, 30, 1, 30, 1, 30, 1, 30, 1, 30, 1, 30, 5, 30, 773, 8, 30, 10, 30, 12, 30, 776, 9, 30, 1, 30, 3, 30, 779, 8, 30, 1, 31, 1, 31, 1, 31, 1, 31, 1, 31, 1, 32, 1, 32, 1, 32, 1, 32, 1, 32, 1, 32, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 1, 33, 3, 33, 835, 8, 33, 1, 34, 1, 34, 1, 34, 1, 35, 1, 35, 1, 35, 1, 35, 1, 36, 1, 36, 1, 36, 1, 36, 1, 36, 1, 36, 1, 36, 1, 37, 1, 37, 1, 37, 1, 37, 1, 37, 1, 37, 1, 37, 1, 37, 1, 38, 1, 38, 1, 38, 1, 38, 1, 38, 1, 38, 1, 38, 1, 38, 1, 38, 1, 39, 1, 39, 1, 39, 1, 39, 1, 39, 1, 39, 1, 39, 1, 40, 1, 40, 1, 40, 1, 40, 1, 40, 1, 40, 1, 40, 1, 40, 1, 41, 1, 41, 1, 41, 1, 41, 1, 41, 1, 41, 1, 41, 1, 41, 1, 41, 1, 41, 1, 42, 1, 42, 1, 42, 1, 42, 1, 42, 1, 42, 1, 42, 1, 42, 1, 42, 1, 42, 1, 43, 1, 43, 1, 43, 1, 43, 1, 43, 1, 43, 1, 43, 1, 44, 1, 44, 1, 44, 1, 44, 1, 44, 1, 44, 1, 45, 1, 45, 1, 45, 1, 45, 1, 45, 1, 45, 1, 45, 1, 46, 1, 46, 1, 46, 1, 46, 1, 46, 1, 46, 1, 46, 1, 46, 1, 46, 1, 46, 1, 46, 1, 46, 1, 46, 1, 46, 1, 47, 1, 47, 1, 47, 1, 47, 1, 48, 1, 48, 1, 48, 1, 49, 1, 49, 1, 49, 1, 49, 1, 50, 1, 50, 1, 50, 1, 50, 1, 51, 1, 51, 1, 51, 1, 51, 1, 51, 1, 51, 1, 51, 3, 51, 959, 8, 51, 1, 52, 1, 52, 1, 52, 1, 52, 1, 52, 1, 52, 1, 52, 3, 52, 968, 8, 52, 1, 53, 1, 53, 1, 54, 1, 54, 1, 54, 1, 55, 1, 55, 1, 55, 1, 55, 1, 55, 1, 55, 1, 55, 1, 55, 1, 56, 1, 56, 1, 56, 1, 56, 1, 56, 1, 56, 1, 56, 1, 56, 1, 57, 1, 57, 1, 57, 1, 57, 1, 57, 1, 57, 1, 57, 1, 57, 1, 58, 1, 58, 1, 58, 1, 58, 1, 58, 1, 58, 1, 58, 1, 58, 1, 59, 1, 59, 1, 59, 1, 59, 1, 59, 1, 59, 1, 59, 1, 59, 1, 60, 1, 60, 1, 60, 1, 60, 1, 60, 1, 60, 1, 60, 1, 60, 1, 61, 1, 61, 1, 61, 1, 61, 1, 61, 1, 61, 1, 61, 1, 61, 1, 62, 1, 62, 1, 62, 1, 62, 1, 62, 1, 62, 1, 62, 1, 62, 1, 63, 1, 63, 1, 63, 1, 63, 1, 63, 1, 63, 1, 63, 1, 63, 1, 64, 1, 64, 1, 64, 1, 64, 1, 64, 1, 64, 1, 64, 1, 64, 1, 65, 1, 65, 1, 65, 1, 65, 1, 65, 1, 65, 1, 65, 1, 65, 1, 66, 1, 66, 1, 66, 1, 66, 1, 66, 1, 66, 1, 66, 1, 66, 1, 67, 1, 67, 1, 67, 1, 67, 1, 68, 1, 68, 1, 68, 1, 68, 1, 68, 1, 68, 1, 68, 1, 68, 1, 68, 1, 69, 1, 69, 1, 69, 1, 69, 1, 70, 1, 70, 1, 70, 1, 70, 1, 70, 1, 71, 1, 71, 1, 71, 1, 71, 1, 71, 1, 72, 1, 72, 1, 72, 1, 72, 1, 72, 1, 72, 1, 73, 1, 73, 1, 73, 1, 73, 1, 74, 1, 74, 1, 74, 1, 74, 1, 75, 1, 75, 1, 75, 1, 75, 1, 76, 1, 76, 1, 76, 1, 76, 1, 76, 1, 76, 1, 76, 1, 77, 1, 77, 1, 77, 1, 77, 1, 77, 1, 77, 1, 77, 1, 78, 1, 78, 1, 78, 1, 78, 1, 78, 1, 78, 1, 78, 1, 78, 1, 79, 1, 79, 1, 79, 1, 79, 1, 79, 1, 79, 1, 79, 1, 79, 1, 80, 1, 80, 1, 80, 1, 80, 1, 81, 1, 81, 1, 81, 1, 81, 1, 81, 1, 82, 1, 82, 1, 82, 1, 82, 1, 83, 1, 83, 1, 83, 1, 83, 1, 83, 1, 84, 1, 84, 1, 84, 1, 84, 1, 85, 1, 85, 1, 85, 1, 85, 1, 85, 1, 86, 1, 86, 1, 86, 1, 86, 1, 87, 1, 87, 1, 87, 1, 87, 1, 87, 1, 88, 1, 88, 1, 88, 1, 88, 1, 89, 1, 89, 1, 89, 1, 89, 1, 89, 1, 90, 1, 90, 1, 90, 1, 90, 1, 91, 1, 91, 1, 91, 1, 91, 1, 91, 1, 92, 1, 92, 1, 92, 1, 92, 1, 92, 1, 93, 1, 93, 1, 93, 1, 93, 1, 93, 1, 93, 1, 94, 1, 94, 1, 94, 1, 94, 1, 94, 1, 95, 1, 95, 1, 95, 1, 95, 1, 95, 1, 95, 1, 96, 1, 96, 1, 96, 1, 96, 1, 96, 1, 97, 1, 97, 1, 97, 1, 97, 1, 97, 1, 97, 1, 98, 1, 98, 1, 98, 1, 98, 1, 98, 1, 99, 1, 99, 1, 99, 1, 99, 1, 99, 1, 99, 1, 100, 1, 100, 1, 100, 1, 100, 1, 100, 1, 100, 1, 101, 1, 101, 1, 101, 1, 101, 1, 101, 1, 101, 1, 102, 1, 102, 1, 102, 1, 102, 1, 102, 1, 102, 1, 102, 1, 102, 1, 102, 1, 102, 1, 103, 1, 103, 1, 103, 1, 103, 1, 103, 1, 103, 1, 103, 1, 103, 1, 104, 1, 104, 1, 104, 1, 104, 1, 104, 1, 104, 1, 104, 1, 104, 1, 105, 1, 105, 1, 105, 1, 105, 1, 105, 1, 105, 1, 106, 1, 106, 1, 106, 1, 106, 1, 106, 1, 107, 1, 107, 1, 107, 1, 107, 1, 108, 1, 108, 1, 108, 1, 108, 1, 108, 1, 108, 1, 108, 1, 109, 1, 109, 1, 109, 1, 109, 1, 109, 1, 110, 1, 110, 1, 110, 1, 110, 1, 110, 1, 110, 1, 110, 1, 110, 1, 110, 1, 110, 1, 110, 1, 110, 1, 111, 1, 111, 1, 111, 1, 111, 1, 111, 1, 112, 1, 112, 1, 112, 1, 112, 1, 112, 1, 112, 1, 112, 1, 112, 1, 112, 1, 112, 1, 112, 1, 113, 1, 113, 1, 113, 1, 113, 1, 113, 1, 113, 1, 114, 1, 114, 1, 114, 1, 114, 1, 115, 1, 115, 1, 115, 1, 116, 1, 116, 1, 116, 1, 116, 1, 117, 1, 117, 1, 117, 1, 117, 1, 117, 1, 117, 1, 118, 1, 118, 1, 118, 1, 118, 1, 118, 1, 118, 1, 118, 1, 118, 1, 118, 1, 118, 1, 118, 1, 118, 1, 119, 1, 119, 1, 119, 1, 119, 1, 119, 1, 119, 1, 119, 1, 119, 1, 120, 1, 120, 1, 120, 1, 120, 1, 120, 1, 120, 1, 120, 1, 121, 1, 121, 1, 121, 1, 121, 1, 122, 1, 122, 1, 122, 1, 122, 1, 122, 1, 123, 1, 123, 1, 123, 1, 123, 1, 123, 1, 123, 1, 123, 1, 123, 1, 124, 1, 124, 1, 124, 1, 124, 1, 124, 1, 124, 1, 124, 1, 124, 1, 125, 1, 125, 1, 125, 1, 125, 1, 125, 1, 125, 1, 125, 1, 125, 1, 126, 1, 126, 1, 126, 1, 126, 1, 126, 1, 126, 1, 126, 1, 126, 1, 127, 1, 127, 1, 127, 1, 127, 1, 127, 1, 127, 1, 128, 1, 128, 1, 128, 1, 128, 1, 128, 1, 128, 1, 128, 1, 129, 1, 129, 1, 129, 1, 129, 1, 129, 1, 129, 1, 130, 1, 130, 1, 130, 1, 130, 1, 130, 1, 130, 1, 130, 1, 130, 1, 130, 1, 130, 1, 130, 1, 131, 1, 131, 1, 131, 1, 131, 1, 131, 1, 131, 1, 131, 1, 131, 1, 131, 1, 132, 1, 132, 1, 132, 1, 132, 1, 132, 1, 132, 1, 132, 1, 132, 1, 132, 1, 133, 1, 133, 1, 133, 1, 133, 1, 133, 1, 133, 1, 133, 1, 133, 1, 134, 1, 134, 1, 134, 1, 134, 1, 134, 1, 134, 1, 134, 1, 135, 1, 135, 1, 135, 1, 135, 1, 135, 1, 135, 1, 136, 1, 136, 1, 136, 1, 136, 1, 136, 1, 136, 1, 136, 1, 136, 1, 136, 1, 136, 1, 137, 1, 137, 1, 137, 1, 137, 1, 137, 1, 138, 1, 138, 1, 138, 1, 138, 1, 138, 1, 138, 1, 138, 1, 138, 1, 138, 1, 139, 1, 139, 1, 139, 1, 139, 1, 139, 1, 139, 1, 139, 1, 139, 1, 139, 1, 139, 1, 140, 1, 140, 1, 140, 1, 140, 1, 140, 1, 140, 1, 141, 1, 141, 1, 141, 1, 141, 1, 141, 1, 141, 1, 141, 1, 142, 1, 142, 1, 142, 1, 142, 1, 142, 1, 142, 1, 142, 1, 142, 1, 143, 1, 143, 1, 143, 1, 143, 1, 143, 1, 143, 1, 143, 1, 143, 1, 143, 1, 144, 1, 144, 1, 144, 1, 144, 1, 145, 1, 145, 1, 145, 1, 145, 1, 145, 1, 145, 1, 145, 1, 145, 1, 145, 1, 145, 1, 145, 3, 145, 1576, 8, 145, 1, 146, 1, 146, 1, 146, 1, 146, 1, 146, 1, 147, 1, 147, 1, 147, 1, 147, 1, 147, 1, 147, 1, 148, 1, 148, 1, 148, 1, 148, 1, 148, 1, 149, 1, 149, 1, 149, 1, 149, 1, 149, 1, 149, 1, 149, 1, 149, 1, 150, 1, 150, 1, 150, 1, 150, 1, 150, 1, 150, 1, 150, 1, 150, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 1, 151, 3, 151, 1627, 8, 151, 1, 152, 1, 152, 1, 152, 1, 152, 1, 152, 1, 152, 1, 153, 1, 153, 1, 153, 1, 153, 1, 153, 1, 154, 1, 154, 1, 154, 1, 154, 1, 154, 1, 154, 1, 155, 1, 155, 1, 155, 1, 155, 1, 155, 1, 156, 1, 156, 1, 156, 1, 156, 1, 157, 1, 157, 1, 157, 1, 157, 1, 157, 1, 157, 1, 157, 1, 157, 1, 157, 1, 157, 1, 157, 1, 157, 3, 157, 1667, 8, 157, 1, 158, 1, 158, 1, 158, 1, 158, 1, 159, 1, 159, 1, 159, 1, 159, 1, 159, 1, 159, 1, 159, 1, 160, 1, 160, 1, 160, 1, 160, 1, 160, 1, 160, 1, 160, 1, 160, 1, 161, 1, 161, 1, 161, 1, 161, 1, 161, 1, 162, 1, 162, 1, 162, 1, 162, 1, 162, 1, 162, 1, 163, 1, 163, 1, 163, 1, 163, 1, 164, 1, 164, 1, 164, 1, 164, 1, 164, 1, 164, 1, 164, 1, 165, 1, 165, 1, 165, 1, 165, 1, 165, 1, 165, 1, 165, 1, 165, 1, 165, 1, 165, 1, 165, 1, 166, 1, 166, 1, 167, 1, 167, 1, 167, 1, 167, 1, 167, 1, 168, 1, 168, 1, 168, 1, 168, 1, 168, 1, 169, 1, 169, 1, 169, 1, 169, 1, 169, 1, 169, 1, 169, 1, 169, 1, 169, 1, 169, 1, 169, 1, 169, 3, 169, 1745, 8, 169, 1, 170, 1, 170, 1, 170, 1, 170, 1, 170, 1, 170, 1, 171, 1, 171, 1, 171, 1, 171, 1, 171, 1, 171, 1, 171, 1, 171, 1, 171, 1, 171, 1, 172, 1, 172, 1, 172, 1, 172, 1, 172, 1, 172, 1, 172, 1, 172, 1, 172, 1, 172, 1, 173, 1, 173, 1, 173, 1, 173, 1, 173, 1, 174, 1, 174, 1, 174, 1, 174, 1, 174, 1, 175, 1, 175, 1, 175, 1, 175, 1, 176, 1, 176, 1, 176, 1, 176, 1, 176, 1, 176, 1, 177, 1, 177, 1, 177, 1, 177, 1, 177, 1, 178, 1, 178, 1, 178, 1, 178, 1, 178, 1, 178, 1, 179, 1, 179, 1, 179, 1, 179, 1, 180, 1, 180, 1, 180, 1, 180, 1, 180, 1, 181, 1, 181, 1, 181, 1, 181, 1, 181, 1, 181, 1, 181, 1, 182, 1, 182, 1, 182, 1, 182, 1, 182, 1, 182, 1, 182, 1, 183, 1, 183, 1, 183, 1, 183, 1, 183, 1, 183, 1, 183, 1, 183, 1, 184, 1, 184, 1, 184, 1, 184, 1, 184, 1, 184, 1, 184, 1, 184, 1, 185, 1, 185, 1, 185, 1, 185, 1, 185, 1, 186, 1, 186, 1, 186, 1, 186, 1, 186, 1, 186, 1, 186, 1, 186, 1, 187, 1, 187, 1, 187, 1, 187, 1, 187, 1, 187, 1, 188, 1, 188, 1, 188, 1, 188, 1, 188, 1, 188, 1, 188, 1, 188, 1, 189, 1, 189, 1, 189, 1, 189, 1, 189, 1, 189, 1, 189, 1, 189, 1, 189, 1, 189, 1, 189, 1, 189, 1, 190, 1, 190, 1, 190, 1, 190, 1, 190, 1, 190, 1, 190, 1, 190, 1, 191, 1, 191, 1, 191, 1, 191, 1, 191, 1, 191, 1, 191, 1, 191, 1, 192, 1, 192, 1, 192, 1, 192, 1, 193, 1, 193, 1, 193, 1, 193, 1, 193, 1, 193, 1, 193, 1, 194, 1, 194, 1, 194, 1, 194, 1, 195, 1, 195, 1, 195, 1, 195, 1, 195, 1, 195, 1, 195, 1, 195, 1, 195, 1, 196, 1, 196, 1, 196, 1, 196, 1, 196, 1, 197, 1, 197, 1, 197, 1, 197, 1, 197, 1, 197, 1, 198, 1, 198, 1, 198, 1, 198, 1, 198, 1, 198, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 1, 199, 3, 199, 1963, 8, 199, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 1, 200, 3, 200, 1991, 8, 200, 1, 201, 1, 201, 1, 201, 1, 201, 1, 201, 1, 201, 1, 201, 1, 201, 1, 202, 1, 202, 1, 202, 1, 202, 1, 202, 1, 202, 1, 202, 1, 202, 1, 202, 1, 202, 1, 203, 1, 203, 1, 203, 1, 203, 1, 203, 1, 203, 1, 203, 1, 203, 1, 204, 1, 204, 1, 204, 1, 204, 1, 204, 1, 204, 1, 204, 1, 204, 1, 205, 1, 205, 1, 205, 1, 205, 1, 205, 1, 205, 1, 206, 1, 206, 1, 206, 1, 206, 1, 206, 1, 206, 1, 206, 1, 206, 1, 207, 1, 207, 1, 207, 1, 207, 1, 208, 1, 208, 1, 208, 1, 208, 1, 208, 1, 208, 1, 209, 1, 209, 1, 209, 1, 209, 1, 209, 1, 209, 1, 209, 1, 210, 1, 210, 1, 210, 1, 210, 1, 210, 1, 210, 1, 210, 1, 210, 1, 210, 1, 210, 1, 210, 1, 210, 3, 210, 2070, 8, 210, 1, 211, 1, 211, 1, 211, 1, 211, 1, 211, 1, 211, 1, 211, 1, 211, 1, 211, 1, 211, 1, 211, 1, 211, 1, 211, 3, 211, 2085, 8, 211, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 1, 212, 3, 212, 2104, 8, 212, 1, 213, 1, 213, 1, 213, 1, 213, 1, 213, 1, 213, 1, 213, 1, 213, 1, 213, 1, 213, 1, 213, 1, 213, 1, 213, 1, 214, 1, 214, 1, 214, 1, 214, 1, 214, 1, 214, 1, 215, 1, 215, 1, 215, 1, 215, 1, 215, 1, 215, 1, 215, 1, 215, 3, 215, 2133, 8, 215, 1, 216, 1, 216, 1, 216, 1, 216, 1, 216, 1, 216, 1, 216, 1, 216, 1, 216, 3, 216, 2144, 8, 216, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 1, 217, 3, 217, 2163, 8, 217, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 1, 218, 3, 218, 2180, 8, 218, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 1, 219, 3, 219, 2202, 8, 219, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 1, 220, 3, 220, 2222, 8, 220, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 1, 221, 3, 221, 2241, 8, 221, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 1, 222, 3, 222, 2258, 8, 222, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 1, 223, 3, 223, 2279, 8, 223, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 1, 224, 3, 224, 2300, 8, 224, 1, 225, 1, 225, 1, 225, 1, 225, 1, 225, 1, 225, 1, 225, 1, 225, 1, 225, 1, 225, 1, 225, 3, 225, 2313, 8, 225, 1, 226, 1, 226, 1, 226, 1, 226, 1, 226, 1, 226, 1, 226, 1, 226, 1, 226, 3, 226, 2324, 8, 226, 1, 227, 1, 227, 1, 227, 1, 227, 1, 227, 1, 227, 1, 227, 1, 228, 1, 228, 1, 228, 1, 228, 1, 228, 1, 228, 1, 228, 1, 228, 1, 228, 1, 228, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 1, 229, 3, 229, 2362, 8, 229, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 1, 230, 3, 230, 2381, 8, 230, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 1, 231, 3, 231, 2405, 8, 231, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 1, 232, 3, 232, 2430, 8, 232, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 1, 233, 3, 233, 2449, 8, 233, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 1, 234, 3, 234, 2474, 8, 234, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 1, 235, 3, 235, 2501, 8, 235, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 1, 236, 3, 236, 2522, 8, 236, 1, 237, 1, 237, 1, 237, 1, 237, 1, 237, 1, 237, 1, 237, 1, 237, 1, 237, 1, 237, 1, 237, 3, 237, 2535, 8, 237, 1, 238, 1, 238, 1, 238, 1, 238, 1, 238, 1, 238, 1, 238, 1, 238, 1, 238, 3, 238, 2546, 8, 238, 1, 239, 1, 239, 1, 239, 1, 239, 1, 239, 1, 239, 1, 239, 1, 239, 1, 240, 1, 240, 1, 240, 1, 240, 1, 241, 1, 241, 1, 241, 1, 241, 1, 241, 1, 242, 1, 242, 1, 242, 1, 242, 1, 242, 1, 243, 1, 243, 1, 243, 1, 244, 1, 244, 1, 244, 1, 245, 1, 245, 1, 245, 1, 245, 1, 245, 1, 246, 1, 246, 1, 246, 1, 246, 1, 246, 1, 247, 1, 247, 1, 247, 1, 247, 1, 248, 1, 248, 1, 248, 1, 248, 1, 248, 1, 249, 1, 249, 1, 249, 1, 249, 1, 250, 1, 250, 1, 250, 1, 250, 1, 250, 1, 251, 1, 251, 1, 251, 1, 251, 1, 251, 1, 252, 1, 252, 1, 252, 1, 252, 1, 253, 1, 253, 1, 253, 1, 254, 1, 254, 1, 254, 1, 254, 1, 255, 1, 255, 1, 255, 1, 255, 1, 256, 1, 256, 1, 256, 1, 256, 1, 256, 1, 256, 1, 256, 1, 256, 1, 256, 1, 256, 1, 257, 1, 257, 1, 257, 1, 257, 1, 257, 1, 257, 1, 257, 1, 257, 1, 257, 1, 257, 1, 258, 1, 258, 1, 258, 1, 258, 1, 258, 1, 258, 1, 258, 1, 258, 1, 258, 1, 258, 1, 258, 1, 259, 1, 259, 1, 259, 1, 259, 1, 259, 1, 259, 1, 259, 1, 259, 1, 259, 1, 259, 1, 259, 1, 260, 1, 260, 1, 260, 1, 260, 1, 260, 1, 260, 1, 260, 1, 260, 1, 260, 1, 260, 1, 260, 1, 260, 1, 260, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 261, 1, 262, 1, 262, 1, 262, 1, 262, 1, 262, 1, 262, 1, 262, 1, 262, 1, 262, 1, 262, 1, 262, 1, 262, 1, 262, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 263, 1, 264, 1, 264, 1, 264, 1, 264, 1, 264, 1, 264, 1, 265, 1, 265, 1, 265, 1, 265, 1, 265, 1, 265, 1, 265, 1, 265, 1, 265, 1, 265, 1, 265, 1, 265, 1, 265, 1, 266, 1, 266, 1, 266, 1, 266, 1, 266, 1, 266, 1, 266, 1, 266, 1, 266, 1, 266, 1, 266, 1, 266, 1, 266, 1, 266, 3, 266, 2757, 8, 266, 1, 267, 1, 267, 1, 267, 1, 267, 1, 267, 1, 268, 1, 268, 1, 268, 1, 268, 1, 269, 1, 269, 1, 269, 1, 269, 1, 269, 1, 270, 1, 270, 1, 270, 1, 270, 1, 270, 1, 270, 1, 270, 1, 271, 1, 271, 1, 271, 1, 271, 1, 271, 1, 271, 1, 271, 1, 272, 1, 272, 1, 272, 1, 272, 1, 272, 1, 272, 1, 272, 1, 272, 1, 273, 1, 273, 1, 273, 1, 273, 1, 273, 1, 273, 1, 273, 1, 273, 1, 273, 1, 274, 1, 274, 1, 274, 1, 274, 1, 274, 1, 274, 1, 274, 1, 274, 1, 274, 1, 274, 1, 274, 1, 275, 1, 275, 1, 275, 1, 275, 1, 275, 1, 275, 1, 275, 1, 275, 1, 275, 1, 275, 1, 275, 1, 276, 1, 276, 1, 276, 1, 276, 1, 276, 1, 276, 1, 276, 1, 276, 1, 276, 1, 276, 1, 276, 1, 276, 1, 276, 1, 276, 3, 276, 2840, 8, 276, 1, 277, 1, 277, 1, 277, 1, 277, 1, 277, 1, 277, 1, 277, 1, 277, 1, 277, 1, 277, 1, 277, 1, 277, 3, 277, 2854, 8, 277, 1, 278, 1, 278, 1, 278, 1, 278, 1, 278, 1, 278, 1, 278, 1, 278, 1, 279, 1, 279, 1, 279, 1, 279, 1, 279, 1, 279, 1, 279, 1, 279, 1, 279, 1, 279, 1, 279, 1, 279, 1, 280, 1, 280, 1, 280, 1, 280, 1, 280, 1, 280, 1, 281, 1, 281, 1, 281, 1, 281, 1, 281, 1, 282, 1, 282, 1, 282, 1, 282, 1, 282, 1, 282, 1, 282, 1, 282, 1, 282, 1, 282, 1, 283, 1, 283, 1, 283, 1, 283, 1, 283, 1, 283, 1, 283, 1, 283, 1, 283, 1, 283, 1, 283, 1, 284, 1, 284, 1, 284, 1, 284, 1, 284, 1, 284, 1, 284, 1, 284, 1, 284, 1, 285, 1, 285, 1, 285, 1, 285, 1, 285, 1, 285, 1, 285, 1, 285, 1, 285, 1, 285, 1, 285, 1, 285, 1, 285, 1, 285, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 286, 1, 287, 1, 287, 1, 287, 1, 287, 1, 287, 1, 287, 1, 287, 1, 287, 1, 287, 1, 287, 1, 287, 1, 287, 1, 288, 1, 288, 1, 288, 1, 288, 1, 288, 1, 288, 1, 288, 1, 288, 1, 288, 1, 288, 1, 289, 1, 289, 1, 289, 1, 289, 1, 289, 1, 290, 1, 290, 1, 290, 1, 290, 1, 290, 1, 290, 1, 290, 1, 290, 1, 290, 1, 290, 1, 290, 1, 290, 1, 291, 1, 291, 1, 291, 1, 291, 1, 291, 1, 291, 1, 291, 1, 291, 1, 291, 1, 291, 1, 292, 1, 292, 1, 292, 1, 292, 1, 292, 1, 292, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 1, 293, 3, 293, 3034, 8, 293, 1, 294, 1, 294, 1, 294, 1, 294, 1, 294, 1, 294, 1, 294, 1, 294, 1, 294, 1, 295, 1, 295, 1, 295, 1, 295, 1, 295, 1, 295, 1, 295, 1, 295, 1, 295, 1, 295, 1, 296, 1, 296, 1, 296, 1, 296, 1, 296, 1, 296, 1, 296, 1, 296, 1, 297, 1, 297, 1, 297, 1, 297, 1, 297, 1, 297, 1, 297, 1, 297, 1, 297, 1, 298, 1, 298, 1, 298, 1, 298, 1, 298, 1, 298, 1, 298, 1, 298, 1, 298, 1, 298, 1, 298, 1, 299, 1, 299, 1, 299, 1, 299, 1, 299, 1, 299, 1, 299, 1, 299, 1, 299, 1, 299, 1, 299, 1, 300, 1, 300, 1, 300, 1, 300, 1, 300, 1, 300, 1, 300, 1, 300, 1, 300, 1, 300, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 1, 301, 3, 301, 3132, 8, 301, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 1, 302, 3, 302, 3155, 8, 302, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 1, 303, 3, 303, 3183, 8, 303, 1, 304, 1, 304, 3, 304, 3187, 8, 304, 1, 304, 1, 304, 5, 304, 3191, 8, 304, 10, 304, 12, 304, 3194, 9, 304, 1, 305, 1, 305, 1, 306, 4, 306, 3199, 8, 306, 11, 306, 12, 306, 3200, 1, 306, 1, 306, 1, 307, 1, 307, 1, 307, 1, 307, 5, 307, 3209, 8, 307, 10, 307, 12, 307, 3212, 9, 307, 1, 307, 1, 307, 1, 307, 1, 307, 1, 307, 1, 308, 1, 308, 1, 308, 1, 308, 5, 308, 3223, 8, 308, 10, 308, 12, 308, 3226, 9, 308, 1, 308, 1, 308, 1, 3210, 0, 309, 1, 1, 3, 2, 5, 3, 7, 4, 9, 5, 11, 6, 13, 7, 15, 8, 17, 9, 19, 10, 21, 11, 23, 12, 25, 13, 27, 14, 29, 15, 31, 16, 33, 17, 35, 18, 37, 19, 39, 20, 41, 21, 43, 22, 45, 23, 47, 24, 49, 25, 51, 26, 53, 27, 55, 28, 57, 29, 59, 30, 61, 31, 63, 32, 65, 33, 67, 34, 69, 35, 71, 36, 73, 37, 75, 38, 77, 39, 79, 40, 81, 41, 83, 42, 85, 43, 87, 44, 89, 45, 91, 46, 93, 47, 95, 48, 97, 49, 99, 50, 101, 51, 103, 52, 105, 53, 107, 54, 109, 55, 111, 56, 113, 57, 115, 58, 117, 59, 119, 60, 121, 61, 123, 62, 125, 63, 127, 64, 129, 65, 131, 66, 133, 67, 135, 68, 137, 69, 139, 70, 141, 71, 143, 72, 145, 73, 147, 74, 149, 75, 151, 76, 153, 77, 155, 78, 157, 79, 159, 80, 161, 81, 163, 82, 165, 83, 167, 84, 169, 85, 171, 86, 173, 87, 175, 88, 177, 89, 179, 90, 181, 91, 183, 92, 185, 93, 187, 94, 189, 95, 191, 96, 193, 97, 195, 98, 197, 99, 199, 100, 201, 101, 203, 102, 205, 103, 207, 104, 209, 105, 211, 106, 213, 107, 215, 108, 217, 109, 219, 110, 221, 111, 223, 112, 225, 113, 227, 114, 229, 115, 231, 116, 233, 117, 235, 118, 237, 119, 239, 120, 241, 121, 243, 122, 245, 123, 247, 124, 249, 125, 251, 126, 253, 127, 255, 128, 257, 129, 259, 130, 261, 131, 263, 132, 265, 133, 267, 134, 269, 135, 271, 136, 273, 137, 275, 138, 277, 139, 279, 140, 281, 141, 283, 142, 285, 143, 287, 144, 289, 145, 291, 146, 293, 147, 295, 148, 297, 149, 299, 150, 301, 151, 303, 152, 305, 153, 307, 154, 309, 155, 311, 156, 313, 157, 315, 158, 317, 159, 319, 160, 321, 161, 323, 162, 325, 163, 327, 164, 329, 165, 331, 166, 333, 167, 335, 168, 337, 169, 339, 170, 341, 171, 343, 172, 345, 173, 347, 174, 349, 175, 351, 176, 353, 177, 355, 178, 357, 179, 359, 180, 361, 181, 363, 182, 365, 183, 367, 184, 369, 185, 371, 186, 373, 187, 375, 188, 377, 189, 379, 190, 381, 191, 383, 192, 385, 193, 387, 194, 389, 195, 391, 196, 393, 197, 395, 198, 397, 199, 399, 200, 401, 201, 403, 202, 405, 203, 407, 204, 409, 205, 411, 206, 413, 207, 415, 208, 417, 209, 419, 210, 421, 211, 423, 212, 425, 213, 427, 214, 429, 215, 431, 216, 433, 217, 435, 218, 437, 219, 439, 220, 441, 221, 443, 222, 445, 223, 447, 224, 449, 225, 451, 226, 453, 227, 455, 228, 457, 229, 459, 230, 461, 231, 463, 232, 465, 233, 467, 234, 469, 235, 471, 236, 473, 237, 475, 238, 477, 239, 479, 240, 481, 241, 483, 242, 485, 243, 487, 244, 489, 245, 491, 246, 493, 247, 495, 248, 497, 249, 499, 250, 501, 251, 503, 252, 505, 253, 507, 254, 509, 255, 511, 256, 513, 257, 515, 258, 517, 259, 519, 260, 521, 261, 523, 262, 525, 263, 527, 264, 529, 265, 531, 266, 533, 267, 535, 268, 537, 269, 539, 270, 541, 271, 543, 272, 545, 273, 547, 274, 549, 275, 551, 276, 553, 277, 555, 278, 557, 279, 559, 280, 561, 281, 563, 282, 565, 283, 567, 284, 569, 285, 571, 286, 573, 287, 575, 288, 577, 289, 579, 290, 581, 291, 583, 292, 585, 293, 587, 294, 589, 295, 591, 296, 593, 297, 595, 298, 597, 299, 599, 300, 601, 301, 603, 302, 605, 303, 607, 304, 609, 305, 611, 0, 613, 306, 615, 307, 617, 308, 1, 0, 11, 1, 0, 48, 57, 1, 0, 49, 57, 2, 0, 43, 43, 45, 45, 1, 0, 39, 39, 1, 0, 34, 34, 1, 0, 96, 96, 2, 0, 65, 90, 95, 95, 3, 0, 48, 57, 65, 90, 95, 95, 10, 0, 192, 214, 216, 246, 248, 8191, 11264, 12287, 12352, 12687, 13056, 13183, 13312, 16383, 19968, 55295, 63744, 64255, 65280, 65520, 3, 0, 9, 10, 12, 13, 32, 32, 2, 0, 10, 10, 13, 13, 3317, 0, 1, 1, 0, 0, 0, 0, 3, 1, 0, 0, 0, 0, 5, 1, 0, 0, 0, 0, 7, 1, 0, 0, 0, 0, 9, 1, 0, 0, 0, 0, 11, 1, 0, 0, 0, 0, 13, 1, 0, 0, 0, 0, 15, 1, 0, 0, 0, 0, 17, 1, 0, 0, 0, 0, 19, 1, 0, 0, 0, 0, 21, 1, 0, 0, 0, 0, 23, 1, 0, 0, 0, 0, 25, 1, 0, 0, 0, 0, 27, 1, 0, 0, 0, 0, 29, 1, 0, 0, 0, 0, 31, 1, 0, 0, 0, 0, 33, 1, 0, 0, 0, 0, 35, 1, 0, 0, 0, 0, 37, 1, 0, 0, 0, 0, 39, 1, 0, 0, 0, 0, 41, 1, 0, 0, 0, 0, 43, 1, 0, 0, 0, 0, 45, 1, 0, 0, 0, 0, 47, 1, 0, 0, 0, 0, 49, 1, 0, 0, 0, 0, 51, 1, 0, 0, 0, 0, 53, 1, 0, 0, 0, 0, 55, 1, 0, 0, 0, 0, 57, 1, 0, 0, 0, 0, 59, 1, 0, 0, 0, 0, 61, 1, 0, 0, 0, 0, 63, 1, 0, 0, 0, 0, 65, 1, 0, 0, 0, 0, 67, 1, 0, 0, 0, 0, 69, 1, 0, 0, 0, 0, 71, 1, 0, 0, 0, 0, 73, 1, 0, 0, 0, 0, 75, 1, 0, 0, 0, 0, 77, 1, 0, 0, 0, 0, 79, 1, 0, 0, 0, 0, 81, 1, 0, 0, 0, 0, 83, 1, 0, 0, 0, 0, 85, 1, 0, 0, 0, 0, 87, 1, 0, 0, 0, 0, 89, 1, 0, 0, 0, 0, 91, 1, 0, 0, 0, 0, 93, 1, 0, 0, 0, 0, 95, 1, 0, 0, 0, 0, 97, 1, 0, 0, 0, 0, 99, 1, 0, 0, 0, 0, 101, 1, 0, 0, 0, 0, 103, 1, 0, 0, 0, 0, 105, 1, 0, 0, 0, 0, 107, 1, 0, 0, 0, 0, 109, 1, 0, 0, 0, 0, 111, 1, 0, 0, 0, 0, 113, 1, 0, 0, 0, 0, 115, 1, 0, 0, 0, 0, 117, 1, 0, 0, 0, 0, 119, 1, 0, 0, 0, 0, 121, 1, 0, 0, 0, 0, 123, 1, 0, 0, 0, 0, 125, 1, 0, 0, 0, 0, 127, 1, 0, 0, 0, 0, 129, 1, 0, 0, 0, 0, 131, 1, 0, 0, 0, 0, 133, 1, 0, 0, 0, 0, 135, 1, 0, 0, 0, 0, 137, 1, 0, 0, 0, 0, 139, 1, 0, 0, 0, 0, 141, 1, 0, 0, 0, 0, 143, 1, 0, 0, 0, 0, 145, 1, 0, 0, 0, 0, 147, 1, 0, 0, 0, 0, 149, 1, 0, 0, 0, 0, 151, 1, 0, 0, 0, 0, 153, 1, 0, 0, 0, 0, 155, 1, 0, 0, 0, 0, 157, 1, 0, 0, 0, 0, 159, 1, 0, 0, 0, 0, 161, 1, 0, 0, 0, 0, 163, 1, 0, 0, 0, 0, 165, 1, 0, 0, 0, 0, 167, 1, 0, 0, 0, 0, 169, 1, 0, 0, 0, 0, 171, 1, 0, 0, 0, 0, 173, 1, 0, 0, 0, 0, 175, 1, 0, 0, 0, 0, 177, 1, 0, 0, 0, 0, 179, 1, 0, 0, 0, 0, 181, 1, 0, 0, 0, 0, 183, 1, 0, 0, 0, 0, 185, 1, 0, 0, 0, 0, 187, 1, 0, 0, 0, 0, 189, 1, 0, 0, 0, 0, 191, 1, 0, 0, 0, 0, 193, 1, 0, 0, 0, 0, 195, 1, 0, 0, 0, 0, 197, 1, 0, 0, 0, 0, 199, 1, 0, 0, 0, 0, 201, 1, 0, 0, 0, 0, 203, 1, 0, 0, 0, 0, 205, 1, 0, 0, 0, 0, 207, 1, 0, 0, 0, 0, 209, 1, 0, 0, 0, 0, 211, 1, 0, 0, 0, 0, 213, 1, 0, 0, 0, 0, 215, 1, 0, 0, 0, 0, 217, 1, 0, 0, 0, 0, 219, 1, 0, 0, 0, 0, 221, 1, 0, 0, 0, 0, 223, 1, 0, 0, 0, 0, 225, 1, 0, 0, 0, 0, 227, 1, 0, 0, 0, 0, 229, 1, 0, 0, 0, 0, 231, 1, 0, 0, 0, 0, 233, 1, 0, 0, 0, 0, 235, 1, 0, 0, 0, 0, 237, 1, 0, 0, 0, 0, 239, 1, 0, 0, 0, 0, 241, 1, 0, 0, 0, 0, 243, 1, 0, 0, 0, 0, 245, 1, 0, 0, 0, 0, 247, 1, 0, 0, 0, 0, 249, 1, 0, 0, 0, 0, 251, 1, 0, 0, 0, 0, 253, 1, 0, 0, 0, 0, 255, 1, 0, 0, 0, 0, 257, 1, 0, 0, 0, 0, 259, 1, 0, 0, 0, 0, 261, 1, 0, 0, 0, 0, 263, 1, 0, 0, 0, 0, 265, 1, 0, 0, 0, 0, 267, 1, 0, 0, 0, 0, 269, 1, 0, 0, 0, 0, 271, 1, 0, 0, 0, 0, 273, 1, 0, 0, 0, 0, 275, 1, 0, 0, 0, 0, 277, 1, 0, 0, 0, 0, 279, 1, 0, 0, 0, 0, 281, 1, 0, 0, 0, 0, 283, 1, 0, 0, 0, 0, 285, 1, 0, 0, 0, 0, 287, 1, 0, 0, 0, 0, 289, 1, 0, 0, 0, 0, 291, 1, 0, 0, 0, 0, 293, 1, 0, 0, 0, 0, 295, 1, 0, 0, 0, 0, 297, 1, 0, 0, 0, 0, 299, 1, 0, 0, 0, 0, 301, 1, 0, 0, 0, 0, 303, 1, 0, 0, 0, 0, 305, 1, 0, 0, 0, 0, 307, 1, 0, 0, 0, 0, 309, 1, 0, 0, 0, 0, 311, 1, 0, 0, 0, 0, 313, 1, 0, 0, 0, 0, 315, 1, 0, 0, 0, 0, 317, 1, 0, 0, 0, 0, 319, 1, 0, 0, 0, 0, 321, 1, 0, 0, 0, 0, 323, 1, 0, 0, 0, 0, 325, 1, 0, 0, 0, 0, 327, 1, 0, 0, 0, 0, 329, 1, 0, 0, 0, 0, 331, 1, 0, 0, 0, 0, 333, 1, 0, 0, 0, 0, 335, 1, 0, 0, 0, 0, 337, 1, 0, 0, 0, 0, 339, 1, 0, 0, 0, 0, 341, 1, 0, 0, 0, 0, 343, 1, 0, 0, 0, 0, 345, 1, 0, 0, 0, 0, 347, 1, 0, 0, 0, 0, 349, 1, 0, 0, 0, 0, 351, 1, 0, 0, 0, 0, 353, 1, 0, 0, 0, 0, 355, 1, 0, 0, 0, 0, 357, 1, 0, 0, 0, 0, 359, 1, 0, 0, 0, 0, 361, 1, 0, 0, 0, 0, 363, 1, 0, 0, 0, 0, 365, 1, 0, 0, 0, 0, 367, 1, 0, 0, 0, 0, 369, 1, 0, 0, 0, 0, 371, 1, 0, 0, 0, 0, 373, 1, 0, 0, 0, 0, 375, 1, 0, 0, 0, 0, 377, 1, 0, 0, 0, 0, 379, 1, 0, 0, 0, 0, 381, 1, 0, 0, 0, 0, 383, 1, 0, 0, 0, 0, 385, 1, 0, 0, 0, 0, 387, 1, 0, 0, 0, 0, 389, 1, 0, 0, 0, 0, 391, 1, 0, 0, 0, 0, 393, 1, 0, 0, 0, 0, 395, 1, 0, 0, 0, 0, 397, 1, 0, 0, 0, 0, 399, 1, 0, 0, 0, 0, 401, 1, 0, 0, 0, 0, 403, 1, 0, 0, 0, 0, 405, 1, 0, 0, 0, 0, 407, 1, 0, 0, 0, 0, 409, 1, 0, 0, 0, 0, 411, 1, 0, 0, 0, 0, 413, 1, 0, 0, 0, 0, 415, 1, 0, 0, 0, 0, 417, 1, 0, 0, 0, 0, 419, 1, 0, 0, 0, 0, 421, 1, 0, 0, 0, 0, 423, 1, 0, 0, 0, 0, 425, 1, 0, 0, 0, 0, 427, 1, 0, 0, 0, 0, 429, 1, 0, 0, 0, 0, 431, 1, 0, 0, 0, 0, 433, 1, 0, 0, 0, 0, 435, 1, 0, 0, 0, 0, 437, 1, 0, 0, 0, 0, 439, 1, 0, 0, 0, 0, 441, 1, 0, 0, 0, 0, 443, 1, 0, 0, 0, 0, 445, 1, 0, 0, 0, 0, 447, 1, 0, 0, 0, 0, 449, 1, 0, 0, 0, 0, 451, 1, 0, 0, 0, 0, 453, 1, 0, 0, 0, 0, 455, 1, 0, 0, 0, 0, 457, 1, 0, 0, 0, 0, 459, 1, 0, 0, 0, 0, 461, 1, 0, 0, 0, 0, 463, 1, 0, 0, 0, 0, 465, 1, 0, 0, 0, 0, 467, 1, 0, 0, 0, 0, 469, 1, 0, 0, 0, 0, 471, 1, 0, 0, 0, 0, 473, 1, 0, 0, 0, 0, 475, 1, 0, 0, 0, 0, 477, 1, 0, 0, 0, 0, 479, 1, 0, 0, 0, 0, 481, 1, 0, 0, 0, 0, 483, 1, 0, 0, 0, 0, 485, 1, 0, 0, 0, 0, 487, 1, 0, 0, 0, 0, 489, 1, 0, 0, 0, 0, 491, 1, 0, 0, 0, 0, 493, 1, 0, 0, 0, 0, 495, 1, 0, 0, 0, 0, 497, 1, 0, 0, 0, 0, 499, 1, 0, 0, 0, 0, 501, 1, 0, 0, 0, 0, 503, 1, 0, 0, 0, 0, 505, 1, 0, 0, 0, 0, 507, 1, 0, 0, 0, 0, 509, 1, 0, 0, 0, 0, 511, 1, 0, 0, 0, 0, 513, 1, 0, 0, 0, 0, 515, 1, 0, 0, 0, 0, 517, 1, 0, 0, 0, 0, 519, 1, 0, 0, 0, 0, 521, 1, 0, 0, 0, 0, 523, 1, 0, 0, 0, 0, 525, 1, 0, 0, 0, 0, 527, 1, 0, 0, 0, 0, 529, 1, 0, 0, 0, 0, 531, 1, 0, 0, 0, 0, 533, 1, 0, 0, 0, 0, 535, 1, 0, 0, 0, 0, 537, 1, 0, 0, 0, 0, 539, 1, 0, 0, 0, 0, 541, 1, 0, 0, 0, 0, 543, 1, 0, 0, 0, 0, 545, 1, 0, 0, 0, 0, 547, 1, 0, 0, 0, 0, 549, 1, 0, 0, 0, 0, 551, 1, 0, 0, 0, 0, 553, 1, 0, 0, 0, 0, 555, 1, 0, 0, 0, 0, 557, 1, 0, 0, 0, 0, 559, 1, 0, 0, 0, 0, 561, 1, 0, 0, 0, 0, 563, 1, 0, 0, 0, 0, 565, 1, 0, 0, 0, 0, 567, 1, 0, 0, 0, 0, 569, 1, 0, 0, 0, 0, 571, 1, 0, 0, 0, 0, 573, 1, 0, 0, 0, 0, 575, 1, 0, 0, 0, 0, 577, 1, 0, 0, 0, 0, 579, 1, 0, 0, 0, 0, 581, 1, 0, 0, 0, 0, 583, 1, 0, 0, 0, 0, 585, 1, 0, 0, 0, 0, 587, 1, 0, 0, 0, 0, 589, 1, 0, 0, 0, 0, 591, 1, 0, 0, 0, 0, 593, 1, 0, 0, 0, 0, 595, 1, 0, 0, 0, 0, 597, 1, 0, 0, 0, 0, 599, 1, 0, 0, 0, 0, 601, 1, 0, 0, 0, 0, 603, 1, 0, 0, 0, 0, 605, 1, 0, 0, 0, 0, 607, 1, 0, 0, 0, 0, 609, 1, 0, 0, 0, 0, 613, 1, 0, 0, 0, 0, 615, 1, 0, 0, 0, 0, 617, 1, 0, 0, 0, 1, 619, 1, 0, 0, 0, 3, 621, 1, 0, 0, 0, 5, 623, 1, 0, 0, 0, 7, 625, 1, 0, 0, 0, 9, 627, 1, 0, 0, 0, 11, 629, 1, 0, 0, 0, 13, 631, 1, 0, 0, 0, 15, 633, 1, 0, 0, 0, 17, 635, 1, 0, 0, 0, 19, 637, 1, 0, 0, 0, 21, 639, 1, 0, 0, 0, 23, 641, 1, 0, 0, 0, 25, 643, 1, 0, 0, 0, 27, 645, 1, 0, 0, 0, 29, 648, 1, 0, 0, 0, 31, 650, 1, 0, 0, 0, 33, 653, 1, 0, 0, 0, 35, 655, 1, 0, 0, 0, 37, 658, 1, 0, 0, 0, 39, 662, 1, 0, 0, 0, 41, 666, 1, 0, 0, 0, 43, 669, 1, 0, 0, 0, 45, 672, 1, 0, 0, 0, 47, 675, 1, 0, 0, 0, 49, 678, 1, 0, 0, 0, 51, 680, 1, 0, 0, 0, 53, 682, 1, 0, 0, 0, 55, 684, 1, 0, 0, 0, 57, 686, 1, 0, 0, 0, 59, 746, 1, 0, 0, 0, 61, 778, 1, 0, 0, 0, 63, 780, 1, 0, 0, 0, 65, 785, 1, 0, 0, 0, 67, 834, 1, 0, 0, 0, 69, 836, 1, 0, 0, 0, 71, 839, 1, 0, 0, 0, 73, 843, 1, 0, 0, 0, 75, 850, 1, 0, 0, 0, 77, 858, 1, 0, 0, 0, 79, 867, 1, 0, 0, 0, 81, 874, 1, 0, 0, 0, 83, 882, 1, 0, 0, 0, 85, 892, 1, 0, 0, 0, 87, 902, 1, 0, 0, 0, 89, 909, 1, 0, 0, 0, 91, 915, 1, 0, 0, 0, 93, 922, 1, 0, 0, 0, 95, 936, 1, 0, 0, 0, 97, 940, 1, 0, 0, 0, 99, 943, 1, 0, 0, 0, 101, 947, 1, 0, 0, 0, 103, 958, 1, 0, 0, 0, 105, 967, 1, 0, 0, 0, 107, 969, 1, 0, 0, 0, 109, 971, 1, 0, 0, 0, 111, 974, 1, 0, 0, 0, 113, 982, 1, 0, 0, 0, 115, 990, 1, 0, 0, 0, 117, 998, 1, 0, 0, 0, 119, 1006, 1, 0, 0, 0, 121, 1014, 1, 0, 0, 0, 123, 1022, 1, 0, 0, 0, 125, 1030, 1, 0, 0, 0, 127, 1038, 1, 0, 0, 0, 129, 1046, 1, 0, 0, 0, 131, 1054, 1, 0, 0, 0, 133, 1062, 1, 0, 0, 0, 135, 1070, 1, 0, 0, 0, 137, 1074, 1, 0, 0, 0, 139, 1083, 1, 0, 0, 0, 141, 1087, 1, 0, 0, 0, 143, 1092, 1, 0, 0, 0, 145, 1097, 1, 0, 0, 0, 147, 1103, 1, 0, 0, 0, 149, 1107, 1, 0, 0, 0, 151, 1111, 1, 0, 0, 0, 153, 1115, 1, 0, 0, 0, 155, 1122, 1, 0, 0, 0, 157, 1129, 1, 0, 0, 0, 159, 1137, 1, 0, 0, 0, 161, 1145, 1, 0, 0, 0, 163, 1149, 1, 0, 0, 0, 165, 1154, 1, 0, 0, 0, 167, 1158, 1, 0, 0, 0, 169, 1163, 1, 0, 0, 0, 171, 1167, 1, 0, 0, 0, 173, 1172, 1, 0, 0, 0, 175, 1176, 1, 0, 0, 0, 177, 1181, 1, 0, 0, 0, 179, 1185, 1, 0, 0, 0, 181, 1190, 1, 0, 0, 0, 183, 1194, 1, 0, 0, 0, 185, 1199, 1, 0, 0, 0, 187, 1204, 1, 0, 0, 0, 189, 1210, 1, 0, 0, 0, 191, 1215, 1, 0, 0, 0, 193, 1221, 1, 0, 0, 0, 195, 1226, 1, 0, 0, 0, 197, 1232, 1, 0, 0, 0, 199, 1237, 1, 0, 0, 0, 201, 1243, 1, 0, 0, 0, 203, 1249, 1, 0, 0, 0, 205, 1255, 1, 0, 0, 0, 207, 1265, 1, 0, 0, 0, 209, 1273, 1, 0, 0, 0, 211, 1281, 1, 0, 0, 0, 213, 1287, 1, 0, 0, 0, 215, 1292, 1, 0, 0, 0, 217, 1296, 1, 0, 0, 0, 219, 1303, 1, 0, 0, 0, 221, 1308, 1, 0, 0, 0, 223, 1320, 1, 0, 0, 0, 225, 1325, 1, 0, 0, 0, 227, 1336, 1, 0, 0, 0, 229, 1342, 1, 0, 0, 0, 231, 1346, 1, 0, 0, 0, 233, 1349, 1, 0, 0, 0, 235, 1353, 1, 0, 0, 0, 237, 1359, 1, 0, 0, 0, 239, 1371, 1, 0, 0, 0, 241, 1379, 1, 0, 0, 0, 243, 1386, 1, 0, 0, 0, 245, 1390, 1, 0, 0, 0, 247, 1395, 1, 0, 0, 0, 249, 1403, 1, 0, 0, 0, 251, 1411, 1, 0, 0, 0, 253, 1419, 1, 0, 0, 0, 255, 1427, 1, 0, 0, 0, 257, 1433, 1, 0, 0, 0, 259, 1440, 1, 0, 0, 0, 261, 1446, 1, 0, 0, 0, 263, 1457, 1, 0, 0, 0, 265, 1466, 1, 0, 0, 0, 267, 1475, 1, 0, 0, 0, 269, 1483, 1, 0, 0, 0, 271, 1490, 1, 0, 0, 0, 273, 1496, 1, 0, 0, 0, 275, 1506, 1, 0, 0, 0, 277, 1511, 1, 0, 0, 0, 279, 1520, 1, 0, 0, 0, 281, 1530, 1, 0, 0, 0, 283, 1536, 1, 0, 0, 0, 285, 1543, 1, 0, 0, 0, 287, 1551, 1, 0, 0, 0, 289, 1560, 1, 0, 0, 0, 291, 1575, 1, 0, 0, 0, 293, 1577, 1, 0, 0, 0, 295, 1582, 1, 0, 0, 0, 297, 1588, 1, 0, 0, 0, 299, 1593, 1, 0, 0, 0, 301, 1601, 1, 0, 0, 0, 303, 1626, 1, 0, 0, 0, 305, 1628, 1, 0, 0, 0, 307, 1634, 1, 0, 0, 0, 309, 1639, 1, 0, 0, 0, 311, 1645, 1, 0, 0, 0, 313, 1650, 1, 0, 0, 0, 315, 1666, 1, 0, 0, 0, 317, 1668, 1, 0, 0, 0, 319, 1672, 1, 0, 0, 0, 321, 1679, 1, 0, 0, 0, 323, 1687, 1, 0, 0, 0, 325, 1692, 1, 0, 0, 0, 327, 1698, 1, 0, 0, 0, 329, 1702, 1, 0, 0, 0, 331, 1709, 1, 0, 0, 0, 333, 1720, 1, 0, 0, 0, 335, 1722, 1, 0, 0, 0, 337, 1727, 1, 0, 0, 0, 339, 1744, 1, 0, 0, 0, 341, 1746, 1, 0, 0, 0, 343, 1752, 1, 0, 0, 0, 345, 1762, 1, 0, 0, 0, 347, 1772, 1, 0, 0, 0, 349, 1777, 1, 0, 0, 0, 351, 1782, 1, 0, 0, 0, 353, 1786, 1, 0, 0, 0, 355, 1792, 1, 0, 0, 0, 357, 1797, 1, 0, 0, 0, 359, 1803, 1, 0, 0, 0, 361, 1807, 1, 0, 0, 0, 363, 1812, 1, 0, 0, 0, 365, 1819, 1, 0, 0, 0, 367, 1826, 1, 0, 0, 0, 369, 1834, 1, 0, 0, 0, 371, 1842, 1, 0, 0, 0, 373, 1847, 1, 0, 0, 0, 375, 1855, 1, 0, 0, 0, 377, 1861, 1, 0, 0, 0, 379, 1869, 1, 0, 0, 0, 381, 1881, 1, 0, 0, 0, 383, 1889, 1, 0, 0, 0, 385, 1897, 1, 0, 0, 0, 387, 1901, 1, 0, 0, 0, 389, 1908, 1, 0, 0, 0, 391, 1912, 1, 0, 0, 0, 393, 1921, 1, 0, 0, 0, 395, 1926, 1, 0, 0, 0, 397, 1932, 1, 0, 0, 0, 399, 1962, 1, 0, 0, 0, 401, 1990, 1, 0, 0, 0, 403, 1992, 1, 0, 0, 0, 405, 2000, 1, 0, 0, 0, 407, 2010, 1, 0, 0, 0, 409, 2018, 1, 0, 0, 0, 411, 2026, 1, 0, 0, 0, 413, 2032, 1, 0, 0, 0, 415, 2040, 1, 0, 0, 0, 417, 2044, 1, 0, 0, 0, 419, 2050, 1, 0, 0, 0, 421, 2069, 1, 0, 0, 0, 423, 2084, 1, 0, 0, 0, 425, 2103, 1, 0, 0, 0, 427, 2105, 1, 0, 0, 0, 429, 2118, 1, 0, 0, 0, 431, 2132, 1, 0, 0, 0, 433, 2143, 1, 0, 0, 0, 435, 2162, 1, 0, 0, 0, 437, 2179, 1, 0, 0, 0, 439, 2201, 1, 0, 0, 0, 441, 2221, 1, 0, 0, 0, 443, 2240, 1, 0, 0, 0, 445, 2257, 1, 0, 0, 0, 447, 2278, 1, 0, 0, 0, 449, 2299, 1, 0, 0, 0, 451, 2312, 1, 0, 0, 0, 453, 2323, 1, 0, 0, 0, 455, 2325, 1, 0, 0, 0, 457, 2332, 1, 0, 0, 0, 459, 2361, 1, 0, 0, 0, 461, 2380, 1, 0, 0, 0, 463, 2404, 1, 0, 0, 0, 465, 2429, 1, 0, 0, 0, 467, 2448, 1, 0, 0, 0, 469, 2473, 1, 0, 0, 0, 471, 2500, 1, 0, 0, 0, 473, 2521, 1, 0, 0, 0, 475, 2534, 1, 0, 0, 0, 477, 2545, 1, 0, 0, 0, 479, 2547, 1, 0, 0, 0, 481, 2555, 1, 0, 0, 0, 483, 2559, 1, 0, 0, 0, 485, 2564, 1, 0, 0, 0, 487, 2569, 1, 0, 0, 0, 489, 2572, 1, 0, 0, 0, 491, 2575, 1, 0, 0, 0, 493, 2580, 1, 0, 0, 0, 495, 2585, 1, 0, 0, 0, 497, 2589, 1, 0, 0, 0, 499, 2594, 1, 0, 0, 0, 501, 2598, 1, 0, 0, 0, 503, 2603, 1, 0, 0, 0, 505, 2608, 1, 0, 0, 0, 507, 2612, 1, 0, 0, 0, 509, 2615, 1, 0, 0, 0, 511, 2619, 1, 0, 0, 0, 513, 2623, 1, 0, 0, 0, 515, 2633, 1, 0, 0, 0, 517, 2643, 1, 0, 0, 0, 519, 2654, 1, 0, 0, 0, 521, 2665, 1, 0, 0, 0, 523, 2678, 1, 0, 0, 0, 525, 2694, 1, 0, 0, 0, 527, 2707, 1, 0, 0, 0, 529, 2723, 1, 0, 0, 0, 531, 2729, 1, 0, 0, 0, 533, 2756, 1, 0, 0, 0, 535, 2758, 1, 0, 0, 0, 537, 2763, 1, 0, 0, 0, 539, 2767, 1, 0, 0, 0, 541, 2772, 1, 0, 0, 0, 543, 2779, 1, 0, 0, 0, 545, 2786, 1, 0, 0, 0, 547, 2794, 1, 0, 0, 0, 549, 2803, 1, 0, 0, 0, 551, 2814, 1, 0, 0, 0, 553, 2839, 1, 0, 0, 0, 555, 2853, 1, 0, 0, 0, 557, 2855, 1, 0, 0, 0, 559, 2863, 1, 0, 0, 0, 561, 2875, 1, 0, 0, 0, 563, 2881, 1, 0, 0, 0, 565, 2886, 1, 0, 0, 0, 567, 2896, 1, 0, 0, 0, 569, 2907, 1, 0, 0, 0, 571, 2916, 1, 0, 0, 0, 573, 2930, 1, 0, 0, 0, 575, 2949, 1, 0, 0, 0, 577, 2961, 1, 0, 0, 0, 579, 2971, 1, 0, 0, 0, 581, 2976, 1, 0, 0, 0, 583, 2988, 1, 0, 0, 0, 585, 2998, 1, 0, 0, 0, 587, 3033, 1, 0, 0, 0, 589, 3035, 1, 0, 0, 0, 591, 3044, 1, 0, 0, 0, 593, 3054, 1, 0, 0, 0, 595, 3062, 1, 0, 0, 0, 597, 3071, 1, 0, 0, 0, 599, 3082, 1, 0, 0, 0, 601, 3093, 1, 0, 0, 0, 603, 3131, 1, 0, 0, 0, 605, 3154, 1, 0, 0, 0, 607, 3182, 1, 0, 0, 0, 609, 3186, 1, 0, 0, 0, 611, 3195, 1, 0, 0, 0, 613, 3198, 1, 0, 0, 0, 615, 3204, 1, 0, 0, 0, 617, 3218, 1, 0, 0, 0, 619, 620, 5, 46, 0, 0, 620, 2, 1, 0, 0, 0, 621, 622, 5, 40, 0, 0, 622, 4, 1, 0, 0, 0, 623, 624, 5, 41, 0, 0, 624, 6, 1, 0, 0, 0, 625, 626, 5, 44, 0, 0, 626, 8, 1, 0, 0, 0, 627, 628, 5, 91, 0, 0, 628, 10, 1, 0, 0, 0, 629, 630, 5, 93, 0, 0, 630, 12, 1, 0, 0, 0, 631, 632, 5, 33, 0, 0, 632, 14, 1, 0, 0, 0, 633, 634, 5, 37, 0, 0, 634, 16, 1, 0, 0, 0, 635, 636, 5, 42, 0, 0, 636, 18, 1, 0, 0, 0, 637, 638, 5, 47, 0, 0, 638, 20, 1, 0, 0, 0, 639, 640, 5, 43, 0, 0, 640, 22, 1, 0, 0, 0, 641, 642, 5, 38, 0, 0, 642, 24, 1, 0, 0, 0, 643, 644, 5, 62, 0, 0, 644, 26, 1, 0, 0, 0, 645, 646, 5, 62, 0, 0, 646, 647, 5, 61, 0, 0, 647, 28, 1, 0, 0, 0, 648, 649, 5, 60, 0, 0, 649, 30, 1, 0, 0, 0, 650, 651, 5, 60, 0, 0, 651, 652, 5, 61, 0, 0, 652, 32, 1, 0, 0, 0, 653, 654, 5, 61, 0, 0, 654, 34, 1, 0, 0, 0, 655, 656, 5, 61, 0, 0, 656, 657, 5, 61, 0, 0, 657, 36, 1, 0, 0, 0, 658, 659, 5, 61, 0, 0, 659, 660, 5, 61, 0, 0, 660, 661, 5, 61, 0, 0, 661, 38, 1, 0, 0, 0, 662, 663, 5, 33, 0, 0, 663, 664, 5, 61, 0, 0, 664, 665, 5, 61, 0, 0, 665, 40, 1, 0, 0, 0, 666, 667, 5, 33, 0, 0, 667, 668, 5, 61, 0, 0, 668, 42, 1, 0, 0, 0, 669, 670, 5, 60, 0, 0, 670, 671, 5, 62, 0, 0, 671, 44, 1, 0, 0, 0, 672, 673, 5, 38, 0, 0, 673, 674, 5, 38, 0, 0, 674, 46, 1, 0, 0, 0, 675, 676, 5, 124, 0, 0, 676, 677, 5, 124, 0, 0, 677, 48, 1, 0, 0, 0, 678, 679, 5, 63, 0, 0, 679, 50, 1, 0, 0, 0, 680, 681, 5, 58, 0, 0, 681, 52, 1, 0, 0, 0, 682, 683, 5, 123, 0, 0, 683, 54, 1, 0, 0, 0, 684, 685, 5, 125, 0, 0, 685, 56, 1, 0, 0, 0, 686, 687, 5, 45, 0, 0, 687, 58, 1, 0, 0, 0, 688, 695, 5, 48, 0, 0, 689, 691, 5, 46, 0, 0, 690, 692, 7, 0, 0, 0, 691, 690, 1, 0, 0, 0, 692, 693, 1, 0, 0, 0, 693, 691, 1, 0, 0, 0, 693, 694, 1, 0, 0, 0, 694, 696, 1, 0, 0, 0, 695, 689, 1, 0, 0, 0, 695, 696, 1, 0, 0, 0, 696, 747, 1, 0, 0, 0, 697, 701, 7, 1, 0, 0, 698, 700, 7, 0, 0, 0, 699, 698, 1, 0, 0, 0, 700, 703, 1, 0, 0, 0, 701, 699, 1, 0, 0, 0, 701, 702, 1, 0, 0, 0, 702, 710, 1, 0, 0, 0, 703, 701, 1, 0, 0, 0, 704, 706, 5, 46, 0, 0, 705, 707, 7, 0, 0, 0, 706, 705, 1, 0, 0, 0, 707, 708, 1, 0, 0, 0, 708, 706, 1, 0, 0, 0, 708, 709, 1, 0, 0, 0, 709, 711, 1, 0, 0, 0, 710, 704, 1, 0, 0, 0, 710, 711, 1, 0, 0, 0, 711, 747, 1, 0, 0, 0, 712, 719, 5, 48, 0, 0, 713, 715, 5, 46, 0, 0, 714, 716, 7, 0, 0, 0, 715, 714, 1, 0, 0, 0, 716, 717, 1, 0, 0, 0, 717, 715, 1, 0, 0, 0, 717, 718, 1, 0, 0, 0, 718, 720, 1, 0, 0, 0, 719, 713, 1, 0, 0, 0, 719, 720, 1, 0, 0, 0, 720, 737, 1, 0, 0, 0, 721, 725, 7, 1, 0, 0, 722, 724, 7, 0, 0, 0, 723, 722, 1, 0, 0, 0, 724, 727, 1, 0, 0, 0, 725, 723, 1, 0, 0, 0, 725, 726, 1, 0, 0, 0, 726, 734, 1, 0, 0, 0, 727, 725, 1, 0, 0, 0, 728, 730, 5, 46, 0, 0, 729, 731, 7, 0, 0, 0, 730, 729, 1, 0, 0, 0, 731, 732, 1, 0, 0, 0, 732, 730, 1, 0, 0, 0, 732, 733, 1, 0, 0, 0, 733, 735, 1, 0, 0, 0, 734, 728, 1, 0, 0, 0, 734, 735, 1, 0, 0, 0, 735, 737, 1, 0, 0, 0, 736, 712, 1, 0, 0, 0, 736, 721, 1, 0, 0, 0, 737, 738, 1, 0, 0, 0, 738, 740, 5, 69, 0, 0, 739, 741, 7, 2, 0, 0, 740, 739, 1, 0, 0, 0, 740, 741, 1, 0, 0, 0, 741, 742, 1, 0, 0, 0, 742, 744, 7, 0, 0, 0, 743, 745, 7, 0, 0, 0, 744, 743, 1, 0, 0, 0, 744, 745, 1, 0, 0, 0, 745, 747, 1, 0, 0, 0, 746, 688, 1, 0, 0, 0, 746, 697, 1, 0, 0, 0, 746, 736, 1, 0, 0, 0, 747, 60, 1, 0, 0, 0, 748, 754, 5, 39, 0, 0, 749, 753, 8, 3, 0, 0, 750, 751, 5, 92, 0, 0, 751, 753, 5, 39, 0, 0, 752, 749, 1, 0, 0, 0, 752, 750, 1, 0, 0, 0, 753, 756, 1, 0, 0, 0, 754, 752, 1, 0, 0, 0, 754, 755, 1, 0, 0, 0, 755, 757, 1, 0, 0, 0, 756, 754, 1, 0, 0, 0, 757, 779, 5, 39, 0, 0, 758, 764, 5, 34, 0, 0, 759, 763, 8, 4, 0, 0, 760, 761, 5, 92, 0, 0, 761, 763, 5, 34, 0, 0, 762, 759, 1, 0, 0, 0, 762, 760, 1, 0, 0, 0, 763, 766, 1, 0, 0, 0, 764, 762, 1, 0, 0, 0, 764, 765, 1, 0, 0, 0, 765, 767, 1, 0, 0, 0, 766, 764, 1, 0, 0, 0, 767, 779, 5, 34, 0, 0, 768, 774, 5, 96, 0, 0, 769, 773, 8, 5, 0, 0, 770, 771, 5, 92, 0, 0, 771, 773, 5, 96, 0, 0, 772, 769, 1, 0, 0, 0, 772, 770, 1, 0, 0, 0, 773, 776, 1, 0, 0, 0, 774, 772, 1, 0, 0, 0, 774, 775, 1, 0, 0, 0, 775, 777, 1, 0, 0, 0, 776, 774, 1, 0, 0, 0, 777, 779, 5, 96, 0, 0, 778, 748, 1, 0, 0, 0, 778, 758, 1, 0, 0, 0, 778, 768, 1, 0, 0, 0, 779, 62, 1, 0, 0, 0, 780, 781, 5, 78, 0, 0, 781, 782, 5, 85, 0, 0, 782, 783, 5, 76, 0, 0, 783, 784, 5, 76, 0, 0, 784, 64, 1, 0, 0, 0, 785, 786, 5, 69, 0, 0, 786, 787, 5, 82, 0, 0, 787, 788, 5, 82, 0, 0, 788, 789, 5, 79, 0, 0, 789, 790, 5, 82, 0, 0, 790, 66, 1, 0, 0, 0, 791, 835, 5, 77, 0, 0, 792, 793, 5, 75, 0, 0, 793, 835, 5, 77, 0, 0, 794, 795, 5, 68, 0, 0, 795, 835, 5, 77, 0, 0, 796, 797, 5, 67, 0, 0, 797, 835, 5, 77, 0, 0, 798, 799, 5, 77, 0, 0, 799, 835, 5, 77, 0, 0, 800, 801, 5, 77, 0, 0, 801, 835, 5, 50, 0, 0, 802, 803, 5, 75, 0, 0, 803, 804, 5, 77, 0, 0, 804, 835, 5, 50, 0, 0, 805, 806, 5, 68, 0, 0, 806, 807, 5, 77, 0, 0, 807, 835, 5, 50, 0, 0, 808, 809, 5, 67, 0, 0, 809, 810, 5, 77, 0, 0, 810, 835, 5, 50, 0, 0, 811, 812, 5, 77, 0, 0, 812, 813, 5, 77, 0, 0, 813, 835, 5, 50, 0, 0, 814, 815, 5, 77, 0, 0, 815, 835, 5, 51, 0, 0, 816, 817, 5, 75, 0, 0, 817, 818, 5, 77, 0, 0, 818, 835, 5, 51, 0, 0, 819, 820, 5, 68, 0, 0, 820, 821, 5, 77, 0, 0, 821, 835, 5, 51, 0, 0, 822, 823, 5, 67, 0, 0, 823, 824, 5, 77, 0, 0, 824, 835, 5, 51, 0, 0, 825, 826, 5, 77, 0, 0, 826, 827, 5, 77, 0, 0, 827, 835, 5, 51, 0, 0, 828, 835, 5, 76, 0, 0, 829, 830, 5, 77, 0, 0, 830, 835, 5, 76, 0, 0, 831, 835, 5, 71, 0, 0, 832, 833, 5, 75, 0, 0, 833, 835, 5, 71, 0, 0, 834, 791, 1, 0, 0, 0, 834, 792, 1, 0, 0, 0, 834, 794, 1, 0, 0, 0, 834, 796, 1, 0, 0, 0, 834, 798, 1, 0, 0, 0, 834, 800, 1, 0, 0, 0, 834, 802, 1, 0, 0, 0, 834, 805, 1, 0, 0, 0, 834, 808, 1, 0, 0, 0, 834, 811, 1, 0, 0, 0, 834, 814, 1, 0, 0, 0, 834, 816, 1, 0, 0, 0, 834, 819, 1, 0, 0, 0, 834, 822, 1, 0, 0, 0, 834, 825, 1, 0, 0, 0, 834, 828, 1, 0, 0, 0, 834, 829, 1, 0, 0, 0, 834, 831, 1, 0, 0, 0, 834, 832, 1, 0, 0, 0, 835, 68, 1, 0, 0, 0, 836, 837, 5, 73, 0, 0, 837, 838, 5, 70, 0, 0, 838, 70, 1, 0, 0, 0, 839, 840, 5, 73, 0, 0, 840, 841, 5, 70, 0, 0, 841, 842, 5, 83, 0, 0, 842, 72, 1, 0, 0, 0, 843, 844, 5, 83, 0, 0, 844, 845, 5, 87, 0, 0, 845, 846, 5, 73, 0, 0, 846, 847, 5, 84, 0, 0, 847, 848, 5, 67, 0, 0, 848, 849, 5, 72, 0, 0, 849, 74, 1, 0, 0, 0, 850, 851, 5, 73, 0, 0, 851, 852, 5, 70, 0, 0, 852, 853, 5, 69, 0, 0, 853, 854, 5, 82, 0, 0, 854, 855, 5, 82, 0, 0, 855, 856, 5, 79, 0, 0, 856, 857, 5, 82, 0, 0, 857, 76, 1, 0, 0, 0, 858, 859, 5, 73, 0, 0, 859, 860, 5, 83, 0, 0, 860, 861, 5, 78, 0, 0, 861, 862, 5, 85, 0, 0, 862, 863, 5, 77, 0, 0, 863, 864, 5, 66, 0, 0, 864, 865, 5, 69, 0, 0, 865, 866, 5, 82, 0, 0, 866, 78, 1, 0, 0, 0, 867, 868, 5, 73, 0, 0, 868, 869, 5, 83, 0, 0, 869, 870, 5, 84, 0, 0, 870, 871, 5, 69, 0, 0, 871, 872, 5, 88, 0, 0, 872, 873, 5, 84, 0, 0, 873, 80, 1, 0, 0, 0, 874, 875, 5, 73, 0, 0, 875, 876, 5, 83, 0, 0, 876, 877, 5, 69, 0, 0, 877, 878, 5, 82, 0, 0, 878, 879, 5, 82, 0, 0, 879, 880, 5, 79, 0, 0, 880, 881, 5, 82, 0, 0, 881, 82, 1, 0, 0, 0, 882, 883, 5, 73, 0, 0, 883, 884, 5, 83, 0, 0, 884, 885, 5, 78, 0, 0, 885, 886, 5, 79, 0, 0, 886, 887, 5, 78, 0, 0, 887, 888, 5, 84, 0, 0, 888, 889, 5, 69, 0, 0, 889, 890, 5, 88, 0, 0, 890, 891, 5, 84, 0, 0, 891, 84, 1, 0, 0, 0, 892, 893, 5, 73, 0, 0, 893, 894, 5, 83, 0, 0, 894, 895, 5, 76, 0, 0, 895, 896, 5, 79, 0, 0, 896, 897, 5, 71, 0, 0, 897, 898, 5, 73, 0, 0, 898, 899, 5, 67, 0, 0, 899, 900, 5, 65, 0, 0, 900, 901, 5, 76, 0, 0, 901, 86, 1, 0, 0, 0, 902, 903, 5, 73, 0, 0, 903, 904, 5, 83, 0, 0, 904, 905, 5, 69, 0, 0, 905, 906, 5, 86, 0, 0, 906, 907, 5, 69, 0, 0, 907, 908, 5, 78, 0, 0, 908, 88, 1, 0, 0, 0, 909, 910, 5, 73, 0, 0, 910, 911, 5, 83, 0, 0, 911, 912, 5, 79, 0, 0, 912, 913, 5, 68, 0, 0, 913, 914, 5, 68, 0, 0, 914, 90, 1, 0, 0, 0, 915, 916, 5, 73, 0, 0, 916, 917, 5, 83, 0, 0, 917, 918, 5, 78, 0, 0, 918, 919, 5, 85, 0, 0, 919, 920, 5, 76, 0, 0, 920, 921, 5, 76, 0, 0, 921, 92, 1, 0, 0, 0, 922, 923, 5, 73, 0, 0, 923, 924, 5, 83, 0, 0, 924, 925, 5, 78, 0, 0, 925, 926, 5, 85, 0, 0, 926, 927, 5, 76, 0, 0, 927, 928, 5, 76, 0, 0, 928, 929, 5, 79, 0, 0, 929, 930, 5, 82, 0, 0, 930, 931, 5, 69, 0, 0, 931, 932, 5, 82, 0, 0, 932, 933, 5, 82, 0, 0, 933, 934, 5, 79, 0, 0, 934, 935, 5, 82, 0, 0, 935, 94, 1, 0, 0, 0, 936, 937, 5, 65, 0, 0, 937, 938, 5, 78, 0, 0, 938, 939, 5, 68, 0, 0, 939, 96, 1, 0, 0, 0, 940, 941, 5, 79, 0, 0, 941, 942, 5, 82, 0, 0, 942, 98, 1, 0, 0, 0, 943, 944, 5, 88, 0, 0, 944, 945, 5, 79, 0, 0, 945, 946, 5, 82, 0, 0, 946, 100, 1, 0, 0, 0, 947, 948, 5, 78, 0, 0, 948, 949, 5, 79, 0, 0, 949, 950, 5, 84, 0, 0, 950, 102, 1, 0, 0, 0, 951, 952, 5, 84, 0, 0, 952, 953, 5, 82, 0, 0, 953, 954, 5, 85, 0, 0, 954, 959, 5, 69, 0, 0, 955, 956, 5, 89, 0, 0, 956, 957, 5, 69, 0, 0, 957, 959, 5, 83, 0, 0, 958, 951, 1, 0, 0, 0, 958, 955, 1, 0, 0, 0, 959, 104, 1, 0, 0, 0, 960, 961, 5, 70, 0, 0, 961, 962, 5, 65, 0, 0, 962, 963, 5, 76, 0, 0, 963, 964, 5, 83, 0, 0, 964, 968, 5, 69, 0, 0, 965, 966, 5, 78, 0, 0, 966, 968, 5, 79, 0, 0, 967, 960, 1, 0, 0, 0, 967, 965, 1, 0, 0, 0, 968, 106, 1, 0, 0, 0, 969, 970, 5, 69, 0, 0, 970, 108, 1, 0, 0, 0, 971, 972, 5, 80, 0, 0, 972, 973, 5, 73, 0, 0, 973, 110, 1, 0, 0, 0, 974, 975, 5, 68, 0, 0, 975, 976, 5, 69, 0, 0, 976, 977, 5, 67, 0, 0, 977, 978, 5, 50, 0, 0, 978, 979, 5, 66, 0, 0, 979, 980, 5, 73, 0, 0, 980, 981, 5, 78, 0, 0, 981, 112, 1, 0, 0, 0, 982, 983, 5, 68, 0, 0, 983, 984, 5, 69, 0, 0, 984, 985, 5, 67, 0, 0, 985, 986, 5, 50, 0, 0, 986, 987, 5, 72, 0, 0, 987, 988, 5, 69, 0, 0, 988, 989, 5, 88, 0, 0, 989, 114, 1, 0, 0, 0, 990, 991, 5, 68, 0, 0, 991, 992, 5, 69, 0, 0, 992, 993, 5, 67, 0, 0, 993, 994, 5, 50, 0, 0, 994, 995, 5, 79, 0, 0, 995, 996, 5, 67, 0, 0, 996, 997, 5, 84, 0, 0, 997, 116, 1, 0, 0, 0, 998, 999, 5, 72, 0, 0, 999, 1000, 5, 69, 0, 0, 1000, 1001, 5, 88, 0, 0, 1001, 1002, 5, 50, 0, 0, 1002, 1003, 5, 66, 0, 0, 1003, 1004, 5, 73, 0, 0, 1004, 1005, 5, 78, 0, 0, 1005, 118, 1, 0, 0, 0, 1006, 1007, 5, 72, 0, 0, 1007, 1008, 5, 69, 0, 0, 1008, 1009, 5, 88, 0, 0, 1009, 1010, 5, 50, 0, 0, 1010, 1011, 5, 68, 0, 0, 1011, 1012, 5, 69, 0, 0, 1012, 1013, 5, 67, 0, 0, 1013, 120, 1, 0, 0, 0, 1014, 1015, 5, 72, 0, 0, 1015, 1016, 5, 69, 0, 0, 1016, 1017, 5, 88, 0, 0, 1017, 1018, 5, 50, 0, 0, 1018, 1019, 5, 79, 0, 0, 1019, 1020, 5, 67, 0, 0, 1020, 1021, 5, 84, 0, 0, 1021, 122, 1, 0, 0, 0, 1022, 1023, 5, 79, 0, 0, 1023, 1024, 5, 67, 0, 0, 1024, 1025, 5, 84, 0, 0, 1025, 1026, 5, 50, 0, 0, 1026, 1027, 5, 66, 0, 0, 1027, 1028, 5, 73, 0, 0, 1028, 1029, 5, 78, 0, 0, 1029, 124, 1, 0, 0, 0, 1030, 1031, 5, 79, 0, 0, 1031, 1032, 5, 67, 0, 0, 1032, 1033, 5, 84, 0, 0, 1033, 1034, 5, 50, 0, 0, 1034, 1035, 5, 68, 0, 0, 1035, 1036, 5, 69, 0, 0, 1036, 1037, 5, 67, 0, 0, 1037, 126, 1, 0, 0, 0, 1038, 1039, 5, 79, 0, 0, 1039, 1040, 5, 67, 0, 0, 1040, 1041, 5, 84, 0, 0, 1041, 1042, 5, 50, 0, 0, 1042, 1043, 5, 72, 0, 0, 1043, 1044, 5, 69, 0, 0, 1044, 1045, 5, 88, 0, 0, 1045, 128, 1, 0, 0, 0, 1046, 1047, 5, 66, 0, 0, 1047, 1048, 5, 73, 0, 0, 1048, 1049, 5, 78, 0, 0, 1049, 1050, 5, 50, 0, 0, 1050, 1051, 5, 79, 0, 0, 1051, 1052, 5, 67, 0, 0, 1052, 1053, 5, 84, 0, 0, 1053, 130, 1, 0, 0, 0, 1054, 1055, 5, 66, 0, 0, 1055, 1056, 5, 73, 0, 0, 1056, 1057, 5, 78, 0, 0, 1057, 1058, 5, 50, 0, 0, 1058, 1059, 5, 68, 0, 0, 1059, 1060, 5, 69, 0, 0, 1060, 1061, 5, 67, 0, 0, 1061, 132, 1, 0, 0, 0, 1062, 1063, 5, 66, 0, 0, 1063, 1064, 5, 73, 0, 0, 1064, 1065, 5, 78, 0, 0, 1065, 1066, 5, 50, 0, 0, 1066, 1067, 5, 72, 0, 0, 1067, 1068, 5, 69, 0, 0, 1068, 1069, 5, 88, 0, 0, 1069, 134, 1, 0, 0, 0, 1070, 1071, 5, 65, 0, 0, 1071, 1072, 5, 66, 0, 0, 1072, 1073, 5, 83, 0, 0, 1073, 136, 1, 0, 0, 0, 1074, 1075, 5, 81, 0, 0, 1075, 1076, 5, 85, 0, 0, 1076, 1077, 5, 79, 0, 0, 1077, 1078, 5, 84, 0, 0, 1078, 1079, 5, 73, 0, 0, 1079, 1080, 5, 69, 0, 0, 1080, 1081, 5, 78, 0, 0, 1081, 1082, 5, 84, 0, 0, 1082, 138, 1, 0, 0, 0, 1083, 1084, 5, 77, 0, 0, 1084, 1085, 5, 79, 0, 0, 1085, 1086, 5, 68, 0, 0, 1086, 140, 1, 0, 0, 0, 1087, 1088, 5, 83, 0, 0, 1088, 1089, 5, 73, 0, 0, 1089, 1090, 5, 71, 0, 0, 1090, 1091, 5, 78, 0, 0, 1091, 142, 1, 0, 0, 0, 1092, 1093, 5, 83, 0, 0, 1093, 1094, 5, 81, 0, 0, 1094, 1095, 5, 82, 0, 0, 1095, 1096, 5, 84, 0, 0, 1096, 144, 1, 0, 0, 0, 1097, 1098, 5, 84, 0, 0, 1098, 1099, 5, 82, 0, 0, 1099, 1100, 5, 85, 0, 0, 1100, 1101, 5, 78, 0, 0, 1101, 1102, 5, 67, 0, 0, 1102, 146, 1, 0, 0, 0, 1103, 1104, 5, 73, 0, 0, 1104, 1105, 5, 78, 0, 0, 1105, 1106, 5, 84, 0, 0, 1106, 148, 1, 0, 0, 0, 1107, 1108, 5, 71, 0, 0, 1108, 1109, 5, 67, 0, 0, 1109, 1110, 5, 68, 0, 0, 1110, 150, 1, 0, 0, 0, 1111, 1112, 5, 76, 0, 0, 1112, 1113, 5, 67, 0, 0, 1113, 1114, 5, 77, 0, 0, 1114, 152, 1, 0, 0, 0, 1115, 1116, 5, 67, 0, 0, 1116, 1117, 5, 79, 0, 0, 1117, 1118, 5, 77, 0, 0, 1118, 1119, 5, 66, 0, 0, 1119, 1120, 5, 73, 0, 0, 1120, 1121, 5, 78, 0, 0, 1121, 154, 1, 0, 0, 0, 1122, 1123, 5, 80, 0, 0, 1123, 1124, 5, 69, 0, 0, 1124, 1125, 5, 82, 0, 0, 1125, 1126, 5, 77, 0, 0, 1126, 1127, 5, 85, 0, 0, 1127, 1128, 5, 84, 0, 0, 1128, 156, 1, 0, 0, 0, 1129, 1130, 5, 68, 0, 0, 1130, 1131, 5, 69, 0, 0, 1131, 1132, 5, 71, 0, 0, 1132, 1133, 5, 82, 0, 0, 1133, 1134, 5, 69, 0, 0, 1134, 1135, 5, 69, 0, 0, 1135, 1136, 5, 83, 0, 0, 1136, 158, 1, 0, 0, 0, 1137, 1138, 5, 82, 0, 0, 1138, 1139, 5, 65, 0, 0, 1139, 1140, 5, 68, 0, 0, 1140, 1141, 5, 73, 0, 0, 1141, 1142, 5, 65, 0, 0, 1142, 1143, 5, 78, 0, 0, 1143, 1144, 5, 83, 0, 0, 1144, 160, 1, 0, 0, 0, 1145, 1146, 5, 67, 0, 0, 1146, 1147, 5, 79, 0, 0, 1147, 1148, 5, 83, 0, 0, 1148, 162, 1, 0, 0, 0, 1149, 1150, 5, 67, 0, 0, 1150, 1151, 5, 79, 0, 0, 1151, 1152, 5, 83, 0, 0, 1152, 1153, 5, 72, 0, 0, 1153, 164, 1, 0, 0, 0, 1154, 1155, 5, 83, 0, 0, 1155, 1156, 5, 73, 0, 0, 1156, 1157, 5, 78, 0, 0, 1157, 166, 1, 0, 0, 0, 1158, 1159, 5, 83, 0, 0, 1159, 1160, 5, 73, 0, 0, 1160, 1161, 5, 78, 0, 0, 1161, 1162, 5, 72, 0, 0, 1162, 168, 1, 0, 0, 0, 1163, 1164, 5, 84, 0, 0, 1164, 1165, 5, 65, 0, 0, 1165, 1166, 5, 78, 0, 0, 1166, 170, 1, 0, 0, 0, 1167, 1168, 5, 84, 0, 0, 1168, 1169, 5, 65, 0, 0, 1169, 1170, 5, 78, 0, 0, 1170, 1171, 5, 72, 0, 0, 1171, 172, 1, 0, 0, 0, 1172, 1173, 5, 67, 0, 0, 1173, 1174, 5, 79, 0, 0, 1174, 1175, 5, 84, 0, 0, 1175, 174, 1, 0, 0, 0, 1176, 1177, 5, 67, 0, 0, 1177, 1178, 5, 79, 0, 0, 1178, 1179, 5, 84, 0, 0, 1179, 1180, 5, 72, 0, 0, 1180, 176, 1, 0, 0, 0, 1181, 1182, 5, 67, 0, 0, 1182, 1183, 5, 83, 0, 0, 1183, 1184, 5, 67, 0, 0, 1184, 178, 1, 0, 0, 0, 1185, 1186, 5, 67, 0, 0, 1186, 1187, 5, 83, 0, 0, 1187, 1188, 5, 67, 0, 0, 1188, 1189, 5, 72, 0, 0, 1189, 180, 1, 0, 0, 0, 1190, 1191, 5, 83, 0, 0, 1191, 1192, 5, 69, 0, 0, 1192, 1193, 5, 67, 0, 0, 1193, 182, 1, 0, 0, 0, 1194, 1195, 5, 83, 0, 0, 1195, 1196, 5, 69, 0, 0, 1196, 1197, 5, 67, 0, 0, 1197, 1198, 5, 72, 0, 0, 1198, 184, 1, 0, 0, 0, 1199, 1200, 5, 65, 0, 0, 1200, 1201, 5, 67, 0, 0, 1201, 1202, 5, 79, 0, 0, 1202, 1203, 5, 83, 0, 0, 1203, 186, 1, 0, 0, 0, 1204, 1205, 5, 65, 0, 0, 1205, 1206, 5, 67, 0, 0, 1206, 1207, 5, 79, 0, 0, 1207, 1208, 5, 83, 0, 0, 1208, 1209, 5, 72, 0, 0, 1209, 188, 1, 0, 0, 0, 1210, 1211, 5, 65, 0, 0, 1211, 1212, 5, 83, 0, 0, 1212, 1213, 5, 73, 0, 0, 1213, 1214, 5, 78, 0, 0, 1214, 190, 1, 0, 0, 0, 1215, 1216, 5, 65, 0, 0, 1216, 1217, 5, 83, 0, 0, 1217, 1218, 5, 73, 0, 0, 1218, 1219, 5, 78, 0, 0, 1219, 1220, 5, 72, 0, 0, 1220, 192, 1, 0, 0, 0, 1221, 1222, 5, 65, 0, 0, 1222, 1223, 5, 84, 0, 0, 1223, 1224, 5, 65, 0, 0, 1224, 1225, 5, 78, 0, 0, 1225, 194, 1, 0, 0, 0, 1226, 1227, 5, 65, 0, 0, 1227, 1228, 5, 84, 0, 0, 1228, 1229, 5, 65, 0, 0, 1229, 1230, 5, 78, 0, 0, 1230, 1231, 5, 72, 0, 0, 1231, 196, 1, 0, 0, 0, 1232, 1233, 5, 65, 0, 0, 1233, 1234, 5, 67, 0, 0, 1234, 1235, 5, 79, 0, 0, 1235, 1236, 5, 84, 0, 0, 1236, 198, 1, 0, 0, 0, 1237, 1238, 5, 65, 0, 0, 1238, 1239, 5, 67, 0, 0, 1239, 1240, 5, 79, 0, 0, 1240, 1241, 5, 84, 0, 0, 1241, 1242, 5, 72, 0, 0, 1242, 200, 1, 0, 0, 0, 1243, 1244, 5, 65, 0, 0, 1244, 1245, 5, 84, 0, 0, 1245, 1246, 5, 65, 0, 0, 1246, 1247, 5, 78, 0, 0, 1247, 1248, 5, 50, 0, 0, 1248, 202, 1, 0, 0, 0, 1249, 1250, 5, 82, 0, 0, 1250, 1251, 5, 79, 0, 0, 1251, 1252, 5, 85, 0, 0, 1252, 1253, 5, 78, 0, 0, 1253, 1254, 5, 68, 0, 0, 1254, 204, 1, 0, 0, 0, 1255, 1256, 5, 82, 0, 0, 1256, 1257, 5, 79, 0, 0, 1257, 1258, 5, 85, 0, 0, 1258, 1259, 5, 78, 0, 0, 1259, 1260, 5, 68, 0, 0, 1260, 1261, 5, 68, 0, 0, 1261, 1262, 5, 79, 0, 0, 1262, 1263, 5, 87, 0, 0, 1263, 1264, 5, 78, 0, 0, 1264, 206, 1, 0, 0, 0, 1265, 1266, 5, 82, 0, 0, 1266, 1267, 5, 79, 0, 0, 1267, 1268, 5, 85, 0, 0, 1268, 1269, 5, 78, 0, 0, 1269, 1270, 5, 68, 0, 0, 1270, 1271, 5, 85, 0, 0, 1271, 1272, 5, 80, 0, 0, 1272, 208, 1, 0, 0, 0, 1273, 1274, 5, 67, 0, 0, 1274, 1275, 5, 69, 0, 0, 1275, 1276, 5, 73, 0, 0, 1276, 1277, 5, 76, 0, 0, 1277, 1278, 5, 73, 0, 0, 1278, 1279, 5, 78, 0, 0, 1279, 1280, 5, 71, 0, 0, 1280, 210, 1, 0, 0, 0, 1281, 1282, 5, 70, 0, 0, 1282, 1283, 5, 76, 0, 0, 1283, 1284, 5, 79, 0, 0, 1284, 1285, 5, 79, 0, 0, 1285, 1286, 5, 82, 0, 0, 1286, 212, 1, 0, 0, 0, 1287, 1288, 5, 69, 0, 0, 1288, 1289, 5, 86, 0, 0, 1289, 1290, 5, 69, 0, 0, 1290, 1291, 5, 78, 0, 0, 1291, 214, 1, 0, 0, 0, 1292, 1293, 5, 79, 0, 0, 1293, 1294, 5, 68, 0, 0, 1294, 1295, 5, 68, 0, 0, 1295, 216, 1, 0, 0, 0, 1296, 1297, 5, 77, 0, 0, 1297, 1298, 5, 82, 0, 0, 1298, 1299, 5, 79, 0, 0, 1299, 1300, 5, 85, 0, 0, 1300, 1301, 5, 78, 0, 0, 1301, 1302, 5, 68, 0, 0, 1302, 218, 1, 0, 0, 0, 1303, 1304, 5, 82, 0, 0, 1304, 1305, 5, 65, 0, 0, 1305, 1306, 5, 78, 0, 0, 1306, 1307, 5, 68, 0, 0, 1307, 220, 1, 0, 0, 0, 1308, 1309, 5, 82, 0, 0, 1309, 1310, 5, 65, 0, 0, 1310, 1311, 5, 78, 0, 0, 1311, 1312, 5, 68, 0, 0, 1312, 1313, 5, 66, 0, 0, 1313, 1314, 5, 69, 0, 0, 1314, 1315, 5, 84, 0, 0, 1315, 1316, 5, 87, 0, 0, 1316, 1317, 5, 69, 0, 0, 1317, 1318, 5, 69, 0, 0, 1318, 1319, 5, 78, 0, 0, 1319, 222, 1, 0, 0, 0, 1320, 1321, 5, 70, 0, 0, 1321, 1322, 5, 65, 0, 0, 1322, 1323, 5, 67, 0, 0, 1323, 1324, 5, 84, 0, 0, 1324, 224, 1, 0, 0, 0, 1325, 1326, 5, 70, 0, 0, 1326, 1327, 5, 65, 0, 0, 1327, 1328, 5, 67, 0, 0, 1328, 1329, 5, 84, 0, 0, 1329, 1330, 5, 68, 0, 0, 1330, 1331, 5, 79, 0, 0, 1331, 1332, 5, 85, 0, 0, 1332, 1333, 5, 66, 0, 0, 1333, 1334, 5, 76, 0, 0, 1334, 1335, 5, 69, 0, 0, 1335, 226, 1, 0, 0, 0, 1336, 1337, 5, 80, 0, 0, 1337, 1338, 5, 79, 0, 0, 1338, 1339, 5, 87, 0, 0, 1339, 1340, 5, 69, 0, 0, 1340, 1341, 5, 82, 0, 0, 1341, 228, 1, 0, 0, 0, 1342, 1343, 5, 69, 0, 0, 1343, 1344, 5, 88, 0, 0, 1344, 1345, 5, 80, 0, 0, 1345, 230, 1, 0, 0, 0, 1346, 1347, 5, 76, 0, 0, 1347, 1348, 5, 78, 0, 0, 1348, 232, 1, 0, 0, 0, 1349, 1350, 5, 76, 0, 0, 1350, 1351, 5, 79, 0, 0, 1351, 1352, 5, 71, 0, 0, 1352, 234, 1, 0, 0, 0, 1353, 1354, 5, 76, 0, 0, 1354, 1355, 5, 79, 0, 0, 1355, 1356, 5, 71, 0, 0, 1356, 1357, 5, 49, 0, 0, 1357, 1358, 5, 48, 0, 0, 1358, 236, 1, 0, 0, 0, 1359, 1360, 5, 77, 0, 0, 1360, 1361, 5, 85, 0, 0, 1361, 1362, 5, 76, 0, 0, 1362, 1363, 5, 84, 0, 0, 1363, 1364, 5, 73, 0, 0, 1364, 1365, 5, 78, 0, 0, 1365, 1366, 5, 79, 0, 0, 1366, 1367, 5, 77, 0, 0, 1367, 1368, 5, 73, 0, 0, 1368, 1369, 5, 65, 0, 0, 1369, 1370, 5, 76, 0, 0, 1370, 238, 1, 0, 0, 0, 1371, 1372, 5, 80, 0, 0, 1372, 1373, 5, 82, 0, 0, 1373, 1374, 5, 79, 0, 0, 1374, 1375, 5, 68, 0, 0, 1375, 1376, 5, 85, 0, 0, 1376, 1377, 5, 67, 0, 0, 1377, 1378, 5, 84, 0, 0, 1378, 240, 1, 0, 0, 0, 1379, 1380, 5, 83, 0, 0, 1380, 1381, 5, 81, 0, 0, 1381, 1382, 5, 82, 0, 0, 1382, 1383, 5, 84, 0, 0, 1383, 1384, 5, 80, 0, 0, 1384, 1385, 5, 73, 0, 0, 1385, 242, 1, 0, 0, 0, 1386, 1387, 5, 69, 0, 0, 1387, 1388, 5, 82, 0, 0, 1388, 1389, 5, 70, 0, 0, 1389, 244, 1, 0, 0, 0, 1390, 1391, 5, 69, 0, 0, 1391, 1392, 5, 82, 0, 0, 1392, 1393, 5, 70, 0, 0, 1393, 1394, 5, 67, 0, 0, 1394, 246, 1, 0, 0, 0, 1395, 1396, 5, 66, 0, 0, 1396, 1397, 5, 69, 0, 0, 1397, 1398, 5, 83, 0, 0, 1398, 1399, 5, 83, 0, 0, 1399, 1400, 5, 69, 0, 0, 1400, 1401, 5, 76, 0, 0, 1401, 1402, 5, 73, 0, 0, 1402, 248, 1, 0, 0, 0, 1403, 1404, 5, 66, 0, 0, 1404, 1405, 5, 69, 0, 0, 1405, 1406, 5, 83, 0, 0, 1406, 1407, 5, 83, 0, 0, 1407, 1408, 5, 69, 0, 0, 1408, 1409, 5, 76, 0, 0, 1409, 1410, 5, 74, 0, 0, 1410, 250, 1, 0, 0, 0, 1411, 1412, 5, 66, 0, 0, 1412, 1413, 5, 69, 0, 0, 1413, 1414, 5, 83, 0, 0, 1414, 1415, 5, 83, 0, 0, 1415, 1416, 5, 69, 0, 0, 1416, 1417, 5, 76, 0, 0, 1417, 1418, 5, 75, 0, 0, 1418, 252, 1, 0, 0, 0, 1419, 1420, 5, 66, 0, 0, 1420, 1421, 5, 69, 0, 0, 1421, 1422, 5, 83, 0, 0, 1422, 1423, 5, 83, 0, 0, 1423, 1424, 5, 69, 0, 0, 1424, 1425, 5, 76, 0, 0, 1425, 1426, 5, 89, 0, 0, 1426, 254, 1, 0, 0, 0, 1427, 1428, 5, 68, 0, 0, 1428, 1429, 5, 69, 0, 0, 1429, 1430, 5, 76, 0, 0, 1430, 1431, 5, 84, 0, 0, 1431, 1432, 5, 65, 0, 0, 1432, 256, 1, 0, 0, 0, 1433, 1434, 5, 71, 0, 0, 1434, 1435, 5, 69, 0, 0, 1435, 1436, 5, 83, 0, 0, 1436, 1437, 5, 84, 0, 0, 1437, 1438, 5, 69, 0, 0, 1438, 1439, 5, 80, 0, 0, 1439, 258, 1, 0, 0, 0, 1440, 1441, 5, 83, 0, 0, 1441, 1442, 5, 85, 0, 0, 1442, 1443, 5, 77, 0, 0, 1443, 1444, 5, 83, 0, 0, 1444, 1445, 5, 81, 0, 0, 1445, 260, 1, 0, 0, 0, 1446, 1447, 5, 83, 0, 0, 1447, 1448, 5, 85, 0, 0, 1448, 1449, 5, 77, 0, 0, 1449, 1450, 5, 80, 0, 0, 1450, 1451, 5, 82, 0, 0, 1451, 1452, 5, 79, 0, 0, 1452, 1453, 5, 68, 0, 0, 1453, 1454, 5, 85, 0, 0, 1454, 1455, 5, 67, 0, 0, 1455, 1456, 5, 84, 0, 0, 1456, 262, 1, 0, 0, 0, 1457, 1458, 5, 83, 0, 0, 1458, 1459, 5, 85, 0, 0, 1459, 1460, 5, 77, 0, 0, 1460, 1461, 5, 88, 0, 0, 1461, 1462, 5, 50, 0, 0, 1462, 1463, 5, 77, 0, 0, 1463, 1464, 5, 89, 0, 0, 1464, 1465, 5, 50, 0, 0, 1465, 264, 1, 0, 0, 0, 1466, 1467, 5, 83, 0, 0, 1467, 1468, 5, 85, 0, 0, 1468, 1469, 5, 77, 0, 0, 1469, 1470, 5, 88, 0, 0, 1470, 1471, 5, 50, 0, 0, 1471, 1472, 5, 80, 0, 0, 1472, 1473, 5, 89, 0, 0, 1473, 1474, 5, 50, 0, 0, 1474, 266, 1, 0, 0, 0, 1475, 1476, 5, 83, 0, 0, 1476, 1477, 5, 85, 0, 0, 1477, 1478, 5, 77, 0, 0, 1478, 1479, 5, 88, 0, 0, 1479, 1480, 5, 77, 0, 0, 1480, 1481, 5, 89, 0, 0, 1481, 1482, 5, 50, 0, 0, 1482, 268, 1, 0, 0, 0, 1483, 1484, 5, 65, 0, 0, 1484, 1485, 5, 82, 0, 0, 1485, 1486, 5, 65, 0, 0, 1486, 1487, 5, 66, 0, 0, 1487, 1488, 5, 73, 0, 0, 1488, 1489, 5, 67, 0, 0, 1489, 270, 1, 0, 0, 0, 1490, 1491, 5, 82, 0, 0, 1491, 1492, 5, 79, 0, 0, 1492, 1493, 5, 77, 0, 0, 1493, 1494, 5, 65, 0, 0, 1494, 1495, 5, 78, 0, 0, 1495, 272, 1, 0, 0, 0, 1496, 1497, 5, 83, 0, 0, 1497, 1498, 5, 69, 0, 0, 1498, 1499, 5, 82, 0, 0, 1499, 1500, 5, 73, 0, 0, 1500, 1501, 5, 69, 0, 0, 1501, 1502, 5, 83, 0, 0, 1502, 1503, 5, 83, 0, 0, 1503, 1504, 5, 85, 0, 0, 1504, 1505, 5, 77, 0, 0, 1505, 274, 1, 0, 0, 0, 1506, 1507, 5, 82, 0, 0, 1507, 1508, 5, 65, 0, 0, 1508, 1509, 5, 78, 0, 0, 1509, 1510, 5, 75, 0, 0, 1510, 276, 1, 0, 0, 0, 1511, 1512, 5, 70, 0, 0, 1512, 1513, 5, 79, 0, 0, 1513, 1514, 5, 82, 0, 0, 1514, 1515, 5, 69, 0, 0, 1515, 1516, 5, 67, 0, 0, 1516, 1517, 5, 65, 0, 0, 1517, 1518, 5, 83, 0, 0, 1518, 1519, 5, 84, 0, 0, 1519, 278, 1, 0, 0, 0, 1520, 1521, 5, 73, 0, 0, 1521, 1522, 5, 78, 0, 0, 1522, 1523, 5, 84, 0, 0, 1523, 1524, 5, 69, 0, 0, 1524, 1525, 5, 82, 0, 0, 1525, 1526, 5, 67, 0, 0, 1526, 1527, 5, 69, 0, 0, 1527, 1528, 5, 80, 0, 0, 1528, 1529, 5, 84, 0, 0, 1529, 280, 1, 0, 0, 0, 1530, 1531, 5, 83, 0, 0, 1531, 1532, 5, 76, 0, 0, 1532, 1533, 5, 79, 0, 0, 1533, 1534, 5, 80, 0, 0, 1534, 1535, 5, 69, 0, 0, 1535, 282, 1, 0, 0, 0, 1536, 1537, 5, 67, 0, 0, 1537, 1538, 5, 79, 0, 0, 1538, 1539, 5, 82, 0, 0, 1539, 1540, 5, 82, 0, 0, 1540, 1541, 5, 69, 0, 0, 1541, 1542, 5, 76, 0, 0, 1542, 284, 1, 0, 0, 0, 1543, 1544, 5, 80, 0, 0, 1544, 1545, 5, 69, 0, 0, 1545, 1546, 5, 65, 0, 0, 1546, 1547, 5, 82, 0, 0, 1547, 1548, 5, 83, 0, 0, 1548, 1549, 5, 79, 0, 0, 1549, 1550, 5, 78, 0, 0, 1550, 286, 1, 0, 0, 0, 1551, 1552, 5, 89, 0, 0, 1552, 1553, 5, 69, 0, 0, 1553, 1554, 5, 65, 0, 0, 1554, 1555, 5, 82, 0, 0, 1555, 1556, 5, 70, 0, 0, 1556, 1557, 5, 82, 0, 0, 1557, 1558, 5, 65, 0, 0, 1558, 1559, 5, 67, 0, 0, 1559, 288, 1, 0, 0, 0, 1560, 1561, 5, 65, 0, 0, 1561, 1562, 5, 83, 0, 0, 1562, 1563, 5, 67, 0, 0, 1563, 290, 1, 0, 0, 0, 1564, 1565, 5, 74, 0, 0, 1565, 1566, 5, 73, 0, 0, 1566, 1576, 5, 83, 0, 0, 1567, 1568, 5, 87, 0, 0, 1568, 1569, 5, 73, 0, 0, 1569, 1570, 5, 68, 0, 0, 1570, 1571, 5, 69, 0, 0, 1571, 1572, 5, 67, 0, 0, 1572, 1573, 5, 72, 0, 0, 1573, 1574, 5, 65, 0, 0, 1574, 1576, 5, 82, 0, 0, 1575, 1564, 1, 0, 0, 0, 1575, 1567, 1, 0, 0, 0, 1576, 292, 1, 0, 0, 0, 1577, 1578, 5, 67, 0, 0, 1578, 1579, 5, 72, 0, 0, 1579, 1580, 5, 65, 0, 0, 1580, 1581, 5, 82, 0, 0, 1581, 294, 1, 0, 0, 0, 1582, 1583, 5, 67, 0, 0, 1583, 1584, 5, 76, 0, 0, 1584, 1585, 5, 69, 0, 0, 1585, 1586, 5, 65, 0, 0, 1586, 1587, 5, 78, 0, 0, 1587, 296, 1, 0, 0, 0, 1588, 1589, 5, 67, 0, 0, 1589, 1590, 5, 79, 0, 0, 1590, 1591, 5, 68, 0, 0, 1591, 1592, 5, 69, 0, 0, 1592, 298, 1, 0, 0, 0, 1593, 1594, 5, 85, 0, 0, 1594, 1595, 5, 78, 0, 0, 1595, 1596, 5, 73, 0, 0, 1596, 1597, 5, 67, 0, 0, 1597, 1598, 5, 72, 0, 0, 1598, 1599, 5, 65, 0, 0, 1599, 1600, 5, 82, 0, 0, 1600, 300, 1, 0, 0, 0, 1601, 1602, 5, 85, 0, 0, 1602, 1603, 5, 78, 0, 0, 1603, 1604, 5, 73, 0, 0, 1604, 1605, 5, 67, 0, 0, 1605, 1606, 5, 79, 0, 0, 1606, 1607, 5, 68, 0, 0, 1607, 1608, 5, 69, 0, 0, 1608, 302, 1, 0, 0, 0, 1609, 1610, 5, 67, 0, 0, 1610, 1611, 5, 79, 0, 0, 1611, 1612, 5, 78, 0, 0, 1612, 1613, 5, 67, 0, 0, 1613, 1614, 5, 65, 0, 0, 1614, 1615, 5, 84, 0, 0, 1615, 1616, 5, 69, 0, 0, 1616, 1617, 5, 78, 0, 0, 1617, 1618, 5, 65, 0, 0, 1618, 1619, 5, 84, 0, 0, 1619, 1627, 5, 69, 0, 0, 1620, 1621, 5, 67, 0, 0, 1621, 1622, 5, 79, 0, 0, 1622, 1623, 5, 78, 0, 0, 1623, 1624, 5, 67, 0, 0, 1624, 1625, 5, 65, 0, 0, 1625, 1627, 5, 84, 0, 0, 1626, 1609, 1, 0, 0, 0, 1626, 1620, 1, 0, 0, 0, 1627, 304, 1, 0, 0, 0, 1628, 1629, 5, 69, 0, 0, 1629, 1630, 5, 88, 0, 0, 1630, 1631, 5, 65, 0, 0, 1631, 1632, 5, 67, 0, 0, 1632, 1633, 5, 84, 0, 0, 1633, 306, 1, 0, 0, 0, 1634, 1635, 5, 70, 0, 0, 1635, 1636, 5, 73, 0, 0, 1636, 1637, 5, 78, 0, 0, 1637, 1638, 5, 68, 0, 0, 1638, 308, 1, 0, 0, 0, 1639, 1640, 5, 70, 0, 0, 1640, 1641, 5, 73, 0, 0, 1641, 1642, 5, 88, 0, 0, 1642, 1643, 5, 69, 0, 0, 1643, 1644, 5, 68, 0, 0, 1644, 310, 1, 0, 0, 0, 1645, 1646, 5, 76, 0, 0, 1646, 1647, 5, 69, 0, 0, 1647, 1648, 5, 70, 0, 0, 1648, 1649, 5, 84, 0, 0, 1649, 312, 1, 0, 0, 0, 1650, 1651, 5, 76, 0, 0, 1651, 1652, 5, 69, 0, 0, 1652, 1653, 5, 78, 0, 0, 1653, 314, 1, 0, 0, 0, 1654, 1655, 5, 76, 0, 0, 1655, 1656, 5, 79, 0, 0, 1656, 1657, 5, 87, 0, 0, 1657, 1658, 5, 69, 0, 0, 1658, 1667, 5, 82, 0, 0, 1659, 1660, 5, 84, 0, 0, 1660, 1661, 5, 79, 0, 0, 1661, 1662, 5, 76, 0, 0, 1662, 1663, 5, 79, 0, 0, 1663, 1664, 5, 87, 0, 0, 1664, 1665, 5, 69, 0, 0, 1665, 1667, 5, 82, 0, 0, 1666, 1654, 1, 0, 0, 0, 1666, 1659, 1, 0, 0, 0, 1667, 316, 1, 0, 0, 0, 1668, 1669, 5, 77, 0, 0, 1669, 1670, 5, 73, 0, 0, 1670, 1671, 5, 68, 0, 0, 1671, 318, 1, 0, 0, 0, 1672, 1673, 5, 80, 0, 0, 1673, 1674, 5, 82, 0, 0, 1674, 1675, 5, 79, 0, 0, 1675, 1676, 5, 80, 0, 0, 1676, 1677, 5, 69, 0, 0, 1677, 1678, 5, 82, 0, 0, 1678, 320, 1, 0, 0, 0, 1679, 1680, 5, 82, 0, 0, 1680, 1681, 5, 69, 0, 0, 1681, 1682, 5, 80, 0, 0, 1682, 1683, 5, 76, 0, 0, 1683, 1684, 5, 65, 0, 0, 1684, 1685, 5, 67, 0, 0, 1685, 1686, 5, 69, 0, 0, 1686, 322, 1, 0, 0, 0, 1687, 1688, 5, 82, 0, 0, 1688, 1689, 5, 69, 0, 0, 1689, 1690, 5, 80, 0, 0, 1690, 1691, 5, 84, 0, 0, 1691, 324, 1, 0, 0, 0, 1692, 1693, 5, 82, 0, 0, 1693, 1694, 5, 73, 0, 0, 1694, 1695, 5, 71, 0, 0, 1695, 1696, 5, 72, 0, 0, 1696, 1697, 5, 84, 0, 0, 1697, 326, 1, 0, 0, 0, 1698, 1699, 5, 82, 0, 0, 1699, 1700, 5, 77, 0, 0, 1700, 1701, 5, 66, 0, 0, 1701, 328, 1, 0, 0, 0, 1702, 1703, 5, 83, 0, 0, 1703, 1704, 5, 69, 0, 0, 1704, 1705, 5, 65, 0, 0, 1705, 1706, 5, 82, 0, 0, 1706, 1707, 5, 67, 0, 0, 1707, 1708, 5, 72, 0, 0, 1708, 330, 1, 0, 0, 0, 1709, 1710, 5, 83, 0, 0, 1710, 1711, 5, 85, 0, 0, 1711, 1712, 5, 66, 0, 0, 1712, 1713, 5, 83, 0, 0, 1713, 1714, 5, 84, 0, 0, 1714, 1715, 5, 73, 0, 0, 1715, 1716, 5, 84, 0, 0, 1716, 1717, 5, 85, 0, 0, 1717, 1718, 5, 84, 0, 0, 1718, 1719, 5, 69, 0, 0, 1719, 332, 1, 0, 0, 0, 1720, 1721, 5, 84, 0, 0, 1721, 334, 1, 0, 0, 0, 1722, 1723, 5, 84, 0, 0, 1723, 1724, 5, 69, 0, 0, 1724, 1725, 5, 88, 0, 0, 1725, 1726, 5, 84, 0, 0, 1726, 336, 1, 0, 0, 0, 1727, 1728, 5, 84, 0, 0, 1728, 1729, 5, 82, 0, 0, 1729, 1730, 5, 73, 0, 0, 1730, 1731, 5, 77, 0, 0, 1731, 338, 1, 0, 0, 0, 1732, 1733, 5, 85, 0, 0, 1733, 1734, 5, 80, 0, 0, 1734, 1735, 5, 80, 0, 0, 1735, 1736, 5, 69, 0, 0, 1736, 1745, 5, 82, 0, 0, 1737, 1738, 5, 84, 0, 0, 1738, 1739, 5, 79, 0, 0, 1739, 1740, 5, 85, 0, 0, 1740, 1741, 5, 80, 0, 0, 1741, 1742, 5, 80, 0, 0, 1742, 1743, 5, 69, 0, 0, 1743, 1745, 5, 82, 0, 0, 1744, 1732, 1, 0, 0, 0, 1744, 1737, 1, 0, 0, 0, 1745, 340, 1, 0, 0, 0, 1746, 1747, 5, 86, 0, 0, 1747, 1748, 5, 65, 0, 0, 1748, 1749, 5, 76, 0, 0, 1749, 1750, 5, 85, 0, 0, 1750, 1751, 5, 69, 0, 0, 1751, 342, 1, 0, 0, 0, 1752, 1753, 5, 68, 0, 0, 1753, 1754, 5, 65, 0, 0, 1754, 1755, 5, 84, 0, 0, 1755, 1756, 5, 69, 0, 0, 1756, 1757, 5, 86, 0, 0, 1757, 1758, 5, 65, 0, 0, 1758, 1759, 5, 76, 0, 0, 1759, 1760, 5, 85, 0, 0, 1760, 1761, 5, 69, 0, 0, 1761, 344, 1, 0, 0, 0, 1762, 1763, 5, 84, 0, 0, 1763, 1764, 5, 73, 0, 0, 1764, 1765, 5, 77, 0, 0, 1765, 1766, 5, 69, 0, 0, 1766, 1767, 5, 86, 0, 0, 1767, 1768, 5, 65, 0, 0, 1768, 1769, 5, 76, 0, 0, 1769, 1770, 5, 85, 0, 0, 1770, 1771, 5, 69, 0, 0, 1771, 346, 1, 0, 0, 0, 1772, 1773, 5, 68, 0, 0, 1773, 1774, 5, 65, 0, 0, 1774, 1775, 5, 84, 0, 0, 1775, 1776, 5, 69, 0, 0, 1776, 348, 1, 0, 0, 0, 1777, 1778, 5, 84, 0, 0, 1778, 1779, 5, 73, 0, 0, 1779, 1780, 5, 77, 0, 0, 1780, 1781, 5, 69, 0, 0, 1781, 350, 1, 0, 0, 0, 1782, 1783, 5, 78, 0, 0, 1783, 1784, 5, 79, 0, 0, 1784, 1785, 5, 87, 0, 0, 1785, 352, 1, 0, 0, 0, 1786, 1787, 5, 84, 0, 0, 1787, 1788, 5, 79, 0, 0, 1788, 1789, 5, 68, 0, 0, 1789, 1790, 5, 65, 0, 0, 1790, 1791, 5, 89, 0, 0, 1791, 354, 1, 0, 0, 0, 1792, 1793, 5, 89, 0, 0, 1793, 1794, 5, 69, 0, 0, 1794, 1795, 5, 65, 0, 0, 1795, 1796, 5, 82, 0, 0, 1796, 356, 1, 0, 0, 0, 1797, 1798, 5, 77, 0, 0, 1798, 1799, 5, 79, 0, 0, 1799, 1800, 5, 78, 0, 0, 1800, 1801, 5, 84, 0, 0, 1801, 1802, 5, 72, 0, 0, 1802, 358, 1, 0, 0, 0, 1803, 1804, 5, 68, 0, 0, 1804, 1805, 5, 65, 0, 0, 1805, 1806, 5, 89, 0, 0, 1806, 360, 1, 0, 0, 0, 1807, 1808, 5, 72, 0, 0, 1808, 1809, 5, 79, 0, 0, 1809, 1810, 5, 85, 0, 0, 1810, 1811, 5, 82, 0, 0, 1811, 362, 1, 0, 0, 0, 1812, 1813, 5, 77, 0, 0, 1813, 1814, 5, 73, 0, 0, 1814, 1815, 5, 78, 0, 0, 1815, 1816, 5, 85, 0, 0, 1816, 1817, 5, 84, 0, 0, 1817, 1818, 5, 69, 0, 0, 1818, 364, 1, 0, 0, 0, 1819, 1820, 5, 83, 0, 0, 1820, 1821, 5, 69, 0, 0, 1821, 1822, 5, 67, 0, 0, 1822, 1823, 5, 79, 0, 0, 1823, 1824, 5, 78, 0, 0, 1824, 1825, 5, 68, 0, 0, 1825, 366, 1, 0, 0, 0, 1826, 1827, 5, 87, 0, 0, 1827, 1828, 5, 69, 0, 0, 1828, 1829, 5, 69, 0, 0, 1829, 1830, 5, 75, 0, 0, 1830, 1831, 5, 68, 0, 0, 1831, 1832, 5, 65, 0, 0, 1832, 1833, 5, 89, 0, 0, 1833, 368, 1, 0, 0, 0, 1834, 1835, 5, 68, 0, 0, 1835, 1836, 5, 65, 0, 0, 1836, 1837, 5, 84, 0, 0, 1837, 1838, 5, 69, 0, 0, 1838, 1839, 5, 68, 0, 0, 1839, 1840, 5, 73, 0, 0, 1840, 1841, 5, 70, 0, 0, 1841, 370, 1, 0, 0, 0, 1842, 1843, 5, 68, 0, 0, 1843, 1844, 5, 65, 0, 0, 1844, 1845, 5, 89, 0, 0, 1845, 1846, 5, 83, 0, 0, 1846, 372, 1, 0, 0, 0, 1847, 1848, 5, 68, 0, 0, 1848, 1849, 5, 65, 0, 0, 1849, 1850, 5, 89, 0, 0, 1850, 1851, 5, 83, 0, 0, 1851, 1852, 5, 51, 0, 0, 1852, 1853, 5, 54, 0, 0, 1853, 1854, 5, 48, 0, 0, 1854, 374, 1, 0, 0, 0, 1855, 1856, 5, 69, 0, 0, 1856, 1857, 5, 68, 0, 0, 1857, 1858, 5, 65, 0, 0, 1858, 1859, 5, 84, 0, 0, 1859, 1860, 5, 69, 0, 0, 1860, 376, 1, 0, 0, 0, 1861, 1862, 5, 69, 0, 0, 1862, 1863, 5, 79, 0, 0, 1863, 1864, 5, 77, 0, 0, 1864, 1865, 5, 79, 0, 0, 1865, 1866, 5, 78, 0, 0, 1866, 1867, 5, 84, 0, 0, 1867, 1868, 5, 72, 0, 0, 1868, 378, 1, 0, 0, 0, 1869, 1870, 5, 78, 0, 0, 1870, 1871, 5, 69, 0, 0, 1871, 1872, 5, 84, 0, 0, 1872, 1873, 5, 87, 0, 0, 1873, 1874, 5, 79, 0, 0, 1874, 1875, 5, 82, 0, 0, 1875, 1876, 5, 75, 0, 0, 1876, 1877, 5, 68, 0, 0, 1877, 1878, 5, 65, 0, 0, 1878, 1879, 5, 89, 0, 0, 1879, 1880, 5, 83, 0, 0, 1880, 380, 1, 0, 0, 0, 1881, 1882, 5, 87, 0, 0, 1882, 1883, 5, 79, 0, 0, 1883, 1884, 5, 82, 0, 0, 1884, 1885, 5, 75, 0, 0, 1885, 1886, 5, 68, 0, 0, 1886, 1887, 5, 65, 0, 0, 1887, 1888, 5, 89, 0, 0, 1888, 382, 1, 0, 0, 0, 1889, 1890, 5, 87, 0, 0, 1890, 1891, 5, 69, 0, 0, 1891, 1892, 5, 69, 0, 0, 1892, 1893, 5, 75, 0, 0, 1893, 1894, 5, 78, 0, 0, 1894, 1895, 5, 85, 0, 0, 1895, 1896, 5, 77, 0, 0, 1896, 384, 1, 0, 0, 0, 1897, 1898, 5, 77, 0, 0, 1898, 1899, 5, 65, 0, 0, 1899, 1900, 5, 88, 0, 0, 1900, 386, 1, 0, 0, 0, 1901, 1902, 5, 77, 0, 0, 1902, 1903, 5, 69, 0, 0, 1903, 1904, 5, 68, 0, 0, 1904, 1905, 5, 73, 0, 0, 1905, 1906, 5, 65, 0, 0, 1906, 1907, 5, 78, 0, 0, 1907, 388, 1, 0, 0, 0, 1908, 1909, 5, 77, 0, 0, 1909, 1910, 5, 73, 0, 0, 1910, 1911, 5, 78, 0, 0, 1911, 390, 1, 0, 0, 0, 1912, 1913, 5, 81, 0, 0, 1913, 1914, 5, 85, 0, 0, 1914, 1915, 5, 65, 0, 0, 1915, 1916, 5, 82, 0, 0, 1916, 1917, 5, 84, 0, 0, 1917, 1918, 5, 73, 0, 0, 1918, 1919, 5, 76, 0, 0, 1919, 1920, 5, 69, 0, 0, 1920, 392, 1, 0, 0, 0, 1921, 1922, 5, 77, 0, 0, 1922, 1923, 5, 79, 0, 0, 1923, 1924, 5, 68, 0, 0, 1924, 1925, 5, 69, 0, 0, 1925, 394, 1, 0, 0, 0, 1926, 1927, 5, 76, 0, 0, 1927, 1928, 5, 65, 0, 0, 1928, 1929, 5, 82, 0, 0, 1929, 1930, 5, 71, 0, 0, 1930, 1931, 5, 69, 0, 0, 1931, 396, 1, 0, 0, 0, 1932, 1933, 5, 83, 0, 0, 1933, 1934, 5, 77, 0, 0, 1934, 1935, 5, 65, 0, 0, 1935, 1936, 5, 76, 0, 0, 1936, 1937, 5, 76, 0, 0, 1937, 398, 1, 0, 0, 0, 1938, 1939, 5, 80, 0, 0, 1939, 1940, 5, 69, 0, 0, 1940, 1941, 5, 82, 0, 0, 1941, 1942, 5, 67, 0, 0, 1942, 1943, 5, 69, 0, 0, 1943, 1944, 5, 78, 0, 0, 1944, 1945, 5, 84, 0, 0, 1945, 1946, 5, 73, 0, 0, 1946, 1947, 5, 76, 0, 0, 1947, 1963, 5, 69, 0, 0, 1948, 1949, 5, 80, 0, 0, 1949, 1950, 5, 69, 0, 0, 1950, 1951, 5, 82, 0, 0, 1951, 1952, 5, 67, 0, 0, 1952, 1953, 5, 69, 0, 0, 1953, 1954, 5, 78, 0, 0, 1954, 1955, 5, 84, 0, 0, 1955, 1956, 5, 73, 0, 0, 1956, 1957, 5, 76, 0, 0, 1957, 1958, 5, 69, 0, 0, 1958, 1959, 5, 46, 0, 0, 1959, 1960, 5, 73, 0, 0, 1960, 1961, 5, 78, 0, 0, 1961, 1963, 5, 67, 0, 0, 1962, 1938, 1, 0, 0, 0, 1962, 1948, 1, 0, 0, 0, 1963, 400, 1, 0, 0, 0, 1964, 1965, 5, 80, 0, 0, 1965, 1966, 5, 69, 0, 0, 1966, 1967, 5, 82, 0, 0, 1967, 1968, 5, 67, 0, 0, 1968, 1969, 5, 69, 0, 0, 1969, 1970, 5, 78, 0, 0, 1970, 1971, 5, 84, 0, 0, 1971, 1972, 5, 82, 0, 0, 1972, 1973, 5, 65, 0, 0, 1973, 1974, 5, 78, 0, 0, 1974, 1991, 5, 75, 0, 0, 1975, 1976, 5, 80, 0, 0, 1976, 1977, 5, 69, 0, 0, 1977, 1978, 5, 82, 0, 0, 1978, 1979, 5, 67, 0, 0, 1979, 1980, 5, 69, 0, 0, 1980, 1981, 5, 78, 0, 0, 1981, 1982, 5, 84, 0, 0, 1982, 1983, 5, 82, 0, 0, 1983, 1984, 5, 65, 0, 0, 1984, 1985, 5, 78, 0, 0, 1985, 1986, 5, 75, 0, 0, 1986, 1987, 5, 46, 0, 0, 1987, 1988, 5, 73, 0, 0, 1988, 1989, 5, 78, 0, 0, 1989, 1991, 5, 67, 0, 0, 1990, 1964, 1, 0, 0, 0, 1990, 1975, 1, 0, 0, 0, 1991, 402, 1, 0, 0, 0, 1992, 1993, 5, 65, 0, 0, 1993, 1994, 5, 86, 0, 0, 1994, 1995, 5, 69, 0, 0, 1995, 1996, 5, 82, 0, 0, 1996, 1997, 5, 65, 0, 0, 1997, 1998, 5, 71, 0, 0, 1998, 1999, 5, 69, 0, 0, 1999, 404, 1, 0, 0, 0, 2000, 2001, 5, 65, 0, 0, 2001, 2002, 5, 86, 0, 0, 2002, 2003, 5, 69, 0, 0, 2003, 2004, 5, 82, 0, 0, 2004, 2005, 5, 65, 0, 0, 2005, 2006, 5, 71, 0, 0, 2006, 2007, 5, 69, 0, 0, 2007, 2008, 5, 73, 0, 0, 2008, 2009, 5, 70, 0, 0, 2009, 406, 1, 0, 0, 0, 2010, 2011, 5, 71, 0, 0, 2011, 2012, 5, 69, 0, 0, 2012, 2013, 5, 79, 0, 0, 2013, 2014, 5, 77, 0, 0, 2014, 2015, 5, 69, 0, 0, 2015, 2016, 5, 65, 0, 0, 2016, 2017, 5, 78, 0, 0, 2017, 408, 1, 0, 0, 0, 2018, 2019, 5, 72, 0, 0, 2019, 2020, 5, 65, 0, 0, 2020, 2021, 5, 82, 0, 0, 2021, 2022, 5, 77, 0, 0, 2022, 2023, 5, 69, 0, 0, 2023, 2024, 5, 65, 0, 0, 2024, 2025, 5, 78, 0, 0, 2025, 410, 1, 0, 0, 0, 2026, 2027, 5, 67, 0, 0, 2027, 2028, 5, 79, 0, 0, 2028, 2029, 5, 85, 0, 0, 2029, 2030, 5, 78, 0, 0, 2030, 2031, 5, 84, 0, 0, 2031, 412, 1, 0, 0, 0, 2032, 2033, 5, 67, 0, 0, 2033, 2034, 5, 79, 0, 0, 2034, 2035, 5, 85, 0, 0, 2035, 2036, 5, 78, 0, 0, 2036, 2037, 5, 84, 0, 0, 2037, 2038, 5, 73, 0, 0, 2038, 2039, 5, 70, 0, 0, 2039, 414, 1, 0, 0, 0, 2040, 2041, 5, 83, 0, 0, 2041, 2042, 5, 85, 0, 0, 2042, 2043, 5, 77, 0, 0, 2043, 416, 1, 0, 0, 0, 2044, 2045, 5, 83, 0, 0, 2045, 2046, 5, 85, 0, 0, 2046, 2047, 5, 77, 0, 0, 2047, 2048, 5, 73, 0, 0, 2048, 2049, 5, 70, 0, 0, 2049, 418, 1, 0, 0, 0, 2050, 2051, 5, 65, 0, 0, 2051, 2052, 5, 86, 0, 0, 2052, 2053, 5, 69, 0, 0, 2053, 2054, 5, 68, 0, 0, 2054, 2055, 5, 69, 0, 0, 2055, 2056, 5, 86, 0, 0, 2056, 420, 1, 0, 0, 0, 2057, 2058, 5, 83, 0, 0, 2058, 2059, 5, 84, 0, 0, 2059, 2060, 5, 68, 0, 0, 2060, 2061, 5, 69, 0, 0, 2061, 2070, 5, 86, 0, 0, 2062, 2063, 5, 83, 0, 0, 2063, 2064, 5, 84, 0, 0, 2064, 2065, 5, 68, 0, 0, 2065, 2066, 5, 69, 0, 0, 2066, 2067, 5, 86, 0, 0, 2067, 2068, 5, 46, 0, 0, 2068, 2070, 5, 83, 0, 0, 2069, 2057, 1, 0, 0, 0, 2069, 2062, 1, 0, 0, 0, 2070, 422, 1, 0, 0, 0, 2071, 2072, 5, 83, 0, 0, 2072, 2073, 5, 84, 0, 0, 2073, 2074, 5, 68, 0, 0, 2074, 2075, 5, 69, 0, 0, 2075, 2076, 5, 86, 0, 0, 2076, 2085, 5, 80, 0, 0, 2077, 2078, 5, 83, 0, 0, 2078, 2079, 5, 84, 0, 0, 2079, 2080, 5, 68, 0, 0, 2080, 2081, 5, 69, 0, 0, 2081, 2082, 5, 86, 0, 0, 2082, 2083, 5, 46, 0, 0, 2083, 2085, 5, 80, 0, 0, 2084, 2071, 1, 0, 0, 0, 2084, 2077, 1, 0, 0, 0, 2085, 424, 1, 0, 0, 0, 2086, 2087, 5, 67, 0, 0, 2087, 2088, 5, 79, 0, 0, 2088, 2089, 5, 86, 0, 0, 2089, 2090, 5, 65, 0, 0, 2090, 2104, 5, 82, 0, 0, 2091, 2092, 5, 67, 0, 0, 2092, 2093, 5, 79, 0, 0, 2093, 2094, 5, 86, 0, 0, 2094, 2095, 5, 65, 0, 0, 2095, 2096, 5, 82, 0, 0, 2096, 2097, 5, 73, 0, 0, 2097, 2098, 5, 65, 0, 0, 2098, 2099, 5, 78, 0, 0, 2099, 2100, 5, 67, 0, 0, 2100, 2101, 5, 69, 0, 0, 2101, 2102, 5, 46, 0, 0, 2102, 2104, 5, 80, 0, 0, 2103, 2086, 1, 0, 0, 0, 2103, 2091, 1, 0, 0, 0, 2104, 426, 1, 0, 0, 0, 2105, 2106, 5, 67, 0, 0, 2106, 2107, 5, 79, 0, 0, 2107, 2108, 5, 86, 0, 0, 2108, 2109, 5, 65, 0, 0, 2109, 2110, 5, 82, 0, 0, 2110, 2111, 5, 73, 0, 0, 2111, 2112, 5, 65, 0, 0, 2112, 2113, 5, 78, 0, 0, 2113, 2114, 5, 67, 0, 0, 2114, 2115, 5, 69, 0, 0, 2115, 2116, 5, 46, 0, 0, 2116, 2117, 5, 83, 0, 0, 2117, 428, 1, 0, 0, 0, 2118, 2119, 5, 68, 0, 0, 2119, 2120, 5, 69, 0, 0, 2120, 2121, 5, 86, 0, 0, 2121, 2122, 5, 83, 0, 0, 2122, 2123, 5, 81, 0, 0, 2123, 430, 1, 0, 0, 0, 2124, 2125, 5, 86, 0, 0, 2125, 2126, 5, 65, 0, 0, 2126, 2133, 5, 82, 0, 0, 2127, 2128, 5, 86, 0, 0, 2128, 2129, 5, 65, 0, 0, 2129, 2130, 5, 82, 0, 0, 2130, 2131, 5, 46, 0, 0, 2131, 2133, 5, 83, 0, 0, 2132, 2124, 1, 0, 0, 0, 2132, 2127, 1, 0, 0, 0, 2133, 432, 1, 0, 0, 0, 2134, 2135, 5, 86, 0, 0, 2135, 2136, 5, 65, 0, 0, 2136, 2137, 5, 82, 0, 0, 2137, 2144, 5, 80, 0, 0, 2138, 2139, 5, 86, 0, 0, 2139, 2140, 5, 65, 0, 0, 2140, 2141, 5, 82, 0, 0, 2141, 2142, 5, 46, 0, 0, 2142, 2144, 5, 80, 0, 0, 2143, 2134, 1, 0, 0, 0, 2143, 2138, 1, 0, 0, 0, 2144, 434, 1, 0, 0, 0, 2145, 2146, 5, 78, 0, 0, 2146, 2147, 5, 79, 0, 0, 2147, 2148, 5, 82, 0, 0, 2148, 2149, 5, 77, 0, 0, 2149, 2150, 5, 68, 0, 0, 2150, 2151, 5, 73, 0, 0, 2151, 2152, 5, 83, 0, 0, 2152, 2163, 5, 84, 0, 0, 2153, 2154, 5, 78, 0, 0, 2154, 2155, 5, 79, 0, 0, 2155, 2156, 5, 82, 0, 0, 2156, 2157, 5, 77, 0, 0, 2157, 2158, 5, 46, 0, 0, 2158, 2159, 5, 68, 0, 0, 2159, 2160, 5, 73, 0, 0, 2160, 2161, 5, 83, 0, 0, 2161, 2163, 5, 84, 0, 0, 2162, 2145, 1, 0, 0, 0, 2162, 2153, 1, 0, 0, 0, 2163, 436, 1, 0, 0, 0, 2164, 2165, 5, 78, 0, 0, 2165, 2166, 5, 79, 0, 0, 2166, 2167, 5, 82, 0, 0, 2167, 2168, 5, 77, 0, 0, 2168, 2169, 5, 73, 0, 0, 2169, 2170, 5, 78, 0, 0, 2170, 2180, 5, 86, 0, 0, 2171, 2172, 5, 78, 0, 0, 2172, 2173, 5, 79, 0, 0, 2173, 2174, 5, 82, 0, 0, 2174, 2175, 5, 77, 0, 0, 2175, 2176, 5, 46, 0, 0, 2176, 2177, 5, 73, 0, 0, 2177, 2178, 5, 78, 0, 0, 2178, 2180, 5, 86, 0, 0, 2179, 2164, 1, 0, 0, 0, 2179, 2171, 1, 0, 0, 0, 2180, 438, 1, 0, 0, 0, 2181, 2182, 5, 78, 0, 0, 2182, 2183, 5, 79, 0, 0, 2183, 2184, 5, 82, 0, 0, 2184, 2185, 5, 77, 0, 0, 2185, 2186, 5, 83, 0, 0, 2186, 2187, 5, 68, 0, 0, 2187, 2188, 5, 73, 0, 0, 2188, 2189, 5, 83, 0, 0, 2189, 2202, 5, 84, 0, 0, 2190, 2191, 5, 78, 0, 0, 2191, 2192, 5, 79, 0, 0, 2192, 2193, 5, 82, 0, 0, 2193, 2194, 5, 77, 0, 0, 2194, 2195, 5, 46, 0, 0, 2195, 2196, 5, 83, 0, 0, 2196, 2197, 5, 46, 0, 0, 2197, 2198, 5, 68, 0, 0, 2198, 2199, 5, 73, 0, 0, 2199, 2200, 5, 83, 0, 0, 2200, 2202, 5, 84, 0, 0, 2201, 2181, 1, 0, 0, 0, 2201, 2190, 1, 0, 0, 0, 2202, 440, 1, 0, 0, 0, 2203, 2204, 5, 78, 0, 0, 2204, 2205, 5, 79, 0, 0, 2205, 2206, 5, 82, 0, 0, 2206, 2207, 5, 77, 0, 0, 2207, 2208, 5, 83, 0, 0, 2208, 2209, 5, 73, 0, 0, 2209, 2210, 5, 78, 0, 0, 2210, 2222, 5, 86, 0, 0, 2211, 2212, 5, 78, 0, 0, 2212, 2213, 5, 79, 0, 0, 2213, 2214, 5, 82, 0, 0, 2214, 2215, 5, 77, 0, 0, 2215, 2216, 5, 46, 0, 0, 2216, 2217, 5, 83, 0, 0, 2217, 2218, 5, 46, 0, 0, 2218, 2219, 5, 73, 0, 0, 2219, 2220, 5, 78, 0, 0, 2220, 2222, 5, 86, 0, 0, 2221, 2203, 1, 0, 0, 0, 2221, 2211, 1, 0, 0, 0, 2222, 442, 1, 0, 0, 0, 2223, 2224, 5, 66, 0, 0, 2224, 2225, 5, 69, 0, 0, 2225, 2226, 5, 84, 0, 0, 2226, 2227, 5, 65, 0, 0, 2227, 2228, 5, 68, 0, 0, 2228, 2229, 5, 73, 0, 0, 2229, 2230, 5, 83, 0, 0, 2230, 2241, 5, 84, 0, 0, 2231, 2232, 5, 66, 0, 0, 2232, 2233, 5, 69, 0, 0, 2233, 2234, 5, 84, 0, 0, 2234, 2235, 5, 65, 0, 0, 2235, 2236, 5, 46, 0, 0, 2236, 2237, 5, 68, 0, 0, 2237, 2238, 5, 73, 0, 0, 2238, 2239, 5, 83, 0, 0, 2239, 2241, 5, 84, 0, 0, 2240, 2223, 1, 0, 0, 0, 2240, 2231, 1, 0, 0, 0, 2241, 444, 1, 0, 0, 0, 2242, 2243, 5, 66, 0, 0, 2243, 2244, 5, 69, 0, 0, 2244, 2245, 5, 84, 0, 0, 2245, 2246, 5, 65, 0, 0, 2246, 2247, 5, 73, 0, 0, 2247, 2248, 5, 78, 0, 0, 2248, 2258, 5, 86, 0, 0, 2249, 2250, 5, 66, 0, 0, 2250, 2251, 5, 69, 0, 0, 2251, 2252, 5, 84, 0, 0, 2252, 2253, 5, 65, 0, 0, 2253, 2254, 5, 46, 0, 0, 2254, 2255, 5, 73, 0, 0, 2255, 2256, 5, 78, 0, 0, 2256, 2258, 5, 86, 0, 0, 2257, 2242, 1, 0, 0, 0, 2257, 2249, 1, 0, 0, 0, 2258, 446, 1, 0, 0, 0, 2259, 2260, 5, 66, 0, 0, 2260, 2261, 5, 73, 0, 0, 2261, 2262, 5, 78, 0, 0, 2262, 2263, 5, 79, 0, 0, 2263, 2264, 5, 77, 0, 0, 2264, 2265, 5, 68, 0, 0, 2265, 2266, 5, 73, 0, 0, 2266, 2267, 5, 83, 0, 0, 2267, 2279, 5, 84, 0, 0, 2268, 2269, 5, 66, 0, 0, 2269, 2270, 5, 73, 0, 0, 2270, 2271, 5, 78, 0, 0, 2271, 2272, 5, 79, 0, 0, 2272, 2273, 5, 77, 0, 0, 2273, 2274, 5, 46, 0, 0, 2274, 2275, 5, 68, 0, 0, 2275, 2276, 5, 73, 0, 0, 2276, 2277, 5, 83, 0, 0, 2277, 2279, 5, 84, 0, 0, 2278, 2259, 1, 0, 0, 0, 2278, 2268, 1, 0, 0, 0, 2279, 448, 1, 0, 0, 0, 2280, 2281, 5, 69, 0, 0, 2281, 2282, 5, 88, 0, 0, 2282, 2283, 5, 80, 0, 0, 2283, 2284, 5, 79, 0, 0, 2284, 2285, 5, 78, 0, 0, 2285, 2286, 5, 68, 0, 0, 2286, 2287, 5, 73, 0, 0, 2287, 2288, 5, 83, 0, 0, 2288, 2300, 5, 84, 0, 0, 2289, 2290, 5, 69, 0, 0, 2290, 2291, 5, 88, 0, 0, 2291, 2292, 5, 80, 0, 0, 2292, 2293, 5, 79, 0, 0, 2293, 2294, 5, 78, 0, 0, 2294, 2295, 5, 46, 0, 0, 2295, 2296, 5, 68, 0, 0, 2296, 2297, 5, 73, 0, 0, 2297, 2298, 5, 83, 0, 0, 2298, 2300, 5, 84, 0, 0, 2299, 2280, 1, 0, 0, 0, 2299, 2289, 1, 0, 0, 0, 2300, 450, 1, 0, 0, 0, 2301, 2302, 5, 70, 0, 0, 2302, 2303, 5, 68, 0, 0, 2303, 2304, 5, 73, 0, 0, 2304, 2305, 5, 83, 0, 0, 2305, 2313, 5, 84, 0, 0, 2306, 2307, 5, 70, 0, 0, 2307, 2308, 5, 46, 0, 0, 2308, 2309, 5, 68, 0, 0, 2309, 2310, 5, 73, 0, 0, 2310, 2311, 5, 83, 0, 0, 2311, 2313, 5, 84, 0, 0, 2312, 2301, 1, 0, 0, 0, 2312, 2306, 1, 0, 0, 0, 2313, 452, 1, 0, 0, 0, 2314, 2315, 5, 70, 0, 0, 2315, 2316, 5, 73, 0, 0, 2316, 2317, 5, 78, 0, 0, 2317, 2324, 5, 86, 0, 0, 2318, 2319, 5, 70, 0, 0, 2319, 2320, 5, 46, 0, 0, 2320, 2321, 5, 73, 0, 0, 2321, 2322, 5, 78, 0, 0, 2322, 2324, 5, 86, 0, 0, 2323, 2314, 1, 0, 0, 0, 2323, 2318, 1, 0, 0, 0, 2324, 454, 1, 0, 0, 0, 2325, 2326, 5, 70, 0, 0, 2326, 2327, 5, 73, 0, 0, 2327, 2328, 5, 83, 0, 0, 2328, 2329, 5, 72, 0, 0, 2329, 2330, 5, 69, 0, 0, 2330, 2331, 5, 82, 0, 0, 2331, 456, 1, 0, 0, 0, 2332, 2333, 5, 70, 0, 0, 2333, 2334, 5, 73, 0, 0, 2334, 2335, 5, 83, 0, 0, 2335, 2336, 5, 72, 0, 0, 2336, 2337, 5, 69, 0, 0, 2337, 2338, 5, 82, 0, 0, 2338, 2339, 5, 73, 0, 0, 2339, 2340, 5, 78, 0, 0, 2340, 2341, 5, 86, 0, 0, 2341, 458, 1, 0, 0, 0, 2342, 2343, 5, 71, 0, 0, 2343, 2344, 5, 65, 0, 0, 2344, 2345, 5, 77, 0, 0, 2345, 2346, 5, 77, 0, 0, 2346, 2347, 5, 65, 0, 0, 2347, 2348, 5, 68, 0, 0, 2348, 2349, 5, 73, 0, 0, 2349, 2350, 5, 83, 0, 0, 2350, 2362, 5, 84, 0, 0, 2351, 2352, 5, 71, 0, 0, 2352, 2353, 5, 65, 0, 0, 2353, 2354, 5, 77, 0, 0, 2354, 2355, 5, 77, 0, 0, 2355, 2356, 5, 65, 0, 0, 2356, 2357, 5, 46, 0, 0, 2357, 2358, 5, 68, 0, 0, 2358, 2359, 5, 73, 0, 0, 2359, 2360, 5, 83, 0, 0, 2360, 2362, 5, 84, 0, 0, 2361, 2342, 1, 0, 0, 0, 2361, 2351, 1, 0, 0, 0, 2362, 460, 1, 0, 0, 0, 2363, 2364, 5, 71, 0, 0, 2364, 2365, 5, 65, 0, 0, 2365, 2366, 5, 77, 0, 0, 2366, 2367, 5, 77, 0, 0, 2367, 2368, 5, 65, 0, 0, 2368, 2369, 5, 73, 0, 0, 2369, 2370, 5, 78, 0, 0, 2370, 2381, 5, 86, 0, 0, 2371, 2372, 5, 71, 0, 0, 2372, 2373, 5, 65, 0, 0, 2373, 2374, 5, 77, 0, 0, 2374, 2375, 5, 77, 0, 0, 2375, 2376, 5, 65, 0, 0, 2376, 2377, 5, 46, 0, 0, 2377, 2378, 5, 73, 0, 0, 2378, 2379, 5, 78, 0, 0, 2379, 2381, 5, 86, 0, 0, 2380, 2363, 1, 0, 0, 0, 2380, 2371, 1, 0, 0, 0, 2381, 462, 1, 0, 0, 0, 2382, 2383, 5, 71, 0, 0, 2383, 2384, 5, 65, 0, 0, 2384, 2385, 5, 77, 0, 0, 2385, 2386, 5, 77, 0, 0, 2386, 2387, 5, 65, 0, 0, 2387, 2388, 5, 76, 0, 0, 2388, 2405, 5, 78, 0, 0, 2389, 2390, 5, 71, 0, 0, 2390, 2391, 5, 65, 0, 0, 2391, 2392, 5, 77, 0, 0, 2392, 2393, 5, 77, 0, 0, 2393, 2394, 5, 65, 0, 0, 2394, 2395, 5, 76, 0, 0, 2395, 2396, 5, 78, 0, 0, 2396, 2397, 5, 46, 0, 0, 2397, 2398, 5, 80, 0, 0, 2398, 2399, 5, 82, 0, 0, 2399, 2400, 5, 69, 0, 0, 2400, 2401, 5, 67, 0, 0, 2401, 2402, 5, 73, 0, 0, 2402, 2403, 5, 83, 0, 0, 2403, 2405, 5, 69, 0, 0, 2404, 2382, 1, 0, 0, 0, 2404, 2389, 1, 0, 0, 0, 2405, 464, 1, 0, 0, 0, 2406, 2407, 5, 72, 0, 0, 2407, 2408, 5, 89, 0, 0, 2408, 2409, 5, 80, 0, 0, 2409, 2410, 5, 71, 0, 0, 2410, 2411, 5, 69, 0, 0, 2411, 2412, 5, 79, 0, 0, 2412, 2413, 5, 77, 0, 0, 2413, 2414, 5, 68, 0, 0, 2414, 2415, 5, 73, 0, 0, 2415, 2416, 5, 83, 0, 0, 2416, 2430, 5, 84, 0, 0, 2417, 2418, 5, 72, 0, 0, 2418, 2419, 5, 89, 0, 0, 2419, 2420, 5, 80, 0, 0, 2420, 2421, 5, 71, 0, 0, 2421, 2422, 5, 69, 0, 0, 2422, 2423, 5, 79, 0, 0, 2423, 2424, 5, 77, 0, 0, 2424, 2425, 5, 46, 0, 0, 2425, 2426, 5, 68, 0, 0, 2426, 2427, 5, 73, 0, 0, 2427, 2428, 5, 83, 0, 0, 2428, 2430, 5, 84, 0, 0, 2429, 2406, 1, 0, 0, 0, 2429, 2417, 1, 0, 0, 0, 2430, 466, 1, 0, 0, 0, 2431, 2432, 5, 76, 0, 0, 2432, 2433, 5, 79, 0, 0, 2433, 2434, 5, 71, 0, 0, 2434, 2435, 5, 73, 0, 0, 2435, 2436, 5, 78, 0, 0, 2436, 2449, 5, 86, 0, 0, 2437, 2438, 5, 76, 0, 0, 2438, 2439, 5, 79, 0, 0, 2439, 2440, 5, 71, 0, 0, 2440, 2441, 5, 78, 0, 0, 2441, 2442, 5, 79, 0, 0, 2442, 2443, 5, 82, 0, 0, 2443, 2444, 5, 77, 0, 0, 2444, 2445, 5, 46, 0, 0, 2445, 2446, 5, 73, 0, 0, 2446, 2447, 5, 78, 0, 0, 2447, 2449, 5, 86, 0, 0, 2448, 2431, 1, 0, 0, 0, 2448, 2437, 1, 0, 0, 0, 2449, 468, 1, 0, 0, 0, 2450, 2451, 5, 76, 0, 0, 2451, 2452, 5, 79, 0, 0, 2452, 2453, 5, 71, 0, 0, 2453, 2454, 5, 78, 0, 0, 2454, 2455, 5, 79, 0, 0, 2455, 2456, 5, 82, 0, 0, 2456, 2457, 5, 77, 0, 0, 2457, 2458, 5, 68, 0, 0, 2458, 2459, 5, 73, 0, 0, 2459, 2460, 5, 83, 0, 0, 2460, 2474, 5, 84, 0, 0, 2461, 2462, 5, 76, 0, 0, 2462, 2463, 5, 79, 0, 0, 2463, 2464, 5, 71, 0, 0, 2464, 2465, 5, 78, 0, 0, 2465, 2466, 5, 79, 0, 0, 2466, 2467, 5, 82, 0, 0, 2467, 2468, 5, 77, 0, 0, 2468, 2469, 5, 46, 0, 0, 2469, 2470, 5, 68, 0, 0, 2470, 2471, 5, 73, 0, 0, 2471, 2472, 5, 83, 0, 0, 2472, 2474, 5, 84, 0, 0, 2473, 2450, 1, 0, 0, 0, 2473, 2461, 1, 0, 0, 0, 2474, 470, 1, 0, 0, 0, 2475, 2476, 5, 78, 0, 0, 2476, 2477, 5, 69, 0, 0, 2477, 2478, 5, 71, 0, 0, 2478, 2479, 5, 66, 0, 0, 2479, 2480, 5, 73, 0, 0, 2480, 2481, 5, 78, 0, 0, 2481, 2482, 5, 79, 0, 0, 2482, 2483, 5, 77, 0, 0, 2483, 2484, 5, 68, 0, 0, 2484, 2485, 5, 73, 0, 0, 2485, 2486, 5, 83, 0, 0, 2486, 2501, 5, 84, 0, 0, 2487, 2488, 5, 78, 0, 0, 2488, 2489, 5, 69, 0, 0, 2489, 2490, 5, 71, 0, 0, 2490, 2491, 5, 66, 0, 0, 2491, 2492, 5, 73, 0, 0, 2492, 2493, 5, 78, 0, 0, 2493, 2494, 5, 79, 0, 0, 2494, 2495, 5, 77, 0, 0, 2495, 2496, 5, 46, 0, 0, 2496, 2497, 5, 68, 0, 0, 2497, 2498, 5, 73, 0, 0, 2498, 2499, 5, 83, 0, 0, 2499, 2501, 5, 84, 0, 0, 2500, 2475, 1, 0, 0, 0, 2500, 2487, 1, 0, 0, 0, 2501, 472, 1, 0, 0, 0, 2502, 2503, 5, 80, 0, 0, 2503, 2504, 5, 79, 0, 0, 2504, 2505, 5, 73, 0, 0, 2505, 2506, 5, 83, 0, 0, 2506, 2507, 5, 83, 0, 0, 2507, 2508, 5, 79, 0, 0, 2508, 2522, 5, 78, 0, 0, 2509, 2510, 5, 80, 0, 0, 2510, 2511, 5, 79, 0, 0, 2511, 2512, 5, 73, 0, 0, 2512, 2513, 5, 83, 0, 0, 2513, 2514, 5, 83, 0, 0, 2514, 2515, 5, 79, 0, 0, 2515, 2516, 5, 78, 0, 0, 2516, 2517, 5, 46, 0, 0, 2517, 2518, 5, 68, 0, 0, 2518, 2519, 5, 73, 0, 0, 2519, 2520, 5, 83, 0, 0, 2520, 2522, 5, 84, 0, 0, 2521, 2502, 1, 0, 0, 0, 2521, 2509, 1, 0, 0, 0, 2522, 474, 1, 0, 0, 0, 2523, 2524, 5, 84, 0, 0, 2524, 2525, 5, 68, 0, 0, 2525, 2526, 5, 73, 0, 0, 2526, 2527, 5, 83, 0, 0, 2527, 2535, 5, 84, 0, 0, 2528, 2529, 5, 84, 0, 0, 2529, 2530, 5, 46, 0, 0, 2530, 2531, 5, 68, 0, 0, 2531, 2532, 5, 73, 0, 0, 2532, 2533, 5, 83, 0, 0, 2533, 2535, 5, 84, 0, 0, 2534, 2523, 1, 0, 0, 0, 2534, 2528, 1, 0, 0, 0, 2535, 476, 1, 0, 0, 0, 2536, 2537, 5, 84, 0, 0, 2537, 2538, 5, 73, 0, 0, 2538, 2539, 5, 78, 0, 0, 2539, 2546, 5, 86, 0, 0, 2540, 2541, 5, 84, 0, 0, 2541, 2542, 5, 46, 0, 0, 2542, 2543, 5, 73, 0, 0, 2543, 2544, 5, 78, 0, 0, 2544, 2546, 5, 86, 0, 0, 2545, 2536, 1, 0, 0, 0, 2545, 2540, 1, 0, 0, 0, 2546, 478, 1, 0, 0, 0, 2547, 2548, 5, 87, 0, 0, 2548, 2549, 5, 69, 0, 0, 2549, 2550, 5, 73, 0, 0, 2550, 2551, 5, 66, 0, 0, 2551, 2552, 5, 85, 0, 0, 2552, 2553, 5, 76, 0, 0, 2553, 2554, 5, 76, 0, 0, 2554, 480, 1, 0, 0, 0, 2555, 2556, 5, 80, 0, 0, 2556, 2557, 5, 77, 0, 0, 2557, 2558, 5, 84, 0, 0, 2558, 482, 1, 0, 0, 0, 2559, 2560, 5, 80, 0, 0, 2560, 2561, 5, 80, 0, 0, 2561, 2562, 5, 77, 0, 0, 2562, 2563, 5, 84, 0, 0, 2563, 484, 1, 0, 0, 0, 2564, 2565, 5, 73, 0, 0, 2565, 2566, 5, 80, 0, 0, 2566, 2567, 5, 77, 0, 0, 2567, 2568, 5, 84, 0, 0, 2568, 486, 1, 0, 0, 0, 2569, 2570, 5, 80, 0, 0, 2570, 2571, 5, 86, 0, 0, 2571, 488, 1, 0, 0, 0, 2572, 2573, 5, 70, 0, 0, 2573, 2574, 5, 86, 0, 0, 2574, 490, 1, 0, 0, 0, 2575, 2576, 5, 78, 0, 0, 2576, 2577, 5, 80, 0, 0, 2577, 2578, 5, 69, 0, 0, 2578, 2579, 5, 82, 0, 0, 2579, 492, 1, 0, 0, 0, 2580, 2581, 5, 82, 0, 0, 2581, 2582, 5, 65, 0, 0, 2582, 2583, 5, 84, 0, 0, 2583, 2584, 5, 69, 0, 0, 2584, 494, 1, 0, 0, 0, 2585, 2586, 5, 78, 0, 0, 2586, 2587, 5, 80, 0, 0, 2587, 2588, 5, 86, 0, 0, 2588, 496, 1, 0, 0, 0, 2589, 2590, 5, 88, 0, 0, 2590, 2591, 5, 78, 0, 0, 2591, 2592, 5, 80, 0, 0, 2592, 2593, 5, 86, 0, 0, 2593, 498, 1, 0, 0, 0, 2594, 2595, 5, 73, 0, 0, 2595, 2596, 5, 82, 0, 0, 2596, 2597, 5, 82, 0, 0, 2597, 500, 1, 0, 0, 0, 2598, 2599, 5, 77, 0, 0, 2599, 2600, 5, 73, 0, 0, 2600, 2601, 5, 82, 0, 0, 2601, 2602, 5, 82, 0, 0, 2602, 502, 1, 0, 0, 0, 2603, 2604, 5, 88, 0, 0, 2604, 2605, 5, 73, 0, 0, 2605, 2606, 5, 82, 0, 0, 2606, 2607, 5, 82, 0, 0, 2607, 504, 1, 0, 0, 0, 2608, 2609, 5, 83, 0, 0, 2609, 2610, 5, 76, 0, 0, 2610, 2611, 5, 78, 0, 0, 2611, 506, 1, 0, 0, 0, 2612, 2613, 5, 68, 0, 0, 2613, 2614, 5, 66, 0, 0, 2614, 508, 1, 0, 0, 0, 2615, 2616, 5, 68, 0, 0, 2616, 2617, 5, 68, 0, 0, 2617, 2618, 5, 66, 0, 0, 2618, 510, 1, 0, 0, 0, 2619, 2620, 5, 83, 0, 0, 2620, 2621, 5, 89, 0, 0, 2621, 2622, 5, 68, 0, 0, 2622, 512, 1, 0, 0, 0, 2623, 2624, 5, 85, 0, 0, 2624, 2625, 5, 82, 0, 0, 2625, 2626, 5, 76, 0, 0, 2626, 2627, 5, 69, 0, 0, 2627, 2628, 5, 78, 0, 0, 2628, 2629, 5, 67, 0, 0, 2629, 2630, 5, 79, 0, 0, 2630, 2631, 5, 68, 0, 0, 2631, 2632, 5, 69, 0, 0, 2632, 514, 1, 0, 0, 0, 2633, 2634, 5, 85, 0, 0, 2634, 2635, 5, 82, 0, 0, 2635, 2636, 5, 76, 0, 0, 2636, 2637, 5, 68, 0, 0, 2637, 2638, 5, 69, 0, 0, 2638, 2639, 5, 67, 0, 0, 2639, 2640, 5, 79, 0, 0, 2640, 2641, 5, 68, 0, 0, 2641, 2642, 5, 69, 0, 0, 2642, 516, 1, 0, 0, 0, 2643, 2644, 5, 72, 0, 0, 2644, 2645, 5, 84, 0, 0, 2645, 2646, 5, 77, 0, 0, 2646, 2647, 5, 76, 0, 0, 2647, 2648, 5, 69, 0, 0, 2648, 2649, 5, 78, 0, 0, 2649, 2650, 5, 67, 0, 0, 2650, 2651, 5, 79, 0, 0, 2651, 2652, 5, 68, 0, 0, 2652, 2653, 5, 69, 0, 0, 2653, 518, 1, 0, 0, 0, 2654, 2655, 5, 72, 0, 0, 2655, 2656, 5, 84, 0, 0, 2656, 2657, 5, 77, 0, 0, 2657, 2658, 5, 76, 0, 0, 2658, 2659, 5, 68, 0, 0, 2659, 2660, 5, 69, 0, 0, 2660, 2661, 5, 67, 0, 0, 2661, 2662, 5, 79, 0, 0, 2662, 2663, 5, 68, 0, 0, 2663, 2664, 5, 69, 0, 0, 2664, 520, 1, 0, 0, 0, 2665, 2666, 5, 66, 0, 0, 2666, 2667, 5, 65, 0, 0, 2667, 2668, 5, 83, 0, 0, 2668, 2669, 5, 69, 0, 0, 2669, 2670, 5, 54, 0, 0, 2670, 2671, 5, 52, 0, 0, 2671, 2672, 5, 84, 0, 0, 2672, 2673, 5, 79, 0, 0, 2673, 2674, 5, 84, 0, 0, 2674, 2675, 5, 69, 0, 0, 2675, 2676, 5, 88, 0, 0, 2676, 2677, 5, 84, 0, 0, 2677, 522, 1, 0, 0, 0, 2678, 2679, 5, 66, 0, 0, 2679, 2680, 5, 65, 0, 0, 2680, 2681, 5, 83, 0, 0, 2681, 2682, 5, 69, 0, 0, 2682, 2683, 5, 54, 0, 0, 2683, 2684, 5, 52, 0, 0, 2684, 2685, 5, 85, 0, 0, 2685, 2686, 5, 82, 0, 0, 2686, 2687, 5, 76, 0, 0, 2687, 2688, 5, 84, 0, 0, 2688, 2689, 5, 79, 0, 0, 2689, 2690, 5, 84, 0, 0, 2690, 2691, 5, 69, 0, 0, 2691, 2692, 5, 88, 0, 0, 2692, 2693, 5, 84, 0, 0, 2693, 524, 1, 0, 0, 0, 2694, 2695, 5, 84, 0, 0, 2695, 2696, 5, 69, 0, 0, 2696, 2697, 5, 88, 0, 0, 2697, 2698, 5, 84, 0, 0, 2698, 2699, 5, 84, 0, 0, 2699, 2700, 5, 79, 0, 0, 2700, 2701, 5, 66, 0, 0, 2701, 2702, 5, 65, 0, 0, 2702, 2703, 5, 83, 0, 0, 2703, 2704, 5, 69, 0, 0, 2704, 2705, 5, 54, 0, 0, 2705, 2706, 5, 52, 0, 0, 2706, 526, 1, 0, 0, 0, 2707, 2708, 5, 84, 0, 0, 2708, 2709, 5, 69, 0, 0, 2709, 2710, 5, 88, 0, 0, 2710, 2711, 5, 84, 0, 0, 2711, 2712, 5, 84, 0, 0, 2712, 2713, 5, 79, 0, 0, 2713, 2714, 5, 66, 0, 0, 2714, 2715, 5, 65, 0, 0, 2715, 2716, 5, 83, 0, 0, 2716, 2717, 5, 69, 0, 0, 2717, 2718, 5, 54, 0, 0, 2718, 2719, 5, 52, 0, 0, 2719, 2720, 5, 85, 0, 0, 2720, 2721, 5, 82, 0, 0, 2721, 2722, 5, 76, 0, 0, 2722, 528, 1, 0, 0, 0, 2723, 2724, 5, 82, 0, 0, 2724, 2725, 5, 69, 0, 0, 2725, 2726, 5, 71, 0, 0, 2726, 2727, 5, 69, 0, 0, 2727, 2728, 5, 88, 0, 0, 2728, 530, 1, 0, 0, 0, 2729, 2730, 5, 82, 0, 0, 2730, 2731, 5, 69, 0, 0, 2731, 2732, 5, 71, 0, 0, 2732, 2733, 5, 69, 0, 0, 2733, 2734, 5, 88, 0, 0, 2734, 2735, 5, 82, 0, 0, 2735, 2736, 5, 69, 0, 0, 2736, 2737, 5, 80, 0, 0, 2737, 2738, 5, 76, 0, 0, 2738, 2739, 5, 65, 0, 0, 2739, 2740, 5, 67, 0, 0, 2740, 2741, 5, 69, 0, 0, 2741, 532, 1, 0, 0, 0, 2742, 2743, 5, 73, 0, 0, 2743, 2744, 5, 83, 0, 0, 2744, 2745, 5, 82, 0, 0, 2745, 2746, 5, 69, 0, 0, 2746, 2747, 5, 71, 0, 0, 2747, 2748, 5, 69, 0, 0, 2748, 2757, 5, 88, 0, 0, 2749, 2750, 5, 73, 0, 0, 2750, 2751, 5, 83, 0, 0, 2751, 2752, 5, 77, 0, 0, 2752, 2753, 5, 65, 0, 0, 2753, 2754, 5, 84, 0, 0, 2754, 2755, 5, 67, 0, 0, 2755, 2757, 5, 72, 0, 0, 2756, 2742, 1, 0, 0, 0, 2756, 2749, 1, 0, 0, 0, 2757, 534, 1, 0, 0, 0, 2758, 2759, 5, 71, 0, 0, 2759, 2760, 5, 85, 0, 0, 2760, 2761, 5, 73, 0, 0, 2761, 2762, 5, 68, 0, 0, 2762, 536, 1, 0, 0, 0, 2763, 2764, 5, 77, 0, 0, 2764, 2765, 5, 68, 0, 0, 2765, 2766, 5, 53, 0, 0, 2766, 538, 1, 0, 0, 0, 2767, 2768, 5, 83, 0, 0, 2768, 2769, 5, 72, 0, 0, 2769, 2770, 5, 65, 0, 0, 2770, 2771, 5, 49, 0, 0, 2771, 540, 1, 0, 0, 0, 2772, 2773, 5, 83, 0, 0, 2773, 2774, 5, 72, 0, 0, 2774, 2775, 5, 65, 0, 0, 2775, 2776, 5, 50, 0, 0, 2776, 2777, 5, 53, 0, 0, 2777, 2778, 5, 54, 0, 0, 2778, 542, 1, 0, 0, 0, 2779, 2780, 5, 83, 0, 0, 2780, 2781, 5, 72, 0, 0, 2781, 2782, 5, 65, 0, 0, 2782, 2783, 5, 53, 0, 0, 2783, 2784, 5, 49, 0, 0, 2784, 2785, 5, 50, 0, 0, 2785, 544, 1, 0, 0, 0, 2786, 2787, 5, 72, 0, 0, 2787, 2788, 5, 77, 0, 0, 2788, 2789, 5, 65, 0, 0, 2789, 2790, 5, 67, 0, 0, 2790, 2791, 5, 77, 0, 0, 2791, 2792, 5, 68, 0, 0, 2792, 2793, 5, 53, 0, 0, 2793, 546, 1, 0, 0, 0, 2794, 2795, 5, 72, 0, 0, 2795, 2796, 5, 77, 0, 0, 2796, 2797, 5, 65, 0, 0, 2797, 2798, 5, 67, 0, 0, 2798, 2799, 5, 83, 0, 0, 2799, 2800, 5, 72, 0, 0, 2800, 2801, 5, 65, 0, 0, 2801, 2802, 5, 49, 0, 0, 2802, 548, 1, 0, 0, 0, 2803, 2804, 5, 72, 0, 0, 2804, 2805, 5, 77, 0, 0, 2805, 2806, 5, 65, 0, 0, 2806, 2807, 5, 67, 0, 0, 2807, 2808, 5, 83, 0, 0, 2808, 2809, 5, 72, 0, 0, 2809, 2810, 5, 65, 0, 0, 2810, 2811, 5, 50, 0, 0, 2811, 2812, 5, 53, 0, 0, 2812, 2813, 5, 54, 0, 0, 2813, 550, 1, 0, 0, 0, 2814, 2815, 5, 72, 0, 0, 2815, 2816, 5, 77, 0, 0, 2816, 2817, 5, 65, 0, 0, 2817, 2818, 5, 67, 0, 0, 2818, 2819, 5, 83, 0, 0, 2819, 2820, 5, 72, 0, 0, 2820, 2821, 5, 65, 0, 0, 2821, 2822, 5, 53, 0, 0, 2822, 2823, 5, 49, 0, 0, 2823, 2824, 5, 50, 0, 0, 2824, 552, 1, 0, 0, 0, 2825, 2826, 5, 84, 0, 0, 2826, 2827, 5, 82, 0, 0, 2827, 2828, 5, 73, 0, 0, 2828, 2829, 5, 77, 0, 0, 2829, 2830, 5, 83, 0, 0, 2830, 2831, 5, 84, 0, 0, 2831, 2832, 5, 65, 0, 0, 2832, 2833, 5, 82, 0, 0, 2833, 2840, 5, 84, 0, 0, 2834, 2835, 5, 76, 0, 0, 2835, 2836, 5, 84, 0, 0, 2836, 2837, 5, 82, 0, 0, 2837, 2838, 5, 73, 0, 0, 2838, 2840, 5, 77, 0, 0, 2839, 2825, 1, 0, 0, 0, 2839, 2834, 1, 0, 0, 0, 2840, 554, 1, 0, 0, 0, 2841, 2842, 5, 84, 0, 0, 2842, 2843, 5, 82, 0, 0, 2843, 2844, 5, 73, 0, 0, 2844, 2845, 5, 77, 0, 0, 2845, 2846, 5, 69, 0, 0, 2846, 2847, 5, 78, 0, 0, 2847, 2854, 5, 68, 0, 0, 2848, 2849, 5, 82, 0, 0, 2849, 2850, 5, 84, 0, 0, 2850, 2851, 5, 82, 0, 0, 2851, 2852, 5, 73, 0, 0, 2852, 2854, 5, 77, 0, 0, 2853, 2841, 1, 0, 0, 0, 2853, 2848, 1, 0, 0, 0, 2854, 556, 1, 0, 0, 0, 2855, 2856, 5, 73, 0, 0, 2856, 2857, 5, 78, 0, 0, 2857, 2858, 5, 68, 0, 0, 2858, 2859, 5, 69, 0, 0, 2859, 2860, 5, 88, 0, 0, 2860, 2861, 5, 79, 0, 0, 2861, 2862, 5, 70, 0, 0, 2862, 558, 1, 0, 0, 0, 2863, 2864, 5, 76, 0, 0, 2864, 2865, 5, 65, 0, 0, 2865, 2866, 5, 83, 0, 0, 2866, 2867, 5, 84, 0, 0, 2867, 2868, 5, 73, 0, 0, 2868, 2869, 5, 78, 0, 0, 2869, 2870, 5, 68, 0, 0, 2870, 2871, 5, 69, 0, 0, 2871, 2872, 5, 88, 0, 0, 2872, 2873, 5, 79, 0, 0, 2873, 2874, 5, 70, 0, 0, 2874, 560, 1, 0, 0, 0, 2875, 2876, 5, 83, 0, 0, 2876, 2877, 5, 80, 0, 0, 2877, 2878, 5, 76, 0, 0, 2878, 2879, 5, 73, 0, 0, 2879, 2880, 5, 84, 0, 0, 2880, 562, 1, 0, 0, 0, 2881, 2882, 5, 74, 0, 0, 2882, 2883, 5, 79, 0, 0, 2883, 2884, 5, 73, 0, 0, 2884, 2885, 5, 78, 0, 0, 2885, 564, 1, 0, 0, 0, 2886, 2887, 5, 83, 0, 0, 2887, 2888, 5, 85, 0, 0, 2888, 2889, 5, 66, 0, 0, 2889, 2890, 5, 83, 0, 0, 2890, 2891, 5, 84, 0, 0, 2891, 2892, 5, 82, 0, 0, 2892, 2893, 5, 73, 0, 0, 2893, 2894, 5, 78, 0, 0, 2894, 2895, 5, 71, 0, 0, 2895, 566, 1, 0, 0, 0, 2896, 2897, 5, 83, 0, 0, 2897, 2898, 5, 84, 0, 0, 2898, 2899, 5, 65, 0, 0, 2899, 2900, 5, 82, 0, 0, 2900, 2901, 5, 84, 0, 0, 2901, 2902, 5, 83, 0, 0, 2902, 2903, 5, 87, 0, 0, 2903, 2904, 5, 73, 0, 0, 2904, 2905, 5, 84, 0, 0, 2905, 2906, 5, 72, 0, 0, 2906, 568, 1, 0, 0, 0, 2907, 2908, 5, 69, 0, 0, 2908, 2909, 5, 78, 0, 0, 2909, 2910, 5, 68, 0, 0, 2910, 2911, 5, 83, 0, 0, 2911, 2912, 5, 87, 0, 0, 2912, 2913, 5, 73, 0, 0, 2913, 2914, 5, 84, 0, 0, 2914, 2915, 5, 72, 0, 0, 2915, 570, 1, 0, 0, 0, 2916, 2917, 5, 73, 0, 0, 2917, 2918, 5, 83, 0, 0, 2918, 2919, 5, 78, 0, 0, 2919, 2920, 5, 85, 0, 0, 2920, 2921, 5, 76, 0, 0, 2921, 2922, 5, 76, 0, 0, 2922, 2923, 5, 79, 0, 0, 2923, 2924, 5, 82, 0, 0, 2924, 2925, 5, 69, 0, 0, 2925, 2926, 5, 77, 0, 0, 2926, 2927, 5, 80, 0, 0, 2927, 2928, 5, 84, 0, 0, 2928, 2929, 5, 89, 0, 0, 2929, 572, 1, 0, 0, 0, 2930, 2931, 5, 73, 0, 0, 2931, 2932, 5, 83, 0, 0, 2932, 2933, 5, 78, 0, 0, 2933, 2934, 5, 85, 0, 0, 2934, 2935, 5, 76, 0, 0, 2935, 2936, 5, 76, 0, 0, 2936, 2937, 5, 79, 0, 0, 2937, 2938, 5, 82, 0, 0, 2938, 2939, 5, 87, 0, 0, 2939, 2940, 5, 72, 0, 0, 2940, 2941, 5, 73, 0, 0, 2941, 2942, 5, 84, 0, 0, 2942, 2943, 5, 69, 0, 0, 2943, 2944, 5, 83, 0, 0, 2944, 2945, 5, 80, 0, 0, 2945, 2946, 5, 65, 0, 0, 2946, 2947, 5, 67, 0, 0, 2947, 2948, 5, 69, 0, 0, 2948, 574, 1, 0, 0, 0, 2949, 2950, 5, 82, 0, 0, 2950, 2951, 5, 69, 0, 0, 2951, 2952, 5, 77, 0, 0, 2952, 2953, 5, 79, 0, 0, 2953, 2954, 5, 86, 0, 0, 2954, 2955, 5, 69, 0, 0, 2955, 2956, 5, 83, 0, 0, 2956, 2957, 5, 84, 0, 0, 2957, 2958, 5, 65, 0, 0, 2958, 2959, 5, 82, 0, 0, 2959, 2960, 5, 84, 0, 0, 2960, 576, 1, 0, 0, 0, 2961, 2962, 5, 82, 0, 0, 2962, 2963, 5, 69, 0, 0, 2963, 2964, 5, 77, 0, 0, 2964, 2965, 5, 79, 0, 0, 2965, 2966, 5, 86, 0, 0, 2966, 2967, 5, 69, 0, 0, 2967, 2968, 5, 69, 0, 0, 2968, 2969, 5, 78, 0, 0, 2969, 2970, 5, 68, 0, 0, 2970, 578, 1, 0, 0, 0, 2971, 2972, 5, 74, 0, 0, 2972, 2973, 5, 83, 0, 0, 2973, 2974, 5, 79, 0, 0, 2974, 2975, 5, 78, 0, 0, 2975, 580, 1, 0, 0, 0, 2976, 2977, 5, 76, 0, 0, 2977, 2978, 5, 79, 0, 0, 2978, 2979, 5, 79, 0, 0, 2979, 2980, 5, 75, 0, 0, 2980, 2981, 5, 67, 0, 0, 2981, 2982, 5, 69, 0, 0, 2982, 2983, 5, 73, 0, 0, 2983, 2984, 5, 76, 0, 0, 2984, 2985, 5, 73, 0, 0, 2985, 2986, 5, 78, 0, 0, 2986, 2987, 5, 71, 0, 0, 2987, 582, 1, 0, 0, 0, 2988, 2989, 5, 76, 0, 0, 2989, 2990, 5, 79, 0, 0, 2990, 2991, 5, 79, 0, 0, 2991, 2992, 5, 75, 0, 0, 2992, 2993, 5, 70, 0, 0, 2993, 2994, 5, 76, 0, 0, 2994, 2995, 5, 79, 0, 0, 2995, 2996, 5, 79, 0, 0, 2996, 2997, 5, 82, 0, 0, 2997, 584, 1, 0, 0, 0, 2998, 2999, 5, 65, 0, 0, 2999, 3000, 5, 82, 0, 0, 3000, 3001, 5, 82, 0, 0, 3001, 3002, 5, 65, 0, 0, 3002, 3003, 5, 89, 0, 0, 3003, 586, 1, 0, 0, 0, 3004, 3005, 5, 65, 0, 0, 3005, 3006, 5, 76, 0, 0, 3006, 3007, 5, 71, 0, 0, 3007, 3008, 5, 79, 0, 0, 3008, 3009, 5, 82, 0, 0, 3009, 3010, 5, 73, 0, 0, 3010, 3011, 5, 84, 0, 0, 3011, 3012, 5, 72, 0, 0, 3012, 3013, 5, 77, 0, 0, 3013, 3014, 5, 86, 0, 0, 3014, 3015, 5, 69, 0, 0, 3015, 3016, 5, 82, 0, 0, 3016, 3017, 5, 83, 0, 0, 3017, 3018, 5, 73, 0, 0, 3018, 3019, 5, 79, 0, 0, 3019, 3034, 5, 78, 0, 0, 3020, 3021, 5, 69, 0, 0, 3021, 3022, 5, 78, 0, 0, 3022, 3023, 5, 71, 0, 0, 3023, 3024, 5, 73, 0, 0, 3024, 3025, 5, 78, 0, 0, 3025, 3026, 5, 69, 0, 0, 3026, 3027, 5, 86, 0, 0, 3027, 3028, 5, 69, 0, 0, 3028, 3029, 5, 82, 0, 0, 3029, 3030, 5, 83, 0, 0, 3030, 3031, 5, 73, 0, 0, 3031, 3032, 5, 79, 0, 0, 3032, 3034, 5, 78, 0, 0, 3033, 3004, 1, 0, 0, 0, 3033, 3020, 1, 0, 0, 0, 3034, 588, 1, 0, 0, 0, 3035, 3036, 5, 65, 0, 0, 3036, 3037, 5, 68, 0, 0, 3037, 3038, 5, 68, 0, 0, 3038, 3039, 5, 89, 0, 0, 3039, 3040, 5, 69, 0, 0, 3040, 3041, 5, 65, 0, 0, 3041, 3042, 5, 82, 0, 0, 3042, 3043, 5, 83, 0, 0, 3043, 590, 1, 0, 0, 0, 3044, 3045, 5, 65, 0, 0, 3045, 3046, 5, 68, 0, 0, 3046, 3047, 5, 68, 0, 0, 3047, 3048, 5, 77, 0, 0, 3048, 3049, 5, 79, 0, 0, 3049, 3050, 5, 78, 0, 0, 3050, 3051, 5, 84, 0, 0, 3051, 3052, 5, 72, 0, 0, 3052, 3053, 5, 83, 0, 0, 3053, 592, 1, 0, 0, 0, 3054, 3055, 5, 65, 0, 0, 3055, 3056, 5, 68, 0, 0, 3056, 3057, 5, 68, 0, 0, 3057, 3058, 5, 68, 0, 0, 3058, 3059, 5, 65, 0, 0, 3059, 3060, 5, 89, 0, 0, 3060, 3061, 5, 83, 0, 0, 3061, 594, 1, 0, 0, 0, 3062, 3063, 5, 65, 0, 0, 3063, 3064, 5, 68, 0, 0, 3064, 3065, 5, 68, 0, 0, 3065, 3066, 5, 72, 0, 0, 3066, 3067, 5, 79, 0, 0, 3067, 3068, 5, 85, 0, 0, 3068, 3069, 5, 82, 0, 0, 3069, 3070, 5, 83, 0, 0, 3070, 596, 1, 0, 0, 0, 3071, 3072, 5, 65, 0, 0, 3072, 3073, 5, 68, 0, 0, 3073, 3074, 5, 68, 0, 0, 3074, 3075, 5, 77, 0, 0, 3075, 3076, 5, 73, 0, 0, 3076, 3077, 5, 78, 0, 0, 3077, 3078, 5, 85, 0, 0, 3078, 3079, 5, 84, 0, 0, 3079, 3080, 5, 69, 0, 0, 3080, 3081, 5, 83, 0, 0, 3081, 598, 1, 0, 0, 0, 3082, 3083, 5, 65, 0, 0, 3083, 3084, 5, 68, 0, 0, 3084, 3085, 5, 68, 0, 0, 3085, 3086, 5, 83, 0, 0, 3086, 3087, 5, 69, 0, 0, 3087, 3088, 5, 67, 0, 0, 3088, 3089, 5, 79, 0, 0, 3089, 3090, 5, 78, 0, 0, 3090, 3091, 5, 68, 0, 0, 3091, 3092, 5, 83, 0, 0, 3092, 600, 1, 0, 0, 0, 3093, 3094, 5, 84, 0, 0, 3094, 3095, 5, 73, 0, 0, 3095, 3096, 5, 77, 0, 0, 3096, 3097, 5, 69, 0, 0, 3097, 3098, 5, 83, 0, 0, 3098, 3099, 5, 84, 0, 0, 3099, 3100, 5, 65, 0, 0, 3100, 3101, 5, 77, 0, 0, 3101, 3102, 5, 80, 0, 0, 3102, 602, 1, 0, 0, 0, 3103, 3104, 5, 72, 0, 0, 3104, 3105, 5, 65, 0, 0, 3105, 3132, 5, 83, 0, 0, 3106, 3107, 5, 72, 0, 0, 3107, 3108, 5, 65, 0, 0, 3108, 3109, 5, 83, 0, 0, 3109, 3110, 5, 75, 0, 0, 3110, 3111, 5, 69, 0, 0, 3111, 3132, 5, 89, 0, 0, 3112, 3113, 5, 67, 0, 0, 3113, 3114, 5, 79, 0, 0, 3114, 3115, 5, 78, 0, 0, 3115, 3116, 5, 84, 0, 0, 3116, 3117, 5, 65, 0, 0, 3117, 3118, 5, 73, 0, 0, 3118, 3119, 5, 78, 0, 0, 3119, 3132, 5, 83, 0, 0, 3120, 3121, 5, 67, 0, 0, 3121, 3122, 5, 79, 0, 0, 3122, 3123, 5, 78, 0, 0, 3123, 3124, 5, 84, 0, 0, 3124, 3125, 5, 65, 0, 0, 3125, 3126, 5, 73, 0, 0, 3126, 3127, 5, 78, 0, 0, 3127, 3128, 5, 83, 0, 0, 3128, 3129, 5, 75, 0, 0, 3129, 3130, 5, 69, 0, 0, 3130, 3132, 5, 89, 0, 0, 3131, 3103, 1, 0, 0, 0, 3131, 3106, 1, 0, 0, 0, 3131, 3112, 1, 0, 0, 0, 3131, 3120, 1, 0, 0, 0, 3132, 604, 1, 0, 0, 0, 3133, 3134, 5, 72, 0, 0, 3134, 3135, 5, 65, 0, 0, 3135, 3136, 5, 83, 0, 0, 3136, 3137, 5, 86, 0, 0, 3137, 3138, 5, 65, 0, 0, 3138, 3139, 5, 76, 0, 0, 3139, 3140, 5, 85, 0, 0, 3140, 3155, 5, 69, 0, 0, 3141, 3142, 5, 67, 0, 0, 3142, 3143, 5, 79, 0, 0, 3143, 3144, 5, 78, 0, 0, 3144, 3145, 5, 84, 0, 0, 3145, 3146, 5, 65, 0, 0, 3146, 3147, 5, 73, 0, 0, 3147, 3148, 5, 78, 0, 0, 3148, 3149, 5, 83, 0, 0, 3149, 3150, 5, 86, 0, 0, 3150, 3151, 5, 65, 0, 0, 3151, 3152, 5, 76, 0, 0, 3152, 3153, 5, 85, 0, 0, 3153, 3155, 5, 69, 0, 0, 3154, 3133, 1, 0, 0, 0, 3154, 3141, 1, 0, 0, 0, 3155, 606, 1, 0, 0, 0, 3156, 3157, 5, 80, 0, 0, 3157, 3158, 5, 65, 0, 0, 3158, 3159, 5, 82, 0, 0, 3159, 3160, 5, 65, 0, 0, 3160, 3183, 5, 77, 0, 0, 3161, 3162, 5, 80, 0, 0, 3162, 3163, 5, 65, 0, 0, 3163, 3164, 5, 82, 0, 0, 3164, 3165, 5, 65, 0, 0, 3165, 3166, 5, 77, 0, 0, 3166, 3167, 5, 69, 0, 0, 3167, 3168, 5, 84, 0, 0, 3168, 3169, 5, 69, 0, 0, 3169, 3183, 5, 82, 0, 0, 3170, 3171, 5, 71, 0, 0, 3171, 3172, 5, 69, 0, 0, 3172, 3173, 5, 84, 0, 0, 3173, 3174, 5, 80, 0, 0, 3174, 3175, 5, 65, 0, 0, 3175, 3176, 5, 82, 0, 0, 3176, 3177, 5, 65, 0, 0, 3177, 3178, 5, 77, 0, 0, 3178, 3179, 5, 69, 0, 0, 3179, 3180, 5, 84, 0, 0, 3180, 3181, 5, 69, 0, 0, 3181, 3183, 5, 82, 0, 0, 3182, 3156, 1, 0, 0, 0, 3182, 3161, 1, 0, 0, 0, 3182, 3170, 1, 0, 0, 0, 3183, 608, 1, 0, 0, 0, 3184, 3187, 7, 6, 0, 0, 3185, 3187, 3, 611, 305, 0, 3186, 3184, 1, 0, 0, 0, 3186, 3185, 1, 0, 0, 0, 3187, 3192, 1, 0, 0, 0, 3188, 3191, 7, 7, 0, 0, 3189, 3191, 3, 611, 305, 0, 3190, 3188, 1, 0, 0, 0, 3190, 3189, 1, 0, 0, 0, 3191, 3194, 1, 0, 0, 0, 3192, 3190, 1, 0, 0, 0, 3192, 3193, 1, 0, 0, 0, 3193, 610, 1, 0, 0, 0, 3194, 3192, 1, 0, 0, 0, 3195, 3196, 7, 8, 0, 0, 3196, 612, 1, 0, 0, 0, 3197, 3199, 7, 9, 0, 0, 3198, 3197, 1, 0, 0, 0, 3199, 3200, 1, 0, 0, 0, 3200, 3198, 1, 0, 0, 0, 3200, 3201, 1, 0, 0, 0, 3201, 3202, 1, 0, 0, 0, 3202, 3203, 6, 306, 0, 0, 3203, 614, 1, 0, 0, 0, 3204, 3205, 5, 47, 0, 0, 3205, 3206, 5, 42, 0, 0, 3206, 3210, 1, 0, 0, 0, 3207, 3209, 9, 0, 0, 0, 3208, 3207, 1, 0, 0, 0, 3209, 3212, 1, 0, 0, 0, 3210, 3211, 1, 0, 0, 0, 3210, 3208, 1, 0, 0, 0, 3211, 3213, 1, 0, 0, 0, 3212, 3210, 1, 0, 0, 0, 3213, 3214, 5, 42, 0, 0, 3214, 3215, 5, 47, 0, 0, 3215, 3216, 1, 0, 0, 0, 3216, 3217, 6, 307, 0, 0, 3217, 616, 1, 0, 0, 0, 3218, 3219, 5, 47, 0, 0, 3219, 3220, 5, 47, 0, 0, 3220, 3224, 1, 0, 0, 0, 3221, 3223, 8, 10, 0, 0, 3222, 3221, 1, 0, 0, 0, 3223, 3226, 1, 0, 0, 0, 3224, 3222, 1, 0, 0, 0, 3224, 3225, 1, 0, 0, 0, 3225, 3227, 1, 0, 0, 0, 3226, 3224, 1, 0, 0, 0, 3227, 3228, 6, 308, 0, 0, 3228, 618, 1, 0, 0, 0, 69, 0, 693, 695, 701, 708, 710, 717, 719, 725, 732, 734, 736, 740, 744, 746, 752, 754, 762, 764, 772, 774, 778, 834, 958, 967, 1575, 1626, 1666, 1744, 1962, 1990, 2069, 2084, 2103, 2132, 2143, 2162, 2179, 2201, 2221, 2240, 2257, 2278, 2299, 2312, 2323, 2361, 2380, 2404, 2429, 2448, 2473, 2500, 2521, 2534, 2545, 2756, 2839, 2853, 3033, 3131, 3154, 3182, 3186, 3190, 3192, 3200, 3210, 3224, 1, 6, 0, 0];
const mathjsLexer_atn = new index_web.atn.ATNDeserializer().deserialize(serializedATN);
const decisionsToDFA = mathjsLexer_atn.decisionToState.map((ds, index) => new index_web.dfa.DFA(ds, index));
class mathjsLexer extends index_web.Lexer {
  static grammarFileName = "";
  static channelNames = [];
  static modeNames = [];
  static literalNames = [];
  static symbolicNames = [];
  static ruleNames = [];
  constructor(input) {
    super(input);
    this._interp = new index_web.atn.LexerATNSimulator(this, mathjsLexer_atn, decisionsToDFA, new index_web.atn.PredictionContextCache());
  }
}
;// ./src/math/mathjsVisitor.js
// Generated from mathjs.g4 by ANTLR 4.13.2
// jshint ignore: start


// This class defines a complete generic visitor for a parse tree produced by mathjsParser.

class mathjsVisitor extends index_web.tree.ParseTreeVisitor {
  // Visit a parse tree produced by mathjsParser#prog.
  visitProg(ctx) {
    return this.visitChildren(ctx);
  }

  // Visit a parse tree produced by mathjsParser#expr.
  visitExpr(ctx) {
    return this.visitChildren(ctx);
  }

  // Visit a parse tree produced by mathjsParser#num.
  visitNum(ctx) {
    return this.visitChildren(ctx);
  }

  // Visit a parse tree produced by mathjsParser#arrayJson.
  visitArrayJson(ctx) {
    return this.visitChildren(ctx);
  }

  // Visit a parse tree produced by mathjsParser#parameter2.
  visitParameter2(ctx) {
    return this.visitChildren(ctx);
  }
}
;// ./src/math/mathjsParser.js


const mathjsParser_serializedATN = [4, 1, 308, 508, 2, 0, 7, 0, 2, 1, 7, 1, 2, 2, 7, 2, 2, 3, 7, 3, 2, 4, 7, 4, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 26, 8, 1, 10, 1, 12, 1, 29, 9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 40, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 54, 8, 1, 10, 1, 12, 1, 57, 9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 70, 8, 1, 10, 1, 12, 1, 73, 9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 87, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 96, 8, 1, 10, 1, 12, 1, 99, 9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 106, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 113, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 156, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 173, 8, 1, 3, 1, 175, 8, 1, 3, 1, 177, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 197, 8, 1, 3, 1, 199, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 216, 8, 1, 3, 1, 218, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 235, 8, 1, 3, 1, 237, 8, 1, 3, 1, 239, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 252, 8, 1, 3, 1, 254, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 263, 8, 1, 11, 1, 12, 1, 264, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 276, 8, 1, 3, 1, 278, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 287, 8, 1, 10, 1, 12, 1, 290, 9, 1, 3, 1, 292, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 298, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 305, 8, 1, 10, 1, 12, 1, 308, 9, 1, 1, 1, 5, 1, 311, 8, 1, 10, 1, 12, 1, 314, 9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 322, 8, 1, 10, 1, 12, 1, 325, 9, 1, 1, 1, 5, 1, 328, 8, 1, 10, 1, 12, 1, 331, 9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 339, 8, 1, 1, 1, 1, 1, 3, 1, 343, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 379, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 394, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 414, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 425, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 438, 8, 1, 3, 1, 440, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 451, 8, 1, 10, 1, 12, 1, 454, 9, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 465, 8, 1, 10, 1, 12, 1, 468, 9, 1, 3, 1, 470, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, 487, 8, 1, 10, 1, 12, 1, 490, 9, 1, 1, 2, 3, 2, 493, 8, 2, 1, 2, 1, 2, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 3, 3, 504, 8, 3, 1, 4, 1, 4, 1, 4, 0, 1, 2, 5, 0, 2, 4, 6, 8, 0, 27, 3, 0, 35, 35, 38, 38, 175, 175, 26, 0, 39, 40, 42, 45, 51, 51, 68, 68, 71, 72, 74, 74, 79, 100, 107, 108, 112, 113, 115, 116, 118, 118, 121, 123, 135, 135, 145, 151, 157, 158, 160, 160, 164, 164, 167, 167, 169, 171, 173, 173, 220, 221, 228, 229, 232, 232, 257, 264, 286, 287, 290, 290, 24, 0, 41, 41, 46, 47, 73, 73, 102, 102, 105, 106, 117, 117, 128, 129, 136, 136, 138, 138, 154, 156, 163, 163, 165, 166, 172, 172, 184, 184, 187, 187, 190, 192, 203, 203, 209, 209, 250, 250, 252, 252, 254, 255, 277, 278, 301, 301, 304, 304, 12, 0, 48, 50, 75, 76, 119, 120, 130, 131, 152, 152, 193, 195, 197, 197, 202, 202, 204, 208, 210, 212, 215, 217, 248, 248, 1, 0, 52, 55, 1, 0, 56, 67, 35, 0, 69, 70, 77, 78, 101, 101, 103, 104, 109, 109, 111, 111, 114, 114, 124, 127, 132, 134, 139, 144, 153, 153, 162, 162, 168, 168, 186, 186, 188, 189, 196, 196, 198, 201, 213, 214, 219, 219, 222, 223, 225, 227, 231, 231, 234, 234, 239, 239, 249, 249, 251, 251, 253, 253, 256, 256, 265, 265, 267, 267, 273, 276, 281, 281, 291, 292, 295, 300, 302, 303, 3, 0, 110, 110, 176, 177, 268, 268, 3, 0, 159, 159, 185, 185, 266, 266, 8, 0, 161, 161, 218, 218, 224, 224, 230, 230, 233, 233, 235, 238, 240, 240, 283, 285, 1, 0, 178, 183, 2, 0, 241, 241, 244, 246, 1, 0, 242, 243, 1, 0, 279, 280, 1, 0, 288, 289, 2, 0, 34, 34, 167, 167, 1, 0, 8, 10, 2, 0, 11, 12, 29, 29, 1, 0, 13, 16, 1, 0, 17, 22, 12, 0, 39, 40, 42, 43, 74, 74, 157, 158, 164, 164, 167, 167, 169, 171, 173, 173, 257, 258, 269, 272, 286, 287, 290, 290, 7, 0, 41, 41, 46, 47, 156, 156, 163, 163, 172, 172, 277, 278, 301, 301, 7, 0, 153, 153, 168, 168, 265, 265, 267, 267, 281, 281, 295, 300, 302, 303, 2, 0, 159, 159, 266, 266, 2, 0, 283, 285, 288, 289, 1, 0, 30, 31, 2, 0, 32, 292, 294, 305, 598, 0, 10, 1, 0, 0, 0, 2, 342, 1, 0, 0, 0, 4, 492, 1, 0, 0, 0, 6, 503, 1, 0, 0, 0, 8, 505, 1, 0, 0, 0, 10, 11, 3, 2, 1, 0, 11, 12, 5, 0, 0, 1, 12, 1, 1, 0, 0, 0, 13, 14, 6, 1, -1, 0, 14, 15, 5, 2, 0, 0, 15, 16, 3, 2, 1, 0, 16, 17, 5, 3, 0, 0, 17, 343, 1, 0, 0, 0, 18, 19, 5, 7, 0, 0, 19, 343, 3, 2, 1, 40, 20, 21, 5, 293, 0, 0, 21, 22, 5, 2, 0, 0, 22, 27, 3, 2, 1, 0, 23, 24, 5, 4, 0, 0, 24, 26, 3, 2, 1, 0, 25, 23, 1, 0, 0, 0, 26, 29, 1, 0, 0, 0, 27, 25, 1, 0, 0, 0, 27, 28, 1, 0, 0, 0, 28, 30, 1, 0, 0, 0, 29, 27, 1, 0, 0, 0, 30, 31, 5, 3, 0, 0, 31, 343, 1, 0, 0, 0, 32, 33, 7, 0, 0, 0, 33, 34, 5, 2, 0, 0, 34, 35, 3, 2, 1, 0, 35, 36, 5, 4, 0, 0, 36, 39, 3, 2, 1, 0, 37, 38, 5, 4, 0, 0, 38, 40, 3, 2, 1, 0, 39, 37, 1, 0, 0, 0, 39, 40, 1, 0, 0, 0, 40, 41, 1, 0, 0, 0, 41, 42, 5, 3, 0, 0, 42, 343, 1, 0, 0, 0, 43, 44, 5, 36, 0, 0, 44, 45, 5, 2, 0, 0, 45, 46, 3, 2, 1, 0, 46, 47, 5, 4, 0, 0, 47, 55, 3, 2, 1, 0, 48, 49, 5, 4, 0, 0, 49, 50, 3, 2, 1, 0, 50, 51, 5, 4, 0, 0, 51, 52, 3, 2, 1, 0, 52, 54, 1, 0, 0, 0, 53, 48, 1, 0, 0, 0, 54, 57, 1, 0, 0, 0, 55, 53, 1, 0, 0, 0, 55, 56, 1, 0, 0, 0, 56, 58, 1, 0, 0, 0, 57, 55, 1, 0, 0, 0, 58, 59, 5, 3, 0, 0, 59, 343, 1, 0, 0, 0, 60, 61, 5, 37, 0, 0, 61, 62, 5, 2, 0, 0, 62, 63, 3, 2, 1, 0, 63, 64, 5, 4, 0, 0, 64, 65, 3, 2, 1, 0, 65, 66, 5, 4, 0, 0, 66, 71, 3, 2, 1, 0, 67, 68, 5, 4, 0, 0, 68, 70, 3, 2, 1, 0, 69, 67, 1, 0, 0, 0, 70, 73, 1, 0, 0, 0, 71, 69, 1, 0, 0, 0, 71, 72, 1, 0, 0, 0, 72, 74, 1, 0, 0, 0, 73, 71, 1, 0, 0, 0, 74, 75, 5, 3, 0, 0, 75, 343, 1, 0, 0, 0, 76, 77, 7, 1, 0, 0, 77, 78, 5, 2, 0, 0, 78, 79, 3, 2, 1, 0, 79, 80, 5, 3, 0, 0, 80, 343, 1, 0, 0, 0, 81, 82, 7, 2, 0, 0, 82, 83, 5, 2, 0, 0, 83, 86, 3, 2, 1, 0, 84, 85, 5, 4, 0, 0, 85, 87, 3, 2, 1, 0, 86, 84, 1, 0, 0, 0, 86, 87, 1, 0, 0, 0, 87, 88, 1, 0, 0, 0, 88, 89, 5, 3, 0, 0, 89, 343, 1, 0, 0, 0, 90, 91, 7, 3, 0, 0, 91, 92, 5, 2, 0, 0, 92, 97, 3, 2, 1, 0, 93, 94, 5, 4, 0, 0, 94, 96, 3, 2, 1, 0, 95, 93, 1, 0, 0, 0, 96, 99, 1, 0, 0, 0, 97, 95, 1, 0, 0, 0, 97, 98, 1, 0, 0, 0, 98, 100, 1, 0, 0, 0, 99, 97, 1, 0, 0, 0, 100, 101, 5, 3, 0, 0, 101, 343, 1, 0, 0, 0, 102, 105, 7, 4, 0, 0, 103, 104, 5, 2, 0, 0, 104, 106, 5, 3, 0, 0, 105, 103, 1, 0, 0, 0, 105, 106, 1, 0, 0, 0, 106, 343, 1, 0, 0, 0, 107, 108, 7, 5, 0, 0, 108, 109, 5, 2, 0, 0, 109, 112, 3, 2, 1, 0, 110, 111, 5, 4, 0, 0, 111, 113, 3, 2, 1, 0, 112, 110, 1, 0, 0, 0, 112, 113, 1, 0, 0, 0, 113, 114, 1, 0, 0, 0, 114, 115, 5, 3, 0, 0, 115, 343, 1, 0, 0, 0, 116, 117, 7, 6, 0, 0, 117, 118, 5, 2, 0, 0, 118, 119, 3, 2, 1, 0, 119, 120, 5, 4, 0, 0, 120, 121, 3, 2, 1, 0, 121, 122, 5, 3, 0, 0, 122, 343, 1, 0, 0, 0, 123, 124, 7, 7, 0, 0, 124, 125, 5, 2, 0, 0, 125, 343, 5, 3, 0, 0, 126, 127, 5, 137, 0, 0, 127, 128, 5, 2, 0, 0, 128, 129, 3, 2, 1, 0, 129, 130, 5, 4, 0, 0, 130, 131, 3, 2, 1, 0, 131, 132, 5, 4, 0, 0, 132, 133, 3, 2, 1, 0, 133, 134, 5, 4, 0, 0, 134, 135, 3, 2, 1, 0, 135, 136, 5, 3, 0, 0, 136, 343, 1, 0, 0, 0, 137, 138, 7, 8, 0, 0, 138, 139, 5, 2, 0, 0, 139, 140, 3, 2, 1, 0, 140, 141, 5, 4, 0, 0, 141, 142, 3, 2, 1, 0, 142, 143, 5, 4, 0, 0, 143, 144, 3, 2, 1, 0, 144, 145, 5, 3, 0, 0, 145, 343, 1, 0, 0, 0, 146, 147, 7, 9, 0, 0, 147, 148, 5, 2, 0, 0, 148, 149, 3, 2, 1, 0, 149, 150, 5, 4, 0, 0, 150, 151, 3, 2, 1, 0, 151, 152, 5, 4, 0, 0, 152, 155, 3, 2, 1, 0, 153, 154, 5, 4, 0, 0, 154, 156, 3, 2, 1, 0, 155, 153, 1, 0, 0, 0, 155, 156, 1, 0, 0, 0, 156, 157, 1, 0, 0, 0, 157, 158, 5, 3, 0, 0, 158, 343, 1, 0, 0, 0, 159, 160, 5, 174, 0, 0, 160, 161, 5, 2, 0, 0, 161, 162, 3, 2, 1, 0, 162, 163, 5, 4, 0, 0, 163, 164, 3, 2, 1, 0, 164, 165, 5, 4, 0, 0, 165, 176, 3, 2, 1, 0, 166, 167, 5, 4, 0, 0, 167, 174, 3, 2, 1, 0, 168, 169, 5, 4, 0, 0, 169, 172, 3, 2, 1, 0, 170, 171, 5, 4, 0, 0, 171, 173, 3, 2, 1, 0, 172, 170, 1, 0, 0, 0, 172, 173, 1, 0, 0, 0, 173, 175, 1, 0, 0, 0, 174, 168, 1, 0, 0, 0, 174, 175, 1, 0, 0, 0, 175, 177, 1, 0, 0, 0, 176, 166, 1, 0, 0, 0, 176, 177, 1, 0, 0, 0, 177, 178, 1, 0, 0, 0, 178, 179, 5, 3, 0, 0, 179, 343, 1, 0, 0, 0, 180, 181, 7, 10, 0, 0, 181, 182, 5, 2, 0, 0, 182, 183, 3, 2, 1, 0, 183, 184, 5, 3, 0, 0, 184, 343, 1, 0, 0, 0, 185, 186, 7, 11, 0, 0, 186, 187, 5, 2, 0, 0, 187, 188, 3, 2, 1, 0, 188, 189, 5, 4, 0, 0, 189, 190, 3, 2, 1, 0, 190, 191, 5, 4, 0, 0, 191, 198, 3, 2, 1, 0, 192, 193, 5, 4, 0, 0, 193, 196, 3, 2, 1, 0, 194, 195, 5, 4, 0, 0, 195, 197, 3, 2, 1, 0, 196, 194, 1, 0, 0, 0, 196, 197, 1, 0, 0, 0, 197, 199, 1, 0, 0, 0, 198, 192, 1, 0, 0, 0, 198, 199, 1, 0, 0, 0, 199, 200, 1, 0, 0, 0, 200, 201, 5, 3, 0, 0, 201, 343, 1, 0, 0, 0, 202, 203, 7, 12, 0, 0, 203, 204, 5, 2, 0, 0, 204, 205, 3, 2, 1, 0, 205, 206, 5, 4, 0, 0, 206, 207, 3, 2, 1, 0, 207, 208, 5, 4, 0, 0, 208, 209, 3, 2, 1, 0, 209, 210, 5, 4, 0, 0, 210, 217, 3, 2, 1, 0, 211, 212, 5, 4, 0, 0, 212, 215, 3, 2, 1, 0, 213, 214, 5, 4, 0, 0, 214, 216, 3, 2, 1, 0, 215, 213, 1, 0, 0, 0, 215, 216, 1, 0, 0, 0, 216, 218, 1, 0, 0, 0, 217, 211, 1, 0, 0, 0, 217, 218, 1, 0, 0, 0, 218, 219, 1, 0, 0, 0, 219, 220, 5, 3, 0, 0, 220, 343, 1, 0, 0, 0, 221, 222, 5, 247, 0, 0, 222, 223, 5, 2, 0, 0, 223, 224, 3, 2, 1, 0, 224, 225, 5, 4, 0, 0, 225, 226, 3, 2, 1, 0, 226, 227, 5, 4, 0, 0, 227, 238, 3, 2, 1, 0, 228, 229, 5, 4, 0, 0, 229, 236, 3, 2, 1, 0, 230, 231, 5, 4, 0, 0, 231, 234, 3, 2, 1, 0, 232, 233, 5, 4, 0, 0, 233, 235, 3, 2, 1, 0, 234, 232, 1, 0, 0, 0, 234, 235, 1, 0, 0, 0, 235, 237, 1, 0, 0, 0, 236, 230, 1, 0, 0, 0, 236, 237, 1, 0, 0, 0, 237, 239, 1, 0, 0, 0, 238, 228, 1, 0, 0, 0, 238, 239, 1, 0, 0, 0, 239, 240, 1, 0, 0, 0, 240, 241, 5, 3, 0, 0, 241, 343, 1, 0, 0, 0, 242, 243, 7, 13, 0, 0, 243, 244, 5, 2, 0, 0, 244, 245, 3, 2, 1, 0, 245, 246, 5, 4, 0, 0, 246, 253, 3, 2, 1, 0, 247, 248, 5, 4, 0, 0, 248, 251, 3, 2, 1, 0, 249, 250, 5, 4, 0, 0, 250, 252, 3, 2, 1, 0, 251, 249, 1, 0, 0, 0, 251, 252, 1, 0, 0, 0, 252, 254, 1, 0, 0, 0, 253, 247, 1, 0, 0, 0, 253, 254, 1, 0, 0, 0, 254, 255, 1, 0, 0, 0, 255, 256, 5, 3, 0, 0, 256, 343, 1, 0, 0, 0, 257, 258, 5, 282, 0, 0, 258, 259, 5, 2, 0, 0, 259, 262, 3, 2, 1, 0, 260, 261, 5, 4, 0, 0, 261, 263, 3, 2, 1, 0, 262, 260, 1, 0, 0, 0, 263, 264, 1, 0, 0, 0, 264, 262, 1, 0, 0, 0, 264, 265, 1, 0, 0, 0, 265, 266, 1, 0, 0, 0, 266, 267, 5, 3, 0, 0, 267, 343, 1, 0, 0, 0, 268, 269, 7, 14, 0, 0, 269, 270, 5, 2, 0, 0, 270, 277, 3, 2, 1, 0, 271, 272, 5, 4, 0, 0, 272, 275, 3, 2, 1, 0, 273, 274, 5, 4, 0, 0, 274, 276, 3, 2, 1, 0, 275, 273, 1, 0, 0, 0, 275, 276, 1, 0, 0, 0, 276, 278, 1, 0, 0, 0, 277, 271, 1, 0, 0, 0, 277, 278, 1, 0, 0, 0, 278, 279, 1, 0, 0, 0, 279, 280, 5, 3, 0, 0, 280, 343, 1, 0, 0, 0, 281, 282, 5, 305, 0, 0, 282, 291, 5, 2, 0, 0, 283, 288, 3, 2, 1, 0, 284, 285, 5, 4, 0, 0, 285, 287, 3, 2, 1, 0, 286, 284, 1, 0, 0, 0, 287, 290, 1, 0, 0, 0, 288, 286, 1, 0, 0, 0, 288, 289, 1, 0, 0, 0, 289, 292, 1, 0, 0, 0, 290, 288, 1, 0, 0, 0, 291, 283, 1, 0, 0, 0, 291, 292, 1, 0, 0, 0, 292, 293, 1, 0, 0, 0, 293, 343, 5, 3, 0, 0, 294, 295, 5, 33, 0, 0, 295, 297, 5, 2, 0, 0, 296, 298, 3, 2, 1, 0, 297, 296, 1, 0, 0, 0, 297, 298, 1, 0, 0, 0, 298, 299, 1, 0, 0, 0, 299, 343, 5, 3, 0, 0, 300, 301, 5, 27, 0, 0, 301, 306, 3, 6, 3, 0, 302, 303, 5, 4, 0, 0, 303, 305, 3, 6, 3, 0, 304, 302, 1, 0, 0, 0, 305, 308, 1, 0, 0, 0, 306, 304, 1, 0, 0, 0, 306, 307, 1, 0, 0, 0, 307, 312, 1, 0, 0, 0, 308, 306, 1, 0, 0, 0, 309, 311, 5, 4, 0, 0, 310, 309, 1, 0, 0, 0, 311, 314, 1, 0, 0, 0, 312, 310, 1, 0, 0, 0, 312, 313, 1, 0, 0, 0, 313, 315, 1, 0, 0, 0, 314, 312, 1, 0, 0, 0, 315, 316, 5, 28, 0, 0, 316, 343, 1, 0, 0, 0, 317, 318, 5, 5, 0, 0, 318, 323, 3, 2, 1, 0, 319, 320, 5, 4, 0, 0, 320, 322, 3, 2, 1, 0, 321, 319, 1, 0, 0, 0, 322, 325, 1, 0, 0, 0, 323, 321, 1, 0, 0, 0, 323, 324, 1, 0, 0, 0, 324, 329, 1, 0, 0, 0, 325, 323, 1, 0, 0, 0, 326, 328, 5, 4, 0, 0, 327, 326, 1, 0, 0, 0, 328, 331, 1, 0, 0, 0, 329, 327, 1, 0, 0, 0, 329, 330, 1, 0, 0, 0, 330, 332, 1, 0, 0, 0, 331, 329, 1, 0, 0, 0, 332, 333, 5, 6, 0, 0, 333, 343, 1, 0, 0, 0, 334, 343, 5, 294, 0, 0, 335, 343, 5, 305, 0, 0, 336, 338, 3, 4, 2, 0, 337, 339, 7, 15, 0, 0, 338, 337, 1, 0, 0, 0, 338, 339, 1, 0, 0, 0, 339, 343, 1, 0, 0, 0, 340, 343, 5, 31, 0, 0, 341, 343, 5, 32, 0, 0, 342, 13, 1, 0, 0, 0, 342, 18, 1, 0, 0, 0, 342, 20, 1, 0, 0, 0, 342, 32, 1, 0, 0, 0, 342, 43, 1, 0, 0, 0, 342, 60, 1, 0, 0, 0, 342, 76, 1, 0, 0, 0, 342, 81, 1, 0, 0, 0, 342, 90, 1, 0, 0, 0, 342, 102, 1, 0, 0, 0, 342, 107, 1, 0, 0, 0, 342, 116, 1, 0, 0, 0, 342, 123, 1, 0, 0, 0, 342, 126, 1, 0, 0, 0, 342, 137, 1, 0, 0, 0, 342, 146, 1, 0, 0, 0, 342, 159, 1, 0, 0, 0, 342, 180, 1, 0, 0, 0, 342, 185, 1, 0, 0, 0, 342, 202, 1, 0, 0, 0, 342, 221, 1, 0, 0, 0, 342, 242, 1, 0, 0, 0, 342, 257, 1, 0, 0, 0, 342, 268, 1, 0, 0, 0, 342, 281, 1, 0, 0, 0, 342, 294, 1, 0, 0, 0, 342, 300, 1, 0, 0, 0, 342, 317, 1, 0, 0, 0, 342, 334, 1, 0, 0, 0, 342, 335, 1, 0, 0, 0, 342, 336, 1, 0, 0, 0, 342, 340, 1, 0, 0, 0, 342, 341, 1, 0, 0, 0, 343, 488, 1, 0, 0, 0, 344, 345, 10, 38, 0, 0, 345, 346, 7, 16, 0, 0, 346, 487, 3, 2, 1, 39, 347, 348, 10, 37, 0, 0, 348, 349, 7, 17, 0, 0, 349, 487, 3, 2, 1, 38, 350, 351, 10, 36, 0, 0, 351, 352, 7, 18, 0, 0, 352, 487, 3, 2, 1, 37, 353, 354, 10, 35, 0, 0, 354, 355, 7, 19, 0, 0, 355, 487, 3, 2, 1, 36, 356, 357, 10, 34, 0, 0, 357, 358, 5, 23, 0, 0, 358, 487, 3, 2, 1, 35, 359, 360, 10, 33, 0, 0, 360, 361, 5, 24, 0, 0, 361, 487, 3, 2, 1, 34, 362, 363, 10, 32, 0, 0, 363, 364, 5, 25, 0, 0, 364, 365, 3, 2, 1, 0, 365, 366, 5, 26, 0, 0, 366, 367, 3, 2, 1, 33, 367, 487, 1, 0, 0, 0, 368, 369, 10, 54, 0, 0, 369, 370, 5, 1, 0, 0, 370, 371, 7, 20, 0, 0, 371, 372, 5, 2, 0, 0, 372, 487, 5, 3, 0, 0, 373, 374, 10, 53, 0, 0, 374, 375, 5, 1, 0, 0, 375, 376, 7, 21, 0, 0, 376, 378, 5, 2, 0, 0, 377, 379, 3, 2, 1, 0, 378, 377, 1, 0, 0, 0, 378, 379, 1, 0, 0, 0, 379, 380, 1, 0, 0, 0, 380, 487, 5, 3, 0, 0, 381, 382, 10, 52, 0, 0, 382, 383, 5, 1, 0, 0, 383, 384, 7, 22, 0, 0, 384, 385, 5, 2, 0, 0, 385, 386, 3, 2, 1, 0, 386, 387, 5, 3, 0, 0, 387, 487, 1, 0, 0, 0, 388, 389, 10, 51, 0, 0, 389, 390, 5, 1, 0, 0, 390, 393, 7, 10, 0, 0, 391, 392, 5, 2, 0, 0, 392, 394, 5, 3, 0, 0, 393, 391, 1, 0, 0, 0, 393, 394, 1, 0, 0, 0, 394, 487, 1, 0, 0, 0, 395, 396, 10, 50, 0, 0, 396, 397, 5, 1, 0, 0, 397, 398, 7, 23, 0, 0, 398, 399, 5, 2, 0, 0, 399, 400, 3, 2, 1, 0, 400, 401, 5, 4, 0, 0, 401, 402, 3, 2, 1, 0, 402, 403, 5, 3, 0, 0, 403, 487, 1, 0, 0, 0, 404, 405, 10, 49, 0, 0, 405, 406, 5, 1, 0, 0, 406, 407, 5, 161, 0, 0, 407, 408, 5, 2, 0, 0, 408, 409, 3, 2, 1, 0, 409, 410, 5, 4, 0, 0, 410, 413, 3, 2, 1, 0, 411, 412, 5, 4, 0, 0, 412, 414, 3, 2, 1, 0, 413, 411, 1, 0, 0, 0, 413, 414, 1, 0, 0, 0, 414, 415, 1, 0, 0, 0, 415, 416, 5, 3, 0, 0, 416, 487, 1, 0, 0, 0, 417, 418, 10, 48, 0, 0, 418, 419, 5, 1, 0, 0, 419, 420, 7, 24, 0, 0, 420, 421, 5, 2, 0, 0, 421, 424, 3, 2, 1, 0, 422, 423, 5, 4, 0, 0, 423, 425, 3, 2, 1, 0, 424, 422, 1, 0, 0, 0, 424, 425, 1, 0, 0, 0, 425, 426, 1, 0, 0, 0, 426, 427, 5, 3, 0, 0, 427, 487, 1, 0, 0, 0, 428, 429, 10, 47, 0, 0, 429, 430, 5, 1, 0, 0, 430, 431, 7, 13, 0, 0, 431, 432, 5, 2, 0, 0, 432, 439, 3, 2, 1, 0, 433, 434, 5, 4, 0, 0, 434, 437, 3, 2, 1, 0, 435, 436, 5, 4, 0, 0, 436, 438, 3, 2, 1, 0, 437, 435, 1, 0, 0, 0, 437, 438, 1, 0, 0, 0, 438, 440, 1, 0, 0, 0, 439, 433, 1, 0, 0, 0, 439, 440, 1, 0, 0, 0, 440, 441, 1, 0, 0, 0, 441, 442, 5, 3, 0, 0, 442, 487, 1, 0, 0, 0, 443, 444, 10, 46, 0, 0, 444, 445, 5, 1, 0, 0, 445, 446, 5, 282, 0, 0, 446, 447, 5, 2, 0, 0, 447, 452, 3, 2, 1, 0, 448, 449, 5, 4, 0, 0, 449, 451, 3, 2, 1, 0, 450, 448, 1, 0, 0, 0, 451, 454, 1, 0, 0, 0, 452, 450, 1, 0, 0, 0, 452, 453, 1, 0, 0, 0, 453, 455, 1, 0, 0, 0, 454, 452, 1, 0, 0, 0, 455, 456, 5, 3, 0, 0, 456, 487, 1, 0, 0, 0, 457, 458, 10, 45, 0, 0, 458, 459, 5, 1, 0, 0, 459, 460, 5, 305, 0, 0, 460, 469, 5, 2, 0, 0, 461, 466, 3, 2, 1, 0, 462, 463, 5, 4, 0, 0, 463, 465, 3, 2, 1, 0, 464, 462, 1, 0, 0, 0, 465, 468, 1, 0, 0, 0, 466, 464, 1, 0, 0, 0, 466, 467, 1, 0, 0, 0, 467, 470, 1, 0, 0, 0, 468, 466, 1, 0, 0, 0, 469, 461, 1, 0, 0, 0, 469, 470, 1, 0, 0, 0, 470, 471, 1, 0, 0, 0, 471, 487, 5, 3, 0, 0, 472, 473, 10, 44, 0, 0, 473, 474, 5, 5, 0, 0, 474, 475, 5, 305, 0, 0, 475, 487, 5, 6, 0, 0, 476, 477, 10, 43, 0, 0, 477, 478, 5, 5, 0, 0, 478, 479, 3, 2, 1, 0, 479, 480, 5, 6, 0, 0, 480, 487, 1, 0, 0, 0, 481, 482, 10, 42, 0, 0, 482, 483, 5, 1, 0, 0, 483, 487, 3, 8, 4, 0, 484, 485, 10, 39, 0, 0, 485, 487, 5, 8, 0, 0, 486, 344, 1, 0, 0, 0, 486, 347, 1, 0, 0, 0, 486, 350, 1, 0, 0, 0, 486, 353, 1, 0, 0, 0, 486, 356, 1, 0, 0, 0, 486, 359, 1, 0, 0, 0, 486, 362, 1, 0, 0, 0, 486, 368, 1, 0, 0, 0, 486, 373, 1, 0, 0, 0, 486, 381, 1, 0, 0, 0, 486, 388, 1, 0, 0, 0, 486, 395, 1, 0, 0, 0, 486, 404, 1, 0, 0, 0, 486, 417, 1, 0, 0, 0, 486, 428, 1, 0, 0, 0, 486, 443, 1, 0, 0, 0, 486, 457, 1, 0, 0, 0, 486, 472, 1, 0, 0, 0, 486, 476, 1, 0, 0, 0, 486, 481, 1, 0, 0, 0, 486, 484, 1, 0, 0, 0, 487, 490, 1, 0, 0, 0, 488, 486, 1, 0, 0, 0, 488, 489, 1, 0, 0, 0, 489, 3, 1, 0, 0, 0, 490, 488, 1, 0, 0, 0, 491, 493, 5, 29, 0, 0, 492, 491, 1, 0, 0, 0, 492, 493, 1, 0, 0, 0, 493, 494, 1, 0, 0, 0, 494, 495, 5, 30, 0, 0, 495, 5, 1, 0, 0, 0, 496, 497, 7, 25, 0, 0, 497, 498, 5, 26, 0, 0, 498, 504, 3, 2, 1, 0, 499, 500, 3, 8, 4, 0, 500, 501, 5, 26, 0, 0, 501, 502, 3, 2, 1, 0, 502, 504, 1, 0, 0, 0, 503, 496, 1, 0, 0, 0, 503, 499, 1, 0, 0, 0, 504, 7, 1, 0, 0, 0, 505, 506, 7, 26, 0, 0, 506, 9, 1, 0, 0, 0, 46, 27, 39, 55, 71, 86, 97, 105, 112, 155, 172, 174, 176, 196, 198, 215, 217, 234, 236, 238, 251, 253, 264, 275, 277, 288, 291, 297, 306, 312, 323, 329, 338, 342, 378, 393, 413, 424, 437, 439, 452, 466, 469, 486, 488, 492, 503];
const mathjsParser_atn = new index_web.atn.ATNDeserializer().deserialize(mathjsParser_serializedATN);
const mathjsParser_decisionsToDFA = mathjsParser_atn.decisionToState.map((ds, index) => new index_web.dfa.DFA(ds, index));
const sharedContextCache = new index_web.atn.PredictionContextCache();
class mathjsParser extends index_web.Parser {
  static grammarFileName = "mathjs.g4";
  static literalNames = [];
  static symbolicNames = [];
  static ruleNames = [];
  constructor(input) {
    super(input);
    this._interp = new index_web.atn.ParserATNSimulator(this, mathjsParser_atn, mathjsParser_decisionsToDFA, sharedContextCache);
    this.ruleNames = mathjsParser.ruleNames;
    this.literalNames = mathjsParser.literalNames;
    this.symbolicNames = mathjsParser.symbolicNames;
  }
  sempred(localctx, ruleIndex, predIndex) {
    return true;
  }
  A(a) {
    this.match(a);
  }
  B(a, b) {
    this.match(a);
    this.match(b);
  }
  C(a, b, c) {
    this.match(a);
    this.match(b);
    this.match(c);
  }
  D(a, b, c, d) {
    this.match(a);
    this.match(b);
    this.match(c);
    this.match(d);
  }
  E(a, b) {
    this.state = a;
    this.expr(b);
  }
  I(a, b, e, f) {
    this.match(a);
    this.match(b);
    this.state = e;
    this.expr(f);
  }
  J(a, e, f) {
    this.match(a);
    this.state = e;
    this.expr(f);
  }
  R() {
    return this._input.LT(1);
  }
  Q() {
    this._errHandler.reportMatch(this);
    this.consume();
  }
  W(a, b, c) {
    this.pushNewRecursionContext(a, b, c);
  }
  X() {
    this._errHandler.sync(this);
  }
  Y() {
    return this._input.LA(1);
  }
  Z() {
    this._errHandler.sync(this);
    return this._input.LA(1);
  }
  prog() {
    let localctx = new ProgContext(this, this._ctx, this.state);
    this.enterRule(localctx, 0, 0);
    try {
      this.enterOuterAlt(localctx, 1);
      this.E(10, 0);
      this.match(-1);
    } catch (re) {
      if (re instanceof index_web.error.RecognitionException) {
        localctx.exception = re;
        this._errHandler.reportError(this, re);
        this._errHandler.recover(this, re);
      } else {
        throw re;
      }
    } finally {
      this.exitRule();
    }
    return localctx;
  }
  expr(_p) {
    if (_p === undefined) {
      _p = 0;
    }
    const _parentctx = this._ctx;
    const _parentState = this.state;
    let localctx = new ExprContext(this, this._ctx, _parentState);
    const _startState = 2;
    this.enterRecursionRule(localctx, 2, 1, _p);
    var _la = 0;
    try {
      this.enterOuterAlt(localctx, 1);
      this.X();
      var la_ = this._interp.adaptivePredict(this._input, 32, this._ctx);
      switch (la_) {
        case 1:
          this.J(2, 15, 0);
          this.A(3);
          break;
        case 2:
          this.J(7, 19, 40);
          break;
        case 3:
          this.I(293, 2, 22, 0);
          _la = this.Z();
          while (_la === 4) {
            this.J(4, 24, 0);
            _la = this.Z();
          }
          this.A(3);
          break;
        case 4:
          _la = this.Y();
          if (!(_la === 35 || _la === 38 || _la === 175)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 34, 0);
          this.J(4, 36, 0);
          _la = this.Z();
          if (_la === 4) {
            this.J(4, 38, 0);
          }
          this.A(3);
          break;
        case 5:
          this.I(36, 2, 45, 0);
          this.J(4, 47, 0);
          _la = this.Z();
          while (_la === 4) {
            this.J(4, 49, 0);
            this.J(4, 51, 0);
            _la = this.Z();
          }
          this.A(3);
          break;
        case 6:
          this.I(37, 2, 62, 0);
          this.J(4, 64, 0);
          this.J(4, 66, 0);
          _la = this.Z();
          while (_la === 4) {
            this.J(4, 68, 0);
            _la = this.Z();
          }
          this.A(3);
          break;
        case 7:
          _la = this.Y();
          if (!((_la - 39 & ~0x1f) === 0 && (1 << _la - 39 & 536875131) !== 0 || (_la - 71 & ~0x1f) === 0 && (1 << _la - 71 & 1073741579) !== 0 || (_la - 107 & ~0x1f) === 0 && (1 << _la - 107 & 268553059) !== 0 || (_la - 145 & ~0x1f) === 0 && (1 << _la - 145 & 390639743) !== 0 || (_la - 220 & ~0x1f) === 0 && (1 << _la - 220 & 4867) !== 0 || (_la - 257 & ~0x1f) === 0 && (1 << _la - 257 & 1610612991) !== 0 || _la === 290)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 78, 0);
          this.A(3);
          break;
        case 8:
          _la = this.Y();
          if (!((_la - 41 & ~0x1f) === 0 && (1 << _la - 41 & 97) !== 0 || _la === 73 || _la === 102 || (_la - 105 & ~0x1f) === 0 && (1 << _la - 105 & 2172653571) !== 0 || (_la - 138 & ~0x1f) === 0 && (1 << _la - 138 & 436666369) !== 0 || (_la - 172 & ~0x1f) === 0 && (1 << _la - 172 & 2149355521) !== 0 || _la === 209 || (_la - 250 & ~0x1f) === 0 && (1 << _la - 250 & 402653237) !== 0 || _la === 301 || _la === 304)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 83, 0);
          _la = this.Z();
          if (_la === 4) {
            this.J(4, 85, 0);
          }
          this.A(3);
          break;
        case 9:
          _la = this.Y();
          if (!((_la - 48 & ~0x1f) === 0 && (1 << _la - 48 & 402653191) !== 0 || (_la - 119 & ~0x1f) === 0 && (1 << _la - 119 & 6147) !== 0 || _la === 152 || (_la - 193 & ~0x1f) === 0 && (1 << _la - 193 & 30341655) !== 0 || _la === 248)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 92, 0);
          _la = this.Z();
          while (_la === 4) {
            this.J(4, 94, 0);
            _la = this.Z();
          }
          this.A(3);
          break;
        case 10:
          _la = this.Y();
          if (!((_la - 52 & ~0x1f) === 0 && (1 << _la - 52 & 15) !== 0)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.X();
          var la_ = this._interp.adaptivePredict(this._input, 6, this._ctx);
          if (la_ === 1) {
            this.B(2, 3);
          }
          break;
        case 11:
          _la = this.Y();
          if (!((_la - 56 & ~0x1f) === 0 && (1 << _la - 56 & 4095) !== 0)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 109, 0);
          _la = this.Z();
          if (_la === 4) {
            this.J(4, 111, 0);
          }
          this.A(3);
          break;
        case 12:
          _la = this.Y();
          if (!((_la - 69 & ~0x1f) === 0 && (1 << _la - 69 & 771) !== 0 || (_la - 101 & ~0x1f) === 0 && (1 << _la - 101 & 2273322253) !== 0 || (_la - 133 & ~0x1f) === 0 && (1 << _la - 133 & 537923523) !== 0 || (_la - 168 & ~0x1f) === 0 && (1 << _la - 168 & 3493068801) !== 0 || (_la - 200 & ~0x1f) === 0 && (1 << _la - 200 & 2395496451) !== 0 || (_la - 234 & ~0x1f) === 0 && (1 << _la - 234 & 2152366113) !== 0 || (_la - 267 & ~0x1f) === 0 && (1 << _la - 267 & 4076880833) !== 0 || (_la - 299 & ~0x1f) === 0 && (1 << _la - 299 & 27) !== 0)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 118, 0);
          this.J(4, 120, 0);
          this.A(3);
          break;
        case 13:
          _la = this.Y();
          if (!(_la === 110 || _la === 176 || _la === 177 || _la === 268)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.B(2, 3);
          break;
        case 14:
          this.I(137, 2, 128, 0);
          this.J(4, 130, 0);
          this.J(4, 132, 0);
          this.J(4, 134, 0);
          this.A(3);
          break;
        case 15:
          _la = this.Y();
          if (!(_la === 159 || _la === 185 || _la === 266)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 139, 0);
          this.J(4, 141, 0);
          this.J(4, 143, 0);
          this.A(3);
          break;
        case 16:
          _la = this.Y();
          if (!(_la === 161 || (_la - 218 & ~0x1f) === 0 && (1 << _la - 218 & 6197313) !== 0 || (_la - 283 & ~0x1f) === 0 && (1 << _la - 283 & 7) !== 0)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 148, 0);
          this.J(4, 150, 0);
          this.J(4, 152, 0);
          _la = this.Z();
          if (_la === 4) {
            this.J(4, 154, 0);
          }
          this.A(3);
          break;
        case 17:
          this.I(174, 2, 161, 0);
          this.J(4, 163, 0);
          this.J(4, 165, 0);
          _la = this.Z();
          if (_la === 4) {
            this.J(4, 167, 0);
            _la = this.Z();
            if (_la === 4) {
              this.J(4, 169, 0);
              _la = this.Z();
              if (_la === 4) {
                this.J(4, 171, 0);
              }
            }
          }
          this.A(3);
          break;
        case 18:
          _la = this.Y();
          if (!((_la - 178 & ~0x1f) === 0 && (1 << _la - 178 & 63) !== 0)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 182, 0);
          this.A(3);
          break;
        case 19:
          _la = this.Y();
          if (!((_la - 241 & ~0x1f) === 0 && (1 << _la - 241 & 57) !== 0)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 187, 0);
          this.J(4, 189, 0);
          this.J(4, 191, 0);
          _la = this.Z();
          if (_la === 4) {
            this.J(4, 193, 0);
            _la = this.Z();
            if (_la === 4) {
              this.J(4, 195, 0);
            }
          }
          this.A(3);
          break;
        case 20:
          _la = this.Y();
          if (!(_la === 242 || _la === 243)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 204, 0);
          this.J(4, 206, 0);
          this.J(4, 208, 0);
          this.J(4, 210, 0);
          _la = this.Z();
          if (_la === 4) {
            this.J(4, 212, 0);
            _la = this.Z();
            if (_la === 4) {
              this.J(4, 214, 0);
            }
          }
          this.A(3);
          break;
        case 21:
          this.I(247, 2, 223, 0);
          this.J(4, 225, 0);
          this.J(4, 227, 0);
          _la = this.Z();
          if (_la === 4) {
            this.J(4, 229, 0);
            _la = this.Z();
            if (_la === 4) {
              this.J(4, 231, 0);
              _la = this.Z();
              if (_la === 4) {
                this.J(4, 233, 0);
              }
            }
          }
          this.A(3);
          break;
        case 22:
          _la = this.Y();
          if (!(_la === 279 || _la === 280)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 244, 0);
          this.J(4, 246, 0);
          _la = this.Z();
          if (_la === 4) {
            this.J(4, 248, 0);
            _la = this.Z();
            if (_la === 4) {
              this.J(4, 250, 0);
            }
          }
          this.A(3);
          break;
        case 23:
          this.I(282, 2, 259, 0);
          _la = this.Z();
          do {
            this.J(4, 261, 0);
            _la = this.Z();
          } while (_la === 4);
          this.A(3);
          break;
        case 24:
          _la = this.Y();
          if (!(_la === 288 || _la === 289)) {
            this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(2, 270, 0);
          _la = this.Z();
          if (_la === 4) {
            this.J(4, 272, 0);
            _la = this.Z();
            if (_la === 4) {
              this.J(4, 274, 0);
            }
          }
          this.A(3);
          break;
        case 25:
          this.B(305, 2);
          _la = this.Z();
          if ((_la & ~0x1f) === 0 && (1 << _la & 3892314276) !== 0 || (_la - 32 & ~0x1f) === 0 && (1 << _la - 32 & 4294967291) !== 0 || (_la - 64 & ~0x1f) === 0 && (1 << _la - 64 & 4294967295) !== 0 || (_la - 96 & ~0x1f) === 0 && (1 << _la - 96 & 4294967295) !== 0 || (_la - 128 & ~0x1f) === 0 && (1 << _la - 128 & 4294967295) !== 0 || (_la - 160 & ~0x1f) === 0 && (1 << _la - 160 & 4294967295) !== 0 || (_la - 192 & ~0x1f) === 0 && (1 << _la - 192 & 4294967295) !== 0 || (_la - 224 & ~0x1f) === 0 && (1 << _la - 224 & 4294967295) !== 0 || (_la - 256 & ~0x1f) === 0 && (1 << _la - 256 & 4294844415) !== 0 || (_la - 288 & ~0x1f) === 0 && (1 << _la - 288 & 262143) !== 0) {
            this.E(283, 0);
            _la = this.Z();
            while (_la === 4) {
              this.J(4, 285, 0);
              _la = this.Z();
            }
          }
          this.A(3);
          break;
        case 26:
          this.B(33, 2);
          _la = this.Z();
          if ((_la & ~0x1f) === 0 && (1 << _la & 3892314276) !== 0 || (_la - 32 & ~0x1f) === 0 && (1 << _la - 32 & 4294967291) !== 0 || (_la - 64 & ~0x1f) === 0 && (1 << _la - 64 & 4294967295) !== 0 || (_la - 96 & ~0x1f) === 0 && (1 << _la - 96 & 4294967295) !== 0 || (_la - 128 & ~0x1f) === 0 && (1 << _la - 128 & 4294967295) !== 0 || (_la - 160 & ~0x1f) === 0 && (1 << _la - 160 & 4294967295) !== 0 || (_la - 192 & ~0x1f) === 0 && (1 << _la - 192 & 4294967295) !== 0 || (_la - 224 & ~0x1f) === 0 && (1 << _la - 224 & 4294967295) !== 0 || (_la - 256 & ~0x1f) === 0 && (1 << _la - 256 & 4294844415) !== 0 || (_la - 288 & ~0x1f) === 0 && (1 << _la - 288 & 262143) !== 0) {
            this.E(296, 0);
          }
          this.A(3);
          break;
        case 27:
          this.A(27);
          this.state = 301;
          this.arrayJson();
          this.X();
          var _alt = this._interp.adaptivePredict(this._input, 27, this._ctx);
          while (_alt != 2 && _alt != index_web.atn.ATN.INVALID_ALT_NUMBER) {
            if (_alt === 1) {
              this.A(4);
              this.state = 303;
              this.arrayJson();
            }
            this.X();
            _alt = this._interp.adaptivePredict(this._input, 27, this._ctx);
          }
          _la = this.Z();
          while (_la === 4) {
            this.A(4);
            _la = this.Z();
          }
          this.A(28);
          break;
        case 28:
          this.J(5, 318, 0);
          this.X();
          var _alt = this._interp.adaptivePredict(this._input, 29, this._ctx);
          while (_alt != 2 && _alt != index_web.atn.ATN.INVALID_ALT_NUMBER) {
            if (_alt === 1) {
              this.J(4, 320, 0);
            }
            this.X();
            _alt = this._interp.adaptivePredict(this._input, 29, this._ctx);
          }
          _la = this.Z();
          while (_la === 4) {
            this.A(4);
            _la = this.Z();
          }
          this.A(6);
          break;
        case 29:
          this.A(294);
          break;
        case 30:
          this.A(305);
          break;
        case 31:
          this.state = 336;
          this.num();
          this.X();
          var la_ = this._interp.adaptivePredict(this._input, 31, this._ctx);
          if (la_ === 1) {
            localctx.unit = this.R();
            _la = this.Y();
            if (!(_la === 34 || _la === 167)) {
              localctx.unit = this._errHandler.recoverInline(this);
            } else {
              this.Q();
            }
          }
          break;
        case 32:
          this.A(31);
          break;
        case 33:
          this.A(32);
          break;
      }
      this._ctx.stop = this._input.LT(-1);
      this.X();
      var _alt = this._interp.adaptivePredict(this._input, 43, this._ctx);
      while (_alt != 2 && _alt != index_web.atn.ATN.INVALID_ALT_NUMBER) {
        if (_alt === 1) {
          if (this._parseListeners !== null) {
            this.triggerExitRuleEvent();
          }
          this.X();
          var la_ = this._interp.adaptivePredict(this._input, 42, this._ctx);
          switch (la_) {
            case 1:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              localctx.op = this.R();
              _la = this.Y();
              if (!((_la & ~0x1f) === 0 && (1 << _la & 1792) !== 0)) {
                localctx.op = this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.E(346, 39);
              break;
            case 2:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              localctx.op = this.R();
              _la = this.Y();
              if (!((_la & ~0x1f) === 0 && (1 << _la & 536877056) !== 0)) {
                localctx.op = this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.E(349, 38);
              break;
            case 3:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              localctx.op = this.R();
              _la = this.Y();
              if (!((_la & ~0x1f) === 0 && (1 << _la & 122880) !== 0)) {
                localctx.op = this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.E(352, 37);
              break;
            case 4:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              localctx.op = this.R();
              _la = this.Y();
              if (!((_la & ~0x1f) === 0 && (1 << _la & 8257536) !== 0)) {
                localctx.op = this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.E(355, 36);
              break;
            case 5:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              localctx.op = this.J(23, 358, 35);
              break;
            case 6:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              localctx.op = this.J(24, 361, 34);
              break;
            case 7:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.J(25, 364, 0);
              this.J(26, 366, 33);
              break;
            case 8:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.A(1);
              _la = this.Y();
              if (!((_la - 39 & ~0x1f) === 0 && (1 << _la - 39 & 27) !== 0 || _la === 74 || (_la - 157 & ~0x1f) === 0 && (1 << _la - 157 & 95363) !== 0 || (_la - 257 & ~0x1f) === 0 && (1 << _la - 257 & 1610674179) !== 0 || _la === 290)) {
                this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.B(2, 3);
              break;
            case 9:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.A(1);
              _la = this.Y();
              if (!((_la - 41 & ~0x1f) === 0 && (1 << _la - 41 & 97) !== 0 || (_la - 156 & ~0x1f) === 0 && (1 << _la - 156 & 65665) !== 0 || (_la - 277 & ~0x1f) === 0 && (1 << _la - 277 & 16777219) !== 0)) {
                this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.A(2);
              _la = this.Z();
              if ((_la & ~0x1f) === 0 && (1 << _la & 3892314276) !== 0 || (_la - 32 & ~0x1f) === 0 && (1 << _la - 32 & 4294967291) !== 0 || (_la - 64 & ~0x1f) === 0 && (1 << _la - 64 & 4294967295) !== 0 || (_la - 96 & ~0x1f) === 0 && (1 << _la - 96 & 4294967295) !== 0 || (_la - 128 & ~0x1f) === 0 && (1 << _la - 128 & 4294967295) !== 0 || (_la - 160 & ~0x1f) === 0 && (1 << _la - 160 & 4294967295) !== 0 || (_la - 192 & ~0x1f) === 0 && (1 << _la - 192 & 4294967295) !== 0 || (_la - 224 & ~0x1f) === 0 && (1 << _la - 224 & 4294967295) !== 0 || (_la - 256 & ~0x1f) === 0 && (1 << _la - 256 & 4294844415) !== 0 || (_la - 288 & ~0x1f) === 0 && (1 << _la - 288 & 262143) !== 0) {
                this.E(377, 0);
              }
              this.A(3);
              break;
            case 10:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.A(1);
              _la = this.Y();
              if (!(_la === 153 || _la === 168 || (_la - 265 & ~0x1f) === 0 && (1 << _la - 265 & 3221291013) !== 0 || (_la - 297 & ~0x1f) === 0 && (1 << _la - 297 & 111) !== 0)) {
                this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.J(2, 385, 0);
              this.A(3);
              break;
            case 11:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.A(1);
              _la = this.Y();
              if (!((_la - 178 & ~0x1f) === 0 && (1 << _la - 178 & 63) !== 0)) {
                this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.X();
              var la_ = this._interp.adaptivePredict(this._input, 34, this._ctx);
              if (la_ === 1) {
                this.B(2, 3);
              }
              break;
            case 12:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.A(1);
              _la = this.Y();
              if (!(_la === 159 || _la === 266)) {
                this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.J(2, 399, 0);
              this.J(4, 401, 0);
              this.A(3);
              break;
            case 13:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.G(1, 161, 2, 408, 0);
              this.J(4, 410, 0);
              _la = this.Z();
              if (_la === 4) {
                this.J(4, 412, 0);
              }
              this.A(3);
              break;
            case 14:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.A(1);
              _la = this.Y();
              if (!((_la - 283 & ~0x1f) === 0 && (1 << _la - 283 & 103) !== 0)) {
                this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.J(2, 421, 0);
              _la = this.Z();
              if (_la === 4) {
                this.J(4, 423, 0);
              }
              this.A(3);
              break;
            case 15:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.A(1);
              _la = this.Y();
              if (!(_la === 279 || _la === 280)) {
                this._errHandler.recoverInline(this);
              } else {
                this.Q();
              }
              this.J(2, 432, 0);
              _la = this.Z();
              if (_la === 4) {
                this.J(4, 434, 0);
                _la = this.Z();
                if (_la === 4) {
                  this.J(4, 436, 0);
                }
              }
              this.A(3);
              break;
            case 16:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.G(1, 282, 2, 447, 0);
              _la = this.Z();
              while (_la === 4) {
                this.J(4, 449, 0);
                _la = this.Z();
              }
              this.A(3);
              break;
            case 17:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.C(1, 305, 2);
              _la = this.Z();
              if ((_la & ~0x1f) === 0 && (1 << _la & 3892314276) !== 0 || (_la - 32 & ~0x1f) === 0 && (1 << _la - 32 & 4294967291) !== 0 || (_la - 64 & ~0x1f) === 0 && (1 << _la - 64 & 4294967295) !== 0 || (_la - 96 & ~0x1f) === 0 && (1 << _la - 96 & 4294967295) !== 0 || (_la - 128 & ~0x1f) === 0 && (1 << _la - 128 & 4294967295) !== 0 || (_la - 160 & ~0x1f) === 0 && (1 << _la - 160 & 4294967295) !== 0 || (_la - 192 & ~0x1f) === 0 && (1 << _la - 192 & 4294967295) !== 0 || (_la - 224 & ~0x1f) === 0 && (1 << _la - 224 & 4294967295) !== 0 || (_la - 256 & ~0x1f) === 0 && (1 << _la - 256 & 4294844415) !== 0 || (_la - 288 & ~0x1f) === 0 && (1 << _la - 288 & 262143) !== 0) {
                this.E(461, 0);
                _la = this.Z();
                while (_la === 4) {
                  this.J(4, 463, 0);
                  _la = this.Z();
                }
              }
              this.A(3);
              break;
            case 18:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.C(5, 305, 6);
              break;
            case 19:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.J(5, 478, 0);
              this.A(6);
              break;
            case 20:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.A(1);
              this.state = 483;
              this.parameter2();
              break;
            case 21:
              localctx = new ExprContext(this, _parentctx, _parentState);
              this.W(localctx, _startState, 1);
              this.A(8);
              break;
          }
        }
        this.X();
        _alt = this._interp.adaptivePredict(this._input, 43, this._ctx);
      }
    } catch (error) {
      if (error instanceof index_web.error.RecognitionException) {
        localctx.exception = error;
        this._errHandler.reportError(this, error);
        this._errHandler.recover(this, error);
      } else {
        throw error;
      }
    } finally {
      this.unrollRecursionContexts(_parentctx);
    }
    return localctx;
  }
  num() {
    let localctx = new NumContext(this, this._ctx, this.state);
    this.enterRule(localctx, 4, 2);
    var _la = 0;
    try {
      this.enterOuterAlt(localctx, 1);
      _la = this.Z();
      if (_la === 29) {
        this.A(29);
      }
      this.A(30);
    } catch (re) {
      if (re instanceof index_web.error.RecognitionException) {
        localctx.exception = re;
        this._errHandler.reportError(this, re);
        this._errHandler.recover(this, re);
      } else {
        throw re;
      }
    } finally {
      this.exitRule();
    }
    return localctx;
  }
  arrayJson() {
    let localctx = new ArrayJsonContext(this, this._ctx, this.state);
    this.enterRule(localctx, 6, 3);
    var _la = 0;
    try {
      this.X();
      switch (this._input.LA(1)) {
        case 30:
        case 31:
          this.enterOuterAlt(localctx, 1);
          localctx.key = this.R();
          _la = this.Y();
          if (!(_la === 30 || _la === 31)) {
            localctx.key = this._errHandler.recoverInline(this);
          } else {
            this.Q();
          }
          this.J(26, 498, 0);
          break;
        case 32:
        case 33:
        case 34:
        case 35:
        case 36:
        case 37:
        case 38:
        case 39:
        case 40:
        case 41:
        case 42:
        case 43:
        case 44:
        case 45:
        case 46:
        case 47:
        case 48:
        case 49:
        case 50:
        case 51:
        case 52:
        case 53:
        case 54:
        case 55:
        case 56:
        case 57:
        case 58:
        case 59:
        case 60:
        case 61:
        case 62:
        case 63:
        case 64:
        case 65:
        case 66:
        case 67:
        case 68:
        case 69:
        case 70:
        case 71:
        case 72:
        case 73:
        case 74:
        case 75:
        case 76:
        case 77:
        case 78:
        case 79:
        case 80:
        case 81:
        case 82:
        case 83:
        case 84:
        case 85:
        case 86:
        case 87:
        case 88:
        case 89:
        case 90:
        case 91:
        case 92:
        case 93:
        case 94:
        case 95:
        case 96:
        case 97:
        case 98:
        case 99:
        case 100:
        case 101:
        case 102:
        case 103:
        case 104:
        case 105:
        case 106:
        case 107:
        case 108:
        case 109:
        case 110:
        case 111:
        case 112:
        case 113:
        case 114:
        case 115:
        case 116:
        case 117:
        case 118:
        case 119:
        case 120:
        case 121:
        case 122:
        case 123:
        case 124:
        case 125:
        case 126:
        case 127:
        case 128:
        case 129:
        case 130:
        case 131:
        case 132:
        case 133:
        case 134:
        case 135:
        case 136:
        case 137:
        case 138:
        case 139:
        case 140:
        case 141:
        case 142:
        case 143:
        case 144:
        case 145:
        case 146:
        case 147:
        case 148:
        case 149:
        case 150:
        case 151:
        case 152:
        case 153:
        case 154:
        case 155:
        case 156:
        case 157:
        case 158:
        case 159:
        case 160:
        case 161:
        case 162:
        case 163:
        case 164:
        case 165:
        case 166:
        case 167:
        case 168:
        case 169:
        case 170:
        case 171:
        case 172:
        case 173:
        case 174:
        case 175:
        case 176:
        case 177:
        case 178:
        case 179:
        case 180:
        case 181:
        case 182:
        case 183:
        case 184:
        case 185:
        case 186:
        case 187:
        case 188:
        case 189:
        case 190:
        case 191:
        case 192:
        case 193:
        case 194:
        case 195:
        case 196:
        case 197:
        case 198:
        case 199:
        case 200:
        case 201:
        case 202:
        case 203:
        case 204:
        case 205:
        case 206:
        case 207:
        case 208:
        case 209:
        case 210:
        case 211:
        case 212:
        case 213:
        case 214:
        case 215:
        case 216:
        case 217:
        case 218:
        case 219:
        case 220:
        case 221:
        case 222:
        case 223:
        case 224:
        case 225:
        case 226:
        case 227:
        case 228:
        case 229:
        case 230:
        case 231:
        case 232:
        case 233:
        case 234:
        case 235:
        case 236:
        case 237:
        case 238:
        case 239:
        case 240:
        case 241:
        case 242:
        case 243:
        case 244:
        case 245:
        case 246:
        case 247:
        case 248:
        case 249:
        case 250:
        case 251:
        case 252:
        case 253:
        case 254:
        case 255:
        case 256:
        case 257:
        case 258:
        case 259:
        case 260:
        case 261:
        case 262:
        case 263:
        case 264:
        case 265:
        case 266:
        case 267:
        case 268:
        case 269:
        case 270:
        case 271:
        case 272:
        case 273:
        case 274:
        case 275:
        case 276:
        case 277:
        case 278:
        case 279:
        case 280:
        case 281:
        case 282:
        case 283:
        case 284:
        case 285:
        case 286:
        case 287:
        case 288:
        case 289:
        case 290:
        case 291:
        case 292:
        case 294:
        case 295:
        case 296:
        case 297:
        case 298:
        case 299:
        case 300:
        case 301:
        case 302:
        case 303:
        case 304:
        case 305:
          this.enterOuterAlt(localctx, 2);
          this.state = 499;
          this.parameter2();
          this.J(26, 501, 0);
          break;
        default:
          throw new index_web.error.NoViableAltException(this);
      }
    } catch (re) {
      if (re instanceof index_web.error.RecognitionException) {
        localctx.exception = re;
        this._errHandler.reportError(this, re);
        this._errHandler.recover(this, re);
      } else {
        throw re;
      }
    } finally {
      this.exitRule();
    }
    return localctx;
  }
  parameter2() {
    let localctx = new Parameter2Context(this, this._ctx, this.state);
    this.enterRule(localctx, 8, 4);
    var _la = 0;
    try {
      this.enterOuterAlt(localctx, 1);
      _la = this.Y();
      if (!((_la - 32 & ~0x1f) === 0 && (1 << _la - 32 & 4294967295) !== 0 || (_la - 64 & ~0x1f) === 0 && (1 << _la - 64 & 4294967295) !== 0 || (_la - 96 & ~0x1f) === 0 && (1 << _la - 96 & 4294967295) !== 0 || (_la - 128 & ~0x1f) === 0 && (1 << _la - 128 & 4294967295) !== 0 || (_la - 160 & ~0x1f) === 0 && (1 << _la - 160 & 4294967295) !== 0 || (_la - 192 & ~0x1f) === 0 && (1 << _la - 192 & 4294967295) !== 0 || (_la - 224 & ~0x1f) === 0 && (1 << _la - 224 & 4294967295) !== 0 || (_la - 256 & ~0x1f) === 0 && (1 << _la - 256 & 4294967295) !== 0 || (_la - 288 & ~0x1f) === 0 && (1 << _la - 288 & 262111) !== 0)) {
        this._errHandler.recoverInline(this);
      } else {
        this.Q();
      }
    } catch (re) {
      if (re instanceof index_web.error.RecognitionException) {
        localctx.exception = re;
        this._errHandler.reportError(this, re);
        this._errHandler.recover(this, re);
      } else {
        throw re;
      }
    } finally {
      this.exitRule();
    }
    return localctx;
  }
}
class ProgContext extends index_web.ParserRuleContext {
  constructor(parser, parent, invokingState) {
    super(parent, invokingState);
    this.parser = parser;
    this.ruleIndex = 0;
  }
  expr() {
    return this.getTypedRuleContext(ExprContext, 0);
  }
  accept(visitor) {
    return visitor.visitProg(this);
  }
}
class ExprContext extends index_web.ParserRuleContext {
  constructor(parser, parent, invokingState) {
    super(parent, invokingState);
    this.parser = parser;
    this.ruleIndex = 1;
    this.unit = null;
    this.op = null;
  }
  expr = function (i) {
    return this.getTypedRuleContexts(ExprContext);
  };
  arrayJson = function (i) {
    return this.getTypedRuleContexts(ArrayJsonContext);
  };
  num() {
    return this.getTypedRuleContext(NumContext, 0);
  }
  parameter2() {
    return this.getTypedRuleContext(Parameter2Context, 0);
  }
  accept(visitor) {
    if (visitor instanceof mathjsVisitor) {
      return visitor.visitExpr(this);
    }
  }
}
class NumContext extends index_web.ParserRuleContext {
  constructor(parser, parent, invokingState) {
    super(parent, invokingState);
    this.parser = parser;
    this.ruleIndex = 2;
  }
  accept(visitor) {
    return visitor.visitNum(this);
  }
}
class ArrayJsonContext extends index_web.ParserRuleContext {
  constructor(parser, parent, invokingState) {
    super(parent, invokingState);
    this.parser = parser;
    this.ruleIndex = 3;
    this.key = null;
  }
  expr() {
    return this.getTypedRuleContext(ExprContext, 0);
  }
  parameter2() {
    return this.getTypedRuleContext(Parameter2Context, 0);
  }
  accept(visitor) {
    return visitor.visitArrayJson(this);
  }
}
class Parameter2Context extends index_web.ParserRuleContext {
  constructor(parser, parent, invokingState) {
    super(parent, invokingState);
    this.parser = parser;
    this.ruleIndex = 4;
  }
  accept(visitor) {
    return visitor.visitParameter2(this);
  }
}
;// ./src/AlgorithmEngineHelper.js





class AlgorithmEngineHelper {
  static ParseFormula(exp, errorListener) {
    if (!exp || exp.trim() === '') {
      throw new Error("Parameter exp invalid !");
    }
    if (!errorListener) {
      throw new Error("Parameter errorListener invalid !");
    }
    let stream = new AntlrCharStream(exp);
    let lexer = new mathjsLexer(stream);
    lexer.removeErrorListeners();
    lexer.addErrorListener(errorListener);
    let tokens = new CommonTokenStream(lexer);
    let parser = new mathjsParser(tokens);
    parser.removeErrorListeners();
    parser.addErrorListener(errorListener);
    let context = parser.prog();
    let visitor = new mathjsVisitor();
    return visitor.visit(context);
  }
}
;// ./src/index.js



/* harmony default export */ const src = ({
  AlgorithmEngineHelper: AlgorithmEngineHelper
});

// Browser support
if (typeof window !== 'undefined') {
  window.ToolGood = window.ToolGood || {};
  window.ToolGood.antlr4 = index_web;
  window.ToolGood.Algorithm = {
    AlgorithmEngineHelper: AlgorithmEngineHelper
  };
}
__webpack_exports__ = __webpack_exports__["default"];
/******/ 	return __webpack_exports__;
/******/ })()
;
});