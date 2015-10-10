namespace SwtorCaster.ViewModel
{
    using GalaSoft.MvvmLight.Ioc;
    using Microsoft.Practices.ServiceLocation;

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AbilitiesViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public AbilitiesViewModel AbilitiesViewModel => ServiceLocator.Current.GetInstance<AbilitiesViewModel>();

        public static void Cleanup()
        {
            var abilitiesViewModel = ServiceLocator.Current.GetInstance<AbilitiesViewModel>();
            abilitiesViewModel.Stop();
        }
    }
}