using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using WordGenerator.Engine;
using System.Collections.ObjectModel;

namespace WordGenerator
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            m_data = new MainWindowViewModel();
            m_data.UserEntryChanged += OnUserEntryChanged;

            FindFiles();           
            Learn();

            this.DataContext = m_data;
        }

        MainWindowViewModel m_data;

        #region Folders

        public string BaseFolder
        {
            get { return System.IO.Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WordGenerator"); }
        }

        public string UserWordsFile
        {
            get { return System.IO.Path.Combine(BaseFolder, "mywords.txt"); }
        }

        public string SourceFilesFolder
        {
            get { return System.IO.Path.Combine(BaseFolder, "sources"); }
        }

        #endregion

        Generator m_engine = new Generator();

        private void FindFiles()
        {
            m_data.UserWords = new WordList(UserWordsFile);

            if (Directory.Exists(SourceFilesFolder))
            {
                var files = Directory.EnumerateFiles(SourceFilesFolder, "*.txt");
                m_data.Sources = new ObservableCollection<WordList>(files.Select(file => new WordList(file)));
            }
        }

        private void Learn()
        {
            foreach (var source in m_data.Sources)
                m_engine.Learn(source);
            m_engine.Learn(m_data.UserWords);
        }

        void OnUserEntryChanged(object sender, EventArgs e)
        {
            var words = m_engine.Suggest(m_data.UserEntry).Where(x => x.Length > 2).Take(500);

            m_data.SuggestedWords = words;
        }
    }
}
