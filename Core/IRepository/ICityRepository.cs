using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.IRepository
{
    public interface ICityRepository : IDisposable
    {
        IEnumerable<City> GetAllByStateId(int stateId);
    }
}