using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MediaPlayer.EventArgs;
using MediaPlayer.Library.Business;
using System.ComponentModel;
using MediaPlayer.Presentation.Windows;
using Utilities.Business;
using MediaPlayer.ValueConverters;
using System.Globalization;
using Utilities.Exception;

namespace MediaPlayer.Presentation.UserControls.Pages
{
    /// <summary>
    /// Interaction logic for RootFoldersView.xaml
    /// </summary>
    public partial class RootFoldersView : UserControl
    {
        #region Events

        /// <summary>
        /// Fires when the path to a saved root folder changes
        /// </summary>
        public event RootFolderPathChangedEventHandler PathChanged;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(String), typeof(RootFoldersView));

        public static readonly DependencyProperty AssociatedTypeProperty = DependencyProperty.Register("AssociatedType", typeof(MediaItemTypeEnum), typeof(RootFoldersView));

        public static readonly DependencyProperty RootFoldersProperty = DependencyProperty.Register("RootFolders", typeof(RootFolder[]), typeof(RootFoldersView));

        public static readonly DependencyProperty IsOrganisingProperty = DependencyProperty.Register("IsOrganising", typeof(Boolean), typeof(RootFoldersView));

        public static readonly DependencyProperty ShowHiddenProperty = DependencyProperty.Register("ShowHidden", typeof(Boolean?), typeof(RootFoldersView));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the view's header
        /// </summary>
        public String Header
        {
            get { return (String)GetValue(RootFoldersView.HeaderProperty); }
            set { SetValue(RootFoldersView.HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of media item the root folders in the view are associated with
        /// </summary>
        public MediaItemTypeEnum AssociatedType
        {
            get { return (MediaItemTypeEnum)GetValue(RootFoldersView.AssociatedTypeProperty); }
            set { SetValue(RootFoldersView.AssociatedTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the root folders displayed in the view
        /// </summary>
        public RootFolder[] RootFolders
        {
            get { return GetValue(RootFoldersView.RootFoldersProperty) as RootFolder[]; }
            set { SetValue(RootFoldersView.RootFoldersProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not the library is currently being organised
        /// </summary>
        public Boolean IsOrganising
        {
            get { return (Boolean)GetValue(RootFoldersView.IsOrganisingProperty); }
            set { SetValue(RootFoldersView.IsOrganisingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value determining whether or not hidden media items should be displayed
        /// </summary>
        public Boolean? ShowHidden
        {
            get { return (Boolean?)GetValue(ShowHiddenProperty); }
            set { SetValue(ShowHiddenProperty, value); }
        }

        /// <summary>
        /// Gets the current sort descriptions in the view
        /// </summary>
        public SortDescriptionCollection SortDescriptions
        {
            get { return dgRootFolders.Items.SortDescriptions; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the RootFoldersView class
        /// </summary>
        public RootFoldersView()
        {
            InitializeComponent();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fires the PathChanged event
        /// </summary>
        /// <param name="e">Arguments to pass to the event</param>
        private void OnPathChanged(RootFolderPathChangedEventArgs e)
        {
            if (PathChanged != null)
                PathChanged(this, e);
        }

        /// <summary>
        /// Gets the tags available to root folders displayed in the view
        /// </summary>
        /// <returns>Tags available to root folders displayed in the view</returns>
        private IntelligentString[] GetTags()
        {
            IntelligentString[] tags;

            switch (AssociatedType)
            {
                case MediaItemTypeEnum.Video:

                    VideoTagsConverter vtc = new VideoTagsConverter();
                    tags = (IntelligentString[])vtc.Convert(ShowHidden, typeof(IntelligentString[]), null, CultureInfo.CurrentCulture);

                    break;

                case MediaItemTypeEnum.Song:
                    SongTagsConverter stc = new SongTagsConverter();
                    tags = (IntelligentString[])stc.Convert(ShowHidden, typeof(IntelligentString[]), null, CultureInfo.CurrentCulture);

                    break;

                default:
                    throw new UnknownEnumValueException(AssociatedType);
            }

            return tags;
        }

        /// <summary>
        /// Adds a new root folder to the system
        /// </summary>
        private void AddRootFolder()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before adding any root folders");
                    return;
                }

                RootFolderDialog rootFolderDialog = new RootFolderDialog(GetTags(), AssociatedType);
                rootFolderDialog.Owner = Application.Current.MainWindow;
                rootFolderDialog.PathChanged += rootFolderDialog_PathChanged;

                if (GeneralMethods.GetNullableBoolValue(rootFolderDialog.ShowDialog()))
                {
                    List<RootFolder> rootFolders = new List<RootFolder>(RootFolders);
                    rootFolders.Add(rootFolderDialog.SelectedRootFolder);
                    rootFolders.Sort();

                    List<SortDescription> sortDescriptions = new List<SortDescription>(SortDescriptions);

                    RootFolders = rootFolders.ToArray();

                    SortDescriptions.Clear();

                    foreach (SortDescription sortDescription in sortDescriptions)
                        SortDescriptions.Add(sortDescription);

                    dgRootFolders.SelectedItem = rootFolderDialog.SelectedRootFolder;
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not add root folder: ");
            }
        }
        
        /// <summary>
        /// Edits the selected root folder
        /// </summary>
        private void EditSelectedRootFolder()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before editing any root folders");
                    return;
                }

                RootFolder selectedRootFolder = dgRootFolders.SelectedItem as RootFolder;

                if (selectedRootFolder == null)
                {
                    GeneralMethods.MessageBoxApplicationError("Please select a root folder to edit");
                    return;
                }

                RootFolderDialog rootFolderDialog = new RootFolderDialog(GetTags(), selectedRootFolder.Clone() as RootFolder);
                rootFolderDialog.Owner = Application.Current.MainWindow;
                rootFolderDialog.PathChanged += rootFolderDialog_PathChanged;

                if (GeneralMethods.GetNullableBoolValue(rootFolderDialog.ShowDialog()))
                {
                    List<RootFolder> rootFolders = new List<RootFolder>(RootFolders);
                    rootFolders.Remove(selectedRootFolder);
                    rootFolders.Add(rootFolderDialog.SelectedRootFolder);

                    List<SortDescription> sortDescriptions = new List<SortDescription>(SortDescriptions);

                    RootFolders = rootFolders.ToArray();

                    SortDescriptions.Clear();

                    foreach (SortDescription sortDescription in sortDescriptions)
                        SortDescriptions.Add(sortDescription);

                    dgRootFolders.SelectedItem = rootFolderDialog.SelectedRootFolder;
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not edit root folder: ");
            }
        }

        /// <summary>
        /// Deletes the selected root folders from the system
        /// </summary>
        private void DeleteSelectedRootFolders()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before deleting any root folders");
                    return;
                }

                List<RootFolder> selectedRootFolders = new List<RootFolder>();

                foreach (RootFolder fileType in dgRootFolders.SelectedItems)
                    selectedRootFolders.Add(fileType);

                MessageBoxResult result;

                switch (selectedRootFolders.Count)
                {
                    case 0:
                        GeneralMethods.MessageBoxApplicationError("Please select 1 or more root folders to delete");
                        return;

                    case 1:
                        result = GeneralMethods.MessageBox("Are you sure you want to delete " + selectedRootFolders[0].Path + "?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        break;

                    default:
                        result = GeneralMethods.MessageBox("Are you sure you want to delete these root folders?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        break;
                }

                if (result == MessageBoxResult.Yes)
                    DeleteRootFolders(selectedRootFolders.ToArray());
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not delete root folders: ");
            }
        }

        /// <summary>
        /// Removes the specified root folders from the system
        /// </summary>
        /// <param name="rootFolder">Root folders being removed from the system</param>
        private void DeleteRootFolders(RootFolder[] toDelete)
        {
            try
            {
                List<RootFolder> rootFolders = new List<RootFolder>(RootFolders);
                List<RootFolder> lstToDelete = new List<RootFolder>(toDelete);

                while (lstToDelete.Count > 0)
                {
                    RootFolder rootFolder = lstToDelete[0];
                    rootFolder.Delete();

                    rootFolders.Remove(rootFolder);
                    lstToDelete.Remove(rootFolder);

                    //must update priority each time to reflect automatic database change
                    for (Int16 priority = 0; priority < rootFolders.Count; priority++)
                        rootFolders[priority].Priority = priority;
                }

                List<SortDescription> sortDescriptions = new List<SortDescription>(SortDescriptions);

                RootFolders = rootFolders.ToArray();

                SortDescriptions.Clear();

                foreach (SortDescription sortDescription in sortDescriptions)
                    SortDescriptions.Add(sortDescription);
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not delete root folders: ");
            }
        }

        /// <summary>
        /// Moves the selected root folders up in the priority list
        /// </summary>
        private void MoveUp()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before editing any root folders.");
                    return;
                }

                if (RootFolders.Length > 0)
                {
                    //get list of selected root folders
                    List<RootFolder> selectedRootFolders = new List<RootFolder>(dgRootFolders.SelectedItems.Cast<RootFolder>());

                    if (selectedRootFolders.Count > 0)
                    {
                        //sort by priority
                        selectedRootFolders.Sort();

                        //get list of original paths from selected root folders
                        IntelligentString[] selectedRootFolderPaths =
                            (from selectedRootFolder in selectedRootFolders
                             select selectedRootFolder.Path).ToArray();

                        //if the first root folder is selected then we can't move them up
                        if (selectedRootFolders[0] == RootFolders[0])
                            return;

                        foreach (RootFolder rootFolder in selectedRootFolders)
                        {
                            //get the root folder moving down
                            RootFolder moveDown = RootFolders[rootFolder.Priority - 1];

                            RootFolder.SwitchRootFolderPaths(rootFolder, moveDown);
                        }

                        //select the root folders with the paths that moved up
                        dgRootFolders.SelectedItems.Clear();

                        foreach (IntelligentString selectedRootFolderPath in selectedRootFolderPaths)
                            dgRootFolders.SelectedItems.Add(RootFolders.Single(p => p.Path == selectedRootFolderPath));
                    }
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not move root folder(s): ");
            }
        }

        /// <summary>
        /// Moves the selected root folders down in the priority list
        /// </summary>
        private void MoveDown()
        {
            try
            {
                if (IsOrganising)
                {
                    GeneralMethods.MessageBoxApplicationError("Please wait for the library to finish organising before editing any root folders.");
                    return;
                }

                if (RootFolders.Length > 0)
                {
                    //get list of selected root folders
                    List<RootFolder> selectedRootFolders = new List<RootFolder>(dgRootFolders.SelectedItems.Cast<RootFolder>());

                    if (selectedRootFolders.Count > 0)
                    {
                        //sort by priority
                        selectedRootFolders.Sort();

                        //get list of original paths from selected root folders
                        IntelligentString[] selectedRootFolderPaths =
                            (from selectedRootFolder in selectedRootFolders
                             select selectedRootFolder.Path).ToArray();

                        //if the last root folder is selected then we can't move them down
                        if (selectedRootFolders.Last() == RootFolders.Last())
                            return;

                        //move down in reverse order
                        selectedRootFolders.Reverse();

                        foreach (RootFolder rootFolder in selectedRootFolders)
                        {
                            //get the root folder moving up
                            RootFolder moveUp = RootFolders[rootFolder.Priority + 1];

                            RootFolder.SwitchRootFolderPaths(rootFolder, moveUp);
                        }

                        //select the root folders with the paths that moved down
                        dgRootFolders.SelectedItems.Clear();

                        foreach (IntelligentString selectedRootFolderPath in selectedRootFolderPaths)
                            dgRootFolders.SelectedItems.Add(RootFolders.Single(p => p.Path == selectedRootFolderPath));
                    }
                }
            }
            catch (System.Exception e)
            {
                GeneralMethods.MessageBoxException(e, "Could not move root folder(s): ");
            }
        }

        #endregion

        #region Event Handlers

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgRootFolders.Items.SortDescriptions.Add(new SortDescription("Priority", ListSortDirection.Ascending));
        }

        private void cmRootFolders_Opened(object sender, RoutedEventArgs e)
        {
            miEdit.IsEnabled = dgRootFolders.SelectedItems.Count == 1;
            miDelete.IsEnabled = dgRootFolders.SelectedItems.Count >= 1;
            miMoveUp.IsEnabled = dgRootFolders.SelectedItems.Count > 0;
            miMoveDown.IsEnabled = dgRootFolders.SelectedItems.Count > 0;
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedRootFolder();
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteSelectedRootFolders();
        }

        private void miMoveUp_Click(object sender, RoutedEventArgs e)
        {
            MoveUp();
        }

        private void miMoveDown_Click(object sender, RoutedEventArgs e)
        {
            MoveDown();
        }

        private void dgRootFolders_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    DeleteSelectedRootFolders();
                    break;
            }
        }

        private void pmbRootFolders_UpClicked(object sender, RoutedEventArgs e)
        {
            MoveUp();
        }

        private void pmbRootFolders_AddClicked(object sender, RoutedEventArgs e)
        {
            AddRootFolder();
        }

        private void pmbRootFolders_DeleteClicked(object sender, RoutedEventArgs e)
        {
            DeleteSelectedRootFolders();
        }

        private void pmbRootFolders_DownClicked(object sender, RoutedEventArgs e)
        {
            MoveDown();
        }

        private void rootFolderDialog_PathChanged(object sender, RootFolderPathChangedEventArgs e)
        {
            OnPathChanged(e);
        }

        #endregion
    }
}
