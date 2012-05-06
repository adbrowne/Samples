namespace WidgetServices
{
    using System;

    using NHibernate;

    public class UnitOfWork : IDisposable
    {
        private readonly ISession _session;

        private ITransaction _transaction;

        public UnitOfWork(ISession session)
        {
            this._session = session;
            this._transaction = session.BeginTransaction();
        }

        public ISession Session
        {
            get
            {
                return this._session;
            }
        }

        public void Dispose()
        {
            try
            {
                if (this._transaction != null &&
                    !this._transaction.WasCommitted &&
                    !this._transaction.WasRolledBack)
                    this._transaction.Commit();
                this._transaction = null;
            }
            catch (Exception)
            {
                this.Rollback();
                throw;
            }

        }

        private void Rollback()
        {
            this._transaction.Rollback();
        }
    }
}