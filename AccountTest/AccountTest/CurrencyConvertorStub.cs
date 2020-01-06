using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank
{
    public class CurrencyConvertorStub: ICurrencyConvertor
    {
        float rateEurRon;
        public CurrencyConvertorStub(float _rateEurRon)
        {
            rateEurRon = _rateEurRon;
        }

        public float EurToRon(float ValueInEur)
        {
            return ValueInEur * rateEurRon;
        }

        public float RonToEur(float ValueInRon)
        {
            return ValueInRon / rateEurRon;
        }
    }
}
