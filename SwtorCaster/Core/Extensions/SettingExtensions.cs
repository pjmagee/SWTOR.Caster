namespace SwtorCaster.Core.Extensions
{
    using Domain;

    public static class SettingExtensions
    {
        public static bool CanPlay(this EventSetting setting, LogLine line)
        {
            return setting.IsPlayerDeath(line) ||
                   setting.IsPlayerKill(line) ||
                   setting.IsEnterCombat(line) ||
                   setting.IsExitCombat(line) || 
                   setting.IsAbilityActivate(line) || 
                   setting.IsAbilityCancel(line);
        }

        public static bool IsAbilityActivate(this EventSetting setting, LogLine line)
        {
            return setting.EventDetailType == EventDetailType.AbilityActivate && 
                      line.EventDetailType == EventDetailType.AbilityActivate;
        }

        public static bool IsEnterCombat(this EventSetting setting, LogLine line)
        {
            return setting.EventDetailType == EventDetailType.EnterCombat && 
                      line.EventDetailType == EventDetailType.EnterCombat;
        }

        public static bool IsExitCombat(this EventSetting setting, LogLine line)
        {
            return setting.EventDetailType == EventDetailType.ExitCombat && 
                      line.EventDetailType == EventDetailType.ExitCombat;
        }

        public static bool IsPlayerDeath(this EventSetting setting, LogLine line)
        {
            return setting.EventDetailType == EventDetailType.Death &&
                      line.EventDetailType == EventDetailType.Death && 
                      line.IsCurrentPlayer;
        }

        public static bool IsPlayerKill(this EventSetting setting, LogLine line)
        {
            return setting.EventDetailType == EventDetailType.Death && 
                      line.EventDetailType == EventDetailType.Death && 
                     !line.IsCurrentPlayer;
        }

        public static bool IsAbilityCancel(this EventSetting setting, LogLine line)
        {
            return setting.EventDetailType == EventDetailType.AbilityCancel &&
                      line.EventDetailType == EventDetailType.AbilityCancel && 
                      line.IsCurrentPlayer;
        }
    }
}