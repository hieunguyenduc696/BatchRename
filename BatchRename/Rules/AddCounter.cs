using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class AddCounter : IRule
    {
        public string Name { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
        public int NumberOfDigits { get; set; }

        public AddCounter()
        {
            Name = "AddCounter";
        }
        public string GetRuleName()
        {
            return Name;
        }
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
}
