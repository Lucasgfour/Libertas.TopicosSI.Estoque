namespace Shared.Entities.Request {
    public class EstoqueInsertRequest {

        public string numerovenda { get; set; } = "";
        public String data { get; set; } = "";
        public string cpf_cnpj { get; set; } = "";
        public string nome { get; set; } = "";
        public string cpf_vendedor { get; set; } = "";
        public ItemInsertRequest[]? itens { get; set; }
        public double valordeentrada { get; set; }
        public int quantidadeparcelas { get; set; }

        public EstoqueInsertRequest() { }

        public DateTime GetData() => DateTime.Parse(data);

    }
}
