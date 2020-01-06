using System;
using System.Collections.Generic;
using System.Text;

namespace bank
{
    public interface ICurrencyConvertor
    {
        //implemented in the real CurrencyConvertor and also in the CurrencyConvertorStub
        float EurToRon(float ValueInEur);
        float RonToEur(float ValueInRon);
    }

    public class Account
    {
        private float balance;
        private float minBalance = 10;
        private float credit_balance;
        private float granted_loan; // used when computing the monthly installment

        public Account()
        {
            balance = 0;
            credit_balance = 0;
        }

        public Account(float value)
        {
            balance = value;
        }

        public void Deposit(float amount)
        {
            balance += amount;
        }

        public void Withdraw(float amount)
        {
            balance -= amount;
        }

        public virtual void TransferFunds(Account destination, float amount)
        {
            destination.Deposit(amount);
            Withdraw(amount);
        }

        public Account TransferMinFunds(Account destination, float amount)
        {
            if (amount <= 0)
            {
                throw new NotEnoughFundsException();
            }
            if (Balance - amount > MinBalance)
            {
                destination.Deposit(amount);
                Withdraw(amount);
            }
            else throw new NotEnoughFundsException();
            return destination;
        }

        public void Grant(float amount)
        {
            if (amount >= 0 && amount < 10000) //considering 10000 is the maximum loan value
            {
                granted_loan = amount;
                credit_balance = amount;
            }
            else
                throw new NotEnoughFundsException();
        }

        public void WithdrawFromCreditCard(float amount) //vezi ca asta e diferit de Withdraw
                                                        //functia asta e ca si cum ai face un imprumut(sau cumperi pe cardu de credit)
        {
            if (amount >= 0 && amount <= credit_balance && credit_balance>0)
            {
                credit_balance -= amount;
            }
            else
                throw new NotEnoughFundsException();
        }

        public virtual float ComputeRawIntstallment() //without interest
        {
            float precentage = 0.13f;
            float raw_installment = (precentage * (granted_loan-credit_balance));
            return raw_installment;
        }
        public virtual float ComputeFullInstallment() //including interest
        {
           
            float interest = 0.03f;
            float full_installment = ComputeRawIntstallment() * (1 + interest);
            return full_installment;
        }
        
        public virtual void PayInstallment()
        {
            if (!(credit_balance == granted_loan))
            {
                if (ComputeFullInstallment() <= balance)
                {
                    Withdraw(ComputeFullInstallment());
                    credit_balance += ComputeRawIntstallment();
                }
                else
                    throw new NotEnoughFundsException();
            }
            else
                throw new InvalidInstallmentException();
            
        }

        public void TransferFundsFromEurAmount(Account destination, float amount, ICurrencyConvertor convertor)
        {
            float amountRON = convertor.EurToRon(amount);
            destination.Deposit(amountRON);
            this.Withdraw(amountRON);
        }

        public float Balance
        {
            get { return balance; }
        }

        public float MinBalance
        {
            get { return minBalance; }
        }

        public float CreditBalance
        {
            get { return credit_balance; }
        }

    }
    public class NotEnoughFundsException : ApplicationException
    {
    }

    public class InvalidInstallmentException : ApplicationException
    {
    }
}