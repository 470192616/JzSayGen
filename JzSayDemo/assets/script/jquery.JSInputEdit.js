(function ($) {

    $.JSInputEdit = {
        PostUrl: '',
        PostProcess: null,
        KeyPress: function (cate, key, pressCode) {
            //
            var code = pressCode.toString();
            if (code == '13') {
                $.JSInputEdit.Update(cate, key);
            }
            else if (code == '27') {
                $.JSInputEdit.Cancel(cate, key);
            }
        },
        Show: function (cate, key) {
            //
            var v = $('#JSEditView' + cate + key).html();

            $('#JSEditView' + cate + key).hide();
            $('#JSEditBtnEditor' + cate + key).hide();

            $('#JSEditBtnUpdate' + cate + key).show();
            $('#JSEditBtnCancel' + cate + key).show();

            $('#JSEditInput' + cate + key).val(v).show().focus().select();
        },
        Hide: function (cate, key) {
            //
            $('#JSEditView' + cate + key).show();
            $('#JSEditBtnEditor' + cate + key).show();

            $('#JSEditBtnUpdate' + cate + key).hide();
            $('#JSEditBtnCancel' + cate + key).hide();
            $('#JSEditInput' + cate + key).hide();
        },
        Cancel: function (cate, key) {
            //
            $.JSInputEdit.Hide(cate, key);
        },
        Update: function (cate, key) {
            //
            if ($.JSInputEdit.PostUrl == '') return;
            if (typeof ($.JSInputEdit.PostProcess) != 'function') return;

            var v = $('#JSEditInput' + cate + key).val();
            $.post($.JSInputEdit.PostUrl, { "Cate": cate, "Key": key, "Val": v }, function (result) {
                if ($.JSInputEdit.PostProcess(cate, key, result) == false) return;

                $('#JSEditView' + cate + key).html(v);
                $.JSInputEdit.Hide(cate, key);
            });

        }
    };

    $.fn.JSInputEdit = function (cate, inputWidth) {
        //文字编辑器                
        var s = [];
        s.push('<div id="JSEditPanel' + cate + '#KEY#" class="JSEditPanel">');
        s.push('    <a id="JSEditBtnEditor' + cate + '#KEY#" title="点击编辑" href="javascript:;" onclick="$.JSInputEdit.Show(\'' + cate + '\',\'#KEY#\')"><i class="icon-edit"></i></a>');
        s.push('    <a id="JSEditBtnUpdate' + cate + '#KEY#" title="确定更新(&amp;Enter)" href="javascript:;" onclick="$.JSInputEdit.Update(\'' + cate + '\',\'#KEY#\')" style="display:none;"><i class="icon-ok"></i></a>');
        s.push('    <a id="JSEditBtnCancel' + cate + '#KEY#" title="取消编辑(&amp;Esc)" href="javascript:;" onclick="$.JSInputEdit.Cancel(\'' + cate + '\',\'#KEY#\')" style="display:none;"><i class="icon-cancel"></i></a>');
        s.push('</div>');
        var cmd = s.join('');

        this.each(function () {
            //
            var o = $(this);
            var k = o.attr('jseditkey');
            var v = o.html();
            var x = [];
            x.push('<label id="JSEditView' + cate + '' + k + '" class="JSEditView">' + v + '</label>');
            x.push('<input type="text" id="JSEditInput' + cate + '' + k + '" style="width:' + inputWidth + ';" onkeypress="$.JSInputEdit.KeyPress(\'' + cate + '\',\'' + k + '\',event.keyCode||event.which)" class="JSEditInput" value="' + v + '" />');
            x.push(cmd.replace(new RegExp('#KEY#', "gi"), k));
            o.attr('jseditcate', cate).empty().append(x.join(''));

        });

        this.hover(function () {
            var o = $(this);
            var k = o.attr('jseditkey');
            var c = o.attr('jseditcate');
            $('#JSEditPanel' + c + k).show();
            o.addClass('JSInputEditOver');
        }, function () {
            var o = $(this);
            var k = o.attr('jseditkey');
            var c = o.attr('jseditcate');
            $('#JSEditPanel' + c + k).hide();
            o.removeClass('JSInputEditOver');
        });


    };
})(jQuery);


/*
$.JSInputEdit.PostUrl = '?cmd=update';
$.JSInputEdit.PostProcess = function (cate, key, result) {
    // 返回  true更新成功，false更新失败
    return false;
};

$('.MoneyPanel').JSInputEdit('cateType1','50px');
$('.NumberPanel').JSInputEdit('cateType2','60px');
*/