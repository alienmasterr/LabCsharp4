using Lab_CSharp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Lab_CSharp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> People { get; set; } = new();
        private ObservableCollection<Person> _filteredPeople;
        public ObservableCollection<Person> FilteredPeople
        {
            get => _filteredPeople;
            set
            {
                _filteredPeople = value;
                OnPropertyChanged();
            }
        }

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                FilterPeople();
                OnPropertyChanged();
            }
        }

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel()
        {
            AddCommand = new RelayCommand(_ => AddPerson());
            EditCommand = new RelayCommand(_ => EditPerson(), _ => SelectedPerson != null);
            DeleteCommand = new RelayCommand(_ => DeletePerson(), _ => SelectedPerson != null);

            GenerateDemoData(); // тільки при першому запуску
        }

        private void GenerateDemoData()
        {
            if (People.Count == 0)
            {
                for (int i = 0; i < 50; i++)
                {
                    People.Add(new Person(
                        $"Name{i}",
                        $"Surname{i}",
                        $"email{i}@domain.com",
                        DateTime.Today.AddYears(-20).AddDays(i)));
                }
            }

            FilterPeople(); // застосуємо фільтрацію на старті
        }

        private void FilterPeople()
        {
            var filtered = People.Where(p =>
                string.IsNullOrEmpty(SearchQuery)  p.FirstName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) 
                p.LastName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                p.Email.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            FilteredPeople = new ObservableCollection<Person>(filtered);
        }

        private void AddPerson()
        {
            var person = new Person("New", "User", "example@email.com", DateTime.Today.AddYears(-18));
            People.Add(person);
            FilterPeople();
        }

        private void EditPerson()
        {
            if (SelectedPerson != null)
            {
                SelectedPerson.Update("Edited", "User", "edited@email.com", DateTime.Today.AddYears(-25));
                OnPropertyChanged(nameof(People)); // оновити відображення
                FilterPeople(); // повторна фільтрація після редагування
            }
        }

        private void DeletePerson()
        {
            if (SelectedPerson != null)
            {
                People.Remove(SelectedPerson);
                FilterPeople();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}