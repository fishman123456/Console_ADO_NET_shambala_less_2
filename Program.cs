using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
   
    internal class Program
    {
        // 1. вспомогательная процедура, создающая и открывающая подключение к БД
        static SqlConnection OpenDbConnection()
        {
            // обработка исключений будет выполняться выше по стеку
            string connectionString = @"Data Source=fishman\SQLEXPRESS;
                                    Initial Catalog=computer_game_db;
                                    Integrated Security=SSPI;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        // 2. вспомогательная процедура, читающая и выводящая табличный результат запроса (SqlDatareader)
        static void ReadQueryResult(SqlDataReader queryResult)
        {
            // 1. вывести названия столбцов результирующей таблицы (представления)
            for (int i = 0; i < queryResult.FieldCount - 1; i++)
            {
                Console.Write($"{queryResult.GetName(i)} - ");
            }
            Console.WriteLine(queryResult.GetName(queryResult.FieldCount - 1));
            // 2. вывести значения построчно
            while (queryResult.Read())
            {
                for (int i = 0; i < queryResult.FieldCount - 1; i++)
                {
                    Console.Write($"{queryResult[i]} - ");
                }
                Console.WriteLine(queryResult[queryResult.FieldCount - 1]);
            }
        }

        // 3. процедура получения всех записей таблицы
        static void SelectAllRows()
        {
            SqlConnection connection = null;
            SqlDataReader queryResult = null;
            try
            {
                // 1. открыть соединение
                connection = OpenDbConnection();
                // 2. подготовить запрос
                SqlCommand query = new SqlCommand("SELECT * FROM game_t", connection);
                // 3. выполнить запрос с табличным результом
                queryResult = query.ExecuteReader();
                // 4. считать запрос (универсальный способ)
                ReadQueryResult(queryResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something wrong: {ex.Message}");
            }
            finally
            {
                connection?.Close();    // закрыть соединение (если != null)
                queryResult?.Close();
            }
        }
        // 5. процедура добавления новой записи в таблицу
        static void InsertRow(string name, int released_in, decimal price)
        {
            SqlConnection connection = null;
            try
            {
                // 1. открыть соединение
                connection = OpenDbConnection();
                // 2. подготовить запрос [name_f] ,[released_in_f]   ,[prise_f]
                string cmdString =
                    $"INSERT INTO game_t (name_f, released_in_f, price_f) VALUES ('{name}', {released_in}, {price});";
                SqlCommand cmd = new SqlCommand(cmdString, connection);
                // 3. выполнить запрос
                int rowsAffected = cmd.ExecuteNonQuery();   // выполнение запроса, изменяющего строки таблицы
                                                            // 4. проверить результат выполнения
                if (rowsAffected != 1)
                {
                    Console.WriteLine($"INSERT failed, rowsAffected != 1 ({rowsAffected})");
                }
                else
                {
                    Console.WriteLine("Successfully inserted");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something wrong: {ex.Message}");
            }
            finally
            {
                connection?.Close();    // закрыть соединение (если != null)
            }
        }

        // 5. процедура удаления записи из таблицы
        static void DeleteRow(int id)
        {
            SqlConnection connection = null;
            try
            {
                // 1. открыть соединение
                connection = OpenDbConnection();
                // 2. подготовить запрос
                string cmdString =
                    $"DELETE from game_t where id ={id};";
                SqlCommand cmd = new SqlCommand(cmdString, connection);
                // 3. выполнить запрос
                int rowsAffected = cmd.ExecuteNonQuery();   // выполнение запроса, изменяющего строки таблицы
                                                            // 4. проверить результат выполнения
                if (rowsAffected != 1)
                {
                    Console.WriteLine($"DELETE failed, rowsAffected != 1 ({rowsAffected})");
                }
                else
                {
                    Console.WriteLine("Successfully deleted");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something wrong: {ex.Message}");
            }
            finally
            {
                connection?.Close();    // закрыть соединение (если != null)
            }
        }
        static void Main(string[] arg)
        {
           // InsertRow("uop", 2015, 600);
           DeleteRow(6);
            SelectAllRows();

        }
    }
}
