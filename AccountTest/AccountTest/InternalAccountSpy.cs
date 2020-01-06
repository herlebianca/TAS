using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank
{
    //overrides Deposit & Withdraw methods of the Account class in order to record the method calls. 
    //this is an Internal Spy, not a Dependency Spy, because the SUT call siblings methods of the same class not external classes/methods.
    public class InternalAccountSpy : Account
    {
        List<String> actions = new List<string>();
        public new void Deposit(float amount)
        {
            base.Deposit(amount);
            actions.Add("Deposit "+amount);
        }

        public new void Withdraw(float amount)
        {
            base.Withdraw(amount);
            actions.Add("Withdraw " + amount);
        }

        public List<String> GetActions()
        {
            return actions;
        }
    }
}
