using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await this.repositorioMedico.Select());
        }

        // GET: api/Medicos/id
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Models.Medico medico = await this.repositorioMedico.Select(id);

            if (medico is null)
                return NotFound();

            return Ok(medico);
        }

        // GET: api/Medicos?crm=123456/BH
        [HttpGet]
        public async Task<IHttpActionResult> Get(string CRM)
        {
            Models.Medico medico = await this.repositorioMedico.Select(CRM);

            if (medico is null)
                return NotFound();

            return Ok(medico);
        }

        // GET: api/Medicos?nome=zeca
        [HttpGet]
        public async Task<IHttpActionResult> GetByNome(string nome)
        {
            if (nome.Length < 3)
                return BadRequest("O nome deve ter no mínimo 5 caracteres.");

            return Ok(await this.repositorioMedico.SelectByNome(nome));
        }

        // POST: api/Medicos        
        [HttpPost]
        public async Task<IHttpActionResult> Post(Models.Medico medico)
        {
            if (! await this.repositorioMedico.Insert(medico))
                return InternalServerError();

            return Ok(medico);
        }

        // PUT: api/Medicos/5
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, Models.Medico medico)
        {
            if (id != medico.Id)
                return BadRequest("O id da requisição não coincide com o id do médico");

            if (! await this.repositorioMedico.Update(medico))
                return NotFound();

            return Ok(medico);
        }

        // DELETE: api/Medicos/5
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        { 
            if (!await this.repositorioMedico.Delete(id))
                return NotFound();

            return Ok();
        }
    }
}
