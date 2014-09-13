using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;


namespace JzSayGen
{
    /// <summary>
    /// 随机验证码
    /// </summary>
    public class RandImageCode : IHttpHandler, IRequiresSessionState
    {
        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// session关键字
        /// </summary>
        public static readonly string CheckSessionKey = "VerifyRandImageCode";

        private const double PI = 1.2415926535897932384626433832795;

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            var code = RandImageCode.GetCode(5);
            context.Session[RandImageCode.CheckSessionKey] = code;
            ResponseImage2(code, context.Response);
        }

        /// <summary>
        /// 产生随机的数字和字母的组合
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetCode(int len)
        {
            char[] Mather = { '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
            StringBuilder sb = new StringBuilder();
            int n = Mather.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int x = 0; x < len; x++)
            {
                int Rnd = random.Next(0, n);
                sb.Append(Mather[Rnd]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 产生扭曲
        /// </summary>
        /// <param name="srcBmp"></param>
        /// <param name="bXDir"></param>
        /// <param name="dMultValue"></param>
        /// <param name="dPhase"></param>
        /// <returns></returns>
        System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap bitmap = new Bitmap(srcBmp.Width, srcBmp.Height);

            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
            graphics.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, bitmap.Width, bitmap.Height);
            graphics.Dispose();

            double dBaseAxisLen = bXDir ? (double)bitmap.Height : (double)bitmap.Width;

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI * (double)j) / dBaseAxisLen : (PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < bitmap.Width && nOldY >= 0 && nOldY < bitmap.Height)
                    {
                        bitmap.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return bitmap;
        }

        /// <summary>
        /// 产生验证图片信息
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="Response"></param>
        protected void ResponseImage2(string checkCode, HttpResponse Response)
        {
            int iwidth = 80;// (int)(checkCode.Length * 25);
            using (System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 30))
            {
                System.Random rand = new Random(~unchecked((int)DateTime.Now.Ticks));
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.Clear(Color.White);
                    //定义颜色
                    Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple, Color.SkyBlue };
                    //定义字体 
                    string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体", "Comic Sans MS" };

                    Color nowColor = c[rand.Next(8)]; //Color.FromArgb(rand.Next());

                    /*
                    //rand = new Random(~unchecked((int)DateTime.Now.Ticks));
                    //随机输出噪点
                    for (int i = 0; i < 50; i++)
                    {
                        int x = rand.Next(image.Width);
                        int y = rand.Next(image.Height);
                        //image.SetPixel(x, y, nowColor);
                        g.DrawPie(new Pen(nowColor, 0), x, y, 2, 2, 1, 1);
                    }
                    */

                    //rand = new Random(~unchecked((int)DateTime.Now.Ticks));
                    //输出不同字体和颜色的验证码字符
                    for (int i = 0; i < checkCode.Length; i++)
                    {
                        int findex = rand.Next(6);
                        Font _font = new System.Drawing.Font(font[findex], 16, System.Drawing.FontStyle.Bold);
                        Brush b = new System.Drawing.SolidBrush(nowColor);

                        g.DrawString(checkCode.Substring(i, 1), _font, b, rand.Next(1, 8) + (i * 14), rand.Next(6));
                    }

                    /*
                    //rand = new Random(~unchecked((int)DateTime.Now.Ticks));
                    //画图片的前景噪音点
                    for (int i = 0; i < 50; i++)
                    {
                        int x = rand.Next(image.Width);
                        int y = rand.Next(image.Height);
                        //image.SetPixel(x, y, Color.FromArgb(rand.Next()));
                        //image.SetPixel(x, y, nowColor);
                        g.DrawPie(new Pen(nowColor, 0), x, y, 2, 2, 1, 1);
                    }
                    */

                    //画一个边框
                    //g.DrawRectangle(new Pen(nowColor, 0), 0, 0, image.Width - 1, image.Height - 1);
                }

                rand = new Random(~unchecked((int)DateTime.Now.Ticks));
                //using (var ximage = TwistImage(image, true, rand.Next(3, 8), rand.Next(1, 4)))
                //{
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                Response.ClearContent();
                Response.ContentType = "image/png";
                Response.BinaryWrite(ms.ToArray());
                //}
            }
        }
    }
}
