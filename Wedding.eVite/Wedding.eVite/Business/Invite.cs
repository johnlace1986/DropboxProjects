using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;
using Utilities.Business;
using Utilities.Data;
using Utilities.Data.SQL;
using sp = Utilities.Data.StoredProcedure;

namespace Wedding.eVite.Business
{
    [DbObjectMetaData("InviteId", DbType.Int32)]
    [Serializable]
    public class Invite : SqlDbObject<Int32>, IComparable<Invite>
    {
        #region Fields
        
        /// <summary>
        /// Gets or sets the email address the invite was sent to
        /// </summary>
        private String emailAddress;

        /// <summary>
        /// Gets or sets the password for the invite
        /// </summary>
        private String password;

        /// <summary>
        /// Gets or sets a value determining whether or not the password has been changed from the one randmoly generated when the invite was created
        /// </summary>
        private Boolean hasChangedPassword;

        /// <summary>
        /// Gets or sets a value determining whether or not the invite is for an admin user
        /// </summary>
        private Boolean isAdmin;

        /// <summary>
        /// Gets or sets a value determining whether or not the invite included the ceremony
        /// </summary>
        private Boolean includesCeremony;

        /// <summary>
        /// Gets or sets a value determining whether or not the guests in the invite would like to reserve a room at Sandhole
        /// </summary>
        private Boolean reserveSandholeRoom;

        /// <summary>
        /// Gets or sets a value determining whether or not messages sent to this invite should also be emailed
        /// </summary>
        private Boolean emailMessages;

        /// <summary>
        /// Gets or sets a value determining whether or not an email should be sent to the email address when the gift website is created
        /// </summary>
        private Boolean notifyGiftWebsite;

        /// <summary>
        /// Gets or sets the extra guests included in the invite
        /// </summary>
        private ObservableCollection<Guest> guests;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the email address the invite was sent to
        /// </summary>
        [sp.DataColumn("EmailAddress", DbType.String)]
        public String EmailAddress
        {
            get { return emailAddress; }
            set
            {
                emailAddress = value;
                OnPropertyChanged("EmailAddress");
            }
        }

        /// <summary>
        /// Gets or sets the password for the invite
        /// </summary>
        [sp.DataColumn("Password", DbType.String)]
        public String Password
        {
            get { return password; }
            private set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the password has been changed from the one randmoly generated when the invite was created
        /// </summary>
        [sp.DataColumn("HasChangedPassword", DbType.Boolean)]
        public Boolean HasChangedPassword
        {
            get { return hasChangedPassword; }
            private set
            {
                hasChangedPassword = value;
                OnPropertyChanged("HasChangedPassword");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the invite is for an admin user
        /// </summary>
        [sp.DataColumn("IsAdmin", DbType.Boolean)]
        public Boolean IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the invite included the ceremony
        /// </summary>
        [sp.DataColumn("IncludesCeremony", DbType.Boolean)]
        public Boolean IncludesCeremony
        {
            get { return includesCeremony; }
            set
            {
                includesCeremony = value;
                OnPropertyChanged("IncludesCeremony");
            }
        }
        
        /// <summary>
        /// Gets or sets a value determining whether or not the guests in the invite would like to reserve a room at Sandhole
        /// </summary>
        [sp.DataColumn("ReserveSandholeRoom", DbType.Boolean)]
        public Boolean ReserveSandholeRoom
        {
            get { return reserveSandholeRoom; }
            set
            {
                reserveSandholeRoom = value;
                OnPropertyChanged("ReserveSandholeRoom");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not messages sent to this invite should also be emailed
        /// </summary>
        [sp.DataColumn("EmailMessages", DbType.Boolean)]
        public Boolean EmailMessages
        {
            get { return emailMessages; }
            set
            {
                emailMessages = value;
                OnPropertyChanged("EmailMessages");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not an email should be sent to the email address when the gift website is created
        /// </summary>
        [sp.DataColumn("NotifyGiftWebsite", DbType.Boolean)]
        public Boolean NotifyGiftWebsite
        {
            get { return notifyGiftWebsite; }
            set
            {
                notifyGiftWebsite = value;
                OnPropertyChanged("NotifyGiftWebsite");
            }
        }

        /// <summary>
        /// Gets or sets the guests in the invite
        /// </summary>
        public ObservableCollection<Guest> Guests
        {
            get { return guests; }
            private set
            {
                guests = value;
                OnPropertyChanged("Guests");
            }
        }

        /// <summary>
        /// Gets the list of guests in the invite
        /// </summary>
        public String GuestList
        {
            get
            {
                String guestList = String.Empty;

                if (Guests.Count > 0)
                {
                    //group guests by surname
                    Dictionary<String, IEnumerable<Guest>> surnameGroups = new Dictionary<String, IEnumerable<Guest>>();

                    foreach (Guest guest in Guests)
                    {
                        if (!surnameGroups.Keys.Contains(guest.Surname))
                            surnameGroups.Add(guest.Surname, new List<Guest>());

                        surnameGroups[guest.Surname] = surnameGroups[guest.Surname].Concat(new Guest[] { guest });
                    }

                    guestList = GeneralMethods.GroupItems<KeyValuePair<String, IEnumerable<Guest>>>(surnameGroups, surnameGroup => Guest.GroupGuestsBySurname(surnameGroup.Value), ", ", " and ");
                }

                return guestList;
            }
        }

        /// <summary>
        /// Gets the list of forenames of guests in the invite
        /// </summary>
        public String GuestListForenames
        {
            get
            {
                return GeneralMethods.GroupItems<Guest>(Guests, guest => guest.Forename, ", ", " and ");
            }
        }

        /// <summary>
        /// Gets a value determining whether or not all guests in the invite have RSVP'd
        /// </summary>
        public Boolean RSVP
        {
            get { return Guests.All(p => p.RSVP); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Invite class
        /// </summary>
        public Invite()
            : base()
        {
            Id = -1;
            EmailAddress = String.Empty;
            IsAdmin = false;
            IncludesCeremony = false;
            ReserveSandholeRoom = false;
            EmailMessages = true;
            NotifyGiftWebsite = false;
            Guests = new ObservableCollection<Guest>();

            ResetPassword();
        }

        /// <summary>
        /// Initialises a new instance of the Invite class
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite</param>
        public Invite(SqlConnection conn, Int32 inviteId)
            : base(conn, inviteId)
        {
        }

        #endregion

        #region Instance Methods
        
        /// <summary>
        /// Resets the password
        /// </summary>
        public void ResetPassword()
        {
            Password = GeneratePassword();
            HasChangedPassword = false;
        }

        /// <summary>
        /// Checks the specified password matches the password of the invite
        /// </summary>
        /// <param name="password">Password to check</param>
        /// <returns>True if the specified password matches the password of the invite, false if not</returns>
        public Boolean CheckPassword(String password)
        {
            if (String.IsNullOrEmpty(password))
                return false;

            if (HasChangedPassword)
                password = GeneralMethods.HashString(password);

            return password == Password;
        }

        /// <summary>
        /// Changes the user's password in the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="password">New password for the user</param>
        public void ChangePassword(SqlConnection conn, String password)
        {
            Password = GeneralMethods.HashString(password);
            HasChangedPassword = true;

            try
            {
                Save(conn);
            }
            catch
            {
                HasChangedPassword = false;
                throw;
            }
        }

        /// <summary>
        /// Gets the number of unread messages that belong to the invite
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="isRequesterAdmin">Value determining whether or not the invite requesting the messages is an admin user</param>
        /// <returns>Number of unread messages that belong to the invite</returns>
        public Int32 GetUnreadMessagesCount(SqlConnection conn, Boolean isRequesterAdmin)
        {
            return Message.GetInviteUnreadMessageCount(conn, Id, isRequesterAdmin);
        }

        /// <summary>
        /// Creates an XML element containing the data for the invite
        /// </summary>
        /// <param name="document">XML document the element will be added to</param>
        /// <returns>XML element containing the data for the invite</returns>
        public XmlElement ToXml(XmlDocument document)
        {
            XmlElement invite = document.CreateElement("INVITE");

            XmlAttribute id = document.CreateAttribute("Id");
            id.Value = Id.ToString();
            invite.Attributes.Append(id);

            XmlAttribute emailAddress = document.CreateAttribute("EmailAddress");
            emailAddress.Value = EmailAddress;
            invite.Attributes.Append(emailAddress);

            XmlAttribute isAdmin = document.CreateAttribute("IsAdmin");
            isAdmin.Value = IsAdmin.ToString();
            invite.Attributes.Append(isAdmin);

            XmlAttribute includesCeremony = document.CreateAttribute("IncludesCeremony");
            includesCeremony.Value = IncludesCeremony.ToString();
            invite.Attributes.Append(includesCeremony);

            XmlAttribute reserveSandholeRoom = document.CreateAttribute("ReserveSandholeRoom");
            reserveSandholeRoom.Value = ReserveSandholeRoom.ToString();
            invite.Attributes.Append(reserveSandholeRoom);

            XmlAttribute emailMessages = document.CreateAttribute("EmailMessages");
            emailMessages.Value = EmailMessages.ToString();
            invite.Attributes.Append(emailMessages);

            XmlAttribute notifyGiftWebsite = document.CreateAttribute("NotifyGiftWebsite");
            notifyGiftWebsite.Value = NotifyGiftWebsite.ToString();
            invite.Attributes.Append(notifyGiftWebsite);

            XmlElement guests = document.CreateElement("GUESTS");
            invite.AppendChild(guests);

            foreach (Guest guest in Guests)
            {
                guests.AppendChild(guest.ToXml(document));
            }

            return invite;
        }

        #endregion

        #region Static Methods
        
        /// <summary>
        /// Gets all invites currently in the system
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <returns>All invites currently in the system</returns>
        public static Invite[] GetInvites(SqlConnection conn)
        {
            return Data.Invite.GetInvites(conn);
        }

        /// <summary>
        /// Generates a password for the invite
        /// </summary>
        /// <returns>Password generated for the invite</returns>
        private static String GeneratePassword()
        {
            Random rnd = new Random();
            String password = String.Empty;

            for (Int32 i = 0; i < 10; i++)
            {
                password += (Char)rnd.Next(65, 90);
            }

            return password;
        }

        #endregion

        #region SqlDbObject Members

        protected override void Load(SqlConnection conn, Int32 id)
        {
            Invite invite = Data.Invite.GetInviteById(conn, id);

            Id = invite.Id;
            EmailAddress = invite.EmailAddress;
            Password = invite.Password;
            HasChangedPassword = invite.HasChangedPassword;
            IsAdmin = invite.IsAdmin;
            IncludesCeremony = invite.IncludesCeremony;
            ReserveSandholeRoom = invite.ReserveSandholeRoom;
            EmailMessages = invite.EmailMessages;
            NotifyGiftWebsite = invite.NotifyGiftWebsite;
            Guests = invite.Guests;
        }
        
        protected override void Load(SqlConnection conn, sp.DataRow row)
        {
            base.Load(conn, row);
            Guests = new ObservableCollection<Guest>(Guest.GetGuestsByInviteId(conn, Id));
        }

        protected override void AddToDatabase(SqlConnection conn)
        {
            AddToDatabase(conn, "AddInvite");

            foreach (Guest guest in Guests)
                guest.Add(conn, Id);
        }

        protected override void UpdateInDatabase(SqlConnection conn)
        {
            UpdateInDatabase(conn, "UpdateInvite");
            
            GeneralMethods.SyncCollections<Guest>(
                Guest.GetGuestsByInviteId(conn, Id),
                Guests,
                (oldGuest, newGuest) => oldGuest.Id == newGuest.Id,
                guest => guest.Add(conn, Id),
                (oldGuest, newGuest) => newGuest.Update(conn, Id),
                guest => guest.Delete(conn));
        }

        protected override void DeleteFromDatabase(SqlConnection conn)
        {
            DeleteFromDatabase(conn, "DeleteInvite");
        }

        protected override void ResetProperties()
        {
            Id = -1;
        }

        #endregion

        #region IComparable<Invite> Members

        public int CompareTo(Invite other)
        {
            return EmailAddress.CompareTo(other.EmailAddress);
        }

        #endregion
    }
}
