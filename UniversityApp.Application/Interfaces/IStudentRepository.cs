using System;
using System.Collections.Generic;
using System.Text;

using UniversityApp.Domain.Entities;

namespace UniversityApp.Application.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(int id);

        
    }
}
