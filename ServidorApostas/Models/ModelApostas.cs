using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ServidorApostas.Models
{
    public partial class ModelApostas
    {
        [Key]
        public int id { get; set; } //Identificação da aposta

        [Required]
        [StringLength(300)]
        public string chave { get; set; } //Chave inserida pelo utilizador

        [Required]
        [StringLength(300)]
        public string data { get; set; } //Data de registo da chave

        public bool registada { get; set; } = false; //Por defeito é false. Indica se a chave foi arquivada ou não na base de dados

        //Serve de conexão entre os models
        public int utilizadorid { get; set; } //Identificação do utilizador
        public ModelUtilizador utilizador { get; set; } 
    }
}
