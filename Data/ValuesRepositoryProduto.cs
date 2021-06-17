using ASPNETCore_StoredProcs.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_StoredProcs.Data
{
    public class ValuesRepositoryProduto
    {
        private readonly string _connectionString;

        public ValuesRepositoryProduto(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<List<ModelProduto>> GetAll(ModelProduto value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Produto", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcao", 1)); // 1: getAll - 2: GetById - 3: Insert - 4: Update - 5: DeleteById
                    cmd.Parameters.Add(new SqlParameter("@idFornecedor",        value.idFornecedor));
                    cmd.Parameters.Add(new SqlParameter("@idProduto",           value.idProduto));
                    cmd.Parameters.Add(new SqlParameter("@descricaoProduto",    value.descricao));
                    cmd.Parameters.Add(new SqlParameter("@nomeFornecedor",      value.nomeFornecedor));
                    cmd.Parameters.Add(new SqlParameter("@precoMinimo",         value.precoMinimo));
                    cmd.Parameters.Add(new SqlParameter("@precoMaximo",         value.precoMaximo));

                    var response = new List<ModelProduto>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }
                    return response;
                }
            }
        }

        private ModelProduto MapToValue(SqlDataReader reader)
        {
            return new ModelProduto()
            {
                idProduto           = (int)reader["idproduto"],
                descricao           = (string)reader["descricao"],
                preco               = (decimal)reader["preco"],
                quantidadeEstoque   = (int)reader["quantidadeEstoque"],
                idFornecedor        = (int)reader["idFornecedor"],
                nomeFornecedor      = (string)reader["nome"],
                result              = (string)reader["result"]
            };
        }

        public async Task<ModelProduto> GetById(int Id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Produto", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcao", 1)); // 1: getAll - 1: GetById - 3: Insert - 4: Update - 5: DeleteById
                    cmd.Parameters.Add(new SqlParameter("@idProduto", Id));

                    ModelProduto response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToValue(reader);
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<ModelProduto> Insert(ModelProduto value)
        {
            ModelProduto response = null;
            if (value.descricao.Count() < 10 || value.descricao.Count() > 300 || value.preco == null || (value.quantidadeEstoque < 1 && value.quantidadeEstoque == null) && value.idFornecedor == null)
            {
                return new ModelProduto()
                {
                    idProduto = null,
                    descricao = null,
                    preco = null,
                    quantidadeEstoque = null,
                    idFornecedor = null,
                    nomeFornecedor = null,
                    result = "Erro ao Cadastrar - Valores fora dos padrões. Id Não pode ser alterado. Descrição Obrigatório Mínimo 10 caracteres, Máximo 300 caracteres. Preço em R$ Obrigatório. Quantidade em estoque Obrigatório Mínimo 1 e Fornecedor Obrigatório"
                };
            }

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Produto", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcao", 3)); // 1: getAll - 2: GetById - 3: Insert - 4: Update - 5: DeleteById
                    cmd.Parameters.Add(new SqlParameter("@descricaoProduto", value.descricao));
                    cmd.Parameters.Add(new SqlParameter("@preco", value.preco));
                    cmd.Parameters.Add(new SqlParameter("@idFornecedor", value.idFornecedor));
                    cmd.Parameters.Add(new SqlParameter("@qtdeProduto", value.quantidadeEstoque));

                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToValue(reader);
                        }
                    }
                    return response;
                }
            }
        }

        public async Task<ModelProduto> Update(ModelProduto value)
        {
            ModelProduto response = null;
            if (value.descricao.Count() < 10 || value.descricao.Count() > 300 || value.preco == null || (value.quantidadeEstoque < 1 && value.quantidadeEstoque == null) && string.IsNullOrEmpty(value.nomeFornecedor))
            {
                return new ModelProduto()
                {
                    idProduto = null,
                    descricao = null,
                    preco = null,
                    quantidadeEstoque = null,
                    idFornecedor = null,
                    nomeFornecedor = null,
                    result = "Erro ao alterar - Valores fora dos padrões. Id Não pode ser alterado. Descrição Obrigatório Mínimo 10 caracteres, Máximo 300 caracteres. Preço em R$ Obrigatório. Quantidade em estoque Obrigatório Mínimo 1 e Fornecedor Obrigatório"
                };
            }

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Produto", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcao", 4)); // 1: getAll - 2: GetById - 3: Insert - 4: Update - 5: DeleteById
                    cmd.Parameters.Add(new SqlParameter("@idProduto", value.idProduto));
                    cmd.Parameters.Add(new SqlParameter("@descricaoProduto", value.descricao));
                    cmd.Parameters.Add(new SqlParameter("@preco", value.preco));
                    cmd.Parameters.Add(new SqlParameter("@qtdeProduto", value.quantidadeEstoque));
                    cmd.Parameters.Add(new SqlParameter("@idFornecedor", value.idFornecedor));

                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToValue(reader);
                        }
                    }
                    return response;
                }
            }
        }

        public async Task DeleteById(int Id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Produto", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcao", 5)); // 1: getAll - 2: GetById - 3: Insert - 4: Update - 5: DeleteById
                    cmd.Parameters.Add(new SqlParameter("@idProduto", Id));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
