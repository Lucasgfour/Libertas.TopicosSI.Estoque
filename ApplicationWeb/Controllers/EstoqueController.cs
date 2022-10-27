using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Shared.Entities.Request;
using Shared.Entities.Response;

namespace ApplicationWeb.Controllers {
    [Route("estoque")]
    [ApiController]
    public class EstoqueController : ControllerBase {

        private readonly EstoqueService estoqueService;

        public EstoqueController(EstoqueService estoqueService) {
            this.estoqueService = estoqueService;
        }

        [HttpPost]
        public void Add(EstoqueInsertRequest estoque) => estoqueService.Add(estoque);

        [HttpGet]
        public IEnumerable<MovimentoEstoqueQueryResponse> GetMovimentosEstoques() {
            return estoqueService.GetMovimentosEstoques();
        }

        [HttpGet]
        [Route("{codProduto}")]
        public IEnumerable<VendaPorProdutoQueryResponse> GetVendaPorProduto(string codProduto) {
            return estoqueService.GetVendaPorProduto(codProduto);
        }

        [HttpGet]
        [Route("consulta/{numeroVenda}")]
        public EstoqueQueryResponse GetVenda(string numeroVenda) => estoqueService.GetVenda(numeroVenda);

    }
}
