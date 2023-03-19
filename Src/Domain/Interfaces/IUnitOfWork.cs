using System;
using Domain.Interfaces.Repositories;

namespace Domain.Interfaces
{
	public interface IUnitOfWork : IAsyncDisposable
    {
        ICompanyRepository CompanyRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        Task CompleteAsync();
    }
}

