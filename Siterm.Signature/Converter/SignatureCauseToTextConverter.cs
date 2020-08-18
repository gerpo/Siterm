using System;
using System.Globalization;
using System.Windows.Data;
using Siterm.Signature.Resources;

namespace Siterm.Signature.Converter
{
    internal class SignatureCauseToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SignatureCause cause)) return string.Empty;

            return cause switch
            {
                SignatureCause.Instruction =>
                    $"{UiStrings.SignatureConfirmBaseString} {UiStrings.InstructionCauseString}",
                SignatureCause.Instructor =>
                    $"{UiStrings.SignatureConfirmBaseString} {UiStrings.InstructorCauseString}",
                SignatureCause.ServiceReport =>
                    $"{UiStrings.SignatureConfirmBaseString} {UiStrings.ServiceReportCauseString}",
                _ => $"{UiStrings.SignatureConfirmBaseString} {UiStrings.InstructionCauseString}"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}