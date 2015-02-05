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
    class ImageBrowserViewModel : INotifyPropertyChanged
    {

        public ImageBrowserViewModel()
        {
            PM = new PathsManager();
        }

        public BasicProperties Properties { get; set; }

        public string Folder { get; set; }

        private PathsManager PM;

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
            get
            {
                if (selectedFile == null)
                {
                    return "";
                }
                return selectedFile.Name;
            }
        }

        public string Size
        {
            get
            {
                if (selectedFile == null)
                {
                    return "";
                }
                return Math.Round(Properties.Size/1024.0,2).ToString()+" kb" ;
            }
        }

        public string Path
        {
            get
            {
                if (selectedFile == null)
                {
                    return "";
                }
                return selectedFile.Path;
            }
        }

        public string Date
        {
            get
            {
                if (selectedFile == null)
                {
                    return "";
                }
                return Properties.DateModified.ToString();
            }
        }


        public Visibility DetailVisibility
        {
            get
            {
                if (selectedFile == null)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        private void NotifySelectedFileParams()
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

        private StorageFile selectedFile;
        public StorageFile SelectedFile
        {
            get
            {
                return selectedFile;
            }

            set
            {
                selectedFile = value;
                NotifySelectedFileParams();
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
