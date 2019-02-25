using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Discord365.UI.User.MessagesPage.Message
{
    /// <summary>
    /// Interaction logic for TextMessageContent.xaml
    /// </summary>
    public partial class TextMessageContent : UserControl
    {
        public TextMessageContent()
        {
            InitializeComponent();
            MessageText = "";
        }

        private string OriginalText = "";

        public string MessageText
        {
            get => OriginalText;
            set
            {
                OriginalText = value;

                GridContent.Children.Clear();

                var content = new MarkdownTextBox.PlainTextBox() { Text = value }; //MarkdownTextBox.Markdown.GetContentFromText(value);
                content.Margin = new Thickness(-2, 0, 0, 0);

                GridContent.Children.Add(content);
            }
        }
    }
}
