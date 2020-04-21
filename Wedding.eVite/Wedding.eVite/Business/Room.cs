using System;
using System.Data;
using System.Data.SqlClient;
using Utilities.Data;
using Utilities.Data.SQL;
using sp = Utilities.Data.StoredProcedure;

namespace Wedding.eVite.Business
{
    [DbObjectMetaData("RoomId", DbType.Int32)]
    public class Room : SqlDbObject<Int32>, IComparable<Room>
    {
        #region Fields
        
        /// <summary>
        /// Gets or sets the name of the room
        /// </summary>
        private String name;

        /// <summary>
        /// Gets or sets the number of beds in the room
        /// </summary>
        private Int32 beds;

        /// <summary>
        /// Gets or sets the guests assigned to the room
        /// </summary>
        private Guest[] guests;

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets the name of the room
        /// </summary>
        [sp.DataColumn("Name", DbType.String)]
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the number of beds in the room
        /// </summary>
        [sp.DataColumn("Beds", DbType.Int32)]
        public Int32 Beds
        {
            get { return beds; }
            set
            {
                beds = value;
                OnPropertyChanged("Beds");
            }
        }

        /// <summary>
        /// Gets or sets the guests assigned to the room
        /// </summary>
        public Guest[] Guests
        {
            get { return guests; }
            private set
            {
                guests = value;
                OnPropertyChanged("Guests");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Room class
        /// </summary>
        public Room()
            : base()
        {
            Id = -1;
            Name = String.Empty;
            Beds = 0;
            Guests = new Guest[0];
        }

        /// <summary>
        /// Initialises a new instance of the Room class
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="roomId">Unique identifier of the room</param>
        public Room(SqlConnection conn, Int32 roomId)
            : base(conn, roomId)
        {
        }

        #endregion
        
        #region Static Methods
        
        /// <summary>
        /// Gets the rooms currently in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>Rooms currently in the system</returns>
        public static Room[] GetRooms(SqlConnection conn)
        {
            return Data.Room.GetRooms(conn);
        }

        #endregion

        #region SqlDbObject Members

        protected override void Load(SqlConnection conn, Int32 id)
        {
            Room room = Data.Room.GetRoomById(conn, id);

            Id = room.Id;
            Name = room.Name;
            Beds = room.Beds;
            Guests = room.Guests;
        }

        protected override void Load(SqlConnection conn, sp.DataRow row)
        {
            base.Load(conn, row);
            Guests = Guest.GetGuestsByRoomId(conn, Id);
        }

        protected override void AddToDatabase(SqlConnection conn)
        {
            AddToDatabase(conn, "AddRoom");
        }

        protected override void UpdateInDatabase(SqlConnection conn)
        {
            UpdateInDatabase(conn, "UpdateRoom");
        }

        protected override void DeleteFromDatabase(SqlConnection conn)
        {
            DeleteFromDatabase(conn, "DeleteRoom");
        }

        protected override void ResetProperties()
        {
            Id = -1;
        }

        #endregion

        #region IComparable<Room> Members

        public int CompareTo(Room other)
        {
            return Id.CompareTo(other.Id);
        }

        #endregion
    }
}
