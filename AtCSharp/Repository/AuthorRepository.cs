using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApp.Models;

namespace Api.Repository
{
    public class AuthorRepository
    {
        private string ConnectionString { get; set; }

        public AuthorRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("AtConnectionString");
        }

        public List<Author> GetAuthors()
        {
            List<Author> result = new List<Author>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM [dbo].[Author]";
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Author person = new Author(
                        Guid.Parse(reader["Id"].ToString()),
                        reader["FirstName"].ToString(),
                        reader["LastName"].ToString(),
                        reader["Email"].ToString(),
                        DateTime.Parse(reader["Birthday"].ToString())
                    );
                    result.Add(person);
                }

                connection.Close();
                return result;
            }
        }

        public Author GetAuthorById(string id)
        {
            List<Author> result = new List<Author>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM [dbo].[Author] WHERE Id = @P1";
                sqlCommand.Parameters.AddWithValue("P1", id);
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Author person = new Author(
                        Guid.Parse(reader["Id"].ToString()),
                        reader["FirstName"].ToString(),
                        reader["LastName"].ToString(),
                        reader["Email"].ToString(),
                        DateTime.Parse(reader["Birthday"].ToString())
                    );
                    result.Add(person);
                }

                connection.Close();
                if (result.Count > 0) return result[0];
                else return null;
            }
        }

        public void Save(Author author)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "INSERT INTO [dbo].[Author](Id, FirstName, LastName, Email, Birthday) VALUES (@P1, @P2, @P3, @P4, @P5)";
                sqlCommand.Parameters.AddWithValue("P1", author.Id);
                sqlCommand.Parameters.AddWithValue("P2", author.FirstName);
                sqlCommand.Parameters.AddWithValue("P3", author.LastName);
                sqlCommand.Parameters.AddWithValue("P4", author.Email);
                sqlCommand.Parameters.AddWithValue("P5", author.Birthday);

                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Update(Author author)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open) connection.Open();

                SqlCommand sqlCommand = connection.CreateCommand();
                sqlCommand.CommandText = "UPDATE [dbo].[Author] SET FirstName = @P2, LastName = @P3, Email = @P4, Birthday = @P5 where Id = @P1";
                sqlCommand.Parameters.AddWithValue("P1", author.Id);
                sqlCommand.Parameters.AddWithValue("P2", author.FirstName);
                sqlCommand.Parameters.AddWithValue("P3", author.LastName);
                sqlCommand.Parameters.AddWithValue("P4", author.Email);
                sqlCommand.Parameters.AddWithValue("P5", author.Birthday);

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
                sqlCommand.CommandText = "DELETE FROM [dbo].[Author] Where Id = @P1";
                sqlCommand.Parameters.AddWithValue("P1", id);

                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
