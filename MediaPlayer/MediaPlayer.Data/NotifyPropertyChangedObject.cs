using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;

namespace MediaPlayer.Data
{
    [Serializable]
    public abstract class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        #region Instance Methods

        /// <summary>
        /// Fires the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        protected void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyPropertyChanged Members

        [field:NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
