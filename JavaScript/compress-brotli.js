import fs from 'fs';
import zlib from 'zlib';

// 读取原始文件
const inputFile = 'dist/toolgood.algorithm.js';
const outputFile = 'dist/toolgood.algorithm.js.br';

// 检查输入文件是否存在
if (!fs.existsSync(inputFile)) {
  console.error('Error: Input file not found');
  process.exit(1);
}

console.log('Compressing file with Brotli...');

// 读取文件内容
const input = fs.readFileSync(inputFile);

// 使用 Brotli 压缩
zlib.brotliCompress(input, { level: zlib.constants.BROTLI_MAX_QUALITY }, (err, compressed) => {
  if (err) {
    console.error('Compression error:', err);
    process.exit(1);
  }
  
  // 写入压缩文件
  fs.writeFileSync(outputFile, compressed);
  
  // 计算压缩率
  const originalSize = input.length;
  const compressedSize = compressed.length;
  const compressionRatio = ((1 - compressedSize / originalSize) * 100).toFixed(2);
  
  console.log('Compression completed successfully!');
  console.log(`Original size: ${originalSize} bytes`);
  console.log(`Compressed size: ${compressedSize} bytes`);
  console.log(`Compression ratio: ${compressionRatio}%`);
  console.log(`Compressed file: ${outputFile}`);
});
