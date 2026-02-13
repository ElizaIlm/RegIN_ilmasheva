using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static RegIN_Ilmashevaa.Pages.Confirmation;

namespace RegIN_Ilmashevaa.Pages
{
    /// <summary>
    /// Логика взаимодействия для Confirmation.xaml
    /// </summary>
    public partial class Confirmation : Page
    {
        
        public Confirmation(TypeConfirmaton TypeConfirmaton)
        {
            InitializeComponent();
            ThisTypeConfirmaton = TypeConfirmaton;
            SendMailCode();
        }
        public enum TypeConfirmaton
        {
            Login, Regin
               
        }
        TypeConfirmaton ThisTypeConfirmaton;
        public int Code = 0;

        public void SendMailCode()
        {
            Code = new Random().Next(100000, 999999);
            Classes.SendMail.SendMessage($"Login code: {Code}", MainWindow.mainWindow.UserLogIn.Login);
           
            Thread TSendMailCode = new Thread(TimerSendMailCode);
            TSendMailCode.Start();
        }
        public void TimerSendMailCode()
        {
            for (int i = 0; i < 60; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    LTimer.Content = $"A second message can be sent after {(60 - i)} seconds";
                });
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(() =>
            {
                // Включаем кнопку отправить повторно
                BSendMessage.IsEnabled = true;
                // Изменяем данные на текстовом поле
                LTimer.Content = "";
            });
        }
        private void SendMail(object sender, RoutedEventArgs e)
        {
            SendMailCode();
        }
        private void SetCode(object sender, KeyEventArgs e)
        {
            // Если текст введённый в поле 6 символов
            if (TbCode.Text.Length == 6)
            {
                // Вызываем метод проверки кода
                SetCode();
            }
        }

        private void SetCode(object sender, System.Windows.RoutedEventArgs e) =>
            SetCode();
        void SetCode()
        {
            if (TbCode.Text == Code.ToString() && TbCode.IsEnabled == true)
            {
                TbCode.IsEnabled = false;
                if (ThisTypeConfirmaton == TypeConfirmaton.Login)
                {
                    // Выводим сообщение о том что пользователь авторизовался
                    MessageBox.Show("Авторизация пользователя успешно подтверждена.");
                }
                else
                {
                    // Если тип подтверждения является регистрацией
                    MainWindow.mainWindow.UserLogIn.SetUser();
                    // Выводим сообщение о том что пользователь зарегистрировался
                    MessageBox.Show("Регистрация пользователя успешно подтверждена.");
                }
            }
        }
        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}
}
