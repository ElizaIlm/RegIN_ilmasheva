using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegIN_Ilmashevaa.Classes
{
    public class User
    {
       
            /// <summary>
            /// Идентификатор пользователя
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Логин пользователя
            /// </summary>
            public string Login { get; set; }

            /// <summary>
            /// Пароль пользователя
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Имя пользователя
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Изображение пользователя
            /// </summary>
            public byte[] Image { get; set; } = new byte[0];

            /// <summary>
            /// Дата и время обновления пользователя
            /// </summary>
            public DateTime DateTimeUpdate { get; set; }

            /// <summary>
            /// Дата и время создания пользователя
            /// </summary>
            public DateTime DateTimeCreate { get; set; }

            /// <summary>
            /// Событие успешной авторизации
            /// </summary>
            public CorrectLogin HandlerCorrectLogin;

            /// <summary>
            /// Событие неуспешной авторизации
            /// </summary>
            public InCorrectLogin HandlerInCorrectLogin;
            public delegate void CorrectLogin();
            public delegate void InCorrectLogin();
            public void GetUserLogin(string Login)
            {
                this.Id = -1;
                this.Login = string.Empty;
                this.Password = string.Empty;
                this.Name = string.Empty;
                this.Image = new byte[0];

                // Открываем соединение с базой данных
                MySqlConnection mySqlConnection = WorkingDB.OpenConnection();

                // Если соединение с базой данных успешно открыто
                if (WorkingDB.OpenConnection(mySqlConnection))
                {
                    // Выполняем запрос получения пользователя по логину
                    MySqlDataReader userQuery = WorkingDB.Query($"SELECT * FROM `users` WHERE `Login` = '{Login}'", mySqlConnection);

                    // Проверяем, что существуют данные для чтения
                    if (userQuery.HasRows)
                    {
                        // Читаем пришедшие данные
                        userQuery.Read();

                        // Записываем код пользователя
                        this.Id = userQuery.GetInt32(0);
                        // Записываем логин пользователя
                        this.Login = userQuery.GetString(1);
                        // Записываем пароль пользователя
                        this.Password = userQuery.GetString(2);
                        // Записываем имя пользователя
                        this.Name = userQuery.GetString(3);

                        // Проверяем, что изображение установлено
                        if (!userQuery.IsDBNull(4))
                        {
                            // Получаем длину изображения
                            long imageLength = userQuery.GetBytes(4, 0, null, 0, 0);
                            // Задаём размер массива
                            this.Image = new byte[imageLength];
                            // Записываем изображение пользователя
                            userQuery.GetBytes(4, 0, Image, 0, (int)imageLength);
                        }
                        this.DateTimeUpdate = userQuery.GetDateTime(5);
                        // Записываем дату создания
                        this.DateTimeCreate = userQuery.GetDateTime(6);
                        HandlerCorrectLogin.Invoke();
                    }
                    else
                        HandlerCorrectLogin.Invoke();
                }
                else
                    HandlerCorrectLogin.Invoke();
                WorkingDB.CloseConnection(mySqlConnection);



            }
        public void CrateNewPassword()
        {
            // Если наш логин не равен пустому значению
            // А это значит что наш пользователь существует
            if (Login != String.Empty)
            {
                // Вызываем функцию генерации пароля
                Password = GeneratePass();
                // Открываем подключение к базе данных
                MySqlConnection mySqlConnection = WorkingDB.OpenConnection();
                // Проверяем что подключение действительно открыто
                if (WorkingDB.OpenConnection(mySqlConnection))
                {
                    // Выполняем запрос, обновляя пароль у выбранного пользователя
                    WorkingDB.Query($"UPDATE `users` SET `Password`='{this.Password}' WHERE `Login` = '{this.Login}'", mySqlConnection);
                }
                // Закрываем подключение к базе данных
                WorkingDB.CloseConnection(mySqlConnection);
                // Отправляем сообщение на почту, о том что пароль изменён
                SendMail.SendMessage($"Your account password has been changed.\nNew password: {this.Password}", this.Login);
            }
        }
        public string GeneratePass()
        {
            // Создаём коллекцию, составляющую из символов
            List<char> NewPassword = new List<char>();

            // Инициализируем рандом, которая будет случайно выбирать символы
            Random rnd = new Random();

            // Символы нумерации
            char[] ArrNumbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            // Символы знаков
            char[] ArrSymbols = { '|', '-', '_', '!', '@', '#', '$', '%', '&', '*', '=', '+' };

            // Символы английской раскладки
            char[] ArrUppercase = { 'q', 'w', 'e', 'r', 't', 's', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm' };

            // Выбираем 1 случайную цифру
            for (int i = 0; i < 1; i++)
            {
                // Добавляем цифру в коллекцию
                NewPassword.Add(ArrNumbers[rnd.Next(0, ArrNumbers.Length)]);
            }

            // Выбираем 1 случайный символ
            for (int i = 0; i < 1; i++)
            {
                // Добавляем символ в коллекцию
                NewPassword.Add(ArrSymbols[rnd.Next(0, ArrSymbols.Length)]);
            }

            // Выбираем 2 случайные буквы английской раскладки верхнего регистра
            for (int i = 0; i < 2; i++)
            {
                // Добавляем букву английской раскладки в коллекцию
                NewPassword.Add(char.ToUpper(ArrUppercase[rnd.Next(0, ArrUppercase.Length)]));
            }

            // Выбираем 6 случайные буквы английской раскладки нижнего регистра
            for (int i = 0; i < 6; i++)
            {
                // Добавляем букву английской раскладки в коллекцию
                NewPassword.Add(ArrUppercase[rnd.Next(0, ArrUppercase.Length)]);
            }
            // Тем самым, перемешиваем коллекцию символов
            for (int i = 0; i < NewPassword.Count; i++)
            {
                // Выбираем случайный символ
                int RandomSymbol = rnd.Next(0, NewPassword.Count);
                // Запоминаем случайный символ
                char Symbol = NewPassword[RandomSymbol];
                // Меняем случайный символ на порядковый символ в коллекции
                NewPassword[RandomSymbol] = NewPassword[i];
                // Меняем порядковый символ в коллекции на случайный
                NewPassword[i] = Symbol;
            }

            // Объявляем переменную, которая будет содержать пароль
            string NPASSWORD = "";
            // Перебираем коллекцию
            for (int i = 0; i < NewPassword.Count; i++)
                // Добавляем в переменную с паролем символ из коллекции
                NPASSWORD += NewPassword[i];

            // Возвращаем пароль
            return NPASSWORD;
        }
    }
}
