

using System.ComponentModel.DataAnnotations;

namespace ASPNETCore_StoredProcs.Models
{
    public class ModelProduto
    {
        [Key]
        public int? idProduto { get; set; }
        public string descricao{ get; set; }
        public decimal? preco { get; set; }
        public int? quantidadeEstoque { get; set; }
        public int? idFornecedor { get; set; }
        public string nomeFornecedor { get; set; }
        public decimal? precoMinimo { get; set; }
        public decimal? precoMaximo { get; set; }
        public string result { get; set; }

    }


}
