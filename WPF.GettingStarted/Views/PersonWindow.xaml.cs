namespace WPF.GettingStarted.Views
{
    using Catel.Windows;
    using ViewModels;

    public partial class PersonWindow
    {
        public PersonWindow()
            : this(null) { }

        public PersonWindow(PersonViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
