using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
using TaskManagerCourse.Client.Views.AddWindows;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.ViewModels
{
    public class UserDeskPageViewModel : BindableBase
    {
        private AuthToken _token;
        private DesksRequestService _desksRequestService;
        private UserRequestService _usersRequestService;
        private CommonViewService _viewService;
        private DesksViewService _desksViewService;

        #region COMMANDS
        public DelegateCommand OpenEditDeskCommand { get; private set; }

        public DelegateCommand CreateOrUpdateDeskCommand { get; private set; }
        public DelegateCommand DeleteDeskCommand { get; private set; }
        public DelegateCommand SelectPhotoForDeskCommand { get; private set; }
        public DelegateCommand AddNewColumnItemCommand { get; private set; }
        public DelegateCommand<object> RemoveItemColumnCommand { get; private set; }


        #endregion
        public UserDeskPageViewModel(AuthToken token)
        {
            _token = token;
            _viewService = new CommonViewService();
            _desksRequestService = new DesksRequestService();
            _usersRequestService = new UserRequestService();
            _desksViewService = new DesksViewService(_token, _desksRequestService);
            UpdatePage();
            OpenEditDeskCommand = new DelegateCommand(OpenUpdateDesk);
            CreateOrUpdateDeskCommand = new DelegateCommand(UpdateDesk);
            DeleteDeskCommand = new DelegateCommand(DeleteDesk);
            SelectPhotoForDeskCommand = new DelegateCommand(SelectPhotoForDesk);
            AddNewColumnItemCommand = new DelegateCommand(AddNewColumnItem);
            RemoveItemColumnCommand = new DelegateCommand<object>(RemoveColumnItem);

            ContextMenuCommands.Add("Edit", OpenEditDeskCommand);
            ContextMenuCommands.Add("Delete", DeleteDeskCommand);

        }
        #region PROPERTIES
        private List<ModelClient<DeskModel>> _allDesks = new List<ModelClient<DeskModel>>();
        public List<ModelClient<DeskModel>> AllDesks 
        {
            get => _allDesks;
            set 
            {
                _allDesks = value;
                RaisePropertyChanged(nameof(AllDesks));
            }
        }

        private ModelClient<DeskModel> _selectedDesk;
        private ObservableCollection<ColumnBindingHelp> _columnsForNewDesk = new ObservableCollection<ColumnBindingHelp>()
        {
            new ColumnBindingHelp("New"),
            new ColumnBindingHelp("InProgress"),
            new ColumnBindingHelp("InRewiev"),
            new ColumnBindingHelp("Completed")

        };
        public ObservableCollection<ColumnBindingHelp> ColumnsForNewDesk
        {
            get => _columnsForNewDesk;
            set
            {
                _columnsForNewDesk = value;
                RaisePropertyChanged(nameof(ColumnsForNewDesk));
                if (SelectedDesk != null && SelectedDesk.Model != null)
                {
                    SelectedDesk.Model.Columns = ColumnsForNewDesk.Select(c => c.Value).ToArray();
                }
            }
        }
        public ModelClient<DeskModel> SelectedDesk
        {
            get => _selectedDesk;
            set
            {
                _selectedDesk = value;
                RaisePropertyChanged(nameof(SelectedDesk));
            }
        }
        private Dictionary<string, DelegateCommand> _contextMenuCommands = new Dictionary<string, DelegateCommand>();

        public Dictionary<string, DelegateCommand> ContextMenuCommands
        {
            get => _contextMenuCommands;
            set
            {
                _contextMenuCommands = value;
                RaisePropertyChanged(nameof(ContextMenuCommands));
            }
        }

        #endregion
        #region METHODS
        private void UpdateDesk() 
        {
            SelectedDesk.Model.Columns = ColumnsForNewDesk.Select(c => c.Value).ToArray();
            _desksViewService.UpdateDesk(SelectedDesk.Model);
            UpdatePage();
        }
        private void DeleteDesk()
        {
            _desksViewService.DeleteDesk(SelectedDesk.Model.Id);
            UpdatePage();
        }
        private void OpenUpdateDesk() 
        {
            SelectedDesk = _desksViewService.GetDeskClientById(SelectedDesk.Model.Id);
            ColumnsForNewDesk = new ObservableCollection<ColumnBindingHelp>(SelectedDesk.Model.Columns.Select(c => new ColumnBindingHelp(c)));
            _desksViewService.OpenDeskViewInfo(SelectedDesk.Model.Id, this);
        }
        private void SelectPhotoForDesk()
        {
            SelectedDesk = _desksViewService.SelectPhotoForDesk(SelectedDesk);


        }
        private void UpdatePage()
        {

            SelectedDesk = null;
            AllDesks = _desksViewService.GetAllDesks();
            _viewService.CurrentOpenedWindow?.Close();

        }
        private void AddNewColumnItem()
        {
            ColumnsForNewDesk.Add(new ColumnBindingHelp("Column"));
        }
        private void RemoveColumnItem(object item)
        {
            var itemToRemove = item as ColumnBindingHelp;
            ColumnsForNewDesk.Remove(itemToRemove);
        }
        #endregion
    }
}
