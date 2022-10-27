namespace Shared.Entities.Request {
    public class ItemInsertRequest {

        public int codproduto { get; set; }
        public string descricao { get; set; } = "";
        public int quantidade { get; set; }
        public double valorunitario { get; set; }

    }
}
