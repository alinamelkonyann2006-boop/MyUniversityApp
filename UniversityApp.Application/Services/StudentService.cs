using System;
using System.Collections.Generic;
using System.Text;
using UniversityApp.Application.Interfaces;
using UniversityApp.Domain.Entities;

namespace UniversityApp.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Student student)
        {
            await _repository.AddAsync(student);
        }

        public async Task UpdateAsync(Student student)
        {
            await _repository.UpdateAsync(student);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}