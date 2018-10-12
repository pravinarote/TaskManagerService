using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using TaskManager.DataLayer.Repository;

namespace TaskManager.DataLayer.UnitOfWork
{
    /// <summary>  
    /// Unit of Work class responsible for DB transactions  
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private TaskManagerContext _context = null;
        private ITaskRepository _repository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UnitOfWork()
        {
            _context = new TaskManagerContext();
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                if (this._repository == null)
                    this._repository = new TaskRepository(_context);
                return _repository;
            }
        }

        /// <summary>  
        /// Save method.  
        /// </summary>  
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }

        }

        private bool disposed = false;

        /// <summary>  
        /// Protected Virtual Dispose method  
        /// </summary>  
        /// <param name="disposing"></param>  
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>  
        /// Dispose method  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
