using System;
using System.Collections.Generic;
using System.Text;
using NA.Domain.Interfaces;

namespace NA.Domain.Services
{
    public interface ITempService : ICRUD
    {

    }

    public class TempService : ITempService
    {
        public void Get<T>(T model)
        {
            throw new NotImplementedException();
        }

        public string FindOne()
        {
            return "1231";
        }

        public void Add<T>(T model)
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T model)
        {
            throw new NotImplementedException();
        }

        public void Edit<T>(T model)
        {
            throw new NotImplementedException();
        }
    }
}
