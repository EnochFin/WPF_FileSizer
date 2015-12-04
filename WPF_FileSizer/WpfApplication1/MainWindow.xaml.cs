using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string path = textDirectory.Text;
            if (Directory.Exists(path))
            {
                fileTreeView.ItemsSource = new MyFileInfo(path).SubFiles;
            }
            else
            {
                List<String> val = new List<string>();
                val.Add("No directory \"" + path + "\" exists");
                fileTreeView.ItemsSource = val;
            }
            
        }

        private void fileTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MyFileInfo selectedItem = fileTreeView.SelectedItem as MyFileInfo;
            if (selectedItem != null)
            {
                editFileCount(selectedItem);
                editFileSize(selectedItem);
                editLastChanged(selectedItem);
            }
        }

        private void editLastChanged(MyFileInfo selectedItem)
        {
            LastEditText.Text = "Last Edited: " + selectedItem.LastEdit.ToString();
        }

        private void editFileSize(MyFileInfo selectedItem)
        {
            BytesToUnitsConverter converter = new BytesToUnitsConverter();
            SizeText.Text = "Size: " + converter.Convert(selectedItem.Size, null, null, null);
        }

        private void editFileCount(MyFileInfo file)
        {
            int fileCount = file.FileCount;
            if (fileCount == 1)
            {
                FileCountText.Text = fileCount + " File";
            }
            else if (fileCount > 1 || fileCount == 0)
            {
                FileCountText.Text = fileCount + " Files";
            }
            else
            {
                FileCountText.Text = "N/A";
            }

        }
        
    }
}
