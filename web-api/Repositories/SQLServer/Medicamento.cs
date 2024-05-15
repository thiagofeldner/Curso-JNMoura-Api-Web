using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace web_api.Repositories.SQLServer
{
    public class Medicamento
    {
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;

        private readonly MemoryCache _cache;
        private readonly CacheItemPolicy _policy;
        private readonly string cacheKey = "medicamentoKey";

        public Medicamento(string connectionString)
        {
            this.conn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand
            {
                Connection = conn
            };

            _cache = MemoryCache.Default;
            _policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30)
            };
        }        

        public async Task<List<Models.Medicamento>> Select()
        {
            var cacheItem = _cache.Get(cacheKey);

            if (cacheItem is null)
            {
                List<Models.Medicamento> medicamentos = new List<Models.Medicamento>();

                using (this.conn)
                {
                    await this.conn.OpenAsync();

                    using (this.cmd)
                    {
                        this.cmd.CommandText = "select id, nome, dataFabricacao, dataVencimento from medicamento;";

                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            while (await dr.ReadAsync())
                            {
                                Models.Medicamento medicamento = new Models.Medicamento();
                                medicamento.Id = (int)dr["id"];
                                medicamento.Nome = dr["nome"].ToString();
                                medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);

                                if (!(dr["dataVencimento"] is DBNull))
                                    medicamento.DataVencimento = Convert.ToDateTime(dr["dataVencimento"]);

                                medicamentos.Add(medicamento);
                            }
                        }
                    }
                }

                _cache.Add(cacheKey, medicamentos, _policy);
                return medicamentos;
            }
            return (List<Models.Medicamento>) cacheItem;
        }

        public async Task<Models.Medicamento> Select(int id)
        {
            Models.Medicamento medicamento = null;

            using (this.conn)
            {
                await this.conn.OpenAsync();

                using (this.cmd)
                {
                    this.cmd.CommandText = "select id, nome, dataFabricacao, dataVencimento from medicamento where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    using (SqlDataReader dr = await this.cmd.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            medicamento = new Models.Medicamento();

                            medicamento.Id = (int)dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);

                            if (!(dr["dataVencimento"] is DBNull))
                                medicamento.DataVencimento = Convert.ToDateTime(dr["dataVencimento"]);

                        }
                    }
                }
            }
            return medicamento;
        }

        public async Task<List<Models.Medicamento>> Select(string nome)
        {
            List<Models.Medicamento> medicamentos = new List<Models.Medicamento>();

            using (this.conn)
            {
                await this.conn.OpenAsync();

                using (this.cmd)
                {
                    this.cmd.CommandText = "select id, nome, dataFabricacao, dataVencimento from medicamento where nome like @nome;";
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";

                    using (SqlDataReader dr = await this.cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Models.Medicamento medicamento = new Models.Medicamento();
                            medicamento.Id = (int)dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);

                            if (!(dr["dataVencimento"] is DBNull))
                                medicamento.DataVencimento = Convert.ToDateTime(dr["dataVencimento"]);

                            medicamentos.Add(medicamento);
                        }
                    }
                }
            }
            return medicamentos;
        }

        public async Task<bool> Insert(Models.Medicamento medicamento)
        {
            using (this.conn) 
            {
                await this.conn.OpenAsync();

                using (this.cmd)
                {
                    this.cmd.CommandText = "insert into medicamento(Nome, DataFabricacao, DataVencimento) values(@nome, @datafabricacao, @datavencimento); select convert(int,scope_identity());";
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medicamento.Nome;
                    this.cmd.Parameters.Add(new SqlParameter("@datafabricacao", SqlDbType.Date)).Value = medicamento.DataFabricacao;

                    if (medicamento.DataVencimento != null)
                        cmd.Parameters.Add(new SqlParameter("@dataVencimento", SqlDbType.Date)).Value = medicamento.DataVencimento;
                    else
                        cmd.Parameters.Add(new SqlParameter("@dataVencimento", SqlDbType.Date)).Value = DBNull.Value;

                    medicamento.Id = (int)await cmd.ExecuteScalarAsync();

                    this._cache.Remove(cacheKey);
                }            
            }
            return medicamento.Id != 0;
        }

        public async Task<bool> Update(Models.Medicamento medicamento)
        {
            int linhasAfetadas = 0;

            using (this.conn)
            {
                await this.conn.OpenAsync();

                using (this.cmd)
                {
                    this.cmd.CommandText = "update medicamento set nome = @nome, datafabricacao = @datafabricacao, datavencimento = @datavencimento where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar)).Value = medicamento.Id;
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medicamento.Nome;
                    this.cmd.Parameters.Add(new SqlParameter("@datafabricacao", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    this.cmd.Parameters.Add(new SqlParameter("@datavencimento", SqlDbType.Date)).Value = medicamento.DataVencimento;

                    linhasAfetadas = await this.cmd.ExecuteNonQueryAsync();
                    this._cache.Remove(cacheKey);
                }
            }
            return linhasAfetadas == 1;
        }

        public async Task<bool> Delete(int id)
        {
            int linhasAfetadas = 0;

            using (this.conn)
            {
                await this.conn.OpenAsync();

                using (this.cmd)
                {
                    this.cmd.CommandText = "delete from medicamento where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    linhasAfetadas = await this.cmd.ExecuteNonQueryAsync();
                    this._cache.Remove(cacheKey);
                }
            }
            return linhasAfetadas == 1;
        }
    }
}