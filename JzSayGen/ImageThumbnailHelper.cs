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
        /// 生成缩略图
        /// </summary>
        /// <param name="originalSrc">原图地址 c:\uploads\a.jpg</param>
        /// <param name="thumbnailSrc">原图地址 c:\uploads\a_s.jpg</param>
        /// <param name="thumbWidth">缩略图宽度</param>
        /// <param name="thumbHeight">缩略图高度</param>        
        public static void MakeThumbnail(string originalSrc, string thumbnailSrc, int thumbWidth, int thumbHeight)
        {
            using (System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalSrc))
            {
                var tSize = NewSize(thumbWidth, thumbHeight, originalImage.Width, originalImage.Height);
                int x = tSize.Width < thumbWidth ? (thumbWidth - tSize.Width) / 2 : 0;
                int y = tSize.Height < thumbHeight ? (thumbHeight - tSize.Height) / 2 : 0;

                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(thumbWidth, thumbHeight))
                {
                    using (Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        g.Clear(Color.White);

                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                        g.DrawImage(originalImage, new Rectangle(x, y, tSize.Width, tSize.Height), new Rectangle(0, 0, originalImage.Width, originalImage.Height), GraphicsUnit.Pixel);

                        bitmap.Save(thumbnailSrc, ImageFormat.Jpeg);
                        /*
                        var quality = new long[] { 80 };
                        var encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                        var encoderParams = new EncoderParameters();
                        encoderParams.Param[0] = encoderParam;
                        ImageCodecInfo jpegIci = ImageCodecInfo.GetImageEncoders().Single(c => c.FormatDescription.Equals("JPEG"));
                        if (jpegIci != null)
                            bitmap.Save(thumbnailSrc, jpegIci, encoderParams);
                        else
                            bitmap.Save(thumbnailSrc, ImageFormat.Jpeg);
                        */
                    }
                }

            }
        }



    }
}
