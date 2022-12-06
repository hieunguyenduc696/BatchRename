using Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
        BindingList<File> _workingFiles;
        BindingList<Folder> _workingFolders;
        ProgressBarData _progressData = new ProgressBarData();
        RuleFactory _ruleFactory = RuleFactory.Instance();
        BindingList<Parameter> _configRuleParameters;
        bool workingOnFiles = true;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void configBindingSource()
        {
            _listOfRules = new List<IRule>();
            _presetRules = new BindingList<RenamingRule>();
            _workingFiles = new BindingList<File>();
            _workingFolders = new BindingList<Folder>();
            _configRuleParameters = new BindingList<Parameter>();

            RenamingRules.Items.Clear();
            FileTab.Items.Clear();
            FolderTab.Items.Clear();

            loadRules();
            loadPreset();
            resetProgressBar();

            RenamingRules.ItemsSource = _presetRules;
            FileTab.ItemsSource = _workingFiles;
            FolderTab.ItemsSource = _workingFolders;
            ProcessGroupBox.DataContext = _progressData;
        }

        private void resetProgressBar()
        {
            _progressData.CurrentName = "Waiting for new batch";
            _progressData.Current = 0;
            _progressData.Total = 100;
        }
        private void loadRules()
        {
            // dll change here
            _listOfRules.Add(new AddCounter());
            _listOfRules.Add(new ChangeExtension() { NewExtension = "file" });
            RuleFactory.Config(_listOfRules);
        }

        private void sizingDataGrid()
        {
            FileTab.Width = MainPanel.ActualWidth + 20 - 230;
            FolderTab.Width = MainPanel.ActualWidth + 20 - 230;
        }

        private void sizingScreen()
        {
            sizingDataGrid();

            ProcessGroupBox.Width = MainPanel.ActualWidth - ButtonGroupBox.Width - 10;
            FunctionalPanel.Width = ButtonGroupBox.Width + ProcessGroupBox.Width;
            RenamingRules.Height = MainPanel.ActualHeight - 150;


        }

        private void switchWorkingData()
        {
            if (workingOnFiles == true)
            {
                FolderCanvas.Visibility = Visibility.Visible;
                FileCanvas.Visibility = Visibility.Hidden;
                workingOnFiles = false;
            }
            else
            {
                FolderCanvas.Visibility = Visibility.Hidden;
                FileCanvas.Visibility = Visibility.Visible;
                workingOnFiles = true;
            }
            
        }
        private BindingList<Parameter> listToBinding(List<Parameter> list)
        {
            BindingList<Parameter> bList = new BindingList<Parameter>();
            foreach(Parameter parameter in list)
            {
                bList.Add(parameter);
            }

            return bList;
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
            workingOnFiles = false;
            switchWorkingData();
            //Panel.SetZIndex(FileCanvas, 1);
            //Panel.SetZIndex(FolderCanvas, 0);
            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Multiselect = true;
            if (screen.ShowDialog() == true)
            {
                //_workingFiles.Clear();
                foreach (var file in screen.FileNames)
                {
                    _workingFiles.Add(new File()
                    {
                        Filename = System.IO.Path.GetFileName(file),
                        Path = file
                    });
                }
                sizingDataGrid();
            }
        }

        private void addFolder_Click(object sender, RoutedEventArgs e)
        {
            workingOnFiles = true;
            switchWorkingData();
            //Panel.SetZIndex(FolderCanvas, 1);
            //Panel.SetZIndex(FileCanvas, 0);
            string directory;
            var screen = new FolderBrowserDialog();
            if (screen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                directory = screen.SelectedPath;
                string[] subDirectory = Directory.GetDirectories(directory);

                //_workingFolders.Clear();
                foreach (var dir in subDirectory)
                {
                    _workingFolders.Add(new Folder()
                    {
                        Foldername = dir.Substring(directory.Length + 1),
                        Path = dir
                    });
                }
                sizingDataGrid();
            }
        }

        private void startRenaming_Click(object sender, RoutedEventArgs e)
        {
            if (_workingFiles.Count != 0 )
            {
                Clear.IsEnabled = false;
                _progressData.setNewProgress(0, "Starting new batch: ", 100);

                for (int i = 0; i < _workingFiles.Count; i++)
                {
                    string newName = "";
                    _progressData.setNewProgress(i + 1, _workingFiles[i].Filename, _workingFiles.Count);
                    for (int j = 0; j < _presetRules.Count; j++)
                    {
                        IRule rule = _ruleFactory.CreateRule(_presetRules[j].Name);
                        rule.iterateConfig(j);

                        if (_presetRules[j].Checked == true)
                        {
                            try
                            {
                                newName = rule.Rename(_workingFiles[i].Filename);
                            }
                            catch (Exception exception)
                            {
                                _workingFiles[i].Erro = $"Error at rule: {_presetRules[j].Name}";
                                break;
                            }
                        }
                    }
                    _workingFiles[i].Newfilename = newName;

                }

                _progressData.setNewProgress(_workingFiles.Count, "Finishing renaming", _workingFiles.Count);
                _progressData.setNewProgress(_workingFiles.Count, "All done", _workingFiles.Count);
                System.Windows.MessageBox.Show("Your work is completed!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                _progressData.setNewProgress(0, "Waiting for new batch", _workingFiles.Count);
                Clear.IsEnabled = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Please add some files/folders", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FolderCanvas.Visibility = Visibility.Hidden;
            FileCanvas.Visibility = Visibility.Visible;
            //Panel.SetZIndex(FileCanvas, 1);
            //Panel.SetZIndex(FolderCanvas, 0);
            sizingScreen();

            configBindingSource();
   

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sizingScreen();
        }

        private void ConfigRule_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = e.Source as System.Windows.Controls.Button;
            RenamingRule presetRule = btn.DataContext as RenamingRule;

            string ruleName = presetRule.Name;

            IRule curRule = _ruleFactory.CreateRule(ruleName);
            this._configRuleParameters = this.listToBinding(curRule.GetParameters());;

            var screen = new ConfigRule(ruleName, this._configRuleParameters);
            screen.Owner = this;

            if (screen.ShowDialog() == true)
            {
                for (int i = 0; i < screen._configRuleParameters.Count; i++)
                {
                    curRule.UpdateConfigParameters(screen._configRuleParameters.ToList<Parameter>());
                    RuleFactory.updateConfig(ruleName, curRule);
                }
            }
        }

        private void clearData_Click(object sender, RoutedEventArgs e)
        {
            if (workingOnFiles == true)
            {
                _workingFiles.Clear();
            }
            else
            {
                _workingFolders.Clear();
            }
        }

        private void switchData_Click(object sender, RoutedEventArgs e)
        {
            switchWorkingData();
        }
    }
}
