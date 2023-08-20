using DryIoc;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismDemo.Models;
using PrismDemo.Views;
using System;
using System.Security.Claims;
using System.Xml.Linq;

namespace PrismDemo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _information = "INSERT INTO Students ...";
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        public string Information
        {
            get { return _information; }
            set { SetProperty(ref _information, value); }
        }

        public MainWindowViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _eventAggregator.GetEvent<MessagerStr>().Subscribe(ReceiveStudent);
            _regionManager.RegisterViewWithRegion("FormRegion", typeof(StudentForm));
            _regionManager.RegisterViewWithRegion("ListRegion", typeof(StudentList));
        }

        private void ReceiveStudent(string student)
        {
          
            Information = student;
        }

    }
}
