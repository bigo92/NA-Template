using NA.DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace NA.Domain.Interfaces
{
    public interface ICRUDService<T> where T: class
    {
        List<Template> Get();
        string FindOne();
        void Add(T model);
        void Delete(T model);
        void Edit(T model);
    }
}
