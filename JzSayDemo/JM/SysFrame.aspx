<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysFrame.aspx.cs" Inherits="JzSayDemo.JM.SysFrame" %>
<!DOCTYPE html>
<html>
	<head>
        <meta charset="utf-8" />
		<title>JS</title>    	
        <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0">
        <meta name="author" content="jzsay.com">
        <meta name="keywords" content="jzsay.com 价值说" />
        <meta name="description" content="jzsay.com 价值说"/>
        
        <link rel="stylesheet" href="/assets/css/base.css" type="text/css" />
        <link rel="stylesheet" href="/assets/css/fontello.css" type="text/css" />
        <link rel="stylesheet" href="/assets/css/animation.css" type="text/css" />
        <link rel="stylesheet" href="/assets/css/formexten.css" type="text/css" />

        <script src="/assets/script/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="/assets/script/jquery.nicescroll.js" type="text/javascript"></script>        
        <script src="/assets/script/jquery.formexten.js" type="text/javascript"></script>

        <script src="/assets/script/JSBox.js" type="text/javascript"></script>
        <script src="/assets/script/JSTipBox.js" type="text/javascript"></script>
        <script src="/assets/script/jquery.JSInputEdit.js" type="text/javascript"></script>

        <style type="text/css">
            html,body{overflow:hidden;}
            .mainHeader{height:60px; position:fixed;   right:0;    top:0;    width:100%;    z-index: 11; background-color:#F5F5F5; border-bottom:solid 3px #3c5880;}
            .mainSider{bottom: 0;    left: 0;    position: absolute;    top: 60px;    width:180px;background-color:#f6f7f7;    border-right: 1px solid #dbdbdb;}
            .mainContainer{ bottom: 0;    left: 185px;    position: absolute;    right: 0;    top: 60px;}            
            .mainContainer iframe {    bottom: 0;    height: 100%;    left: 0;    position: absolute;    right: 0;    top: 0;    width: 100%;}

            .menu{display:block; height:30px; line-height:26px; background-color:#FFF;margin:3px 0px;}
            .menu i{font-size:20px;}
            .menu span{display:inline-block; width:80px; height:22px; line-height:25px; overflow:hidden;}            
            .menu sup{ font-weight:normal; font-size:12px; color:#FFF; background-color:#16a0d3; line-height:18px; display:inline-block;border-radius:3px; padding:0 5px; white-space: nowrap;}
            .menu b{font-size:20px; display:inline-block; position:absolute; right:3px;}
            
            .vs{}
            .vs li{}
            .vs a{display:block; height:30px; line-height:30px; background-color:#FFF; margin-bottom:2px;padding-left:10px;}
            .vs a:hover{ background-color:#F1F1F1}
            .vs a span{ }
            .vs a b {float:right; font-size:18px;}
            .vs a em{ font-weight:normal; float:right; font-size:12px; color:#FFF; background-color:#16a0d3; line-height:18px; border-radius:3px; padding:0 5px; margin-top:6px; margin-right:6px; white-space: nowrap;}
            .vss a{padding-left:30px;}
        </style>
    </head>
<body>    
    <div class="mainHeader">
        <a href="http://kindeditor.net" target="_blank" style="color:#F00">kindeditor.net编辑器</a>
        <a href="WaterFall.aspx" target="_blank">瀑布流</a>
        <a href="zui.html" target="_blank">基本元素</a>
        <a href="UIResetPass.aspx" target="mainframe">修改密码</a>
        <a href="/AjaxBin/Bridge-LogOutSys.ashx">退出登录</a>        
    </div>
    <div class="mainSider">

        <ul class="vs">
            <li><a href="javascript:;"><i class="icon-keyboard"></i><span>代码库</span><b class="icon-angle-double-left"></b></a>
                <ul class="vss">
                    <li><a href="UIIntroList.aspx" target="mainframe"><i class="icon-right-dir"></i><span>信息列表</span></a></li>
                    <li><a href="UIIntroEdit.aspx" target="mainframe"><i class="icon-right-dir"></i><span>信息添加</span></a></li>

                    <li><a href="UICategoryList.aspx" target="mainframe"><i class="icon-right-dir"></i><span>类别列表</span></a></li>
                    <li><a href="UICategoryEdit.aspx" target="mainframe"><i class="icon-right-dir"></i><span>类别添加</span></a></li>

                    <li><a href="UIKeyWordsList.aspx" target="mainframe"><i class="icon-right-dir"></i><span>分词列表</span></a></li>
                    <li><a href="UIKeyWordsEdit.aspx" target="mainframe"><i class="icon-right-dir"></i><span>分词添加</span></a></li>

                    <li><a href="UISqldo.aspx" target="mainframe"><i class="icon-right-dir"></i><span>数据操作</span></a></li>
                    
                    <li><a href="UILogs.aspx" target="mainframe"><i class="icon-sliders"></i><span>系统日志</span></a></li>
                </ul>
            </li>
            
            <li><a href="javascript:;"><i class="icon-folder-open"></i><span>11111111</span><b class="icon-angle-double-left"></b></a>
                <ul class="vss">
                    <li><a href="#"><i class="icon-right-dir"></i><span>1-aaaaaa</span><em class="">5</em></a></li>
                    <li><a href="#"><i class="icon-right-dir"></i><span>1-bbbbbb</span><em class="">5</em></a></li>
                </ul>
            </li>            
            <li><a href="#"><i class="icon-right-dir"></i><span>222222222</span><em class="">5</em></a></li>
            <li><a href="#">33333333</a></li>
            <li><a href="#">44444444</a></li>
        </ul>
                
        
        

    </div>
    <div class="mainContainer">
        <iframe id="mainframe" name="mainframe" frameborder="0" marginheight="0" marginwidth="0" scrolling="no" src="SysHi.aspx"></iframe>
    </div>
    

    <script type="text/javascript">
        $(function () {
            $(".mainSider").niceScroll({ cursorcolor: "#ff9900" });
        });
    </script>
</body>
</html>