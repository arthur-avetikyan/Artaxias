using Artaxias.Data.Models.Feadback;
using Artaxias.Data.Models.Organization;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artaxias.Data.Models
{
    [Table(nameof(DomainState))]
    public class DomainState
    {
        public DomainState()
        {
            ReviewerReviewees = new List<ReviewerReviewee>();
            Absences = new List<Absence>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [InverseProperty("DomainState")]
        public virtual IList<ReviewerReviewee> ReviewerReviewees { get; set; }

        public virtual IList<Absence> Absences { get; set; }

        public virtual IList<Bonus> Bonuses { get; set; }
    }
}
