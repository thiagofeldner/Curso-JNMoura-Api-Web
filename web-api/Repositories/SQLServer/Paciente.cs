using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace web_api.Repositories.SQLServer
{
    public class Paciente
    {
        private readonly string connectionString;

        public Paciente(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Models.Paciente> Select()
        {
            List<Models.Paciente> pacientes = new List<Models.Paciente>();

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $"select codigo, nome, datanascimento from paciente;";
                    cmd.Connection = conn;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Models.Paciente paciente = new Models.Paciente();

                            paciente.Codigo = (int)dr["codigo"];
                            paciente.Nome = (string)dr["nome"];
                            paciente.DataNascimento = (DateTime)dr["datanascimento"];

                            pacientes.Add(paciente);
                        }
                    }
                }
            }

            return pacientes;
        }

        public Models.Paciente Select(int id)
        {
            Models.Paciente paciente = null;

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {                    
                    cmd.Connection = conn;
                    cmd.CommandText = "select codigo, nome, datanascimento from paciente where codigo = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            paciente = new Models.Paciente();
                            paciente.Codigo = (int)dr["codigo"];
                            paciente.Nome = dr["nome"].ToString();
                            paciente.DataNascimento = (DateTime)dr["datanascimento"];
                        }
                    }
                }
            }

            return paciente;
        }
        
        public bool Insert(Models.Paciente paciente)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "insert into paciente(nome, datanascimento) values(@nome, @datanascimento); select convert(int,scope_identity());";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = paciente.Nome;
                    cmd.Parameters.Add(new SqlParameter("@datanascimento", SqlDbType.Date)).Value = paciente.DataNascimento;

                    paciente.Codigo = (int)cmd.ExecuteScalar();
                }
            }

            return paciente.Codigo != 0;
        }

        public bool Update(Models.Paciente paciente) 
        {
            int linhasAfetadas = 0;

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "update paciente set nome = @nome , datanascimento = @datanascimento where codigo = @codigo;";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = paciente.Nome;
                    cmd.Parameters.Add(new SqlParameter("@datanascimento", SqlDbType.Date)).Value = paciente.DataNascimento;
                    cmd.Parameters.Add(new SqlParameter("codigo", SqlDbType.Int)).Value = paciente.Codigo;

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            return linhasAfetadas == 1;
        }

        public bool Delete(int codigo)
        {
            int linhasAfetadas = 0;

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "delete from paciente where codigo = @codigo;";
                    cmd.Parameters.Add(new SqlParameter("codigo", SqlDbType.Int)).Value = codigo;
                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }
            return linhasAfetadas == 1;
        }
    }
}