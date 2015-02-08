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
        private ImageBrowserViewModel vm;
        private NavigationHelper navigationHelper;
        private BrowserPageSettingsSerializer Settings = new BrowserPageSettingsSerializer();
        private StorageFile SelectedFile;


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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            SaveState();
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            SaveState();
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            LoadState();
           
        }

        private void LoadState()
        {
            vm = Settings.ViewModel;
            this.DataContext = this.vm;

            //lastPathManager.Paths = new List<string>(viewModel.LastPaths);
        }

        private void SaveState()
        {
            Settings.ViewModel = vm;
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
            if (folder == null)
            {
                //canceled
                return;
            }
            StorageApplicationPermissions.FutureAccessList.Add(folder);
            vm.addPath(folder.Path);
            vm.SelectedPathIndex = vm.LastPaths.Count-1;
        }

        private async void loadThubmadilands()
        {

            if (vm.Folder == null)
                return;
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

            ImageGalleryGrid_SelectionChanged(null, null);
        }



        private void LastPathsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           var aa= (sender as ComboBox).SelectedItem;
           vm.Folder = aa as string;
           loadThubmadilands();
        }

        private async void ImageGalleryGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ImageGalleryGrid.SelectedItem = ImageGalleryGrid.Items[vm.SelectedImageIndex];
            }catch{

            }
                if (ImageGalleryGrid.SelectedItem!=null)
            {
                var Properties = await (ImageGalleryGrid.SelectedItem as Smallmage).File.GetBasicPropertiesAsync();
                vm.Size = Math.Round(Properties.Size / 1024.0, 2).ToString() + " kb";
                vm.Date = Properties.DateModified.ToString();

                SelectedFile = (ImageGalleryGrid.SelectedItem as Smallmage).File;

                vm.Filename = SelectedFile.Name;
                vm.Path = SelectedFile.Path;
                vm.DetailsVisible = true;

                var bitmap = new BitmapImage();
                var stream = await (ImageGalleryGrid.SelectedItem as Smallmage).File.OpenReadAsync();
                await bitmap.SetSourceAsync(stream);

                ImageMain.Source = bitmap;
            }
            else
            {
                SelectedFile = null;
                vm.DetailsVisible = false;
            }
            vm.NotifySelectedFileParams();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //edit

            this.Frame.Navigate(typeof(ImegeEdit), SelectedFile);

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
                var text = dialog.InputText + SelectedFile.FileType;
                await SelectedFile.RenameAsync(text, NameCollisionOption.GenerateUniqueName);
                loadThubmadilands();
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //info
           

        }



    }
}
