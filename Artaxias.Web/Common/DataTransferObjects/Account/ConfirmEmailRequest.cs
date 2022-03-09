
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Account
{
    public class ConfirmEmailRequest
    {
        [Required]
        [StringLength(maximumLength: 1000, MinimumLength = 2, ErrorMessage = "Username cannot be empty.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(maximumLength: 1000, MinimumLength = 2, ErrorMessage = "Token cannot be empty.")]
        public string Token { get; set; }
    }
}