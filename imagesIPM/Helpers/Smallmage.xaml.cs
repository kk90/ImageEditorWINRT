using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace imagesIPM.Helpers
{
    public sealed partial class Smallmage : UserControl
    {
        private StorageFile file;
        private BitmapImage bitmap;

        public BitmapImage Bitmap
        {
            get { return bitmap; }
            set { bitmap = value; }
        }

        public StorageFile File
        {
            get { return file; }
        }


        public Smallmage(StorageFile file)
        {
            this.InitializeComponent();
            
            this.file = file;

            name.Text = file.Name;

            LoadThumbnail(file);
        }

        private async Task LoadThumbnail(StorageFile file)
        {
            var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.SingleItem);
            var stream = thumbnail.CloneStream();
            bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(stream);
            tumb.Source = bitmap;
        }



    }
}
