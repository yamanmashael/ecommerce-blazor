namespace Blazor.Data
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public DateTime CreatedAt { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; }
    }

    public class CreateUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
      
    }

    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

      
        public bool EmailConfirmed { get; set; }
      
    }


    public class RequestUserDto
    {
        public string? SearchTerm { get; set; }
        public int? RoleId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }



    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }





    public class UserRoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
    }

    public class CreateUserRoleDto
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }

    }



}
