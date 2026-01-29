export * from './Constants.js';
export * from './Precision.js';
export * from './ExcelFunctions.js';
export * from './Distributions/index.js';
export * from './RootFinding/index.js';
export * from './SpecialFunctions/index.js';
export * from './Statistics/index.js';

// Browser support
if (typeof window !== 'undefined') {
    window.MathNet = {
        Constants: require('./Constants.js'),
        Precision: require('./Precision.js'),
        ExcelFunctions: require('./ExcelFunctions.js'),
        Distributions: require('./Distributions/index.js'),
        RootFinding: require('./RootFinding/index.js'),
        SpecialFunctions: require('./SpecialFunctions/index.js'),
        Statistics: require('./Statistics/index.js')
    };
}
