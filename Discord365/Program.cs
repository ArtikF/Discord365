using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Discord365
{
    class Program
    {
        static int Main(string[] args)
        {
            if(Environment.OSVersion.Version.Major <= 5 || // xp or earlier
                (Environment.OSVersion.Version.Minor == 0 && Environment.OSVersion.Version.Major == 6)) // vista
            {
                var msg = System.Windows.MessageBox.Show("Your OS is not supported by Discord365. The minimal supported version is Windows 7. Do you want to continue?", "Discord365", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning);

                if(msg != System.Windows.MessageBoxResult.Yes)
                {
                    return 1;
                }
            }

            Discord365.Core.App.RunCore(args);

            return 0;
        }
    }
}
