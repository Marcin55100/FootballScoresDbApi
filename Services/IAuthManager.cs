using FootballScoresDbApi.Models.DTOs;

namespace FootballScoresDbApi.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDTO loginUserDTO);
        Task<string> CreateToken();
    }
}
