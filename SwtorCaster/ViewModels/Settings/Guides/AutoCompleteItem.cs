namespace SwtorCaster.ViewModels
{
    public class AutoCompleteItem
    {
        public string AbilityId { get; set; }

        public string AbilityName { get; set; }

        public string AbilityImage { get; set; }

        public AutoCompleteItem(string abilityImage)
        {
            AbilityImage = abilityImage;
        }
    }
}