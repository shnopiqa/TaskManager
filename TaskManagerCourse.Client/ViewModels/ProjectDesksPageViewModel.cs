using Newtonsoft.Json.Linq;
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
using TaskManagerCourse.Common.Models;

using TaskManagerCourse.Client.Views.AddWindows;
using System.IO;

namespace TaskManagerCourse.Client.ViewModels
{
    public class ProjectDesksPageViewModel : BindableBase
    {
        private CommonViewService _viewService;
        private DesksRequestService _desksRequestService;
        private UserRequestService _usersRequestService;
        #region COMMANDS
        public DelegateCommand OpenNewDeskCommand { get; private set; }
        public DelegateCommand<object> OpenUpdateDeskCommand { get; private set; }
        public DelegateCommand CreateOrUpdateDeskCommand { get; private set; }
        public DelegateCommand DeleteDeskCommand { get; private set; }
        public DelegateCommand SelectPhotoForDeskCommand { get; private set; }
        public DelegateCommand AddNewColumnItemCommand { get; private set; }
        public DelegateCommand<object> RemoveColumnItemCommand { get; private set; }

        #endregion
        #region PROPETIES
        private AuthToken _token;
        private ProjectModel _project;
        public UserModel CurrentUser
        {
            get => _usersRequestService.GetCurrentUser(_token);
        }
        public ProjectDesksPageViewModel(AuthToken token, ProjectModel project) 
        {
            _token = token;
            _project = project;
            _viewService = new CommonViewService();
            _desksRequestService = new DesksRequestService();
            _usersRequestService = new UserRequestService();
            UpdatePage();
            
            OpenNewDeskCommand = new DelegateCommand(OpenNewDesk);
            OpenUpdateDeskCommand = new DelegateCommand<object>(OpenUpdateDesk);
            CreateOrUpdateDeskCommand = new DelegateCommand(CreateOrUpdateDesk);
            DeleteDeskCommand = new DelegateCommand(DeleteDesk);
            SelectPhotoForDeskCommand = new DelegateCommand(SelectPhotoForDesk);
            AddNewColumnItemCommand = new DelegateCommand(AddNewColumnItem);
            RemoveColumnItemCommand = new DelegateCommand<object>(RemoveColumnItem);


        }
        private List<ModelClient<DeskModel>> _projectDesks = new List<ModelClient<DeskModel>>();

        public List<ModelClient<DeskModel>> ProjectDesks
        {
            get => _projectDesks; 
            set 
            {
                _projectDesks = value; 
                RaisePropertyChanged(nameof(ProjectDesks));
            }
        }
        private ModelClientAction _typeActionWithDesk;
        public ModelClientAction TypeActionWithDesk
        {
            get => _typeActionWithDesk;
            set
            {
                _typeActionWithDesk = value;
                RaisePropertyChanged(nameof(TypeActionWithDesk));
            }
        }
        private ModelClient<DeskModel> _selectedDesk;

        public ModelClient<DeskModel> SelectedDesk
        {
            get => _selectedDesk;
            set
            {
                _selectedDesk = value;
                RaisePropertyChanged(nameof(SelectedDesk));
            }
           
        }
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
                if(SelectedDesk != null && SelectedDesk.Model != null) 
                {
                    SelectedDesk.Model.Columns = ColumnsForNewDesk.Select(c => c.Value).ToArray();
                }
            }
        }
        #endregion
        private List<ModelClient<DeskModel>> GetDesks(int projectId) 
        {
            var result = new List<ModelClient<DeskModel>>();
            var desks = _desksRequestService.GetDesksByProject(_token, _project.Id);
            if(desks != null) 
            {
                result = desks.Select(d => new ModelClient<DeskModel>(d)).ToList();
            }
            return result;
        }
        private void OpenNewDesk()
        {

            SelectedDesk = new ModelClient<DeskModel>(new DeskModel());
            TypeActionWithDesk = ModelClientAction.Create;
            var wnd = new CreateOrUpdateDeskWindow();
            _viewService.OpenWindow(wnd, this);


        }
        private void OpenUpdateDesk(object deksId)
        {
            SelectedDesk = GetDeskClientById(deksId);
            if(CurrentUser.Id != SelectedDesk.Model.AdminId) 
            {
                _viewService.ShowMessage("Вы не админ");
                return;
            }

            TypeActionWithDesk = ModelClientAction.Update;
            var wnd = new CreateOrUpdateDeskWindow();
            _viewService.OpenWindow(wnd, this);


        }
        private void CreateOrUpdateDesk()
        {
            if(TypeActionWithDesk == ModelClientAction.Create) 
            {
                CreateDesk();
            }
            if(TypeActionWithDesk == ModelClientAction.Update) 
            {
                
                    UpdateDesk();
            }
            UpdatePage();

        }
        private void CreateDesk()
        {
            SelectedDesk.Model.Columns = ColumnsForNewDesk.Select(c => c.Value).ToArray();
            SelectedDesk.Model.ProjectId = _project.Id;
            var resultAction = _desksRequestService.CreateDesk(_token, SelectedDesk.Model);
            _viewService.ShowActionResult(resultAction, "deks is created");

        }

        private void UpdateDesk()
        {
            var resultAction = _desksRequestService.UpdateDesk(_token, SelectedDesk.Model);
            _viewService.ShowActionResult(resultAction, "deks is updated");

        }
        private void DeleteDesk()
        {
            var resultAction = _desksRequestService.DelettDesksById(_token, SelectedDesk.Model.Id);
            _viewService.ShowActionResult(resultAction, "deks is deleted");
            UpdatePage();

        }
        private void UpdatePage()
        {
            
            SelectedDesk = null;
            ProjectDesks = GetDesks(_project.Id);
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
        private ModelClient<DeskModel> GetDeskClientById(object deskId)
        {
            try
            {
                int id = (int)deskId;
                DeskModel desk = _desksRequestService.GetDesksById(_token, id);
                return new ModelClient<DeskModel>(desk);
            }
            catch (FormatException)
            {
                return new ModelClient<DeskModel>(null);
            }

        }
        private void SelectPhotoForDesk()
        {
            _viewService.SetPhotoForObject(SelectedDesk.Model);
            SelectedDesk = new ModelClient<DeskModel>(SelectedDesk.Model);

        }
    }
}
