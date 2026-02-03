using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RegIN_Ilmashevaa.Classes
{
    public class SendMail
    {
        /// <summary>
        /// Отправка сообщения по электронной почте
        /// </summary>
        /// <param name="Message">Текст сообщения</param>
        /// <param name="To">Адрес получателя</param>
        public static void SendMessage(string Message, string To)
        {
            // Создаём SMTP клиент, в качестве хоста указываем адрес
            var smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                // Указываем порт по которому передаём сообщение
                Port = 587,
                // Указываем почту, с которой будет отправляться сообщение, и пароль от этой почты
                Credentials = new NetworkCredential("Elik2908@yandex.ru", "kpfvmklvsgxhlfsi"),
                // Включаем поддержку SSL
                EnableSsl = true,
            };

            // Вызываем метод Send, который отправляет письмо на указанный адрес
            smtpClient.Send("Elik2908@yandex.ru", To, "Проект RegIn", Message);
        }
    }
}
