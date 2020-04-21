using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utilities.Business;
using Xceed.Wpf.Toolkit;

namespace Utilities.Presentation.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for TimeSpanPicker.xaml
    /// </summary>
    public partial class TimeSpanPicker : UserControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(TimeSpan), typeof(TimeSpanPicker), new PropertyMetadata(OnValuePropertyChanged));

        public static readonly DependencyProperty DaysProperty = DependencyProperty.Register("Days", typeof(Int32), typeof(TimeSpanPicker), new PropertyMetadata(OnTimeSpanPartChanged));

        public static readonly DependencyProperty HoursProperty = DependencyProperty.Register("Hours", typeof(Int32), typeof(TimeSpanPicker), new PropertyMetadata(OnTimeSpanPartChanged));

        public static readonly DependencyProperty MinutesProperty = DependencyProperty.Register("Minutes", typeof(Int32), typeof(TimeSpanPicker), new PropertyMetadata(OnTimeSpanPartChanged));

        public static readonly DependencyProperty SecondsProperty = DependencyProperty.Register("Seconds", typeof(Int32), typeof(TimeSpanPicker), new PropertyMetadata(OnTimeSpanPartChanged));

        public static readonly DependencyProperty IncludeDaysProperty = DependencyProperty.Register("IncludeDays", typeof(Boolean), typeof(TimeSpanPicker), new PropertyMetadata(OnTimeSpanPartChanged));

        #endregion

        #region Fields

        private Boolean bubbleTimeSpanPartChangeEvent = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value currently selected
        /// </summary>
        public TimeSpan Value
        {
            get { return (TimeSpan)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the currently selected number of days
        /// </summary>
        public Int32 Days
        {
            get { return (Int32)GetValue(DaysProperty); }
            set { SetValue(DaysProperty, value); }
        }

        /// <summary>
        /// Gets or sets the currently selected number of hours
        /// </summary>
        public Int32 Hours
        {
            get { return (Int32)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        /// <summary>
        /// Gets or sets the currently selected number of minutes
        /// </summary>
        public Int32 Minutes
        {
            get { return (Int32)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the currently selected number of seconds
        /// </summary>
        public Int32 Seconds
        {
            get { return (Int32)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value determining whether or not days should be included in the view
        /// </summary>
        public Boolean IncludeDays
        {
            get { return (Boolean)GetValue(IncludeDaysProperty); }
            set { SetValue(IncludeDaysProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the TimeSpanPicker class
        /// </summary>
        public TimeSpanPicker()
        {
            InitializeComponent();
        }

        #endregion

        #region Static Methods

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSpanPicker tsp = d as TimeSpanPicker;
            tsp.bubbleTimeSpanPartChangeEvent = false;

            try
            {
                if (tsp.IncludeDays)
                    tsp.Days = tsp.Value.Days;
                else
                {
                    if (tsp.Value.Days > 0)
                        tsp.Value = new TimeSpan(0, tsp.Value.Hours, tsp.Value.Minutes, tsp.Value.Seconds);
                }

                tsp.Hours = tsp.Value.Hours;
                tsp.Minutes = tsp.Value.Minutes;
                tsp.Seconds = tsp.Value.Seconds;
            }
            finally
            {
                tsp.bubbleTimeSpanPartChangeEvent = true;
            }
        }

        private static void OnTimeSpanPartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSpanPicker tsp = d as TimeSpanPicker;

            if (tsp.bubbleTimeSpanPartChangeEvent)
                tsp.Value = new TimeSpan(tsp.IncludeDays ? tsp.Days : 0, tsp.Hours, tsp.Minutes, tsp.Seconds);
        }

        #endregion
    }
}
