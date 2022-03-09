using System;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class BonusRequest
    {
        public int Id { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Bonus Amount must be greater than 0.")]
        public double Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Comment should contain at least one word.")]
        public string Comment { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Receiver must be selected.")]
        public int ReceiverId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Approver must be selected.")]
        public int ApproverId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Requester must be selected.")]
        public int RequesterId { get; set; }

        public int DomainStateId { get; set; }
    }
}
