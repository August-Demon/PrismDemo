using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using PrismDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismDemo.ViewModels
{
    public class StudentListViewModel:BindableBase
    {
        public ObservableCollection<Student> Students { get; } = new();

        public delegate void AllowAddNewEventHandle(bool value);

        public event AllowAddNewEventHandle AllowAddNewEvent;

        private readonly IEventAggregator _eventAggregator;

        private bool _allowAddNew;
        public bool AllowAddNew
        {
            get { return _allowAddNew; }
            set { SetProperty(ref _allowAddNew, value); AllowAddNewEvent?.Invoke(value); }
        }

        public int StudentCount => Students?.Count ?? 0;


        public StudentListViewModel(IEventAggregator eventAggregator)
        {
            Students.CollectionChanged += (_ , _) => OnPropertyChanged( new PropertyChangedEventArgs(nameof(StudentCount)));

            AllowAddNewEvent += OnAllowAddNewChanging;

            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<MessagerStudent>().Subscribe(ReceiveStudent);
        }

        private void ReceiveStudent(Student student)
        {
            Students.Add(student);
        }

        private void OnAllowAddNewChanging(bool value)
        {
            _eventAggregator.GetEvent<MessagerEvent>().Publish(value);
        }
    }
}
