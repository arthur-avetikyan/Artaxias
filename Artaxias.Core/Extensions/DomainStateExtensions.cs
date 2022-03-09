
using Artaxias.Core.Enums;
using System;

namespace Artaxias.Core.Extensions
{
    public static class DomainStateExtensions
    {
        public static string GetDomainStateName(this int state)
        {
            return Enum.GetName(typeof(EDomainState), state);
        }

        public static string GetDomainStateName(this EDomainState state)
        {
            return Enum.GetName(typeof(EDomainState), state);
        }

        public static string GenerateDomainStateColor(this int stateId)
        {
            EDomainState state = (EDomainState)stateId;
            return state switch
            {
                EDomainState.Pending => "#ffef96",
                EDomainState.Requested => "#f9ff56",

                EDomainState.Processing => "#b7d7e8",
                EDomainState.Changed => "#bee8e1",
                EDomainState.Reopened => "#92a8d1",
                EDomainState.Commented => "#c2c2e8",

                EDomainState.Approved => "#c4ed7c",
                EDomainState.Rejected => "#eea29a",

                EDomainState.Completed => "#aced9e",
                EDomainState.Abandoned => "#ed847d",

                _ => "#FFFFFF"
            };
        }

        public static string GenerateDomainStateIconName(this int stateId)
        {
            EDomainState state = (EDomainState)stateId;
            return state switch
            {
                EDomainState.Pending => "warning",
                EDomainState.Requested => "help",

                EDomainState.Processing => "autorenew",
                EDomainState.Reopened => "settings_backup_restore",
                EDomainState.Changed => "change_history",
                EDomainState.Commented => "comment",

                EDomainState.Approved => "check_box",
                EDomainState.Rejected => "cancel",

                EDomainState.Completed => "thumb_up",
                EDomainState.Abandoned => "thumb_down",

                _ => "error"
            };
        }
    }
}
