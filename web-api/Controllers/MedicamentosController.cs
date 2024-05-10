using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace web_api.Controllers
{
    public class MedicamentosController : ApiController
    {
        private readonly Repositories.SQLServer.Medicamento repositorioMedicamento;

        public MedicamentosController()
        {
            this.repositorioMedicamento = new Repositories.SQLServer.Medicamento(Configurations.Database.getConnectionString());
        }

        // GET: api/Medicamentos
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(this.repositorioMedicamento.Select());
        }

        // GET: api/Medicamentos/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Models.Medicamento medicamento = this.repositorioMedicamento.Select(id);

            if (medicamento is null)
                return NotFound();

            return Ok(medicamento);
        }

        [HttpGet]
        public IHttpActionResult Get(string nome) {

            if (nome.Length < 3)
                return BadRequest("O nome deve ter no mínimo 3 caracteres.");

            return Ok(this.repositorioMedicamento.Select(nome));
        }

        // POST: api/Medicamentos
        [HttpPost]
        public IHttpActionResult Post(Models.Medicamento medicamento)
        {
            if (!this.repositorioMedicamento.Insert(medicamento))
                return InternalServerError();

            return Ok(medicamento);
        }

        // PUT: api/Medicamentos/5
        [HttpPut]
        public IHttpActionResult Put(int id, Models.Medicamento medicamento)
        {
            if (id != medicamento.Id)
                return BadRequest("O id da requisição não coincide com o id do medicamento");

            if (!this.repositorioMedicamento.Update(medicamento))
                return NotFound();

            return Ok(medicamento);
        }

        // DELETE: api/Medicamentos/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (!this.repositorioMedicamento.Delete(id))
                return NotFound();

            return Ok();
        }
    }
}
