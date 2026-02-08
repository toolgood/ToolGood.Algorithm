class UnitFactorSynonyms {
    constructor(...items) {
        this._synonyms = [...items];
    }

    // Find if some synonym of a given UnitFactor is included in this UnitFactor
    contains(synonyms) {
        if (synonyms instanceof UnitFactorSynonyms) {
            for (let syn of synonyms._synonyms) {
                if (this.contains(syn)) {
                    return true;
                }
            }
            return false;
        } else if (typeof synonyms === 'string') {
            return this._synonyms.some(syn => 
                syn.toLowerCase() === synonyms.toLowerCase()
            );
        }
        return false;
    }

    toString() {
        return this._synonyms.join(', ');
    }
}

// 为了模拟 C# 中的隐式转换，添加一个静态方法
UnitFactorSynonyms.fromString = function(synonym) {
    return new UnitFactorSynonyms(synonym);
};

export { UnitFactorSynonyms };