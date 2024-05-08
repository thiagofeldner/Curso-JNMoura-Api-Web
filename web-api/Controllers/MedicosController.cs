using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;

namespace web_api.Controllers
{
    public class MedicosController : ApiController
    {
        private readonly string conectionString;

        public MedicosController()
        {
            this.conectionString = @"Server=TFELDNER\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";
            //this.conectionString = = @"Server=G4F-THIAGOF\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";
        }

        // GET: api/Medicos
        [HttpGet]
        public IHttpActionResult Obter()
        {
            List<Models.Medico> medicos = new List<Models.Medico>();

            using (SqlConnection conn = new SqlConnection(this.conectionString))
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
            return Ok(medicos);
        }

        // GET: api/Medicos?crm=123456/BH
        [HttpGet]
        public IHttpActionResult ObterPorId(string crm)
        {
            Models.Medico medico = new Models.Medico();
            
            using (SqlConnection conn = new SqlConnection(this.conectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    // cmd.CommandText = $"select id, crm, nome from medico where crm = '{crm}';";
                    cmd.CommandText = "select id, crm, nome from medico where crm = @crm;";
                    cmd.Parameters.Add(new SqlParameter("@crm", System.Data.SqlDbType.VarChar)).Value = crm;
                    cmd.Connection = conn;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            medico.Id = (int)dr["id"];
                            medico.CRM = dr["crm"].ToString();
                            medico.Nome = dr["nome"].ToString();
                        }
                    }
                }
            }
            if (medico.CRM == "")
                return NotFound();

            return Ok(medico);
        }

        // GET: api/Medicos/id
        [HttpGet]
        public IHttpActionResult ObterPorCRM(int id)
        {
            Models.Medico medico = new Models.Medico();

            using (SqlConnection conn = new SqlConnection(this.conectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $"select id, crm, nome from medico where id = {id};";
                    cmd.Connection = conn;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            medico.Id = id;
                            medico.CRM = dr["crm"].ToString();
                            medico.Nome = dr["nome"].ToString();
                        }
                    }
                }
            }
            if (medico.CRM == "")
                return NotFound();

            return Ok(medico);
        }

        // POST: api/Medicos        
        [HttpPost]
        public IHttpActionResult Adicionar(Models.Medico medico)
        {
            int linhasAfetadas = 0;
                        
            using (SqlConnection conn = new SqlConnection(this.conectionString))
            {
                conn.Open();
                                
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "insert into medico(crm, nome) values(@crm, @nome);";
                    cmd.Parameters.Add(new SqlParameter("@crm", System.Data.SqlDbType.VarChar)).Value = medico.CRM;
                    cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = medico.Nome;
                    cmd.Connection = conn;
                    linhasAfetadas = cmd.ExecuteNonQuery();
                }                
            }
            
            if (linhasAfetadas == 0)
                return InternalServerError();

            return Ok(medico);
        }

        // PUT: api/Medicos/5
        [HttpPut]
        public IHttpActionResult Atualizar(int id, Models.Medico medico)
        {
            if (id != medico.Id)
                return BadRequest("O id da requisição não coincide com o Id do médico.");

            int linhasAfetadas = 0;

            using (SqlConnection conn = new SqlConnection(this.conectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "update medico set crm = @crm, nome = @nome where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@crm", System.Data.SqlDbType.VarChar)).Value = medico.CRM;
                    cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = medico.Nome;
                    cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.VarChar)).Value = medico.Id;
                    cmd.Connection = conn;
                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            if (linhasAfetadas == 0)
                return InternalServerError();

            return Ok(medico);
        }

        // DELETE: api/Medicos/5
        [HttpDelete]
        public IHttpActionResult Excluir(int id)
        {
            int linhasAfetadas = 0;

            using (SqlConnection conn = new SqlConnection(this.conectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "delete from medico where id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.VarChar)).Value = id;
                    cmd.Connection = conn;
                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            if (linhasAfetadas == 0)
                return NotFound();

            return Ok();
        }
    }
}
