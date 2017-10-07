using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.IService
{
    public interface ICountryService : IDisposable
    {
        IEnumerable<Country> GetAll();
    }
}