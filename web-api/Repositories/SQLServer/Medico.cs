using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace web_api.Repositories.SQLServer
{
    public class Medico
    {
        private readonly SqlConnection conn;
        private readonly SqlCommand cmd;

        public Medico(string connectionString)
        {
            this.conn = new SqlConnection(connectionString);
            this.cmd = new SqlCommand();
            this.cmd.Connection = conn;
        }

        public List<Models.Medico> Select()
        {
            List<Models.Medico> medicos = new List<Models.Medico>();

            using (this.conn)
            {
                conn.Open();

                using (this.cmd)
                {                   
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

            using (this.conn)
            {
                conn.Open();

                using (this.cmd)
                {
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

            using (this.conn)
            {
                conn.Open();

                using (this.cmd)
                {
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

        public List<Models.Medico> SelectByNome(string nome)
        {
            List<Models.Medico> medicos = new List<Models.Medico>();

            using (this.conn)
            {
                conn.Open();

                using (this.cmd)
                {
                    cmd.CommandText = "select id, crm, nome from medico where nome like @nome;";
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = $"%{nome}%";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
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

        public bool Insert(Models.Medico medico)
        {
            using (this.conn)
            {
                conn.Open();

                using (this.cmd)
                {
                    cmd.CommandText = "insert into medico(crm, nome)Svalues(@crm, @nome); select convert(int,scope_identity());";
                    cmd.Parameters.Add(new SqlParameter("@crm", SqlDbType.VarChar)).Value = medico.CRM;
                    cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar)).Value = medico.Nome;
                    medico.Id = (int) cmd.ExecuteScalar();
                }
            }

            return medico.Id != 0;
        }

        public bool Update(Models.Medico medico)
        {
            int linhasAfetadas = 0;

            using (this.conn)
            {
                conn.Open();

                using (this.cmd)
                {
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

            using (this.conn)
            {
                conn.Open();

                using  (this.cmd)
                {
                    cmd.CommandText = "delete from medico where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;

                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            return linhasAfetadas == 1;
        }

    }
}