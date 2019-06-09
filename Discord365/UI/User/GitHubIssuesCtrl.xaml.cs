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
using static Discord365.UI.Extensions;

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

            this.FadeIn(500);
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/feel-the-dz3n/Discord365/issues");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(async () =>
            {
                var github = new GitHubClient(new ProductHeaderValue("Discord365"));
                var issues = await github.Issue.GetAllForRepository("feel-the-dz3n", "Discord365");
                int start = 0;

                Dispatcher.Invoke(() => tbLoading.FadeOut(250));

                Thread.Sleep(250);

                Dispatcher.Invoke(() => IssuesBox.Items.Clear());

                foreach (var issue in issues)
                {
                    if (issue.Locked)
                        continue;

                    Dispatcher.Invoke(() =>
                    {
                        var grid = GetGrid(issue.Number.ToString(), issue.Title, issue.HtmlUrl);
                        IssuesBox.Items.Add(grid);
                        grid.FadeIn(350, start);
                    });

                    start += 50;
                }
            }).Start();
        }

        private Grid GetGrid(string number, string title, string link)
        {
            Grid g = new Grid();
            g.Margin = new Thickness(0, 2, 0, 2);
            g.Tag = link;

            WrapPanel p = new WrapPanel();
            p.Orientation = Orientation.Horizontal;
            g.Children.Add(p);

            TextBlock b = new TextBlock();
            p.Children.Add(b);
            b.Text = "#" + number;
            b.Margin = new Thickness(8, 0, 0, 0);
            b.Foreground = new SolidColorBrush(Color.FromArgb(75, 255, 255, 255));

            b = new TextBlock();
            p.Children.Add(b);
            b.Text = title;
            b.Margin = new Thickness(4, 0, 0, 0);
            b.Foreground = new SolidColorBrush(Color.FromArgb(155, 255, 255, 255));

            //b = new TextBlock();
            //p.Children.Add(b);
            //b.Text = "by " + author;
            //b.Margin = new Thickness(4, 0, 0, 0);
            //b.Foreground = new SolidColorBrush(Color.FromArgb(75, 255, 255, 255));

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

        private void TextBlock_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/feel-the-dz3n/Discord365/issues/new");
        }
    }
}

