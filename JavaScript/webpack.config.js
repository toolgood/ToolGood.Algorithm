import path from 'path';
import { fileURLToPath } from 'url';
import webpack from 'webpack';
import fs from 'fs';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

// 读取 license 文件内容并转换为注释格式
const licensePath = path.resolve(__dirname, '../license.md');
const licenseContent = fs.readFileSync(licensePath, 'utf8');
const licenseComment = `/*!\n${licenseContent.split('\n').map(line => ` * ${line}`).join('\n')}\n */`;

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
  plugins: [
    new webpack.DefinePlugin({
      'process.env': {
        NODE_ENV: JSON.stringify('production')
      },
      'process': {
        env: {
          NODE_ENV: JSON.stringify('production')
        },
        version: JSON.stringify('')
      }
    }),
    new webpack.BannerPlugin({
      banner: licenseComment,
      raw: true,
      entryOnly: true
    })
  ],
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