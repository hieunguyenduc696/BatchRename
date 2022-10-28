using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RenamingRules
{
    public class RenamingRules
    {
        public List<string> ChangeExtension(List<string> fileNames, string newExtension)
        {
            List<string> result = new List<string>();
            string pattern = $"[0-9a-z]+.$";

            foreach (string name in fileNames)
            {
                result.Add(Regex.Replace(name, pattern, newExtension));
            }

            return result;
        }
        public List<string> AddCounter(List<string> fileNames, int start, int step, int numberOfDigits)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < fileNames.Count; i++)
            {
                string[] splitName = fileNames[i].Split('.');
                string counter = (start + i * step).ToString();

                while (counter.Length < numberOfDigits)
                {
                    counter = "0" + counter;
                }

                splitName[splitName.Length - 2] += counter;
                result.Add(string.Join(".", splitName));
            }

            return result;
        }
        public List<string> RemoveSpaceAtStartEnd(List<string> fileNames)
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

        public List<string> LowerCaseAndRemoveSpace(List<string> fileNames)
        {
            List<string> result = new List<string>();

            string pattern = " ";
            foreach (string name in fileNames)
            {
                result.Add(Regex.Replace(Regex.Replace(name, pattern, ""), @"\w", word => word.ToString().ToLower()));

            }

            return result;
        }

        public List<string> ReplacePattern(List<string> fileNames, string character, string replacedWith)
        {
            List<string> result = new List<string>();

            foreach (string name in fileNames)
            {
                string[] splitName = name.Split('.');
                string[] n = splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray();

                string file_name = string.Join("", n);
                string file_extension = splitName[splitName.Length - 1];

                result.Add(Regex.Replace(file_name, character, replacedWith) + "." + file_extension);

            }

            return result;
        }
        public List<string> AddPreffix(List<string> fileNames, string preffix)
        {
            List<string> result = new List<string>();

            foreach (string name in fileNames)
            {
                string[] splitName = name.Split('.');
                string[] n = splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray();

                string file_name = string.Join("", n);
                string file_extension = splitName[splitName.Length - 1];

                result.Add(Regex.Replace(file_name, @"^", preffix) + "." + file_extension);
            }

            return result;
        }
        public List<string> AddSuffix(List<string> fileNames, string suffix)
        {
            List<string> result = new List<string>();

            foreach (string name in fileNames)
            {
                string[] splitName = name.Split('.');
                string[] n = splitName.Where((val, idx) => idx != splitName.Length - 1).ToArray();

                string file_name = string.Join("", n);
                string file_extension = splitName[splitName.Length - 1];

                result.Add(Regex.Replace(file_name, @"$", suffix) + "." + file_extension);
            }

            return result;
        }
    }
}