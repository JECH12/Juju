using Core.Context;
using Core.Repository;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoreContext _context;

        private bool _disposed;

        private Repository<Customer> _customerRepository;

        private Repository<Logs> _logRepository;

        private Repository<Post> _postRepository;

        public UnitOfWork(CoreContext context)
        {
            _context = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public Repository<Customer> CustomerRepository
        {
            get
            {
                if (_customerRepository == null)
                    _customerRepository = new Repository<Customer>(_context);

                return _customerRepository;
            }
        }

        public Repository<Logs> LogRepository
        {
            get
            {
                if (_logRepository == null)
                    _logRepository = new Repository<Logs>(_context);

                return _logRepository;
            }
        }

        public Repository<Post> PostRepository
        {
            get
            {
                if (_postRepository == null)
                    _postRepository = new Repository<Post>(_context);

                return _postRepository;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Save() => _context.SaveChanges();
    }
}
