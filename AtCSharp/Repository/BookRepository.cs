using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApp.Models;

namespace Api.Repository
{
    public class BookRepository
    {
        private string ConnectionString { get; set; }

        public BookRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("AtConnectionString");
        }

        public List<Book> GetBooks()
        {
            List<Book> result = new List<Book>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM [dbo].[Book]";
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Book person = new Book(
                        Guid.Parse(reader["Id"].ToString()),
                        reader["Title"].ToString(),
                        reader["ISBN"].ToString(),
                        int.Parse(reader["Year"].ToString())
                    );
                    result.Add(person);
                }

                connection.Close();
                return result;
            }
        }

        public Book GetBookById(string id)
        {
            List<Book> result = new List<Book>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM [dbo].[Book] WHERE Id = @P1";
                sqlCommand.Parameters.AddWithValue("P1", id);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Book person = new Book(
                        Guid.Parse(reader["Id"].ToString()),
                        reader["Title"].ToString(),
                        reader["ISBN"].ToString(),
                        int.Parse(reader["Year"].ToString())
                    );
                    result.Add(person);
                }

                connection.Close();
                if (result.Count > 0) return result[0];
                else return null;
            }
        }

        public void Save(Book book)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "INSERT INTO [dbo].[Author](Id, Title, ISBN, Year) VALUES (@P1, @P2, @P3, @P4)";
                sqlCommand.Parameters.AddWithValue("P1", book.Id);
                sqlCommand.Parameters.AddWithValue("P2", book.Title);
                sqlCommand.Parameters.AddWithValue("P3", book.ISBN);
                sqlCommand.Parameters.AddWithValue("P4", book.Year);

                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Update(Book book)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "UPDATE [dbo].[Book] SET Title = @P2, ISBN = @P3, Year = @P4 where Id = @P1";
                sqlCommand.Parameters.AddWithValue("P1", book.Id);
                sqlCommand.Parameters.AddWithValue("P2", book.Title);
                sqlCommand.Parameters.AddWithValue("P3", book.ISBN);
                sqlCommand.Parameters.AddWithValue("P4", book.Year);

                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(Guid id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "DELETE FROM [dbo].[Book] Where Id = @P1";
                sqlCommand.Parameters.AddWithValue("P1", id);

                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
