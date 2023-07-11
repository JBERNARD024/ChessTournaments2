using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ChessTournaments.Models
{
    public class Partidas
    {
        public int Id { get; set; }

        //*******************************

        ///<summary>
        ///FK para o Torneio
        /// </summary>

        [ForeignKey(nameof(Torneio))]
        [Display(Name = "Torneio")]

        public int TorneioFK { get; set; }

        public Torneio Torneio { get; set; }

        ///<summary>
        ///FK para o Jogador 1
        /// </summary>

        [ForeignKey(nameof(Jogador1))]
        [Display(Name = "Jogador Peças Brancas")]

        public int? JogadorFK1 { get; set; }

        public Pessoa? Jogador1 { get; set; }

        ///<summary>
        ///FK para o Jogador 2
        /// </summary>

        [ForeignKey(nameof(Jogador2))]
        [Display(Name = "Jogador Peças Pretas")]

        public int? JogadorFK2 { get; set; }

        public Pessoa? Jogador2 { get; set; }

        //********************************

        /// <summary>
        /// Ronda do Torneio em que decorreu a Partida
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Ronda { get; set; }

        /// <summary>
        /// Resultado da Partida
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Resultado { get; set; }

        ///<summary>
        ///Data do dia do jogo
        /// </summary>

        [Display(Name = "Data do Jogo")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime dataJogo { get; set; }

        /// <summary>
        /// Número de Movimentos efetuados na Partida
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public int Movimentos { get; set; }
    }
}