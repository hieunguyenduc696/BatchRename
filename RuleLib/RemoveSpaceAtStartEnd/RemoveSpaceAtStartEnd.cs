using Rule;
using System.Text.RegularExpressions;

namespace RemoveSpaceAtStartEnd
{
    public class RemoveSpaceAtStartEnd : IRule, ICloneable
    {
        public string Name { get; set; }

        public RemoveSpaceAtStartEnd()
        {
            this.Name = "RemoveSpaceAtStartEnd";
        }
        public object Clone()
        {
            return MemberwiseClone();
        }

        public List<Parameter> GetParameters()
        {
            return new List<Parameter>();
        }

        public string GetRuleName()
        {
            return this.Name;
        }

        public void iterateConfig(int idx)
        {

        }

        public void ParseConfigFromString(string data)
        {
            
        }

        public string ParseConfigToString()
        {
            return this.Name;
        }

        public string Rename(string origin, string type)
        {
            string pattern = @"^\s+|\s+$";

            if (type == "file")
            {
                string[] splitName = origin.Split('.');

                string fileName = string.Join(".", splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray());
                string extension = splitName[splitName.Length - 1];

                return Regex.Replace(fileName, pattern, "") + "." + extension;
            }

            return Regex.Replace(origin, pattern, "");
        }

        public List<string> RenameList(List<string> origins)
        {
            throw new NotImplementedException();
        }

        public void SetDefault()
        {
            
        }

        public void UpdateConfigParameters(List<Parameter> updatedList)
        {
            
        }
    }
}