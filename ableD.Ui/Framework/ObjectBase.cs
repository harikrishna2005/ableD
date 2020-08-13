using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ableD.Ui.Framework
{
    public class ObjectBase : INotifyPropertyChanged
    {
        /// <summary>
        ///     Property Changing Notification 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            /* if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            */


            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
