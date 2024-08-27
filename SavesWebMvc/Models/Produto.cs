using salesWebMvc.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models
{
    public class Produto
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Preço { get; set; }

        // Relacionamento com Departamento
        public int DepartamentoId { get; set; }
        public Department Departamento { get; set; }

        // Relacionamento com Vendas
        public ICollection<Venda> Vendas { get; set; }
    }
}
