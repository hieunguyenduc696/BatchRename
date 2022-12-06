using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchRename
{
    public class ChangeExtension : IRule, ICloneable
    {
        public string NewExtension { get; set; }
        public string Name { get; set; }

        public string GetRuleName()
        {
            return Name;
        }
        public ChangeExtension()
        {
            Name = "ChangeExtension";
        }
        public List<string> RenameList(List<string> fileNames)
        {
            List<string> result = new List<string>();
            string pattern = $"[0-9a-z]+.$";

            foreach (string name in fileNames)
            {
                result.Add(Regex.Replace(name, pattern, NewExtension));
            }

            return result;
        }

        public string Rename(string fileName)
        {
            string result = "";
            string pattern = $"[0-9a-z]+.$";

            result= Regex.Replace(fileName, pattern, NewExtension);

            return result;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public List<Parameter> GetParameters()
        {

             List<Parameter> list = new List<Parameter>();
             list.Add(new Parameter() { Name = "NewExtension", Type = "extension", StringValue = this.NewExtension });    
             return list;

        }

        public void UpdateConfigParameters(List<Parameter> updatedList)
        {
            this.NewExtension = updatedList[0].StringValue;
        }

        public void iterateConfig(int idx)
        {

        }
    }
}
