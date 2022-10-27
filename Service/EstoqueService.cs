using Repository;
using Shared.Entities.Request;
using Shared.Entities.Response;
using System.Net.Http.Headers;
using System.Text;

namespace Service {
    public class EstoqueService {

        private readonly MyContext myContext;

        public EstoqueService(MyContext myContext) {
            this.myContext = myContext;
        }

        public List<MovimentoEstoqueQueryResponse> GetMovimentosEstoques() {

            var sql = new StringBuilder();

            sql.Append("SELECT");
            sql.Append("\n      est.codproduto 'codProduto',");
            sql.Append("\n      est.descricao 'descricao',");
            sql.Append("\n      ROUND(SUM(est.quantidade) * -1) 'saldoEstoque'");
            sql.Append("\n  FROM mov_estoque est");
            sql.Append("\n  GROUP BY est.codproduto");
            sql.Append("\n  ORDER BY est.codproduto ASC");

            var command = myContext.BuildMyCommand(sql.ToString());

            var result = command.ReadAll<MovimentoEstoqueQueryResponse>();

            return result;

        }

        public List<VendaPorProdutoQueryResponse> GetVendaPorProduto(string codProduto) {

            var sql = new StringBuilder();

            sql.Append(" SELECT");
            sql.Append("    ven.numerovenda 'numeroVenda',");
            sql.Append("    ven.data 'data',");
            sql.Append("    est.codproduto 'codProduto',");
            sql.Append("    est.descricao 'descricao',");
            sql.Append("    est.quantidade 'quantidade',");
            sql.Append("    est.valorunitario 'valorUnitario',");
            sql.Append("    (est.quantidade * est.valorunitario) 'valorTotal'");
            sql.Append(" FROM mov_estoque est");
            sql.Append(" INNER JOIN venda ven ON ven.numerovenda = est.codvenda");
            sql.Append(" WHERE");
            sql.Append("    est.codproduto = @Produto");

            var command = myContext.BuildMyCommand(sql.ToString());
            command.AddParameter("@Produto", codProduto);

            var result = command.ReadAll<VendaPorProdutoQueryResponse>();

            return result;

        }

        public EstoqueQueryResponse GetVenda(string numeroVenda) {

            var sql = new StringBuilder();

            sql.Append("SELECT * FROM venda WHERE numerovenda = @numeroVenda");

            var command = myContext.BuildMyCommand(sql.ToString());
            command.AddParameter("@numeroVenda", numeroVenda);

            var result = command.Read<EstoqueQueryResponse>();

            if (result == null)
                throw new Exception($"Venda não localizada ({numeroVenda}) !");

            sql.Clear();
            sql.Append("SELECT * FROM mov_estoque WHERE codvenda = @numeroVenda");

            command = myContext.BuildMyCommand(sql.ToString());
            command.AddParameter("@numeroVenda", numeroVenda);

            result.itens = command.ReadAll<ItemQueryResponse>().ToArray();

            return result;

        }

        public void Add(EstoqueInsertRequest estoque) {

            if (estoque.cpf_cnpj.Equals(""))
                throw new Exception("CPF/CNPJ não informado.");

            if (estoque.cpf_vendedor.Equals(""))
                throw new Exception("CPF do vendedor não informado.");

            if (estoque.quantidadeparcelas < 0)
                throw new Exception("Quantidade de parcela deve igual ou superior a 1");

            if (estoque.itens == null)
                throw new Exception("Não há produtos para a venda informada.");

            var sql = new StringBuilder();

            sql.Append("INSERT INTO venda SET");
            sql.Append("\n numerovenda = @numerovenda");
            sql.Append("\n ,data = @data");
            sql.Append("\n ,cpf_cnpj = @cpf_cnpj");
            sql.Append("\n ,nome = @nome");
            sql.Append("\n ,cpf_vendedor = @cpf_vendedor");
            sql.Append("\n ,quantidadeparcelas = @quantidadeparcelas");
            sql.Append("\n ,valordeentrada = @valordeentrada");

            var command = myContext.BuildMyCommand(sql.ToString());

            var parameters = new {
                numerovenda = estoque.numerovenda,
                data = estoque.GetData(),
                cpf_cnpj = estoque.cpf_cnpj,
                nome = estoque.nome,
                cpf_vendedor = estoque.cpf_vendedor,
                quantidadeparcelas = estoque.quantidadeparcelas,
                valordeentrada = estoque.valordeentrada
            };

            command.AddParameters(parameters);
            command.Execute();

            sql.Clear();

            sql.Append("INSERT INTO mov_estoque SET");
            sql.Append("\n codvenda = @codvenda");
            sql.Append("\n ,codproduto = @codproduto");
            sql.Append("\n ,descricao = @descricao");
            sql.Append("\n ,quantidade = @quantidade");
            sql.Append("\n ,valorunitario = @valorunitario");

            estoque.itens.ToList().ForEach((prod) => {

                if (prod.quantidade < 1)
                    throw new Exception($"Quantidade produto ({prod.codproduto}) não pode ser inferior a 1.");

                if (prod.valorunitario < 0.01)
                    throw new Exception($"Valor unitário produto ({prod.codproduto}) não pode ser inferior a 0.01");

                var item_parameter = new {
                    codvenda = estoque.numerovenda,
                    codproduto = prod.codproduto,
                    descricao = prod.descricao,
                    quantidade = prod.quantidade,
                    valorunitario = prod.valorunitario
                };

                command = myContext.BuildMyCommand(sql.ToString());
                command.AddParameters(item_parameter);
                command.Execute();

            });

        }

    }
}
