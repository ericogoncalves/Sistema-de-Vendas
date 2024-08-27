using salesWebMvc.Models;
using SalesWebMvc.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models
{
    public class Venda
    {
        public int VendaId { get; set; }
        public decimal Valor { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }

        // Relacionamento com Produto
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        // Relacionamento com Vendedor
        public int VendedorId { get; set; }
        public Seller Vendedor { get; set; }

        // Relacionamento com Departamento
        public int DepartamentoId { get; set; }
        public Department Departamento { get; set; }
        public SaleStatus Status { get; set; }

    }

}
