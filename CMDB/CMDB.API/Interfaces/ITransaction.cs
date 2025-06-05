using Microsoft.EntityFrameworkCore.Storage;

namespace CMDB.API.Interfaces
{
    /// <summary>
    /// Interface for transaction management.
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// This method commits the transaction.
        /// </summary>
        void Commit();
        /// <summary>
        /// This method rolls back the transaction.
        /// </summary>
        void Rollback();
    }
    /// <summary>
    /// Implementation of the transaction management interface.
    /// </summary>
    public class DbTransaction : ITransaction
    {
        private readonly IDbContextTransaction _efTransaction;
        /// <summary>
        /// constructor for the DbTransaction class.
        /// </summary>
        /// <param name="efTransaction"></param>
        public DbTransaction(IDbContextTransaction efTransaction)
        {
            _efTransaction = efTransaction;
        }
        /// <inheritdoc />
        public void Commit()
        {
            _efTransaction.Commit();
        }
        /// <inheritdoc />
        public void Rollback()
        {
            _efTransaction.Rollback();
        }
        /// <summary>
        /// Disposes the transaction.
        /// </summary>
        public void Dispose()
        {
            _efTransaction.Dispose();
        }
    }
}