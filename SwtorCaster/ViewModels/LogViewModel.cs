namespace SwtorCaster.ViewModels
{
    using System.Windows;
    using Caliburn.Micro;
    using Core.Services;

    public class LogViewModel : Screen
    {
        private readonly ILoggerService _loggerService;

        public override string DisplayName { get; set; } = "SWTOR Caster - Log";

        public void Focus()
        {
            var window = GetView() as Window;
            window?.Activate();
        }

        public LogViewModel(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public string LogText => _loggerService.Text;

        public void ClearLog()
        {
            _loggerService.Clear();
            Refresh();
        }
    }
}