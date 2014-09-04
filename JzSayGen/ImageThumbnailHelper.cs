using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace JzSayGen
{
    /// <summary>
    /// 图片缩略图
    /// </summary>
    public class ImageThumbnailHelper
    {
        /// <summary>
        /// 
        /// </summary>
        class ThumbnailSize
        {
            public Int32 Width { get; set; }
            public Int32 Height { get; set; }
            public ThumbnailSize(Int32 w, Int32 h)
            {
                this.Width = w;
                this.Height = h;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxWidth"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        static ThumbnailSize FixSize(int maxWidth, int width, int height)
        {
            if (maxWidth >= width) return new ThumbnailSize(width, height);
            return new ThumbnailSize(maxWidth, Convert.ToInt32((double)(height * maxWidth) / (double)width));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        static ThumbnailSize NewSize(int maxWidth, int maxHeight, int width, int height)
        {
            double w;
            double h;
            double sw = Convert.ToDouble(width);
            double sh = Convert.ToDouble(height);
            double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight);
            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }
            return new ThumbnailSize(Convert.ToInt32(w), Convert.ToInt32(h));
        }

        ///<summary>
        /// 生成缩略图，并自动按比例缩放
        ///</summary>
        ///<param name="sourceImg">原图地址 c:\uploads\a.jpg</param>
        ///<param name="fileName">保存的地址 c:\uploads\a_30.jpg</param>
        ///<param name="maxWidth">最大宽</param>
        ///<param name="maxHeight">最大高等于0则只限制宽度</param>
        public static void CreateThumbnail(string sourceImg, string fileName, int maxWidth, int maxHeight = 0)
        {
            using (Image img = Image.FromFile(sourceImg))
            {
                if (img.Width <= maxWidth && (maxHeight == 0 || img.Height <= maxWidth))
                {
                    img.Save(fileName);
                    return;
                }

                ThumbnailSize newSize = maxHeight == 0 ? FixSize(maxWidth, img.Width, img.Height) : NewSize(maxWidth, maxHeight, img.Width, img.Height);

                using (var outBmp = new Bitmap(newSize.Width, newSize.Height))
                {
                    using (Graphics g = Graphics.FromImage(outBmp))
                    {
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                    }
                    var quality = new long[] { 50 };
                    var encoderParam = new EncoderParameter(Encoder.Quality, quality);
                    var encoderParams = new EncoderParameters();
                    encoderParams.Param[0] = encoderParam;
                    ImageCodecInfo jpegIci = ImageCodecInfo.GetImageEncoders().Single(c => c.FormatDescription.Equals("JPEG"));
                    if (jpegIci != null)
                        outBmp.Save(fileName, jpegIci, encoderParams);
                    else
                        //outBmp.Save(fileName, img.RawFormat);
                        outBmp.Save(fileName, ImageFormat.Jpeg);
                }
            }
        }
    }
}
