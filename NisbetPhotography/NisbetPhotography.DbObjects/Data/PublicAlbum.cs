﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NisbetPhotography.DbObjects.Data
{
    internal static class PublicAlbum
    {
        /// <summary>
        /// Creates a new Business.PublicAlbum object from the data in the DataTableReader
        /// </summary>
        /// <param name="dtr">DataTableReader containing the PublicAlbum's data</param>
        /// <returns>Business.PublicAlbum object</returns>
        private static Business.PublicAlbum GetPublicAlbumFromDataTableReader(DataTableReader dtr)
        {
            Business.PublicAlbum album = new Business.PublicAlbum();
            album.Id = (Int16)dtr["Id"];
            album.Name = (String)dtr["Name"];
            album.Description = (String)dtr["Description"];
            album.Images = GetPublicImagesByParentId(album.Id);
            album.DateCreated = (DateTime)dtr["DateCreated"];
            album.IsInDatabase = true;

            return album;
        }

        /// <summary>
        /// Returns all albums in the database
        /// </summary>
        /// <returns>Array containing all albums in the database</returns>
        public static Business.PublicAlbum[] GetPublicAlbums()
        {
            List<Business.PublicAlbum> albums = new List<Business.PublicAlbum>();

            using (DataTableReader dtr = Control.ExecuteReader("GetPublicAlbums"))
            {
                while (dtr.Read())
                    albums.Add(GetPublicAlbumFromDataTableReader(dtr));
            }

            return albums.ToArray();
        }

        /// <summary>
        /// Loads a specific album from the database
        /// </summary>
        /// <param name="publicAlbumId">Unique identifier of the album being loaded</param>
        /// <returns>Business.PublicAlbum object</returns>
        public static Business.PublicAlbum GetPublicAlbumById(Int16 publicAlbumId)
        {
            using (DataTableReader dtr = Control.ExecuteReader("GetPublicAlbumById", Control.GetParameter("PublicAlbumId", DbType.Int16, publicAlbumId)))
            {
                if (dtr.Read())
                    return GetPublicAlbumFromDataTableReader(dtr);
                else
                    throw new Exception.SpecifiedDbObjectNotFoundException("No album exists in the database with the Id number \"" + publicAlbumId.ToString() + "\"");
            }
        }

        /// <summary>
        /// Adds the album's details to the database
        /// </summary>
        /// <param name="publicAlbum">PublicAlbum object being added to the database</param>
        /// <returns>New unique identifier for the PublicAlbum, generated by the database</returns>
        public static Int16 AddPublicAlbum(Business.PublicAlbum publicAlbum)
        {
            try
            {
                return (Int16)Control.ExecuteScalar("AddPublicAlbum", publicAlbum.GetParametersForStoredProcedure(false));
            }
            catch (System.Exception e)
            {
                throw new Exception.AddDbObjectException("Could not add public album", e);
            }
        }

        /// <summary>
        /// Updates the album's details in the database
        /// </summary>
        /// <param name="publicAlbum">PublicAlbum object being updated in the database</param>
        public static void UpdatePublicAlbum(Business.PublicAlbum publicAlbum)
        {
            try
            {
                Control.ExecuteNonQuery("UpdatePublicAlbum", publicAlbum.GetParametersForStoredProcedure(true));
            }
            catch (System.Exception e)
            {
                throw new Exception.UpdateDbObjectException("Could not update public album", e);
            }
        }

        /// <summary>
        /// Deletes the specified album from the database
        /// </summary>
        /// <param name="publicAlbumId">Unique identifier of the album being deleted</param>
        public static void DeletePublicAlbum(Int16 publicAlbumId)
        {
            try
            {
                Control.ExecuteNonQuery("DeletePublicAlbum", Control.GetParameter("PublicAlbumId", DbType.Int16, publicAlbumId));
            }
            catch (System.Exception e)
            {
                throw new Exception.DeleteDbObjectException("Could not delete album", e);
            }
        }

        /// <summary>
        /// Determines whether a public album currently exists with the specified name
        /// </summary>
        /// <param name="name">Name to search for</param>
        /// <returns>True if an album already exists with the specified name, false if not</returns>
        public static Boolean PublicAlbumNameExists(String name)
        {
            return (Boolean)Control.ExecuteScalar("PublicAlbumNameExists", Control.GetParameter("Name", DbType.String, name));
        }

        /// <summary>
        /// Returns all images that belong to the specified album
        /// </summary>
        /// <param name="parentId">Unique identifier of the parent album</param>
        /// <returns>Array containing all images that belong to the specified album</returns>
        public static Business.PublicImage[] GetPublicImagesByParentId(Int16 parentId)
        {
            List<Business.PublicImage> albums = new List<Business.PublicImage>();

            using (DataTableReader dtr = Control.ExecuteReader("GetPublicImagesByParentId", Control.GetParameter("ParentId", DbType.Int16, parentId)))
            {
                while (dtr.Read())
                    albums.Add(new Business.PublicImage()
                        {
                            Id = (Int16)dtr["Id"],
                            ImageUrl = (String)dtr["ImageUrl"],
                            Caption = (String)dtr["Caption"],
                            Thumbnail = (Boolean)dtr["Thumbnail"]
                        });
            }

            return albums.ToArray();
        }

        /// <summary>
        /// Adds an image to the public album
        /// </summary>
        /// <param name="parentId">Unique identifier of album being added to</param>
        /// <param name="imageUrl">URL to physical image file being added</param>
        /// <param name="caption">Caption to be displayed with the image</param>
        /// <param name="thumbnail">Determines whether the image will act as the thumbail for the album</param>
        /// <returns>New unique identifier of the image, generated by the database</returns>
        public static Int16 AddImageToPublicAlbum(Int16 parentId, String imageUrl, String caption, Boolean thumbnail)
        {
            try
            {
                List<StoredProcedureParameter> parameters = new List<StoredProcedureParameter>();
                parameters.Add(Control.GetParameter("ParentId", DbType.Int16, parentId));
                parameters.Add(Control.GetParameter("ImageUrl", DbType.String, imageUrl));
                parameters.Add(Control.GetParameter("Caption", DbType.String, caption));
                parameters.Add(Control.GetParameter("Thumbnail", DbType.Boolean, thumbnail));

                return (Int16)Control.ExecuteScalar("AddImageToPublicAlbum", parameters.ToArray());
            }
            catch (System.Exception e)
            {
                throw new Exception.AddChildToParentException("Could not add image to public album", e);
            }
        }

        /// <summary>
        /// Removes an image from the specified album
        /// </summary>
        /// <param name="publicImageId">Unique identifier of image being removed</param>
        /// <param name="parentId">Unique identifier of album image is being removed from</param>
        public static void RemoveImageFromPublicAlbum(Int16 parentId, Int16 publicImageId)
        {
            try
            {
                List<StoredProcedureParameter> parameters = new List<StoredProcedureParameter>();
                parameters.Add(Control.GetParameter("ParentId", DbType.Int16, parentId));
                parameters.Add(Control.GetParameter("PublicImageId", DbType.Int16, publicImageId));

                Control.ExecuteNonQuery("RemoveImageFromPublicAlbum", parameters.ToArray());
            }
            catch (System.Exception e)
            {
                throw new Exception.RemoveChildFromParentException("Could not remove image from album", e);
            }
        }

        /// <summary>
        /// Sets the thumbnail image for the specified album to the specified image
        /// </summary>
        /// <param name="parentId">Unique identifier of the album the thumbnail is being set for</param>
        /// <param name="portfolioImageId">Unique identifier of the image to be set at the thumbnail image for the album</param>
        public static void SetPublicAlbumThumbnailImage(Int16 parentId, Int16 publicImageId)
        {
            try
            {
                List<StoredProcedureParameter> parameters = new List<StoredProcedureParameter>();
                parameters.Add(Control.GetParameter("ParentId", DbType.Int16, parentId));
                parameters.Add(Control.GetParameter("PublicImageId", DbType.Int16, publicImageId));

                Control.ExecuteNonQuery("SetPublicAlbumThumbnailImage", parameters.ToArray());
            }
            catch (System.Exception e)
            {
                throw new Exception.SetThumbnailImageException("Could not set album thumbnail image", e);
            }
        }

        /// <summary>
        /// Chages the caption of the image with the specified Id number
        /// </summary>
        /// <param name="parentId">Unique identifier of the album containing the image</param>
        /// <param name="publicImageId">Unique identifier of the image who's caption is being changed</param>
        /// <param name="caption">New caption for the image</param>
        public static void ChangePublicImageCaption(Int16 parentId, Int16 publicImageId, String caption)
        {
            try
            {
                List<StoredProcedureParameter> parameters = new List<StoredProcedureParameter>();
                parameters.Add(Control.GetParameter("ParentId", DbType.Int16, parentId));
                parameters.Add(Control.GetParameter("PublicImageId", DbType.Int16, publicImageId));
                parameters.Add(Control.GetParameter("Caption", DbType.String, caption));

                Control.ExecuteNonQuery("ChangePublicImageCaption", parameters.ToArray());
            }
            catch (System.Exception e)
            {
                throw new Exception.SetImageCaptionException("Could not set the image's caption", e);
            }
        }
    }
}