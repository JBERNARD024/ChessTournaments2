using System.ComponentModel.DataAnnotations.Schema;

namespace ChessTournaments.Models
{
    public class Fotografia
    {
        public int Id { get; set; }

        ///<summary>
        ///Nome do documento com a fotografia da pessoa
        /// </summary>
        public string NomeFicheiro { get; set; }

        ///<summary>
        ///Data em que a fotografia foi tirada
        /// </summary>

        public DateTime Data { get; set; }

        ///<summary>
        ///Local onde a fotografia foi tirada
        /// </summary>

        public string Local { get; set; }

        /**********************************************************/

        ///<summary>
        ///FK para identificar a Pessoa a quem a Fotografia pertence
        /// </summary>
        [ForeignKey(nameof(Pessoa))]

        public int? PessoaFK { get; set; }

        public Pessoa Pessoa { get; set; }

        /**********************************************************/

        ///<summary>
        ///FK para identificar a Equipa a quem a Fotografia pertence
        /// </summary>
        [ForeignKey(nameof(Equipa))]

        public int? EquipaFK { get; set; }

        public Equipa Equipa { get; set; }
    }
}