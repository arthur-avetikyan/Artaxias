namespace Artaxias.Models
{
    public partial class UserRole
    {

        public UserRole()
        {
            OnCreated();
        }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }

        #region Extensibility Method Definitions

        partial void OnCreated();

        #endregion
    }

}
