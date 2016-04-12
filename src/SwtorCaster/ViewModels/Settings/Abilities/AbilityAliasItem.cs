namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;

    public class AbilityAliasItem : PropertyChangedBase
    {
        private string name;

        public AbilityAliasItem(string name = "")
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value == name) return;
                name = value;
                NotifyOfPropertyChange();
            }
        }
    }
}