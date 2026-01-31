ace.define(
    'ace/mode/my-mode',
    [
        "require",
        "exports",
        "module",
        "ace/lib/oop",
        "ace/mode/text",
        "ace/mode/text_highlight_rules",
        "ace/worker/worker_client"
    ],
    function (require, exports, module) {
        var oop = require("ace/lib/oop");
        var TextMode = require("ace/mode/text").Mode;
        var TextHighlightRules = require("ace/mode/text_highlight_rules").TextHighlightRules;

        var MyHighlightRules = function () {
            this.$rules = {
                "start": [
                    { token: "comment", regex: "//.*$" },
                    { token: "comment", regex: "\\/\\*", next: "comment" },
                    { token: "string", regex: /"(\\"|[^"])*"/ },
                    { token: "string", regex: /'(\\'|[^'])*'/ },
                    { token: "string", regex: /[“”](\\“|\\”|[^“”])*[“”]/ },
                    { token: "string", regex: /[‘’](\\‘|\\’|[^‘’])*[‘’]/ },
                    { token: "string", regex: /`(\\`|[^`])*`/ },
                    { token: "constant.numeric", regex: "(\\d+(?:(?:\\.\\d*)?(?:[eE][+-]?\\d+)?)?\\b|\\d+(?:\\.\\d*)?([MmCcDdKk]?[Mm][23]?|[Kk]?[Gg]|[Tt])\\b)" },
                    { token: "keyword.operator", regex: "!|%|/|\\*|\\-|\\+" },
                    { token: "keyword.control", regex: "===|==|<>|!==|!=|<=|>=|=|<|>|&&|\\|\\||\\?|\\:|！=|！==|？" },
                    { token: "punctuation.operator", regex: "\\,|\\.|，|," },
                    { token: "paren.lparen", regex: "[\\[(（]" },
                    { token: "paren.rparen", regex: "[\\])）]" },
                    { token: "keyword.control", regex: "[Ii][Ff]\\b|[Ii][Ff][Ee][Rr][Rr][Oo][Rr]\\b|[Ii][Ss][Nn][Uu][Mm][Bb][Ee][Rr]\\b|[Ii][Ss][Tt][Ee][Xx][Tt]\\b|[Ii][Ss][Ee][Rr][Rr][Oo][Rr]\\b|[Ii][Ss][Nn][Oo][Nn][Tt][Ee][Xx][Tt]\\b|[Ii][Ss][Ll][Oo][Gg][Ii][Cc][Aa][Ll]\\b|[Ii][Ss][Ee][Vv][Ee][Nn]\\b|[Ii][Ss][Oo][Dd][Dd]\\b|[Ii][Ss][Nn][Uu][Ll][Ll]\\b|[Ii][Ss][Nn][Uu][Ll][Ll][Oo][Rr][Ee][Rr][Rr][Oo][Rr]\\b|[Ii][Ss][Rr][Ee][Gg][Ee][Xx]\\b|[Ii][Ss][Mm][Aa][Tt][Cc][Hh]\\b|[Ii][Ss][Nn][Uu][Ll][Ll][Oo][Rr][Ee][Mm][Pp][Tt][Yy]\\b|[Ii][Ss][Nn][Uu][Ll][Ll][Oo][Rr][Ww][Hh][Ii][Tt][Ee][Ss][Pp][Aa][Cc][Ee]\\b" },
                    { token: "keyword.control", regex: "[Aa][Nn][Dd]\\b|[Oo][Rr]\\b|[Nn][Oo][Tt]\\b" },
                    { token: "constant.numeric", regex: "[Nn][Uu][Ll][Ll]\\b|[Tt][Rr][Uu][Ee]\\b|[Ff][Aa][Ll][Ss][Ee]\\b|[Ee]\\b|[Pp][Ii]\\b" },
                    { token: "constant.numeric", regex: "[Dd][Ee][Cc]2[Bb][Ii][Nn]\\b|[Dd][Ee][Cc]2[Hh][Ee][Xx]\\b|[Dd][Ee][Cc]2[Oo][Cc][Tt]\\b|[Hh][Ee][Xx]2[Bb][Ii][Nn]\\b|[Hh][Ee][Xx]2[Dd][Ee][Cc]\\b|[Hh][Ee][Xx]2[Oo][Cc][Tt]\\b|[Oo][Cc][Tt]2[Bb][Ii][Nn]\\b|[Oo][Cc][Tt]2[Dd][Ee][Cc]\\b|[Oo][Cc][Tt]2[Hh][Ee][Xx]\\b|[Bb][Ii][Nn]2[Oo][Cc][Tt]\\b|[Bb][Ii][Nn]2[Dd][Ee][Cc]\\b|[Bb][Ii][Nn]2[Hh][Ee][Xx]\\b" },

                    { token: "constant.numeric", regex: "[Aa][Bb][Ss]\\b|[Qq][Uu][Oo][Tt][Ii][Ee][Nn][Tt]\\b|[Mm][Oo][Dd]\\b|[Ss][Ii][Gg][Nn]\\b|[Ss][Qq][Rr][Tt]\\b|[Tt][Rr][Uu][Nn][Cc]\\b|[Ii][Nn][Tt]\\b|[Gg][Cc][Dd]\\b|[Ll][Cc][Mm]\\b|[Cc][Oo][Mm][Bb][Ii][Nn]\\b|[Pp][Ee][Rr][Mm][Uu][Tt]\\b|[Dd][Ee][Gg][Rr][Ee][Ee][Ss]\\b|[Rr][Aa][Dd][Ii][Aa][Nn][Ss]\\b|[Cc][Oo][Ss]\\b|[Cc][Oo][Ss][Hh]\\b|[Ss][Ii][Nn]\\b|[Ss][Ii][Nn][Hh]\\b|[Tt][Aa][Nn]\\b|[Tt][Aa][Nn][Hh]\\b|[Aa][Cc][Oo][Ss]\\b|[Aa][Cc][Oo][Ss][Hh]\\b|[Aa][Ss][Ii][Nn]\\b|[Aa][Ss][Ii][Nn][Hh]\\b|[Aa][Tt][Aa][Nn]\\b|[Aa][Tt][Aa][Nn][Hh]\\b|[Aa][Tt][Aa][Nn]2\\b|[Rr][Oo][Uu][Nn][Dd]\\b|[Rr][Oo][Uu][Nn][Dd][Dd][Oo][Ww][Nn]\\b|[Rr][Oo][Uu][Nn][Dd][Uu][Pp]\\b|[Cc][Ee][Ii][Ll][Ii][Nn][Gg]\\b|[Ff][Ll][Oo][Oo][Rr]\\b|[Ee][Vv][Ee][Nn]\\b|[Oo][Dd][Dd]\\b|[Mm][Rr][Oo][Uu][Nn][Dd]\\b|[Rr][Aa][Nn][Dd]\\b|[Rr][Aa][Nn][Dd][Bb][Ee][Tt][Ww][Ee][Ee][Nn]\\b|[Ff][Aa][Cc][Tt]\\b|[Ff][Aa][Cc][Tt][Dd][Oo][Uu][Bb][Ll][Ee]\\b|[Pp][Oo][Ww][Ee][Rr]\\b|[Ee][Xx][Pp]\\b|[Ll][Nn]\\b|[Ll][Oo][Gg]\\b|[Ll][Oo][Gg]10\\b|[Mm][Uu][Ll][Tt][Ii][Nn][Oo][Mm][Ii][Aa][Ll]\\b|[Pp][Rr][Oo][Dd][Uu][Cc][Tt]\\b|[Ss][Qq][Rr][Tt][Pp][Ii]\\b|[Ss][Uu][Mm][Ss][Qq]\\b" },
                    { token: "string", regex: "[Aa][Ss][Cc]\\b|[Jj][Ii][Ss]\\b|[Ww][Ii][Dd][Ee][Cc][Hh][Aa][Rr]\\b|[Cc][Hh][Aa][Rr]\\b|[Cc][Ll][Ee][Aa][Nn]\\b|[Cc][Oo][Dd][Ee]\\b|[Cc][Oo][Nn][Cc][Aa][Tt][Ee][Nn][Aa][Tt][Ee]\\b|[Ee][Xx][Aa][Cc][Tt]\\b|[Ff][Ii][Nn][Dd]\\b|[Ff][Ii][Xx][Ee][Dd]\\b|[Ll][Ee][Ff][Tt]\\b|[Ll][Ee][Nn]\\b|[Ll][Oo][Ww][Ee][Rr]\\b|[Tt][Oo][Ll][Oo][Ww][Ee][Rr]\\b|[Mm][Ii][Dd]\\b|[Pp][Rr][Oo][Pp][Ee][Rr]\\b|[Rr][Ee][Pp][Ll][Aa][Cc][Ee]\\b|[Rr][Ee][Pp][Tt]\\b|[Rr][Ii][Gg][Hh][Tt]\\b|[Rr][Mm][Bb]\\b|[Ss][Ee][Aa][Rr][Cc][Hh]\\b|[Ss][Uu][Bb][Ss][Tt][Ii][Tt][Uu][Tt][Ee]\\b|[Tt]\\b|[Tt][Ee][Xx][Tt]\\b|[Tt][Rr][Ii][Mm]\\b|[Uu][Pp][Pp][Ee][Rr]\\b|[Tt][Oo][Uu][Pp][Pp][Ee][Rr]\\b|[Vv][Aa][Ll][Uu][Ee]\\b" },
                    { token: "support.variable", regex: "[Aa][Dd][Dd][Yy][Ee][Aa][Rr][Ss]\\b|[Aa][Dd][Dd][Mm][Oo][Nn][Tt][Hh][Ss]\\b|[Aa][Dd][Dd][Dd][Aa][Yy][Ss]\\b|[Aa][Dd][Dd][Hh][Oo][Uu][Rr][Ss]\\b|[Aa][Dd][Dd][Mm][Ii][Nn][Uu][Tt][Ee][Ss]\\b|[Aa][Dd][Dd][Ss][Ee][Cc][Oo][Nn][Dd][Ss]\\b|[Dd][Aa][Tt][Ee][Vv][Aa][Ll][Uu][Ee]\\b|[Tt][Ii][Mm][Ee][Vv][Aa][Ll][Uu][Ee]\\b|[Dd][Aa][Tt][Ee]\\b|[Tt][Ii][Mm][Ee]\\b|[Nn][Oo][Ww]\\b|[Tt][Oo][Dd][Aa][Yy]\\b|[Yy][Ee][Aa][Rr]\\b|[Mm][Oo][Nn][Tt][Hh]\\b|[Dd][Aa][Yy]\\b|[Hh][Oo][Uu][Rr]\\b|[Mm][Ii][Nn][Uu][Tt][Ee]\\b|[Ss][Ee][Cc][Oo][Nn][Dd]\\b|[Ww][Ee][Ee][Kk][Dd][Aa][Yy]\\b|[Dd][Aa][Tt][Ee][Dd][Ii][Ff]\\b|[Dd][Aa][Yy][Ss]360\\b|[Ee][Dd][Aa][Tt][Ee]\\b|[Ee][Oo][Mm][Oo][Nn][Tt][Hh]\\b|[Nn][Ee][Tt][Ww][Oo][Rr][Kk][Dd][Aa][Yy][Ss]\\b|[Ww][Oo][Rr][Kk][Dd][Aa][Yy]\\b|[Ww][Ee][Ee][Kk][Nn][Uu][Mm]\\b" },
                    { token: "support.class", regex: "[Aa][Rr][Rr][Aa][Yy]\\b|[Mm][Aa][Xx]\\b|[Mm][Ee][Dd][Ii][Aa][Nn]\\b|[Mm][Ii][Nn]\\b|[Qq][Uu][Aa][Rr][Tt][Ii][Ll][Ee]\\b|[Mm][Oo][Dd][Ee]\\b|[Ll][Aa][Rr][Gg][Ee]\\b|[Ss][Mm][Aa][Ll][Ll]\\b|[Pp][Ee][Rr][Cc][Ee][Nn][Tt][Ii][Ll][Ee]\\b|[Pp][Ee][Rr][Cc][Ee][Nn][Tt][Rr][Aa][Nn][Kk]\\b|[Aa][Vv][Ee][Rr][Aa][Gg][Ee]\\b|[Aa][Vv][Ee][Rr][Aa][Gg][Ee][Ii][Ff]\\b|[Gg][Ee][Oo][Mm][Ee][Aa][Nn]\\b|[Hh][Aa][Rr][Mm][Ee][Aa][Nn]\\b|[Cc][Oo][Uu][Nn][Tt]\\b|[Cc][Oo][Uu][Nn][Tt][Ii][Ff]\\b|[Ss][Uu][Mm]\\b|[Ss][Uu][Mm][Ii][Ff]\\b|[Aa][Vv][Ee][Dd][Ee][Vv]\\b|[Ss][Tt][Dd][Ee][Vv]\\b|[Ss][Tt][Dd][Ee][Vv][Pp]\\b|[Dd][Ee][Vv][Ss][Qq]\\b|[Vv][Aa][Rr]\\b|[Vv][Aa][Rr][Pp]\\b|[Nn][Oo][Rr][Mm][Dd][Ii][Ss][Tt]\\b|[Nn][Oo][Rr][Mm][Ii][Nn][Vv]\\b|[Nn][Oo][Rr][Mm][Ss][Dd][Ii][Ss][Tt]\\b|[Nn][Oo][Rr][Mm][Ss][Ii][Nn][Vv]\\b|[Bb][Ee][Tt][Aa][Dd][Ii][Ss][Tt]\\b|[Bb][Ee][Tt][Aa][Ii][Nn][Vv]\\b|[Bb][Ii][Nn][Oo][Mm][Dd][Ii][Ss][Tt]\\b|[Ee][Xx][Pp][Oo][Nn][Dd][Ii][Ss][Tt]\\b|[Ff][Dd][Ii][Ss][Tt]\\b|[Ff][Ii][Nn][Vv]\\b|[Ff][Ii][Ss][Hh][Ee][Rr]\\b|[Ff][Ii][Ss][Hh][Ee][Rr][Ii][Nn][Vv]\\b|[Gg][Aa][Mm][Mm][Aa][Dd][Ii][Ss][Tt]\\b|[Gg][Aa][Mm][Mm][Aa][Ii][Nn][Vv]\\b|[Gg][Aa][Mm][Mm][Aa][Ll][Nn]\\b|[Hh][Yy][Pp][Gg][Ee][Oo][Mm][Dd][Ii][Ss][Tt]\\b|[Ll][Oo][Gg][Ii][Nn][Vv]\\b|[Ll][Oo][Gg][Nn][Oo][Rr][Mm][Dd][Ii][Ss][Tt]\\b|[Nn][Ee][Gg][Bb][Ii][Nn][Oo][Mm][Dd][Ii][Ss][Tt]\\b|[Pp][Oo][Ii][Ss][Ss][Oo][Nn]\\b|[Tt][Dd][Ii][Ss][Tt]\\b|[Tt][Ii][Nn][Vv]\\b|[Ww][Ee][Ii][Bb][Uu][Ll][Ll]\\b" },
                    { token: "string", regex: "[Uu][Rr][Ll][Ee][Nn][Cc][Oo][Dd][Ee]\\b|[Uu][Rr][Ll][Dd][Ee][Cc][Oo][Dd][Ee]\\b|[Hh][Tt][Mm][Ll][Ee][Nn][Cc][Oo][Dd][Ee]\\b|[Hh][Tt][Mm][Ll][Dd][Ee][Cc][Oo][Dd][Ee]\\b|[Bb][Aa][Ss][Ee]64[Tt][Oo][Tt][Ee][Xx][Tt]\\b|[Bb][Aa][Ss][Ee]64[Uu][Rr][Ll][Tt][Oo][Tt][Ee][Xx][Tt]\\b|[Tt][Ee][Xx][Tt][Tt][Oo][Bb][Aa][Ss][Ee]64\\b|[Tt][Ee][Xx][Tt][Tt][Oo][Bb][Aa][Ss][Ee]64[Uu][Rr][Ll]\\b|[Rr][Ee][Gg][Ee][Xx]\\b|[Rr][Ee][Gg][Ee][Xx][Rr][Ee][Pp][Aa][Ll][Cc][Ee]\\b|[Gg][Uu][Ii][Dd]\\b|[Mm][Dd]5\\b|[Ss][Hh][Aa]1\\b|[Ss][Hh][Aa]256\\b|[Ss][Hh][Aa]512\\b|[Hh][Mm][Aa][Cc][Mm][Dd]5\\b|[Hh][Mm][Aa][Cc][Ss][Hh][Aa]1\\b|[Hh][Mm][Aa][Cc][Ss][Hh][Aa]256\\b|[Hh][Mm][Aa][Cc][Ss][Hh][Aa]512\\b|[Tt][Rr][Ii][Mm][Ss][Tt][Aa][Rr][Tt]\\b|[Ll][Tt][Rr][Ii][Mm]\\b|[Tt][Rr][Ii][Mm][Ee][Nn][Dd]\\b|[Rr][Tt][Rr][Ii][Mm]\\b|[Ii][Nn][Dd][Ee][Xx][Oo][Ff]\\b|[Ll][Aa][Ss][Tt][Ii][Nn][Dd][Ee][Xx][Oo][Ff]\\b|[Ss][Pp][Ll][Ii][Tt]\\b|[Jj][Oo][Ii][Nn]\\b|[Ss][Uu][Bb][Ss][Tt][Rr][Ii][Nn][Gg]\\b|[Ss][Tt][Aa][Rr][Tt][Ss][Ww][Ii][Tt][Hh]\\b|[Ee][Nn][Dd][Ss][Ww][Ii][Tt][Hh]\\b|[Rr][Ee][Mm][Oo][Vv][Ee][Ss][Tt][Aa][Rr][Tt]\\b|[Rr][Ee][Mm][Oo][Vv][Ee][Ee][Nn][Dd]\\b|[Jj][Ss][Oo][Nn]\\b|[Ll][Oo][Oo][Kk][Ff][Ll][Oo][Oo][Rr]\\b|[Ll][Oo][Oo][Kk][Cc][Ee][Ii][Ll][Ii][Nn][Gg]\\b" },
                    { token: "string", regex: "[Ii][Nn]|[Hh][Aa][Ss]|[Pp][Aa][Rr][Aa][Mm]" },
                    { token: "support.class", regex: "[Ss][Qq]|[Ll][123]" },

                    { token: "identifier", regex: "[a-zA-Z_$][a-zA-Z0-9_$]*\\b" },
                    { token: "identifier", regex: "\\{[^\\{\\}]+\\}" },
                    { token: "identifier", regex: "【[^【】]+】" },
                    { token: "identifier", regex: "#[^#]+#" },
                    { token: "identifier", regex: "[a-zA-Z_\u00c0-\u00d6\u00d8-\u00f6\u00f8-\u2fff\u3040-\u318f\u3300-\u337f\u3400-\ud7ff\uf900-\ufaff\uff00-\ufff0][a-zA-Z0-9_\u00c0-\u00d6\u00d8-\u00f6\u00f8-\u2fff\u3040-\u318f\u3300-\u337f\u3400-\ud7ff\uf900-\ufaff\uff00-\ufff0]*\\b" },
                    { token: "identifier", regex: "@[a-zA-Z_\u00c0-\u00d6\u00d8-\u00f6\u00f8-\u2fff\u3040-\u318f\u3300-\u337f\u3400-\ud7ff\uf900-\ufaff\uff00-\ufff0][a-zA-Z0-9_\u00c0-\u00d6\u00d8-\u00f6\u00f8-\u2fff\u3040-\u318f\u3300-\u337f\u3400-\ud7ff\uf900-\ufaff\uff00-\ufff0]*\\b" }
                ],
                "comment": [{ token: "comment", regex: "\\*\\/", next: "start" }, { defaultToken: "comment" }]
            };
        };
        oop.inherits(MyHighlightRules, TextHighlightRules);
        var MyMode = function () { this.HighlightRules = MyHighlightRules; };
        oop.inherits(MyMode, TextMode);

        (function () {
            this.$id = "ace/mode/my-mode";
            var WorkerClient = require("ace/worker/worker_client").WorkerClient;
            this.createWorker = function (session) {
                this.$worker = new WorkerClient(["ace"], "ace/worker/my-worker", "MyWorker");
                this.$worker.attachToDocument(session.getDocument());
                this.$worker.on("errors", function (e) { session.setAnnotations(e.data); });
                this.$worker.on("annotate", function (e) { session.setAnnotations(e.data); });
                this.$worker.on("terminate", function () { session.clearAnnotations(); });
                return this.$worker;
            };
        }).call(MyMode.prototype);
        exports.Mode = MyMode;
    }
);