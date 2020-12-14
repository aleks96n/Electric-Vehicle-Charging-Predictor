using ElectricVehicleChargingPredictor;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ElectricVehicleChargingPredictor
{
    public enum ImageModificationType
    {
        Crop,
        Resize,
        CropNResize
    };

    public static class ImageHelper
    {
        /// <summary>
        /// Modifies an image image.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="w">The w.</param>
        /// <param name="h">The h.</param>
        /// <param name="modType">Type of the mod. Crop or Resize</param>
        /// <returns>New Image Id</returns>
        public static SimpleResult ModifyImage(Bitmap img, 
            int x, int y, int w, int h, int maxW, int maxH, 
            ImageModificationType modType, string saveImagePath, bool maintainRatio = false)
        {
            var result = new SimpleResult { Success = false };

            try
            {
                var resizeW = maxW > 0 ? maxW : w;
                var resizeH = maxH > 0 ? maxH : h;
                if ((modType == ImageModificationType.Resize || modType == ImageModificationType.CropNResize) && maintainRatio)
                {
                    float imgW = modType == ImageModificationType.Resize ? (float)img.Width : (float)w;
                    float imgH = modType == ImageModificationType.Resize ? (float)img.Height : (float)h;
                    float scale = Math.Min(Math.Min((float)resizeW / imgW, (float)resizeH / imgH), (float)1);
                    resizeW = Convert.ToInt32(imgW * scale);
                    resizeH = Convert.ToInt32(imgH * scale);

                    if (modType == ImageModificationType.Resize)
                    {
                        w = resizeW;
                        h = resizeH;
                    }
                }

                using (Bitmap _bitmap = new Bitmap(w, h))
                {
                    _bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                    using (Graphics _graphic = Graphics.FromImage(_bitmap))
                    {
                        _graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        _graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        _graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        _graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                        if (modType == ImageModificationType.Crop || modType == ImageModificationType.CropNResize)
                        {
                            _graphic.DrawImage(img, 0, 0, w, h);
                            _graphic.DrawImage(img, new Rectangle(0, 0, w, h), x, y, w, h, GraphicsUnit.Pixel);
                        }

                        if (modType == ImageModificationType.Resize)
                        {
                            _graphic.DrawImage(img, 0, 0, img.Width, img.Height);
                            _graphic.DrawImage(img, new Rectangle(0, 0, w, h), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                        }

                        if (modType == ImageModificationType.CropNResize)
                        {
                            using (Bitmap _bitmap2 = new Bitmap(resizeW, resizeH))
                            {
                                _bitmap2.SetResolution(_bitmap.HorizontalResolution, _bitmap.VerticalResolution);
                                using (Graphics _graphic2 = Graphics.FromImage(_bitmap2))
                                {
                                    _graphic2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                    _graphic2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                    _graphic2.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                                    _graphic2.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                                    _graphic2.DrawImage(_bitmap, 0, 0, _bitmap.Width, _bitmap.Height);
                                    _graphic2.DrawImage(_bitmap, new Rectangle(0, 0, resizeW, resizeH), 0, 0, _bitmap.Width, _bitmap.Height, GraphicsUnit.Pixel);
                                }

                                if (File.Exists(saveImagePath)) File.Delete(saveImagePath);
                                _bitmap2.Save(saveImagePath, GetImageFormat(saveImagePath));
                            }
                        }
                        else
                        {
                            if (File.Exists(saveImagePath)) File.Delete(saveImagePath);
                            _bitmap.Save(saveImagePath, GetImageFormat(saveImagePath));
                        }
                    }
                }

                result.Success = true;
            }
            catch
            {
                throw;
            }
            return result;
        }

        public static ImageFormat GetImageFormat(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException(
                    string.Format("Unable to determine file extension for fileName: {0}", fileName));

            switch (extension.ToLower())
            {
                case @".bmp":
                    return ImageFormat.Bmp;

                case @".gif":
                    return ImageFormat.Gif;

                case @".ico":
                    return ImageFormat.Icon;

                case @".jpg":
                case @".jpeg":
                    return ImageFormat.Jpeg;

                case @".png":
                    return ImageFormat.Png;

                case @".tif":
                case @".tiff":
                    return ImageFormat.Tiff;

                case @".wmf":
                    return ImageFormat.Wmf;

                default:
                    throw new NotImplementedException();
            }
        }

        /*private SimpleResult CropImage(VendorRegisterContractorViewModel model)
        {
            var result = new SimpleResult { Success = false };
            try
            {
                if (model.Photo != null)
                {
                    Bitmap img = new Bitmap(model.Photo.FilePath);

                    var x = model.CropPixel.x;
                    var y = model.CropPixel.y;
                    var w = model.CropPixel.w;
                    var h = model.CropPixel.h;
                    var maxW = 300;
                    var maxH = 300;

                    using (Bitmap _bitmap = new Bitmap(w, h))
                    {
                        _bitmap.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                        using (Graphics _graphic = Graphics.FromImage(_bitmap))
                        {
                            _graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            _graphic.SmoothingMode = SmoothingMode.HighQuality;
                            _graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            _graphic.CompositingQuality = CompositingQuality.HighQuality;
                            _graphic.CompositingMode = CompositingMode.SourceOver;

                            //Code used to crop
                            _graphic.DrawImage(img, 0, 0, w, h);
                            _graphic.DrawImage(img, new Rectangle(0, 0, w, h), x, y, w, h, GraphicsUnit.Pixel);

                            //Code I used to resize
                            _graphic.DrawImage(img, 0, 0, img.Width, img.Height);
                            _graphic.DrawImage(img, new Rectangle(0, 0, maxW, maxH), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);


                        }
                    }
                }
            }
            catch
            {
                throw;
            };

            return result;
        }

        internal Image Crop(Image image, Rectangle r)
        {
            Bitmap bmpCrop;
            using (Bitmap bmpImage = new Bitmap(image))
            {
                bmpCrop = bmpImage.Clone(r, bmpImage.PixelFormat);
                bmpImage.Dispose();
            }
            return (Image)(bmpCrop);
        }

        internal Image ResizeTo(Image sourceImage, int width, int height)
        {
            System.Drawing.Image newImage = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(sourceImage, new Rectangle(0, 0, width, height));
                gr.Dispose();
            }
            return newImage;
        }*/
    }
}
