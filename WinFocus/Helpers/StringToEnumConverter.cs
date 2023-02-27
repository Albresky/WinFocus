//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WinFocus.Helpers;

//using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Data;

//namespace WinFocus.Helpers;

//public class StringToEnumConverter
//{
//    public StringToEnumConverter()
//    {
//    }

//    public object Convert(object value, Type targetType, object parameter)
//    {
//        if (parameter is string enumString)
//        {
//            switch (enumString)
//            {
//                case "4K":
//                    return 
//            }

//            if (!Enum.IsDefined(typeof(ElementTheme), value))
//            {
//                throw new ArgumentException("ExceptionEnumToBooleanConverterValueMustBeAnEnum");
//            }

//            var enumValue = Enum.Parse(typeof(ElementTheme), enumString);

//            return enumValue.Equals(value);
//        }

//        throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, string language)
//    {
//        if (parameter is string enumString)
//        {
//            return Enum.Parse(typeof(ElementTheme), enumString);
//        }

//        throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
//    }
//}

