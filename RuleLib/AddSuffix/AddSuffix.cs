using Rule;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AddSuffix
{
    public class AddSuffix : IRule, ICloneable
    {
        public string Name { get; set; }
        public string Suffix { get; set; }
        public AddSuffix()
        {
            Name = "AddSuffix";
            Suffix = "defaultSuffix";
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

                return Regex.Replace(file_name, @"$", Suffix) + "." + file_extension;
            }
            else return Regex.Replace(fileName, @"$", Suffix);
      

        }

        public List<Rule.Parameter> GetParameters()
        {
            List<Parameter> list = new List<Parameter>();
            list.Add(new Parameter() { Name = "Suffix", Type = "string", StringValue = this.Suffix.ToString() });
            return list;
        }

        public void SetDefault()
        {

        }
        public void UpdateConfigParameters(List<Parameter> updatedList)
        {
            try
            {
                this.Suffix = updatedList[0].StringValue;

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
                    this.Suffix = splitParameters[1];

                }
                catch (Exception exception)
                {
                    this.Suffix = "";
                }

            }
        }

        public string ParseConfigToString()
        {
            string result = "";
            if (this.Suffix == "")
            {
                this.Suffix = "defaultSuffix";
            }
            result += this.Name + "@val@" + this.Suffix;
            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}