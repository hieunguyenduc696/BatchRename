using Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Panel = System.Windows.Controls.Panel;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        List<IRule> _listOfRules;
        BindingList<RenamingRule> _presetRules;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private void configBindingSource()
        {
            _listOfRules = new List<IRule>();
            _presetRules = new BindingList<RenamingRule>();
            loadRules();
            loadPreset();
        }

        private void loadRules()
        {
            // dll change here
            _listOfRules.Add(new AddCounter());
            _listOfRules.Add(new ChangeExtension() { newExtension = "file" });
        }

        private void loadPreset()
        {
            for (int i = 0; i < _listOfRules.Count; i++)
            {
                _presetRules.Add(new RenamingRule() { Name = _listOfRules[i].GetRuleName(), Checked = true });
            }
        }
        private void addFile_Click(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(FileCanvas, 1);
            Panel.SetZIndex(FolderCanvas, 0);
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Multiselect = true;
            if (screen.ShowDialog() == true)
            {
                foreach (var file in screen.FileNames)
                {
                    FileTab.Items.Add(new File()
                    {
                        Filename = System.IO.Path.GetFileName(file),
                        Path = file
                    });
                }
            }
        }

        private void addFolder_Click(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(FolderCanvas, 1);
            Panel.SetZIndex(FileCanvas, 0);
            string directory;
            var screen = new FolderBrowserDialog();
            if (screen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                directory = screen.SelectedPath;
                string[] subDirectory = Directory.GetDirectories(directory);

                foreach (var dir in subDirectory)
                {
                    FolderTab.Items.Add(new Folder()
                    {
                        Foldername = dir.Substring(directory.Length + 1),
                        Path = dir
                    });
                }
            }
        }

        private void startRenaming_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(FileCanvas, 1);
            Panel.SetZIndex(FolderCanvas, 0);
            RenamingRules.ItemsSource = _presetRules;
            configBindingSource();
            RenamingRules.Items.Add(new RenamingRule(){ Name = "HO", Checked = false });
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FileTab.Width = MainPanel.ActualWidth + 20 - 200;
            FolderTab.Width = MainPanel.ActualWidth + 20 - 200;
        }

        private void ConfigRule_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
