using System;
using NA.Domain.Bases;
using NA.Domain.Interfaces;

namespace NA.Domain.Services
{
    public interface ITempService2 : ICRUD
    {

    }

    public class TempService2 : ITempService2
    {
        public TempService2()
        {

        }

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
