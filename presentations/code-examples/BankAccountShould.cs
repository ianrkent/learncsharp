using System;
using Xunit;

namespace code_examples
{
    public class BankAccountShould
    {
        [Fact]
        public void ReduceBalanceWhenWithdrawing()
        {
            var sut = new BankAccount(50);
            sut.Withdraw(20);
            Assert.Equal(30, sut.Balance);
        }
    }
}
