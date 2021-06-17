using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_StoredProcs.Models
{
    public class ModelFornecedor
    {  
        public int idFornecedor { get; set; }
        public string nome { get; set; }
        [ForeignKey("ProdutoFornecedor")]
        public List<ModelProduto>produtos { get; set; }
    }

}
