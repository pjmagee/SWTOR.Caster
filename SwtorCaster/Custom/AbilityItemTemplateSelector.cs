namespace SwtorCaster.Custom
{
    using System.Windows;
    using System.Windows.Controls;
    using ViewModels;

    public class AbilityItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Critical { get; set; }
        public DataTemplate Normal { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var logItem = item as CombatLogViewModel;

            if (logItem != null)
            {
                return logItem.IsCrit ? Critical : Normal;
            }

            return base.SelectTemplate(item, container);
        }
    }
}