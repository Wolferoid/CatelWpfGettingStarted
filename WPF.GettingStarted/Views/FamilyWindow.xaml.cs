namespace WPF.GettingStarted.Views
{
    using Catel.Windows;
    using ViewModels;

    public partial class FamilyWindow
    {
        public FamilyWindow()
            : this(null) { }

        public FamilyWindow(FamilyWindowViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
