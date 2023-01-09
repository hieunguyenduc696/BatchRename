using Rule;
using System.Text.RegularExpressions;

namespace ReplacePattern
{
    public class ReplaceCharacter : IRule, ICloneable
    {
        public string Name { get; set; }
        public string CharactersListString { get; set; }
        public string ReplacedWith { get; set; }
        public ReplaceCharacter()
        {
            Name = "ReplaceCharacter";
            CharactersListString = ",";
            ReplacedWith = ",";
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
            string[] characters = new string[CharactersListString.Length];
            for (int i = 0; i < CharactersListString.Length; i++)
            {
                characters[i] = new string(CharactersListString[i], 1);
            }

            if (type == "file")
            {
                string[] splitName = fileName.Split('.');
                string[] n = splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray();
 

                string file_name = string.Join(".", n);
                string file_extension = splitName[splitName.Length - 1];

                for (int i = 0; i < characters.Length; i++)
                    //file_name = Regex.Replace(file_name, characters[i], ReplacedWith);
                    file_name = file_name.Replace(characters[i], ReplacedWith);

                return file_name + "." + file_extension;
            }
            for (int i = 0; i < characters.Length; i++)
                //fileName = Regex.Replace(fileName, characters[i], ReplacedWith);
                fileName = fileName.Replace(characters[i], ReplacedWith); 

            return fileName;
        }

        public List<Rule.Parameter> GetParameters()
        {
            List<Parameter> list = new List<Parameter>();
            list.Add(new Parameter() { Name = "CharactersListString", Type = "pattern", StringValue = this.CharactersListString.ToString() });
            list.Add(new Parameter() { Name = "ReplacedWith", Type = "pattern", StringValue = this.ReplacedWith.ToString() });
            return list;
        }

        public void SetDefault()
        {

        }
        public void UpdateConfigParameters(List<Parameter> updatedList)
        {
            try
            {
                this.CharactersListString = updatedList[0].StringValue;

            }
            catch (Exception e)
            {

            }

            try
            {
                this.ReplacedWith = updatedList[1].StringValue;

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
            if (splitParameters.Length == 3)
            {
                try
                {
                    this.CharactersListString = splitParameters[1];
                    
                }
                catch (Exception exception)
                {
                    this.CharactersListString = ",";
                }

                try
                {
                    this.ReplacedWith = splitParameters[2];

                }
                catch (Exception exception)
                {
                    this.ReplacedWith = ",";
                }

            }
        }

        public string ParseConfigToString()
        {
            string result = "";
            if (CharactersListString == "")
            {
                CharactersListString = ",";
            }
            if (ReplacedWith == "")
            {
                ReplacedWith = ",";
            }
            result += this.Name + "@val@" + this.CharactersListString + "@val@" + this.ReplacedWith;
            return result;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}