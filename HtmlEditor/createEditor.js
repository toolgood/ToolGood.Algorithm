ace.define('antlr4/index', function (require, exports, module) { module.exports = algorithm.antlr4; });
ace.config.setModuleUrl('ace/mode/my-mode', 'my-mode.js');
ace.config.setModuleUrl('ace/worker/my-worker', 'my-worker.js');
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