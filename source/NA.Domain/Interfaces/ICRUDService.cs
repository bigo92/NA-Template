using System;
using System.Collections.Generic;
using System.Text;

namespace NA.Domain.Interfaces
{
    public interface ICRUDService
    {
        void Get<T>(T model);

        string FindOne();

        void Add<T>(T model);

        void Edit<T>(T model);

        void Delete<T>(T model);
    }
}
