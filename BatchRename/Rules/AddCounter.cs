//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Rule;
//namespace BatchRename
//{
//    public class AddCounter : Rule.IRule, ICloneable
//    {
//        public string Name { get; set; }
//        public int Start { get; set; }
//        public int Step { get; set; }
//        public int NumberOfDigits { get; set; }

//        public int Counter { get; set; }
//        public AddCounter()
//        {
//            Name = "AddCounter";
//        }
//        public string GetRuleName()
//        {
//            return Name;
//        }
//        public List<string> RenameList(List<string> fileNames)
//        {
//            List<string> result = new List<string>();

//            for (int i = 0; i < fileNames.Count; i++)
//            {
//                string[] splitName = fileNames[i].Split('.');
//                string counter = (Start + i * Step).ToString();

//                while (counter.Length < NumberOfDigits)
//                {
//                    counter = "0" + counter;
//                }

//                splitName[splitName.Length - 2] += counter;
//                result.Add(string.Join(".", splitName));
//            }

//            return result;
//        }

//        public string Rename(string fileName)
//        {
//            string result = "";


//            string[] splitName = fileName.Split('.');
//            string counter = (Start + Counter * Step).ToString();

//            while (counter.Length < NumberOfDigits)
//             {
//                counter = "0" + counter;
//             }

//            splitName[splitName.Length - 2] += counter;

//            result = String.Join(".", splitName);
//            return result;
//        }

//        public object Clone()
//        {
//            return this.MemberwiseClone();
//        }

//        public List<Rule.Parameter> GetParameters()
//        {
//            List<Rule.Parameter> list = new List<Rule.Parameter> ();
//            list.Add(new Rule.Parameter() { Name = "Start", Type = "int", StringValue = this.Start.ToString() });
//            list.Add(new Rule.Parameter() { Name = "Step", Type = "int", StringValue = this.Step.ToString() });
//            list.Add(new Rule.Parameter() { Name = "NumberOfDigits", Type = "int", StringValue = this.NumberOfDigits.ToString() });
//            return list;
//        }

//        public void SetDefault()
//        {
//            this.Start = this.Step = this.NumberOfDigits = 0;
//        }
//        public void UpdateConfigParameters(List<Rule.Parameter> updatedList)
//        {
//            try
//            {
//                int newValue = int.Parse(updatedList[0].StringValue);
//                this.Start = newValue;

//            }
//            catch(Exception e)
//            {

//            }
//            try
//            {
//                int newValue = int.Parse(updatedList[1].StringValue);
//                this.Step = newValue;

//            }
//            catch (Exception e)
//            {

//            }
//            try
//            {
//                int newValue = int.Parse(updatedList[2].StringValue);
//                this.NumberOfDigits = newValue;

//            }
//            catch (Exception e)
//            {

//            }
//        }

//        public void iterateConfig(int idx)
//        {
//            this.Counter = idx;
//        }

//        public void ParseConfigFromString(string data)
//        {
//            string[] splitParameters = data.Split("@val@");
//            if(splitParameters.Length == 4)
//            {
//                try
//                {
//                    this.Start = int.Parse(splitParameters[1]);
//                    this.Step = int.Parse(splitParameters[2]);
//                    this.NumberOfDigits = int.Parse(splitParameters[3]);
//                }
//                catch(Exception exception)
//                {
//                    this.Step = this.Start = this.NumberOfDigits = 0;
//                }
//            }
//        }

//        public string ParseConfigToString()
//        {
//            string result = "";
//            result += this.Name + "@val@"  + this.Start + "@val@" + this.Step + "@val@" + this.NumberOfDigits;
//            return result;
//        }
//    }
//}


