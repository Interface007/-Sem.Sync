// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseContext.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DatabaseContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsSqlDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using Sem.GenericHelpers;

    public class DatabaseContext
    {
        public DatabaseContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public SerializableDictionary<string, List<DataMapping>> MappingCache { get; set; }

        public IEnumerable<T> OpenTable<T>(string storeProcedureName) where T : new()
        {
            var result = new List<T>();

            using (var connection = new SqlConnection(this.ConnectionString))
            using (var command = new SqlCommand(storeProcedureName, connection))
            {
                connection.Open();
                try
                {
                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    var mappings = this.GetMapping(typeof(T), storeProcedureName);
                    var mappingMax = mappings.Count;

                    while (reader.Read())
                    {
                        var entity = new T();

                        for (var i = 0; i < mappingMax; i++)
                        {
                            Tools.SetPropertyValue(
                                entity, 
                                mappings[i].PathToProperty, 
                                reader.GetValue(reader.GetOrdinal(mappings[i].DatabaseField)).ToString());
                        }

                        result.Add(entity);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }

        private List<DataMapping> GetMapping(Type type, string sourceName)
        {
            var cacheKey = sourceName + "=>" + type.FullName;
            if (!this.MappingCache.ContainsKey(cacheKey))
            {
                var result = new List<DataMapping>();

                this.MappingCache.Add(cacheKey, result);
            }

            return this.MappingCache[sourceName];
        }
    }
}
