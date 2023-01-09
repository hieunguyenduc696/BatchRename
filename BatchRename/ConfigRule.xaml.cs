using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BatchRename
{

    public partial class ConfigRule : Window
    {
        public BindingList<Rule.Parameter> _configRuleParameters = new BindingList<Rule.Parameter>();
        public string RuleName = "";
        bool isError = false;
        public ConfigRule(string ruleName, BindingList<Rule.Parameter> configRuleParameters)
        {
            InitializeComponent();
            RuleName = ruleName;
            _configRuleParameters = configRuleParameters;
  
            this.DataContext = this;
            ConfigLabel.Text = $"Configuration for: {RuleName}";
            ConfigParamatersList.ItemsSource = _configRuleParameters;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _configRuleParameters.Count;i++)
            {
                try
                {
                    Regex pattern = new Regex(@"");
                    switch (_configRuleParameters[i].Type) {
                        case "int":
                            int a = int.Parse(_configRuleParameters[i].StringValue);
                            break;
                        case "extension":
                            pattern = new Regex(@"^[\w\-. ]+$");
                            if (pattern.IsMatch(_configRuleParameters[i].StringValue) == false)
                            {
                                MessageBox.Show("Your inputs are invalid please recheck!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            break;
                        case "pattern":
                            pattern = new Regex(@"^[\w\-. ]+$");
                            if (pattern.IsMatch(_configRuleParameters[i].StringValue) == false)
                            {
                                MessageBox.Show("Your inputs are invalid please recheck!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            break;
                        default:
                            break;
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Your inputs are invalid please recheck!", "Warning!",MessageBoxButton.OK,MessageBoxImage.Warning);
                    return;
                }
            }
            DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
