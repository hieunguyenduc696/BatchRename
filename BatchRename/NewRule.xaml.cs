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
using Rule;
namespace BatchRename
{
    /// <summary>
    /// Interaction logic for NewRule.xaml
    /// </summary>
    public partial class NewRule : Window
    {
        public BindingList<RenamingRule> RulesSelected = new BindingList<RenamingRule>();

        public string Name {
            get;
            set;
        }
        public string Label { get; set; }


        public NewRule(List<Rule.IRule> rules, string label)
        {
            InitializeComponent();
            for (int i = 0; i < rules.Count;i++)
            {
                RulesSelected.Add(new RenamingRule() { Name = rules[i].GetRuleName(), Checked = false });
            }
            this.Label = label;
            RulesList.ItemsSource = RulesSelected;
            NewRuleLabel.Text = Label;
            this.DataContext = this;
        }
        public void setDefaulInput()
        {
            NameInput.Text = Name;
        }
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Name = NameInput.Text;
            DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
