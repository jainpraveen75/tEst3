using System;
using System.Collections.Generic;
using Core.Entities;

namespace Core.IService
{
    public interface IStateService : IDisposable
    {
        IEnumerable<State> GetAllByCountryId(int countryId);
    }
}