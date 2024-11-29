using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Npgsql;

namespace App.Services.DataBaseService
{
    public class DataBaseService : IDataBaseService
    {
        private readonly string _connectionString;
        private readonly List<NpgsqlParameter> _parameters;

        public DataBaseService(string connectionString)
        {
            _connectionString = connectionString;
            _parameters = new List<NpgsqlParameter>();
        }

        public IDataBaseService AddParameter<T>(string parameterName, T value)
        {
            var param = new NpgsqlParameter(parameterName, value);
            _parameters.Add(param);
            return this;
        }

        public int ExecuteNonQuery(string query, bool isStoredProc = false)
        {
            int noOfAffectedRows = 0;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    if (isStoredProc)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    cmd.Parameters.AddRange(_parameters.ToArray());

                    connection.Open();
                    noOfAffectedRows = cmd.ExecuteNonQuery();
                }
            }

            _parameters.Clear();
            return noOfAffectedRows;
        }

        public IEnumerable<T> ExecuteQuery<T>(string query)
        {
            IList<T> list = new List<T>();
            Type type = typeof(T);

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                using (var cmd = new NpgsqlCommand(query, connection))
                {

                    cmd.Parameters.AddRange(_parameters.ToArray());

                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T obj = (T)Activator.CreateInstance(type);
                            type.GetProperties().ToList().ForEach(p =>
                            {
                                if (reader[p.Name] != DBNull.Value)
                                {
                                    p.SetValue(obj, reader[p.Name]);
                                }
                            });
                            list.Add(obj);
                        }
                    }
                }
            }

            _parameters.Clear();
            return list;
        }

        public T ExecuteScalar<T>(string query, bool isStoredProc = false)
        {
            T result = default;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    if (isStoredProc)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    cmd.Parameters.AddRange(_parameters.ToArray());

                    connection.Open();
                    result = (T)cmd.ExecuteScalar();
                }
            }

            _parameters.Clear();
            return result;
        }
    }
}