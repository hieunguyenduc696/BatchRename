using Rule;

namespace PascalCase
{
    public class PascalCase : IRule, ICloneable
    {
        public string Name { get; set; }

        public PascalCase()
        {
            this.Name = "PascalCase";
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
            List<int> replacePos = new List<int>();

            if (origin[0] != '_')
            {
                replacePos.Add(0);
            }

            if (type == "file")
            {
                string[] splitName = origin.Split('.');

                string fileName = string.Join(".", splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray());
                string extension = splitName[splitName.Length - 1];

                for (int i = 0; i < fileName.Length; i++)
                {
                    if (fileName[i] == '_')
                    {
                        if (i + 1 <= fileName.Length - 1)
                        {
                            replacePos.Add(i + 1);
                        }

                    }
                }
                for (int i = 0; i < replacePos.Count; i++)
                {
                    fileName = fileName.Substring(0, replacePos[i]) + (fileName.Substring(replacePos[i], 1)).ToUpper() + fileName.Substring(replacePos[i] + 1);
                }
                return fileName + "." + extension;
            }
            else
            {
                string result = origin;
                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i] == '_')
                    {
                        if (i + 1 <= result.Length - 1)
                        {
                            replacePos.Add(i + 1);
                        }

                    }
                }

                for (int i = 0; i < replacePos.Count; i++)
                {

                    result = result.Substring(0, replacePos[i]) + (result.Substring(replacePos[i], 1)).ToUpper() + result.Substring(replacePos[i] + 1);
                }
                return result;
            }

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