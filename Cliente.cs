namespace SistemaPagamentos;

// Representa um cliente da loja.
// Nome e CPF só podem ser definidos na criação (construtor).
// Depois de criado, ninguém de fora consegue alterar esses valores.
public class Cliente
{
    public string Nome { get; }
    public string Cpf { get; }

    public Cliente(string nome, string cpf)
    {
        Nome = nome;
        Cpf = cpf;
    }
}
