using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace bank
{
    public class LogSpy: ILogger
    {
        List<String> actions = new List<string>();
        public void Log(String message)
        {
            actions.Add(MethodBase.GetCurrentMethod().Name + "(" + DateTime.Now + ") "+ message);
        }


        public List<String> GetActions()
        {
            return actions;
        }

        public int GetNumberOfCalls()
        {
            return actions.Count;
        }
    }
}
