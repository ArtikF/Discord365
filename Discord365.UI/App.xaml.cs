using Avalonia;
using Avalonia.Markup.Xaml;

namespace Discord365.UI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
