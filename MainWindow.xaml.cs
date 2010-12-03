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
using WordCreator.Engine;
using System.Collections.ObjectModel;

namespace WordCreator
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

            this.DataContext = m_data;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            m_data.Dispose();
            this.DataContext = m_data = null ;
        }

        MainWindowViewModel m_data;

        private void ListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var list = (ListView)sender;

            removeSourceMenuItem.Visibility = list.SelectedItems.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            removeSourceMenuItem.CommandParameter = list.SelectedItems.Cast<WordList>().ToArray();
        }

        private void HandleSuggestedWordDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var word = ((ListViewItem)sender).Content as SuggestedWord;

            if (word.IsInUserWords) m_data.RemoveUserWordCommand.Execute(word.Word);
            else m_data.AddUserWordCommand.Execute(word.Word);
        }
    }
}
