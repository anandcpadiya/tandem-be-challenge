using AutoMapper;
using TandemBEProject.DAL;
using TandemBEProject.DTOs;
using TandemBEProject.Models;

namespace TandemBEProject.Services
{
    public class UsersService
    {
        private readonly IMapper _mapper;
        private readonly IDbService _dbService;

        public UsersService(IMapper mapper, IDbService dbService)
        {
            _mapper = mapper;
            _dbService = dbService;
        }

        internal async Task<UserResponseDto> CreateUser(CreateUserRequestDto createUserRequest)
        {
            UserModel model = _mapper.Map<UserModel>(createUserRequest);
            model.UserId = Guid.NewGuid();
            model.Name = $"{createUserRequest.FirstName} {createUserRequest.MiddleName} {createUserRequest.LastName}";

            await _dbService.AddUser(model);

            return _mapper.Map<UserResponseDto>(model);
        }

        internal async Task<UserResponseDto?> GetUserByEmail(string email)
        {
            UserModel? userModel = await _dbService.GetUserByEmail(email);

            return _mapper.Map<UserResponseDto>(userModel);
        }

        internal async Task<UserResponseDto> UpdateUserByEmail(CreateUserRequestDto updatedUser)
        {
            UserModel model = _mapper.Map<UserModel>(updatedUser);
            model.Name = $"{updatedUser.FirstName} {updatedUser.MiddleName} {updatedUser.LastName}";

            UserModel updatedModel = await _dbService.UpdateUserByEmail(model);

            return _mapper.Map<UserResponseDto>(updatedModel);
        }
    }
}
