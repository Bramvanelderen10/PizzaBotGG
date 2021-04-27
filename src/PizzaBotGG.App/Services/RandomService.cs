using System;

namespace PizzaBotGG.App.Services
{
	public class RandomService : IRandomService
	{
		private readonly Random _random;

		public RandomService()
		{
			_random = new Random();
		}
		public int Random(int min, int max)
			=> _random.Next(min, max);
	}
}
