namespace code_examples
{
    public class BankAccount {

        private decimal _balance = 0;
        private string _holderName;

        public decimal Balance => _balance;
        
        public  string HolderName { 
            get => _holderName; 
            set => _holderName = value; 
        }

        // public BankAccount(decimal intitialDeposit) => Deposit(intitialDeposit);
    
        // public void Deposit (decimal amount) => Balance += amount;

        // public void Withdraw (decimal amount) => Balance -= amount; 
    } 
}
