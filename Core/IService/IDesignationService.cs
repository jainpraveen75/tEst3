using System;
using System.Collections;
using System.Collections.Generic;
using Core.Entities;

namespace Core.IService
{
    public interface IDesignationService : IDisposable
    {
        IEnumerable<Designation> GetAll();
    }
}