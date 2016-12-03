namespace SwtorCaster.Core.Services.Parsing
{
    using System;
    using System.Text.RegularExpressions;
    using Domain.Log;
    using System.Diagnostics;
    using SwtorCaster.Core.Services.Logging;

    public class CombatLogParser : ICombatLogParser
    {
        private static string CurrentPlayer { get; set; }

        private const string BaseLineRegexPattern = @"^\[(?<TimeStamp>[^\]]*)\] \[(?<Source>[^\]]*?)\] \[(?<Target>[^\]]*?)\] \[(?<Ability>[^\]]*?)\] \[(?<Effect>.*?)\] \((?<Value>.*?)\)($|( <(?<Threat>[^>]*?)>))";
        private const string TimeStampRegexPattern = @"^(\d{2}):(\d{2}):(\d{2})\.(\d{3})";
        private const string NpcRegexPattern = @"^([^{]*) {(\d*)}:(\d*)";
        private const string CompanionRegexPattern = @"^([^:]*):([^{]*) {(\d*)}";
        private const string EntityRegexPattern = @"^([^{]*) {(\d*)}";
        private const string EffectRegexPattern = @"^([^{]* {\d*}): ([^{]* {\d*})";
        private const string AbsorbRegexPattern = @"\((\d+) ([^\)]*)\)";
        private const string ValueRegexPattern = @"^(\d+)(\*)?( \w+ {\d+})?(\(\w+ {\d+}\))?( -(\w+ {\d+})?)?( \(\d+ \w+ {\d+}\))?$";

        private static readonly Regex BaseLineRegex = new Regex(BaseLineRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex TimeStampRegex = new Regex(TimeStampRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex NpcRegex = new Regex(NpcRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex CompanionRegex = new Regex(CompanionRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex EntityRegex = new Regex(EntityRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex EffectRegex = new Regex(EffectRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex AbsorbRegex = new Regex(AbsorbRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex ValueRegex = new Regex(ValueRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private CombatLogEvent CombatLogEvent { get; set; }

        private readonly ILoggerService loggerService;

        public CombatLogParser(ILoggerService loggerService)
        {
            this.loggerService = loggerService;
        }

        public CombatLogEvent Parse(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new ParseException("Line was null.");

            CombatLogEvent = new CombatLogEvent();

            var match = BaseLineRegex.Match(line);

            if (match.Success)
            {
                ProcessTimeStamp(match.Groups["TimeStamp"].Value);
                ProcessAbility(match.Groups["Ability"].Value);
                ProcessEffect(match.Groups["Effect"].Value);
                ProcessValue(match.Groups["Value"].Value);
                ProcessThreat(match.Groups["Threat"].Value);
                ProcessSource(match.Groups["Source"].Value);
                ProcessTarget(match.Groups["Target"].Value);
            }

            ProcessTrackedPlayer();

            return CombatLogEvent;
        }

        private void ProcessTrackedPlayer()
        {
            if (CombatLogEvent.IsSafeLogin())
            {
                CurrentPlayer = CombatLogEvent.Source.DisplayName;
            }

            if (string.IsNullOrEmpty(CurrentPlayer) && CombatLogEvent.IsAbilityActivate())
            {
                CurrentPlayer = CombatLogEvent.Source.DisplayName;
            }

            if (CombatLogEvent.Source != null)
            {
                CombatLogEvent.Source.IsThisPlayer = CurrentPlayer == CombatLogEvent.Source.DisplayName;
            }

            if (CombatLogEvent.Target != null)
            {
                CombatLogEvent.Target.IsThisPlayer = CurrentPlayer == CombatLogEvent.Target.DisplayName;
            }
        }

        private void ProcessThreat(string threat)
        {
            if (string.IsNullOrEmpty(threat)) return;
            CombatLogEvent.Threat = Convert.ToInt32(threat);
        }

        private void ProcessValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            var match = ValueRegex.Match(value);
            if (!match.Success) return;

            CombatLogEvent.Value = Convert.ToInt32(match.Groups[1].Value);
            CombatLogEvent.IsCrit = !string.IsNullOrEmpty(match.Groups[2].Value);

            ProcessDamageType(match);
            ProcessDamageModifier(match);
            ProcessMitigation(match);
            ProcessAbsorb(match);
        }

        private void ProcessAbsorb(Match match)
        {
            string absorbString = match.Groups[7].Value.Trim();

            if (string.IsNullOrEmpty(absorbString)) return;

            var absorbMatch = AbsorbRegex.Match(absorbString);

            if (absorbMatch.Success)
            {
                CombatLogEvent.AbsorbedValue = Convert.ToInt32(absorbMatch.Groups[1].Value);
                CombatLogEvent.AbsorbType = ProcessEntity(absorbMatch.Groups[2].Value);
            }
        }

        private void ProcessMitigation(Match match)
        {
            string mitigationString = match.Groups[5].Value.Trim();

            if (!string.IsNullOrEmpty(mitigationString))
            {
                CombatLogEvent.Mitigation = ProcessEntity(mitigationString.Substring(1));
            }
        }

        private void ProcessDamageModifier(Match match)
        {
            string reflectString = match.Groups[4].Value.Trim();

            if (!string.IsNullOrEmpty(reflectString))
            {
                CombatLogEvent.DamageModifier = ProcessEntity(reflectString.Substring(1, reflectString.LastIndexOf("}")));
            }
        }

        private void ProcessDamageType(Match match)
        {
            string damageTypeString = match.Groups[3].Value.Trim();

            if (!string.IsNullOrEmpty(damageTypeString))
            {
                CombatLogEvent.DamageType = ProcessEntity(damageTypeString);
            }
        }

        private void ProcessEffect(string effect)
        {
            if (string.IsNullOrEmpty(effect)) return;

            var match = EffectRegex.Match(effect);

            if (!match.Success) return;

            CombatLogEvent.EffectType = ProcessEntity(match.Groups[1].Value);
            CombatLogEvent.EffectName = ProcessEntity(match.Groups[2].Value);
        }

        private void ProcessAbility(string ability)
        {
            if (string.IsNullOrEmpty(ability)) return;
            CombatLogEvent.Ability = ProcessEntity(ability);
        }

        private CombatLogEntity ProcessEntity(string value)
        {
            var entity = new CombatLogEntity(CombatLogEntity.EmptyEntityName, entityId: 0);

            var match = EntityRegex.Match(value);

            if (!match.Success) return entity;

            ProcessDisplayName(entity, match);

            entity.EntityId = Convert.ToInt64(match.Groups[2].Value);

            return entity;
        }

        private static void ProcessDisplayName(CombatLogEntity entity, Match match)
        {
            string name = match.Groups[1].Value;

            if (!string.IsNullOrEmpty(name))
            {
                entity.DisplayName = name;
            }
        }

        private void ProcessTarget(string target)
        {
            CombatLogEvent.Target = ProcessParticipant(target);
        }

        private void ProcessSource(string source)
        {
            CombatLogEvent.Source = ProcessParticipant(source);
        }

        private void ProcessTimeStamp(string timeStamp)
        {
            var match = TimeStampRegex.Match(timeStamp);
            if (!match.Success) return;
            CombatLogEvent.TimeStamp = DateTime.Parse(timeStamp);
        }

        private CombatLogParticipant ProcessParticipant(string entity)
        {
            string name;

            var participant = new CombatLogParticipant(CombatLogParticipant.EmptyParticipantName, entityId: 0)
            {
                IsPlayer = entity.StartsWith("@")
            };

            if (participant.IsPlayer)
            {
                name = MatchByPlayer(entity, participant);
            }
            else // NPC
            {
                name = MatchByNPC(entity, participant);
            }

            if (!string.IsNullOrEmpty(name))
            {
                participant.DisplayName = name;
            }

            return participant;
        }

        private static string MatchByNPC(string entity, CombatLogParticipant participant)
        {
            var match = NpcRegex.Match(entity);

            if (match.Success)
            {
                participant.EntityId = Convert.ToInt64(match.Groups[2].Value);
                participant.UniqueId = Convert.ToInt64(match.Groups[3].Value);
                return match.Groups[1].Value;
            }

            return null;
        }

        private static string MatchByPlayer(string entity, CombatLogParticipant participant)
        {
            if (entity.EndsWith("}"))
            {
                var match = CompanionRegex.Match(entity.Substring(1));

                if (match.Success)
                {
                    participant.IsPlayerCompanion = true;
                    participant.CompanionOwner = match.Groups[1].Value;
                    participant.EntityId = Convert.ToInt64(match.Groups[3].Value);
                    return match.Groups[2].Value;
                }
            }
            else
            {
                return entity.Substring(1);
            }

            return null;
        }
    }
}