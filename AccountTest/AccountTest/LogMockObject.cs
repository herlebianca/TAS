using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace bank
{
    public class LogMockObject : ILogger //daca vrei sa iti faci tu un MockObject in loc sa folosesti Moq.Mock<clasa/interfata pe care sa o imite>, asta e structura:
    {
        List<String> performedLogActions = new List<string>();
        List<String> expectedLogActions = new List<string>();
        int expectedNumberOfCalls = 0;

        public void Log(String message)
        {
            performedLogActions.Add("method " + MethodBase.GetCurrentMethod().Name + " was called with message : " + message);
        }

        public void AddExpectedLogMessage(String message)
        {
            expectedLogActions.Add(message);
        }

        public bool Verify()
        {
            if (GetNumberOfCalls() != expectedNumberOfCalls) return false;

            // in this specific example is the same like the test above. Could be splitted for different types of logs.
            if (performedLogActions.Count() != expectedLogActions.Count()) return false;

            for (int i = 0; i < performedLogActions.Count(); i++)
            {
                Console.WriteLine(performedLogActions[i]);
                Console.WriteLine(expectedLogActions[i]);

                if (!performedLogActions[i].Equals(expectedLogActions[i])) return false;
            }

                return true;
        }

        public List<String> GetActions()
        {
            return performedLogActions;
        }

        public int GetNumberOfCalls()
        {
            return performedLogActions.Count;
        }

        internal void ExpectedNumberOfCalls(int p)
        {
            expectedNumberOfCalls = p;
        }
    }
}
