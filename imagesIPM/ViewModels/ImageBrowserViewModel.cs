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

        public BasicProperties Properties { get; set; }

        public string Folder { get; set; }

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


        private List<string> lastPaths;
        public List<string> LastPaths
        {
            get
            {
                 if (lastPaths == null)
                    lastPaths = new List<string>();
                return lastPaths;
            }

            set
            {
                lastPaths = value;
                Notify("LastPaths");
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
        public Windows.UI.Xaml.Media.Imaging.BitmapImage ImageSource;
    }
}
