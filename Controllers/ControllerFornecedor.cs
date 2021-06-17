using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETCore_StoredProcs.Data;
using ASPNETCore_StoredProcs.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCore_StoredProcs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerFornecedor : ControllerBase
    {
        private readonly ValuesRepositoryFornecedor _repository;

        public ControllerFornecedor(ValuesRepositoryFornecedor repository)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET api/ControllerFornecedor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelFornecedor>>> Get()
        {
            return await _repository.GetAll();
        }

        // GET api/ControllerFornecedor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelFornecedor>> Get(int id)
        {
            var response = await _repository.GetById(id);
            if (response == null) { return NotFound(); }
            return response;
        }

        // POST api/ControllerFornecedor
        [HttpPost]
        public async Task Post([FromBody] ModelFornecedor value)
        {
            await _repository.Insert(value);
        }

        // PUT api/ControllerFornecedor/5
        [HttpPut]
        public async void Put([FromBody] ModelFornecedor value)
        {
            await _repository.Update(value);
        }

        // DELETE api/ControllerFornecedor/5
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            var response = await _repository.DeleteById(id);
            if (response == null) { return "Registro exluído com sucesso!"; }
            return response;

     
        }
    }
}
