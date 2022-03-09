
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artaxias.Data.Models.Organization
{
    public class AbsenceType
    {
        public AbsenceType()
        {
            AbsencesAssigned = new List<Absence>();
        }

        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public virtual IList<Absence> AbsencesAssigned { get; }
    }
}
