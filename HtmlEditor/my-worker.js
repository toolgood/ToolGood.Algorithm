if (!importScripts) {
    var importScripts = (function (globalEval) {
        var xhr = new XMLHttpRequest;
        return function importScripts() {
            var
                args = Array.prototype.slice.call(arguments)
                , len = args.length
                , i = 0
                , meta
                , data
                , content
                ;
            for (; i < len; i++) {
                if (args[i].substr(0, 5).toLowerCase() === "data:") {
                    data = args[i];
                    content = data.indexOf(",");
                    meta = data.substr(5, content).toLowerCase();
                    data = decodeURIComponent(data.substr(content + 1));
                    if (/;\s*base64\s*[;,]/.test(meta)) {
                        data = atob(data);
                    }
                    if (/;\s*charset=[uU][tT][fF]-?8\s*[;,]/.test(meta)) {
                        data = decodeURIComponent(escape(data));
                    }
                } else {
                    xhr.open("GET", location.origin + "/_/inputControl" + args[i], false);
                    xhr.send(null);
                    data = xhr.responseText;
                }
                globalEval(data);
            }
        };
    }(eval));
}
importScripts("/ace-ext/worker.js");
importScripts("/ace/ace.js");
importScripts("/ace-ext/mirror.js");

// 加载公式校验器：路径容错，避免 importScripts 抛错直接打死 worker（那样编辑器会静默不报错）
(function () {
    var candidates = ["/toolgood.algorithm.js", "toolgood.algorithm.js", "../toolgood.algorithm.js"];
    for (var i = 0; i < candidates.length; i++) {
        try {
            importScripts(candidates[i]);
            if ((typeof self !== 'undefined' && self['ToolGood.Algorithm'])
                || typeof algorithm !== 'undefined') {
                return;
            }
        } catch (e) {
            // 换下一个路径再试
        }
    }
})();

ace.define('ace/worker/my-worker', ["require", "exports", "module", "ace/lib/oop", "ace/worker/mirror"], function (require, exports, module) {
    "use strict";
    var oop = require("ace/lib/oop");
    var Mirror = require("ace/worker/mirror").Mirror;
    var MyWorker = function (sender) {
        Mirror.call(this, sender);
        this.setTimeout(500);
        this.$dialect = null;
    };

    oop.inherits(MyWorker, Mirror);

    // 取校验器：UMD 会挂到 self["ToolGood.Algorithm"]，构造函数里还会建 self.ToolGood.Algorithm；
    // 同时兼容旧包的 algorithm 全局
    function getAlgorithm() {
        if (typeof self !== 'undefined') {
            if (self['ToolGood.Algorithm']) { return self['ToolGood.Algorithm']; }
            if (self.ToolGood && self.ToolGood.Algorithm) { return self.ToolGood.Algorithm; }
        }
        if (typeof algorithm !== 'undefined' && algorithm) {
            return algorithm;
        }
        return null;
    }

    // 收集错误并转成 ACE 的 annotation
    function createListener(annotations) {
        return {
            syntaxError: function (recognizer, offendingSymbol, line, column, msg, e) {
                annotations.push({
                    row: line - 1,
                    column: column,
                    text: msg,
                    type: "error"
                });
            },
            reportAmbiguity: function () { },
            reportAttemptingFullContext: function () { },
            reportContextSensitivity: function () { }
        };
    }

    function validate(input) {
        var annotations = [];
        var api = getAlgorithm();
        if (!api || !api.AlgorithmEngineHelper) {
            // 校验器没加载时明确提示，避免"静默不报错"让人以为公式没问题
            annotations.push({
                row: 0, column: 0,
                text: 'Formula validator is not loaded (toolgood.algorithm.js).',
                type: "error"
            });
            return annotations;
        }
        try {
            // 词法内部已做大小写/全角归一化，这里不再预处理原文
            api.AlgorithmEngineHelper.ParseFormula(input, createListener(annotations));
        } catch (e) {
            annotations.push({ column: 0, row: 0, text: e.message, type: "error" });
        }
        return annotations;
    }

    (function () {
        this.onUpdate = function () {
            var value = this.doc.getValue();
            var annotations = validate(value);
            this.sender.emit("annotate", annotations);
        };
    }).call(MyWorker.prototype);
    exports.MyWorker = MyWorker;
});