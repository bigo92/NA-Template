﻿using System;
using NA.Domain.Interfaces;
using NA.Domain.Bases;
using Microsoft.Extensions.Logging;

namespace NA.Domain.Services
{
    public interface ITempService : ICRUD
    {

    }

    public class TempService : ITempService, IDisposable
    {
        private readonly IDispatcherFactory _dp;
        private readonly ILogger<TempService> _log;
        public TempService(IDispatcherFactory dp, ILogger<TempService> log)
        {
            _dp = dp;_log = log;
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

        public void Dispose()
        {
            _log.LogError("Dispose Service");
        }
    }
}
