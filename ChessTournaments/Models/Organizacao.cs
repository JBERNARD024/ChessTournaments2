using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ChessTournaments.Models
{
    public class Organizacao
    {
        public int Id { get; set; }

        /// <summary>
        /// Nome da Organização
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Nome { get; set; }

        /// <summary>
        /// Email da Organização
        /// </summary>
        [EmailAddress(ErrorMessage = "O {0} não está corretamente escrito")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Email { get; set; }

        /// <summary>
        /// Telemóvel da Organização
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Telemóvel")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "O {0} deve ter {1} dígitos.")]
        [RegularExpression("9[1236][0-9]{7}", ErrorMessage = "O número de {0} deve começar por 91, 92, 93 ou 96, e ter 9 dígitos")]
        public string Telemovel { get; set; }

        /// <summary>
        /// Morada da Organização
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Morada { get; set; }

        ///<<summary>
        ///Código Postal da Organização
        /// </summary>
        [Display(Name = "Código Postal")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "O {0} deve ter {1} dígitos.")]
        [RegularExpression("[1-9][0-9]{3}-[0-9]{3} [A-ZÇÁÉÍÓÚÊÂÎÔÛÀÃÕ ]+",
                         ErrorMessage = "O {0} tem de ser da forma XXXX-XXX LOCALIDADE")]
        public string CodigoPostal { get; set; }
    }
}