﻿#region

using Bogus;
using Rusty.Template.Domain;
using Rusty.Template.Infrastructure.Database;

#endregion

namespace Rusty.Template.Tests.Integration;

public abstract class BaseTests : IClassFixture<WebApiFactory>
{
	private readonly AppDbContext _appDbContext;
	protected readonly NSwagClient Client;

	protected BaseTests(WebApiFactory apiFactory)
	{
		Client = new NSwagClient(apiFactory.CreateClient());
	}

	protected async Task InitUserDataAsync()
	{
		//Set the randomizer seed if you wish to generate repeatable data sets.
		Randomizer.Seed = new Random(8675309);
		var testData = new Faker<User>()
					   .RuleFor(o => o.Email, f => f.Person.Email)
					   .RuleFor(o => o.UserName, f => f.Person.UserName)
					   .RuleFor(o => o.Password, f => f.PickRandom<string>())
					   .Generate(500);
		await _appDbContext.Users.AddRangeAsync(testData);
	}
}