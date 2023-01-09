using Rule;
using System.Text.RegularExpressions;

namespace LowerCaseAndRemoveSpace
{
    public class LowerCaseAndRemoveSpace : IRule, ICloneable
    {
        public string Name { get; set; }

        public LowerCaseAndRemoveSpace()
        {
            this.Name = "LowerCaseAndRemoveSpace";
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
            return Regex.Replace(Regex.Replace(origin, " ", ""), @"\w", word => word.ToString().ToLower());
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