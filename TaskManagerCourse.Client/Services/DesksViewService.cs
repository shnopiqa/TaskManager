﻿using Newtonsoft.Json.Linq;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Views.AddWindows;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Services
{
    public class DesksViewService
    {
        private AuthToken _token;
        private DesksRequestService _desksRequestService;
        private CommonViewService _viewService;
        public DesksViewService(AuthToken token, DesksRequestService deskRequestService) 
        {
            _token = token;
            _desksRequestService = deskRequestService;
            _viewService = new CommonViewService();
        }
        public ModelClient<DeskModel> GetDeskClientById(object projectId)
        {
            try
            {
                int id = (int)projectId;
                DeskModel desk = _desksRequestService.GetDesksById(_token, id);
                return new ModelClient<DeskModel>(desk);
            }
            catch (FormatException)
            {
                return new ModelClient<DeskModel>(null);
            }

        }
        public List<ModelClient<DeskModel>> GetDesks(int projectId)
        {
            var result = new List<ModelClient<DeskModel>>();
            var desks = _desksRequestService.GetDesksByProject(_token, projectId);
            if(desks != null) 
            {
                result = desks.Select(d => new ModelClient<DeskModel>(d)).ToList();

            }
            return result;
        }
        public List<ModelClient<DeskModel>> GetAllDesks()
        {
            var result = new List<ModelClient<DeskModel>>();
            var desks = _desksRequestService.GetAllDesks(_token);
            if (desks != null)
            {
                result = desks.Select(d => new ModelClient<DeskModel>(d)).ToList();

            }
            return result;
        }
        public void OpenDeskViewInfo(object deksId, BindableBase context )
        {

            var wnd = new CreateOrUpdateDeskWindow();
            _viewService.OpenWindow(wnd, context);


        }
        public void UpdateDesk(DeskModel desk)
        {
            var resultAction = _desksRequestService.UpdateDesk(_token, desk);
            _viewService.ShowActionResult(resultAction, "deks is updated");

        }
        public void DeleteDesk(int deskId)
        {
            var resultAction = _desksRequestService.DelettDesksById(_token, deskId);
            _viewService.ShowActionResult(resultAction, "deks is deleted");
        

        }
        public ModelClient<DeskModel> SelectPhotoForDesk(ModelClient<DeskModel> selectedDesk) 
        {
            if(selectedDesk?.Model != null) 
            {
                _viewService.SetPhotoForObject(selectedDesk.Model);
                selectedDesk = new ModelClient<DeskModel>(selectedDesk.Model);
                
            }
            return selectedDesk;
        }

    }
}
