using Core.Repository;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UnitOfWork
{
    public  interface IUnitOfWork: IDisposable
    {
        int Save();
        Repository<Customer> CustomerRepository { get; }
        Repository<Logs> LogRepository { get; }
        Repository<Post> PostRepository { get; }
    }
}
