using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
  
    class RuleFactory
    {
        private static RuleFactory? _instance = null;
        private static Dictionary<string, IRule> _rules = new Dictionary<string, IRule>();

        public static void Config(List<IRule> rules)
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

        public IRule CreateRule(string loai)
        {
            IRule result = (IRule)(_rules[loai] as ICloneable)!.Clone();
            return result;
        }

        public static void updateConfig(string Name, IRule updatedRule)
        {
            _rules[Name] = updatedRule;
        }
    }
}
