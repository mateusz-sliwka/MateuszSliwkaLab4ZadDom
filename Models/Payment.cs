using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MateuszSliwkaLab4ZadDom.Models
{
    class Payment
    {
        [Key]
        [DisplayName("ID wpłaty")]
        public int Id { get; set; } //id wplaty

        [DisplayName("ID wpłacajacego")]
        public int AccountId { get; set; } //id uzytkownika wplacajaego

        public virtual Account Account { get; set; } //konto wplacajacego

        [DisplayName("Kwota wpłaty")]
        public float Amount { get; set; } //kwota wplacona

     
    }
}
