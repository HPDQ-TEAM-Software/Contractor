/* http://github.com/mindmup/bootstrap-wysiwyg */
!function(a){"use strict";var b=function(b){var c=a.Deferred(),d=new FileReader;return d.onload=function(a){c.resolve(a.target.result)},d.onerror=c.reject,d.onprogress=c.notify,d.readAsDataURL(b),c.promise()};a.fn.cleanHtml=function(){var b=a(this).html();return b&&b.replace(/(<br>|\s|<div><br><\/div>|&nbsp;)*$/,"")},a.fn.wysiwyg=function(c){var d,e,f,g=this,h=function(){e.activeToolbarClass&&a(e.toolbarSelector).find(f).each(function(){try{var b=a(this).data(e.commandRole);document.queryCommandState(b)?a(this).addClass(e.activeToolbarClass):a(this).removeClass(e.activeToolbarClass)}catch(c){}})},i=function(a,b){var c=a.split(" "),d=c.shift(),e=c.join(" ")+(b||"");document.execCommand(d,0,e),h()},j=function(b){a.each(b,function(a,b){g.keydown(a,function(a){g.attr("contenteditable")&&g.is(":visible")&&(a.preventDefault(),a.stopPropagation(),i(b))}).keyup(a,function(a){g.attr("contenteditable")&&g.is(":visible")&&(a.preventDefault(),a.stopPropagation())})})},k=function(){try{var a=window.getSelection();if(a.getRangeAt&&a.rangeCount)return a.getRangeAt(0)}catch(b){}},l=function(){d=k()},m=function(){try{var a=window.getSelection();if(d){try{a.removeAllRanges()}catch(b){document.body.createTextRange().select(),document.selection.empty()}a.addRange(d)}}catch(c){}},n=function(c){g.focus(),a.each(c,function(c,d){/^image\//.test(d.type)?a.when(b(d)).done(function(a){i("insertimage",a)}).fail(function(a){e.fileUploadError("file-reader",a)}):e.fileUploadError("unsupported-file-type",d.type)})},o=function(a,b){m(),document.queryCommandSupported("hiliteColor")&&document.execCommand("hiliteColor",0,b||"transparent"),l(),a.data(e.selectionMarker,b)},p=function(b,c){b.find(f).click(function(){m(),g.focus(),i(a(this).data(c.commandRole)),l()}),b.find("[data-toggle=dropdown]").click(m);var d=!!window.navigator.msPointerEnabled||!!document.all&&!!document.addEventListener;b.find("input[type=text][data-"+c.commandRole+"]").on("webkitspeechchange change",function(){var b=this.value;this.value="",m(),b&&(g.focus(),i(a(this).data(c.commandRole),b)),l()}).on("focus",function(){if(!d){var b=a(this);b.data(c.selectionMarker)||(o(b,c.selectionColor),b.focus())}}).on("blur",function(){if(!d){var b=a(this);b.data(c.selectionMarker)&&o(b,!1)}}),b.find("input[type=file][data-"+c.commandRole+"]").change(function(){m(),"file"===this.type&&this.files&&this.files.length>0&&n(this.files),l(),this.value=""})},q=function(){g.on("dragenter dragover",!1).on("drop",function(a){var b=a.originalEvent.dataTransfer;a.stopPropagation(),a.preventDefault(),b&&b.files&&b.files.length>0&&n(b.files)})};return e=a.extend({},a.fn.wysiwyg.defaults,c),f="a[data-"+e.commandRole+"],button[data-"+e.commandRole+"],input[type=button][data-"+e.commandRole+"]",j(e.hotKeys),e.dragAndDropImages&&q(),p(a(e.toolbarSelector),e),g.attr("contenteditable",!0).on("mouseup keyup mouseout",function(){l(),h()}),a(window).bind("touchend",function(a){var b=g.is(a.target)||g.has(a.target).length>0,c=k(),d=c&&c.startContainer===c.endContainer&&c.startOffset===c.endOffset;d&&!b||(l(),h())}),this},a.fn.wysiwyg.defaults={hotKeys:{"ctrl+b meta+b":"bold","ctrl+i meta+i":"italic","ctrl+u meta+u":"underline","ctrl+z meta+z":"undo","ctrl+y meta+y meta+shift+z":"redo","ctrl+l meta+l":"justifyleft","ctrl+r meta+r":"justifyright","ctrl+e meta+e":"justifycenter","ctrl+j meta+j":"justifyfull","shift+tab":"outdent",tab:"indent"},toolbarSelector:"[data-role=editor-toolbar]",commandRole:"edit",activeToolbarClass:"btn-info",selectionMarker:"edit-focus-marker",selectionColor:"darkgrey",dragAndDropImages:!0,fileUploadError:function(a,b){console.log("File upload error",a,b)}}}(window.jQuery);