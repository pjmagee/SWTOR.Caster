namespace SwtorCaster.ViewModels
{
    using System.Diagnostics;

    public class AboutViewModel : FocusableScreen
    {
        public override string DisplayName { get; set; } = "SWTOR Caster - About";

        public void OpenSWTORCaster()
        {
            Process.Start("https://torcommunity.com/tools/swtor-caster");
        }

        public void OpenGuild()
        {
            Process.Start("http://awakenedgamers.com/");
        }

        public void OpenReferral()
        {
            Process.Start("http://www.swtor.com/r/rpNrdB");
        }

        public void OpenTwitch()
        {
            Process.Start("http://www.twitch.tv/delegate_");
        }

        public void OpenGithub()
        {
            Process.Start("https://github.com/pjmagee/swtor-caster/");
        }

        public void OpenJediPedia()
        {
            Process.Start("https://swtor.jedipedia.net/en");
        }
    }
}