using System;
using System.Collections.Generic;
using System.Text;

using VinylStore.Core.Interfaces;
using VinylStore.Infrastructure.Data;

namespace VinylStore.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VinylStoreContext _context;

        public UnitOfWork(VinylStoreContext context)
        {
            _context = context;
        }

        public async Task ExecuteInTransactionAsync(
            Func<Task> operation)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // Ejecutar las operaciones de negocio
                await operation();

                // Guardar cambios pendientes
                await _context.SaveChangesAsync();

                // Confirmar la transacción
                await transaction.CommitAsync();
            }
            catch
            {
                // Revertir todos los cambios de la transacción
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}
