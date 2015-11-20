namespace SwtorCaster.ViewModels
{
    using System.Diagnostics;

    public class AboutViewModel : FocusableScreen
    {
        public override string DisplayName { get; set; } = "SWTOR Caster - About";

        public void OpenSWTORCaster()
        {
            Process.Start("http://pjmagee.github.io/swtor-caster/");
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
            Process.Start("http://www.twitch.tv/sneakymaan");
        }

        public void OpenGithub()
        {
            Process.Start("https://github.com/pjmagee/swtor-caster/");
        }

        public void OpenTorCommunity()
        {
            Process.Start("http://torcommunity.com/");
        }
    }
}