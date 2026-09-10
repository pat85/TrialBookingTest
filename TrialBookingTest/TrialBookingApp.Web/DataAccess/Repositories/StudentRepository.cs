using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TrialBookingApp.Web.Business.Interfaces.Repositories;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.DataAccess.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _context.Students
                .FirstOrDefaultAsync(x => x.StudentId == id);
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students
                .ToListAsync();
        }

        public async Task<List<Student>> GetByParentIdAsync(Guid parentId)
        {
            return await _context.Students
                .Where(x => x.ParentId == parentId)
                .ToListAsync();
        }

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public void Update(Student student)
        {
            _context.Students.Update(student);
        }

        public void Delete(Student student)
        {
            _context.Students.Remove(student);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Students
                .AnyAsync(x => x.StudentId == id);
        }
    }
}
