namespace SwtorCaster.ViewModel
{
    using GalaSoft.MvvmLight.Ioc;
    using Microsoft.Practices.ServiceLocation;

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<AbilitiesViewModel>();
        }
        
        public AbilitiesViewModel AbilitiesViewModel => ServiceLocator.Current.GetInstance<AbilitiesViewModel>();

        public static void Cleanup()
        {
            var abilitiesViewModel = ServiceLocator.Current.GetInstance<AbilitiesViewModel>();
            abilitiesViewModel.Stop();
        }
    }
}