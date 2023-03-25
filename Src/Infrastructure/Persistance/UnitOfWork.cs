using System;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistance.Repositories;

namespace Infrastructure.Persistance
{
	public class UnitOfWork : IUnitOfWork
	{
        private readonly WgDbContext _context;
        private bool _disposed;
        public ICompanyRepository CompanyRepository 
        {
            get
            {
                if (_companyRepository is null)
                    _companyRepository = new CompanyRepository(_context);
                return _companyRepository;
            }
        }
        private CompanyRepository _companyRepository;
        public IEmployeeRepository EmployeeRepository         {
            get
            {
                if (_employeeRepository is null)
                    _employeeRepository = new EmployeeRepository(_context);
                return _employeeRepository;
            }
        }
        private EmployeeRepository _employeeRepository;
        public UnitOfWork(WgDbContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync() => await _context.SaveChangesAsync();
        public async ValueTask DisposeAsync()
        {
            await DisposedAsync(true);
            GC.SuppressFinalize(this);
        }
        protected virtual async ValueTask DisposedAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    await _context.DisposeAsync();
                }
                _disposed = true;
            }
        }
    }
}

