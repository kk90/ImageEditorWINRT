using imagesIPM.Common;
using imagesIPM.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace imagesIPM
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ImegeEdit : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ImageEditorViewModel vm;
       
        private WriteableBitmap editBitmap;
        private WriteableBitmap originalBitmap;


        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }


        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public ImegeEdit()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            vm = new ImageEditorViewModel();
            this.DataContext = vm;
            
        }


        private async Task<WriteableBitmap> LoadWritableBitmap(string path)
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            var data = await FileIO.ReadBufferAsync(file);

            var ms = new InMemoryRandomAccessStream();
            var dw = new Windows.Storage.Streams.DataWriter(ms);
            dw.WriteBuffer(data);
            await dw.StoreAsync();
            ms.Seek(0);

            var bm = new BitmapImage();
            await bm.SetSourceAsync(ms);

            var wb = new WriteableBitmap(bm.PixelWidth, bm.PixelHeight);
            ms.Seek(0);

            await wb.SetSourceAsync(ms);

            return wb;
        }

        


        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

        }

 
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {

        }

        #region NavigationHelper registration


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            var file = e.Parameter as StorageFile;
            LoadBaseImage(file.Path);
        }

        private async void LoadBaseImage(string path)
        {
            vm.PathToImage = path;
            editBitmap = await LoadWritableBitmap(path);
            originalBitmap = editBitmap.Clone();

            MainImage.Source = editBitmap;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //close
            navigationHelper.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //save
            vm.ImgChanged = false;
            editBitmap.SaveAsAsync(vm.PathToImage);
            originalBitmap = editBitmap.Clone();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //revert
            editBitmap = originalBitmap.Clone();
            MainImage.Source = editBitmap;
            vm.ImgChanged = false;

        }
         
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //negative
            editBitmap = editBitmap.Invert();
            MainImage.Source = editBitmap;
            vm.ImgChanged = true;
        }

        bool mbrightnessvisibility = false;
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //brightnes
            if (mbrightnessvisibility)
            {
                BrightnesVisibility.Visibility = Visibility.Collapsed;
            }
            else
            {
                BrightnesVisibility.Visibility = Visibility.Visible;
            }
            mbrightnessvisibility = !mbrightnessvisibility;


        }

        double brightness=0;

        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            brightness += 0.1;
            editBitmap.Lighten(0.1);
            MainImage.Source = editBitmap;
            vm.ImgChanged = true;
        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (brightness > 0.1)
            {
                brightness -= 0.1;
                editBitmap.Lighten(-0.1);
                MainImage.Source = editBitmap;
                vm.ImgChanged = true;
            }
        }


        bool cropfirstclick = true;


        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
    
            //editBitmap= editBitmap.Crop(0,0,100,100);
            //MainImage.Source = editBitmap;

            ////crop
            //RectangleGeometry rect = new RectangleGeometry();
            //rect.Rect = new Rect((Window.Current.Bounds.Width - 480) / 2, (Window.Current.Bounds.Height - 340) / 2, 480, 340);
            //path.Data = rect;

            if (cropfirstclick)
            {

                

                cropfirstclick = false;
            }
            else
            {
                

                var actualwidth=MainImage.ActualWidth;

                var pixelwidth = editBitmap.PixelWidth;


                var ratio = pixelwidth / actualwidth;
                var margin = (MainGrid.ActualWidth - MainImage.ActualWidth) / 2;

                a.X = (a.X - margin) * ratio;
                a.Y = a.Y * ratio;

                b.X = (b.X - margin) * ratio;
                b.Y = b.Y * ratio;
                var rect1 = new Rect(a, b);

                editBitmap = editBitmap.Crop(rect1);
                MainImage.Source = editBitmap;

                a = new Point()
                {
                    X = 0,
                    Y = 0
                };

                b = a;
                rect1 = new Rect(a, b);
                RectangleGeometry rect = new RectangleGeometry();
                rect.Rect = rect1;
                path.Data = rect;
                cropfirstclick = true;
            }

            
            
            vm.ImgChanged = true;
        }

        private bool cropclicked = true;
        Point a, b;
        private void MainImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!cropfirstclick)
            {

                if (cropclicked)
                {

                    a = e.GetPosition(MainGrid);
                    b = a;
                    cropclicked = false;
                }
                else
                {
                    b = e.GetPosition(MainGrid);
                    cropclicked = true;
                    performeDrawRect();
                }
                
            }
            
        }

        private void performeDrawRect()
        {
            RectangleGeometry rect = new RectangleGeometry();
            rect.Rect = new Rect(a, b);
            path.Data = rect;
        }

        private void MainImage_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!cropclicked && !cropfirstclick)
            {
                b = e.GetCurrentPoint(MainGrid).Position;
                performeDrawRect();
            }
           

        } 
    }
}
