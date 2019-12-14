using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MateuszSliwkaLab4ZadDom.Models
{
    class Withdrawal
    {
        [Key]
        [DisplayName("ID wypłaty")]
        public int Id { get; set; }
        [DisplayName("ID wypłacającego")]
        public int AccountId { get; set; }

        public virtual Account account { get; set; }

        [DisplayName("Kwota wypłaty")]
       public  float Amount { get; set; }
    }
}
