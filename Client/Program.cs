using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace gRpc01.Client
{
  class Program
  {
    static async Task Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      var channel = GrpcChannel.ForAddress("https://localhost:5001");
      var client = new Characters.CharactersClient(channel);
      if (1 == 2)
      {
        var response = await client.GetCharacterAsync(new CharacterRequest { Id = 3 });
        Console.WriteLine($"character is: {response.Character.FirstName} {response.Character.LastName}");
      }

      if (1 == 2)
      {
        var request = client.SearchCharacters(new SearchRequest { Query = "s" });
        await foreach (var response in request.ResponseStream.ReadAllAsync())
        {
          Console.WriteLine($"charater is: {response.Character.FirstName} {response.Character.LastName}");
        }
      }

      var key = Console.ReadKey();
      using (var call = client.DoSum())
      {
        while (char.IsDigit(key.KeyChar))
        {
          var number = int.Parse(key.KeyChar.ToString());
          var request = new SumRequest { Value = number };

          await call.RequestStream.WriteAsync(request);
          key = Console.ReadKey();
				}
        await call.RequestStream.CompleteAsync();

        var response = await call.ResponseAsync;
        Console.WriteLine();
        Console.WriteLine($"Total is {response.Total}");
			}

      Console.ReadKey();
    }
  }
}
