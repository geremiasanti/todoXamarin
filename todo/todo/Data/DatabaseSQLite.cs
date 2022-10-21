using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using todo.Models;

namespace todo.Data
{
    public class DatabaseSQLite
    {
        readonly SQLiteAsyncConnection database;

        public DatabaseSQLite(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TodoItem>().Wait();
        }

        public Task<List<TodoItem>> GetTodoItemsAsync()
        {
            //Get all notes.
            return database.Table<TodoItem>().ToListAsync();
        }

        public Task<TodoItem> GetTodoItemsAsync(int id)
        {
            // Get a specific todoItem.
            return database.Table<TodoItem>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveTodoItemAsync(TodoItem todoItem)
        {
            if (todoItem.Id != 0)
            {
                // Update an existing todoItem.
                return database.UpdateAsync(todoItem);
            }
            else
            {
                // Save a new todoItem.
                return database.InsertAsync(todoItem);
            }
        }

        public Task<int> DeleteTodoItemAsync(TodoItem todoItem)
        {
            // Delete a todoItem.
            return database.DeleteAsync(todoItem);
        }
    }
}
