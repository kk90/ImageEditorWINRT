using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imagesIPM.ViewModels
{
    class ImageEditorViewModel
    {
        string pathToImage;

        public string PathToImage
        {
            get { return pathToImage; }
            set { pathToImage = value; }
        }

        bool imgChanged = false;

        public bool ImgChanged
        {
            get { return imgChanged; }
            set { imgChanged = value; }
        }
    }
}
