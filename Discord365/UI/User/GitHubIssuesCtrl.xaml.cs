using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Discord365.UI.User
{
    /// <summary>
    /// Interaction logic for GitHubIssuesCtrl.xaml
    /// </summary>
    public partial class GitHubIssuesCtrl : UserControl
    {
        public GitHubIssuesCtrl()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/discord365/Discord365/issues");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(async () => 
            {
                var github = new GitHubClient(new ProductHeaderValue("Discord365"));
                var issues = await github.Issue.GetAllForRepository("discord365", "Discord365");
                
                foreach(var issue in issues)
                {
                    if (issue.Locked)
                        continue;

                    Dispatcher.Invoke(() =>
                    {
                        string title = issue.Title;
                        IssuesBox.Items.Add(GetGrid(title, issue.HtmlUrl));
                    });
                }
            }).Start();
        }

        private Grid GetGrid(string title, string link)
        {
            Grid g = new Grid();
            g.Margin = new Thickness(0, 2, 0, 2);
            g.Tag = link;

            TextBlock b = new TextBlock();
            g.Children.Add(b);
            b.Text = title;
            b.HorizontalAlignment = HorizontalAlignment.Left;
            b.VerticalAlignment = VerticalAlignment.Center;
            b.Margin = new Thickness(8, 0, 0, 0);
            b.Foreground = new SolidColorBrush(Color.FromArgb(155, 255, 255, 255));

            return g;
        }

        private void IssuesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object o = IssuesBox.SelectedItem;

            if (o == null)
                return;

            if (o is Grid)
            {
                var g = o as Grid;

                if (g.Tag != null && g.Tag is string)
                {
                    var tag = g.Tag as string;
                    System.Diagnostics.Process.Start(tag);
                }
            }

        }
    }
}

