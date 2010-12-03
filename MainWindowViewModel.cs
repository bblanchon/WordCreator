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
    class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        public MainWindowViewModel()
        {
            AddSourceCommand = new RelayCommand(AddSourceCmdExecuted);
            RemoveSourceCommand = new RelayCommand(RemoveSourceCmdExecuted);
            AddUserWordCommand = new RelayCommand(AddUserWordExecuted);
            RemoveUserWordCommand = new RelayCommand(RemoveUserWordExecuted);

            Sources = new ObservableCollection<WordList>();
            Sources.CollectionChanged += OnSourcesCollectionChanged;

            FindFiles();

            UserWords = new WordList(UserWordsFile);
            m_engine.Learn(UserWords.Words);
        }

        public void Dispose()
        {
            UserWords.Save();
        }

        #region Sources

        public ObservableCollection<WordList> Sources { get; set; }

        void OnSourcesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (WordList list in e.NewItems)
                {
                    if (list.IsEnabled) LearnSource(list);
                    list.Enabled += LearnSource;
                    list.Disabled += UnlearnSource;
                }
            }

            if (e.OldItems != null)
            {
                foreach (WordList list in e.OldItems)
                {
                    if (list.IsEnabled) UnlearnSource(list);
                    list.Enabled -= LearnSource;
                    list.Disabled -= UnlearnSource;
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                m_engine.Clear();
            }
        }

        void LearnSource(WordList list)
        {
            m_engine.Learn(list.Words);
            UpdateSuggestedWords();
        }

        void UnlearnSource(WordList list)
        {
            m_engine.Unlearn(list.Words);
            UpdateSuggestedWords();
        }

        #endregion

        public WordList UserWords { private set; get; }

        #region SuggestedWords

        private IEnumerable<SuggestedWord> m_suggestedWords;

        public IEnumerable<SuggestedWord> SuggestedWords
        {
            set { m_suggestedWords = value; RaisePropertyChanged("SuggestedWords"); }
            get { return m_suggestedWords; }
        }

        private void UpdateSuggestedWords()
        {
            if (string.IsNullOrEmpty(UserEntry)) return;

            var words = m_engine.Suggest(UserEntry).Where(x => x.String.Length > 2).Take(300);

            SuggestedWords = words.Select(wp => new SuggestedWord(wp.String, wp.Probability,
                Sources.Any(src => src.Words.Contains(wp.String)),
                UserWords.Words.Contains(wp.String)));
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
            UpdateSuggestedWords();
        }

        #endregion

        #region Commands

        #region Add source

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
                    Sources.Add(new WordList(dstFile));
                }
            }
        }

        #endregion

        #region Remove source

        public ICommand RemoveSourceCommand { private set; get; }
        
        private void RemoveSourceCmdExecuted(object parameter)
        {
            var selection = parameter as IEnumerable<WordList>;

            foreach( WordList list in selection )
            {
                File.Delete(list.FilePath);
                Sources.Remove (list);
            }
        }

        #endregion

        #region Add to user words

        public ICommand AddUserWordCommand { private set; get; }

        private void AddUserWordExecuted(object parameter)
        {
            var word = parameter as string;

            UserWords.Words.Add(word);
            m_engine.Learn(word);

            UpdateSuggestedWords();
        }

        #endregion

        #region Remove user word

        public ICommand RemoveUserWordCommand { private set; get; }

        private void RemoveUserWordExecuted(object parameter)
        {
            var word = parameter as string;

            UserWords.Words.Remove(word);
            m_engine.Unlearn(word);

            UpdateSuggestedWords();
        }

        #endregion

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
