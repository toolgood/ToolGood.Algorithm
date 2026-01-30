import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

export default {
  entry: './src/index.js',
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'toolgood.algorithm.js',
    library: {
      name: 'ToolGood.Algorithm',
      type: 'umd',
      export: 'default'
    },
    globalObject: 'this'
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
          options: {
            presets: [
              [
                '@babel/preset-env',
                {
                  targets: {
                    browsers: ['defaults']
                  },
                  modules: false,
                  useBuiltIns: 'usage',
                  corejs: 3
                }
              ]
            ],
            plugins: [
              '@babel/plugin-transform-runtime'
            ]
          }
        }
      }
    ]
  },
  resolve: {
    extensions: ['.js'],
    fallback: {
      "crypto": "crypto-browserify",
      "buffer": "buffer",
      "stream": "stream-browserify",
      "vm": "vm-browserify",
      "string_decoder": "string_decoder",
      "path": false,
      "fs": false,
      "net": false,
      "tls": false,
      "child_process": false
    }
  },
  plugins: [],
  optimization: {
    minimize: true,
    usedExports: true,
    sideEffects: false
  },
  performance: {
    hints: false,
    maxAssetSize: 1000000,
    maxEntrypointSize: 1000000
  }
};