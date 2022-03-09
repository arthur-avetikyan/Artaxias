
using Artaxias.Data.Models;
using Artaxias.Data.Models.Feadback;
using Artaxias.Data.Models.Organization;

using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;

namespace Artaxias.Models
{
    public partial class User : IdentityUser<int>
    {
        // public virtual int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public IList<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public IList<FeedbackTemplate> FeedbackTemplates { get; set; } = new List<FeedbackTemplate>();

        public IList<Review> Reviews { get; set; } = new List<Review>();

        public IList<ContractTemplate> ContractTemplates { get; set; } = new List<ContractTemplate>();
    }
}