using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NisbetPhotography.DbObjects.Business
{
    public class PublicAlbum : DbObject
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the album
        /// </summary>
        public Int16 Id { get; internal set; }

        /// <summary>
        /// Name of the album
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Description for the album
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// All images that have been added to the album
        /// </summary>
        public PublicImage[] Images { get; internal set; }

        /// <summary>
        /// Quantity of images held in this album
        /// </summary>
        public int ImageCount
        {
            get { return Images.Length; }
        }

        /// <summary>
        /// Thumbnail image for the album
        /// </summary>
        public PublicImage Thumbnail
        {
            get
            {
                foreach (PublicImage image in Images)
                    if (image.Thumbnail)
                        return image;

                return new PublicImage() { Id = -1, ImageUrl = null, Caption = null, Thumbnail = true };
            }
        }

        /// <summary>
        /// Date and time the album was created
        /// </summary>
        public DateTime DateCreated { get; internal set; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public PublicAlbum()
        {
            Id = -1;
            Name = String.Empty;
            Description = String.Empty;
            Images = new PublicImage[0];
            DateCreated = DateTime.Now;

            IsInDatabase = false;
        }

        /// <summary>
        /// Loads a new instance of the PublicAlbum class from the database
        /// </summary>
        /// <param name="publicAlbumId">Unique identifier of the album</param>
        public PublicAlbum(Int16 publicAlbumId)
        {
            PublicAlbum clone = Data.PublicAlbum.GetPublicAlbumById(publicAlbumId);

            Id = clone.Id;
            Name = clone.Name;
            Description = clone.Description;
            Images = clone.Images;
            DateCreated = clone.DateCreated;

            IsInDatabase = true;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Gets the public image from the Images array with the specified Id number
        /// </summary>
        /// <param name="publicImageId">Unique identifier of the image to look for</param>
        /// <returns>PortfolioImage object with the relevant Id number</returns>
        public PublicImage GetPublicImageById(Int16 publicImageId)
        {
            foreach (PublicImage image in Images)
                if (image.Id == publicImageId)
                    return image;

            throw new IndexOutOfRangeException("The album does not contain an image with the Id number \"" + publicImageId.ToString() + "\"");
        }

        /// <summary>
        /// Adds an image to the album
        /// </summary>
        /// <param name="imageUrl">URL of the image being added</param>
        /// <param name="caption">Caption to be displayed with the image</param>
        public void AddImage(String imageUrl, String caption)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot add image to album because album does not exist in the database");

            List<PublicImage> currentImages = new List<PublicImage>();

            foreach (PublicImage image in Images)
                currentImages.Add(image);

            Boolean blnThumbnail = (currentImages.Count == 0);

            Int16 portfolioImageId = Data.PublicAlbum.AddImageToPublicAlbum(Id, imageUrl, caption, blnThumbnail);
            currentImages.Add(new PublicImage() { Id = portfolioImageId, ImageUrl = imageUrl, Caption = caption, Thumbnail = blnThumbnail });
            Images = currentImages.ToArray();
        }

        /// <summary>
        /// Removes an image from the album
        /// </summary>
        /// <param name="index">Array index of the image being removed</param>
        public void RemoveImage(Int16 publicImageId)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot remove image from album because album does not exist in the database");

            PublicImage? pi = null;

            List<PublicImage> currentImages = new List<PublicImage>();

            foreach (PublicImage image in Images)
            {
                if (image.Id == publicImageId)
                    pi = image;
                else
                    currentImages.Add(image);
            }

            if (pi == null)
                throw new IndexOutOfRangeException("The album does not contain an image with the Id number \"" + publicImageId.ToString() + "\"");

            Boolean wasThumbnail = pi.Value.Thumbnail;
            Data.PublicAlbum.RemoveImageFromPublicAlbum(Id, publicImageId);

            Images = currentImages.ToArray();

            if (Images.Length > 0)
                if (wasThumbnail)
                    SetThumbnailImage(Images[0].Id);
        }

        /// <summary>
        /// Sets the thumbnail image for the album
        /// </summary>
        /// <param name="index">Array index of the image being used as the thumbnail</param>
        public void SetThumbnailImage(Int16 publicImageId)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot set the thumbnail image for album because album does not exist in the database");

            PublicImage? pi = null;

            List<PublicImage> currentImages = new List<PublicImage>();

            for (int i = 0; i < Images.Length; i++)
            {
                if (Images[i].Id == publicImageId)
                {
                    pi = Images[i];
                    Images[i].Thumbnail = true;
                    Data.PublicAlbum.SetPublicAlbumThumbnailImage(Id, Images[i].Id);
                }
                else
                    Images[i].Thumbnail = false;
            }

            if (pi == null)
                throw new IndexOutOfRangeException("The album does not contain an image with the Id number \"" + publicImageId.ToString() + "\"");
        }

        /// <summary>
        /// Chages the caption of the image with the specified Id number
        /// </summary>
        /// <param name="publicImageId">Unique identifier of the image who's caption is being changed</param>
        /// <param name="caption">New caption for the image</param>
        public void ChangeImageCaption(Int16 publicImageId, String caption)
        {
            PublicImage image = GetPublicImageById(publicImageId);
            image.Caption = caption;

            Data.PublicAlbum.ChangePublicImageCaption(Id, publicImageId, caption);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns all albums in the database
        /// </summary>
        /// <returns>Array containing all albums in the database</returns>
        public static PublicAlbum[] GetPublicAlbums()
        {
            return Data.PublicAlbum.GetPublicAlbums();
        }

        /// <summary>
        /// Determines whether a public album currently exists with the specified name
        /// </summary>
        /// <param name="name">Name to search for</param>
        /// <returns>True if an album already exists with the specified name, false if not</returns>
        public static Boolean PublicAlbumNameExists(String name)
        {
            return Data.PublicAlbum.PublicAlbumNameExists(name);
        }

        /// <summary>
        /// Returns all images that belong to the specified album
        /// </summary>
        /// <param name="parentId">Unique identifier of the parent album</param>
        /// <returns>Array containing all images that belong to the specified album</returns>
        public static PublicImage[] GetPublicImagesByParentId(Int16 parentId)
        {
            return Data.PublicAlbum.GetPublicImagesByParentId(parentId);
        }

        #endregion

        #region DbObject Members

        protected override void ExecuteAddStoredProcedure()
        {
            Id = Data.PublicAlbum.AddPublicAlbum(this);
        }

        protected override void ExecuteUpdateStoredProcedure()
        {
            Data.PublicAlbum.UpdatePublicAlbum(this);
        }

        protected override void ExecuteDeleteStoredProcedure()
        {
            Data.PublicAlbum.DeletePublicAlbum(Id);
        }

        protected override bool ValidateProperties(out Exception.InvalidPropertyException error)
        {
            error = null;

            if (String.IsNullOrEmpty(Name))
                error = new Exception.InvalidPropertyException("PublicAlbum.Name cannot be empty");

            return (error == null);
        }

        protected override void ResetIdToDefaultValue()
        {
            Id = -1;
        }

        internal override Data.StoredProcedureParameter[] GetParametersForStoredProcedure(bool updatedStoredProc)
        {
            List<Data.StoredProcedureParameter> parameters = new List<Data.StoredProcedureParameter>();

            if (updatedStoredProc)
                parameters.Add(Data.Control.GetParameter("PublicAlbumId", DbType.Int16, Id));
            else
                parameters.Add(Data.Control.GetParameter("DateCreated", DbType.DateTime, DateCreated));

            parameters.Add(Data.Control.GetParameter("Name", DbType.String, Name));
            parameters.Add(Data.Control.GetParameter("Description", DbType.String, Description));

            return parameters.ToArray();
        }

        #endregion
    }
}
