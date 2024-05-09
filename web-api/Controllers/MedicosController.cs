using System.Web.Http;

namespace web_api.Controllers
{
    public class MedicosController : ApiController
    {
        private readonly Repositories.SQLServer.Medico repositorioMedico;

        public MedicosController()
        {
            this.repositorioMedico = new Repositories.SQLServer.Medico(Configurations.Database.getConnectionString());
        }

        // GET: api/Medicos
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(this.repositorioMedico.Select());
        }

        // GET: api/Medicos/id
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Models.Medico medico = this.repositorioMedico.Select(id);

            if (medico is null)
                return NotFound();

            return Ok(medico);
        }

        // GET: api/Medicos?crm=123456/BH
        [HttpGet]
        public IHttpActionResult Get(string CRM)
        {
            Models.Medico medico = this.repositorioMedico.Select(CRM);

            if (medico is null)
                return NotFound();

            return Ok(medico);
        }

        // POST: api/Medicos        
        [HttpPost]
        public IHttpActionResult Post(Models.Medico medico)
        {
            if (!this.repositorioMedico.Insert(medico))
                return InternalServerError();

            return Ok(medico);
        }

        // PUT: api/Medicos/5
        [HttpPut]
        public IHttpActionResult Put(int id, Models.Medico medico)
        {
            if (id != medico.Id)
                return BadRequest("O id da requisição não coincide com o id do médico");

            if (!this.repositorioMedico.Update(medico))
                return NotFound();

            return Ok(medico);
        }

        // DELETE: api/Medicos/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (!this.repositorioMedico.Delete(id))
                return NotFound();

            return Ok();
        }
    }
}
