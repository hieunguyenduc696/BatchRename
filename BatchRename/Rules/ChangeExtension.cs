using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchRename
{
    public class ChangeExtension : IRule
    {
        public string newExtension { get; set; }
        public string Name { get; set; }

        public string GetRuleName()
        {
            return Name;
        }
        public ChangeExtension()
        {
            Name = "ChangeExtension";
        }
        public List<string> Rename(List<string> fileNames)
        {
            List<string> result = new List<string>();
            string pattern = $"[0-9a-z]+.$";

            foreach (string name in fileNames)
            {
                result.Add(Regex.Replace(name, pattern, newExtension));
            }

            return result;
        }
    }
}
