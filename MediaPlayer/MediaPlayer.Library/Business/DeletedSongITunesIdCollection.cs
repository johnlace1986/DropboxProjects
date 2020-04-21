using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Utilities.Data;

namespace MediaPlayer.Library.Business
{
    public class DeletedSongITunesIdCollection : IEnumerable<Int16>
    {
        #region Fields

        /// <summary>
        /// Gets or sets the collection of iTunes IDs of songs that have been deleted
        /// </summary>
        private List<Int16> deletedSongITunesIds = new List<Int16>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the iTunes ID in the collection at the specified index
        /// </summary>
        /// <param name="index">Index of the desired iTunes ID</param>
        /// <returns>iTunes ID in the collection at the specified index</returns>
        public Int16 this[Int32 index]
        {
            get { return deletedSongITunesIds[index]; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the DeletedSongITunesIdCollection class
        /// </summary>
        public DeletedSongITunesIdCollection()
        {
            using (SqlConnection conn = SqlDbObject.GetConnection())
            {
                try
                {
                    conn.Open();
                    deletedSongITunesIds = new List<Int16>(Data.DeletedSongITunesIdCollection.GetDeletedSongITunesIds(conn));
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds a new iTunes ID to the collection
        /// </summary>
        /// <param name="iTunesId">iTunes ID being added to the collection</param>
        public void Add(Int16 iTunesId)
        {
            if (deletedSongITunesIds.Any(p => p == iTunesId))
                throw new ArgumentException("iTunes ID already exists");

            using (SqlConnection conn = SqlDbObject.GetConnection())
            {
                try
                {
                    conn.Open();
                    Data.DeletedSongITunesIdCollection.AddDeletedSongITunesId(conn, iTunesId);
                }
                finally
                {
                    conn.Close();
                }
            }

            deletedSongITunesIds.Add(iTunesId);
            deletedSongITunesIds.Sort();
        }

        /// <summary>
        /// Removes the specified iTunes ID from the collection
        /// </summary>
        /// <param name="iTunesId">iTunes ID being removed from the collection</param>
        public void Remove(Int16 iTunesId)
        {
            if (!deletedSongITunesIds.Any(p => p == iTunesId))
                throw new ArgumentException("iTunes ID does not exist");

            using (SqlConnection conn = SqlDbObject.GetConnection())
            {
                try
                {
                    conn.Open();
                    Data.DeletedSongITunesIdCollection.DeleteDeletedSongITunesId(conn, iTunesId);
                }
                finally
                {
                    conn.Close();
                }
            }

            deletedSongITunesIds.Remove(iTunesId);
        }

        #endregion

        #region IEnumerable<Int16> Members

        public IEnumerator<short> GetEnumerator()
        {
            return deletedSongITunesIds.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return deletedSongITunesIds.GetEnumerator();
        }

        #endregion
    }
}
