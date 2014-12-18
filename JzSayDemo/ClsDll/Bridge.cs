using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using JzSayGen;
using System.Drawing;
using System.IO;

namespace JzSayDemo.ClsDll
{
    /// <summary>
    /// 
    /// </summary>
    public class Bridge : AjaxBinHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 验证码sessionKey
        /// </summary>
        const string SafeSessionKey = "WebSafeCode";

        /// <summary>
        /// 验证码
        /// </summary>
        public void SafeImage()
        {
            string checkCode = RandImageCode.GetCode(6);
            this.CurrentContext.Session[SafeSessionKey] = checkCode;

            int iwidth = 100;
            using (System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 30))
            {
                System.Random rand = new Random(~unchecked((int)DateTime.Now.Ticks));
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.Clear(Color.White);
                    Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple, Color.SkyBlue };
                    string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体", "Comic Sans MS" };
                    Color nowColor = c[rand.Next(8)];

                    for (int i = 0; i < checkCode.Length; i++)
                    {
                        int findex = rand.Next(6);
                        Font _font = new System.Drawing.Font(font[findex], 16, System.Drawing.FontStyle.Bold);
                        Brush b = new System.Drawing.SolidBrush(nowColor);

                        g.DrawString(checkCode.Substring(i, 1), _font, b, rand.Next(1, 8) + (i * 14), rand.Next(6));
                    }
                }
                rand = new Random(~unchecked((int)DateTime.Now.Ticks));
                
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                this.CurrentContext.Response.ClearContent();
                this.CurrentContext.Response.ContentType = "image/png";
                this.CurrentContext.Response.BinaryWrite(ms.ToArray());
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        [AjaxBin(ResponseFormat = "FormTarget")]
        public void LogOutSys()
        {
            MemberPassPort.SigOut();
            this.CurrentContext.Response.Redirect("/");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPass"></param>
        /// <param name="safeCode"></param>
        /// <returns></returns>
        [AjaxBin(ResponseFormat = "FormTarget",RequestMethod="POST")]
        public string LoginSys(string userName, string userPass, string safeCode)
        {
            this.Code = 1;
            if (userName.IsNullOrEmpty()) return "请输入登录账户";
            if (userPass.IsNullOrEmpty()) return "请输入登录密码";

            if (this.CurrentContext.Session["ReLogin"] != null)
            {
                if (safeCode.IsNullOrEmpty()) return "请输入验证码";

                var usc = this.CurrentContext.Session[SafeSessionKey] as string;
                if (string.IsNullOrEmpty(usc)) return "验证码获取失败请刷新验证码";

                if (usc.ToString().Equals(safeCode, StringComparison.OrdinalIgnoreCase) == false) return "验证码输入错误";
            }
            this.CurrentContext.Session["ReLogin"] = "yes";

            /*
             admin admin888
              
INSERT INTO WebSafe (LoginName,LoginSalt,LoginPass,CreateTS,UpdateTS)
values ('admin','C3ED740FA8','XE889GwjXRuwQ+NsHvJei4t72bCeboOSFpwzCb25n/Y=',
20141208154515289,20141208154515289)
             */

            using (DBDataContext db = new DBDataContext(SqlHelper.DB_CONN_STRING))
            {
                var su = db.WebSafe.FirstOrDefault(x => x.LoginName == userName);
                if (su == null) return "登录账户不存在或登录密码错误";
                string mp = (userPass + su.LoginSalt).SHA256();
                if (su.LoginPass.Equals(mp) == false) return "登录账户不存在或登录密码错误";

                MemberPassPort.SigIn(new MemberPassPort()
                {
                    UserKey = su.LoginName
                }, this.CurrentContext);
            }

            this.Code = 0;
            return "";
        }

        /// <summary>
        /// 图片裁剪
        /// </summary>
        /// <param name="src"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="objw"></param>
        /// <param name="objh"></param>
        /// <returns></returns>
        [AjaxBin(Credential = AjaxBinCredential.No, ResponseFormat = "Json")]
        public string JcropPost(string src, int x, int y, int w, int h, Int32 objw, Int32 objh)
        {
            this.Code = 1;
            if (src.IsNullOrEmpty()) return "源图片为空";
            if (w < 1 || h < 1) return "宽度或高度错误";
            if (objw < 1 || objh < 1) return "目标图片宽度或高度错误";

            if (src.StartsWith("/src/Temp/", StringComparison.OrdinalIgnoreCase) == false) return "没有操作权限";

            string path = this.CurrentContext.Server.MapPath("~").Replace("\\", "/").TrimEnd('/');
            if (File.Exists(path + src) == false) return "源图片未找到";

            Int32 pos = src.LastIndexOf('.');
            if (-1 == pos) return "不是图片文件";

            string eName = src.Substring(pos).ToLower(); //扩展名
            if (".jpg.png.gif".Contains(eName) == false) return "图片文件必须是jpg、png或者gif格式";

            Int32 v = src.LastIndexOf('/');
            string saveSrc = src.Substring(0, v + 12) + "_tmp" + DateTime.Now.ToString("HHmmssff") + eName;
            string cObjSrc = src.Substring(0, v + 12) + "_obj" + DateTime.Now.ToString("HHmmssff") + eName;

            using (System.Drawing.Image originalImage = System.Drawing.Image.FromFile(path + src))
            {
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(w, h, originalImage.PixelFormat))
                {
                    bmp.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, w, h), x, y, w, h, System.Drawing.GraphicsUnit.Pixel);

                        bmp.Save(path + saveSrc, originalImage.RawFormat);
                    }
                }
            }

            this.Code = 0;
            if (w > objw + 5 || h > objh + 5)
            {
                //允许5个像素的冗差
                ImageThumbnailHelper.MakeThumbnail(path + saveSrc, path + cObjSrc, objw, objh);
                return cObjSrc;
            }
            else if (w < objw - 5 && h < objh - 5)
            {
                ImageThumbnailHelper.MakeThumbnail(path + saveSrc, path + cObjSrc, objw, objh);
                return cObjSrc;
            }
            return saveSrc;
        }

        /// <summary>
        /// 临时文件上传
        /// </summary>
        /// <param name="token">会话token</param>
        /// <param name="imgsize">建议图片尺寸 宽*高 150*120</param>
        /// <param name="biger">是否显示大图</param>
        /// <returns></returns>
        [AjaxBin(Credential = AjaxBinCredential.No, ResponseFormat = "Json")]
        public string TempFileUpload(string token, string imgsize, string biger)
        {
            this.Code = 1;
            Int32 maxWidth = biger.IsNullOrEmpty() ? 500 : 606; //最大宽度，即屏幕显示区域的最大可视宽度

            if (imgsize.IsNullOrEmpty() || -1 == imgsize.IndexOf('*')) return "图片尺寸未定义";
            Int32 wishWidth = imgsize.Split('*')[0].ToInt32(); //建议宽度
            Int32 wishHeight = imgsize.Split('*')[1].ToInt32(); //建议高度
            if (wishWidth < 1 || wishWidth > maxWidth || wishHeight < 1) return "图片尺寸错误";

            //this.Member = new MemberPassPort().TokenStrDecrypt(token);
            //if (this.Member == null) return "登录数据获取失败";

            if (this.CurrentContext.Request.Files.Count == 0) return "没有图片上传";

            string tRoot = this.CurrentContext.Server.MapPath("~");
            string wRoot = "/src/Temp/" + DateTime.Now.ToString("yyyyMM") + "/";
            if (Directory.Exists(tRoot + wRoot) == false) Directory.CreateDirectory(tRoot + wRoot);

            HttpPostedFile file = this.CurrentContext.Request.Files[0];
            if (file == null || file.ContentLength < 2) return "上传图片为空";

            Int32 pos = file.FileName.LastIndexOf('.');
            if (-1 == pos) return "不是上传的图片文件";

            string eName = file.FileName.Substring(pos).ToLower(); //扩展名
            if (".jpg.png.gif".Contains(eName) == false) return "图片文件必须是jpg、png或者gif格式";

            string imgStr = "";

            string fk = MACPrimaryKey.GetNowTS.ToString().Substring(6);
            imgStr = wRoot + fk + eName;
            file.SaveAs(tRoot + imgStr); //保存源图片

            Int32 factWidth = 0; //实际宽度
            Int32 factHeight = 0; //实际高度
            using (System.Drawing.Image uf = System.Drawing.Image.FromFile(tRoot + imgStr))
            {
                factWidth = uf.Width;
                factHeight = uf.Height;
                if (factWidth < wishWidth || factHeight < wishHeight)
                {
                    factWidth = wishWidth;
                    factHeight = wishHeight;
                    string miIcon = wRoot + fk + "_min" + DateTime.Now.ToString("HHmmssff") + ".jpg";
                    ImageThumbnailHelper.MakeThumbnail(tRoot + imgStr, tRoot + miIcon, wishWidth, wishHeight);
                    imgStr = miIcon;
                }
                else if (factWidth > maxWidth)
                {
                    factWidth = maxWidth;
                    factHeight = Convert.ToInt32(uf.Height * maxWidth / uf.Width);
                    string mxIcon = wRoot + fk + "_max" + DateTime.Now.ToString("HHmmssff") + ".jpg";
                    ImageThumbnailHelper.MakeThumbnail(tRoot + imgStr, tRoot + mxIcon, maxWidth, factHeight);
                    imgStr = mxIcon;
                }
            }

            //imgStr = General.ParseImageUrl(this.CurrentContext, imgStr);

            this.Code = 0;
            return string.Format("{0}*{1}*{2}", factWidth, factHeight, imgStr);
        }

    }
}