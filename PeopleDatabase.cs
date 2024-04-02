using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppSQLite
{
    public class PeopleDatabase
    {
        SQLiteAsyncConnection Database;

        public PeopleDatabase()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<Person>();
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            await Init();
            return await Database.Table<Person>().ToListAsync();
        }

        public async Task<int> SavePersonAsync(Person person)
        {
            int rowsAffected = 0;
            await Init();

            if (person.ID != 0)
                rowsAffected = await Database.UpdateAsync(person);
            else
                rowsAffected = await Database.InsertAsync(person);

            return rowsAffected;
        }

        public async Task<int> DeleteAllPeopleAsync()
        {
            return await Database.DeleteAllAsync<Person>();
        }

        public async Task ResetIdsAsync()
        {
            await Database.DropTableAsync<Person>();
            await Database.CreateTableAsync<Person>();
        }
    }
}
