using imagesIPM.Common;
using imagesIPM.Helpers;
using imagesIPM.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace imagesIPM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ImageBrowserViewModel vm;
        private NavigationHelper navigationHelper;


        public MainPage()
        {
            this.InitializeComponent();
            vm= new ImageBrowserViewModel();
            this.DataContext = vm;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            App.Current.Suspending += App_Suspending;

        }

        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            
        }

        private void LoadState()
        {
            //vm = Settings.ViewModel;
            //this.DataContext = this.viewModel;

            //lastPathManager.Paths = new List<string>(viewModel.LastPaths);
        }

        private void SaveState()
        {
            //Settings.ViewModel = viewModel;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FolderPicker();
                picker.FileTypeFilter.Add(".bmp");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".gif");
                picker.FileTypeFilter.Add(".png");

            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            var folder = await picker.PickSingleFolderAsync();
            StorageApplicationPermissions.FutureAccessList.Add(folder);
            //this.UpdateLastPaths(folder.Path);
            vm.Folder = folder.Path;
            loadThubmadilands();

        }

        private async void loadThubmadilands()
        {
            ImageGalleryGrid.Items.Clear();

            var folder = await StorageFolder.GetFolderFromPathAsync(vm.Folder);
            var files = await folder.GetFilesAsync();

            foreach (var file in files)
            {
                if (SupportedFiles.IsSupported(file.FileType))
                {
                    var img = new Smallmage(file);

                    ImageGalleryGrid.Items.Add(img);
                }
            }

            
        }



        private void LastPathsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void ImageGalleryGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImageGalleryGrid.SelectedItem!=null)
            {
                vm.Properties = await (ImageGalleryGrid.SelectedItem as Smallmage).File.GetBasicPropertiesAsync();
                vm.SelectedFile = (ImageGalleryGrid.SelectedItem as Smallmage).File;


                var bitmap = new BitmapImage();
                var stream = await (ImageGalleryGrid.SelectedItem as Smallmage).File.OpenReadAsync();
                await bitmap.SetSourceAsync(stream);

                ImageMain.Source = bitmap;
            }
            else
            {
                vm.SelectedFile = null;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //edit

            this.Frame.Navigate(typeof(ImegeEdit), vm.SelectedFile);

        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //remove

            var messageDialog = new Windows.UI.Popups.MessageDialog("Remove?");
             messageDialog.Commands.Add(new UICommand("Yes", async (command) =>
            {
            var file = await StorageFile.GetFileFromPathAsync(vm.Path);
            await file.DeleteAsync();
            loadThubmadilands();
            }));

             messageDialog.Commands.Add(new UICommand("No"));

            await messageDialog.ShowAsync();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {

            var dialog = new InputDialog();
            dialog.InputText = vm.Filename.Split('.')[0];
            var result = await dialog.ShowAsync(
                "Nazwa","",
                "OK",
                "Cancel");

            if (result == "OK")
            {
                var text = dialog.InputText + vm.SelectedFile.FileType;
                await vm.SelectedFile.RenameAsync(text, NameCollisionOption.GenerateUniqueName);
                loadThubmadilands();
            }
        }



    }
}
