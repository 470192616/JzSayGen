var JSBox = {
    MaskObj: null, //蒙板对象
    MaskIsShow: false, //蒙板是否显示
    CurIndex: 300, // 当前z-index的值
    BoxCount: 0, //窗口数量
    MaskShow: function () {
        //显示蒙板
        if (this.MaskIsShow) return;
        this.MaskIsShow = true;
        if (this.MaskObj) {
            this.MaskObj.show();
            return;
        }
        this.MaskObj = $('<div class="JSBoxMask"></div>').css({ "z-index": (this.CurIndex++).toString() });
        $(document.body).append(this.MaskObj);
        this.MaskObj.show();
    },
    MaskHide: function () {
        //隐藏蒙板
        if (!this.MaskIsShow) return;
        this.MaskObj.hide();
        this.MaskIsShow = false;
    },
    CreateBox: function (idStr, config) {
        //创建窗口
        var s = [];
        var c = {
            Title: '', //窗口标题
            Content: '', //窗口内容
            TypeKey: '', //类型 w i e s
            ShowClose: true, //是否显示关闭按钮
            Buttons: null //按钮组
        };
        $.extend(c, config);

        s.push('<div id="JSBox' + idStr + '" class="JSBoxPanel">');

        if (c.Title != '') {
            s.push('<div class="JSBoxPanelHeader" id="JSBoxHead' + idStr + '">');
            s.push('     <i class="icon-th-large"></i>');
            s.push('     <label style="font-size:14px;">' + c.Title + '</label>');
            if (c.ShowClose) {
                s.push(' <a href="javascript:;" onclick="JSBox.Close(\'' + idStr + '\')" class="closeBtn" title="关闭窗口"><i class="icon-cancel"></i></a>');
            }
            s.push('</div>');
        }

        s.push('    <div class="JSBoxPanelBodyer" id="JSBoxBody' + idStr + '">');
        switch (c.TypeKey) {
            case 'w':
                s.push(' <i class="icon-help-circled" style="font-size:34px;color:#FF9900"></i>');
                break;
            case 'i':
                s.push(' <i class="icon-info-circled" style="font-size:34px;color:#0064B5"></i>');
                break;
            case 'e':
                s.push(' <i class="icon-cancel-circled" style="font-size:34px;color:#C81623"></i>');
                break;
            case 's':
                s.push(' <i class="icon-ok-circled" style="font-size:34px;color:#41871D"></i>');
                break;
            default:
                s.push('');
                break;
        }
        s.push(c.Content);
        s.push('    </div>');

        if (c.Buttons) {
            s.push('<div class="JSBoxPanelFooter" id="JSBoxFoot' + idStr + '">');
            for (var i in c.Buttons) {
                s.push(' <input type="button" class="btn" value="' + i + '" onclick="' + c.Buttons[i] + '(\'' + idStr + '\',\'' + i + '\')" /> ');
            }
            s.push('</div>');
        }

        s.push('</div>');

        var a = $(s.join('')).css({ "z-index": (this.CurIndex++).toString() });
        $(document.body).append(a);
        return a;
    },
    Close: function (jsBoxId) {
        //关闭
        $('#JSBox' + jsBoxId).slideUp(500, function () { $(this).remove(); });

        if (!this.MaskIsShow) return;
        this.BoxCount--;
        if (this.BoxCount <= 0) {
            this.MaskHide();
            if (this.BoxCount < 0) this.BoxCount = 0;
        }
    },
    Show: function (config, idStr) {
        //
        this.MaskShow();
        this.BoxCount++;

        var key = idStr || (new Date().getTime()).toString();
        this.CreateBox(key, config).slideDown(500);

        return key;
    },
    Alert: function (msg, typeKey) {
        //               
        var c = {
            Title: '系统提示',
            ShowClose: true,
            TypeKey: (typeKey || 'i'),
            Content: msg,
            Buttons: { "确定": "JSBox.Close" }
        };
        return this.Show(c);
    },
    Confirm: function (msg, fnName, callBackKey) {
        //
        var btns = { "确定": fnName, "取消": 'JSBox.Close' };
        var c = {
            Title: '系统提示',
            ShowClose: false,
            TypeKey: 'w',
            Content: msg,
            Buttons: btns
        };
        return this.Show(c, callBackKey);
    },
    Iframe: function (title, iframeSrc, height, buttons) {
        //
        var key = (new Date().getTime()).toString();
        var c = {
            Title: title,
            ShowClose: false,
            Content: '<iframe id="JSBoxIframe' + key + '" name="JSBoxIframe' + key + '" src="' + iframeSrc + '" width="100%" height="' + height + '" frameborder="0" scrolling="no" marginwidth="0" marginheight="0"></iframe>',
            Buttons: buttons
        };
        return this.Show(c, key);
    }
};



/*
function aaFun(cbk) {
    JSBox.Close(cbk);
    JSBox.Alert('abc')
}
function bbFun(cbk) {
    alert(cbk);
}

function tsss() {

    var btns = { "走起": 'aaFun', "通天塔": "bbFun", "关闭": 'JSBox.Close' };
    var c = {
        Title: '信息编辑',
        Content: 'abd<br />sdfsdf<br />fsd',
        Buttons: btns
    };
    JSBox.Show(c);

}

function AutoClose() {
    //
    var k = JSBox.Show({ TypeKey: 'i', Content: 'abczou' });
    setTimeout(function () {
        JSBox.Close(k);
    }, 2000);
}

function Iframe() {
    //
    var btns = { "走起": 'aaFun', "通天塔": "bbFun", "关闭": 'JSBox.Close' };
    JSBox.Iframe("信息编辑", "tsIframe.aspx", "100", btns);
}
*/