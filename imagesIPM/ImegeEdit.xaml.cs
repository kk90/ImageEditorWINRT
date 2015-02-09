using imagesIPM.Common;
using imagesIPM.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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


        private async void LoadBaseImage(StorageFile file)
        {
            var bitmap = new BitmapImage();
            var stream = await file.OpenReadAsync();
            await bitmap.SetSourceAsync(stream);
            MainImage.Source = bitmap;
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
            LoadBaseImage(file);
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
        }
    }
}
