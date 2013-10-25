using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace WordCreator
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }


        void LoadL10nResources()
        {
            var uri = new Uri(string.Format("pack://application:,,,/L10n/Resources.{0}.xaml", System.Threading.Thread.CurrentThread.CurrentUICulture));
            var dictionary = new ResourceDictionary { Source = uri };
            this.Resources.MergedDictionaries.Add(dictionary);            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                LoadL10nResources();
            }
            catch
            {
                // go chance that the resources doesn't exist
            }
        }
    }
}
