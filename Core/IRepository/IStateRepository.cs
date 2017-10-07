using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.IRepository
{
    public interface IStateRepository : IDisposable
    {
        IEnumerable<State> GetAllByCountryId(int countryId);
    }
}