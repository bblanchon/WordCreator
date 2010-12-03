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


        ResourceDictionary GetL10nResources()
        {
            return new ResourceDictionary()
            {
                Source = new Uri(string.Format("pack://application:,,,/L10n/Resources.{0}.xaml", System.Threading.Thread.CurrentThread.CurrentUICulture))
            };
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //this.Resources.MergedDictionaries.Clear();
            this.Resources.MergedDictionaries.Add(GetL10nResources());
        }
    }
}
