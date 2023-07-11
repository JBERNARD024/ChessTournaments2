using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ChessTournaments.Models
{
    public class Torneio
    {
        public int Id { get; set; }

        /// <summary>
        /// Nome do Torneio
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Data de Início do Torneio
        /// </summary>

        [Display(Name = "Data de Início do Torneio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime dataInicio { get; set; }

        /// <summary>
        /// Data de Fim do Torneio
        /// </summary>

        [Display(Name = "Data de Fim do Torneio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime dataFim { get; set; }

        /// <summary>
        /// Local em que se relizou o Torneio
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Local { get; set; }

        /// <summary>
        /// 
        /// </summary>

        [ForeignKey(nameof(Organizacao))]
        [Display(Name = "Organização")]
        public int OrganizacaoFK { get; set; }

        public Organizacao? Organizacao { get; set; }

        /// <summary>
        /// Valor de todos os prêmios definidos pela organização
        /// </summary>
        /// 
        [Display(Name = "Valor Total dos Prémios do Torneio")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public int ValorPremio { get; set; }

        /// <summary>
        /// Descrição do Torneio
        /// </summary>

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Descricao { get; set; }

        /// <summary>
        /// Website do Torneio
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Website { get; set; }


    }
}