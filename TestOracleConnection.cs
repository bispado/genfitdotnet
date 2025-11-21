using System;
using Oracle.ManagedDataAccess.Client;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=Fiap#2025;";
        
        Console.WriteLine("Testando conexão com Oracle...");
        Console.WriteLine($"Connection String: Data Source=oracle.fiap.com.br:1521/ORCL;User Id=rm558515;Password=***");
        
        try
        {
            using (var connection = new OracleConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("✓ Conexão estabelecida com sucesso!");
                
                using (var command = new OracleCommand("SELECT 1 FROM DUAL", connection))
                {
                    var result = command.ExecuteScalar();
                    Console.WriteLine($"✓ Query de teste executada: {result}");
                }
                
                // Testar se as tabelas existem
                using (var command = new OracleCommand("SELECT COUNT(*) FROM USER_TABLES WHERE TABLE_NAME = 'USERS'", connection))
                {
                    var count = command.ExecuteScalar();
                    Console.WriteLine($"✓ Verificação de tabelas: {count} tabela(s) USER encontrada(s)");
                }
                
                Console.WriteLine("\nConexão com Oracle funcionando corretamente!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Erro ao conectar: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"  Detalhes: {ex.InnerException.Message}");
            }
        }
    }
}
