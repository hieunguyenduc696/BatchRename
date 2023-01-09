using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rule;

namespace BatchRename
{
  
    class RuleFactory
    {
        private static RuleFactory? _instance = null;
        private static Dictionary<string, Rule.IRule> _rules = new Dictionary<string, Rule.IRule>();

        public static void Config(List<Rule.IRule> rules)
        {
            foreach (var rule in rules)
            {
                _rules.Add(rule.GetRuleName(), rule);
            }
        }
        private RuleFactory()
        {
            // Do nothing
        }

        public static RuleFactory Instance()
        {
            if (_instance == null)
            {
                _instance = new RuleFactory();
            }

            return _instance;
        }

        public Rule.IRule CreateRule(string loai)
        {
            Rule.IRule result = (Rule.IRule)(_rules[loai] as ICloneable)!.Clone();
            return result;
        }

        public static void updateConfig(string Name, Rule.IRule updatedRule)
        {
            _rules[Name] = updatedRule;
        }
    }
}
