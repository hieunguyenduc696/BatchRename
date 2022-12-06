using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    interface IRule
    {
        string Rename(string origin);
        List<string> RenameList(List<string> origins);
        string GetRuleName();
        List<Parameter> GetParameters();

        void UpdateConfigParameters(List<Parameter> updatedList);
        void iterateConfig(int idx); // use for setup iterating
    }
}
