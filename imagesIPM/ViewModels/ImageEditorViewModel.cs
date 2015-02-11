using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imagesIPM.ViewModels
{
    class ImageEditorViewModel : INotifyPropertyChanged
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
            set { imgChanged = value;
            Notify("ImgChanged");
            }
        }

        private void Notify(string name)
        {
            if (PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(name);
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
