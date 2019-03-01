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
    /// Interaction logic for SpoilerText.xaml
    /// </summary>
    public partial class SpoilerText : UserControl
    {
        public bool ShowSecretText
        {
            set
            {
                if (value)
                    BorderReveal.Visibility = Visibility.Collapsed;
                else
                    BorderReveal.Visibility = Visibility.Visible;
            }
        }

        public string SecretText { get => tbSecretText.Text; set => tbSecretText.Text = value; }

        public SpoilerText(string text = "")
        {
            InitializeComponent();
            SecretText = text;
        }

        private void BorderReveal_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            const int time = 500;
            const int timebrd = 250;
            tbSecretText.FadeIn(timebrd, time);
            BorderReveal.FadeOut(timebrd);
        }
    }
}
