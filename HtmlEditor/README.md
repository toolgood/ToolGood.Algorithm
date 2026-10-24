说明
=====

代码从`ToolGood.FlowVision`移过来的， 目前没时间同步，大家凑活着用吧。

公式校验
-----

`toolgood.algorithm.js` 由 `../JavaScript2` 打包产出（手写解析器，无 antlr4 依赖）：

```bash
cd JavaScript2 && npm run build
cp dist/toolgood.algorithm.js ../HtmlEditor/toolgood.algorithm.js
```

校验有两条路径，都用同一个解析器：

| 场景 | 校验位置 |
|---|---|
| `http(s)://` 打开 | ace 的 Web Worker（`my-worker.js`），500ms 去抖 |
| `file://` 打开（worker 会被浏览器 CORS 拦截） | 主线程（`createEditor.js` 里的 `installLocalValidator`），300ms 去抖 |

两条路径都通过 `ToolGood.Algorithm.AlgorithmEngineHelper.ParseFormula(exp, listener)` 上报错误，
listener 的 `syntaxError(recognizer, offendingSymbol, line, column, msg, e)` 会被转成 ace 的 annotation。

如果校验器没加载成功，编辑器会在第 1 行显示 `Formula validator is not loaded (toolgood.algorithm.js).`，
不会再出现"明明有语法错但什么都不提示"的情况。
