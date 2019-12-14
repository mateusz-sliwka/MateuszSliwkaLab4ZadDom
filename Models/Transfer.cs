using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MateuszSliwkaLab4ZadDom.Models
{
    class Transfer
    {
        [Key]
        [DisplayName("ID przelewu")]
        public int Id { get; set; } 

        [DisplayName("ID nadawcy")]
        public int SenderId { get; set; }

        [DisplayName("ID odbiorcy")]
        public int RecipientId { get; set; }


       public virtual Account Sender { get; set; } //konto nadawcy

        public virtual Account Recipient { get; set; } //konto odbiorc

        [DisplayName("Kwota przelewu")]
        public float Amount { get; set; } //kwota przelewu


        public override string ToString() //przeciazenie metody do string
        { 
            return "\n##OD: "+SenderId.ToString() + " DLA: "+RecipientId.ToString() + " KWOTA: "+ Amount.ToString()+"##";
        }
    }
}
