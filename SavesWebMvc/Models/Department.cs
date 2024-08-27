using System;
using System.Linq;
using SalesWebMvc.Models;
using System.Collections.Generic;
using SalesWebMvc.Models.ViewModels;

namespace salesWebMvc.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Seller> Sellers { get; set; } = new List<Seller>();
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public List<Venda> Vendas { get; set; } = new List<Venda>();

        public Department() 
        {
        }

        public Department(string name)
        {
            Name = name;
        }
        public void AddSeller(Seller seller) 
        {
            Sellers.Add(seller);
        }
        public double TotalSales(DateTime initial, DateTime final) 
        {
            return Sellers.Sum(seller => seller.TotalSales(initial,final));
        }
    }
}
