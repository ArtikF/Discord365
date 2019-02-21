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
    /// Interaction logic for MarkdownBox.xaml
    /// </summary>
    public partial class MarkdownBox : UserControl
    {
        public FlowDocument Document
        {
            get => tbMessage.Document;
            set => tbMessage.Document = value;
        } 

        public MarkdownBox(FlowDocument d = null)
        {
            InitializeComponent();

            if (d != null)
                Document = d;
        }
    }
}
