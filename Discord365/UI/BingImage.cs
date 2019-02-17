using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord365.UI
{
    public class BingImage
    {
        public ImageInfo[] images;
        public TooltipInfo tooltips;
    }

    public class TooltipInfo
    {
        public string loading;
        public string previous;
        public string next;
        public string walle;
        public string walls;
    }

    public class ImageInfo
    {
        public string startdate;
        public string fullstartdate;
        public string enddate;
        public string url;
        public string urlbase;
        public string copyright;
        public Uri copyrightlink;
        public string title;
        public string quiz;
        public bool wp;
        public string hsh;
        public int drk;
        public int top;
        public int bot;
        public string[] hs;
    }
}
