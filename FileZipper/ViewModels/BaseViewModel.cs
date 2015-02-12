using System;
using System.ComponentModel;

namespace FileZipper.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IDisposable
    {

        #region << Constructor >>

        protected BaseViewModel() { }

        #endregion

        #region << INotifyPropertyChangedMembers >>

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this objects PropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if(handler != null)
            {
                var e = new PropertyChangedEventArgs( propertyName );
                handler( this, e );
            }
        }

        #endregion

        #region IDisposableMembers >>

        /// <summary>
        /// Invoked when this object is being removed from the application and will be subject to GC.
        /// </summary>
        public void Dispose()
        {
            this.OnDispose();
        }

        /// <summary>
        /// Children can override for any additional cleanup desired.
        /// </summary>
        protected virtual void OnDispose()
        {

        }

        #endregion

    }
}
