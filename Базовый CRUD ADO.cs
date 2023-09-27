using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FirstDbApp
{
    internal class Program
    {
        // ЗАДАЧА: 
        // реализовать базовые CRUD-операции для таблицы БД
        // в виде процедур (статических методов класса Program)
        // CRUD - create read update delete
        // - добавить новую запись в таблицу    +
        // - получить все записи таблицы        +
        // - получить запись таблицы по id      +
        // - изменить запись
        // - удалить запись

        // 1. вспомогательная процедура, создающая и открывающая подключение к БД
        static SqlConnection OpenDbConnection()
        {
            // обработка исключений будет выполняться выше по стеку
            string connectionString = @"Data Source=LAPTOP-SCD1CHJF\SQLEXPRESS;
                                            Initial Catalog=computer_games_db;
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
            bool noRows = true;
            while (queryResult.Read())
            {
                noRows = false;
                for (int i = 0; i < queryResult.FieldCount - 1; i++)
                {
                    Console.Write($"{queryResult[i]} - ");
                }
                Console.WriteLine(queryResult[queryResult.FieldCount - 1]);
            }
            if (noRows)
            {
                Console.WriteLine("No rows in result");
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
            } catch (Exception ex) {
                Console.WriteLine($"Something wrong: {ex.Message}");
            } finally
            {
                connection?.Close();    // закрыть соединение (если != null)
                queryResult?.Close();
            }
        }

        // 4. процедура получения записи по id
        static void SelectRowById(int id)
        {
            SqlConnection connection = null;
            SqlDataReader queryResult = null;
            try
            {
                // 1. открыть соединение
                connection = OpenDbConnection();
                // 2. подготовить запрос
                SqlCommand query = new SqlCommand($"SELECT * FROM game_t WHERE id = {id}", connection);
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
                // 2. подготовить запрос
                string cmdString = 
                    $"INSERT INTO game_t (name_f, released_in_f, price_f) VALUES ('{name}', {released_in}, {price});";
                SqlCommand cmd = new SqlCommand(cmdString, connection);
                // 3. выполнить запрос
                int rowsAffected = cmd.ExecuteNonQuery();   // выполнение запроса, изменяющего строки таблицы
                // 4. проверить результат выполнения
                if (rowsAffected != 1)
                {
                    Console.WriteLine($"INSERT failed, rowsAffected != 1 ({rowsAffected})");
                } else
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

        // 6. процедура удаления записи
        static void DeleteRow(int id)
        {
            SqlConnection connection = null;
            try
            {
                // 1. открыть соединение
                connection = OpenDbConnection();
                // 2. подготовить запрос
                string cmdString =
                    $"DELETE FROM game_t WHERE id = {id}";
                SqlCommand cmd = new SqlCommand(cmdString, connection);
                // 3. выполнить запрос
                int rowsAffected = cmd.ExecuteNonQuery();   // выполнение запроса, изменяющего строки таблицы
                // 4. проверить результат выполнения
                switch (rowsAffected)
                {
                    case 0:
                        Console.WriteLine("No rows matching for delte");
                        break;
                    case 1:
                        Console.WriteLine("Row removed");
                        break;
                    default:
                        Console.WriteLine("DELETE failed, rowsAffected != 0 || rowsAffected != 1");
                        break;
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

        // 7. процедура обновления записи
        static void UpdateRow(int id, string newName, int newReleasedIn, decimal newPrice)
        {
            
        }

        //static void Main(string[] args)
        //{
        //    InsertRow("Left4Dead2", 2012, 2000);
        //    SelectAllRows();
        //}
    }
}
