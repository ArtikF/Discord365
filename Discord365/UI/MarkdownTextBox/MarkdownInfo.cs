using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Discord365.UI.MarkdownTextBox
{
    public class MarkdownInfo
    {
        public static int DefaultFontSize = 13;
        public static SolidColorBrush DefaultForeground = new SolidColorBrush(Color.FromArgb(0xB2, 0xFF, 0xFF, 0xFF));

        public enum MarkdownType
        {
            PlainText,
            Italic,
            Bold,
            ItalicBold,
            SingleCode,
            MultiCode,
            Spoiler
        };
        
        public MarkdownInfo(MarkdownType type, string content = "")
        {
            Type = type;
            Content = content;
        }

        public DependencyObject GetControl()
        {
            DependencyObject d = null;

            if (Type == MarkdownType.PlainText)
            {
                var run = new Run((string)Content)
                {
                    Foreground = DefaultForeground,
                    FontSize = DefaultFontSize
                };

                d = run;

                if (Type == MarkdownType.Bold)
                    d = new Bold(run);
                else if (Type == MarkdownType.Italic)
                    d = new Italic(run);
                else if (Type == MarkdownType.ItalicBold)
                    d = new Italic(new Bold(run));
            }
            else if (Type == MarkdownType.Spoiler)
                return new SpoilerText((string)Content);

            return d;
        }

        public bool IsRun()
        {
            if (Type == MarkdownType.PlainText ||
                Type == MarkdownType.Bold ||
                Type == MarkdownType.Italic ||
                Type == MarkdownType.ItalicBold)
                return true;

            return false;
        }

        public bool IsBreakingTextBox()
        {
            if (Type == MarkdownType.SingleCode || Type == MarkdownType.MultiCode)
                return true;
            else
                return false;
        }

        public MarkdownType Type = MarkdownType.PlainText;
        public int StartPositon = -1, EndPosition = -1;
        public object Content = "";
    }
}
