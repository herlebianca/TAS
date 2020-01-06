using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Moq;
using System.Xml;
using System.Linq;

namespace bank
{
    public interface ILogger
    {
        void Log(string message);
        //functia Log nu are o implementare pentru ca nu ne intereseaza ce face, ci doar daca a fost apelata(eventual si de cate ori) sau nu
    }
    
    public class AccountForLogSpy:Account //clasa Account, dar cu logguri = mesaje la apeluri de functii
    {
       public ILogger logger { get; set; }
       public IClient client { get; set; }
        public AccountForLogSpy(float amount, IClient client, ILogger logger) :  base(amount)
        {
            this.logger = logger;
            this.client = client;
        }
        public override void TransferFunds(Account destination, float amount)
        {
            base.TransferFunds(destination, amount);
            logger.Log("method Log was called with message : Transaction : " + amount.ToString() + " & " + destination.Balance.ToString());
        }

        public override void PayInstallment()
        {
            base.PayInstallment();
            logger.Log("method Log was called with message: credit balance after payment = " + base.CreditBalance.ToString());
        }

        public override float ComputeRawIntstallment()
        {
            var temp = base.ComputeRawIntstallment();
            logger.Log("method Log was called with message: Raw Installment = " +temp.ToString());
            return temp;
        }

        public override float ComputeFullInstallment()
        {
            var temp = base.ComputeFullInstallment();
            logger.Log("method Log was called with message: Full Installment = " + temp.ToString());
            return temp;
        }
    }
    [TestFixture]
    public class AccountTestMockFramework
    {

        [SetUp]
        public void InitAccount()
        {
            //arrange
        }

        [Test]
        [Category("pass")]
        public void TransferFunds()
        {

            //arrange the MockObject
            var logMock = new Moq.Mock<ILogger>();

            //arrange SUT

            var client = new ClientDummy();
            var source = new AccountForLogSpy(200, client, logMock.Object);
            var destination = new AccountForLogSpy(150, client, logMock.Object);

            //set mocked logger expectations

            logMock.Setup(d => d.Log("method Log was called with message : Transaction : 100 & 250"));

            //logMock.ExpectedNumberOfCalls(1);
            
            //act

            source.TransferFunds(destination, 100.00F);

            //assert 
            Assert.AreEqual(250.00F, destination.Balance);
            Assert.AreEqual(100.00F, source.Balance);

            //mock object verify

            logMock.Verify(_ => _.Log("method Log was called with message : Transaction : 100 & 250"), Times.Once());
       }
       
        [Test]
        public void TransferFundsFromEurAmount_MockFramework_ShouldWork()
        {

            //arrange

            var source = new Account(200);
            var destination = new Account(150);

            var rateEurRon = 4.4F;
            var convertorMock = new Mock<ICurrencyConvertor>();

            convertorMock.Setup(_ => _.EurToRon(20)).Returns(20*rateEurRon); // set mock to act as a TestDouble Stub - gives IndirectInputs to the SUT

            //act
            source.TransferFundsFromEurAmount(destination, 20.00F, convertorMock.Object);

            //assert
            Assert.AreEqual(150.00F + 20 * rateEurRon, destination.Balance);
            Assert.AreEqual(200.00F - 20 * rateEurRon, source.Balance);

            convertorMock.Verify(_ => _.EurToRon(20), Times.Once()); //verify behavior 
        }

        [Test]
        public void PayInstallment()
        {
            //arrange the MockObject
            var logMock = new Moq.Mock<ILogger>();

            //arrange the SUT
            var client = new ClientDummy();
            var credit_card = new AccountForLogSpy(500,client,logMock.Object);
            credit_card.Grant(1000);
            credit_card.WithdrawFromCreditCard(200);

            //act
            credit_card.PayInstallment();

            //assert
            Assert.AreEqual(826F, credit_card.CreditBalance);

            //mock object verify
            logMock.Verify(_ => _.Log("method Log was called with message: Raw Installment = 26"), Times.Exactly(3));
            logMock.Verify(_ => _.Log("method Log was called with message: Full Installment = 26.78"), Times.Exactly(2));
            logMock.Verify(_ => _.Log("method Log was called with message: credit balance after payment = 826"),Times.Once);

        }
    }
}