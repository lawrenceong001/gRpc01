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
      var request = client.SearchCharacters(new SearchRequest { Query = "s" });
      await foreach(var response in request.ResponseStream.ReadAllAsync())
      {
        Console.WriteLine($"charater is: {response.Character.FirstName} {response.Character.LastName}");
			}
      Console.ReadKey();
    }
  }
}
