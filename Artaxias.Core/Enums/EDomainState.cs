
namespace Artaxias.Core.Enums
{
    public enum EDomainState
    {
        Requested = 1,
        Pending = 2,

        Processing = 3,
        Changed = 4,
        Reopened = 5,
        Commented = 6,

        Approved = 7,
        Rejected = 8,

        Completed = 9,
        Abandoned = 10
    }
}
