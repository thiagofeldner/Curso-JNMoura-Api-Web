using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace web_api.Repositories.SQLServer
{
    public class Paciente
    {
        private readonly SqlConnection _conn;
        private readonly SqlCommand _cmd;

        public Paciente(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
            _cmd = new SqlCommand();
            _cmd.Connection = _conn;
        }

        public List<Models.Paciente> Select()
        {
            List<Models.Paciente> pacientes = new List<Models.Paciente>();

            using (this._conn)
            {
                _conn.Open();

                using (this._cmd)
                {
                    _cmd.CommandText = $"select codigo, nome, datanascimento from paciente;";
                    _cmd.Connection = _conn;

                    using (SqlDataReader dr = _cmd.ExecuteReader())
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

            using (this._conn)
            {
                _conn.Open();

                using (this._cmd)
                {   
                    _cmd.CommandText = "select codigo, nome, datanascimento from paciente where codigo = @id;";
                    _cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    using (SqlDataReader dr = _cmd.ExecuteReader())
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
            using (this._conn)
            {
                _conn.Open();

                using (this._cmd)
                {
                    _cmd.CommandText = "insert into paciente(nome, datanascimento) values(@nome, @datanascimento); select convert(int,scope_identity());";
                    _cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = paciente.Nome;
                    _cmd.Parameters.Add(new SqlParameter("@datanascimento", SqlDbType.Date)).Value = paciente.DataNascimento;

                    paciente.Codigo = (int) _cmd.ExecuteScalar();
                }
            }

            return paciente.Codigo != 0;
        }

        public bool Update(Models.Paciente paciente) 
        {
            int linhasAfetadas = 0;

            using (this._conn)
            {
                _conn.Open();

                using (this._cmd)
                {
                    _cmd.CommandText = "update paciente set nome = @nome , datanascimento = @datanascimento where codigo = @codigo;";
                    _cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = paciente.Nome;
                    _cmd.Parameters.Add(new SqlParameter("@datanascimento", SqlDbType.Date)).Value = paciente.DataNascimento;
                    _cmd.Parameters.Add(new SqlParameter("codigo", SqlDbType.Int)).Value = paciente.Codigo;

                    linhasAfetadas = _cmd.ExecuteNonQuery();
                }
            }

            return linhasAfetadas == 1;
        }

        public bool Delete(int codigo)
        {
            int linhasAfetadas = 0;

            using (this._conn)
            {
                _conn.Open();

                using (this._cmd)
                {
                    _cmd.CommandText = "delete from paciente where codigo = @codigo;";
                    _cmd.Parameters.Add(new SqlParameter("codigo", SqlDbType.Int)).Value = codigo;
                    linhasAfetadas = _cmd.ExecuteNonQuery();
                }
            }
            return linhasAfetadas == 1;
        }
    }
}