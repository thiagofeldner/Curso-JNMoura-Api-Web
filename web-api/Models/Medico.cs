using System.ComponentModel.DataAnnotations;

namespace web_api.Models
{
    public class Medico
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="CRM é obrigatório.")]
        [StringLength(9, ErrorMessage = "CRM deve ter no máximo 9 caracteres.")]
        public string CRM { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage ="Números e caracteres especiais não são permitidos no Nome.")]
        public string Nome { get; set; }

        //public List<string> Especialidades { get; set; }

        public Medico()
        {
            this.CRM = string.Empty;
            this.Nome = "";
            //this.Especialidades = new List<string>();
        }
    }
}