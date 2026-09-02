using System.Globalization;

namespace SistemaPagamentos;

public class Program
{
    // Lista que guarda todas as vendas cadastradas, enquanto o programa roda.
    private static List<Venda> vendas = new List<Venda>();

    public static void Main()
    {
        bool continuar = true;

        while (continuar)
        {
            MostrarMenu();
            string? opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    CadastrarVenda();
                    break;
                case "2":
                    ListarVendas();
                    break;
                case "3":
                    RealizarPagamento();
                    break;
                case "0":
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Ops, opção inválida. Tente novamente.");
                    break;
            }

            Console.WriteLine();
        }
    }

    private static void MostrarMenu()
    {
        Console.WriteLine(".-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
        Console.WriteLine("SISTEMA DE VENDAS");
        Console.WriteLine(".-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
        Console.WriteLine("1 - Cadastrar venda");
        Console.WriteLine("2 - Listar vendas");
        Console.WriteLine("3 - Realizar pagamento");
        Console.WriteLine("0 - Sair");
        Console.WriteLine(".-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
        Console.Write("Escolha uma opção: ");
    }

    private static void CadastrarVenda()
    {
        Console.Write("Número: ");
        int numero = int.Parse(Console.ReadLine()!);

        Console.Write("Cliente: ");
        string nome = Console.ReadLine()!;

        Console.Write("CPF: ");
        string cpf = Console.ReadLine()!;

        Console.Write("Valor: ");
        decimal valor = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

        try
        {
            Cliente cliente = new Cliente(nome, cpf);
            Venda venda = new Venda(numero, cliente, valor);
            vendas.Add(venda);

            Console.WriteLine();
            Console.WriteLine("Venda cadastrada com sucesso!");
            Console.WriteLine($"Situação: {venda.Situacao}");
        }
        catch (ArgumentException erro)
        {
            Console.WriteLine($"Erro ao cadastrar venda: {erro.Message}");
        }
    }

    private static void ListarVendas()
    {
        if (vendas.Count == 0)
        {
            Console.WriteLine("Nenhuma venda cadastrada.");
            return;
        }

        foreach (Venda venda in vendas)
        {
            Console.WriteLine($"Venda: {venda.Numero}");
            Console.WriteLine($"Cliente: {venda.Cliente.Nome}");
            Console.WriteLine($"Valor original: {FormatarMoeda(venda.ValorCompra)}");
            Console.WriteLine($"Situação: {venda.Situacao}");

            if (venda.Situacao == "Pago" && venda.FormaPagamentoUtilizada != null)
            {
                Console.WriteLine($"Forma de pagamento: {venda.FormaPagamentoUtilizada.Nome}");
                Console.WriteLine($"Valor final: {FormatarMoeda(venda.ValorFinal)}");
            }

            Console.WriteLine();
        }
    }

    private static void RealizarPagamento()
    {
        Console.Write("Número da venda: ");
        int numero = int.Parse(Console.ReadLine()!);

        Venda? venda = vendas.FirstOrDefault(v => v.Numero == numero);

        if (venda == null)
        {
            Console.WriteLine("Venda não encontrada.");
            return;
        }
        Console.WriteLine(".-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
        Console.WriteLine("Digite a opção para a forma de pagamento:");
        Console.WriteLine(".-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
        Console.WriteLine("1 - PIX");
        Console.WriteLine("2 - Cartão de crédito");
        Console.WriteLine("3 - Dinheiro");
        Console.Write("Opção: ");
        string? opcaoPagamento = Console.ReadLine();

        FormaPagamento? formaPagamento = opcaoPagamento switch
        {
            "1" => new PagamentoPix(),
            "2" => new PagamentoCartao(),
            "3" => new PagamentoDinheiro(),
            _ => null
        };

        if (formaPagamento == null)
        {
            Console.WriteLine("Forma de pagamento inválida.");
            return;
        }

        try
        {
            decimal valorOriginal = venda.ValorCompra;

            venda.Pagar(formaPagamento);

            Console.WriteLine();
            Console.WriteLine($"Valor original: {FormatarMoeda(valorOriginal)}");
            Console.WriteLine($"Forma de pagamento: {formaPagamento.Nome}");
            Console.WriteLine($"Valor final: {FormatarMoeda(venda.ValorFinal)}");
            Console.WriteLine(".-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-.-");
            Console.WriteLine("Pagamento realizado com sucesso.");
        }
        catch (InvalidOperationException erro)
        {
            Console.WriteLine($"Erro: {erro.Message}");
        }
    }

    private static string FormatarMoeda(decimal valor)
    {
        return "R$ " + valor.ToString("F2", CultureInfo.InvariantCulture).Replace(".", ",");
    }
}
