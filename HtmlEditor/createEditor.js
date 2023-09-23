ace.define('antlr4/index', function (require, exports, module) { module.exports = algorithm.antlr4; });
ace.config.setModuleUrl('ace/mode/my-mode', '/_/inputcontrol/my-mode.js');
ace.config.setModuleUrl('ace/worker/my-worker', '/_/inputcontrol/my-worker.js');
ace.mymodeFirstCreate = true;
ace.jsmodeFirstCreate = true;
window.createEditor=function(id, outId) {
    var editor = ace.edit(id);
    editor.getSession().setMode('ace/mode/my-mode');
    editor.setTheme("ace/theme/crimson_editor");

    editor.setOptions({
        'showLineNumbers': false,
        'fontSize': 14,
        'wrapBehavioursEnabled': false,
        'enableLiveAutocompletion': true,
        'indentedSoftWrap': false,
        'printMargin': false,
        'showFoldWidgets': false,
        'dragEnabled': false,
        'indentedSoftWrap': false,
        'highlightActiveLine': false,
        'autoScrollEditorIntoView': true,
        'enableBasicAutocompletion': true,
        'maxLines': 100,
    })
    editor.setShowPrintMargin(false);
    editor.renderer.setScrollMargin(5, 6, 5, 9);
    var session = editor.getSession();
    session.setUseWrapMode(false);

    if (ace.mymodeFirstCreate) {
        ace.mymodeFirstCreate = false;
        var langTools = ace.require("ace/ext/language_tools");
        var rhymeCompleter = {
            getCompletions: function (editor, session, pos, prefix, callback) {
                if (editor.getSession().getMode().$id != 'ace/mode/my-mode') { callback(null, []); return }
                if (prefix.length === 0) { callback(null, []); return }
                callback(null, ace_keywords.map(function (ea) { return { pinyin: ea.pinyin, caption: ea.caption, name: ea.text, value: ea.text, meta: ea.meta } }));
            }
        }
        langTools.addCompleter(rhymeCompleter);
    }

    editor.setValue($("#" + outId).val());
    editor.getSession().on('change', function (e) { $("#" + outId).val(editor.getValue()); });
    editor.gotoLine(0);
}
window.createjsEditor=function (id, outId) {
    var editor = ace.edit(id);
    editor.getSession().setMode('ace/mode/javascript');
    editor.setTheme("ace/theme/crimson_editor");

    editor.setOptions({
        'showLineNumbers': true,
        'fontSize': 16,
        'wrapBehavioursEnabled': false,
        'enableLiveAutocompletion': true,
        'indentedSoftWrap': false,
        'printMargin': false,
        'showFoldWidgets': false,
        'dragEnabled': false,
        'indentedSoftWrap': false,
        'highlightActiveLine': false,
        'autoScrollEditorIntoView': true,
        'enableBasicAutocompletion': true,
        'enableSnippets': true,
        'maxLines': 100,
        'minLines': 8,
    })
    editor.setShowPrintMargin(false);
    editor.renderer.setScrollMargin(5, 6, 5, 9);
    var session = editor.getSession();
    session.setUseWrapMode(false);

    if (ace.jsmodeFirstCreate) {
        ace.jsmodeFirstCreate = false;
        var langTools = ace.require("ace/ext/language_tools");
        ace_keywords_js = [{ caption: "getDatas", text: "getDatas()", meta: "获取附带数据" },
        { caption: "hasKey", text: "hasKey(参数名)", meta: "判断参数名是否存在" },
        { caption: "getValue", text: "getValue(参数名)", meta: "获取参数" },
        { caption: "setValue", text: "setValue(参数名, 值)", meta: "设置参数" },
        { caption: "error", text: "error(错误信息)", meta: "设置错误" },
        ]
        var rhymeCompleter = {
            getCompletions: function (editor, session, pos, prefix, callback) {
                if (editor.getSession().getMode().$id != 'ace/mode/javascript') { callback(null, []); return }
                if (prefix.length === 0) { callback(null, []); return }
                callback(null, ace_keywords_js.map(function (ea) { return { caption: ea.caption, name: ea.text, value: ea.text, meta: ea.meta } }));
            }
        }
        langTools.addCompleter(rhymeCompleter);
    }

    editor.setValue($("#" + outId).val());
    editor.getSession().on('change', function (e) { $("#" + outId).val(editor.getValue()); });
    editor.gotoLine(0);
}
window.createjsonEditor = function (id, outId) {
    var editor = ace.edit(id);
    editor.getSession().setMode('ace/mode/json5');
    editor.setTheme("ace/theme/crimson_editor");

    editor.setOptions({
        'showLineNumbers': false,
        'fontSize': 16,
        'wrapBehavioursEnabled': false,
        'enableLiveAutocompletion': true,
        'indentedSoftWrap': false,
        'printMargin': false,
        'showFoldWidgets': false,
        'dragEnabled': false,
        'indentedSoftWrap': false,
        'highlightActiveLine': false,
        'autoScrollEditorIntoView': true,
        'enableBasicAutocompletion': true,
        'enableSnippets': true,
        'maxLines': 100,
        'minLines': 1,
    })
    editor.setShowPrintMargin(false);
    editor.renderer.setScrollMargin(5, 6, 5, 9);
    var session = editor.getSession();
    session.setUseWrapMode(false);

    editor.setValue($("#" + outId).val());
    editor.getSession().on('change', function (e) { $("#" + outId).val(editor.getValue()); });
    editor.gotoLine(0);
}
var stringWidth = function (fontSize, content) {
    var $span = $('<span></span>').hide().css('font-size', fontSize).text(content);
    var w = $span.appendTo('body').width();
    $span.remove();
    return w;
};
window.jhcAutoWidth = function () {
    $('.ace_autocomplete').each(function () {
        if ($(this).is(":hidden")) return true;
        var jige = $(this).find('.ace_line').length;
        if (jige < 1) return '';
        var maxText = '';
        for (var i = 0; i < jige; i++) {
            var nowText = $(this).find('.ace_line').eq(i).text();
            if (nowText.length > maxText.length) maxText = nowText;
        }
        var jhcWidth = 200 + stringWidth('20', maxText);
        if ($(this).width() < jhcWidth) {
            $(this).css('width', jhcWidth + 'px');
        }
    })
}
setInterval("jhcAutoWidth()", "100");