using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RenamingRules
{
    interface IRule
    {
        List<string> Rename(List<string> origin);
    }

    public class ChangeExtension : IRule
    {
        public string newExtension { get; set; }

        ChangeExtension(string newExtension)
        {
            this.newExtension = newExtension;
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
    public class AdddCounter : IRule
    {
        public int Start { get; set; }
        public int Step { get; set; }
        public int NumberOfDigits { get; set; }
        public List<string> Rename(List<string> fileNames)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < fileNames.Count; i++)
            {
                string[] splitName = fileNames[i].Split('.');
                string counter = (Start + i * Step).ToString();

                while (counter.Length < NumberOfDigits)
                {
                    counter = "0" + counter;
                }

                splitName[splitName.Length - 2] += counter;
                result.Add(string.Join(".", splitName));
            }

            return result;
        }
    }
    public class RemoveSpaceAtStartEnd : IRule
    {
        public List<string> Rename(List<string> fileNames)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < fileNames.Count; i++)
            {
                string[] splitName = fileNames[i].Split('.');

                string pattern = @"^\s+|\s+$";
                string fileName = string.Join(".", splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray());
                string extension = splitName[splitName.Length - 1];

                result.Add(Regex.Replace(fileName, pattern, "") + "." + extension);
            }

            return result;
        }
    }
    public class LowerCaseAndRemoveSpace : IRule
    {
        public List<string> Rename(List<string> fileNames)
        {
            List<string> result = new List<string>();

            string pattern = " ";
            foreach (string name in fileNames)
            {
                result.Add(Regex.Replace(Regex.Replace(name, pattern, ""), @"\w", word => word.ToString().ToLower()));

            }

            return result;
        }
    }
    public class ReplacePattern : IRule {
        public List<string> Characters { get; set; }
        public string ReplacedWith { get; set; }
        ReplacePattern()
        {
            Characters = new List<string>();
        }
        public List<string> Rename(List<string> fileNames)
        {
            List<string> result = new List<string>();

            foreach (string name in fileNames)
            {
                string[] splitName = name.Split('.');
                string[] n = splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray();

                string file_name = string.Join("", n);
                string file_extension = splitName[splitName.Length - 1];
                for (int i = 0; i < Characters.Count; i++)
                result.Add(Regex.Replace(file_name, Characters[i], ReplacedWith) + "." + file_extension);

            }

            return result;
        }
    }
    public class AddPreffix : IRule
    {
        public string Preffix { get; set; }
        public List<string> Rename(List<string> fileNames)
        {
            List<string> result = new List<string>();

            foreach (string name in fileNames)
            {
                string[] splitName = name.Split('.');
                string[] n = splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray();

                string file_name = string.Join("", n);
                string file_extension = splitName[splitName.Length - 1];

                result.Add(Regex.Replace(file_name, @"^", Preffix) + "." + file_extension);
            }

            return result;
        }
    }

    public class AddSuffix : IRule
    {
        public string Suffix { get; set; }
        public List<string> Rename(List<string> fileNames)
        {
            List<string> result = new List<string>();

            foreach (string name in fileNames)
            {
                string[] splitName = name.Split('.');
                string[] n = splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray();

                string file_name = string.Join("", n);
                string file_extension = splitName[splitName.Length - 1];

                result.Add(Regex.Replace(file_name, @"$", Suffix) + "." + file_extension);
            }

            return result;
        }
    }
}