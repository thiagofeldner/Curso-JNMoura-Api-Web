using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Web.Http;
using web_api.Models;

namespace web_api.Controllers
{
    public class PacientesController : ApiController
    {
        // GET: api/Pacientes
        public IHttpActionResult Get()
        {
            List<Models.Paciente> pacientes = new List<Models.Paciente> ();

            //string conectionString = @"Server=G4F-THIAGOF\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";
            string conectionString = @"Server=TFELDNER\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(conectionString))
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

            return Ok(pacientes);
        }

        // GET: api/Pacientes/5
        public IHttpActionResult Get(int id)
        {
            Models.Paciente paciente = new Models.Paciente();

            //string conectionString = @"Server=G4F-THIAGOF\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";
            string conectionString = @"Server=TFELDNER\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(conectionString))
            {
                conn.Open();                
                
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = $"select codigo, nome, datanascimento from paciente where codigo = {id};";
                    //cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = paciente.Codigo;
                    cmd.Connection = conn;
                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            paciente.Codigo = (int)dr["codigo"];
                            paciente.Nome = dr["nome"].ToString();
                            paciente.DataNascimento = (DateTime)dr["datanascimento"];
                        }
                    }                   
                }
            }

            if (paciente.Codigo == 0)
                return NotFound();
            
            return Ok(paciente);
        }

        // POST: api/Pacientes
        public IHttpActionResult Post(Models.Paciente paciente)
        {
            //Conexão = String de conexão
            //SGBD: TFELDNER\SQLEXPRESS - casa
            //SGBD: G4F-THIAGOF\SQLEXPRESS - Empresa
            //Base: consultorio
            //String de conexão:Server=TFELDNER\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;

            //int linhasAfetadas = 0;
            //string conectionString = @"Server=G4F-THIAGOF\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";

            string conectionString = @"Server=TFELDNER\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(conectionString))
            {
                conn.Open();
                
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "insert into paciente(nome, datanascimento) values(@nome, @datanascimento); select convert(int, @@IDENTITY) as codigo;";
                    cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = paciente.Nome;
                    cmd.Parameters.Add(new SqlParameter("@datanascimento", System.Data.SqlDbType.DateTime)).Value = paciente.DataNascimento;
                    cmd.Connection = conn;
                    paciente.Codigo = (int) cmd.ExecuteScalar();
                }
            }

            if (paciente.Codigo == 0)
                return InternalServerError();

            //return Ok(paciente);
            //return Created($"api/pacientes/{paciente.Codigo}", paciente);
            return Content(HttpStatusCode.Created, paciente);
        }

        // PUT: api/Pacientes/5
        public IHttpActionResult Put(int id, Models.Paciente paciente)
        {
            if (id != paciente.Codigo)
                return BadRequest("O Id da requisição não coincide com o código do paciente.");

            int linhasAfetadas = 0;

            //string conectionString = @"Server=G4F-THIAGOF\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";
            string conectionString = @"Server=TFELDNER\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(conectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "update paciente set nome = @nome , datanascimento = @datanascimento where codigo = @id;";
                    cmd.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = paciente.Nome;
                    cmd.Parameters.Add(new SqlParameter("@datanascimento", System.Data.SqlDbType.DateTime)).Value = paciente.DataNascimento.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add(new SqlParameter("id", System.Data.SqlDbType.Int)).Value = paciente.Codigo;

                    cmd.Connection = conn;
                    linhasAfetadas = cmd.ExecuteNonQuery();
                }
            }

            if (linhasAfetadas == 0)
                return NotFound();
                    
            return Ok(paciente);

        }

        // DELETE: api/Pacientes/5
        public IHttpActionResult Delete(int id)
        {
            int linhasAfetadas = 0;

            //string conectionString = @"Server=G4F-THIAGOF\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";
            string conectionString = @"Server=TFELDNER\SQLEXPRESS;Database=consultorio;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(conectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "delete from paciente where codigo = @id;";
                    cmd.Parameters.Add(new SqlParameter("id", System.Data.SqlDbType.Int)).Value = id;
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
