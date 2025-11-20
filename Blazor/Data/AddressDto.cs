using System.ComponentModel.DataAnnotations;

namespace Blazor.Data
{
    public class AddressDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string FullAddress { get; set; }

    }

    public class CreateAddressDto
    {

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Full Address is required.")]
        [StringLength(250, ErrorMessage = "Full Address cannot exceed 250 characters.")]
        public string FullAddress { get; set; }

        public bool IsDefault { get; set; }
    }


    public class UpdateAddressDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid Phone Number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Full Address is required.")]
        [StringLength(250, ErrorMessage = "Full Address cannot exceed 250 characters.")]
        public string FullAddress { get; set; }

        public bool IsDefault { get; set; }

    }
}
