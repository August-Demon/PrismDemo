using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using PrismDemo.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismDemo.ViewModels
{
    public class StudentFormViewModel: BindableBase
    {


        public delegate void PropertyChangedEventHandle(string value);

        public event PropertyChangedEventHandle PropertyChangedEvent;

        private string _name;

		public string Name
		{
			get { return _name; }
			set {SetProperty(ref _name,value); PropertyChangedEvent?.Invoke(SqlQuery); }
		}

		private string _class;

		public string Class
        {
			get { return _class; }
			set { SetProperty(ref _class, value); PropertyChangedEvent?.Invoke(SqlQuery); }
		}

		private string _phone;

		public string Phone
		{
			get { return _phone; }
			set { SetProperty(ref _phone, value); PropertyChangedEvent?.Invoke(SqlQuery); }
		}

        public string SqlQuery =>
       $"INSERT INTO Students (Name, Class, Phone) VALUES ('{Name}', '{Class}', '{Phone}')";

        private bool _allowAddNew;

		private readonly IEventAggregator _eventAggregator;
        public StudentFormViewModel(IEventAggregator  eventAggregator)
        {
            _eventAggregator = eventAggregator;
			_eventAggregator.GetEvent<MessagerEvent>().Subscribe(showMethod);
            PropertyChangedEvent += PropertyChangedMessage;
        }

        private void PropertyChangedMessage(string value)
        {
            _eventAggregator.GetEvent<MessagerStr>().Publish(value);
        }

        void showMethod(bool value)
		{
            _allowAddNew = value;
			AddNewCommand.RaiseCanExecuteChanged();

        }

        private DelegateCommand _addNewCommand;
        public DelegateCommand AddNewCommand =>
            _addNewCommand ?? (_addNewCommand = new DelegateCommand(AddNew, CanExecuteCommand));

		private void AddNew()
		{
			_eventAggregator.GetEvent<MessagerStudent>().Publish(new(Name,Class,Phone));
		}

		bool CanExecuteCommand()
		{
            return _allowAddNew;
		}


    }
}
