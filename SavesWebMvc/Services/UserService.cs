using salesWebMvc.Models;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;
using SalesWebMvc.Models.ViewModels;

namespace SalesWebMvc.Services
{
    public class UserService
    {
        private readonly SalesWebMvcContext _context;

        public UserService(SalesWebMvcContext context) 
        {
            _context = context;
        }

        public async Task<List<User>> FindAllAsync() 
        {
            return await _context.Users.ToListAsync();
        }
        public async Task InsertAsync(User obj) 
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }
        public async Task<User> FindByIdAsync(int id) 
        {
            return await _context.Users.FirstOrDefaultAsync(obj => obj.Id == id);
        }
        public async Task RemoveAsync(int id) 
        {
            try
            {
                var obj = await _context.Users.FindAsync(id);
                _context.Users.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) 
            {
                throw new IntegrityException("Não foi possivel deletar o vendedor.");
            }
        }
        public async Task UpdateAsync(User obj) 
        {
            bool hasAny = await _context.Users.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny) 
            {
                throw new NotFoundException("Id não encontrado.");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e) 
            {
                throw new DbConcurrencyException(e.Message);
            }

        }
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

    }
}
