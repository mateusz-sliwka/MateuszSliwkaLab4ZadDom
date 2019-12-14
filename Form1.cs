using MateuszSliwkaLab4ZadDom.Models;
using MateuszSliwkaLab4ZadDom.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MateuszSliwkaLab4ZadDom
{
    public partial class Form1 : Form
    {
        private readonly IBankGeneric<Account> _accounts;
        private readonly IBankGeneric<Transfer> _transfers;
        private readonly IBankGeneric<Payment> _payments;
        private readonly IBankGeneric<Withdrawal> _withdrawals;
        private List<TextBox> newAccount;
        private List<TextBox> editAccount;
        private int currentAccountId, currentWithadrawtId, currentTransferId, currentPaymentId;
        public Form1()
        {
            InitializeComponent();
            _accounts = new BankGeneric<Account>(); //zainicjownaie nowych list obiektow z bazy danego typu
            _transfers = new BankGeneric<Transfer>();
            _payments = new BankGeneric<Payment>();
            _withdrawals = new BankGeneric<Withdrawal>();
            newAccount = new List<TextBox>();
            editAccount = new List<TextBox>();
            newAccount.Add(firstnameTextBox); //dodanie textboxa do listy textboxow sekcji nowego konta zeby bylo latwiej je czyscic, przekazac cala liste
            newAccount.Add(lastnameTextBox);
            newAccount.Add(peselTextBox);
            
            editAccount.Add(newNameTextBox); //to samo dla edycji
            editAccount.Add(newLastNameTextBox);
            editAccount.Add(newPeselTextBox);
         
            LoadPayments(); //wywolanie przeladowania tabel
            LoadWithdrawals();
            LoadTransfers();
            LoadAccounts();
        }
        private void LoadAccounts() //przeladowanie tabeli kont, usuwanie wyswietlania kolumn ktore nie sa potrzebne
        {
            accountsGridView.DataSource = _accounts.GetAll();
            for(int i=4;i<8;i++)
            accountsGridView.Columns[i].Visible = false;
        }
        private void LoadTransfers() //jak wyzej tylko dla przelewow
        {

            transferGridView.DataSource = _transfers.GetAll();
            transferGridView.Columns[3].Visible = false;
            transferGridView.Columns[4].Visible = false;

        }
        private void LoadPayments() //jak wyzej tylko dla platnosci
        {
            paymentGridView.DataSource = _payments.GetAll();
            paymentGridView.Columns[2].Visible = false;
       
        }
        private void LoadWithdrawals()
        {
            withdrawalGridView.DataSource = _withdrawals.GetAll();
            withdrawalGridView.Columns[2].Visible = false;
        }

        private void cleanTextBoxes(List<TextBox> list) //wyczyszczenie wskazanych checkboxow
        {
            foreach(TextBox x in list)
            {
                x.Text = "";
            }
        }

        private void fillAcounts(ComboBox comboBox) //wypelnienie wskazanego comboboxa kontami
        {
            comboBox.Items.Clear();
            foreach (Account a in _accounts.GetAll())
                comboBox.Items.Add(a);
        }
        private void addAccountEnableButton_Click(object sender, EventArgs e) //funkcja 
        {
            cleanTextBoxes(newAccount); //czyszczenie textboxow sektoru dodawania
            cleanTextBoxes(editAccount); //czyszczenie textboxow sektoru edycji
            addAcountGroupBox.Enabled = true; //wlaczneie groupboxa dodawania
            editAccountGroupBox.Enabled = false; //wylaczenie groupboxa edycji
        }

        private void editAccountEnableButton_Click(object sender, EventArgs e) //funkcja edytujaca obiekt (opisana nizej)
        {


            if (accountsGridView.SelectedRows.Count > 0)
            {
                cleanTextBoxes(newAccount);
                cleanTextBoxes(editAccount);
                addAcountGroupBox.Enabled = false;
                editAccountGroupBox.Enabled = true;
                var selectedJumperIndex = Int32.Parse(accountsGridView.SelectedRows[0].Cells[0].Value.ToString());
                newNameTextBox.Text = _accounts.GetById(selectedJumperIndex).FirstName;
                newLastNameTextBox.Text = _accounts.GetById(selectedJumperIndex).LastName;
                newPeselTextBox.Text = _accounts.GetById(selectedJumperIndex).PESEL;
                currentAccountId = _accounts.GetById(selectedJumperIndex).Id;
            }
        }
        private void addAccountButton_Click(object sender, EventArgs e) //funkcja dodajaca nowy rekord (opisana nizej)
        {
            Account account = new Account
            {
                FirstName = firstnameTextBox.Text,
                LastName = lastnameTextBox.Text,
                PESEL = peselTextBox.Text
            };
            _accounts.Create(account);
            _accounts.Save();
            LoadAccounts();
            cleanTextBoxes(newAccount);
            addAcountGroupBox.Enabled = false;
            MessageBox.Show("Dodano nowego klienta!");
        }

        private void deleteAccountButton_Click(object sender, EventArgs e) //funkcja usuwujaca wybrany rekord (opisana nizej)
        {
            if (accountsGridView.SelectedRows.Count > 0)
            {
                var selectedJumperIndex = Int32.Parse(accountsGridView.SelectedRows[0].Cells[0].Value.ToString());
                _accounts.DeleteById(selectedJumperIndex);
                _accounts.Save();
            }
            LoadAccounts();
        }

        private void updateAccountButton_Click(object sender, EventArgs e) //funkcja aktualizacujaca redkord opisania nizej
        {
            Account account = _accounts.GetById(currentAccountId);
            account.PESEL = newPeselTextBox.Text;
            account.LastName = newLastNameTextBox.Text;
            account.FirstName = newNameTextBox.Text;

            _accounts.Update(account);
            _accounts.Save();
            LoadAccounts();
            cleanTextBoxes(editAccount);
            editAccountGroupBox.Enabled = false;
            MessageBox.Show("Zaktualizowano dane klienta!");

        }

        private void newTransferEnableButton_Click(object sender, EventArgs e) //funkcja odblokowujaca panel dodania, blokujaca panel edycji
        {
            cleanTextBoxes(newAccount);
            cleanTextBoxes(editAccount);
            addTransferGroupBox.Enabled = true;
            editTransferGroupBox.Enabled = false;
            fillAcounts(RecipientComboBox);
            fillAcounts(SenderComboBox);
        }

        private void addTransferButton_Click(object sender, EventArgs e) //funkcja dodawania opisana nizej
        {
            Transfer transfer = new Transfer
            {
                Amount = float.Parse(valueTextBox.Text),
                RecipientId = ((Account)RecipientComboBox.SelectedItem).Id,
                SenderId = ((Account)SenderComboBox.SelectedItem).Id
        
            };
          
            
            _transfers.Create(transfer);
            _transfers.Save();
            LoadTransfers();
            RecipientComboBox.Items.Clear();
            RecipientComboBox.ResetText();
            SenderComboBox.Items.Clear();
            SenderComboBox.ResetText();
            valueTextBox.Text = "";
            addTransferGroupBox.Enabled = false;
            MessageBox.Show("Dodany nowy przelew!");
        }

        private void deleteTransferEnableButton_Click(object sender, EventArgs e) //funkcja usuwujaca opisana nizej
        {
            if (transferGridView.SelectedRows.Count > 0)
            {
                var selectedJumperIndex = Int32.Parse(transferGridView.SelectedRows[0].Cells[0].Value.ToString());
                _transfers.DeleteById(selectedJumperIndex);
                _transfers.Save();
            }

            LoadTransfers();
        }

        private void addWithadraw_Click(object sender, EventArgs e)
        {
   
            Withdrawal
                withdrawal = new Withdrawal
            {
                Amount = float.Parse(withadrawValueTextBox.Text),
                AccountId = ((Account)withadrawMakerCombo.SelectedItem).Id,
              
            };

            _withdrawals.Create(withdrawal);
            _withdrawals.Save();
            LoadWithdrawals();
           withadrawMakerCombo.Items.Clear();
            withadrawMakerCombo.ResetText();
   withadrawValueTextBox.Text = "";
            addWithadrawalGroupBox.Enabled = false;
            MessageBox.Show("Dodany nowa wyplate!");
        }

        private void button3_Click(object sender, EventArgs e) //funkcja wlaczajaca panel dodawania wyplat i wypelniajaca comboboxa userami
        {
            fillAcounts(withadrawMakerCombo);
            addWithadrawalGroupBox.Enabled = true;
        }

        private void updateWithadrawButton_Click(object sender, EventArgs e) //funkcja akutalizacji, opisania nizej
        {
            Withdrawal w = _withdrawals.GetById(currentWithadrawtId);
            w.Amount = float.Parse(valueNewWithadrawTextBox.Text);
            w.AccountId = ((Account)newWithadrawalAuthorCombo.SelectedItem).Id;
            _withdrawals.Update(w);
            _withdrawals.Save();
            LoadWithdrawals();
            valueNewWithadrawTextBox.Text = "";
            newWithadrawalAuthorCombo.Items.Clear();
            newWithadrawalAuthorCombo.ResetText();
            editWithadrawalGroupBox.Enabled = false;
            MessageBox.Show("Zaktualizowano dane wypłaty!");
       
        }

        private void deleteWithadrawButton_Click(object sender, EventArgs e) //funkcja usuwujaca wybrany obiekt z tabelii
        {
            if (withdrawalGridView.SelectedRows.Count > 0)
            {
                var selectedJumperIndex = Int32.Parse(withdrawalGridView.SelectedRows[0].Cells[0].Value.ToString());
                _withdrawals.DeleteById(selectedJumperIndex);
                _withdrawals.Save();
            }
            LoadWithdrawals();
        }

        private void updateTransferButton_Click(object sender, EventArgs e) //funkcja dzialajaca analogiznie do innych update, jedna z nim opisana nizej
        {
            Withdrawal w = _withdrawals.GetById(currentWithadrawtId);
            Transfer t = _transfers.GetById(currentTransferId);
            t.Amount = float.Parse(newValueTextBox.Text);
            t.RecipientId = ((Account)newRecipientComboBox.SelectedItem).Id;
            t.SenderId = ((Account)newSenderComboBox.SelectedItem).Id;
            
            _transfers.Update(t);
            _transfers.Save();
            LoadTransfers();
            newValueTextBox.Text = "";
           newSenderComboBox.Items.Clear();
            newSenderComboBox.ResetText();
            newRecipientComboBox.Items.Clear();
            newRecipientComboBox.ResetText();
            editTransferGroupBox.Enabled = false;
            MessageBox.Show("Zaktualizowano dane przelewu!");
        }

        private void newIncomeButton_Click(object sender, EventArgs e) //funkcja odblokowywujaca groupBox dodawania, blokowanie groupBox edycji
        {
            newIncomeGroupBox.Enabled = true;
            editIncomeGroupBox.Enabled = false;

            fillAcounts(incomerCb);
        }

        private void addIncomeButton_Click(object sender, EventArgs e) //funkcja add dzialajaca analogicznie do innych funkcji add 
        {
                Payment payment = new Payment
                {
                    Amount = float.Parse(amountIncomeTextBox.Text), 
                    AccountId = ((Account)incomerCb.SelectedItem).Id,

                };

            _payments.Create(payment);
            _payments.Save();
            LoadPayments();
            incomerCb.Items.Clear();
            incomerCb.ResetText();
            amountIncomeTextBox.Text = "";
            newIncomeGroupBox.Enabled = false;
            MessageBox.Show("Dodany nowa wplate!");
        }

        private void deleteIncomeButton_Click(object sender, EventArgs e) //usuniecie wplaty
        {
            if (paymentGridView.SelectedRows.Count > 0) //jezeli wybrano wiecej niz jeden wiersz
            {
                var selectedIndex = Int32.Parse(paymentGridView.SelectedRows[0].Cells[0].Value.ToString()); //odczytanie wybranego indexu
                _payments.DeleteById(selectedIndex); //usuniecie platnosci o danym ID
                _payments.Save(); //zapisanie platnosci
            }
            LoadPayments();//przeladowanie gridview
        }

        private void saveUpdatePaymentButton_Click(object sender, EventArgs e) //zapisanie aktualizacji platnosci
        {
            Payment p  = _payments.GetById(currentPaymentId);//zczytanie platnosci po id, wczesniej zapisalismy obecne id wybierajac zapisane wczensiej
            p.Amount = float.Parse(newIncomeValueTextBox.Text); //zaktualizowanie platnosci na te pobrana z textboxa 
            p.AccountId = ((Account)newIncomerComboBox.SelectedItem).Id; //zaktualizownie ID konta na id wybango z selecta
            _payments.Update(p); //zaktualizowanie platnosci
            _payments.Save(); //zapisanie platnosci
            LoadWithdrawals(); //odswiezenie wyplat
            newIncomeValueTextBox.Text = ""; //wyczyszczenie  textboxa
            newIncomerComboBox.Items.Clear(); //usuniecie opcji wyboru klinta
            newIncomerComboBox.ResetText(); //usuniecie ostatnio wybranej opcji z pola comboboxa
            editIncomeGroupBox.Enabled = false; //wylaczenie groupboxa edycji
            MessageBox.Show("Zaktualizowano dane wpłaty!"); //wyslanie msg do uzytkownika ze zaktualizoano
        }

        private void checkWhoWithButton_Click(object sender, EventArgs e) //funkcja wyswietalacaja dane wybierajcaego podobnie jak w fkcji nizej
        {
            if (withdrawalGridView.SelectedRows.Count > 0)
            {
                var selectedJumperIndex = Int32.Parse(withdrawalGridView.SelectedRows[0].Cells[0].Value.ToString());
                Withdrawal w = _withdrawals.GetById(selectedJumperIndex);
                Account owner = _accounts.GetById(w.AccountId);
                MessageBox.Show("Wypłacający: " + owner + " jego ID to "+owner.Id);
            }
      
        }

        private void checkWhoIncomeButton_Click(object sender, EventArgs e) //funckcja wyswietalaja dane wplacajecego podobnie jak w fkcji nizej
        {
            if (paymentGridView.SelectedRows.Count > 0)
            {
                var selectedJumperIndex = Int32.Parse(paymentGridView.SelectedRows[0].Cells[0].Value.ToString());
                Payment p = _payments.GetById(selectedJumperIndex);
                Account owner = _accounts.GetById(p.AccountId);
                MessageBox.Show("Wpłacający: " + owner + " jego ID to " + owner.Id);
            }
        }

        private void checkWhoTransferButton_Click(object sender, EventArgs e) //funkcja wyswietlajaca czlonkow akcj transferu
        {
            if (transferGridView.SelectedRows.Count > 0)
            {
                var selectedIndex = Int32.Parse(transferGridView.SelectedRows[0].Cells[0].Value.ToString());
                Transfer p = _transfers.GetById(selectedIndex); //zczytanie transferu
                Account sender2 = _accounts.GetById(p.SenderId); //zczytanie nadawcy
                Account recipient = _accounts.GetById(p.RecipientId); //odbiorcy
                MessageBox.Show("Nadawca: " + sender2 + " jego ID to " + sender2.Id+"\nOdbiorca: "+recipient +" jego ID to "+recipient.Id);
          //Wyswietlenie danych personalnych nadawcy i odbiorcy
            }
        }
        private float CountSaldo (int accountID) //iterowanie po wszystkich transakcjach i zliczenie salda
        {
            float saldo = 0;
            foreach (Transfer t in _transfers.GetAll())
            {
                if (t.RecipientId == accountID)
                    saldo += t.Amount;
                else if (t.SenderId == accountID)
                    saldo -= t.Amount;
            }
            foreach (Payment p in _payments.GetAll())
            {
                if (p.AccountId == accountID)
                    saldo += p.Amount;
            }
            foreach (Withdrawal w in _withdrawals.GetAll())
            {
                if (w.AccountId == accountID)
                    saldo -= w.Amount;
            }
            return saldo;
        }

        private List<Transfer> UserTransfer (int accountID) //funkcja iterujaca po przelewach,zwracajaca zarowno te ktore odbiera i nadaje nasz uztorkownika
        {
            List<Transfer> result = new List<Transfer>();
            foreach (Transfer t in _transfers.GetAll())
            {
                if (t.RecipientId == accountID || t.SenderId == accountID)
                    result.Add(t);
            }
            return result;
        }
        private List<float> UserWithdrawal(int accountID) //funkcja dzialajaca tak jak ponzisza tyle ze iterujaca po wyplatach
        {
            List<float> result = new List<float>();
            foreach (Withdrawal t in _withdrawals.GetAll())
            {
                if (t.AccountId == accountID)
                    result.Add((-1)*t.Amount);
            }
            return result;
        }
        private List<float> UserIncome(int accountID) //funckja generujaca liste wplat uzytkownika
        {
            List<float> result = new List<float>(); //tymczasowa lista ktora bedzie zwracana 
            foreach (Payment t in _payments.GetAll()) //iterowanie po platnosciach
            {
                if (t.AccountId == accountID) //sprawdzenie czy platnosc zostala wykonana przez uzytkownika jaki nas interesuje 
                    result.Add( t.Amount); //dodanie do listy
            }
            return result; //zwrocenie listy
        }
        private void checkSaldoButton_Click(object sender, EventArgs e) //
        {
            if (accountsGridView.SelectedRows.Count > 0) //sprawdzenie czy zostal wybrany jakis rekord
            {
                var selectedIndex = Int32.Parse(accountsGridView.SelectedRows[0].Cells[0].Value.ToString()); //odczytanie wybranego indexu
   
                Account account = _accounts.GetById(selectedIndex); //odczytanie edytowalnego obiektu konta
                
                MessageBox.Show("Saldo konta wynosi: " + CountSaldo(account.Id) + "\n\nPrzelewy: " + string.Join(",", account.TransfersBy) + string.Join(",", account.TransfersTo) + "\n\nWypłaty: \n" + string.Join("\n", account.Withdrawals) +"\n\nWpłaty: \n"+ string.Join("\n+", account.Payments));
            //WYSWIETLENIE SALDA I OPERACJI WYKONANYCH PRZEZ WYBRANEGO UZYTKOWNIKA
            }
        }

        private void editIncomeButton_Click(object sender, EventArgs e) //OPIS W 460 ln
        {
            if (paymentGridView.SelectedRows.Count > 0)
            {
                fillAcounts(newIncomerComboBox);
                editIncomeGroupBox.Enabled = true;
                newIncomeGroupBox.Enabled = false;
                var selectedJumperIndex = Int32.Parse(paymentGridView.SelectedRows[0].Cells[0].Value.ToString());
                newIncomeValueTextBox.Text = _payments.GetById(selectedJumperIndex).Amount.ToString();
                newIncomerComboBox.SelectedItem = _accounts.GetById(_payments.GetById(selectedJumperIndex).AccountId);
                currentPaymentId= _payments.GetById(selectedJumperIndex).Id;
            }
            LoadPayments();
        }

        private void editTransferEnableButton_Click(object sender, EventArgs e) //OPIS W 460 ln
        {
            if (transferGridView.SelectedRows.Count > 0)
            {
                fillAcounts(newRecipientComboBox);
                fillAcounts(newSenderComboBox);
                editTransferGroupBox.Enabled = true;
                addTransferGroupBox.Enabled = false;
                var selectedJumperIndex = Int32.Parse(transferGridView.SelectedRows[0].Cells[0].Value.ToString());
                newRecipientComboBox.SelectedItem = _accounts.GetById(_transfers.GetById(selectedJumperIndex).RecipientId);

                newSenderComboBox.SelectedItem = _accounts.GetById(_transfers.GetById(selectedJumperIndex).SenderId);
                newValueTextBox.Text = _transfers.GetById(selectedJumperIndex).Amount.ToString();

                currentTransferId = _transfers.GetById(selectedJumperIndex).Id;
            }
        }

        //Przycisk wywolujacy panel edycji wyplaty
        private void editWithadrawbutton_Click(object sender, EventArgs  e) //FUNKCJA DZIALA ANOLIGCZNIE W KAZDYM PANELU EDYCJI (WPLAT, UZYTKOWNIKOW, TRANSFEROW)
        {
            if (withdrawalGridView.SelectedRows.Count > 0) //sprawdzenie czy jakis rekord zostal zaznaczony
            {
                fillAcounts(newWithadrawalAuthorCombo); //wypelnienie selecta uzytkownikow dostepnymi userami
               editWithadrawalGroupBox.Enabled = true; //wlaczenie groupboxa odpowiedzialnego za edycje wyplat
               addWithadrawalGroupBox.Enabled = false; //wylaczenie groupboxa odpowiedzialnego za dodawanie wyplay
                var selectedIndex = Int32.Parse(withdrawalGridView.SelectedRows[0].Cells[0].Value.ToString()); //odczytanie indexu wybranego redkordu
                valueNewWithadrawTextBox.Text = _withdrawals.GetById(selectedIndex).Amount.ToString(); //wypelnienie wartosci wyplaty do edycji
                newWithadrawalAuthorCombo.SelectedItem = _accounts.GetById(_withdrawals.GetById(selectedIndex).AccountId); //wypelnienie selecta osoby
                currentWithadrawtId = _withdrawals.GetById(selectedIndex).Id; //ustawienie id edytowalnego rekordu, bedziemy go wykorzystywac przy wysylaniu update'u
            }

        }
    }
}
