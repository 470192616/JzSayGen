<%@ Page Title="" Language="C#" MasterPageFile="~/JM/Site1.Master" AutoEventWireup="true" CodeBehind="UILogs.aspx.cs" Inherits="JzSayDemo.JM.UILogs" %>
<%@ Import Namespace="JzSayDemo.ClsDll" %>
<%@ Import Namespace="JzSayGen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headerJsAndCss" runat="server">
    <style type="text/css">
        #Waterfall{width:100%;}
        #Waterfall ul.col{width:260px;float:left;margin:0px;padding:0px; list-style-type:none;}
        #Waterfall ul.col li{padding:0px;margin:0px;list-style-type:none;}
        #Waterfall ul.col li .pan{background-color:#e0e0e0;height:250px;padding:0px;margin:10px 5px;}
        
        .clear{float:none; clear:both; width:100%; height:1px; line-height:1px; overflow:hidden;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Holder1" runat="server">
    <div class="rule-float">sdfsd</div>
    
    <div id="Waterfall">
        <ul class="col"></ul>
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
                    s.push('    <div class="pan">');
                    s.push('	    ' + o.Type + '');
                    s.push('        ' + o.Date + '');
                    s.push('        ' + o.Json + '');
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJs" runat="server">
</asp:Content>
