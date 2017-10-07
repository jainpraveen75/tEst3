using System;
using System.Collections;
using System.Collections.Generic;
using Core.Entities;

namespace Core.IRepository
{
    public interface IDesignationRepository : IDisposable
    {
        IEnumerable<Designation> GetAll();
    }
}