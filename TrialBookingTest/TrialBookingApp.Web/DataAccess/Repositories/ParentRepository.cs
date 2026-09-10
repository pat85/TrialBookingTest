using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.DataAccess.Repositories
{
    public class ParentRepository : IParentRepository
    {
        private readonly AppDbContext _context;

        public ParentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Parent?> GetByIdAsync(Guid id)
        {
            return await _context.Parents
                .FirstOrDefaultAsync(x => x.ParentId == id);
        }

        public async Task<List<Parent>> GetAllAsync()
        {
            return await _context.Parents
                .ToListAsync();
        }

        public async Task AddAsync(Parent parent)
        {
            await _context.Parents.AddAsync(parent);
        }

        public void Update(Parent parent)
        {
            _context.Parents.Update(parent);
        }

        public void Delete(Parent parent)
        {
            _context.Parents.Remove(parent);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Parents
                .AnyAsync(x => x.ParentId == id);
        }
    }
}
