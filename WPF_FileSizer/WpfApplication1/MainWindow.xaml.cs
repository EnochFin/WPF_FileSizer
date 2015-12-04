using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            fileTreeView.ItemsSource = new MyFileInfo(textDirectory.Text).SubFiles;
        }

        private void fileTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MyFileInfo selectedItem = (MyFileInfo)fileTreeView.SelectedItem;
            editFileCount(selectedItem);
            editFileSize(selectedItem);
            editLastChanged(selectedItem);
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
            int FileCount = file.FileCount;
            if (FileCount == 1)
            {
                FileCountText.Text = FileCount + " File";
            }
            else if (FileCount > 1)
            {
                FileCountText.Text = FileCount + " Files";
            }
            else
            {
                FileCountText.Text = "N/A";
            }

        }
        
    }
}
