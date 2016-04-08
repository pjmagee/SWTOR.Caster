namespace SwtorCaster.Core.Domain.Log
{
    using System;

    public class CombatLogEventArgs : EventArgs
    {
        public CombatLogEvent Event { get; }

        public CombatLogEventArgs(CombatLogEvent @event)
        {
            Event = @event;
        }
    }
}