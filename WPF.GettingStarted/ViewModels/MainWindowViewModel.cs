namespace WPF.GettingStarted.ViewModels
{
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using WPF.GettingStarted.Models;
    using WPF.GettingStarted.Services.Interfaces;

    public class MainWindowViewModel : ViewModelBase
    {
        #region Stuff
        private readonly IFamilyService _familyService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;
        #endregion

        #region Constractor
        public MainWindowViewModel(IFamilyService familyService, IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            Argument.IsNotNull(() => familyService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);

            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;
            _familyService = familyService;

            AddFamily = new TaskCommand(OnAddFamilyExecuteAsync);
            EditFamily = new TaskCommand(OnEditFamilyExecuteAsync, OnEditFamilyCanExecute);
            RemoveFamily = new TaskCommand(OnRemoveFamilyExecuteAsync, OnRemoveFamilyCanExecute);
        }
        #endregion

        #region Properties
        public override string Title { get { return "WPF.GettingStarted"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

        /// <summary>
        /// Gets the families.
        /// </summary>
        public ObservableCollection<Family> Families
        {
            get { return GetValue<ObservableCollection<Family>>(FamiliesProperty); }
            private set { SetValue(FamiliesProperty, value); }
        }

        /// <summary>
        /// Register the Families property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FamiliesProperty = RegisterProperty("Families", typeof(ObservableCollection<Family>), null);

        /// <summary>
        /// Gets or sets the selected family.
        /// </summary>
        public Family SelectedFamily
        {
            get { return GetValue<Family>(SelectedFamilyProperty); }
            set { SetValue(SelectedFamilyProperty, value); }
        }

        /// <summary>
        /// Register the SelectedFamily property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SelectedFamilyProperty = RegisterProperty("SelectedFamily", typeof(Family), null);
        #endregion

        #region Commands
        /// <summary>
        /// Gets the AddFamily command.
        /// </summary>
        public TaskCommand AddFamily { get; private set; }

        /// <summary>
        /// Method to invoke when the AddFamily command is executed.
        /// </summary>
        private async Task OnAddFamilyExecuteAsync()
        {
            var family = new Family();
            // Note that we use the type factory here because it will automatically take care of any dependencies
            // that the FamilyWindowViewModel will add in the future
            var typeFactory = this.GetTypeFactory();
            var familyWindowViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<FamilyWindowViewModel>(family);
            if (await _uiVisualizerService.ShowDialogAsync(familyWindowViewModel) ?? false)
            {
                Families.Add(family);
            }
        }

        /// <summary>
        /// Gets the EditFamily command.
        /// </summary>
        public TaskCommand EditFamily { get; private set; }

        /// <summary>
        /// Method to check whether the EditFamily command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnEditFamilyCanExecute()
        {
            return SelectedFamily != null;
        }

        /// <summary>
        /// Method to invoke when the EditFamily command is executed.
        /// </summary>
        private async Task OnEditFamilyExecuteAsync()
        {
            // Note that we use the type factory here because it will automatically take care of any dependencies
            // that the PersonViewModel will add in the future
            var typeFactory = this.GetTypeFactory();
            var familyWindowViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<FamilyWindowViewModel>(SelectedFamily);
            await _uiVisualizerService.ShowDialogAsync(familyWindowViewModel);
        }

        /// <summary>
        /// Gets the RemoveFamily command.
        /// </summary>
        public TaskCommand RemoveFamily { get; private set; }

        /// <summary>
        /// Method to check whether the RemoveFamily command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnRemoveFamilyCanExecute()
        {
            return SelectedFamily != null;
        }

        /// <summary>
        /// Method to invoke when the RemoveFamily command is executed.
        /// </summary>
        private async Task OnRemoveFamilyExecuteAsync()
        {
            if (await _messageService.ShowAsync(string.Format("Are you sure you want to delete the family '{0}'?", SelectedFamily),
                "Are you sure?", MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
            {
                Families.Remove(SelectedFamily);
                SelectedFamily = null;
            }
        }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var families = _familyService.LoadFamilies();
            Families = new ObservableCollection<Family>(families);
        }

        protected override async Task CloseAsync()
        {
            _familyService.SaveFamilies(Families);

            await base.CloseAsync();
        }
        #endregion
    }
}
