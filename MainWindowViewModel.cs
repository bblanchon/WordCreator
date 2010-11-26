using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using WordGenerator.Engine;
using System.ComponentModel;

namespace WordGenerator
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<WordList> Sources { get; set; }

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

        public event EventHandler UserEntryChanged;

        public string UserEntry
        {
            set { m_userEntry = value; RaiseUserEntryChanged(); }
            get { return m_userEntry; }
        }

        private void RaiseUserEntryChanged()
        {
            if (UserEntryChanged != null) UserEntryChanged(this, EventArgs.Empty);
            RaisePropertyChanged("UserEntry");
        }

        #endregion

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler  PropertyChanged;
    }
}
