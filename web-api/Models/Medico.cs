namespace web_api.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string CRM { get; set; }
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