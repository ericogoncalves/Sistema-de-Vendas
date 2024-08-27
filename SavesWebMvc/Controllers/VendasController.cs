using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Models.Enums;

namespace SalesWebMvc.Controllers
{
    [Authorize]
    public class VendasController : Controller
    {
        private readonly SalesWebMvcContext _context;

        public VendasController(SalesWebMvcContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
            var vendas = _context.Vendas
                .Include(v => v.Produto)
                .Include(v => v.Departamento)
                .Include(v => v.Vendedor);
            return View(await vendas.ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Produto)
                .Include(v => v.Departamento)
                .Include(v => v.Vendedor)
                .FirstOrDefaultAsync(m => m.VendaId == id);

            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome");
            ViewData["DepartamentoId"] = new SelectList(_context.Department, "Id", "Name");
            ViewData["VendedorId"] = new SelectList(_context.Seller, "Id", "Name");
            return View();
        }

        // POST: Vendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendaId,Valor,Data,ProdutoId,VendedorId,DepartamentoId,Status")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                // Adiciona a venda na tabela Venda
                _context.Add(venda);

                // Cria um registro na tabela SalesRecord usando os dados da venda
                var salesRecord = new SalesRecord
                {
                    Date = venda.Data,
                    Amount = (double)venda.Valor,
                    Status = venda.Status, // Usando o status da venda
                    SellerId = venda.VendedorId,
                    Seller = venda.Vendedor
                };

                // Adiciona o registro na tabela SalesRecord
                _context.Add(salesRecord);

                // Salva ambas as mudanças no banco de dados
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome", venda.ProdutoId);
            ViewData["DepartamentoId"] = new SelectList(_context.Department, "Id", "Name", venda.DepartamentoId);
            ViewData["VendedorId"] = new SelectList(_context.Seller, "Id", "Name", venda.VendedorId);
            return View(venda);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome", venda.ProdutoId);
            ViewData["DepartamentoId"] = new SelectList(_context.Department, "Id", "Name", venda.DepartamentoId);
            ViewData["VendedorId"] = new SelectList(_context.Seller, "Id", "Name", venda.VendedorId);
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(SaleStatus)), venda.Status);
            return View(venda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendaId,Valor,Data,ProdutoId,VendedorId,DepartamentoId,Status")] Venda venda)
        {
            if (id != venda.VendaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.VendaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "ProdutoId", "Nome", venda.ProdutoId);
            ViewData["DepartamentoId"] = new SelectList(_context.Department, "Id", "Name", venda.DepartamentoId);
            ViewData["VendedorId"] = new SelectList(_context.Seller, "Id", "Name", venda.VendedorId);
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(SaleStatus)), venda.Status);
            return View(venda);
        }


        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Produto)
                .Include(v => v.Departamento)
                .Include(v => v.Vendedor)
                .FirstOrDefaultAsync(m => m.VendaId == id);

            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.VendaId == id);
        }
    }
}
