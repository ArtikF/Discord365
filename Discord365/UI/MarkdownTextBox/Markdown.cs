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
        //public static Grid GetContentFromText(string text)
        //{
        //    Grid g = new Grid();

        //    if (Properties.Settings.Default.PlainTextInsteadOfMarkdown)
        //    {
        //        PlainTextBox tb = new PlainTextBox();
        //        tb.Text = text;
        //        g.Children.Add(tb);
        //        return g;
        //    }

        //    WrapPanel w = new WrapPanel();
        //    g.Children.Add(w);

        //    foreach(var md in GetMarkdownFormatting(text))
        //    {
        //        if (md.IsBreakingTextBox())
        //        {

        //        }

        //        if(md.Type == MarkdownInfo.MarkdownType.PlainText)
        //        {
        //        }
        //    }

        //    // Placeholder
        //    FlowDocument flow = new FlowDocument();
        //    flow.Blocks.Add(new Paragraph(new Run(text) { FontFamily = new FontFamily("Segoe UI"), FontSize = 14 }));
        //    w.Children.Add(new MarkdownBox(flow));

        //    return g;
        //}

        //public static List<MarkdownInfo> GetMarkdownFormatting(string source)
        //{
        //    List<MarkdownInfo> result = new List<MarkdownInfo>();


        //    return result;
        //}
    }
}
