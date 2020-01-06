using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace bank
{

    [TestFixture]
    public class AccountTest
    {
        Account source;
        Account destination;
        Account credit_card;

        [SetUp]
        public void InitAccount()
        {
            //arrange
            source = new Account();
            source.Deposit(200.00F);
            destination = new Account();
            destination.Deposit(150.00F);
            credit_card = new Account(); // regular account + credit_balance non-zero
            credit_card.Deposit(500F);
            credit_card.Grant(1000F);
        }

        [Test]
        [Category("pass")]
        public void TransferFunds()
        {
            //act
            source.TransferFunds(destination, 100.00F);

            //assert
            Assert.AreEqual(250.00F, destination.Balance);
            Assert.AreEqual(100.00F, source.Balance);

        }

        [Test, Category("pass")]
        [TestCase(200, 0, 78.2F)]
        [TestCase(200, 0, 189.5F)]
        [TestCase(200, 0, 1F)]
        public void TransferMinFunds(float a, float b, float c)
        {

            Account source = new Account();
            source.Deposit(a);
            Account destination = new Account();
            destination.Deposit(b);

            source.TransferMinFunds(destination, c);
            Assert.AreEqual(c, destination.Balance);
        }

        [Test]
        [Category("pass")]
        [TestCase(1, 999)] //ON
        [TestCase(175.5F, 824.5F)] //IN 
        [TestCase(200, 800)] //IN
        [TestCase(999, 1)] //ON
        public void WithdrawFromCreditCard(float a, float b)//WithdrawFromCreditCard nu-i totuna cu Withdraw -> vezi comentariile din clasa Account
        {
            //arrange
            Account credit_card = new Account();
            credit_card.Deposit(500F);
            credit_card.Grant(1000F);

            //act
            credit_card.WithdrawFromCreditCard(a);

            //assert
            Assert.AreEqual(b, credit_card.CreditBalance);
        }

        [Test]
        [Category("pass")]
        [TestCase(1F, 1000 - 0.87F, 500F - 0.1339F)] //ON
        [TestCase(200F, 1000 - 174F, 500F - 26.78F)] //IN
        [TestCase(700F, 1000 - 609F, 500F - 93.73F)] //IN
        [TestCase(1000F, 1000 - 870F, 500F - 133.9F)] //ON
        public void PayInstallment(float a, float b, float c)
        {
            //arrange
            Account credit_card = new Account();
            credit_card.Deposit(500F);
            credit_card.Grant(1000F);

            //act
            credit_card.WithdrawFromCreditCard(a); //WithdrawFromCreditCard nu-i totuna cu Withdraw -> vezi comentariile din clasa Account
            credit_card.PayInstallment();

            //assert
            Assert.AreEqual(b, credit_card.CreditBalance);
            Assert.AreEqual(c, credit_card.Balance);
        }

        [Test]
        [Category("fail")]
        [TestCase(200, 150,  190)]
        [TestCase(200, 150,   -1)]
        [TestCase(200, 150, -142)]
        [TestCase(200, 150,  345)]

        public void TransferMinFundsFail(int a, int b, int c)
        {
            Account source = new Account();
            source.Deposit(a);
            Account destination = new Account();
            destination.Deposit(b);

            Assert.That(() => destination = source.TransferMinFunds(destination, c), Throws.TypeOf<NotEnoughFundsException>());

        }

        [Test]
        [Category("fail")]
        [TestCase(300F, 700F  )] //OFF
        [TestCase(2.5F, 456.7F)] //OFF
        public void WithdrawFromCreditCardFail(float a, float b)
        {
            Account credit_card = new Account();
            credit_card.Grant(a);

            Assert.That(() => credit_card.WithdrawFromCreditCard(b), Throws.TypeOf<NotEnoughFundsException>());
        }

        [Test]
        [Category("fail")]
        [Combinatorial]

        public void TransferMinFundsFailAll([Values(190, 345)] int a, [Values(0, 20)] int b, [Values(346, 500)]int c)
        {
            Account source = new Account();
            source.Deposit(a);
            Account destination = new Account();
            destination.Deposit(b);

            Assert.That(() => destination = source.TransferMinFunds(destination, c), Throws.TypeOf<NotEnoughFundsException>());

        }

        [Test]
        [Category("fail")]
        [Combinatorial]

        public void PayInstallmentFailNoFunds([Values(1, 10)] float a, [Values(900,1000)] float b)
        {
            Account credit_card = new Account();
            credit_card.Deposit(a);
            credit_card.Grant(1000);
            credit_card.WithdrawFromCreditCard(b);

            Assert.That(() => credit_card.PayInstallment(), Throws.TypeOf<NotEnoughFundsException>());
            
        }

        [Test]
        [Category("fail")]
        [TestCase (645F)]
        [TestCase (279.53F)]
        public void PayInstallmentFailInvalid(float a)
        {
            Account credit_card = new Account();
            credit_card.Grant(a);

            Assert.That(() => credit_card.PayInstallment(), Throws.TypeOf<InvalidInstallmentException>());

        }
    }
    


}