<%@ Page Title="" Language="C#" MasterPageFile="~/JM/Site1.Master" AutoEventWireup="true" CodeBehind="UICategoryEdit.aspx.cs" Inherits="JzSayDemo.JM.UICategoryEdit" %>
<%@ Import Namespace="JzSayDemo.ClsDll" %>
<%@ Import Namespace="JzSayGen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headerJsAndCss" runat="server">
    <link href="/assets/css/jquery.Jcrop.min.css" rel="stylesheet" type="text/css" />       
    <script src="/assets/script/jquery.Jcrop.min.js" type="text/javascript"></script>
    <link href="/assets/script/uploadify/uploadify.css" rel="stylesheet" type="text/css" /> 
    <script src="/assets/script/uploadify/jquery.uploadify.min.js" type="text/javascript"></script>    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Holder1" runat="server">
    <iframe src="/assets/blank.html" name="fTag" style="display:none;"></iframe>
    <form method="post" target="fTag" action="" enctype="multipart/form-data">    
    <table>
        <tr>
            <th width="100"><span class="red fwb">*</span>Url：</th>
            <td><input type="text" name="urlKey" id="urlKey" maxlength="50" class="input" value="<%=HttpUtility.HtmlEncode(this.EditCate.UrlKey)%>" /></td>
            <td><div id="urlKeyTip"></div></td>
        </tr>
        <tr>
            <th><span class="red fwb">*</span>标题：</th>
            <td><input type="text" name="title" maxlength="50" class="input" value="<%=HttpUtility.HtmlEncode(this.EditCate.Title)%>" /></td>
            <td> </td>
        </tr>
        <tr>
            <th valign="top">图标：</th>
            <td colspan="2">
                <input type="file" name="cImg" id="cImg" />
                <div id="tempFileQueue"></div>

                <input type="hidden" name="cImgOld" value="<%=this.EditCate.Icon %>" />
                <input type="hidden" name="cImgNew" id="cImgNew" value="" />
                <div class="bigImageWarring" style="color:#F00; display:none;">
                    您选择的图片尺寸与最佳图片尺寸不符，建议您在图片上按住鼠标左键拖动并调整出需要展示的区域。
                    <div style="height:36px; line-height:36px;">
                        <a style="text-decoration:none; padding:5px;*+padding:5px; background-color:#555; color:#FFF;" class="bigImageTip"></a>
                        &nbsp;&nbsp;
                        <a href="javascript:;" onclick="JcropPost()" class="bigImageCutBtn btn" style="display:none;text-decoration:none; padding:5px;*+padding:5px; color:#FFF;"> 将选择区域生成图片 </a>
                    </div>
                </div>    

                <div>
                    <img id="JcropTarget" src="/assets/image/no.jpg" />
                </div>

                <div class="bigImageWarring" style="color:#F00; display:none;">                    
                    <div style="height:36px; line-height:36px;">
                        <a style="text-decoration:none; padding:5px;*+padding:5px; background-color:#555; color:#FFF;" class="bigImageTip"></a>
                        &nbsp;&nbsp;
                        <a href="javascript:;" onclick="JcropPost()" class="bigImageCutBtn btn" style="display:none;text-decoration:none; padding:5px;*+padding:5px; color:#FFF;"> 将选择区域生成图片 </a>
                    </div>
                    您选择的图片尺寸与最佳图片尺寸不符，建议您在图片上按住鼠标左键拖动并调整出需要展示的区域。
                </div>
            </td>
        </tr>       
        <tr>
            <th>链接：</th>
            <td colspan="2"><input type="text" name="url" maxlength="50" class="input" value="<%=this.EditCate.AttJson%>" /></td>          
        </tr>
        <tr>
            <th> </th>
            <td colspan="2">                
                <input type="submit" value="确定" class="btn" />
                <a href="<%=this.GetQueryStr("backUrl", "UICategoryList.aspx")%>" class="btn">回到列表</a>
            </td>
        </tr>
    </table>
    </form>
    <script type="text/javascript">
        var IsEdit = '<%=this.IsEdit %>';
        var BackUrl = '<%=this.GetQueryStr("backUrl", "UICategoryList.aspx")%>';

        function UrlKeyExists() {
            //
            var urlk = $('#urlKey').val();
            if (urlk == '') {
                $('#urlKeyTip').html('<i class="w">必填</i>');
                return;
            }
            var curl = '?chkUrlKey=' + urlk;
            $.get(curl, function (r) {
                $('#urlKeyTip').html(r == '0' ? '' : '<i class="e">已存在</i>');
            });
        }
        function PostResult(msg) {
            //提交回调            
            alert(msg);
            if (-1 == msg.indexOf('成功')) return;
            location = BackUrl;
        }
        $(function () {
            //
            $('#urlKey').blur(function () {
                //
                UrlKeyExists();
            });
        });
    </script>

    <script type="text/javascript">
        var JcropApi = null;
        var JcropBoundX = 0;
        var JcropBoundY = 0;
        var JcropWidth = 340; //预览图即实际图片的宽度
        var JcropHeight = 260;
        var JcropCutDts = { "Src": '', "x": 0, "y": 0, "w": 0, "h": 0 };
        var JcropToken = 'tokenabc123';
        var JcropForm = $('#cImgNew');

        function JcropInint() {
            //
            $("#JcropTarget").Jcrop({
                minSelect: [15, 15], //选框小于这个值则取消选择区域
                //minSize: [JcropWidth, JcropHeight], //最小选区尺寸
                maxSize: [500, 500], //最大选择区尺寸
                onChange: JcropChange, //拖动选择框时的动作
                onSelect: JcropSelect, //完成选择之后的动作
                onRelease: JcropRelease, //取消选择
                aspectRatio: (JcropWidth / JcropHeight) //图片的宽高比
            }, function () {
                var bounds = this.getBounds();
                //JcropBoundX = bounds[0]; //img的实际宽度，即width属性的值
                //JcropBoundY = bounds[1]; //height属性值
                JcropApi = this;
                JcropApi.disable();
            });
        }

        function JcropChange(c) {
            //
            $('.bigImageTip').html('&nbsp;选择区域的尺寸：' + parseInt(c.w) + ' x ' + parseInt(c.h) + '&nbsp;').show();
        }

        function JcropAreaSet(x, y, w, h) {
            //
            JcropCutDts.x = x;
            JcropCutDts.y = y;
            JcropCutDts.w = w;
            JcropCutDts.h = h;
        }

        function JcropSelect(c) {
            //
            JcropAreaSet(0, 0, 0, 0);
            $('.bigImageCutBtn').hide();

            if (parseInt(c.w) > 0) {
                var rx = JcropWidth / c.w; //其中100为预览图像所在的DIV的宽度，c.w为原图中选择框的宽度，rx即为X方向压缩的比例
                var ry = JcropHeight / c.h;

                JcropAreaSet(c.x, c.y, parseInt(c.w), parseInt(c.h));

                $('.bigImageCutBtn').show();
            };
        };

        function JcropRelease() {
            //
            JcropAreaSet(0, 0, 0, 0);
            $('.bigImageCutBtn').hide();
            $('.bigImageTip').hide();
        }

        function JcropView(src, w, h) {
            //
            //$('#JcropTarget').width(w).height(h);
            JcropForm.val(src);

            $('.bigImageCutBtn').hide();
            $('.bigImageWarring').hide();

            JcropCutDts.Src = src;
            JcropApi.setImage(src, function () {
                //
                var bounds = JcropApi.getBounds();
                JcropBoundX = bounds[0]; //img的实际宽度，即width属性的值
                JcropBoundY = bounds[1]; //height属性值                        

                JcropApi.release();
                JcropApi.disable();

                if ((w > JcropWidth + 5) || (h > JcropHeight + 5)) {
                    //5个像素的冗差
                    JcropApi.enable();
                    $('.bigImageWarring').show();
                }
            });


        }

        function JcropPost() {
            //
            var urlStr = '/AjaxBin/Bridge-JcropPost.ashx?objw=' + JcropWidth + '&objh=' + JcropHeight;
            jQuery.post(urlStr, JcropCutDts, function (json) {
                if (json.Code == 1) {
                    alert(json.Data);
                    return;
                }
                JcropView(json.Data, JcropWidth, JcropHeight);
            });
        }







        function BindTempFileUpload(inputFile, successFun, tokenStr, imgsize) {
            //
            $(inputFile).uploadify({
                'formData': {
                    'imgsize': imgsize,
                    'token': tokenStr
                },
                'queueID': 'tempFileQueue',
                'multi': false,
                'auto': true,
                'successTimeout': 30,
                'removeTimeout': 1,
                'buttonText': '选择图片',
                'fileTypeDesc': '图片文件',
                'fileTypeExts': '*.gif; *.jpg; *.png',
                'swf': '/assets/script/uploadify/uploadify.swf',
                'uploader': '/AjaxBin/Bridge-TempFileUpload.ashx',
                'itemTemplate': '<div id="${fileID}" class="uploadify-queue-item"><span class="fileName">${fileName} (${fileSize})</span><span class="data"></span></div>',
                'onUploadComplete': function (file) {
                    //alert('The file ' + file.name + ' finished processing.');
                },
                'onUploadSuccess': function (file, jsonStr, response) {
                    var json = jQuery.parseJSON(jsonStr);
                    if (json.Code != 0) {
                        alert(json.Data);
                        return;
                    }
                    var imgWidth = parseInt(json.Data.split('*')[0]);
                    var imgHeight = parseInt(json.Data.split('*')[1]);
                    var imgSource = json.Data.split('*')[2];
                    successFun(imgSource, imgWidth, imgHeight);
                },
                'onUploadError': function (file, errorCode, errorMsg, errorString) {
                    //alert('The file ' + file.name + ' could not be uploaded: ' + errorString);
                }
            });
        }



        $(function () {
            //

            JcropInint();

            BindTempFileUpload('#cImg', JcropView, JcropToken, (JcropWidth + '*' + JcropHeight));

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJs" runat="server">
</asp:Content>
