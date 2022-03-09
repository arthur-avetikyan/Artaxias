
using System;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Web.Common.DataTransferObjects.Organization
{
    public class AbsenceRequest
    {
        [Required]
        public int ReceiverId { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int ApproverId { get; set; }

        public bool Approved { get; set; }

        public AbsenceRequest Clone()
        {
            return (AbsenceRequest)MemberwiseClone();
        }
    }
}
