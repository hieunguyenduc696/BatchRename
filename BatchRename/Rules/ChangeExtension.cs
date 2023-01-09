//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using Rule;
//namespace BatchRename
//{
//    public class ChangeExtension : Rule.IRule, ICloneable
//    {
//        public string NewExtension { get; set; }
//        public string Name { get; set; }

//        public string GetRuleName()
//        {
//            return Name;
//        }
//        public ChangeExtension()
//        {
//            Name = "ChangeExtension";
//        }
//        public List<string> RenameList(List<string> fileNames)
//        {
//            List<string> result = new List<string>();
//            string pattern = $"[0-9a-z]+.$";

//            foreach (string name in fileNames)
//            {
//                result.Add(Regex.Replace(name, pattern, NewExtension));
//            }

//            return result;
//        }

//        public string Rename(string fileName)
//        {
//            string result = "";
//            string pattern = $"[0-9a-z]+.$";

//            result= Regex.Replace(fileName, pattern, NewExtension);

//            return result;
//        }
//        public object Clone()
//        {
//            return this.MemberwiseClone();
//        }

//        public List<Rule.Parameter> GetParameters()
//        {

//             List<Rule.Parameter> list = new List<Rule.Parameter>();
//             list.Add(new Rule.Parameter() { Name = "NewExtension", Type = "extension", StringValue = this.NewExtension });    
//             return list;

//        }

//        public void UpdateConfigParameters(List<Rule.Parameter> updatedList)
//        {
//            this.NewExtension = updatedList[0].StringValue;
//        }

//        public void iterateConfig(int idx)
//        {

//        }
//        public void SetDefault()
//        {
//            this.NewExtension = "file";
//        }
//        public void ParseConfigFromString(string data)
//        {
//            string[] splitParameters = data.Split("@val@");
//            if (splitParameters.Length == 2)
//            {
//                this.NewExtension = splitParameters[1];
//            }
//        }

//        public string ParseConfigToString()
//        {
//            return this.Name + "@val@" + this.NewExtension;
//        }
//    }
//}
