using System.ComponentModel;
using System.Reflection.Metadata;

namespace Rule
{
    public interface IRule
    {
        string Rename(string origin, string type);
        List<string> RenameList(List<string> origins);
        string GetRuleName();
        List<Parameter> GetParameters();

        void UpdateConfigParameters(List<Parameter> updatedList);
        void iterateConfig(int idx); // use for setup iterating

        void ParseConfigFromString(string data);

        string ParseConfigToString();

        void SetDefault();
    }

    public class Parameter : INotifyPropertyChanged, ICloneable
    {
        public string Name
        {
            get; set;
        }

        public string Type
        {
            get; set;
        }

        public string StringValue
        {
            get; set;
        }

        public void copyContent(string Name, string Type, string StringValue)
        {
            this.Name = Name;
            this.Type = Type;
            this.StringValue = StringValue;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}