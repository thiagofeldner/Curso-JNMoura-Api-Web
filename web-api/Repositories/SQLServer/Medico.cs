using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace web_api.Repositories.SQLServer
{
    public class Medico
    {
        private readonly string connectionString;

        public Medico(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Models.Medico> Select()
        {
            List<Models.Medico> medicos = new List<Models.Medico>();

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"select id, crm, nome from medico;";
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Models.Medico medico = new Models.Medico();

                            medico.Id = (int)dr["id"];
                            medico.CRM = dr["crm"].ToString();
                            medico.Nome = dr["nome"].ToString();

                            medicos.Add(medico);
                        }
                    }
                }
            }
            return medicos;
        }

        public Models.Medico Select(int id)
        {
            Models.Medico medico = null;

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select crm, nome from medico where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            medico = new Models.Medico();
                            medico.Id = id;
                            medico.CRM = dr["crm"].ToString();
                            medico.Nome = dr["nome"].ToString();
                        }
                    }
                }
            }

            return medico;
        }

        public Models.Medico Select(string CRM)
        {
            Models.Medico medico = null;

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "select id, crm, nome from medico where crm = @crm;";
                    cmd.Parameters.Add(new SqlParameter("@crm", SqlDbType.VarChar)).Value = CRM;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            medico = new Models.Medico();
                            medico.Id = (int)dr["id"];
                            medico.CRM = dr["crm"].ToString();
                            medico.Nome = dr["nome"].ToString();
                        }
                    }
                }
            }
            return medico;
        }

        public bool Insert(Models.Medico medico)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "insert into medico(crm, nome)Svalues(@crm, @nome); select convert(int,scope_identity());";
                    cmd.Parameters.Add(new SqlParameter("@crm", SqlDbType.VarChar)).Value = medico.CRM;
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medico.Nome;
                    cmd.Connection = conn;
                    medico.Id = (int) cmd.ExecuteScalar();
                }
            }

            return medico.Id != 0;
        }

        public bool Update(Models.Medico medico)
        {
            int linhasAfetadas = 0;

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "update medico set crm = @crm, nome = @nome where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@crm", SqlDbType.VarChar)).Value = medico.CRM;
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medico.Nome;
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = medico.Id;
                    
                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            return linhasAfetadas == 1;
        }

        public bool Delete(int id)
        {
            int linhasAfetadas = 0;

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "delete from medico where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            return linhasAfetadas == 1;
        }

    }
}