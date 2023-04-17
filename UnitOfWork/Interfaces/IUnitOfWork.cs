using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bloggers.Repositories.Interfaces;

namespace bloggers.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        void CreateTransaction();
        Task CreateTransactionAsync();
        void Commit();
        Task CommitAsync();
        void Dispose();
        void Rollback();
        Task RollbackAsync();
        Task SaveChangesAsync();
    }
}