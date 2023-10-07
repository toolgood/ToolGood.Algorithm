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
importScripts("/toolgood.algorithm.js");

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

    antlr4 = algorithm.antlr4;
    // class for gathering errors and posting them to ACE editor
    var AnnotatingErrorListener = function (annotations) {
        // antlr4.error.ErrorListener.call(this);
        this.annotations = annotations;
        return this;
    };
    AnnotatingErrorListener.prototype = Object.create(antlr4.error.ErrorListener.prototype);
    AnnotatingErrorListener.prototype.constructor = AnnotatingErrorListener;

    AnnotatingErrorListener.prototype.syntaxError = function (recognizer, offendingSymbol, line, column, msg, e) {
        this.annotations.push({
            row: line - 1,
            column: column,
            text: msg,
            type: "error"
        });
    };

    function validate(input) {
        function StandardChar(o) {
            if (o == '‘') return '\'';
            if (o == '’') return '\'';
            if (o == '“') return '"';
            if (o == '”') return '"';
            if (o == '〔') return '(';
            if (o == '〕') return ')';
            if (o == '＝') return '=';
            if (o == '＋') return '+';
            if (o == '－') return '-';
            if (o == '×') return '*';
            if (o == '÷') return '/';
            if (o == '／') return '/';
            if (o == '，') return ',';
            let c = o.charAt(0);
            if (c == 12288) {
                o = String.fromCharCode(32)
            } else if (c > 65280 && c < 65375) {
                o = String.fromCharCode(c - 65248)
            }
            return o.toUpperCase();
        }
        try {
            var str = "";
            for (let i = 0; i < input.length; i++) {
                let char = input.charAt(i);
                let normalized_char = StandardChar(char);
                str += normalized_char;
            }

            var stream = new antlr4.InputStream(str);
            var lexer = new algorithm.mathLexer(stream);
            var tokens = new antlr4.CommonTokenStream(lexer);
            var parser = new algorithm.mathParser(tokens);
            var annotations = [];
            var listener = new AnnotatingErrorListener(annotations);
            parser.removeErrorListeners();
            parser.addErrorListener(listener);
            parser.prog();
            return annotations;
        } catch (e) {
            return [{ column: 0, row: 0, text: e.message, type: "error" }];
        }
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