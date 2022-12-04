using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    interface IRule
    {
        List<string> Rename(List<string> origin);
        string GetRuleName();
    }
}
