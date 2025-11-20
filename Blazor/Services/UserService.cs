using Blazor.Data;
using Blazored.Toast.Services;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly IToastService _toastService;
        private readonly Authentication _authentication;

        public UserService(HttpClient httpClient, IToastService toastService, Authentication authentication)
        {
            _httpClient = httpClient;
            _toastService = toastService;
            _authentication = authentication;
        }

        public async Task<ResponseModel<IEnumerable<UserDto>>> GetUsersAsync(RequestUserDto request)
        {
            try
            {
                var query = $"api/Users/GetUsers?searchTerm={request.SearchTerm}&roleId={request.RoleId}" +
                            $"&pageNumber={request.PageNumber}&pageSize={request.PageSize}";

                var response = await _httpClient.GetAsync(query);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<IEnumerable<UserDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                _toastService.ShowError(error?.ErrorMassage ?? "Failed to fetch users.");
                return new ResponseModel<IEnumerable<UserDto>> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Unexpected error occurred: {ex.Message}");
                return new ResponseModel<IEnumerable<UserDto>> { Success = false, ErrorMassage = "Unexpected error occurred." };
            }
        }

        public async Task<ResponseModel<UserDto>> GetUserByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Users/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<UserDto>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<UserDto>>();
                _toastService.ShowError(error?.ErrorMassage ?? "User not found or error occurred.");
                return new ResponseModel<UserDto> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Unexpected error occurred: {ex.Message}");
                return new ResponseModel<UserDto> { Success = false, ErrorMassage = "Unexpected error occurred." };
            }
        }

        public async Task<ResponseModel<object>> CreateUserAsync(CreateUserDto user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Users/Create", user);

                if (response.IsSuccessStatusCode)
                {
                    _toastService.ShowSuccess("User created successfully!");
                    return new ResponseModel<object> { Success = true };
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                _toastService.ShowError(error?.ErrorMassage ?? "Failed to create user.");
                return new ResponseModel<object> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Unexpected error occurred: {ex.Message}");
                return new ResponseModel<object> { Success = false, ErrorMassage = "Unexpected error occurred." };
            }
        }

        public async Task<ResponseModel<object>> UpdateUserAsync(UpdateUserDto user)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/Users/Update", user);

                if (response.IsSuccessStatusCode)
                {
                    _toastService.ShowSuccess("User updated successfully!");
                    return new ResponseModel<object> { Success = true };
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                _toastService.ShowError(error?.ErrorMassage ?? "Failed to update user.");
                return new ResponseModel<object> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Unexpected error occurred: {ex.Message}");
                return new ResponseModel<object> { Success = false, ErrorMassage = "Unexpected error occurred." };
            }
        }

        public async Task<ResponseModel<object>> DeleteUserAsync(int userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Users/Delete/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    _toastService.ShowSuccess("User deleted successfully!");
                    return new ResponseModel<object> { Success = true };
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                _toastService.ShowError(error?.ErrorMassage ?? "Failed to delete user.");
                return new ResponseModel<object> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Unexpected error occurred: {ex.Message}");
                return new ResponseModel<object> { Success = false, ErrorMassage = "Unexpected error occurred." };
            }
        }

        public async Task<ResponseModel<IEnumerable<RoleDto>>> GetRolesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Users/Roles");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<IEnumerable<RoleDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                _toastService.ShowError(error?.ErrorMassage ?? "Failed to fetch roles.");
                return new ResponseModel<IEnumerable<RoleDto>> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Unexpected error occurred: {ex.Message}");
                return new ResponseModel<IEnumerable<RoleDto>> { Success = false, ErrorMassage = "Unexpected error occurred." };
            }
        }

        public async Task<ResponseModel<List<UserRoleDto>>> GetAllUserRoleAsync(int userId)
        {
            try
            {
                await _authentication.SetAuthorizeHeader();
                var response = await _httpClient.GetAsync($"api/Users/user_Role/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ResponseModel<List<UserRoleDto>>>();
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                return new ResponseModel<List<UserRoleDto>> { Success = false, ErrorMassage = error.ErrorMassage };
            }
            catch (Exception ex)
            {
                return new ResponseModel<List<UserRoleDto>> { Success = false, ErrorMassage = $"Connection error: {ex.Message}" };
            }
        }

        public async Task<ResponseModel<object>> CreateUserRoleAsync(CreateUserRoleDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Users/User_Role", dto);

                if (response.IsSuccessStatusCode)
                {
                    _toastService.ShowSuccess("User role created successfully!");
                    return new ResponseModel<object> { Success = true };
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                _toastService.ShowError(error?.ErrorMassage ?? "Failed to create user role.");
                return new ResponseModel<object> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Unexpected error occurred: {ex.Message}");
                return new ResponseModel<object> { Success = false, ErrorMassage = "Unexpected error occurred." };
            }
        }

        public async Task<ResponseModel<object>> DeleteUserRoleAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Users/User_Role/{id}");

                if (response.IsSuccessStatusCode)
                {
                    _toastService.ShowSuccess("User role deleted successfully!");
                    return new ResponseModel<object> { Success = true };
                }

                var error = await response.Content.ReadFromJsonAsync<ResponseModel<object>>();
                _toastService.ShowError(error?.ErrorMassage ?? "Failed to delete user role.");
                return new ResponseModel<object> { Success = false, ErrorMassage = error?.ErrorMassage };
            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Unexpected error occurred: {ex.Message}");
                return new ResponseModel<object> { Success = false, ErrorMassage = "Unexpected error occurred." };
            }
        }
    }
}
