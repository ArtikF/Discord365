using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Discord365.UI.ImageViewer
{
    public class ImageInfo
    {
        public string source = "";

        public ImageInfo(string sourceUri)
        {
            source = sourceUri;
        }

        public ImageSource ToImageSource()
        {
            return BitmapFrame.Create(new Uri(source, UriKind.Absolute));
        }
    }
}
