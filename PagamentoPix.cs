namespace SistemaPagamentos;

// ": FormaPagamento" é a HERANÇA -> PagamentoPix "é um tipo de" FormaPagamento.
// Por isso ela é obrigada a implementar Nome e CalcularValorFinal.
public class PagamentoPix : FormaPagamento
{
    public override string Nome => "PIX";

    public override decimal CalcularValorFinal(decimal valor)
    {
        // Regra Pix: 5% de desconto
        return valor - (valor * 0.05m);
    }
}
