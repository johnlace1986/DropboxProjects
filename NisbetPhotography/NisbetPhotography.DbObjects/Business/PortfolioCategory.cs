using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NisbetPhotography.DbObjects.Business
{
    public class PortfolioCategory : DbObject
    {
        #region Properties

        /// <summary>
        /// Unique identifier for the Category
        /// </summary>
        public Int16 Id { get; internal set; }

        /// <summary>
        /// Display name for the category
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// All images that have been added to this category
        /// </summary>
        public PortfolioImage[] Images { get; internal set; }

        /// <summary>
        /// Thumbnail image for the category
        /// </summary>
        public PortfolioImage Thumbnail
        {
            get
            {
                foreach (PortfolioImage image in Images)
                    if (image.Thumbnail)
                        return image;

                return new PortfolioImage() { Id = -1, ImageUrl = null, Thumbnail = true };
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PortfolioCategory()
        {
            Id = -1;
            Name = String.Empty;
            Images = new PortfolioImage[0];

            IsInDatabase = false;
        }

        /// <summary>
        /// Loads a new instance of the PortfolioCategory class from the database
        /// </summary>
        /// <param name="id">Unique identifier of the category</param>
        public PortfolioCategory(Int16 id)
        {
            PortfolioCategory clone = Data.PortfolioCategory.GetPortfolioCategoryById(id);

            Id = clone.Id;
            Name = clone.Name;
            Images = clone.Images;

            IsInDatabase = true;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds an image to the category
        /// </summary>
        /// <param name="imageUrl">URL of the image being added</param>
        public void AddImage(String imageUrl)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot add image to category because category does not exist in the database");

            List<PortfolioImage> currentImages = new List<PortfolioImage>();

            foreach (PortfolioImage image in Images)
                currentImages.Add(image);

            Boolean blnThumbnail = (currentImages.Count == 0);

            Int16 portfolioImageId = Data.PortfolioCategory.AddImageToPortfolioCategory(Id, imageUrl, blnThumbnail);
            currentImages.Add(new PortfolioImage() { Id = portfolioImageId, ImageUrl = imageUrl, Thumbnail = blnThumbnail });
            Images = currentImages.ToArray();
        }

        /// <summary>
        /// Removes an image from the category
        /// </summary>
        /// <param name="index">Array index of the image being removed</param>
        public void RemoveImage(Int16 portfolioImageId)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot remove image from category because category does not exist in the database");

            PortfolioImage? pi = null;

            List<PortfolioImage> currentImages = new List<PortfolioImage>();

            foreach (PortfolioImage image in Images)
            {
                if (image.Id == portfolioImageId)
                    pi = image;
                else
                    currentImages.Add(image);
            }

            if (pi == null)
                throw new IndexOutOfRangeException("The category does not contain an image with the Id number \"" + portfolioImageId.ToString() + "\"");

            Boolean wasThumbnail = pi.Value.Thumbnail;
            Data.PortfolioCategory.RemoveImageFromPortfolioCategory(Id, portfolioImageId);

            Images = currentImages.ToArray();

            if (Images.Length > 0)
                if (wasThumbnail)
                    SetThumbnailImage(Images[0].Id);
        }

        /// <summary>
        /// Sets the thumbnail image for the category
        /// </summary>
        /// <param name="index">Array index of the image being used as the thumbnail</param>
        public void SetThumbnailImage(Int16 portfolioImageId)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot set the thumbnail image for category because category does not exist in the database");

            PortfolioImage? pi = null;

            List<PortfolioImage> currentImages = new List<PortfolioImage>();

            for (int i = 0; i < Images.Length; i++)
            {
                if (Images[i].Id == portfolioImageId)
                {
                    pi = Images[i];
                    Images[i].Thumbnail = true;
                    Data.PortfolioCategory.SetPortfolioCategoryThumbnailImage(Id, Images[i].Id);
                }
                else
                    Images[i].Thumbnail = false;
            }

            if (pi == null)
                throw new IndexOutOfRangeException("The category does not contain an image with the Id number \"" + portfolioImageId.ToString() + "\"");
        }

        /// <summary>
        /// Gets the porfolio image from the Images array with the specified Id number
        /// </summary>
        /// <param name="portfolioImageId">Unique identifier of the image to look for</param>
        /// <returns>PortfolioImage object with the relevant Id number</returns>
        public PortfolioImage GetPortfolioImageById(Int16 portfolioImageId)
        {
            foreach (PortfolioImage image in Images)
                if (image.Id == portfolioImageId)
                    return image;

            throw new IndexOutOfRangeException("The category does not contain an image with the Id number \"" + portfolioImageId.ToString() + "\"");
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns all categories in the database
        /// </summary>
        /// <returns>Array containing all categories in the database</returns>
        public static PortfolioCategory[] GetPortfolioCategories()
        {
            return Data.PortfolioCategory.GetPortfolioCategories();
        }

        /// <summary>
        /// Determines whether there is currently category in the database with the specified name
        /// </summary>
        /// <param name="name">Name to search for</param>
        /// <returns>True if a category currently exists with the specified name, false if not</returns>
        public static Boolean PortfolioCategoryNameExists(String name)
        {
            return Data.PortfolioCategory.PortfolioCategoryNameExists(name);
        }

        /// <summary>
        /// Returns all images that belong to the specified category
        /// </summary>
        /// <param name="parentId">Unique identifier of the parent category</param>
        /// <returns>Array containing all images that belong to the specified category</returns>
        public static PortfolioImage[] GetPortfolioImagesByParentId(Int16 parentId)
        {
            return Data.PortfolioCategory.GetPortfolioImagesByParentId(parentId);
        }

        #endregion

        #region DbObject Members

        /// <summary>
        /// Executes the stored procedure that adds the object to the database
        /// </summary>
        protected override void ExecuteAddStoredProcedure()
        {
            Id = Data.PortfolioCategory.AddPortfolioCategory(this);
        }

        /// <summary>
        /// Executes the stored procedure that updates the row in the database representing the object
        /// </summary>
        protected override void ExecuteUpdateStoredProcedure()
        {
            Data.PortfolioCategory.UpdatePortfolioCategory(this);
        }

        /// <summary>
        /// Executes the stored procedure that deletes the row from the database representing the object
        /// </summary>
        protected override void ExecuteDeleteStoredProcedure()
        {
            Data.PortfolioCategory.DeletePortfolioCategory(Id);
        }

        /// <summary>
        /// Determines whether the current properties for the object are valid
        /// </summary>
        /// <param name="error">Exception thrown if property is invalid</param>
        /// <returns>True if properties are valid, false if not</returns>
        protected override bool ValidateProperties(out Exception.InvalidPropertyException error)
        {
            error = null;

            if (String.IsNullOrEmpty(Name))
                error = new Exception.InvalidPropertyException("PortfolioCategory.Name cannot be null");

            return (error == null);
        }

        /// <summary>
        /// Executes the stored procedure that deletes the row from the database representing the object
        /// </summary>
        protected override void ResetIdToDefaultValue()
        {
            Id = -1;
        }

        internal override Data.StoredProcedureParameter[] GetParametersForStoredProcedure(bool updatedStoredProc)
        {
            List<Data.StoredProcedureParameter> parameters = new List<Data.StoredProcedureParameter>();

            if (updatedStoredProc)
                parameters.Add(Data.Control.GetParameter("PortfolioCategoryId", DbType.Int16, Id));

            parameters.Add(Data.Control.GetParameter("Name", DbType.String, Name));

            return parameters.ToArray();
        }

        #endregion
    }
}
