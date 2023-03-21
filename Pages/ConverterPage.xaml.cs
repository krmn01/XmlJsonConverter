using System.Windows.Controls;
using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using XmlJsonConverter.Core;

namespace XmlJsonConverter
{
    /// <summary>
    /// Logika interakcji dla klasy ConverterPage.xaml
    /// </summary>
    public partial class ConverterPage : Page
    {
        public ConverterPage()
        {
            InitializeComponent();

            DataContext = new ConverterPageViewModel();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                path.Text = Path.GetFullPath(openFileDialog.FileName);
        }
    }
}
