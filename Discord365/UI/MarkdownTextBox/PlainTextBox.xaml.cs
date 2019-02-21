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

namespace Discord365.UI.MarkdownTextBox
{
    /// <summary>
    /// Interaction logic for PlainTextBox.xaml
    /// </summary>
    public partial class PlainTextBox : UserControl
    {
        public string Text
        {
            get => tbMessage.Text;
            set => tbMessage.Text = value;
        }

        public PlainTextBox()
        {
            InitializeComponent();
        }
    }
}
