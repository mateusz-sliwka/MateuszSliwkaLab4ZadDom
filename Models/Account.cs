using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MateuszSliwkaLab4ZadDom.Models
{
    class Account
    {
        [Key]
        [DisplayName("ID użytkownika")]
        public int Id { get; set; } //id konta

        [Required(ErrorMessage = "Pole imię jest wymagane")]
        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "NVARCHAR")]
        [StringLength(250)]
        [DisplayName("Imię")]
        public string FirstName { get; set; } //imie uzytkownika

        [Required(ErrorMessage = "Pole nazwisko jest wymagane")]
        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "NVARCHAR")]
        [StringLength(250)]
        [DisplayName("Nazwisko")]
        public string LastName { get; set; } //nazwisko uzytkownika

        [Required(ErrorMessage = "Pole PESEL jest wymagane")]
        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "NVARCHAR")]
        [StringLength(250)]
        [DisplayName("PESEL")]
        public string PESEL { get; set; } //pesel uzytkownika

        public virtual ICollection<Payment> Payments { get; set; } //lista platnosci uzytkownika

        public virtual ICollection<Withdrawal> Withdrawals { get; set; } //lista wyplat uzytkownika

        public virtual ICollection<Transfer> TransfersTo { get; set; } //lista odebranych transferow uzytkownika

        public virtual ICollection<Transfer> TransfersBy { get; set; } //lista transferow nadanych uzytkownika

        public override string ToString() //przeciazanie metody ToString
        {
            return FirstName + " " + LastName;
        }
    }
}
