using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace web_api.Repositories.SQLServer
{
    public class Medicamento
    {
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;

        public Medicamento(string connectionString)
        {
            this.conn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand();
            this.cmd.Connection = conn;
        }

        public List<Models.Medicamento> Select()
        {
            List<Models.Medicamento> medicamentos = new List<Models.Medicamento>();

            using(this.conn)
            {
                this.conn.Open();

                using (this.cmd)
                {
                    this.cmd.CommandText = "select id, nome, dataFabricacao, dataVencimento from medicamento;";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        { 
                            Models.Medicamento medicamento = new Models.Medicamento();                                                        
                            medicamento.Id = (int)dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);
                            if (dr["dataVencimento"] != DBNull.Value)
                                medicamento.DataVencimento = (DateTime)dr["dataVencimento"];

                            medicamentos.Add(medicamento);                            
                        }
                    }
                }
            }
            return medicamentos;
        }

        public Models.Medicamento Select(int id)
        {
            Models.Medicamento medicamento = null;

            using (this.conn)
            {
                this.conn.Open();

                using (this.cmd)
                {
                    this.cmd.CommandText = "select id, nome, dataFabricacao, dataVencimento from medicamento where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    using (SqlDataReader dr = this.cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            medicamento = new Models.Medicamento();

                            medicamento.Id = (int)dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);
                            if (dr["dataVencimento"] != DBNull.Value)
                                medicamento.DataVencimento = (DateTime)dr["dataVencimento"];

                        }
                    }
                }
            }
            return medicamento;
        }

        public Models.Medicamento Select(string nome)
        {
            Models.Medicamento medicamento = null;

            using (this.conn)
            {
                this.conn.Open();

                using (this.cmd)
                {
                    this.cmd.CommandText = "select id, nome, dataFabricacao, dataVencimento from medicamento where nome like @nome;";
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";

                    using (SqlDataReader dr = this.cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            medicamento = new Models.Medicamento();
                            medicamento.Id = (int)dr["id"];
                            medicamento.Nome = dr["nome"].ToString();
                            medicamento.DataFabricacao = Convert.ToDateTime(dr["datafabricacao"]);
                            if (dr["dataVencimento"] != DBNull.Value)
                                medicamento.DataVencimento = (DateTime)dr["dataVencimento"];
                        }
                    }
                }
            }
            return medicamento;
        }

        public bool Insert(Models.Medicamento medicamento)
        {
            using (this.conn) 
            {
                this.conn.Open();

                using (this.cmd)
                {
                    this.cmd.CommandText = "insert into medicamento(Nome, DataFabricacao, DataVencimento) values(@nome, @datafabricacao, @datavencimento); select convert(int,scope_identity());"; ;
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medicamento.Nome;
                    this.cmd.Parameters.Add(new SqlParameter("@datafabricacao", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    if (medicamento.DataVencimento != null)
                        cmd.Parameters.Add(new SqlParameter("@dataVencimento", System.Data.SqlDbType.Date)).Value = medicamento.DataVencimento;
                    else
                        cmd.Parameters.Add(new SqlParameter("@dataVencimento", System.Data.SqlDbType.Date)).Value = DBNull.Value;

                    medicamento.Id = (int)cmd.ExecuteScalar();
                }            
            }
            return medicamento.Id != 0;
        }

        public bool Update(Models.Medicamento medicamento)
        {
            int linhasAfetadas = 0;

            using (this.conn)
            {
                this.conn.Open();

                using (this.cmd)
                {
                    this.cmd.CommandText = "update medicamento set nome = @nome, datafabricacao = @datafabricacao, datavencimento = @datavencimento where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar)).Value = medicamento.Id;
                    this.cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medicamento.Nome;
                    this.cmd.Parameters.Add(new SqlParameter("@datafabricacao", SqlDbType.Date)).Value = medicamento.DataFabricacao;
                    this.cmd.Parameters.Add(new SqlParameter("@datavencimento", SqlDbType.Date)).Value = medicamento.DataVencimento;

                    linhasAfetadas = this.cmd.ExecuteNonQuery();
                }
            }

            return linhasAfetadas == 1;
        }

        public bool Delete(int id)
        {
            int linhasAfetadas = 0;

            using (this.conn)
            {
                this.conn.Open();

                using (this.cmd)
                {
                    this.cmd.CommandText = "delete from medicamento where id = @id;";
                    this.cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    linhasAfetadas = this.cmd.ExecuteNonQuery();
                }
            }
            return linhasAfetadas == 1;
        }
    }
}