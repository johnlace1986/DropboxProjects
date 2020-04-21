using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Business;
using MediaPlayer.Library.Business;
using System.IO;
using System.Threading;

namespace MediaPlayer.Business
{
    public class OrganisingMediaItemPart : NotifyPropertyChangedObject
    {
        #region Fields

        /// <summary>
        /// Gets or sets the media item the part being organised belongs to
        /// </summary>
        private readonly MediaItem mediaItem;

        /// <summary>
        /// Gets or sets the part being organised
        /// </summary>
        private readonly MediaItemPart part;

        /// <summary>
        /// Gets or sets the path the part will be moved to
        /// </summary>
        private readonly IntelligentString organisedPath;

        /// <summary>
        /// Gets or sets the number of bytes that have currently been copied
        /// </summary>
        private Int64 progress;

        /// <summary>
        /// Gets or sets the status of the part
        /// </summary>
        private OrganisingMediaItemPartStatus status;

        /// <summary>
        /// Gets or sets the errors that occurred while attempting to organise the part
        /// </summary>
        private Dictionary<IntelligentString, System.Exception> errors;

        /// <summary>
        /// Gets or sets the date and time the part began organising
        /// </summary>
        private DateTime dateTimeStarted;

        /// <summary>
        /// Gets or sets the length of time taken to organise the part
        /// </summary>
        private TimeSpan timeTaken;

        /// <summary>
        /// Gets or sets a value determining whether the part needs to be moved
        /// </summary>
        private readonly Boolean requiresMove;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the library organiser that is organising the part
        /// </summary>
        private LibraryOrganiser LibraryOrganiser { get; set; }

        /// <summary>
        /// Gets the media item the part being organised belongs to
        /// </summary>
        public MediaItem MediaItem
        {
            get { return mediaItem; }
        }

        /// <summary>
        /// Gets the part being organised
        /// </summary>
        public MediaItemPart Part
        {
            get { return part; }
        }

        /// <summary>
        /// Gets the index of the part being organised, expressed as a string
        /// </summary>
        public IntelligentString IndexString
        {
            get
            {
                return ((Part.Index + 1).ToString() + " of " + MediaItem.Parts.Count.ToString());
            }
        }

        /// <summary>
        /// Gets the path the part will be moved to
        /// </summary>
        public IntelligentString OrganisedPath
        {
            get { return organisedPath; }
        }

        /// <summary>
        /// Gets or sets the number of bytes that have currently been copied
        /// </summary>
        public Int64 Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
                OnPropertyChanged("TimeRemaining");
                OnPropertyChanged("TransferRate");
                OnPropertyChanged("TransferRateString");

                if (LibraryOrganiser != null)
                    LibraryOrganiser.SetTotalBytesTransfered();
            }
        }

        /// <summary>
        /// Gets or sets the status of the part
        /// </summary>
        public OrganisingMediaItemPartStatus Status
        {
            get { return status; }
            private set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Gets or sets the errors that occurred while attempting to organise the part
        /// </summary>
        public Dictionary<IntelligentString, System.Exception> Errors
        {
            get { return errors; }
            private set
            {
                errors = value;
                OnPropertyChanged("Errors");
                OnPropertyChanged("ErrorCount");
            }
        }

        /// <summary>
        /// Gets the number of errors that occurred during organisation of the part
        /// </summary>
        public int ErrorCount
        {
            get { return Errors.Count; }
        }

        /// <summary>
        /// Gets or sets the length of time taken to organise the part
        /// </summary>
        public TimeSpan TimeTaken
        {
            get { return timeTaken; }
            private set
            {
                timeTaken = value;
                OnPropertyChanged("TimeTaken");
                OnPropertyChanged("TimeRemaining");
                OnPropertyChanged("TransferRate");
                OnPropertyChanged("TransferRateString");
            }
        }

        /// <summary>
        /// Gets the transfer rate of the organisation of the part in bytes per second
        /// </summary>
        public Double TransferRate
        {
            get
            {
                if (Progress == 0)
                    return 0;

                if (TimeTaken == TimeSpan.FromSeconds(0))
                    return 0;

                return Progress / TimeTaken.TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the transfer rate of the organisation of the part in bytes per second, expressed as a string
        /// </summary>
        public IntelligentString TransferRateString
        {
            get
            {
                Double transferRate = TransferRate;

                if (transferRate != 0)
                {
                    Int32 denominationLevel;
                    Boolean negative;

                    IntelligentString.GetSizeDenomination(ref transferRate, out denominationLevel, out negative);

                    IntelligentString transferRateString = transferRate.ToString("0.00") + " (" + IntelligentString.DenominationLevels[denominationLevel] + "/s)";

                    if (negative)
                        transferRateString = "-" + transferRateString;

                    return transferRateString;
                }

                return IntelligentString.Empty;
            }
        }

        /// <summary>
        /// Gets the ammount of time it will take to organise the remaining bytes at the current transfer rate
        /// </summary>
        public TimeSpan TimeRemaining
        {
            get
            {
                Double transferRate = TransferRate;

                if (transferRate != 0)
                {
                    Int64 bytesRemaining = Part.Size - Progress;

                    return TimeSpan.FromSeconds(bytesRemaining / transferRate);
                }

                return TimeSpan.FromSeconds(0);
            }
        }

        /// <summary>
        /// Gets or sets a value determining whether the part needs to be moved
        /// </summary>
        public Boolean RequiresMove
        {
            get { return requiresMove; }
        }
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the OrganisingMediaItemPart class
        /// </summary>
        /// <param name="mediaItem">Media item the part being organised belongs to</param>
        /// <param name="partIndex">Index in the media item's collection of the part being organised</param>
        /// <param name="organisedPath">Path the part will be moved to</param>
        /// <param name="requiresMove">Value determining whether the part needs to be moved</param>
        public OrganisingMediaItemPart(MediaItem mediaItem, int partIndex, IntelligentString organisedPath, Boolean requiresMove)
        {
            this.mediaItem = mediaItem;
            this.part = mediaItem.Parts[partIndex];
            this.organisedPath = organisedPath;
            this.progress = 0;
            this.status = OrganisingMediaItemPartStatus.Waiting;
            this.errors = new Dictionary<IntelligentString, Exception>();
            this.timeTaken = TimeSpan.FromSeconds(0);
            this.requiresMove = requiresMove;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Organises the part
        /// </summary>
        /// <param name="libraryOrganiser">Library organiser that is organising the part</param>
        /// <param name="rootFolders">Root folders used to organise the part</param>
        /// <param name="cancelled">Value determining whether the orgnisation of the part has been cancelled</param>
        /// <param name="reasonForCancel">Description of the reason orgnisation or the part was cancelled</param>
        public void Organise(LibraryOrganiser libraryOrganiser, OrganisingRootFolder[] rootFolders, Boolean cancelled, String reasonForCancel)
        {
            LibraryOrganiser = libraryOrganiser;
            Status = OrganisingMediaItemPartStatus.Organising;
            
            Errors.Clear();
            
            try
            {
                if (cancelled)
                    throw new System.Exception(reasonForCancel);

                if (!File.Exists(part.Location.Value))
                    throw new FileNotFoundException("The media item part was not found", part.Location.Value);

                List<OrganisingRootFolder> typedRootFolders = new List<OrganisingRootFolder>(rootFolders.Where(p => p.MediaItemType == MediaItem.Type));

                if (typedRootFolders.Count == 0)
                    throw new ArgumentException("No root folders specified");

                foreach (OrganisingRootFolder rootFolder in typedRootFolders)
                {
                    try
                    {
                        Organise(rootFolder);

                        Status = OrganisingMediaItemPartStatus.Organised;
                        return;
                    }
                    catch (ThreadAbortException)
                    {
                        throw;
                    }
                    catch (System.Exception e)
                    {
                        LogOrganisationError(rootFolder.Path, e);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Progress = 0;
                Status = OrganisingMediaItemPartStatus.Waiting;

                throw;
            }
            catch (System.Exception e)
            {
                LogOrganisationError(null, e);
            }

            Status = OrganisingMediaItemPartStatus.Error;
        }

        /// <summary>
        /// Moves the media item part to the specified root folder
        /// </summary>
        /// <param name="rootFolder">Root folder to move the media item part to</param>
        private void Organise(OrganisingRootFolder rootFolder)
        {
            Progress = 0;

            if (RequiresMove)
            {
                FileInfo source = new FileInfo(Part.Location.Value);

                DirectoryInfo di = new DirectoryInfo(rootFolder.Path.Value);

                if (!di.Exists)
                    throw new DirectoryNotFoundException("Root folder \"" + di.FullName + "\" does not exist");

                String destinationFilename = di.FullName;

                if (!destinationFilename.EndsWith("\\"))
                    destinationFilename += "\\";

                destinationFilename += OrganisedPath;

                FileInfo destination = new FileInfo(destinationFilename);

                if (destination.Exists)
                    throw new System.Exception("\"" + destinationFilename + "\" already exists");

                if (!destination.Directory.Exists)
                    destination.Directory.Create();

                MovePart(source, destinationFilename);

                Part.Location = destinationFilename;
                MediaItem.Save();
            }
            else
                Progress = Part.Size;

            if (MediaItem.IsHidden)
                File.SetAttributes(Part.Location.Value, FileAttributes.Hidden);
            else
                File.SetAttributes(Part.Location.Value, FileAttributes.Normal);                
        }

        /// <summary>
        /// Moves the part's physical file
        /// </summary>
        /// <param name="source">FileInfo object representing the physical file</param>
        /// <param name="destinationFilename">Filename to move the part to</param>
        private void MovePart(FileInfo source, String destinationFilename)
        {
            try
            {
                dateTimeStarted = DateTime.Now;

                if (destinationFilename.ToLower().StartsWith(source.Directory.Root.FullName.ToLower()))
                {
                    File.Move(source.FullName, destinationFilename);
                    Progress = Part.Size;
                    TimeTaken = DateTime.Now - dateTimeStarted;
                }
                else
                {
                    const int refreshRate = 400;

                    //file stream to read the current video file
                    using (FileStream sourceStream = source.Open(FileMode.Open, FileAccess.Read))
                    {
                        //file stream to write the new video file
                        using (FileStream destinationStream = new FileStream(destinationFilename, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            const int bufferSize = 8192;
                            byte[] buffer = new byte[bufferSize];

                            int cycle = 0;
                            long progress = Progress;

                            while (progress < Part.Size)
                            {
                                //read a set of bytes from the source
                                sourceStream.Read(buffer, 0, bufferSize);

                                //write a set of bytes to the destination
                                destinationStream.Write(buffer, 0, bufferSize);

                                //update the progress property of the video object
                                if (progress < (Part.Size - bufferSize))
                                    progress += bufferSize;
                                else
                                    progress = Part.Size;

                                if ((cycle % refreshRate) == 0)
                                {
                                    Progress = progress;
                                    TimeTaken = DateTime.Now - dateTimeStarted;
                                }

                                cycle++;
                            }

                            Progress = Part.Size;
                        }
                    }

                    source.Delete();
                }
            }
            catch (System.Exception e)
            {
                if (File.Exists(destinationFilename))
                    File.Delete(destinationFilename);

                throw e;
            }
        }

        /// <summary>
        /// Logs and error that occurred during organisation
        /// </summary>
        /// <param name="rootFolder">The root folder the part was being organised into when the error occured</param>
        /// <param name="e">Error that occured during organisation</param>
        private void LogOrganisationError(IntelligentString rootFolder, System.Exception e)
        {
            Progress = Part.Size;

            Errors.Add(rootFolder, e);
            OnPropertyChanged("Errors");
        }

        #endregion
    }
}
