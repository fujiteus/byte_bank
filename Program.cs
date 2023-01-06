using System.Reflection.Metadata.Ecma335;

namespace ByteBank
{

    public class Program
    {

        static void MostrarMenu()
        {
            Console.WriteLine("1 - Inserir novo usuário");
            Console.WriteLine("2 - Deletar um usuário");
            Console.WriteLine("3 - Listar todas as contas registradas");
            Console.WriteLine("4 - Detalhes de um usuário");
            Console.WriteLine("5 - Quantia armazenada no banco");
            Console.WriteLine("6 - Manipular a conta");
            Console.WriteLine("0 - Para sair do programa");
            Console.Write("Digite a opção desejada: ");
        }

        static void RegistrarNovoUsuario(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos)
        {
            Console.Write("Digite o cpf: ");
            cpfs.Add(Console.ReadLine());
            Console.Write("Digite o nome: ");
            titulares.Add(Console.ReadLine());
            Console.Write("Digite a senha: ");
            senhas.Add(Console.ReadLine());
            saldos.Add(0);
        }

        static void DeletarUsuario(List<string> cpfs, List<string> titulares, List<string> senhas, List<double> saldos)
        {
            Console.Write("Digite o cpf: ");
            string cpfParaDeletar = Console.ReadLine();
            int indexParaDeletar = cpfs.FindIndex(cpf => cpf == cpfParaDeletar);

            if (indexParaDeletar == -1)
            {
                Console.WriteLine("Não foi possível deletar esta Conta");
                Console.WriteLine("MOTIVO: Conta não encontrada.");
            }
            else
            {
                cpfs.Remove(cpfParaDeletar);
                titulares.RemoveAt(indexParaDeletar);
                senhas.RemoveAt(indexParaDeletar);
                saldos.RemoveAt(indexParaDeletar);

                Console.WriteLine("Conta deletada com sucesso");
            }
        }

        static void ListarTodasAsContas(List<string> cpfs, List<string> titulares, List<double> saldos)
        {
            for (int i = 0; i < cpfs.Count; i++)
            {
                ApresentaConta(i, cpfs, titulares, saldos);
            }
        }

        static void ApresentarUsuario(List<string> cpfs, List<string> titulares, List<double> saldos)
        {
            Console.Write("Digite o cpf: ");
            string cpfParaApresentar = Console.ReadLine();
            int indexParaApresentar = cpfs.FindIndex(cpf => cpf == cpfParaApresentar);

            if (indexParaApresentar == -1)
            {
                Console.WriteLine("Não foi possível apresentar esta Conta");
                Console.WriteLine("MOTIVO: Conta não encontrada.");
            }

            ApresentaConta(indexParaApresentar, cpfs, titulares, saldos);
        }

        static void ApresentarValorAcumulado(List<double> saldos)
        {
            Console.WriteLine($"Total acumulado no banco: {saldos.Sum()}");
        }

        static void ApresentaConta(int index, List<string> cpfs, List<string> titulares, List<double> saldos)
        {
            Console.WriteLine($"CPF = {cpfs[index]} | Titular = {titulares[index]} | Saldo = R${saldos[index]:F2}");
        }

        static bool Login(string cpfParaLogar, List<string> cpfs, List<string> senhas)
        {
            int indexParaLogar = cpfs.FindIndex(cpf => cpf == cpfParaLogar);
            if (indexParaLogar == -1)
            {
                Console.WriteLine("Usuário não encontrado");
                Console.WriteLine("Redirecionando para a tela inicial...");
                return false;
            }
            else 
            { 
                Console.Write("Senha: ");
                string senha = Console.ReadLine();
                if (senhas.ElementAt(indexParaLogar) == senha)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Senha incorreta");
                    Console.WriteLine("Redirecionando para a tela inicial...");
                    return false;
                }
            }
        }

        static void ManipularConta(string cpfDoUsuario, List<string> cpfs, List<double>saldos, List<string> senhas, List<string> titulares)
        {
            int indexDoUsuario = cpfs.FindIndex(cpf => cpf == cpfDoUsuario);
            string nomeDoUsuario = titulares.ElementAt(indexDoUsuario);
            Console.WriteLine($"Bem vindo(a) {nomeDoUsuario}! Por favor, selecione a operação desejada:");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("1 - Depósito");
            Console.WriteLine("2 - Saque");
            Console.WriteLine("3 - Transferência");
            Console.Write("Digite a opção desejada: ");
            int opcao = int.Parse(Console.ReadLine());
            Console.WriteLine("----------------------------------------------------");
            if (opcao == 1) 
            {
                string stringDoDeposito;
                double valorDoDeposito;
                do
                {
                    Console.Write("Digite o valor a ser depositado: ");
                    stringDoDeposito = Console.ReadLine();
                }
                while (!double.TryParse(stringDoDeposito, out valorDoDeposito) || valorDoDeposito < 0);
                double novoValor = saldos.ElementAt(indexDoUsuario) + valorDoDeposito;
                saldos.RemoveAt(indexDoUsuario);
                saldos.Insert(indexDoUsuario, novoValor);
                Console.WriteLine("Operação concluída, obrigado!");
            }
            else if (opcao == 2)
            {
                Console.WriteLine("Antes de prosseguir, digite sua senha novamente:");
                bool checagemSeguranca = Login(cpfDoUsuario, cpfs, senhas);
                if (checagemSeguranca)
                {
                        string stringDoSaque;
                        double valorDoSaque;
                    do
                    {
                        Console.Write("Digite o valor a ser levantado: ");
                        stringDoSaque = Console.ReadLine();
                    }
                    while (!double.TryParse(stringDoSaque, out valorDoSaque) || valorDoSaque < 0);
                    if (valorDoSaque > saldos.ElementAt(indexDoUsuario))
                    {
                        Console.WriteLine("O seu saldo não é suficiente para realizar esta operação");
                    }
                    else
                    {
                        Console.WriteLine($"Há solicitado o levantamento de R$ {valorDoSaque:F2}. Confirma operação?");
                        Console.WriteLine("1 - Sim");
                        Console.WriteLine("2 - Não");
                        Console.Write("Digite a opção desejada: ");
                        string opcaoDoSaque; 
                        do
                        {
                            opcaoDoSaque = Console.ReadLine();
                        }
                        while (opcaoDoSaque != "1" && opcaoDoSaque != "2");
                        if (opcaoDoSaque == "1") 
                        {
                            double novoValor = saldos.ElementAt(indexDoUsuario) - valorDoSaque;
                            saldos.RemoveAt(indexDoUsuario);
                            saldos.Insert(indexDoUsuario, novoValor);
                            Console.WriteLine("Operação concluída, obrigado!");
                        }
                        else
                        {
                            Console.WriteLine("Operação cancelada!");
                            Console.WriteLine("Redirecionando para a tela inicial...");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Senha incorreta, por favor repita a operação.");
                    Console.WriteLine("Redirecionando para a tela inicial...");
                }
            }
            else if (opcao == 3)
            {
                Console.WriteLine("Antes de prosseguir, digite sua senha novamente:");
                bool checagemSeguranca = Login(cpfDoUsuario, cpfs, senhas);
                if (checagemSeguranca)
                {
                    string stringDaTransferencia;
                    double valorDaTransferencia;
                    do
                    {
                        Console.Write("Digite o valor a ser transferido: ");
                        stringDaTransferencia = Console.ReadLine();
                    }
                    while (!double.TryParse(stringDaTransferencia, out valorDaTransferencia) || valorDaTransferencia < 0);
                    if (valorDaTransferencia > saldos.ElementAt(indexDoUsuario))
                    {
                        Console.WriteLine("O seu saldo não é suficiente para realizar esta operação");
                    }
                    else
                    {
                        Console.Write("Digite o cpf do destinatário: ");
                        string cpfDestinatario = Console.ReadLine();
                        int indexDoDestinatario = cpfs.FindIndex(cpf => cpf == cpfDestinatario);
                        if (indexDoDestinatario == -1)
                        {
                            Console.WriteLine("Destinatário não encontrado");
                            Console.WriteLine("Redirecionando para a tela inicial...");
                        }
                        else
                        {
                            Console.WriteLine($"Há solicitado a transferência de R$ {valorDaTransferencia:F2} para {titulares.ElementAt(indexDoDestinatario)}.");
                            Console.WriteLine("Confirma operação?");
                            Console.WriteLine("1 - Sim");
                            Console.WriteLine("2 - Não");
                            Console.Write("Digite a opção desejada: ");
                            string opcaoDaTransf;
                            do
                            {
                                opcaoDaTransf = Console.ReadLine();
                            }
                            while (opcaoDaTransf != "1" && opcaoDaTransf != "2");
                            if (opcaoDaTransf == "1")
                            {
                                double novoValorUsuario = saldos.ElementAt(indexDoUsuario) - valorDaTransferencia;
                                double novoValorDestinatario = valorDaTransferencia + saldos.ElementAt(indexDoDestinatario);
                                saldos.RemoveAt(indexDoUsuario);
                                saldos.Insert(indexDoUsuario, novoValorUsuario);
                                saldos.RemoveAt(indexDoDestinatario);
                                saldos.Insert(indexDoDestinatario, novoValorDestinatario);
                                Console.WriteLine("Operação concluída, obrigado!");
                            }
                            else
                            {
                                Console.WriteLine("Operação cancelada!");
                                Console.WriteLine("Redirecionando para a tela inicial...");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Senha incorreta, por favor repita a operação.");
                    Console.WriteLine("Redirecionando para a tela inicial...");
                }
            }
        }

        public static void Main(string[] args)
        {

            Console.WriteLine("Antes de começar a usar, vamos configurar alguns valores: ");

            List<string> cpfs = new List<string>();
            List<string> titulares = new List<string>();
            List<string> senhas = new List<string>();
            List<double> saldos = new List<double>();

            int option;
            string stringOption;
            do
            {
                do
                {
                    MostrarMenu();
                    stringOption = Console.ReadLine();
                }
                while (!int.TryParse(stringOption, out option));
                Console.WriteLine("----------------------------------------------------");

                switch (option)
                {
                    case 0:
                        Console.WriteLine("Encerrando o programa, obrigado por utilizar nossos serviços...");
                        break;
                    case 1:
                        RegistrarNovoUsuario(cpfs, titulares, senhas, saldos);
                        break;
                    case 2:
                        DeletarUsuario(cpfs, titulares, senhas, saldos);
                        break;
                    case 3:
                        ListarTodasAsContas(cpfs, titulares, saldos);
                        break;
                    case 4:
                        ApresentarUsuario(cpfs, titulares, saldos);
                        break;
                    case 5:
                        ApresentarValorAcumulado(saldos);
                        break;
                    case 6:
                        Console.WriteLine("Para acessar essa informação, é preciso fazer login.");
                        Console.WriteLine("----------------------------------------------------");
                        Console.Write("CPF: ");
                        string cpfParaLogar = Console.ReadLine();
                        bool loginDoUsuario = Login(cpfParaLogar, cpfs, senhas);
                        if (loginDoUsuario)
                        {
                            ManipularConta(cpfParaLogar, cpfs, saldos, senhas, titulares);
                        }
                        break;
                    case 7:
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Opção inválida, tente novamente!");
                        break;
                }

                Console.WriteLine("----------------------------------------------------");

            } while (option != 0);
        }

    }

}