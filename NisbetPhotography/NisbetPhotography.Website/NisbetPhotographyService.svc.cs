using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Web.Hosting;

namespace NisbetPhotography.Website
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "NisbetPhotographyService" in code, svc and config file together.
    public class NisbetPhotographyService : INisbetPhotographyService
    {
        public Business.AlbumTypeEnum GetAlbumTypeEnum(int enumAsInt)
        {
            return (Business.AlbumTypeEnum)enumAsInt;
        }

        public void UploadImageToCustomerAlbum(Guid customerId, Int16 albumId, byte[] image)
        {
            String customerFolderName = customerId.ToString().Replace("-", "");
            String absoluteFolderPath = HostingEnvironment.ApplicationPhysicalPath + @"Private\Customer\Images\" + customerFolderName + "\\";

            if (!(Directory.Exists(absoluteFolderPath)))
            {
                DirectoryInfo di = Directory.CreateDirectory(absoluteFolderPath);
                di = null;
                GC.Collect();
            }

            String filename = DateTime.Now.Ticks.ToString() + ".jpg";
            String virtualFileName = "../Customer/Images/" + customerFolderName + "/" + filename;
            String absoluteFileName = absoluteFolderPath + filename;

            SaveImageBytes(absoluteFileName, image, true);

            DbObjects.Business.CustomerAlbum album = new DbObjects.Business.CustomerAlbum(albumId, customerId);
            album.AddImage(virtualFileName);
        }

        public void UploadImageToPortfolio(Int16 portfolioCategoryId, byte[] image)
        {
            String filename = DateTime.Now.Ticks.ToString() + ".jpg";
            String virtualFileName = "../../Images/ContentImages/Portfolio/" + filename;
            String absoluteFileName = HostingEnvironment.ApplicationPhysicalPath + @"Images\ContentImages\Portfolio\" + filename;

            SaveImageBytes(absoluteFileName, image, true);

            DbObjects.Business.PortfolioCategory category = new DbObjects.Business.PortfolioCategory(portfolioCategoryId);
            category.AddImage(virtualFileName);
        }

        public void UploadImageToPublicAlbum(Int16 publicAlbumId, byte[] image)
        {
            String filename = DateTime.Now.Ticks.ToString() + ".jpg";
            String virtualFileName = "../../Public/Images/ContentImages/" + filename;
            String absoluteFileName = HostingEnvironment.ApplicationPhysicalPath + @"Public\Images\ContentImages\" + filename;

            SaveImageBytes(absoluteFileName, image, false);

            DbObjects.Business.PublicAlbum album = new DbObjects.Business.PublicAlbum(publicAlbumId);
            album.AddImage(virtualFileName, "");
        }

        private void SaveImageBytes(string filename, byte[] imageBytes, bool resize)
        {
            Image image;

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(imageBytes, 0, imageBytes.Length);

                using (image = Image.FromStream(ms))
                {
                    if (resize)
                        image = ResizeImage((Bitmap)image);

                    image.Save(filename, ImageFormat.Jpeg);
                }
            }

            image = null;
            GC.Collect();
        }

        private Bitmap ResizeImage(Bitmap img)
        {
            int maxPictureWidth = Business.BasePage.MaxPictureWidth;
            int maxPictureHeight = Business.BasePage.MaxPictureHeight;

            if (img.Width > maxPictureWidth)
            {
                double factor = Convert.ToDouble(maxPictureWidth) / Convert.ToDouble(img.Width);
                double newHeight = img.Height * factor;

                return ResizeImage(DrawImage(img, maxPictureWidth, Convert.ToInt32(newHeight)));
            }

            if (img.Height > maxPictureHeight)
            {
                double factor = Convert.ToDouble(maxPictureHeight) / Convert.ToDouble(img.Height);
                double newWidth = img.Width * factor;

                return ResizeImage(DrawImage(img, Convert.ToInt32(newWidth), maxPictureHeight));
            }

            return img;
        }

        private static Bitmap DrawImage(Image img, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.DrawImage(img, 0, 0, width, height);
            }

            return result;
        }
    }
}
