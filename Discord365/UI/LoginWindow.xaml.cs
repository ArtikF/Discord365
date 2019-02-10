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
using System.Windows.Shapes;

namespace Discord365.UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            string token = tbToken.Password;

            App.MainWnd = new MainClientWnd(this);

            Discord.TokenType type = Discord.TokenType.Bot;

            if ((bool)rUser.IsChecked)
                type = Discord.TokenType.User;

            App.MainWnd.StartConnection(token, type);
            App.MainWnd.ShowDialog();

            this.Show();
        }
    }
}
