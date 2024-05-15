using System;
using System.ComponentModel.DataAnnotations;

namespace web_api.Models
{
    public class Medicamento
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]        
        public DateTime DataFabricacao { get; set; }
        public DateTime? DataVencimento { get; set; }
    }
}