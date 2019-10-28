using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Roulette.Infrastructure.DBProvider
{
    public abstract class DataAccessLayerBase : IDisposable
    {
        public static IConfiguration Configuration { get; set; }

        private static string _connectionString => Configuration["connectionString"];

        #region Constructors

        protected DataAccessLayerBase()
        {
            m_connectionTimeout = 180;
            m_connectionString = _connectionString;
        }

        protected DataAccessLayerBase(int timeOut)
        {
            m_connectionTimeout = timeOut;
            m_connectionString = _connectionString;
        }

        protected DataAccessLayerBase(string connString, int timeOut)
        {
            m_connectionTimeout = timeOut;
            m_connectionString = connString;
        }

        #endregion

        #region Properties

        private readonly int m_connectionTimeout;
        private MySqlConnection m_connection;
        private MySqlTransaction m_transaction;

        private readonly string m_connectionString;

        private ConnectionState State => m_connection?.State ?? ConnectionState.Closed;

        private IDbConnection SQL => m_connection;

        #endregion

        #region Base Methods

        public void Open()
        {
            if (m_connection == null)
                m_connection = new MySqlConnection(m_connectionString);

            if (State == ConnectionState.Broken || State == ConnectionState.Closed)
                m_connection.Open();
        }

        public void Close()
        {
            if (m_connection != null && State != ConnectionState.Closed)
                m_connection.Close();
        }

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        public void BeginTransaction(IsolationLevel il)
        {
            if (State != ConnectionState.Open)
                m_connection.Open();

            m_transaction = m_connection.BeginTransaction(il);
        }

        public void CommitTransaction()
        {
            m_transaction?.Commit();
        }

        public void RollBackTransaction()
        {
            m_transaction?.Rollback();
        }

        public void Dispose()
        {
            m_connection?.Dispose();
            m_transaction?.Dispose();
            SQL?.Dispose();
        }

        #endregion

        #region Query Methods

        internal async Task<int> ExecuteScalarAsync(string command, object param)
        {
            return await SQL.ExecuteScalarAsync<int>(command, param, commandType: CommandType.Text);
        }

        internal Task<int> ExecuteAsync(string command)
        {
            return SQL.ExecuteAsync(command, null, m_transaction, null, CommandType.Text);
        }

        internal Task<int> ExecuteAsync(string command, object param)
        {
            return SQL.ExecuteAsync(command, param, m_transaction, null, CommandType.Text);
        }

        internal Task<IEnumerable<T>> QueryAsync<T>(string command)
        {
            return SQL.QueryAsync<T>(command, null, m_transaction, null, CommandType.Text);
        }

        internal async Task<IEnumerable<T>> QueryAsync<T>(string command, object param)
        {
            return await SQL.QueryAsync<T>(command, param, m_transaction, null, CommandType.Text);
        }

        internal async Task<T> QueryFirstOrDefaultAsync<T>(string command)
        {
            return await SQL.QueryFirstOrDefaultAsync<T>(command, null, m_transaction, m_connectionTimeout, CommandType.Text);
        }

        internal async Task<T> QueryFirstOrDefaultAsync<T>(string command, object param)
        {
            return await SQL.QueryFirstOrDefaultAsync<T>(command, param, m_transaction, m_connectionTimeout, CommandType.Text);
        }

        #endregion
    }
}