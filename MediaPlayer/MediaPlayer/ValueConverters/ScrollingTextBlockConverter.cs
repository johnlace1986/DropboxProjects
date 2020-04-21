using System;
using System.Linq;
using System.Windows.Data;
using Utilities.Business;
using System.Windows.Controls;
using System.Windows.Media;

namespace MediaPlayer.ValueConverters
{
    public class ScrollingTextBlockConverter : IMultiValueConverter
    {
        #region Static Methods

        /// <summary>
        /// Calculates whether the text of the text block is trimmed
        /// </summary>
        /// <param name="textBlock">Text block being checked</param>
        /// <returns>True if the text of the text block is trimmed, false if not</returns>
        private static bool CalculateIsTextTrimmed(TextBlock textBlock)
        {
            Typeface typeface = new Typeface(
                textBlock.FontFamily,
                textBlock.FontStyle,
                textBlock.FontWeight,
                textBlock.FontStretch);

            // FormattedText is used to measure the whole width of the text held up by TextBlock container
            FormattedText formattedText = new FormattedText(
                textBlock.Text,
                System.Threading.Thread.CurrentThread.CurrentCulture,
                textBlock.FlowDirection,
                typeface,
                textBlock.FontSize,

                textBlock.Foreground);

            formattedText.MaxTextWidth = textBlock.ActualWidth;

            // When the maximum text width of the FormattedText instance is set to the actual
            // width of the textBlock, if the textBlock is being trimmed to fit then the formatted
            // text will report a larger height than the textBlock. Should work whether the
            // textBlock is single or multi-line.
            return ((int)formattedText.Height > (int)textBlock.ActualHeight);
        }

        #endregion

        #region IMultiValueConverter Members
        
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 3)
            {
                if (values[0] is IntelligentString)
                {
                    IntelligentString text = (IntelligentString)values[0];

                    if (values[1] is Int32)
                    {
                        Int32 ptr = (Int32)values[1];

                        if (values[2] is TextBlock)
                        {
                            TextBlock textBlock = values[2] as TextBlock;
                            IntelligentString actualText = text;

                            if (CalculateIsTextTrimmed(textBlock))
                            {
                                IntelligentString scollingText = IntelligentString.Empty;

                                for (int i = ptr; i < actualText.Length; i++)
                                    scollingText += actualText[i].ToString();

                                scollingText += "      ";

                                for (int i = 0; i < ptr; i++)
                                    scollingText += actualText[i].ToString();

                                return scollingText.Value.Trim();
                            }
                            else
                                return actualText.Value;
                        }
                    }
                }
            }

            return String.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
