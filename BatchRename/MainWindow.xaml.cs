using Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

using DragDropEffects = System.Windows.DragDropEffects;
using ListViewItem = System.Windows.Controls.ListViewItem;


namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        List<Rule.IRule> _listOfRules;

        BindingList<RenamingRule> _presetRules;
        BindingList<Item> _workingFiles;
        BindingList<Item> _workingFolders;
        ProgressBarData _progressData = new ProgressBarData();
        RuleFactory _ruleFactory = RuleFactory.Instance();
        BindingList<Rule.Parameter> _configRuleParameters;

        bool filePreviewMode = false;
        bool folderPreviewMode = false;
        bool workingOnFiles = true;
        BindingList<PresetWorker> _listOfPresets;
        Dictionary<string, PresetWorker> _dictionaryPresets;
        string curPresetSelectedName = "none";

        //ViewModel myViewModel = new ViewModel();
        ViewModel fileViewModel = new ViewModel("file");
        ViewModel folderViewModel = new ViewModel("folder");

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void configBindingSource()
        {
            // View model setup
            //myViewModel.Setup(new List<Object>(), 1, 10, "file");
            //myViewModel.Setup(new List<Object>(), 1, 10, "folder");
            fileViewModel.Setup(new List<Item>(), 1, 10);
            folderViewModel.Setup(new List<Item>(), 1, 10);

            // data config
            _listOfRules = new List<Rule.IRule>();
            _presetRules = new BindingList<RenamingRule>();
            _workingFiles = new BindingList<Item>();
            _workingFolders = new BindingList<Item>();
            _configRuleParameters = new BindingList<Rule.Parameter>();

          

            //_dictionaryOfRules = new Dictionary<string, Rule.IRule>();

            // reseting items for binding
            RenamingRules.Items.Clear();
            //FileTab.Items.Clear();
            //FolderTab.Items.Clear();

            loadRules();
            loadPreset();
            resetProgressBar();

            RenamingRules.ItemsSource = _presetRules;
            FileTab.ItemsSource = _workingFiles;
            FolderTab.ItemsSource = _workingFolders;
            ProcessGroupBox.DataContext = _progressData;

            Pagination.DataContext = fileViewModel; // default file view
            fileViewModel.CurPage = fileViewModel.TotalPages = 1;

            PresetMenu.ItemsSource = _listOfPresets;
            PresetMenu.SelectedItem = _dictionaryPresets["default"];
        }

        private void generateDefaultPreset()
        {
            string directory = @"Presets";
            string location = @"Presets\default.txt";
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            using (StreamWriter sw = System.IO.File.CreateText(location))
            {
                for (int i = 0; i < _listOfRules.Count; i++)
                {
                    if (i != _listOfRules.Count - 1)
                    {
                        sw.WriteLine(_listOfRules[i].ParseConfigToString());
                    }
                    else
                    {
                        sw.Write(_listOfRules[i].ParseConfigToString());
                    }
                }
            };
        }
        private void parsePreset()
        {

            foreach(RenamingRule rule in _presetRules)
            {
                try
                {
                    if (curPresetSelectedName != "none")
                    {
                        Dictionary<string, bool> cur = _dictionaryPresets[curPresetSelectedName].checkedList;
                        if (cur[rule.Name] == true)
                        {
                            rule.Checked = true;
                        }
                        else rule.Checked = false;
                    }
                    Rule.IRule newRule = _ruleFactory.CreateRule(rule.Name);
                    newRule.ParseConfigFromString(rule.StringParameters);
                    RuleFactory.updateConfig(newRule.GetRuleName(), newRule);
                }
                catch (Exception exception)
                {

                }
            }
        }

        private BindingList<RenamingRule> parseRulePresetsFromFile(string location)
        {
            BindingList<RenamingRule> preset = new BindingList<RenamingRule>();
                List<string> lines = System.IO.File.ReadLines(location).ToList();
                int totalLines = lines.Count();
                foreach (string line in lines)
                {
                    string[] splitLine = line.Split("@val@");
                    string ruleName = splitLine[0];
                    try
                    {
                        int idx = 0;
                        for (int i = 0; i < _listOfRules.Count; i++)
                        {
                            if (ruleName == _listOfRules[i].GetRuleName())
                            {
                                idx++;
                            }
                        }
                        if (idx != 0)
                        {
                         preset.Add(new RenamingRule() { Name = ruleName, Checked = false, StringParameters = line });
                        }
                        
                    }
                    catch (Exception exception)
                    {

                    }
                }
            return preset;
        }

        private Dictionary<string,bool> generateCheckedRuleList(BindingList<RenamingRule> input)
        {
            Dictionary<string,bool> result = new Dictionary<string,bool>();
            foreach (RenamingRule r in input)
            {
                result.Add(r.Name, false);
            }
            return result;
        }
        private void loadDefaultPreset()
        {
            string location = @"Presets\default.txt";
            if (!System.IO.File.Exists(location))
            {
                generateDefaultPreset();
            }
            
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
            //_listOfRules.Add(new AddCounter());
            //_listOfRules.Add(new ChangeExtension() { NewExtension = "file" });

            foreach (string file in Directory.EnumerateFiles(@"Rules", "*.dll"))
            {
                string[] splitPath = file.Split("\\");
                string fileName = splitPath[splitPath.Length - 1];
                string className = fileName.Substring(0, fileName.Length - 4);

                var assembly = Assembly.LoadFrom(file);
                var types = assembly.GetTypes();
                
                foreach(var type in types)
                {
                    if (type.IsClass && typeof(Rule.IRule).IsAssignableFrom(type))
                    {
                        Rule.IRule rule = (Rule.IRule)Activator.CreateInstance(type);
                        _listOfRules.Add(rule);
                    }
                }

            }

            //foreach (Rule.IRule r in _listOfRules)
            //{
            //    _dictionaryOfRules.Add(r.GetRuleName(), r);
            //}
            RuleFactory.Config(_listOfRules);
        }

        private void sizingDataGrid()
        {
            FileTab.Width = MainPanel.ActualWidth + 20 - 230;
            FolderTab.Width = MainPanel.ActualWidth + 20 - 230;

            _workingFiles = new BindingList<Item>(fileViewModel.updatePerPage((int)(MainPanel.ActualHeight / 19 - 11)));
            _workingFolders = new BindingList<Item>(folderViewModel.updatePerPage((int)(MainPanel.ActualHeight / 19 - 11)));

            if (workingOnFiles)
            {
                setupEnableChangePageBtn(fileViewModel);
            }
            else setupEnableChangePageBtn(folderViewModel);


            FileTab.ItemsSource = _workingFiles;
            FolderTab.ItemsSource = _workingFolders;
        }

        private void sizingScreen()
        {
            sizingDataGrid();

            ProcessGroupBox.Width = MainPanel.ActualWidth - ButtonGroupBox.Width - 10;
            FunctionalPanel.Width = ButtonGroupBox.Width + ProcessGroupBox.Width;
            Pagination.Margin = new Thickness((MainPanel.ActualWidth) / 2 - 160, (MainPanel.ActualHeight) - 164, 0 , 0);
            RenamingRules.Height = MainPanel.ActualHeight - 210;


        }

        private void switchWorkingData()
        {
            if (workingOnFiles == true)
            {
                //FolderCanvas.Visibility = Visibility.Visible;
                //FileCanvas.Visibility = Visibility.Hidden;
                FileTab.Visibility = Visibility.Hidden;
                FolderTab.Visibility = Visibility.Visible;

                setupEnableChangePageBtn(folderViewModel);
                Pagination.DataContext = folderViewModel;
                
                workingOnFiles = false;
            }
            else
            {
                //FolderCanvas.Visibility = Visibility.Hidden;
                //FileCanvas.Visibility = Visibility.Visible;
                FileTab.Visibility = Visibility.Visible;
                FolderTab.Visibility = Visibility.Hidden;

                setupEnableChangePageBtn(fileViewModel);
                Pagination.DataContext = fileViewModel;

                workingOnFiles = true;
            }
            
        }

        private BindingList<Rule.Parameter> listToBinding(List<Rule.Parameter> list)
        {
            BindingList<Rule.Parameter> bList = new BindingList<Rule.Parameter>();
            foreach(Rule.Parameter parameter in list)
            {
                bList.Add(parameter);
            }

            return bList;
        }


        private void loadPreset()
        {
            _listOfPresets = new BindingList<PresetWorker>();
            _dictionaryPresets = new Dictionary<string, PresetWorker>();
            loadDefaultPreset();
            foreach (string file in Directory.EnumerateFiles(@"Presets", "*.txt"))
            {
                string[] splitPath = file.Split("\\");
                string fileName = splitPath[splitPath.Length - 1];
                string presetName = fileName.Substring(0, fileName.Length - 4);

                BindingList<RenamingRule> _preset = new BindingList<RenamingRule>();
                Dictionary<string, bool> _checkedList = new Dictionary<string, bool>();
                _preset = parseRulePresetsFromFile(file);
                _checkedList = generateCheckedRuleList(_preset);
                _listOfPresets.Add(new PresetWorker() { Name = presetName, PresetRules = _preset, checkedList = _checkedList });
            }

            foreach(PresetWorker preset in _listOfPresets)
            {
                _dictionaryPresets.Add(preset.Name, preset);
            }

            ChangePreset("default");
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
                    fileViewModel.addItemPage(new Item()
                    {
                        Name = System.IO.Path.GetFileName(file),
                        Path = file
                    });

                    _workingFiles = new BindingList<Item>(fileViewModel.selectedItems);
                    FileTab.ItemsSource = _workingFiles;
     

                    setupEnableChangePageBtn(fileViewModel);

                }
                sizingDataGrid();
            }
        }

        private void addFolder_Click(object sender, RoutedEventArgs e)
        {
            workingOnFiles = true;
            switchWorkingData();

            string directory = "";
            var screen = new FolderBrowserDialog();
            
            if (screen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                directory = screen.SelectedPath;
                string[] subDirectory = Directory.GetDirectories(directory);

                //_workingFolders.Clear();
                foreach (var dir in subDirectory)
                {

                    folderViewModel.addItemPage(new Item()
                    {
                        Name = dir.Substring(directory.Length + 1),
                        Path = dir
                    });


                    _workingFolders = new BindingList<Item>(folderViewModel.selectedItems);
                    FolderTab.ItemsSource = _workingFolders;


                    setupEnableChangePageBtn(folderViewModel);

                }
                sizingDataGrid();
            }
        }

        private void ChangePreset(string _presetname)
        {
            _presetRules = _dictionaryPresets["default"].PresetRules;
            parsePreset();
        }
        private void previewRenaming_Click(object sender, RoutedEventArgs e)
        {
            if (workingOnFiles == true)
            {
                int totalItem = fileViewModel.items.Count;
                if (totalItem != 0)
                {
                    Clear.IsEnabled = false;
                    _progressData.setNewProgress(0, "Starting new batch: ", 100);
                    try
                    {
                        for (int i = 0; i < totalItem; i++)
                        {
                            string newName = "";
                            _progressData.setNewProgress(i + 1, fileViewModel.items[i].Name, totalItem);
                            for (int j = 0; j < _presetRules.Count; j++)
                            {
                                Rule.IRule rule = _ruleFactory.CreateRule(_presetRules[j].Name);
                                rule.iterateConfig(i);

                                if (_presetRules[j].Checked == true)
                                {
                                    filePreviewMode = true;
                                    try
                                    {
                                        if (newName == "")
                                        {
                                            newName = rule.Rename(fileViewModel.items[i].Name,"file");
                                        }
                                        else newName = rule.Rename(newName,"file");
                                    }
                                    catch (Exception exception)
                                    {
                                        fileViewModel.items[i].Erro += $"error at rule: {_presetRules[j].Name}, ";
                                        break;
                                    }
                                }
                            }
                            try
                            {
                                string str = fileViewModel.items[i].Erro.Substring(0, 5);
                                if (str != "error")
                                {
                                    fileViewModel.items[i].Erro = $"Done preview result";
                                }
                            }
                            catch(Exception exc)
                            {
                                fileViewModel.items[i].Erro = $"Done preview result";
                            }
                            

                            fileViewModel.items[i].NewName = newName;
                            _progressData.setNewProgress(i + 1, "Finished renaming", totalItem);

                            updatePage(fileViewModel);

                        }

                    }
                    catch (Exception exception)
                    {

                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Please add some files/folders", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                _progressData.setNewProgress(totalItem, "Done preview for this batch", totalItem);
                Clear.IsEnabled = true;
            }
            else
            {
                int totalItem = _workingFolders.Count;
                if (totalItem != 0)
                {
                    Clear.IsEnabled = false;
                    _progressData.setNewProgress(0, "Starting new batch: ", 100);
                    try
                    {
                        for (int i = 0; i < totalItem; i++)
                        {
                            string newName = "";
                            _progressData.setNewProgress(i + 1, folderViewModel.items[i].Name, totalItem);
                            for (int j = 0; j < _presetRules.Count; j++)
                            {
                                Rule.IRule rule = _ruleFactory.CreateRule(_presetRules[j].Name);
                                rule.iterateConfig(i);

                                if (_presetRules[j].Checked == true)
                                {
                                    folderPreviewMode = true;
                                    try
                                    {
                                        if (newName == "")
                                        {
                                            newName = rule.Rename(folderViewModel.items[i].Name,"folder");
                                        }
                                        else newName = rule.Rename(newName,"folder");
                                    }
                                    catch (Exception exception)
                                    {
                                        folderViewModel.items[i].Erro = $"Error at rule: {_presetRules[j].Name}";
                                        break;
                                    }
                                }
                            }

                            try
                            {
                                string str = folderViewModel.items[i].Erro.Substring(0, 5);
                                if (str != "error")
                                {
                                    folderViewModel.items[i].Erro = $"Done preview result";
                                }
                            }
                            catch (Exception exc)
                            {
                                folderViewModel.items[i].Erro = $"Done preview result";
                            }

                            folderViewModel.items[i].NewName = newName;
                            _progressData.setNewProgress(i + 1, "Finishied renaming", totalItem);

                            updatePage(folderViewModel);
                        }
                    }
                    catch (Exception exception)
                    {

                    }
                    _progressData.setNewProgress(totalItem, "Done preview for this batch", totalItem);
                    Clear.IsEnabled = true;
                }
                else
                {
                    System.Windows.MessageBox.Show("Please add some files/folders", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            //System.Windows.MessageBox.Show("Your work is completed!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FolderTab.Visibility = Visibility.Hidden;
            FileTab.Visibility = Visibility.Visible;
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

            Rule.IRule curRule = _ruleFactory.CreateRule(ruleName);
            this._configRuleParameters = this.listToBinding(curRule.GetParameters());;

            var screen = new ConfigRule(ruleName, this._configRuleParameters);
            screen.Owner = this;

            if (screen.ShowDialog() == true)
            {
                BindingList<RenamingRule> _activePresetData = _dictionaryPresets[curPresetSelectedName].PresetRules;
                int targetIdx = -1;
                for (int j = 0; j < _activePresetData.Count; j++)
                {
                    if (_activePresetData[j].Name == ruleName)
                    {
                        targetIdx = j;
                        break;
                    }

                }
                for (int i = 0; i < screen._configRuleParameters.Count; i++)
                {

                    curRule.UpdateConfigParameters(screen._configRuleParameters.ToList<Rule.Parameter>());
                    RuleFactory.updateConfig(ruleName, curRule);
                    _dictionaryPresets[curPresetSelectedName].PresetRules[targetIdx].StringParameters = curRule.ParseConfigToString();
                }
            }
        }

        private void clearData_Click(object sender, RoutedEventArgs e)
        {
            if (workingOnFiles == true)
            {
                fileViewModel = new ViewModel();
                fileViewModel.Setup(new List<Item>(), 1, 10);
                fileViewModel.CurPage = fileViewModel.TotalPages = 1;
                _workingFiles.Clear();
                filePreviewMode = false;
            }
            else
            {
                folderViewModel = new ViewModel();
                folderViewModel.Setup(new List<Item>(), 1, 10);
                folderViewModel.CurPage = folderViewModel.TotalPages = 1;
                _workingFolders.Clear();
                folderPreviewMode = false;
            }
        }

        private void switchData_Click(object sender, RoutedEventArgs e)
        {
            switchWorkingData();
        }

        private void PresetItem_Changed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                PresetWorker item = (PresetWorker)PresetMenu.SelectedItem;
                _presetRules = _dictionaryPresets[item.Name].PresetRules;
                curPresetSelectedName = item.Name;
                parsePreset();
                RenamingRules.ItemsSource = null;
                RenamingRules.ItemsSource = _presetRules;

            } catch (Exception error)
            {
                _presetRules = _dictionaryPresets["default"].PresetRules;
                PresetMenu.SelectedItem = _dictionaryPresets["default"];
            }
        }

        private void RuleChecked_Tick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox cb = e.Source as System.Windows.Controls.CheckBox;
            RenamingRule presetRule = cb.DataContext as RenamingRule;
            _dictionaryPresets[curPresetSelectedName].checkedList[presetRule.Name] = true;
            RenamingRules.ItemsSource = null;
            RenamingRules.ItemsSource = _presetRules;
        }

        private void RuleUnChecked_Tick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox cb = e.Source as System.Windows.Controls.CheckBox;
            RenamingRule presetRule = cb.DataContext as RenamingRule;
            _dictionaryPresets[curPresetSelectedName].checkedList[presetRule.Name] = false;
            RenamingRules.ItemsSource = null;
            RenamingRules.ItemsSource = _presetRules;
        }

        private void NewPreset_Click(object sender, RoutedEventArgs e)
        {
            var screen = new NewRule(_listOfRules,"Create new rule");
            screen.Owner = this;

            if (screen.ShowDialog() == true)
            {
                string presetName = screen.Name;
                BindingList<RenamingRule> selectedRules = screen.RulesSelected;
                if (presetName == "" || _dictionaryPresets.ContainsKey(presetName))
                {
                    System.Windows.MessageBox.Show("Name cant be duplicate or empty!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    string directory = @"Presets";
                    if (!System.IO.Directory.Exists(directory))
                    {
                        System.IO.Directory.CreateDirectory(directory);
                    }
                    using (StreamWriter sw = System.IO.File.CreateText($@"Presets\{presetName}.txt"))
                    {
                        for (int i = 0; i < selectedRules.Count; i++)
                        {
                            if (i != selectedRules.Count - 1)
                            {
                                if (selectedRules[i].Checked == true)
                                {
                                    sw.WriteLine(_listOfRules[i].ParseConfigToString());
                                }
                                
                            }
                            else
                            {
                                if (selectedRules[i].Checked == true)
                                {
                                    sw.Write(_listOfRules[i].ParseConfigToString());
                                }
                                
                            }
                        }
                    };

                    _listOfPresets.Add(new PresetWorker() { Name = presetName });
                    BindingList<RenamingRule> _presetRules = new BindingList<RenamingRule>();
                    for (int i = 0; i < selectedRules.Count; i++)
                    {
                        if (selectedRules[i].Checked == true)
                        {
                            string a = _listOfRules[i].ParseConfigToString();
                            _presetRules.Add(new RenamingRule() { Name = _listOfRules[i].GetRuleName(), Checked = false, StringParameters = _listOfRules[i].ParseConfigToString() });
                        }
                        
                    }
                    _dictionaryPresets.Add(presetName, new PresetWorker() { Name = presetName, PresetRules = _presetRules, checkedList = generateCheckedRuleList(_presetRules) });

                    PresetMenu.SelectedItem = _listOfPresets[_listOfPresets.Count - 1];
                }
            }
        }

        private void UpdatePreset_Rule(PresetWorker curPreset, NewRule screen) 
        {
            for (int i = 0; i < curPreset.PresetRules.Count; i++)
            {
                int count = 0;
                for (int j = 0; j < screen.RulesSelected.Count; j++)
                {
                    if (screen.RulesSelected[j].Name == curPreset.PresetRules[i].Name && screen.RulesSelected[j].Checked == true)
                    {
                        count++;
                    }
                }
                if (count == 0)
                {
                    curPreset.PresetRules.RemoveAt(i);
                    i--;
                }

            }
            for (int i = 0; i < screen.RulesSelected.Count; i++)
            {
                int count = 0;
                if (screen.RulesSelected[i].Checked == true)
                {
                    for (int j = 0; j < curPreset.PresetRules.Count; j++)
                    {
                        if (screen.RulesSelected[i].Name == curPreset.PresetRules[j].Name)
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        curPreset.PresetRules.Add(new RenamingRule() { Name = screen.RulesSelected[i].Name, Checked = false, StringParameters = _ruleFactory.CreateRule(screen.RulesSelected[i].Name).ParseConfigToString() });
                    }
                }

            }
            curPreset.checkedList = generateCheckedRuleList(curPreset.PresetRules);
            int idx = -1;
            for (int i = 0; i < _listOfPresets.Count; i++)
            {
                if (_listOfPresets[i].Name == curPresetSelectedName)
                {
                    idx = i;
                    break;
                }
            }
            _listOfPresets[idx] = curPreset;

            System.Windows.MessageBox.Show("Done updating preset", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void UpdatePreset_Click(object sender, RoutedEventArgs e)
        {
            // doi ten file
            // doi trong dictionary preset
            // doi trong list of preset
            // doi trong cur preset

            var screen = new NewRule(_listOfRules, "Update rule");
            
            screen.Owner = this;
            PresetWorker curPreset = _dictionaryPresets[curPresetSelectedName];
            screen.Name = curPresetSelectedName;
            screen.setDefaulInput();
            for (int i = 0; i < screen.RulesSelected.Count;i++)
            {
                for (int j = 0; j < curPreset.PresetRules.Count;j++)
                {
                    if (curPreset.PresetRules[j].Name == screen.RulesSelected[i].Name)
                    {
                        screen.RulesSelected[i].Checked = true;
                    }
                }
            }

            if (screen.ShowDialog() == true)
            {
                string newName = screen.Name;
                try
                {
                    // file ton tai
                    if (newName == "")
                    {
                        System.Windows.MessageBox.Show("Name cant be default or can't rename default preset", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else if (newName == "default" || curPresetSelectedName == "default")
                    {
                        System.Windows.MessageBox.Show("Name cant be default or can't rename default preset", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else if (newName == curPresetSelectedName)
                    {
                        UpdatePreset_Rule(curPreset, screen);
                    }
                    else if (_dictionaryPresets.ContainsKey(newName) == false)
                    {
                        string directory = @"Presets";
                        string oldLocation = $@"Presets\{curPresetSelectedName}.txt";
                        string newLocation = $@"Presets\{newName}.txt";
                        if (!System.IO.Directory.Exists(directory))
                        {
                            System.IO.Directory.CreateDirectory(directory);
                        }
                        if (System.IO.File.Exists(oldLocation))
                        {
                            System.IO.File.Move(oldLocation, newLocation);
                        }
                        else
                        {
                            throw new Exception();
                        }

                        int idx = -1;
                        for (int i = 0; i < _listOfPresets.Count; i++)
                        {
                            if (_listOfPresets[i].Name == curPresetSelectedName)
                            {
                                idx = i;
                                break;
                            }
                        }

                        curPreset.Name = newName;
                        _dictionaryPresets.Add(newName, curPreset);
                        _dictionaryPresets.Remove(curPresetSelectedName);
                        curPresetSelectedName = newName;
                        curPreset = _dictionaryPresets[curPresetSelectedName];
                        UpdatePreset_Rule(curPreset, screen);

                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Name cant be duplicate to other presets", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }

                }
                catch (Exception exception)
                {
                    System.Windows.MessageBox.Show("Preset is no longer exists in local data, please save first", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }          
            }

        }

        private void SavePreset_Click(object sender, RoutedEventArgs e)
        {
            // Parse config thanh du lieu
            // Save xuong file voi ten selected
            // + File ko ton tai => create new
            // + File ton tai => overwrite
            string directory = @"Presets";
            string location = $@"Presets\{curPresetSelectedName}.txt";
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            PresetWorker selected = _dictionaryPresets[curPresetSelectedName];
            selected.ParseConfigToFile(location);
        }

        private void removePreset(string curPreset)
        {
            int idx = -1;
            for (int i = 0; i < _listOfPresets.Count;i++)
            {
                if (_listOfPresets[i].Name == curPresetSelectedName)
                {
                    idx = i;
                    break;
                }
            }
            _listOfPresets.RemoveAt(idx);
            _dictionaryPresets.Remove(curPreset);
            PresetMenu.SelectedItem = _dictionaryPresets["default"];
            curPresetSelectedName = "default";
        }
        private void DeletePreset_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dialogResult = System.Windows.Forms.MessageBox.Show($"Are you sure want to delete {curPresetSelectedName} preset ?", "Notification",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {

                string curPreset = curPresetSelectedName;
                string location = $@"Presets\{curPreset}.txt";
                if (curPresetSelectedName == "default")
                {
                    System.Windows.MessageBox.Show($"Cannot delete {curPreset}", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                try
                {
                    if (!System.IO.File.Exists(location))
                    {
                        removePreset(curPreset);
                        System.Windows.MessageBox.Show("Preset has already been deleted from local data", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        System.IO.File.Delete(location);
                        removePreset(curPreset);
                        System.Windows.MessageBox.Show($"Done deleting preset {curPreset}", "Notification", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                catch (Exception exception)
                {
                    System.Windows.MessageBox.Show($"Failed to delete preset {curPreset}", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                
            }
            
        }


        private void Drop_Handler(object sender, System.Windows.DragEventArgs e)
        {
            RenamingRule droppedData = e.Data.GetData(typeof(RenamingRule)) as RenamingRule;
            RenamingRule target = ((ListViewItem)(sender)).DataContext as RenamingRule;

            int targetIdx = RenamingRules.Items.IndexOf(target);
            int droppedIdx = RenamingRules.Items.IndexOf(droppedData);

            RenamingRule temp = _presetRules[droppedIdx];
            _presetRules[droppedIdx] = _presetRules[targetIdx];
            _presetRules[targetIdx] = temp;

            RenamingRules.ItemsSource = null;
            RenamingRules.ItemsSource = _presetRules;
        }

        private void Drag_Handler(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is ListViewItem && e.LeftButton == MouseButtonState.Pressed)
            {
                ListViewItem draggedItem = sender as ListViewItem;

                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);

            }
        }
        
        private void setupEnableChangePageBtn(ViewModel viewMd)
        {
            if (viewMd.items.Count == 0)
            {
                viewMd.CurPage = viewMd.TotalPages = 1;
            }
            if (viewMd.TotalPages == 1)
            {
                PrevPage_Btn.IsEnabled = false;
                NextPage_Btn.IsEnabled = false;
            }
            else if (viewMd.CurPage == 1)
            {
                PrevPage_Btn.IsEnabled = false;
                NextPage_Btn.IsEnabled = true;
            }

            else if (viewMd.CurPage == viewMd.TotalPages)
            {
                NextPage_Btn.IsEnabled = false;
                PrevPage_Btn.IsEnabled = true;
            }
            else
            {
                NextPage_Btn.IsEnabled = true;
                PrevPage_Btn.IsEnabled = true;
            }
        }
        private void changePageViewModelSetup(ViewModel viewMd, string updateAction)
        {
            int newPage = 1;
            if (updateAction == "prev")
            {
                newPage = viewMd.CurPage - 1;
            }
            else
            {
                newPage = viewMd.CurPage + 1;
            }

            viewMd.changeItemPage(newPage);

            if (viewMd.Type == "file")
            {

                _workingFiles = new BindingList<Item>(viewMd.selectedItems);
                FileTab.ItemsSource = _workingFiles;

            }
            else
            {
                _workingFolders = new BindingList<Item>(viewMd.selectedItems);
                FolderTab.ItemsSource = _workingFolders;
            }

            setupEnableChangePageBtn(viewMd);

        }
        private void PrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (workingOnFiles)
            {
                changePageViewModelSetup(fileViewModel, "prev");
            }
            else
            {
                changePageViewModelSetup(folderViewModel, "prev");
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (workingOnFiles)
            {
                changePageViewModelSetup(fileViewModel, "next");
            }
            else
            {
 
                changePageViewModelSetup(folderViewModel, "next");
            }

        }

        private void updatePage(ViewModel viewMd)
        {
            if (viewMd.Type == "file")
            {
                fileViewModel.changeItemPage(viewMd.CurPage);
                _workingFiles = new BindingList<Item>(viewMd.selectedItems);
                FileTab.ItemsSource = _workingFiles;
            }
            else
            {
                folderViewModel.changeItemPage(viewMd.CurPage);
                _workingFolders = new BindingList<Item>(viewMd.selectedItems);
                FolderTab.ItemsSource = _workingFolders;
            }
        }
        private void fileRenaming()
        {
            if (workingOnFiles)
            {
                for (int i = 0; i < fileViewModel.items.Count; i++)
                {
                    string[] splitPath = fileViewModel.items[i].Path.Split('\\');
                    string[] n = splitPath.Where((val, idx) => idx != splitPath.Length - 1).ToArray();
                    string newPath = String.Join('\\', n) + '\\' + fileViewModel.items[i].NewName;

                    try
                    {
                        if (fileViewModel.items[i].NewName != "" && fileViewModel.items[i].NewName != fileViewModel.items[i].Name)
                        {
                            File.Move(fileViewModel.items[i].Path, newPath);
                            fileViewModel.items[i].Erro = $"Renamed from {fileViewModel.items[i].Name}";
                            fileViewModel.items[i].Name = fileViewModel.items[i].NewName;
                            fileViewModel.items[i].NewName = "";
                            fileViewModel.items[i].Path = newPath;
                        }
                        else
                        {
                            fileViewModel.items[i].Erro = "Unchanged";
                            fileViewModel.items[i].NewName = "";
                        }

                        updatePage(fileViewModel);
                    }
                    catch (Exception exc)
                    {
                        fileViewModel.items[i].Erro = $"Failed to rename";
                        fileViewModel.items[i].NewName = "";

                        updatePage(fileViewModel);
                    }
                }
                _progressData.setNewProgress(fileViewModel.items.Count, $"Done renaming", fileViewModel.items.Count);
                filePreviewMode = false;
            }
            else
            {
                for (int i = 0; i < folderViewModel.items.Count; i++)
                {
                    _progressData.setNewProgress(i + 1, $"Renaming on {folderViewModel.items[i].Name}", folderViewModel.items.Count);
                    string[] splitPath = folderViewModel.items[i].Path.Split('\\');
                    string[] n = splitPath.Where((val, idx) => idx != splitPath.Length - 1).ToArray();
                    string newPath = String.Join('\\', n) + "\\" + folderViewModel.items[i].NewName;

                    try
                    {
                        if (folderViewModel.items[i].NewName != "" && folderViewModel.items[i].NewName != folderViewModel.items[i].Name)
                        {
                            Directory.Move(folderViewModel.items[i].Path, newPath);
                            folderViewModel.items[i].Erro = $"Renamed from {folderViewModel.items[i].Name}";
                            folderViewModel.items[i].Name = folderViewModel.items[i].NewName;
                            folderViewModel.items[i].NewName = "";
                            folderViewModel.items[i].Path = newPath;
                        }
                        else
                        {
                            folderViewModel.items[i].Erro = "Unchanged";
                            folderViewModel.items[i].NewName = "";
                        }

                        updatePage(folderViewModel);
                    }
                    catch (Exception exc)
                    {
                        folderViewModel.items[i].Erro = $"Failed to rename";
                        folderViewModel.items[i].NewName = "";

                        updatePage(folderViewModel);
                    }
                }
                _progressData.setNewProgress(folderViewModel.items.Count, $"Done renaming", folderViewModel.items.Count);
                folderPreviewMode = false;
            }

    }
        private void startRenaming_Click(object sender, RoutedEventArgs e)
        {
            if (workingOnFiles)
            {
                if (filePreviewMode == false)
                {
                    previewRenaming_Click(sender, e);
                    fileRenaming();
                }
                else
                {
                    fileRenaming();
                }
            }
            else
            {
                if (folderPreviewMode == false)
                {
                    previewRenaming_Click(sender, e);
                    fileRenaming();
                }
                else
                {
                    fileRenaming();
                }
            }
        }

        private void removeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = new Item();
            var idx = -1;
            if (workingOnFiles)
            {
                item = (Item)FileTab.SelectedItem;
                idx = FileTab.SelectedIndex;
                fileViewModel.removeItemPage(idx);
                setupEnableChangePageBtn(fileViewModel);

                updatePage(fileViewModel);

                
            }
            else
            {
                item = (Item)FolderTab.SelectedItem;
                idx = FolderTab.SelectedIndex;
                folderViewModel.removeItemPage(idx);
                setupEnableChangePageBtn(folderViewModel);

                updatePage(folderViewModel);
            }          
        }
    }
}
