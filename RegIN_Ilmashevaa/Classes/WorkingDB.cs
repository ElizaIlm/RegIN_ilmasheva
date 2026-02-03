using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace RegIN_Ilmashevaa.Classes
{
    public class WorkingDB
    {
        readonly static string connection = "server=localhost;port=3306;database=regin;user=root;pwd=root;";

        /// <summary>
        /// Создание и открытие подключения
        /// </summary>
        /// <returns>Открытое подключение или null</returns>
        public static MySqlConnection OpenConnection()
        {
            try
            {
                // Создаём подключение MySql
                MySqlConnection mySqlConnection = new MySqlConnection(connection);
                // Открываем подключение
                mySqlConnection.Open();
                // Возвращаем открытое подключение
                return mySqlConnection;
            }
            catch (Exception exp)
            {
                // В случае ошибки, выводим сообщение в Debug
                Debug.WriteLine(exp.Message);
                // Возвращаем нулевое значение
                return null;
            }
        }
        /// <summary>
        /// Выполнение SQL-запроса и получение результата в виде DataReader
        /// </summary>
        /// <param name="Sql">SQL-запрос</param>
        /// <param name="mySqlConnection">Открытое подключение к базе данных</param>
        /// <returns>Объект MySqlDataReader с результатами запроса</returns>
        public static MySqlDataReader Query(string Sql, MySqlConnection mySqlConnection)
        {
            // Создаём команду для выполнения SQL-запроса
            MySqlCommand mySqlCommand = new MySqlCommand(Sql, mySqlConnection);
            // Возвращаем результат выполненной команды
            return mySqlCommand.ExecuteReader();
        }

        /// <summary>
        /// Функция закрытия соединения с базой данных
        /// </summary>
        /// <param name="mySqlConnection">Открытое MySQL соединение</param>
        public static void CloseConnection(MySqlConnection mySqlConnection)
        {
            // Закрываем подключение с базой данных
            mySqlConnection.Close();
            // Очищаем Pool соединения, для того чтобы оно окончательно закрылось, а не ушло в пул
            MySqlConnection.ClearPool(mySqlConnection);
        }
        /// <summary>
        /// Проверка состояния подключения к базе данных
        /// </summary>
        /// <param name="mySqlConnection">Подключение для проверки</param>
        /// <returns>True, если подключение существует и открыто, иначе False</returns>
        public static bool OpenConnection(MySqlConnection mySqlConnection)
        {
            // Проверяем, если соединение не является нулевым
            // И статус соединения Open
            // В таком случае возвращается TRUE, в противоположном FALSE
            return mySqlConnection != null && mySqlConnection.State == System.Data.ConnectionState.Open;
        }
    }
}
