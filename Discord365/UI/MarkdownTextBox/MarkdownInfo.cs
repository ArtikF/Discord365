using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord365.UI.MarkdownTextBox
{
    public class MarkdownInfo
    {
        public enum MarkdownType
        {
            PlainText,
            Italic,
            Bold,
            ItalicBold,
            SingleCode,
            MultiCode
        };
        
        public MarkdownInfo(MarkdownType type, int start, int end)
        {
            Type = type;
            StartPositon = start;
            EndPosition = end;
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
    }
}
