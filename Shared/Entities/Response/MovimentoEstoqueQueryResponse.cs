namespace Shared.Entities.Response {
    public class MovimentoEstoqueQueryResponse {

        public int codProduto { get; set; }
        public string? descricao { get; set; }
        public decimal saldoEstoque { get; set; }

        public MovimentoEstoqueQueryResponse() { }


    }
}
