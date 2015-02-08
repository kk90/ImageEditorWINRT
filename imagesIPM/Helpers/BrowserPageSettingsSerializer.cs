using imagesIPM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace imagesIPM.Helpers
{
    public class BrowserPageSettingsSerializer
    {
        private IPropertySet settings = ApplicationData.Current.LocalSettings.Values;
        private string vmKey = "IBvm";

        protected object this[string key]
        {
            get { return this.settings[key]; }
            set { this.settings[key] = value; }
        }

        public ImageBrowserViewModel ViewModel
        {
            get
            {
                var serializedViewModel = this[this.vmKey] as string;
                if (string.IsNullOrEmpty(serializedViewModel))
                    return new ImageBrowserViewModel();
                ImageBrowserViewModel vm = new ImageBrowserViewModel();
                try { 
                    vm=JsonConvert.DeserializeObject<ImageBrowserViewModel>(serializedViewModel);
                }
                catch
                {
                    settings.Clear();
                }

                return vm;
            }
            set
            {
                var serializedViewModel = JsonConvert.SerializeObject(value);
                this[this.vmKey] = serializedViewModel;
            }
        }
    }
}
