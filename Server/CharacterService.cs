using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gRpc01.Server
{
	public class CharacterService : gRpc01.Characters.CharactersBase
	{

		public static IEnumerable<Character> Characters = new List<Character>
		{ 
			new Character {Id=1, FirstName="f1", LastName= "l1", Show="s1"},
			new Character {Id=2, FirstName="f2", LastName= "l2", Show="s2"},
			new Character {Id=3, FirstName="f3", LastName= "l3", Show="s3"},
			new Character {Id=4, FirstName="f4", LastName= "l4", Show="s4"},
			new Character {Id=5, FirstName="f5", LastName= "l5", Show="s5"}
		};

		public override Task<CharacterResponse> GetCharacter(CharacterRequest request, ServerCallContext context)
		{
			var character = Characters.FirstOrDefault(i => i.Id == request.Id);
			return Task.FromResult(new CharacterResponse { Character = character });
			// return base.GetCharacter(request, context);
		}

		public override async Task SearchCharacters(SearchRequest request, IServerStreamWriter<CharacterResponse> responseStream, ServerCallContext context)
		{
			var results = Characters.Where(c => c.Show.Contains(request.Query, StringComparison.CurrentCultureIgnoreCase));
			foreach(var result in results)
			{
				await responseStream.WriteAsync(new CharacterResponse { Character = result });
				await Task.Delay(1000);
			}
		}

		public override async Task<SumResponse> DoSum(IAsyncStreamReader<SumRequest> requestStream, ServerCallContext context)
		{
			int count = 0;
			await foreach(var request in requestStream.ReadAllAsync())
			{
				count += request.Value;
				Console.WriteLine($"received number {request.Value}. Total is {count}");
			}
			return new SumResponse { Total = count };
		}
	}
}
