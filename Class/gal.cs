using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Alblums.Class
{
    public class gal
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public string Image { get; set; }

        public gal(string name, double value, string image)
        {
            Name = name;
            Value = value;
            Image = image;
        }
    }
    public class imageGallery
    {
        public List<gal> gallery { get; set; }
        public string refPath { get; set; }

        public imageGallery(List<gal> gallery, string refPath)
        {
            this.gallery = gallery;
            this.refPath = refPath;
        }
    }
}
