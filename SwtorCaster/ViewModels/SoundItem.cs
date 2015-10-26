namespace SwtorCaster.ViewModels
{
    public class SoundItem
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public SoundItem()
        {

        }

        public SoundItem(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}