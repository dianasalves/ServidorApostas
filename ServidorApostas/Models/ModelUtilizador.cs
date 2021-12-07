using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ServidorApostas.Models
{
    public partial class ModelUtilizador
    {
        [Key]
        public int id { get; set; } //Identificação do utilizador

        [Required]
        [StringLength(300)]
        public string nome { get; set; } //Nome do utilizador

        public ICollection<ModelApostas> Apostas { get; set; } //Lista de apostas que cada utilizador tem
    }
}
