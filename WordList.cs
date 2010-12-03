using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WordCreator
{
    class WordList
    {
        public WordList(string filePath)
        {
            m_filePath = filePath;            
            if( File.Exists(filePath) ) Load();
            else m_words = new ObservableCollection<string>();
        }

        #region IsEnabled

        private bool m_isEnabled = true;

        public bool IsEnabled
        {
            get 
            { 
                return m_isEnabled; 
            }
            set
            {
                var oldValue = m_isEnabled;
                m_isEnabled = value;

                if (oldValue == false && value == true && Enabled != null) Enabled(this);

                if (oldValue == true && value == false && Disabled != null) Disabled(this);
            }
        }

        public event Action<WordList> Disabled;
        public event Action<WordList> Enabled;

        #endregion

        #region FilePath

        private string m_filePath;

        public string FilePath { get { return m_filePath; } }

        public string FileName { get { return Path.GetFileName(m_filePath); } }

        #endregion

        #region Words

        private ObservableCollection<string> m_words;

        public ObservableCollection<string> Words { get{return m_words;}}

        #endregion

        #region File load

        const int MIN_WORD_LENGTH = 3;

        private void Load()
        {
            using( var reader = File.OpenText(m_filePath) )
            {                
                var words = EnumerateWords(reader.ReadToEnd()).Where(w=>w.Length>=MIN_WORD_LENGTH);
                m_words = new ObservableCollection<string>(words);
            }
        }

        private IEnumerable<string> EnumerateWords(string source)
        {
            return source.Split(',', ' ', '\t', '\n', '\r', '-', ',', '"', '.', '\'').Select(s => s.ToLower());
        }

        #endregion

        public void Save()
        {
            using (var writer = File.CreateText(m_filePath))
            {
                foreach (var word in m_words)
                    writer.WriteLine(word);
            }
        }
    }
}
