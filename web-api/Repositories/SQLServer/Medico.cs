using System.Collections.Generic;
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

        public List<Models.Medico> GetAll()
        {
            List<Models.Medico> medicos = new List<Models.Medico>();

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $"select id, crm, nome from medico;";
                    cmd.Connection = conn;
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


    }
}