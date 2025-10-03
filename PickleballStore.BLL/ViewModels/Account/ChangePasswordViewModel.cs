using System.ComponentModel.DataAnnotations;

namespace PickleballStore.BLL.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        public required string CurrentPassword { get; set; } 

        [DataType(DataType.Password)]
        public required string NewPassword { get; set; } 

        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; } = string.Empty;
    }
}