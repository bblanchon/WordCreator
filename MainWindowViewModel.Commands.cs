using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace WordCreator
{
    partial class MainWindowViewModel
    {
        private void BuildCommands()
        {
            AddSourceCommand = new RelayCommand(AddSourceCmdExecuted);
            RemoveSourceCommand = new RelayCommand(RemoveSourceCmdExecuted);
            AddUserWordCommand = new RelayCommand(AddUserWordExecuted);
            RemoveUserWordCommand = new RelayCommand(RemoveUserWordExecuted);
            CopyUserWordsCommand = new RelayCommand(CopyUserWordsExecuted, CopyUserWordsCanExecute);
        }
        
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

            foreach (WordList list in selection)
            {
                File.Delete(list.FilePath);
                Sources.Remove(list);
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

        #region Copy user words

        public ICommand CopyUserWordsCommand { private set; get; }

        private bool CopyUserWordsCanExecute(object parameter)
        {
            return UserWords.Words.Count > 0;
        }

        private void CopyUserWordsExecuted(object parameter)
        {
            Clipboard.SetData(DataFormats.Text, string.Join(Environment.NewLine, UserWords.Words));
        }

        #endregion
    }
}
