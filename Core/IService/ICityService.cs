using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.IService
{
    public interface ICityService : IDisposable
    {
        IEnumerable<City> GetAllByStateId(int stateId);
    }
}