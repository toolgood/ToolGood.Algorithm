ace.config.setModuleUrl('ace/mode/my-mode', 'my-mode.js');
ace.config.setModuleUrl('ace/worker/my-worker', 'my-worker.js');
ace.mymodeFirstCreate = true;
ace.jsmodeFirstCreate = true;

// ---- 公式校验（主线程版）----
// 用同一个解析器在主线程校验，错误通过 session.setAnnotations 反馈到行号槽的红色图标上
function getAlgorithmApi() {
    if (window.ToolGood && window.ToolGood.Algorithm) { return window.ToolGood.Algorithm; }
    if (window['ToolGood.Algorithm']) { return window['ToolGood.Algorithm']; }
    return null;
}
function validateFormula(text) {
    var annotations = [];
    var api = getAlgorithmApi();
    if (!api || !api.AlgorithmEngineHelper) {
        annotations.push({
            row: 0, column: 0,
            text: 'Formula validator is not loaded (toolgood.algorithm.js).',
            type: 'error'
        });
        return annotations;
    }
    try {
        api.AlgorithmEngineHelper.ParseFormula(text, {
            syntaxError: function (recognizer, offendingSymbol, line, column, msg) {
                annotations.push({ row: line - 1, column: column, text: msg, type: 'error' });
            },
            reportAmbiguity: function () { },
            reportAttemptingFullContext: function () { },
            reportContextSensitivity: function () { }
        });
    } catch (e) {
        annotations.push({ row: 0, column: 0, text: e.message, type: 'error' });
    }
    return annotations;
}
function installLocalValidator(session) {
    if (session.__localValidatorInstalled) { return; }
    session.__localValidatorInstalled = true;
    var timer = null;
    function run() {
        session.setAnnotations(validateFormula(session.getValue()));
    }
    session.on('change', function () {
        if (timer) { clearTimeout(timer); }
        timer = setTimeout(run, 300);
    });
    run();
}
// my-mode 是 setModuleUrl 异步加载的，createEditor 执行时 session.getMode() 还是默认的 text mode，
// 拿不到 my-mode 上的 $useWorker（为 undefined）。
// 之前只在 mode.$useWorker === false 时安装主线程校验，异步加载场景下这个条件永远不成立，
// 校验器根本装不上，于是公式写错也没有任何提示、不出错误图标。
// 现在改成关掉 worker、主线程校验作为唯一来源，避免 worker 起不来时静默失效。
function useLocalValidatorOnly(session) {
    try {
        if (typeof session.setUseWorker === 'function') { session.setUseWorker(false); }
    } catch (e) { }
    installLocalValidator(session);
}

window.createEditor = function (id, outId) {
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

    // 公式校验统一走主线程，保证错误一定能显示成行号槽里的红色图标
    useLocalValidatorOnly(session);

    if (ace.mymodeFirstCreate) {
        ace.mymodeFirstCreate = false;
        var langTools = ace.require("ace/ext/language_tools");
        var rhymeCompleter = {
            getCompletions: function (editor, session, pos, prefix, callback) {
                if (editor.getSession().getMode().$id != 'ace/mode/my-mode') { callback(null, []); return }
                if (prefix.length === 0) { callback(null, []); return }
                callback(null, ace_keywords.map(function (ea) { return { caption: ea.caption, pinyin: ea.pinyin, name: ea.text, value: ea.text, meta: ea.meta } }));
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