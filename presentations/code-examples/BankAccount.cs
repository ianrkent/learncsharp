namespace code_examples
{
    public class BankAccount {

        private decimal _balance = 0;
        public decimal Balance { 
            get { return _balance; } 
            set { _balance = value; } 
        }
        
        public BankAccount(decimal intitialDeposit) => Deposit(intitialDeposit);
    
        public void Deposit (decimal amount) => Balance += amount;

        public void Withdraw (decimal amount) => Balance -= amount; 
    } 
}
