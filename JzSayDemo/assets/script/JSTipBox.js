/*
提示信息框 v1.0
http://www.jzsay.com/
*/
function JSTip(msg, interval, callBack) {
    //
    this.Tip = $('<div class="JSTip">' + msg + '</div>');
    this.Rotate = function (direction) {
        // 旋转图层
        var oo = this.Tip;
        var val = direction ? 360 : 0;
        var metimer = setInterval(function () {
            if (direction) {
                val = val - 15;
                if (val < 2) { tfVal = "rotate(0deg)"; clearInterval(metimer); }
            } else {
                val = val + 15;
                if (val > 359) { tfVal = "rotate(0deg)"; clearInterval(metimer); }
            }
            var tfVal = "rotate(" + val + "deg)";
            oo.css({ "-webkit-transform": tfVal, "-moz-transform": tfVal, "transform": tfVal });
        }, 20);
    };
    this.ShowTip = function (second, fun) {
        // 显示提示数据
        var me = this;
        me.Tip.animate({ top: '25%', width: '20%', height: 60, right: '40%', opacity: 'show' }, 500);
        me.Rotate();
        setTimeout(function () {
            me.Rotate(true);
            me.Tip.animate({ top: '100%', width: 5, height: 5, right: '0%', opacity: 'hide' }, 500, function () {

                $('#JSTipBox').append(me.Tip.css({ "position": "static", "width": "100%", "height": 60 }));
                me.Tip.slideDown(500);
                setTimeout(function () {
                    me.Tip.slideUp(500, function () {
                        me.Tip.remove();
                        if (typeof (fun) == 'function') fun();
                    });
                }, second);
            });
        }, 2000);
    };

    $(document.body).append(this.Tip);
    this.ShowTip(typeof (interval) == 'number' ? interval : 5000, typeof (interval) == 'function' ? interval : callBack);
}

var JSTipBox = {
    Box: null,
    Show: function (msg, interval, callBack) {
        //
        if (this.Box == null) {
            this.Box = $('<div id="JSTipBox"></div>');
            $(document.body).append(this.Box);
        }
        new JSTip(msg, interval, callBack);
    }
};

/*

JSTipBox.Show('提示信息');
JSTipBox.Show('提示信息',3000);
JSTipBox.Show('提示信息',function(){alert('已关闭')});
JSTipBox.Show('提示信息',3000,function(){alert('已关闭')});

*/