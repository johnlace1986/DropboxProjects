using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Utilities.Business;
using Utilities.Data;
using sp = Utilities.Data.StoredProcedure;

namespace Wedding.eVite.Business
{
    [Serializable]
    public class Guest : NotifyPropertyChangedObject
    {
        #region Fields

        /// <summary>
        /// Gets or sets the unique identifier of the guest
        /// </summary>
        private Int32 id;

        /// <summary>
        /// Gets or sets the forename of the guest
        /// </summary>
        private String forename;

        /// <summary>
        /// Gets or sets the surname of the guest
        /// </summary>
        private String surname;

        /// <summary>
        /// Gets or sets a value determining whether or not the guest is attending
        /// </summary>
        private Boolean? isAttending;

        /// <summary>
        /// Gets or sets the date and time the guest RSVP'd to the invite
        /// </summary>
        private DateTime? dateOfRsvp;

        /// <summary>
        /// Gets or sets a value determining whether or not the guest is a child
        /// </summary>
        private Boolean isChild;

        /// <summary>
        /// Gets or sets a value determining whether or not the guest is a vegetarian
        /// </summary>
        private Boolean isVegetarian;

        /// <summary>
        /// Gets or sets a value determining whether or not the guest is allowed to bring a "plus one"
        /// </summary>
        private Boolean canBringPlusOne;

        /// <summary>
        /// Gets or sets a value determining whether or not the guest is bringing a "plus one"
        /// </summary>
        private Boolean isBringingPlusOne;

        /// <summary>
        /// Gets or sets the forename of the guest's "plus one"
        /// </summary>
        private String plusOneForename;

        /// <summary>
        /// Gets or sets the surname of the guest's "plus one"
        /// </summary>
        private String plusOneSurname;

        /// <summary>
        /// Gets or sets a value determining whether or not the guest's "plus one" is a vegetarian
        /// </summary>
        private Boolean plusOneIsVegetarian;

        /// <summary>
        /// Gets or sets the unique identifier of the table the guest has been assigned to
        /// </summary>
        private Int32? tableId;

        /// <summary>
        /// Gets or sets the unique identifier of the room the guest has been assigned to
        /// </summary>
        private Int32? roomId;

        /// <summary>
        /// Gets or sets the notes for the guest
        /// </summary>
        private String notes;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the guest
        /// </summary>
        public Int32 Id
        {
            get { return id; }
            private set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// Gets or sets the forename of the guest
        /// </summary>
        public String Forename
        {
            get { return forename; }
            set
            {
                forename = value;
                OnPropertyChanged("Forename");
            }
        }

        /// <summary>
        /// Gets or sets the surname of the guest
        /// </summary>
        public String Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }

        /// <summary>
        /// Gets the full name of the guest
        /// </summary>
        public String FullName
        {
            get { return GetGuestFullName(Forename, Surname); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the guest is attending
        /// </summary>
        public Boolean? IsAttending
        {
            get { return isAttending; }
            set
            {
                isAttending = value;
                OnPropertyChanged("IsAttending");
            }
        }

        /// <summary>
        /// Gets or sets the date and time the guest RSVP'd to the invite
        /// </summary>
        public DateTime? DateOfRsvp
        {
            get { return dateOfRsvp; }
            set
            {
                dateOfRsvp = value;
                OnPropertyChanged("DateOfRsvp");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the guest is a child
        /// </summary>
        public Boolean IsChild
        {
            get { return isChild; }
            set
            {
                isChild = value;
                OnPropertyChanged("IsChild");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the guest is a vegetarian
        /// </summary>
        public Boolean IsVegetarian
        {
            get { return isVegetarian; }
            set
            {
                isVegetarian = value;
                OnPropertyChanged("IsVegetarian");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the guest is allowed to bring a "plus one"
        /// </summary>
        public Boolean CanBringPlusOne
        {
            get { return canBringPlusOne; }
            set
            {
                canBringPlusOne = value;
                OnPropertyChanged("CanBringPlusOne");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the guest is bringing a "plus one"
        /// </summary>
        public Boolean IsBringingPlusOne
        {
            get { return isBringingPlusOne; }
            set
            {
                isBringingPlusOne = value;
                OnPropertyChanged("IsBringingPlusOne");
            }
        }

        /// <summary>
        /// Gets or sets the forename of the guest's "plus one"
        /// </summary>
        public String PlusOneForename
        {
            get { return plusOneForename; }
            set
            {
                plusOneForename = value;
                OnPropertyChanged("PlusOneForename");
            }
        }

        /// <summary>
        /// Gets or sets the surname of the guest's "plus one"
        /// </summary>
        public String PlusOneSurname
        {
            get { return plusOneSurname; }
            set
            {
                plusOneSurname = value;
                OnPropertyChanged("PlusOneSurname");
            }
        }

        /// <summary>
        /// Gets the full name of the guest's "plus one"
        /// </summary>
        public String PlusOneFullName
        {
            get { return GetGuestFullName(PlusOneForename, PlusOneSurname); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the guest's "plus one" is a vegetarian
        /// </summary>
        public Boolean PlusOneIsVegetarian
        {
            get { return plusOneIsVegetarian; }
            set
            {
                plusOneIsVegetarian = value;
                OnPropertyChanged("PlusOneIsVegetarian");
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the table the guest has been assigned to
        /// </summary>
        public Int32? TableId
        {
            get { return tableId; }
            private set
            {
                tableId = value;
                OnPropertyChanged("TableId");
            }
        }

        /// <summary>
        /// Gets or sets the unique identifier of the room the guest has been assigned to
        /// </summary>
        public Int32? RoomId
        {
            get { return roomId; }
            private set
            {
                roomId = value;
                OnPropertyChanged("RoomId");
            }
        }

        /// <summary>
        /// Gets a value determining whether or not the guest has completely RSVP'd to the invite
        /// </summary>
        public Boolean RSVP
        {
            get
            {
                if (IsAttending == null)
                    return false;

                if (IsAttending.Value)
                {
                    if (CanBringPlusOne)
                    {
                        if (IsBringingPlusOne)
                        {
                            if (String.IsNullOrEmpty(PlusOneForename))
                                return false;

                            if (String.IsNullOrEmpty(PlusOneSurname))
                                return false;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets or sets the notes for the guest
        /// </summary>
        public String Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged("Notes");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Guest class
        /// </summary>
        public Guest()
            : base()
        {
            Id = -1;
            Forename = String.Empty;
            Surname = String.Empty;
            IsAttending = null;
            DateOfRsvp = null;
            IsChild = true;
            IsVegetarian = false;
            CanBringPlusOne = false;
            IsBringingPlusOne = false;
            PlusOneForename = String.Empty;
            PlusOneSurname = String.Empty;
            PlusOneIsVegetarian = false;
            TableId = null;
            RoomId = null;
            Notes = String.Empty;
        }

        /// <summary>
        /// Initialises a new instance of the Guest class
        /// </summary>
        /// <param name="guestId">Unique identifier of the guest</param>
        /// <param name="forename">Forename of the guest</param>
        /// <param name="surname">Surname of the guest</param>
        /// <param name="isAttending">Value determining whether or not the guest is attending</param>
        /// <param name="dateOfRsvp">Date and time the guest RSVP'd to the invite</param>
        /// <param name="isChild">Value determining whether or not the guest is a child</param>
        /// <param name="isVegetarian">Value determining whether or not the guest is a vegetarian</param>
        /// <param name="canBringPlusOne">Value determining whether or not the guest is allowed to bring a "plus one"</param>
        /// <param name="tableId">Unique identifier of the table the guest has been assigned to</param>
        /// <param name="roomId">Unique identifier of the room the guest has been assigned to</param>
        public Guest(Int32 guestId, String forename, String surname, Boolean? isAttending, DateTime? dateOfRsvp, Boolean isChild, Boolean isVegetarian, Boolean canBringPlusOne, Int32? tableId, Int32? roomId, String notes)
            : this()
        {
            Id = guestId;
            Forename = forename;
            Surname = surname;
            IsAttending = isAttending;
            DateOfRsvp = dateOfRsvp;
            IsChild = isChild;
            IsVegetarian = isVegetarian;
            CanBringPlusOne = canBringPlusOne;
            TableId = tableId;
            RoomId = roomId;
            Notes = notes;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds the guest to the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite the guest is a member of</param>
        internal void Add(SqlConnection conn, Int32 inviteId)
        {
            Id = Data.Guest.AddGuest(conn, inviteId, this);
        }

        /// <summary>
        /// <Updates the guest in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite the guest is a member of</param>
        internal void Update(SqlConnection conn, Int32 inviteId)
        {
            Data.Guest.UpdateGuest(conn, inviteId, this);
        }

        /// <summary>
        /// Deletes the guest from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        internal void Delete(SqlConnection conn)
        {
            Data.Guest.DeleteGuest(conn, Id);
            Id = -1;
        }

        /// <summary>
        /// Creates a stored procedure parameter for each property in the guest
        /// </summary>
        /// <param name="inviteId">Unique identifier of the invite the guest is a member of</param>
        /// <param name="includeId">Value determining whether or not the unique identifier of the guest should be included</param>
        /// <returns>Stored procedure parameter for each property in the guest</returns>
        internal sp.ParameterCollection GetParametersForStoredProcedure(Int32 inviteId, Boolean includeId)
        {
            sp.ParameterCollection parameters = new sp.ParameterCollection();

            if (includeId)
                parameters.AddWithValue("GuestId", DbType.Int32, Id);

            parameters.AddWithValue("InviteId", DbType.Int32, inviteId);
            parameters.AddWithValue("Forename", DbType.String, Forename);
            parameters.AddWithValue("Surname", DbType.String, Surname);
            parameters.AddWithValue("IsAttending", DbType.Boolean, IsAttending);
            parameters.AddWithValue("DateOfRsvp", DbType.DateTime, DateOfRsvp);
            parameters.AddWithValue("IsChild", DbType.Boolean, IsChild);
            parameters.AddWithValue("IsVegetarian", DbType.Boolean, IsVegetarian);
            parameters.AddWithValue("CanBringPlusOne", DbType.Boolean, CanBringPlusOne);
            parameters.AddWithValue("IsBringingPlusOne", DbType.Boolean, IsBringingPlusOne);
            parameters.AddWithValue("PlusOneForename", DbType.String, PlusOneForename);
            parameters.AddWithValue("PlusOneSurname", DbType.String, PlusOneSurname);
            parameters.AddWithValue("PlusOneIsVegetarian", DbType.Boolean, PlusOneIsVegetarian);
            parameters.AddWithValue("TableId", DbType.Int32, TableId);
            parameters.AddWithValue("RoomId", DbType.Int32, RoomId);
            parameters.AddWithValue("Notes", DbType.String, Notes);

            return parameters;
        }

        /// <summary>
        /// Creates an XML element containing the data for the guest
        /// </summary>
        /// <param name="document">XML document the element will be added to</param>
        /// <returns>XML element containing the data for the guest</returns>
        internal XmlElement ToXml(XmlDocument document)
        {
            XmlElement guest = document.CreateElement("GUEST");

            XmlAttribute id = document.CreateAttribute("Id");
            id.Value = Id.ToString();
            guest.Attributes.Append(id);

            XmlElement forename = document.CreateElement("FORENAME");
            forename.InnerText = Forename;
            guest.AppendChild(forename);

            XmlElement surname = document.CreateElement("SURNAME");
            surname.InnerText = Surname;
            guest.AppendChild(surname);

            XmlAttribute isAttending = document.CreateAttribute("IsAttending");
            isAttending.Value = (IsAttending.HasValue ? IsAttending.ToString() : String.Empty);
            guest.Attributes.Append(isAttending);

            XmlAttribute dateOfRsvp = document.CreateAttribute("DateOfRsvp");
            dateOfRsvp.Value = (DateOfRsvp.HasValue ? DateOfRsvp.ToString() : String.Empty);
            guest.Attributes.Append(dateOfRsvp);

            XmlAttribute isChild = document.CreateAttribute("IsChild");
            isChild.Value = IsChild.ToString();
            guest.Attributes.Append(isChild);

            XmlAttribute isVegetarian = document.CreateAttribute("IsVegetarian");
            isVegetarian.Value = IsVegetarian.ToString();
            guest.Attributes.Append(isVegetarian);

            XmlElement plusOne = document.CreateElement("PLUS_ONE");
            guest.AppendChild(plusOne);

            XmlAttribute canBringPlusOne = document.CreateAttribute("CanBringPlusOne");
            canBringPlusOne.Value = CanBringPlusOne.ToString();
            plusOne.Attributes.Append(canBringPlusOne);

            XmlAttribute isBringingPlusOne = document.CreateAttribute("IsBringingPlusOne");
            isBringingPlusOne.Value = IsBringingPlusOne.ToString();
            plusOne.Attributes.Append(isBringingPlusOne);

            XmlElement plusOneForename = document.CreateElement("FORENAME");
            plusOneForename.InnerText = PlusOneForename;
            plusOne.AppendChild(plusOneForename);

            XmlElement plusOneSurname = document.CreateElement("SURNAME");
            plusOneSurname.InnerText = PlusOneSurname;
            plusOne.AppendChild(plusOneSurname);

            XmlAttribute plusOneIsVegetarian = document.CreateAttribute("IsVegetarian");
            plusOneIsVegetarian.Value = PlusOneIsVegetarian.ToString();
            plusOne.Attributes.Append(plusOneIsVegetarian);

            XmlElement notes = document.CreateElement("NOTES");
            notes.InnerText = Notes;
            guest.AppendChild(notes);

            return guest;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Group the specified guests by their surname
        /// </summary>
        /// <param name="guests">Guests being grouped</param>
        /// <returns>String representing the group of guests</returns>
        internal static String GroupGuestsBySurname(IEnumerable<Guest> guests)
        {
            //get the surname of the guests (error if there is not exactly 1 surname in the group)
            String surname = guests.Select(p => p.Surname).Distinct().Single();

            String guestList = GeneralMethods.GroupItems<Guest>(guests, guest => guest.Forename, ", ", " and ");
            guestList += " " + surname;

            return guestList;
        }

        /// <summary>
        /// Gets the full name of a guest
        /// </summary>
        /// <param name="forename">Forename of the guest</param>
        /// <param name="surname">Surname of the guest</param>
        /// <returns>Full name of the guest</returns>
        public static String GetGuestFullName(String forename, String surname)
        {
            return (forename + " " + surname).Trim();
        }

        /// <summary>
        /// Loads an instance of the Guest class using the data found in the data row
        /// </summary>
        /// <param name="row">Data row containing the data for the guest</param>
        /// <returns>Instance of the Guest class using the data found in the data row</returns>
        internal static Guest FromDataRow(sp.DataRow row)
        {
            Int32 id = (Int32)row["GuestId"];
            String forename = (String)row["Forename"];
            String surname = (String)row["Surname"];
            Boolean? isAttending = (Boolean?)row["IsAttending"];
            DateTime? dateOfRsvp = (DateTime?)row["DateOfRsvp"];
            Boolean isChild = (Boolean)row["IsChild"];
            Boolean isVegetarian = (Boolean)row["IsVegetarian"];
            Boolean canBringPlusOne = (Boolean)row["CanBringPlusOne"];
            Boolean isBringingPlusOne = (Boolean)row["IsBringingPlusOne"];
            String plusOneForename = (String)row["PlusOneForename"];
            String plusOneSurname = (String)row["PlusOneSurname"];
            Boolean plusOneIsVegetarian = (Boolean)row["PlusOneIsVegetarian"];
            Int32? tableId = (Int32?)row["TableId"];
            Int32? roomId = (Int32?)row["RoomId"];
            String notes = (String)row["Notes"];

            return new Guest()
            {
                Id = id,
                Forename = forename,
                Surname = surname,
                IsAttending = isAttending,
                DateOfRsvp = dateOfRsvp,
                IsChild = isChild,
                IsVegetarian = isVegetarian,
                CanBringPlusOne = canBringPlusOne,
                IsBringingPlusOne = isBringingPlusOne,
                PlusOneForename = plusOneForename,
                PlusOneSurname = plusOneSurname,
                PlusOneIsVegetarian = plusOneIsVegetarian,
                TableId = tableId,
                RoomId = roomId,
                Notes = notes
            };
        }

        /// <summary>
        /// Gets the guests that are associated with the invite with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite</param>
        /// <returns>Guests that are associated with the invite with the specified unique identifier</returns>
        internal static Guest[] GetGuestsByInviteId(SqlConnection conn, Int32 inviteId)
        {
            return Data.Guest.GetGuestsByInviteId(conn, inviteId);
        }

        /// <summary>
        /// Gets the guests that are assigned to the table with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="tableId">Unique identifier of the table</param>
        /// <returns>Guests that are assigned to the table with the specified unique identifier</returns>
        internal static Guest[] GetGuestsByTableId(SqlConnection conn, Int32 tableId)
        {
            return Data.Guest.GetGuestsByTableId(conn, tableId);
        }

        /// <summary>
        /// Gets the guests that are assigned to the room with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="roomId">Unique identifier of the room</param>
        /// <returns>Guests that are assigned to the room with the specified unique identifier</returns>
        internal static Guest[] GetGuestsByRoomId(SqlConnection conn, Int32 roomId)
        {
            return Data.Guest.GetGuestsByRoomId(conn, roomId);
        }

        /// <summary>
        /// Gets the guests that have not yet been assigned to a table
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>Guests that have not yet been assigned to a table</returns>
        public static Guest[] GetUnassignedTableGuests(SqlConnection conn)
        {
            return Data.Guest.GetUnassignedTableGuests(conn);
        }
        
        /// <summary>
        /// Gets the guests that have not yet been assigned to a room
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>Guests that have not yet been assigned to a room</returns>
        public static Guest[] GetUnassignedRoomGuests(SqlConnection conn)
        {
            return Data.Guest.GetUnassignedRoomGuests(conn);
        }

        /// <summary>
        /// Sets the table ID of the guest with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="guestId">Unique identifier of the guest</param>
        /// <param name="tableId">Unique identifier of the table</param>
        public static void SetGuestTableId(SqlConnection conn, Int32 guestId, Int32? tableId)
        {
            Data.Guest.SetGuestTableId(conn, guestId, tableId);
        }

        /// <summary>
        /// Sets the room ID of the guest with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="guestId">Unique identifier of the guest</param>
        /// <param name="roomId">Unique identifier of the room</param>
        public static void SetGuestRoomId(SqlConnection conn, Int32 guestId, Int32? roomId)
        {
            Data.Guest.SetGuestRoomId(conn, guestId, roomId);
        }

        #endregion
    }
}
