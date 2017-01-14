namespace SwtorCaster.ViewModels.Settings
{
    using Caliburn.Micro;
    using Core.Domain.Settings;

    public class LayoutViewModel : PropertyChangedBase
    {
        public string Name
        {
            get
            {
                if (Layout == Layout.LeftToRight) return "Left to Right";
                if (Layout == Layout.RightToLeft) return "Right to Left";
                return "Not set";
            }
        }

        public Layout Layout { get; set; }
    }
}
