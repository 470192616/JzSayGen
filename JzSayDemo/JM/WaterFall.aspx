<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WaterFall.aspx.cs" Inherits="JzSayDemo.JM.WaterFall" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <script src="/assets/script/jquery-1.10.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        #Waterfall{width:100%;}
        #Waterfall ul.col{width:33.33%;float:left;margin:0px;padding:0px; list-style-type:none;}
        #Waterfall ul.col li{padding:0px;margin:0px;list-style-type:none;}
        #Waterfall ul.col li .pan{background-color:#e0e0e0;height:250px;padding:0px;margin:10px 5px;}
        
        .clear{float:none; clear:both; width:100%; height:1px; line-height:1px; overflow:hidden;}
    </style>
</head>
<body>    

    <a class="btn" href="javascript:;" onclick="createtest()">生成</a>
    <div id="div1"></div>
    <script>
        function createtest() {
            //
            $.get('?createtest=1', function (htm) {
                $('#div1').html(htm);
            });
        }
    </script>
    
   搜索：<input type="text" id="sk" />    

    <div id="Waterfall">
        <ul class="col"></ul>
        <ul class="col"></ul>        
        <ul class="col"></ul>
        <div class="clear"> </div>
    </div>
    <div id="WaterfallLoadingDiv" style="position:fixed;left:40%;top:40%;width:20%;line-height:25px; background-color:#FFF; border:solid 1px #CCC;">数据加载中。。。</div>
    <div id="WaterfallWaringDiv" style="position:fixed;left:40%;top:40%;width:20%;line-height:25px; background-color:#FFF; border:solid 1px #CCC;">没有数据了</div>
    
    <script type="text/javascript">


        var Waterfall = function (getUrl) {
            //瀑布流数据显示
            //getUrl 获取数据的url地址
            var me = this;
            this.CurPage = 1;
            this.IsReady = 0;
            this.ItemPos = 0;
            this.GetDataUrl = getUrl;
            this.Columns = $("#Waterfall .col");
            this.LoadingDiv = $("#WaterfallLoadingDiv");
            this.WaringDiv = $("#WaterfallWaringDiv");
            this.ShowUI = function (json) {
                //
                for (var i = 0; i < json.length; i++) {
                    me.ItemPos++;
                    var o = json[i];
                    var s = [];
                    s.push('<li wfkey="' + me.ItemPos.toString() + '">');
                    s.push('    <div class="pan" style="height:' + o.H + 'px">');
                    s.push('	    ' + o.K + '');
                    s.push('    </div>');
                    s.push('</li>');
                    me.GetMinCol().append(s.join(''));
                }
            };

            this.LoadData = function () {
                //加载数据
                if (me.IsReady == me.CurPage) return;
                me.IsReady = me.CurPage;
                me.WaringDiv.hide();
                me.LoadingDiv.show();

                $.getJSON(me.GetDataUrl + me.CurPage, function (json) {
                    if (json.length == 0) {
                        //数组长度为0则说明没有数据了
                        me.WaringDiv.show();
                        me.LoadingDiv.hide();
                        setTimeout(function () {
                            //隐藏没有数据的提示框
                            me.WaringDiv.hide();
                        }, 5000);
                        return;
                    }
                    me.ShowUI(json);
                    me.LoadingDiv.hide();
                    me.CurPage = me.CurPage + 1;
                });
            };

            this.GetMinCol = function () {
                //获取最短的列                
                var minCol = me.Columns.eq(0);
                me.Columns.each(function (index, elem) {
                    if ($(elem).height() < minCol.height()) {
                        minCol = $(elem);
                    }
                });
                return minCol;
            };

            this.ReLoadData = function (getUrl) {
                //重新加载数据
                if (getUrl) me.GetDataUrl = getUrl;

                me.CurPage = 1;
                me.IsReady = 0;
                me.ItemPos = 0;
                me.Columns.empty();
                me.LoadData();
            };

            $(window).on("scroll", function () {
                var mc = me.GetMinCol();
                if (mc.height() <= $(window).scrollTop() + $(window).height()) {
                    //当最短的ul的高度比窗口滚出去的高度+浏览器高度大时加载新图片
                    me.LoadData();
                }
            });



        };


        function SearchUrl() {
            //获取搜索参数
            var sk = $('#sk').val(); //搜索的关键字
            if (sk != '') sk = '&sk=' + sk;

            return '?ajax=1' + sk + '&page=';
        }

        var su = SearchUrl();
        var wf = new Waterfall(su);
        wf.LoadData(); //加载默认数据


        $('#sk').blur(function () {
            //搜索按钮事件
            var su = SearchUrl();
            wf.ReLoadData(su); //重新加载搜索数据
        });
        
    </script>   
</body>
</html>
