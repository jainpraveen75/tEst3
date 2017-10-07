using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.IRepository
{
    public interface ICountryRepository : IDisposable
    {
        IEnumerable<Country> GetAll();
    }
}