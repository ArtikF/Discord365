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
    /// Interaction logic for MessageEmbed.xaml
    /// </summary>
    public partial class MessageEmbed : UserControl
    {
        private Discord.IEmbed related = null;
        public Discord.IEmbed Related
        {
            get => related;
            set
            {
                related = value;
                Wrap.Children.Clear();

                var title = value.Title;
                var author = value.Author;
                var thumb = value.Thumbnail;
                var image = value.Image;
                var type = value.Type;
                var color = value.Color;
                var description = value.Description;

                if(color != null)
                    BorderStatus.Background = new SolidColorBrush(Color.FromArgb(50, color.Value.R, color.Value.G, color.Value.B));

                if (title != null)
                    Add(GetTitle(title));

                if (description != null)
                    Add(GetDescription(description));
            }
        }

        public void Add(dynamic e)
        {
            if (e == null)
                return;
            
            e.Margin = new Thickness(0, 2, 0, 2);
            Wrap.Children.Add(e);
        }

        public TextBlock GetTitle(string text)
        {
            TextBlock t = new TextBlock();
            t.FontSize = 14;
            t.Text = text;
            return t;
        }

        public TextBlock GetDescription(string text)
        {
            TextBlock t = new TextBlock();
            t.Text = text;
            return t;
        }

        public MessageEmbed()
        {
            InitializeComponent();
            Wrap.Children.Clear();
        }
    }
}
