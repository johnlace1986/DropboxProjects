using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NisbetPhotography.DbObjects.Business
{
    public class CustomerAlbum : DbObject
    {
        #region Fields

        /// <summary>
        /// Unique identifier of the customer the album belongs to
        /// </summary>
        internal Guid customerId;

        /// <summary>
        /// Customer the album belongs to
        /// </summary>
        internal User customer;

        #endregion

        #region Properties

        /// <summary>
        /// Unique identifier of the album
        /// </summary>
        public Int16 Id { get; internal set; }

        /// <summary>
        /// Customer the album belongs to
        /// </summary>
        public User Customer
        {
            get
            {
                if (customerId == Guid.Empty)
                    return null;

                if (customer == null)
                    customer = new User(customerId);

                return customer;
            }
            set
            {
                if (IsInDatabase)
                    throw new Exception.DbObjectAlreadyInDatabaseException("Cannot set the Customer property of an album that already exists in the database");

                if (value == null)
                    customerId = Guid.Empty;
                else
                    customerId = value.UserId;

                customer = value;
            }
        }

        /// <summary>
        /// Name of the album
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Description of the album
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// All images that have been added to the album
        /// </summary>
        public CustomerImage[] Images { get; internal set; }

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
        public CustomerImage Thumbnail
        {
            get
            {
                foreach (CustomerImage image in Images)
                    if (image.Thumbnail)
                        return image;

                return new CustomerImage() { Id = -1, ImageUrl = null, Thumbnail = true };
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
        public CustomerAlbum()
        {
            this.Id = -1;
            this.Customer = null;
            this.Name = String.Empty;
            this.Description = String.Empty;
            this.Images = new CustomerImage[0];
            this.DateCreated = DateTime.Now;
            this.IsInDatabase = false;
        }

        /// <summary>
        /// Loads a new instance of the CustomerAlbum class from the database
        /// </summary>
        /// <param name="customerAlbumId">Unique identifier of the album</param>
        /// <param name="customerId">Unique identifier of the customer the album belongs to</param>
        public CustomerAlbum(Int16 customerAlbumId, Guid customerId)
        {
            CustomerAlbum clone = Data.CustomerAlbum.GetCustomerAlbumById(customerAlbumId, customerId);

            this.Id = clone.Id;
            this.customerId = clone.customerId;
            this.customer = clone.customer;
            this.Name = clone.Name;
            this.Description = clone.Description;
            this.Images = clone.Images;
            this.DateCreated = clone.DateCreated;
            this.IsInDatabase = true;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds an image to the album
        /// </summary>
        /// <param name="imageUrl">URL of the image being added</param>
        public void AddImage(String imageUrl)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot add image to album because album does not exist in the database");

            List<CustomerImage> currentImages = new List<CustomerImage>();

            foreach (CustomerImage image in Images)
                currentImages.Add(image);

            Boolean blnThumbnail = (currentImages.Count == 0);

            Int16 customerImageId = Data.CustomerAlbum.AddImageToCustomerAlbum(Id, customerId, imageUrl, blnThumbnail);
            currentImages.Add(new CustomerImage() { Id = customerImageId, ImageUrl = imageUrl, Thumbnail = blnThumbnail });
            Images = currentImages.ToArray();
        }

        /// <summary>
        /// Removes an image from the album
        /// </summary>
        /// <param name="index">Array index of the image being removed</param>
        public void RemoveImage(Int16 customerImageId)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot remove image from album because album does not exist in the database");

            CustomerImage? pi = null;

            List<CustomerImage> currentImages = new List<CustomerImage>();

            foreach (CustomerImage image in Images)
            {
                if (image.Id == customerImageId)
                    pi = image;
                else
                    currentImages.Add(image);
            }

            if (pi == null)
                throw new IndexOutOfRangeException("The album does not contain an image with the Id number \"" + customerImageId.ToString() + "\"");

            Boolean wasThumbnail = pi.Value.Thumbnail;
            Data.CustomerAlbum.RemoveImageFromCustomerAlbum(customerImageId, Id, customerId);

            Images = currentImages.ToArray();

            if (Images.Length > 0)
                if (wasThumbnail)
                    SetThumbnailImage(Images[0].Id);
        }

        /// <summary>
        /// Sets the thumbnail image for the album
        /// </summary>
        /// <param name="index">Array index of the image being used as the thumbnail</param>
        public void SetThumbnailImage(Int16 customerImageId)
        {
            if (!(IsInDatabase))
                throw new Exception.DbObjectNotInDatabaseException("Cannot set the thumbnail image for album because album does not exist in the database");

            CustomerImage? pi = null;

            List<CustomerImage> currentImages = new List<CustomerImage>();

            for (int i = 0; i < Images.Length; i++)
            {
                if (Images[i].Id == customerImageId)
                {
                    pi = Images[i];
                    Images[i].Thumbnail = true;
                    Data.CustomerAlbum.SetCustomerAlbumThumbnailImage(Images[i].Id, Id, customerId);
                }
                else
                    Images[i].Thumbnail = false;
            }

            if (pi == null)
                throw new IndexOutOfRangeException("The album does not contain an image with the Id number \"" + customerImageId.ToString() + "\"");
        }

        /// <summary>
        /// Gets the porfolio image from the Images array with the specified Id number
        /// </summary>
        /// <param name="customerImageId">Unique identifier of the image to look for</param>
        /// <returns>CustomerImage object with the relevant Id number</returns>
        public CustomerImage GetCustomerImageById(Int16 customerImageId)
        {
            foreach (CustomerImage image in Images)
                if (image.Id == customerImageId)
                    return image;

            throw new IndexOutOfRangeException("The album does not contain an image with the Id number \"" + customerImageId.ToString() + "\"");
        }

        #endregion

        #region DbObject Members

        protected override void ExecuteAddStoredProcedure()
        {
            Id = Data.CustomerAlbum.AddCustomerAlbum(this);

            if (customer != null)
                customer.AddAlbum(this);
        }

        protected override void ExecuteUpdateStoredProcedure()
        {
            Data.CustomerAlbum.UpdateCustomerAlbum(this);
        }

        protected override void ExecuteDeleteStoredProcedure()
        {
            Data.CustomerAlbum.DeleteCustomerAlbum(Id, customerId);

            if (customer != null)
                customer.RemoveAlbum(this);
        }

        protected override bool ValidateProperties(out Exception.InvalidPropertyException error)
        {
            error = null;

            if (Customer == null)
                error = new Exception.InvalidPropertyException("Customer property cannot be null");

            if (Customer.Admin)
                error = new Exception.InvalidPropertyException("Cannot add an album to an Admin user");

            if (String.IsNullOrEmpty(Name))
                error = new Exception.InvalidPropertyException("Name property cannot be null");

            return (error == null);
        }

        protected override void ResetIdToDefaultValue()
        {
            Id = -1;
            Customer = null;
        }

        internal override Data.StoredProcedureParameter[] GetParametersForStoredProcedure(bool updatedStoredProc)
        {
            List<Data.StoredProcedureParameter> parameters = new List<Data.StoredProcedureParameter>();
            
            if (updatedStoredProc)
                parameters.Add(Data.Control.GetParameter("CustomerAlbumId", DbType.Int16, Id));

            parameters.Add(Data.Control.GetParameter("CustomerId", DbType.Guid, customerId));
            parameters.Add(Data.Control.GetParameter("Name", DbType.String, Name));
            parameters.Add(Data.Control.GetParameter("Description", DbType.String, Description));
            parameters.Add(Data.Control.GetParameter("DateCreated", DbType.DateTime, DateCreated));

            return parameters.ToArray();
        }

        #endregion
    }
}
