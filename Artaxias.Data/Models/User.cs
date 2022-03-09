
using Artaxias.Data.Models;
using Artaxias.Data.Models.Feadback;

using System;
using System.Collections.Generic;

namespace Artaxias.Models
{
    public partial class User
    {

        public User() : base()
        {
            Roles_CreatedByUserId = new List<Role>();
            Roles_UpdatedByUserId = new List<Role>();
            UserRoles = new List<UserRole>();
            RefreshTokens = new List<RefreshToken>();
            FeedbackTemplates = new List<FeedbackTemplate>();
            Reviews = new List<Review>();
            OnCreated();
        }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int HashingIterationCount { get; set; }

        public virtual byte[] HashingSalt { get; set; }

        public virtual DateTime CreatedDatetimeUTC { get; set; }

        public virtual DateTime? UpdatedDatetimeUTC { get; set; }

        public virtual IList<Role> Roles_CreatedByUserId { get; set; }

        public virtual IList<Role> Roles_UpdatedByUserId { get; set; }

        public virtual IList<UserRole> UserRoles { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
