namespace Shared.Entities.Response {
    public class ItemQueryResponse {

        public string codvenda { get; set; } = "";
        public int codproduto { get; set; }
        public string descricao { get; set; } = "";
        public int quantidade { get; set; }
        public double valorunitario { get; set; }

        public ItemQueryResponse() { }

    }
}
