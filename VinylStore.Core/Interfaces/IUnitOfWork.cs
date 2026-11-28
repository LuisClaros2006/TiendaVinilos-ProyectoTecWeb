using System;
using System.Collections.Generic;
using System.Text;

namespace VinylStore.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(
            Func<Task> operation);
    }
}

