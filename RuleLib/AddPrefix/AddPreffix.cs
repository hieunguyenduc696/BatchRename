using Rule;
using System.Text.RegularExpressions;

namespace AddPreffix
{
    public class AddPreffix : IRule, ICloneable
    {
        public string Name { get; set; }
        public string Preffix { get; set; }
        public AddPreffix()
        {
            Name = "AddPreffix";
            Preffix = "defaultPreffix";
        }
        public string GetRuleName()
        {
            return Name;
        }
        public List<string> RenameList(List<string> fileNames)
        {
            throw new NotImplementedException();
        }

        public string Rename(string fileName, string type)
        {

            if (type == "file")
            {
                string[] splitName = fileName.Split('.');
                string[] n = splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray();

                string file_name = string.Join(".", n);
                string file_extension = splitName[splitName.Length - 1];

                return Regex.Replace(file_name, @"^", this.Preffix) + "." + file_extension;
            }

            else return Regex.Replace(fileName, @"^", this.Preffix);
        }

        public List<Rule.Parameter> GetParameters()
        {
            List<Parameter> list = new List<Parameter>();
            list.Add(new Parameter() { Name = "Preffix", Type = "string", StringValue = this.Preffix.ToString() });
            return list;
        }

        public void SetDefault()
        {

        }
        public void UpdateConfigParameters(List<Parameter> updatedList)
        {
            try
            {
                this.Preffix = updatedList[0].StringValue;

            }
            catch (Exception e)
            {

            }
        }

        public void iterateConfig(int idx)
        {
 
        }

        public void ParseConfigFromString(string data)
        {
            string[] splitParameters = data.Split("@val@");
            if (splitParameters.Length == 2)
            {
                try
                {
                    this.Preffix = splitParameters[1];

                }
                catch (Exception exception)
                {
                    this.Preffix = "";
                }

            }
        }

        public string ParseConfigToString()
        {
            string result = "";
            if (this.Preffix == "")
            {
                this.Preffix = "defaultPreffix";
            }
            result += this.Name + "@val@" + this.Preffix;
            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}