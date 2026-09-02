namespace SistemaPagamentos;

public class PagamentoCartao : FormaPagamento
{
    public override string Nome => "Cartão de crédito";

    public override decimal CalcularValorFinal(decimal valor)
    {
        // Regra do cartão: 3% de taxa (acréscimo)
        return valor + (valor * 0.03m);
    }
}
