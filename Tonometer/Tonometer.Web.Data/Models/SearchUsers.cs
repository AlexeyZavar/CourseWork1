#region

using Tonometer.Web.Data.Models.Dtos;

#endregion

namespace Tonometer.Web.Data.Models;

public sealed class SearchUsersResponse
{
    public ICollection<UserDto> Data { get; set; } = null!;
}
