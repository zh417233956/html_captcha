using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using WebAPI.Code;

namespace WebAPI.App_Code
{
    /// <summary>
    /// 图片合成
    /// </summary>
    public class MergeProvider
    {
        /// <summary>
        /// 合并9张网络图片
        /// </summary>
        /// <param name="imagesUrl"></param>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] Merge9Images(List<string> imagesUrl, string text, int size=344)
        {
            List<byte[]> list_images = new List<byte[]>();
            foreach (var item in imagesUrl)
            {
                list_images.Add(Download(item));
            }
            return Merge9Images(list_images, text, size);
        }



        /// <summary>
        /// 合并9张图片
        /// </summary>
        /// <param name="images"></param>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] Merge9Images(List<byte[]> images, string text, int size = 344)
        {
            List<Image> listImage = new List<Image>();
            foreach (var item in images)
            {
                listImage.Add(ConvertToImage(item));
            }
            var image = Merge9Images(listImage, text, size);
            var buffers = ConvertToByte(image);
            image.Dispose();
            return buffers;
        }

        /// <summary>
        /// 合并9张图片
        /// </summary>
        /// <param name="images"></param>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image Merge9Images(List<Image> images, string text, int size)
        {
            var width = size;
            var height = size;
            var pf = PixelFormat.Format32bppArgb;
            using (var bg = new Bitmap(width, height + 40, pf))
            {
                using (var g = Graphics.FromImage(bg))
                {
                    g.FillRectangle((Brush)Brushes.Black, 0, 0, width, height + 40);//全幅背景为白色

                    #region 画图 

                    using (var img1 = ZoomToSqure(images[0], size))
                    {
                        var newHeight = height / 3;//  =125
                        var newWidth = width / 3;  //  =125
                        var srcWidth = img1.Width; //  =250
                        var srcHeight = img1.Height;// =250
                        g.DrawImage(img1, new Rectangle(0, 0, newWidth, newHeight), new Rectangle(0, 0, srcWidth, srcHeight), GraphicsUnit.Pixel);
                    }
                    using (var img2 = ZoomToSqure(images[1], size))
                    {
                        var newHeight = height / 3; //  =125
                        var newWidth = width / 3;   //  =125
                        var srcWidth = img2.Width; //   =250
                        var srcHeight = img2.Height;//  =250
                        g.DrawImage(img2, new Rectangle(width / 3, 0, newWidth, newHeight), new Rectangle(0, 0, srcWidth, srcHeight), GraphicsUnit.Pixel);
                    }
                    using (var img3 = ZoomToSqure(images[2], size))
                    {
                        var newHeight = height / 3; //  =125
                        var newWidth = width / 3;   //  =125
                        var srcWidth = img3.Width; //   =250
                        var srcHeight = img3.Height;//  =250
                        g.DrawImage(img3, new Rectangle(width * 2 / 3, 0, newWidth, newHeight), new Rectangle(0, 0, srcWidth, srcHeight), GraphicsUnit.Pixel);
                    }

                    using (var img4 = ZoomToSqure(images[3], size))
                    {
                        var newHeight = height / 3; //  =125
                        var newWidth = width / 3;   //  =125
                        var srcWidth = img4.Width; //   =250
                        var srcHeight = img4.Height;//  =250
                        g.DrawImage(img4, new Rectangle(0, height / 3, newWidth, newHeight), new Rectangle(0, 0, srcWidth, srcHeight), GraphicsUnit.Pixel);
                    }
                    using (var img5 = ZoomToSqure(images[4], size))
                    {
                        var newHeight = height / 3; //  =125
                        var newWidth = width / 3;   //  =125
                        var srcWidth = img5.Width; //   =250
                        var srcHeight = img5.Height;//  =250
                        g.DrawImage(img5, new Rectangle(width / 3, height / 3, newWidth, newHeight), new Rectangle(0, 0, srcWidth, srcHeight), GraphicsUnit.Pixel);
                    }
                    using (var img6 = ZoomToSqure(images[5], size))
                    {
                        var newHeight = height / 3; //  =125
                        var newWidth = width / 3;   //  =125
                        var srcWidth = img6.Width; //   =250
                        var srcHeight = img6.Height;//  =250
                        g.DrawImage(img6, new Rectangle(width * 2 / 3, height / 3, newWidth, newHeight), new Rectangle(0, 0, srcWidth, srcHeight), GraphicsUnit.Pixel);
                    }
                    using (var img4 = ZoomToSqure(images[6], size))
                    {
                        var newHeight = height / 3; //  =125
                        var newWidth = width / 3;   //  =125
                        var srcWidth = img4.Width; //   =250
                        var srcHeight = img4.Height;//  =250
                        g.DrawImage(img4, new Rectangle(0, height * 2 / 3, newWidth, newHeight), new Rectangle(0, 0, srcWidth, srcHeight), GraphicsUnit.Pixel);
                    }
                    using (var img5 = ZoomToSqure(images[7], size))
                    {
                        var newHeight = height / 3; //  =125
                        var newWidth = width / 3;   //  =125
                        var srcWidth = img5.Width; //   =250
                        var srcHeight = img5.Height;//  =250
                        g.DrawImage(img5, new Rectangle(width / 3, height * 2 / 3, newWidth, newHeight), new Rectangle(0, 0, srcWidth, srcHeight), GraphicsUnit.Pixel);
                    }
                    using (var img6 = ZoomToSqure(images[8], size))
                    {
                        var newHeight = height / 3; //  =125
                        var newWidth = width / 3;   //  =125
                        var srcWidth = img6.Width; //   =250
                        var srcHeight = img6.Height;//  =250
                        g.DrawImage(img6, new Rectangle(width * 2 / 3, height * 2 / 3, newWidth, newHeight), new Rectangle(0, 0, srcWidth, srcHeight), GraphicsUnit.Pixel);
                    }

                    #endregion
                    #region 写入文字
                    MemoryStream ms = new MemoryStream();
                    var vCode = new DrawImageCode();
                    vCode.Width = 116;
                    vCode.Text = text;
                    vCode.ValidationCodeCount = text.Length;
                    vCode.CreateImage(ms);
                    var image_Content = ConvertToImage(ms.ToArray());

                    using (var img_last = ZoomToSqure(image_Content, 116))
                    {
                        var newHeight = 40; //  =125
                        var newWidth = 116;   //  =125
                        var srcWidth = newWidth; //   =250
                        var srcHeight = img_last.Height;//  =250
                        g.DrawImage(img_last, new Rectangle(0, height, newWidth, newHeight), new Rectangle(0, 0, srcWidth, srcHeight), GraphicsUnit.Pixel);
                    }
                    image_Content.Dispose();

                    #endregion 写入文字

                    g.Save();
                    foreach (var item in images)
                    {
                        item.Dispose();
                    }
                }
                using (var ms = new MemoryStream())
                {
                    bg.Save(ms, ImageFormat.Png);
                    var buffers = ms.ToArray();
                    return ConvertToImage(buffers);
                }
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static byte[] Download(string imageUrl)
        {
            if (imageUrl.StartsWith("http"))
            {
                using (var ms = new MemoryStream())
                {
                    var request = (HttpWebRequest)HttpWebRequest.Create(imageUrl);// 打开网络连接
                    using (var rs = request.GetResponse().GetResponseStream())// 向服务器请求,获得服务器的回应数据流
                    {
                        byte[] btArray = new byte[512];// 定义一个字节数据,用来向readStream读取内容和向writeStream写入内容
                        int size = rs.Read(btArray, 0, btArray.Length);// 向远程文件读第一次

                        while (size > 0)// 如果读取长度大于零则继续读
                        {
                            ms.Write(btArray, 0, size);// 写入本地文件
                            size = rs.Read(btArray, 0, btArray.Length);// 继续向远程文件读取
                        }
                        return ms.ToArray();
                    }
                }
            }
            else
            {
                using (var ms = new MemoryStream())
                {
                    var img = Image.FromFile(imageUrl);
                    img.Save(ms, img.RawFormat);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// 将byte数组转化为Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image ConvertToImage(byte[] buffer)
        {
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// 将Image转化为byte数组
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ConvertToByte(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }


        /// <summary>
        /// 等比缩放/放大成正方形图片
        /// </summary>
        /// <param name="orginal">原始图片</param>
        /// <param name="size">目标宽高</param>
        /// <param name="cute">超出部分是否剪裁</param>
        /// <returns></returns>
        public static Image ZoomToSqure(Image orginal, int size, bool cute = true)
        {
            var width = 0d;//图片宽度
            var height = 0d;//图片高度
            if (orginal.Width > orginal.Height)//原始图片宽度大于高度
            {
                height = size;
                width = cute ? size : (orginal.Width * height / orginal.Height);
            }
            else if (orginal.Width < orginal.Height)//原始图片高度大于宽度
            {
                width = size;
                height = cute ? size : (orginal.Height * width / orginal.Width);
            }
            else//原始图片是正方形，刚好
            {
                width = size;
                height = size;
            }
            var board = new Bitmap((int)width, (int)height);
            using (var g = Graphics.FromImage(board))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;//设置质量
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;//设置质量
                g.Clear(Color.White);//置背景色
                g.DrawImage(orginal, new Rectangle(0, 0, board.Width, board.Height), new Rectangle(0, 0, orginal.Width, orginal.Height), System.Drawing.GraphicsUnit.Pixel);  //画图
                orginal.Dispose();//释放原图
                return board;
            }
        }
    }    
}