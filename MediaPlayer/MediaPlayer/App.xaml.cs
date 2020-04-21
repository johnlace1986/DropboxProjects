using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using MediaPlayer.Library.Business;
using Utilities.Exception;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Event Handlers

        private void brdHeader_MouseMove(object sender, MouseEventArgs e)
        {
            Border brdHeader = sender as Border;
            Window window = brdHeader.TemplatedParent as Window;

            if (e.LeftButton == MouseButtonState.Pressed)
                window.DragMove();

        }

        private void brdTitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border brdTitleBar = sender as Border;
            Window window = brdTitleBar.TemplatedParent as Window;

            if (e.ClickCount > 1)
                ToggleMaximized(window);
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Button btnMinimize = sender as Button;
            Window window = btnMinimize.TemplatedParent as Window;

            window.WindowState = System.Windows.WindowState.Minimized;
        }

        private void btnRestoreMaximize_Click(object sender, RoutedEventArgs e)
        {
            Button btnRestoreMaximize = sender as Button;
            Window window = btnRestoreMaximize.TemplatedParent as Window;

            ToggleMaximized(window);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Button btnClose = sender as Button;
            Window window = btnClose.TemplatedParent as Window;

            window.Close();
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            Button btnClearSearch = sender as Button;
            TextBox txtSearch = btnClearSearch.TemplatedParent as TextBox;
            txtSearch.Text = "";
        }

        private void thmTop_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thmTop = sender as Thumb;
            Window window = thmTop.TemplatedParent as Window;

            DragUp(window, e.VerticalChange);
        }

        private void thmBottom_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thmBottom = sender as Thumb;
            Window window = thmBottom.TemplatedParent as Window;

            DragDown(window, e.VerticalChange);
        }

        private void thmLeft_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thmLeft = sender as Thumb;
            Window window = thmLeft.TemplatedParent as Window;

            DragLeft(window, e.HorizontalChange);
        }

        private void thmRight_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thmRight = sender as Thumb;
            Window window = thmRight.TemplatedParent as Window;

            DragRight(window, e.HorizontalChange);
        }

        private void thmTopLeft_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thmTopLeft = sender as Thumb;
            Window window = thmTopLeft.TemplatedParent as Window;

            DragUp(window, e.VerticalChange);
            DragLeft(window, e.HorizontalChange);
        }

        private void thmTopRight_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thmTopRight = sender as Thumb;
            Window window = thmTopRight.TemplatedParent as Window;

            DragUp(window, e.VerticalChange);
            DragRight(window, e.HorizontalChange);
        }

        private void thmBottomRight_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thmBottomRight = sender as Thumb;
            Window window = thmBottomRight.TemplatedParent as Window;

            DragDown(window, e.VerticalChange);
            DragRight(window, e.HorizontalChange);
        }

        private void thmBottomLeft_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Thumb thmBottomLeft = sender as Thumb;
            Window window = thmBottomLeft.TemplatedParent as Window;

            DragDown(window, e.VerticalChange);
            DragLeft(window, e.HorizontalChange);
        }

        private void dataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            dataGrid.SelectedItems.Clear();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Toggles the maximized state of the main window
        /// </summary>
        /// <param name="window">The window being modified</param>
        private static void ToggleMaximized(Window window)
        {
            if ((window.ResizeMode == ResizeMode.CanMinimize) ||
                (window.ResizeMode == ResizeMode.NoResize))
                return;

            if (window.WindowState == System.Windows.WindowState.Maximized)
                window.WindowState = System.Windows.WindowState.Normal;
            else
                window.WindowState = System.Windows.WindowState.Maximized;
        }

        /// <summary>
        /// Drags the window upwards
        /// </summary>
        /// <param name="window">The window being modified</param>
        /// <param name="change">The number of pixels the window is being dragged upwards by</param>
        private static void DragUp(Window window, double change)
        {
            bool down = (change > 0);

            if (down)
            {
                if ((window.Height - change) > window.MinHeight)
                {
                    window.Height -= change;
                    window.Top += change;
                }
            }
            else
            {
                window.Height -= change;
                window.Top += change;
            }
        }

        /// <summary>
        /// Drags the window downwards
        /// </summary>
        /// <param name="window">The window being modified</param>
        /// <param name="change">The number of pixels the window is being dragged downwards by</param>
        private static void DragDown(Window window, double change)
        {
            bool down = (change > 0);

            if (down)
                window.Height += change;
            else
                if ((window.Height + change) > window.MinHeight)
                    window.Height += change;
        }

        /// <summary>
        /// Drags the window to the left
        /// </summary>
        /// <param name="window">The window being modified</param>
        /// <param name="change">The number of pixels the window is being dragged to the left by</param>
        private static void DragLeft(Window window, double change)
        {
            bool right = (change > 0);

            if (right)
            {
                if ((window.Width - change) > window.MinWidth)
                {
                    window.Width -= change;
                    window.Left += change;
                }
            }
            else
            {
                window.Width -= change;
                window.Left += change;
            }
        }

        /// <summary>
        /// Drags the window to the right
        /// </summary>
        /// <param name="window">The window being modified</param>
        /// <param name="change">The number of pixels the window is being dragged to the right by</param>
        private static void DragRight(Window window, double change)
        {
            bool right = (change > 0);

            if (right)
                window.Width += change;
            else
                if ((window.Width + change) > window.MinWidth)
                    window.Width += change;
        }

        /// <summary>
        /// Gets or sets the available file types for the specified media item
        /// </summary>
        public static FileType[] GetFileTypesForMediaItem(MediaItem mediaItem)
        {
            if (mediaItem != null)
            {
                MediaPlayerDialog mpd = Application.Current.MainWindow as MediaPlayerDialog;

                if (mpd != null)
                {
                    switch (mediaItem.Type)
                    {
                        case MediaItemTypeEnum.Video:
                            return mpd.VideoFileTypes.ToArray();
                        case MediaItemTypeEnum.Song:
                            return mpd.SongFileTypes.ToArray();
                        default:
                            throw new UnknownEnumValueException(mediaItem.Type);
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
