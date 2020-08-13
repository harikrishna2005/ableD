using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ableD.Ui.Converters
{
    public class VisibilityBoolConverter : IValueConverter
    {
        /// <summary>
        /// 
        ///  TRUE   -   VISIBLE
        ///  FALSE  -   COLLAPSED
        ///  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if(System.Convert.ToBoolean(value) == true)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
