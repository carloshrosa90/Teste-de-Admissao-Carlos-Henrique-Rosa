using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASPNETCore_StoredProcs.Data;
using ASPNETCore_StoredProcs.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCore_StoredProcs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerProduto : ControllerBase
    {
        private readonly ValuesRepositoryProduto _repository;

        public ControllerProduto(ValuesRepositoryProduto repository)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET api/ControllerFornecedor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelProduto>>> Get([FromBody] ModelProduto value)
        {
            return await _repository.GetAll(value);
        }

        // GET api/ControllerFornecedor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelProduto>> Get(int id)
        {
            var response = await _repository.GetById(id);
            if (response == null) { return NotFound(); }
            return response;
        }

        // POST api/ControllerFornecedor
        [HttpPost]
        public async Task<ActionResult<ModelProduto>> Post([FromBody] ModelProduto value)
        {
           return await _repository.Insert(value);
        }

        // PUT api/ControllerFornecedor/5
        [HttpPut]
        public async Task<ActionResult<ModelProduto>> Put([FromBody] ModelProduto value)
        {
            var response = await _repository.Update(value);
            if (response == null || response.result != null)
            return NotFound(response);

            return response;
        }

        // DELETE api/ControllerFornecedor/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
           await _repository.DeleteById(id);
        }
    }
}
