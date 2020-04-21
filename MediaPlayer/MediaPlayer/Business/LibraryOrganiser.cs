using System;
using System.Linq;
using System.Threading;
using MediaPlayer.Library.Business;
using System.Windows.Threading;
using Utilities.Business;
using MediaPlayer.EventArgs;

namespace MediaPlayer.Business
{
    public class LibraryOrganiser : NotifyPropertyChangedObject
    {
        #region Events

        /// <summary>
        /// Fires prior to a media item organising
        /// </summary>
        public event CancelMediaItemsOperationEventHandler OrganisingMediaItem;

        /// <summary>
        /// Fires when the organiser finishs organising
        /// </summary>
        public event EventHandler FinishedOrganising;

        #endregion
        
        #region Fields

        /// <summary>
        /// Gets or sets the thread used to asynchronously organise the library
        /// </summary>
        private Thread workerThread;

        /// <summary>
        /// Gets or sets the parts that the organiser will organise
        /// </summary>
        private readonly OrganisingMediaItemPart[] parts;

        /// <summary>
        /// Gets or sets the part currently being organised
        /// </summary>
        private OrganisingMediaItemPart selectedPart;
        
        /// <summary>
        /// Gets or sets a value determining whether or not the organising is currently organising media items
        /// </summary>
        private Boolean isOrganising;

        /// <summary>
        /// Gets or sets the number of parts remaining to be organised
        /// </summary>
        private Int32 organisedCount;

        /// <summary>
        /// Gets the total number of bytes that have been organised
        /// </summary>
        private Int64 totalBytesTransfered;

        /// <summary>
        /// Gets or sets the time the organiser started organising the library
        /// </summary>
        private DateTime timeStarted;

        /// <summary>
        /// Gets or sets the time that has passed since organisation of the parts began
        /// </summary>
        private TimeSpan timeTaken;

        /// <summary>
        /// Gets or sets the timer used to calculate the time take property
        /// </summary>
        private readonly DispatcherTimer timeTakenTimer = new DispatcherTimer();
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets the parts that the organiser will organise
        /// </summary>
        public OrganisingMediaItemPart[] Parts
        {
            get { return parts; }
        }

        /// <summary>
        /// Gets or sets the part currently being organised
        /// </summary>
        public OrganisingMediaItemPart SelectedPart
        {
            get { return selectedPart; }
            private set
            {
                selectedPart = value;
                OnPropertyChanged("SelectedPart");
            }
        }

        /// <summary>
        /// Get or sets the dispatcher this library organiser is associated with
        /// </summary>
        private Dispatcher Dispatcher { get; set; }

        /// <summary>
        /// Gets or sets a value determining whether or not the organising is currently organising media items
        /// </summary>
        public Boolean IsOrganising
        {
            get { return isOrganising; }
            private set
            {
                isOrganising = value;
                OnPropertyChanged("IsOrganising");
            }
        }

        /// <summary>
        /// Gets or sets the number of parts that have been organised
        /// </summary>
        private Int32 OrganisedCount
        {
            get { return organisedCount; }
            set
            {
                organisedCount = value;
                OnPropertyChanged("OrganisedCount");
                OnPropertyChanged("PartsToOrganise");
            }
        }

        /// <summary>
        /// Gets the number of parts left to be organised
        /// </summary>
        public Int32 PartsToOrganise
        {
            get { return parts.Length - OrganisedCount; }
        }

        /// <summary>
        /// Gets the total number of bytes of all media items being organised
        /// </summary>
        public Int64 TotalBytesToTransfer
        {
            get
            {
                Int64 totalBytesToTransfer = 0;

                foreach (OrganisingMediaItemPart part in Parts)
                    totalBytesToTransfer += part.Part.Size;

                return totalBytesToTransfer;
            }
        }

        /// <summary>
        /// Gets the total number of bytes that have been organised
        /// </summary>
        public Int64 TotalBytesTransfered
        {
            get { return totalBytesTransfered; }
            private set
            {
                totalBytesTransfered = value;
                OnPropertyChanged("TotalBytesTransfered");
                OnPropertyChanged("TotalBytesTransferedString");
                OnPropertyChanged("TotalBytesRemaining");
                OnPropertyChanged("TotalBytesRemainingString");
            }
        }

        /// <summary>
        /// Gets the total number of bytes that have been organised, expressed as a string
        /// </summary>
        public IntelligentString TotalBytesTransferedString
        {
            get { return IntelligentString.FormatSize(TotalBytesTransfered); }
        }

        /// <summary>
        /// Gets the total number of bytes left to organise
        /// </summary>
        public Int64 TotalBytesRemaining
        {
            get { return TotalBytesToTransfer - TotalBytesTransfered; }
        }

        /// <summary>
        /// Gets the total number of bytes left to organise, expressed as a string
        /// </summary>
        public IntelligentString TotalBytesRemainingString
        {
            get { return IntelligentString.FormatSize(TotalBytesRemaining); }
        }

        /// <summary>
        /// Gets or sets the time that has passed since organisation of the parts began
        /// </summary>
        public TimeSpan TimeTaken
        {
            get { return timeTaken; }
            set
            {
                timeTaken = value;
                OnPropertyChanged("TimeTaken");
                OnPropertyChanged("TimeTakenString");
                OnPropertyChanged("TimeRemaining");
                OnPropertyChanged("TimeRemainingString");
            }
        }

        /// <summary>
        /// Gets the time that has passed since organisation of the parts began, expressed as a string
        /// </summary>
        public IntelligentString TimeTakenString
        {
            get { return IntelligentString.FormatDuration(TimeTaken, false); }
        }

        /// <summary>
        /// Gets the estimated remaining time needed to organise the library
        /// </summary>
        public TimeSpan TimeRemaining
        {
            get
            {
                if (SelectedPart != null)
                {
                    Double transferRate = SelectedPart.TransferRate;

                    if (transferRate != 0)
                    {
                        return TimeSpan.FromSeconds(TotalBytesRemaining / transferRate);
                    }
                }

                return TimeSpan.FromSeconds(0);
            }
        }

        /// <summary>
        /// Gets the estimated remaining time needed to organise the library, expressed as a string
        /// </summary>
        public IntelligentString TimeRemainingString
        {
            get { return IntelligentString.FormatDuration(TimeRemaining, false); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the OrganisingMediaItemPart class
        /// </summary>
        /// <param name="parts">Parts that the organiser will organise</param>
        public LibraryOrganiser(OrganisingMediaItemPart[] parts, Dispatcher dispatcher)
        {
            this.parts = parts;
            this.selectedPart = null;
            this.Dispatcher = dispatcher;
            this.IsOrganising = false;
            this.OrganisedCount = 0;
            this.TotalBytesTransfered = 0;
            this.TimeTaken = TimeSpan.FromSeconds(0);

            this.timeTakenTimer.Interval = TimeSpan.FromSeconds(1);
            this.timeTakenTimer.Tick += timeTakenTimer_Tick;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the OrganisingMediaItem event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnOrganisingMediaItem(CancelMediaItemsOperationEventArgs e)
        {
            if (OrganisingMediaItem != null)
                GeneralMethods.ExecuteDelegateOnGuiThread(Dispatcher, () => OrganisingMediaItem(this, e));
        }

        /// <summary>
        /// Fires the FinishedOrganising event
        /// </summary>
        private void OnFinishedOrganising()
        {
            if (FinishedOrganising != null)
                GeneralMethods.ExecuteDelegateOnGuiThread(Dispatcher, () => FinishedOrganising(this, new System.EventArgs()));
        }

        /// <summary>
        /// Asynchronously organises the parts
        /// </summary>
        public void OrganiseAsync(OrganisingRootFolder[] rootFolders)
        {
            if (!IsOrganising)
            {
                IsOrganising = true;

                workerThread = new Thread(new ParameterizedThreadStart(Organise));
                workerThread.Start(rootFolders);
            }
        }

        /// <summary>
        /// Organises the parts
        /// </summary>
        private void Organise(object data)
        {
            try
            {
                timeStarted = DateTime.Now;
                timeTakenTimer.Start();

                OrganisedCount = 0;
                TotalBytesTransfered = 0;
                TimeTaken = TimeSpan.FromSeconds(0);
                
                OrganisingRootFolder[] rootFolders = data as OrganisingRootFolder[];

                //reset progress of parts
                foreach (OrganisingMediaItemPart part in Parts)
                    part.Progress = 0;

                foreach (OrganisingMediaItemPart part in Parts)
                {
                    OrganisingRootFolder[] sortedRootFolders = OrganisingRootFolder.SortForMediaItem(part.MediaItem, rootFolders);

                    SelectedPart = part;

                    CancelMediaItemsOperationEventArgs e = new CancelMediaItemsOperationEventArgs(new MediaItem[1] { part.MediaItem });
                    OnOrganisingMediaItem(e);

                    part.Organise(this, sortedRootFolders, e.Cancel, e.ReasonForCancel);

                    OrganisedCount++;
                }
            }
            catch (ThreadAbortException)
            {
                //do nothing
            }

            timeTakenTimer.Stop();
            SelectedPart = null;
            IsOrganising = false;
            OnFinishedOrganising();
        }

        /// <summary>
        /// Stops the organiser from organising media files
        /// </summary>
        public void StopOrganising()
        {
            if (IsOrganising)
            {
                workerThread.Abort();
                workerThread = null;

                timeTakenTimer.Stop();
                SelectedPart = null;
                IsOrganising = false;
                OrganisedCount = 0;
                TotalBytesTransfered = 0;
                TimeTaken = TimeSpan.FromSeconds(0);
            }
        }

        /// <summary>
        /// Sets the value of the TotalBytesTransfered property
        /// </summary>
        public void SetTotalBytesTransfered()
        {
            TotalBytesTransfered = 0;

            foreach (OrganisingMediaItemPart part in Parts)
                TotalBytesTransfered += part.Progress;
        }

        #endregion

        #region Event Handlers

        private void timeTakenTimer_Tick(object sender, System.EventArgs e)
        {
            TimeTaken = DateTime.Now - timeStarted;
        }

        #endregion
    }
}
