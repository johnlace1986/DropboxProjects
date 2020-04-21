using System;
using System.Collections;
using System.Linq;
using System.Windows.Controls;

namespace MediaPlayer.Presentation.UserControls.ControlExtenders
{
    public class ItemsSourceTrackingDataGrid : DataGrid
    {
        #region Events

        /// <summary>
        /// Fires prior to the items source of the data grid changing
        /// </summary>
        public event EventHandler ItemsSourceChanging;

        /// <summary>
        /// Fires after the items source of the data grid changes
        /// </summary>
        public event EventHandler ItemsSourceChanged;

        #endregion

        #region Instance Methods

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            OnItemsSourceChanging();

            base.OnItemsSourceChanged(oldValue, newValue);

            OnItemsSourceChanged();
        }

        /// <summary>
        /// Fires the ItemsSourceChanging event
        /// </summary>
        private void OnItemsSourceChanging()
        {
            if (ItemsSourceChanging != null)
                ItemsSourceChanging(this, new System.EventArgs());
        }

        /// <summary>
        /// Fires the ItemsSourceChanged event
        /// </summary>
        private void OnItemsSourceChanged()
        {
            if (ItemsSourceChanged != null)
                ItemsSourceChanged(this, new System.EventArgs());
        }

        #endregion

    }
}
