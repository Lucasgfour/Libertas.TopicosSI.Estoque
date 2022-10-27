using Shared.Entities.Request;

namespace Shared.Entities.Response {
    public class EstoqueQueryResponse {

        public string numerovenda { get; set; } = "";
        public DateTime data { get; set; }
        public string cpf_cnpj { get; set; } = "";
        public string nome { get; set; } = "";
        public string cpf_vendedor { get; set; } = "";
        public ItemQueryResponse[]? itens { get; set; }
        public double valordeentrada { get; set; }
        public int quantidadeparcelas { get; set; }

        public EstoqueQueryResponse() { }

    }
}
