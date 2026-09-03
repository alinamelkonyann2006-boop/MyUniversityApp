using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace UniversityApp.Application.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetAsync(int id);

    Task<T> PostAsync(T entity);

    Task<T> PutAsync(T entity);
}
