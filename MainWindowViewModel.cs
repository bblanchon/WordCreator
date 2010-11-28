using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using WordGenerator.Engine;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Collections.Specialized;

namespace WordGenerator
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            AddSourceCommand = new RelayCommand(AddSourceCmdExecuted);

            Sources = new ObservableCollection<WordList>();
            Sources.CollectionChanged += OnSourcesCollectionChanged;

            FindFiles();

            m_engine.Learn(UserWords);
        }


        #region Sources

        public ObservableCollection<WordList> Sources { get; set; }

        void OnSourcesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (WordList list in e.NewItems)
                {
                    m_engine.Learn(list);
                }
            }
        }

        #endregion

        public WordList UserWords;

        #region SuggestedWords

        private IEnumerable<SuggestedWord> m_suggestedWords;

        public IEnumerable<SuggestedWord> SuggestedWords
        {
            set { m_suggestedWords = value; RaisePropertyChanged("SuggestedWords"); }
            get { return m_suggestedWords; }
        }

        #endregion

        #region UserEntry

        public string m_userEntry;

        public string UserEntry
        {
            set { m_userEntry = value; RaiseUserEntryChanged(); }
            get { return m_userEntry; }
        }

        private void RaiseUserEntryChanged()
        {
            OnUserEntryChanged();
            RaisePropertyChanged("UserEntry");
        }

        void OnUserEntryChanged()
        {
            var words = m_engine.Suggest(UserEntry).Where(x => x.Word.Length > 2).Take(500);

            SuggestedWords = words;
        }

        #endregion

        #region Commands

        public ICommand AddSourceCommand { private set; get; }

        private void AddSourceCmdExecuted(object parameter)
        {
            var dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Multiselect = true;

            if (dlg.ShowDialog() == true)
            {
                foreach (var file in dlg.FileNames)
                {
                    var dstFile = Path.Combine(SourceFilesFolder, Path.GetFileName(file));
                    File.Copy(file, dstFile);
                    Sources.Add(new WordList(file));
                }
            }
        }

        #endregion

        #region Folders

        public string BaseFolder
        {
            get { return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WordGenerator"); }
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

        #region Engine

        Generator m_engine = new Generator();

        private void FindFiles()
        {
            UserWords = new WordList(UserWordsFile);

            if (Directory.Exists(SourceFilesFolder))
            {
                var files = Directory.EnumerateFiles(SourceFilesFolder, "*.txt");
                foreach( var file in files ) Sources.Add (new WordList(file));
            }
        }

        #endregion

        #region PropertyChanged

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler  PropertyChanged;

        #endregion
    }
}
