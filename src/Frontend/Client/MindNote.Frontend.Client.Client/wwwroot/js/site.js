"use strict";

var md = null;

var codeEditor = null;

function renderMarkdown(element, content) {
    element.innerHTML = md.render(content);
}

function createCodeEditor(element, model) {
    $(document).ready(function () {
        require(['vs/editor/editor.main'], function () {
            element.innerHTML = "";
            codeEditor = monaco.editor.create(element, {
                value: model.invokeMethod("GetValue"),
                language: "markdown",
                wordWrap: "on",
                minimap: {
                    enabled: false
                },
            });
        });

        codeEditor.onDidChangeModelContent((e) => {
            model.invokeMethodAsync("SetValue", codeEditor.getValue());
        });

        window.onresize = function () {
            if (codeEditor) {
                codeEditor.layout();
            }
        };
    });
}