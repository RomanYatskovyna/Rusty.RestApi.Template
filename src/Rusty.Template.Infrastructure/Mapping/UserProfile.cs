#region

using Mapster;
using Rusty.Template.Contracts.Dtos.User;
using Rusty.Template.Domain;

#endregion

namespace Rusty.Template.Infrastructure.Mapping;

public sealed class UserProfile : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<User, UserDto>()
			.Map(dest => dest.FirstName, src => src.UserInfo == null ? string.Empty : src.UserInfo.FirstName)
			.Map(dest => dest.LastName, src => src.UserInfo == null ? string.Empty : src.UserInfo.LastName)
			// .TwoWays()
			;
	}
}