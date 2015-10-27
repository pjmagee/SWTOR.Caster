namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;

    public class AbilityAliasViewModel : PropertyChangedBase
    {
        private string _name;

        public AbilityAliasViewModel(string name = "")
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange();
            }
        }
    }
}