using System;

namespace web_api.Models
{
    public class Medicamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataFabricacao { get; set; }
        public DateTime? DataVencimento { get; set; }
    }
}