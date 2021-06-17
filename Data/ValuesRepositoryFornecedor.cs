using ASPNETCore_StoredProcs.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCore_StoredProcs.Data
{
    public class ValuesRepositoryFornecedor
    {
        private readonly string _connectionString;
      
        public ValuesRepositoryFornecedor(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<List<ModelFornecedor>> GetAll()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Fornecedor", sql))
                {
                    cmd.Parameters.Add(new SqlParameter("@opcao", 1));
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<ModelFornecedor>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(await MapToValue(reader));
                        }
                    }
                    return response;
                }
            }
        }

        public async Task<ModelFornecedor> MapToValue(SqlDataReader reader)
        {
            List<ModelProduto> _modelProduto = new List<ModelProduto>();

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Produto", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcao", 1)); // 1: getAll - 1: GetById - 3: Insert - 4: Update - 5: DeleteById
                    cmd.Parameters.Add(new SqlParameter("@idFornecedor", (int)reader["idFornecedor"]));
                 
                    await sql.OpenAsync();

                    using (var reader1 = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader1.ReadAsync())
                        {
                            _modelProduto.Add(new ModelProduto
                            {
                                idProduto = (int)reader1["idProduto"],
                                descricao = (string)reader1["descricao"],
                                preco = (decimal)reader1["preco"],
                                quantidadeEstoque = (int)reader1["quantidadeEstoque"],
                                idFornecedor = (int)reader1["idFornecedor"],
                                nomeFornecedor = (string)reader1["nome"],
                                result = (string)reader1["result"]

                            });
                        }
                    }
                }
            }

            return new ModelFornecedor()
            {
                idFornecedor = (int)reader["idFornecedor"],
                nome = (string)reader["nome"],
                produtos = _modelProduto,
            };
        }

        public async Task<ModelFornecedor> GetById(int Id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Fornecedor", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcao", 1));
                    cmd.Parameters.Add(new SqlParameter("@idFornecedor", Id));

                    ModelFornecedor response = null;
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = await MapToValue(reader);
                        }
                    }
                    return response;
                }
            }
        }

        public async Task<ModelFornecedor> Insert(ModelFornecedor value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Fornecedor", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcao", 3));
                    cmd.Parameters.Add(new SqlParameter("@nomeFornecedor", value.nome));
                    ModelFornecedor response = null;
                    await sql.OpenAsync();
                   

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = await MapToValue(reader);
                        }
                    }
                    return response;
                }
            }
        }

        public async Task Update(ModelFornecedor value)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Fornecedor", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcao", 4));
                    cmd.Parameters.Add(new SqlParameter("@idFornecedor", value.idFornecedor));
                    cmd.Parameters.Add(new SqlParameter("@nomeFornecedor",         value.nome));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task<string> DeleteById(int Id)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Fornecedor", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcao", 5));
                    cmd.Parameters.Add(new SqlParameter("@idFornecedor", Id));
                    await sql.OpenAsync();
                    string? retorno = null;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            retorno = reader[0].ToString();
                        }
                    }

                    return retorno;
                }
            }
        }
    }
}
