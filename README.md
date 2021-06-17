Teste de Admissão
O Supermercado Zézinho precisa de um novo sistema para gerir o seu estoque de produtos. Seu papel nesse projeto é desenvolver uma API RESTful que forneça endpoints para gerenciar o cadastro dos produtos e fornecedores.

<h2>Tipo do Projeto: ASP .NET CORE Web API </h2>

<h2>Ferramentas utilizadas:</h2>

1 - Visual Studio 2019;<br>
2 - Packages Manager NuGet;<br>
2.1 - Newtonsoft.Json v13.0.1;<br>
2.2 - Swashbuckle.AspNetCore v5.6.3;<br>
2.3 - System.Data.SqlClient v4.8.2;<br>
3 - Target Framework: .NET 5.0;<br>
4 - Microsoft SQL Server Management Studio 2016	v13.0.16100.1 (Usuário local);<br>
4.1 - Server type: Database Engine;<br>
4.1 - Usuário: (LocalDB)\MSSQLLocalDB;<br>
4.2 - Authentication: Windows Authentication;<br>
4.3 - Database: System Database/master <br>
4.4 - "connectionStrings": {"defaultConnection": "Data Source=(LocalDB)\\MSSQLLocalDB; Initial Catalog=master;"} <br>




# Tabelas

<h5> [dbo].[produto] </h5>

```Sql
USE [master]
GO

CREATE TABLE [dbo].[produto](
	[idProduto] [int] IDENTITY(1,1) NOT NULL,
	[descricao] [varchar](300) NOT NULL,
	[preco] [decimal](10, 2) NOT NULL,
	[quantidadeEstoque] [int] NOT NULL,
	[idFornecedor] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idProduto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[produto] ADD  DEFAULT ((1)) FOR [quantidadeEstoque]
GO

ALTER TABLE [dbo].[produto]  WITH CHECK ADD FOREIGN KEY([idFornecedor])
REFERENCES [dbo].[fornecedor] ([idFornecedor])
GO

```

<h5> [dbo].[fornecedor] </h5>

```Sql
USE [master]
GO

CREATE TABLE [dbo].[fornecedor](
	[idFornecedor] [int] IDENTITY(1,1) NOT NULL,
	[nome] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idFornecedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
```
# Procedures

 [dbo].[usp_Produto] 

```Sql

USE [master]
GO

alter procedure [dbo].[usp_Produto] 
@opcao int = null,
@idFornecedor int = null,
@idProduto int = null,
@nomeFornecedor varchar(50) = null,
@descricaoProduto varchar(300) = null,
@precoMinimo as decimal(10,2) = null,
@precoMaximo as decimal(10,2) = null,
@preco as decimal(10,2) = null,
@qtdeProduto int = null

as
begin

if @opcao = 1 
begin																																											 
		select *, 'Ok' as result from produto p 																																 
		left  join fornecedor f on f.idFornecedor = p.idFornecedor																												 
		where (@idFornecedor		is null or (@idFornecedor is not null and f.idFornecedor = @idFornecedor))									and								 
			  (@descricaoProduto	is null or (@descricaoProduto is not null and upper(p.descricao) like '%' + upper( replace(@descricaoProduto,' ','%')) +'%' ))		and		 
			  (@nomeFornecedor	is null or (@nomeFornecedor is not null and upper(f.nome) like '%' + upper( replace(@nomeFornecedor,' ','%')) +'%' ))		and					 
			  (@precoMinimo			is null or	(@precoMinimo is not null and @precoMinimo <= p.preco))								and											 
			  (@precoMaximo			is null	or (@precoMaximo is not null and @precoMaximo >= p.preco))	and																		 
			  (@idProduto			is null or (@idProduto is not null and p.idProduto = @idProduto))		
return																																											 
end																																												 

if (@opcao = 3)
begin
		insert into produto values (@descricaoProduto,@preco,@qtdeProduto,@idFornecedor)
		select *, 'Ok' as result from produto p 																																 
		left join fornecedor f on f.idFornecedor = p.idFornecedor
		where p.descricao = @descricaoProduto and p.preco =  @preco and p.idFornecedor = @idFornecedor
return
end

if @opcao = 4 
begin
		update produto set preco = @preco, descricao = @descricaoProduto, 
		quantidadeEstoque = @qtdeProduto, idFornecedor = @idFornecedor where idProduto = @idProduto
		
		select *,'Produto alterado com sucesso' as Result 
		from produto p 
		inner join fornecedor f on f.idFornecedor = p.idFornecedor
		where idProduto = @idProduto

return
end

if @opcao = 5
begin
	delete produto where idProduto = @idProduto	
	return
end

end
GO
```

[dbo].[usp_Fornecedor]

```SQl

USE [master]
GO
ALTER procedure [dbo].[usp_Fornecedor]
@opcao int = null,
@idFornecedor int = null,
@nomeFornecedor as varchar(50) = null
as
begin

if @opcao = 1
begin
	select * from fornecedor f where ((@idFornecedor is null) or (@idFornecedor is not null and f.idFornecedor = @idFornecedor))  
end

if @opcao = 3
begin
	insert into fornecedor values (@nomeFornecedor)
end

if @opcao = 4
begin
	update fornecedor set nome = @nomeFornecedor where idFornecedor = @idFornecedor
end

if @opcao = 5
begin

	if @idFornecedor is null
	begin
	select 'Obrigatório informar identificador do Fornecedor.' as retorno
	return
	end

	if not exists(select f.idFornecedor from fornecedor f where f.idFornecedor = @idFornecedor)
	begin
	select 'Fornecedor não existente.' as retorno
	return
	end



	declare @qtdeproduto int = (select count(p.idProduto) from produto p where p.idFornecedor = @idFornecedor)

	if(@qtdeproduto > 0)
	begin
			select 'Fornecedor não pode ser excluído. Possui '+ convert(varchar(max),@qtdeproduto) + ' em estoque.' 
			return
	end
	else
	begin
			delete fornecedor where idFornecedor = @idFornecedor
	end

end

end 



```

# Endpoint

![Screenshot_60](https://user-images.githubusercontent.com/52809418/122434776-c5b09100-cf6d-11eb-85a9-f1eb870fa1f8.png)





GET
/api/ControllerFornecedor

REQUEST
```Json
[
  {
    "idFornecedor": 0,
    "nome": "string",
    "produtos": [
      {
        "idProduto": 0,
        "descricao": "string",
        "preco": 0,
        "quantidadeEstoque": 0,
        "idFornecedor": 0,
        "nomeFornecedor": "string",
        "precoMinimo": 0,
        "precoMaximo": 0,
        "result": "string"
      }
    ]
  }
]
```

POST
/api/ControllerFornecedor

REQUEST

```Json
{
  "idFornecedor": 0,
  "nome": "string"
}

```

PUT
/api/ControllerProduto

```Json
{
  "idProduto": 0,
  "descricao": "string",
  "preco": 0,
  "quantidadeEstoque": 0,
  "idFornecedor": 0,
  "nomeFornecedor": "string"
}
```

GET
/api/ControllerProduto/{id}

RESPONSE

```Json
{
  "idProduto": 0,
  "descricao": "string",
  "preco": 0,
  "quantidadeEstoque": 0,
  "idFornecedor": 0,
  "nomeFornecedor": "string",
  "precoMinimo": 0,
  "precoMaximo": 0,
  "result": "string"
}
```

DELETE
/api/ControllerProduto/{id}

Response 

```Html

Code :200	
Description:Success
No links

```

<h2>ControllerProduto</h2>

GET
/api/ControllerProduto


REQUEST

```Json
{
  "idProduto": 0,
  "descricao": "string",
  "preco": 0,
  "quantidadeEstoque": 0,
  "idFornecedor": 0,
  "nomeFornecedor": "string",
  "precoMinimo": 0,
  "precoMaximo": 0,
}

```
RESPONSE

```jSON
[
  {
    "idProduto": 0,
    "descricao": "string",
    "preco": 0,
    "quantidadeEstoque": 0,
    "idFornecedor": 0,
    "nomeFornecedor": "string",
    "precoMinimo": 0,
    "precoMaximo": 0,
    "result": "string"
  }
]

```

POST
/api/ControllerProduto

REQUEST

```jSON

{
  "idProduto": 0,
  "descricao": "string",
  "preco": 0,
  "quantidadeEstoque": 0,
  "idFornecedor": 0,
  "nomeFornecedor": "string",
  "precoMinimo": 0,
  "precoMaximo": 0,
  "result": "string"
}
```

RESPONSE

```JSON
{
  "idProduto": 0,
  "descricao": "string",
  "preco": 0,
  "quantidadeEstoque": 0,
  "idFornecedor": 0,
  "nomeFornecedor": "string",
  "precoMinimo": 0,
  "precoMaximo": 0,
  "result": "string"
}
```

PUT
/api/ControllerProduto

REQUEST

```jSON
{
  "idProduto": 0,
  "descricao": "string",
  "preco": 0,
  "quantidadeEstoque": 0,
  "idFornecedor": 0,
  "nomeFornecedor": "string",
  "precoMinimo": 0,
  "precoMaximo": 0,
}
```
RESPONSE

```jSON
{
  "idProduto": 0,
  "descricao": "string",
  "preco": 0,
  "quantidadeEstoque": 0,
  "idFornecedor": 0,
  "nomeFornecedor": "string",
  "precoMinimo": 0,
  "precoMaximo": 0,
  "result": "string"
}
```

GET
/api/ControllerProduto/{id}

RESPONSE

```JSON
{
  "idProduto": 0,
  "descricao": "string",
  "preco": 0,
  "quantidadeEstoque": 0,
  "idFornecedor": 0,
  "nomeFornecedor": "string",
  "precoMinimo": 0,
  "precoMaximo": 0,
  "result": "string"
}
```


DELETE
/api/ControllerProduto/{id}

RESPONSE

```Html

Code :200	
Description:Success
No links

```

# Schemas

ModelProduto

![Screenshot_61](https://user-images.githubusercontent.com/52809418/122438465-1fff2100-cf71-11eb-83cd-37e066927f01.png)

ModelProduto

![Screenshot_62](https://user-images.githubusercontent.com/52809418/122438608-445afd80-cf71-11eb-826a-de713ef67723.png)















