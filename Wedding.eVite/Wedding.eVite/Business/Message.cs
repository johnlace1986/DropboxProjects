using System;
using System.Data;
using System.Data.SqlClient;
using Utilities.Data;
using Utilities.Data.SQL;
using sp = Utilities.Data.StoredProcedure;

namespace Wedding.eVite.Business
{
    [DbObjectMetaData("MessageId", DbType.Int32)]
    public class Message : SqlDbObject<Int32>, IComparable<Message>
    {
        #region Fields
        
        /// <summary>
        /// Gets or sets the unique identifier of the invite the message belongs to
        /// </summary>
        private Int32 inviteId;

        /// <summary>
        /// Gets or sets the body of the message
        /// </summary>
        private String body;

        /// <summary>
        /// Gets or sets the date and time the message was sent
        /// </summary>
        private DateTime dateSent;

        /// <summary>
        /// Gets or sets the sender of the message
        /// </summary>
        private MessageSender sender;

        /// <summary>
        /// Gets or sets a value determining whether or not the message has been read by the recipient
        /// </summary>
        private Boolean read;

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier of the invite the message belongs to
        /// </summary>
        [sp.DataColumn("InviteId", DbType.Int32)]
        public Int32 InviteId
        {
            get { return inviteId; }
            set
            {
                inviteId = value;
                OnPropertyChanged("InviteId");
            }
        }

        /// <summary>
        /// Gets or sets the body of the message
        /// </summary>
        [sp.DataColumn("Body", DbType.String)]
        public String Body
        {
            get { return body; }
            set
            {
                body = value;
                OnPropertyChanged("Body");
            }
        }

        /// <summary>
        /// Gets or sets the date and time the message was sent
        /// </summary>
        [sp.DataColumn("DateSent", DbType.DateTime)]
        public DateTime DateSent
        {
            get { return dateSent; }
            set
            {
                dateSent = value;
                OnPropertyChanged("DateSent");
            }
        }

        /// <summary>
        /// Gets or sets the sender of the message
        /// </summary>
        [sp.DataColumn("Sender", DbType.Int32)]
        public MessageSender Sender
        {
            get { return sender; }
            set
            {
                sender = value;
                OnPropertyChanged("Sender");
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the message has been read by the recipient
        /// </summary>
        [sp.DataColumn("Read", DbType.Boolean)]
        public Boolean Read
        {
            get { return read; }
            set
            {
                read = value;
                OnPropertyChanged("Read");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Message class
        /// </summary>
        public Message()
            : base()
        {
            Id = -1;
            InviteId = -1;
            Body = String.Empty;
            DateSent = DateTime.Now;
            Sender = MessageSender.Invite;
            Read = false;
        }

        #endregion

        #region Static Methods
        
        /// <summary>
        /// Loads the messages that belong to the invite with the specified unique identifier from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite</param>
        /// <returns>Messages that belong to the invite with the specified unique identifier from the database</returns>
        public static Message[] GetMessagesByInviteId(SqlConnection conn, Int32 inviteId)
        {
            return Data.Message.GetMessagesByInviteId(conn, inviteId);
        }

        /// <summary>
        /// Loads the unread messages that belong to the invite with the specified unique identifier from the database
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite</param>
        /// <param name="isRequesterAdmin">Value determining whether or not the invite requesting the messages is an admin user</param>
        /// <returns>Unread messages that belong to the invite with the specified unique identifier from the database</returns>
        public static Message[] GetUnreadMessagesByInviteId(SqlConnection conn, Int32 inviteId, Boolean isRequesterAdmin)
        {
            return Data.Message.GetUnreadMessagesByInviteId(conn, inviteId, isRequesterAdmin);
        }

        /// <summary>
        /// Gets the number of unread messages that belong to the invite with the specified unique identifier
        /// </summary>
        /// <param name="conn">Open connection to the database</param>
        /// <param name="inviteId">Unique identifier of the invite</param>
        /// <param name="isRequesterAdmin">Value determining whether or not the invite requesting the messages is an admin user</param>
        /// <returns>Number of unread messages that belong to the invite with the specified unique identifier</returns>
        public static Int32 GetInviteUnreadMessageCount(SqlConnection conn, Int32 inviteId, Boolean isRequesterAdmin)
        {
            return Data.Message.GetInviteUnreadMessageCount(conn, inviteId, isRequesterAdmin);
        }

        #endregion

        #region SqlDbObject Members
        
        protected override void AddToDatabase(SqlConnection conn)
        {
            AddToDatabase(conn, "AddMessage");
        }

        protected override void UpdateInDatabase(SqlConnection conn)
        {
            UpdateInDatabase(conn, "UpdateMessage");
        }

        protected override void DeleteFromDatabase(SqlConnection conn)
        {
            DeleteFromDatabase(conn, "DeleteMessage");
        }

        protected override void ResetProperties()
        {
            Id = -1;
        }

        #endregion

        #region IComparable<Message> Members

        public int CompareTo(Message other)
        {
            return DateSent.CompareTo(other.DateSent);
        }

        #endregion
    }
}
