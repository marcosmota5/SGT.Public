namespace SGT.HelperClasses
{
    public interface IPageViewModel
    {
        string Name { get; }
        string Icone { get; }

        public void LimparViewModel();
    }
}