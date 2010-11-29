using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

namespace WordGenerator
{
    class WordList
    {
        public WordList(string filePath)
        {
            m_filePath = filePath;            
            if( File.Exists(filePath) ) Load();
            else m_words = new ObservableCollection<string>();
        }

        #region FilePath

        private string m_filePath;

        public string FilePath { get { return m_filePath; } }

        public string FileName { get { return Path.GetFileName(m_filePath); } }

        #endregion

        #region Words

        private ObservableCollection<string> m_words;

        public ObservableCollection<string> Words { get{return m_words;}}

        #endregion

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
    }
}
