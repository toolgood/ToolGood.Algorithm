import { UnitNotSupportedException } from './UnitNotSupportedException.js';
import { UnitFactorSynonyms } from './UnitFactorSynonyms.js';

class UnitFactors {
    constructor() {
        this.factors = {};
    }

    // 添加单位因子
    set(synonyms, factor) {
        this.factors[synonyms.toString()] = { synonyms, factor };
    }

    // 获取所有键
    get keys() {
        return Object.values(this.factors).map(item => item.synonyms);
    }

    // 获取所有值
    get values() {
        return Object.values(this.factors).map(item => item.factor);
    }

    // 获取所有条目
    get entries() {
        return Object.values(this.factors);
    }

    // Find the key or null for a given unit
    findUnit(synonyms) {
        for (let item of Object.values(this.factors)) {
            if (item.synonyms.contains(synonyms)) {
                return item.synonyms;
            }
        }
        return null;
    }

    // Get the factor for a given unit
    findFactor(synonyms) {
        for (let item of Object.values(this.factors)) {
            if (item.synonyms.contains(synonyms)) {
                return item.factor;
            }
        }
        throw new UnitNotSupportedException(synonyms.toString());
    }
}

export { UnitFactors };