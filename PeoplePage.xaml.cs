using Microsoft.Maui.Controls;
using SQLite;
using System;

namespace MauiAppSQLite
{
    public partial class PeoplePage : ContentPage
    {
        private readonly PeopleDatabase _database;

        public PeoplePage(PeopleDatabase database)
        {
            InitializeComponent();
            _database = database;
        }

        async void OnAddPersonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryName.Text))
            {
                StatusLabel.Text = "Error: Please enter a name.";
                return;
            }

            var person = new Person { Name = EntryName.Text };
            int rowsAffected = await _database.SavePersonAsync(person);

            if (rowsAffected > 0)
            {
                StatusLabel.Text = $"{rowsAffected} record(s) added [Name]: {person.Name}";
            }
            else
            {
                StatusLabel.Text = "Error: Failed to add person.";
            }
        }

        async void OnGetAllPeopleClicked(object sender, EventArgs e)
        {
            var people = await _database.GetPeopleAsync();

            if (people.Count == 0)
            {
                StatusLabel.Text = "No people found.";
            }
            else
            {
                PeopleListView.ItemsSource = people;
                StatusLabel.Text = "";
            }
        }

        async void OnSortClicked(object sender, EventArgs e)
        {
            var sortedPeople = (await _database.GetPeopleAsync()).OrderBy(p => p.Name).ToList();           
            PeopleListView.ItemsSource = sortedPeople;
        }

        async void OnDeleteAllClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Confirmation", "Are you sure you want to delete all people?", "Yes", "No");
            if (result)
            {
                await _database.DeleteAllPeopleAsync();
                await _database.ResetIdsAsync();
                await LoadPeopleList();
                await DisplayAlert("Success", "All people deleted successfully.", "OK");
            }
        }
        async Task LoadPeopleList()
        {
            var people = await _database.GetPeopleAsync();
            PeopleListView.ItemsSource = people;
        }
    }
}
