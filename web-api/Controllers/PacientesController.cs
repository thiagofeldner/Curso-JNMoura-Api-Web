using System.Data.SqlClient;
using System.Web.Http;

namespace web_api.Controllers
{
    public class PacientesController : ApiController
    {
        private readonly Repositories.SQLServer.Paciente repositorioPaciente;

        public PacientesController()
        {
            this.repositorioPaciente = new Repositories.SQLServer.Paciente(Configurations.Database.getConnectionString());
        }

        // GET: api/Pacientes
        public IHttpActionResult Get()
        {
            return Ok(this.repositorioPaciente.Select());
        }

        // GET: api/Pacientes/5
        public IHttpActionResult Get(int id)
        {
            Models.Paciente paciente = this.repositorioPaciente.Select(id);
                    
            if (paciente is null)
                return NotFound();
            
            return Ok(paciente);
        }

        // POST: api/Pacientes
        public IHttpActionResult Post(Models.Paciente paciente)
        {
            if(!this.repositorioPaciente.Insert(paciente))
                return InternalServerError();

            return Ok(paciente);

        }

        // PUT: api/Pacientes/5
        public IHttpActionResult Put(int id, Models.Paciente paciente)
        {
            if (id != paciente.Codigo)
                return BadRequest("O Id da requisição não coincide com o código do paciente.");

            if (!this.repositorioPaciente.Update(paciente))
                return NotFound();
                    
            return Ok(paciente);
        }

        // DELETE: api/Pacientes/5
        public IHttpActionResult Delete(int id)
        {
            if (!this.repositorioPaciente.Delete(id))
                return NotFound();

            return Ok();
        }
    }
}
