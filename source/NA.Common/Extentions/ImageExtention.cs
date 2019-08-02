using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace NA.Common.Extentions
{
    public static class ImageExtention
    {
        public static MemoryStream Resize(this Image image,int with = 0, int height = 0, int quality = 100,string type = ".jpg")
        {
            if (with == 0 && height == 0)//no resize
            {
                using (var stream = new MemoryStream())
                {
                    var qualityParamId = Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                    ImageCodecInfo codec = null;
                    if (type == ".png")
                    {                        
                        codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Png.Guid);
                    }
                    else if (type == ".gif")
                    {
                        codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Gif.Guid);
                    }
                    else if (type == ".ico")
                    {
                        codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Icon.Guid);
                    }
                    else
                    {
                        codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);
                    }
                    
                    image.Save(stream, codec, encoderParameters);
                    return stream;
                }
            }
            //logic
            if (with != 0 && height != 0)
            {
                var imgW = image.Width;
                var imgH = image.Height;

                //check scale w
                var imgWS = with;
                var imgHS = (with * imgH) / imgW;
                var disH = imgHS - height;

                imgHS = height;
                imgWS = (height * imgW) / imgH;
                var disW = imgWS - with;

                if (disH < disW)
                {
                    height = 0;
                }
                else
                {
                    with = 0;
                }
            }
            
            int widthRS, heightRS;
            if (height == 0)
            {
                widthRS = with;
                heightRS = Convert.ToInt32(image.Height * with / (double)image.Width);
            }
            else
            {
                widthRS = Convert.ToInt32(image.Width * height / (double)image.Height);
                heightRS = height;
            }
            var resized = new Bitmap(widthRS, heightRS);
            using (var graphics = Graphics.FromImage(resized))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(image, 0, 0, widthRS, heightRS);

                using (var stream = new MemoryStream())
                {
                    var qualityParamId = Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                    ImageCodecInfo codec = null;
                    if (type == ".png")
                    {
                        codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Png.Guid);
                    }
                    else if (type == ".gif")
                    {
                        codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Gif.Guid);
                    }
                    else if (type == ".ico")
                    {
                        codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Icon.Guid);
                    }
                    else
                    {
                        codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);
                    }
                    resized.Save(stream, codec, encoderParameters);
                    return stream;
                }
            }
        }

        private static MemoryStream Crop(Image sourceImage, int sourceX, int sourceY, int sourceWidth, int sourceHeight, int destinationWidth, int destinationHeight)
        {
            var crop = new Bitmap(destinationWidth, destinationHeight);
            using (var graphics = Graphics.FromImage(crop))
            {

                graphics.DrawImage(
                  sourceImage,
                  new Rectangle(0, 0, destinationWidth, destinationHeight),
                  new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                  GraphicsUnit.Pixel
                );

                using (var stream = new MemoryStream())
                {                    
                    crop.Save(stream, crop.RawFormat);
                    return stream;
                }
            }
        }
    }
}
