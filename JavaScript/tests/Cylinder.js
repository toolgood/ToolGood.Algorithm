import { AlgorithmEngineEx } from '../src/AlgorithmEngineEx.js';
import { Operand } from '../src/Operand.js';

// 定义圆柱信息
class Cylinder extends AlgorithmEngineEx {
  constructor(radius, height) {
    super();
    this._radius = radius;
    this._height = height;
  }

  GetParameter(parameter) {
    if (parameter === "半径") {
      return Operand.Create(this._radius);
    }
    if (parameter === "直径") {
      return Operand.Create(this._radius * 2);
    }
    if (parameter === "高") {
      return Operand.Create(this._height);
    }
    return super.GetParameter(parameter);
  }

  ExecuteDiyFunction(funcName, operands) {
    if (funcName === "求面积") {
      if (operands.length === 1) {
        const r = operands[0].ToNumber().NumberValue;
        return r * r * Math.PI;
      }
    }
    return super.ExecuteDiyFunction(funcName, operands);
  }
}

// 导出 Cylinder 类
export { Cylinder };