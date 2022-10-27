namespace Shared.Entities.Response {
    public class VendaPorProdutoQueryResponse {

        public string? numeroVenda { get; set; }
        public DateTime? data { get; set; }
        public int codProduto { get; set; }
        public string? descricao { get; set; }
        public int quantidade { get; set; }
        public double valorUnitario { get; set; }
        public double valorTotal { get; set; }

        public VendaPorProdutoQueryResponse() { }

    }
}
