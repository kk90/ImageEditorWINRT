using imagesIPM.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;

namespace imagesIPM.ViewModels
{
    public class ImageBrowserViewModel : INotifyPropertyChanged
    {



        public string Folder { get; set; }

        private PathsManager PM = new PathsManager();

        public PathsManager PM1
        {
            get { return PM; }
            set { PM = value; }
        }

        public List<string> LastPaths
        {
            get { return 
                new List<string>(PM.list());
            }
        }

        public void addPath(string path)
        {
            PM.PushPath(path);
            Notify("LastPaths");
        }

        public string Filename
        {
            get;
            set;
        }

        public string Size
        {
            get;
            set;

        }

        public string Path
        {
            get;
            set;
        }

        public string Date
        {
            get;set;
        }


        public Visibility DetailVisibility
        {
            get
            {
                if (DetailsVisible)
                {
                    return Visibility.Visible;
                    
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public void NotifySelectedFileParams()
        {
            Notify("Filename");
            Notify("Size");
            Notify("Date");
            Notify("Name");
            Notify("DetailVisibility");
            Notify("ImageSource");
        }

        private int mSelectedPathIndex;

        public int SelectedPathIndex
        {
            get { return mSelectedPathIndex; }
            set { mSelectedPathIndex = value;
            Notify("SelectedPathIndex");
            }
        }

        private int mSelectedImageIndex;
        public int SelectedImageIndex
        {
            get
            {
                return mSelectedImageIndex;
            }
            set
            {
                mSelectedImageIndex=value;
                Notify("SelectedImageIndex");
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


        public bool DetailsVisible { get; set; }
    }
}
