using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Discord365.UI.MarkdownTextBox
{
    public class Markdown
    {
        public static TextBlock GetContentFromText(string text)
        {
            TextBlock t = new TextBlock();

            t.FontSize = MarkdownInfo.DefaultFontSize;
            t.Foreground = MarkdownInfo.DefaultForeground;
            t.Text = text;
            return t;
            // unimplemented

            var mds = GetMarkdownFormatting(text);
            var controls = GetControlsFromMarkdown(mds);

            for (int i = 0; i < controls.Count; i++)
            {
                var c = controls[i];

                if (c is Inline)
                    t.Inlines.Add((Inline)c);
                else if (c is UIElement)
                    t.Inlines.Add((UIElement)c);
            }

            return t;
        }

        public static List<DependencyObject> GetControlsFromMarkdown(List<MarkdownInfo> mds)
        {
            List<DependencyObject> l = new List<DependencyObject>();

            for (int i = 0; i < mds.Count; i++)
            {
                var md = mds[i];
                l.Add(md.GetControl());
            }

            return l;
        }

        public static List<MarkdownInfo> GetMarkdownFormatting(string source)
        {
            List<MarkdownInfo> result = new List<MarkdownInfo>();

            for(int i = 0; i < source.Length; i++)
            {
                char c = SafeGetChar(source, i);
                char c1 = SafeGetChar(source, i + 1);
                char c2 = SafeGetChar(source, i + 2);

                if (c == '*' && c1 != '*') // italic
                {
                    int start = i + 2;
                    int end = LookForTheEnd(source, i, "*") + 1;

                    if (end >= 0 && end < source.Length)
                    {
                        MarkdownInfo info = new MarkdownInfo(MarkdownInfo.MarkdownType.Italic, GetStringFromIdx(source, start, end));
                        result.Add(info);
                    }
                }
                else if (c == '*' && c1 == '*') // bold
                {
                    int start = i + 2;
                    int end = LookForTheEnd(source, i, "**") + 2;

                    if (end >= 0 && end < source.Length)
                    {
                    }
                }
                else if (c == '|' && c1 == '|') // spoiler
                {
                    int start = i + 2;
                    int end = LookForTheEnd(source, i, "||") + 2;

                    if (end >= 0 && end < source.Length)
                    {
                        MarkdownInfo info = new MarkdownInfo(MarkdownInfo.MarkdownType.Spoiler, GetStringFromIdx(source, start, end));
                        result.Add(info);
                    }
                }
            }

            return result;
        }

        private static string GetStringFromIdx(string text, int start, int end)
        {
            StringBuilder b = new StringBuilder();

            for(int i = start; i < end; i++)
            {
                b.Append(SafeGetChar(text, i));
            }

            return b.ToString();
        }

        private static int LookForTheEnd(string text, int start, string endchars)
        {
            for (int i = start; i < text.Length; i++)
            {
                char c = SafeGetChar(text, i);
                char c1 = SafeGetChar(text, i + 1);
                char c2 = SafeGetChar(text, i + 2);

                if (CheckForEnding(c, c1, c2, endchars))
                    return i + endchars.Length - 1;
            }

            return start;
        }

        private static bool CheckForEnding(char c, char c1, char c2, string endchars)
        {
            if (c == SafeGetChar(endchars, 0)) // 0
            {
                if (endchars.Length == 1)
                    return true;

                if (c1 == SafeGetChar(endchars, 1)) // 1
                {
                    if (endchars.Length == 2)
                        return true;

                    if (c2 == SafeGetChar(endchars, 2)) // 2
                        return true;
                }
            }

            return false;
        }

        private static char SafeGetChar(string text, int index)
        {
            if(index >= 0 && text.Length > index)
                return text[index];

            return '\0';
        }
    }
}
