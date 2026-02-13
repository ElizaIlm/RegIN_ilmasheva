using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Imaging = Aspose.Imaging;
using Brushes = System.Windows.Media.Brushes;
namespace RegIN_Ilmashevaa.Pages
{
    /// <summary>
    /// Логика взаимодействия для Regin.xaml
    /// </summary>
    public partial class Regin : Page
    {
        OpenFileDialog FileDialogImage = new OpenFileDialog();
        /// <summary>
        /// Проверка на ввод логина
        /// </summary>
        bool BCorrectLogin = false;
        /// <summary>
        /// Проверка на ввод пароля
        /// </summary>
        bool BCorrectPassword = false;
        /// <summary>
        /// Проверка на подтверждение пароля
        /// </summary>
        bool BCorrectConfirmPassword = false;
        /// <summary>
        /// Проверка на указания изображения
        /// </summary>
        bool BSetImages = false;
        public Regin()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogIn.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += InCorrectLogin;
            FileDialogImage.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg";
            FileDialogImage.RestoreDirectory = true;
            // Указываем название диалогового окна
            FileDialogImage.Title = "Choose a photo for your avatar";
        }
        private void CorrectLogin()
        {
            // Выводим сообщения красным цветом, о том что такой логин уже используется
            SetNotification("Login already in use", Brushes.Red);
            // Отключаем проверку на ввод логина
            BCorrectLogin = false;
        }
        private void InCorrectLogin() =>
            SetNotification("", Brushes.Black);
        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Вызываем метод ввода логина
                SetLogin();
            }
        }
        private void SetLogin(object sender, System.Windows.RoutedEventArgs e) =>
        // Вызываем метод ввода логина
        SetLogin();
        public void SetLogin()
        {
            // Регулярное выражение для почты
            Regex regex = new Regex(@"\([a-zA-Z0-9._-]{4,}@[a-zA-Z0-9._-]{2,}\.[a-zA-Z0-9._-]{2,}\)");
            // Введён ли логин зависит от того регулярного выражения
            BCorrectLogin = regex.IsMatch(TbLogin.Text);
            // Если регулярное выражение
            if (regex.IsMatch(TbLogin.Text) == true)
            {
                // Выводим пустое уведомление чёрным цветом
                SetNotification("", Brushes.Black);
                // Вызываем получение данных пользователя по логину
                MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);
            }
            else
                // Если введён логин не удовлетворяющий регулярное выражение, выводим сообщение красным цветом
                SetNotification("Invalid login", Brushes.Red);

            // Вызываем метод авторизации
            OnRegin();

        }
        #region SetPassword
        /// <summary>
        /// Метод ввода пароля
        /// </summary>
        private void SetPassword(object sender, System.Windows.RoutedEventArgs e) =>
            // Вызываем метод ввода пароля
            SetPassword();

        /// <summary>
        /// Метод ввода пароля
        /// </summary>
        private void SetPassword(object sender, KeyEventArgs e)
        {
            // Если нажата клавиша Enter
            if (e.Key == Key.Enter)
            {
                // Вызываем метод ввода пароля
                SetPassword();
            }
        }
        public void SetPassword()
        {
            // Регулярное выражение
            Regex regex = new Regex(@"(?=.*[0-9])(?=.*[!@#$%^&?*\-_=])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&?*\-_=]{10,}");

            BCorrectPassword = regex.IsMatch(TbPassword.Password);
            if (regex.IsMatch(TbPassword.Password) == true)
            {
                SetNotification("", Brushes.Black);
                if (TbConfirmPassword.Password.Length > 0)
                {
                    ConfirmPassword(true);
                }
                OnRegin();
            }
            else
            {
                SetNotification("Invalid password", Brushes.Red);
            }
        }

        #endregion
        #region SetConfirmPassword
        private void ConfirmPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ConfirmPassword();
        }

        /// <summary>
        /// Метод повторного ввода пароля
        /// </summary>
        private void ConfirmPassword(object sender, System.Windows.RoutedEventArgs e) =>
            // Вызываем метод повторения пароля
            ConfirmPassword();

        // <summary>
        // Метод повторного ввода пароля
        // </summary>
        public void ConfirmPassword(bool Pass = false)
        {
            // Записываем результат сравнения паролей в переменную
            BCorrectConfirmPassword = TbConfirmPassword.Password == TbPassword.Password;

            // Если пароль не совпадает с повторением пароля
            if (TbConfirmPassword.Password != TbPassword.Password)
            {
                // Выводим сообщение о том, что пароли не совпадают, красным цветом
                SetNotification("Passwords do not match", Brushes.Red);
            }
            else
            {
                // Если пароли совпадают, выводим пустое сообщение чёрным цветом
                SetNotification("", Brushes.Black);

                // Если проверка идёт не из метода проверки пароля
                // Исключаем зацикливание методов
                if (!Pass)
                {
                    // Вызываем проверку пароля
                    SetPassword();
                }
            }
        }

        #endregion
        void OnRegin()
        {
            // Ссылка на ведён
            if (!BCorrectLogin)
                // Останавливаем регистрацию
                return;

            // Если наименование не введено
            if (TbName.Text.Length == 0)
                // Останавливаем регистрацию
                return;

            // Если не введён пароль
            if (!BCorrectPassword)
                // Останавливаем регистрацию
                return;

            // Если пароль не подтверждён
            if (!BCorrectConfirmPassword)
                // Останавливаем регистрацию
                return;

            // Указываем пользовательский логин
            MainWindow.mainWindow.UserLogIn.Login = TbLogin.Text;

            // Указываем пользовательский пароль
            MainWindow.mainWindow.UserLogIn.Password = TbPassword.Password;

            // Указываем пользовательское имя
            MainWindow.mainWindow.UserLogIn.Name = TbName.Text;

            // Если указано изображение
            if (BSetImages)
                // Разбиваем изображение на байты
                MainWindow.mainWindow.UserLogIn.Image = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\IUser.jpg");

            // Указываем дату обновления
            MainWindow.mainWindow.UserLogIn.DateUpdate = DateTime.Now;

            // Указываем дату создания
            MainWindow.mainWindow.UserLogIn.DateCreate = DateTime.Now;

            // Открываем страницу подтверждения через почту
            MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmaton.Regin));
        }
        private void SetName(object sender, TextCompositionEventArgs e)
        {
            // Проверяем что символ относится к категории букв
            e.Handled = !(Char.IsLetter(e.Text, 0));
        }
        public void SetNotification(string message, Brush color)
        {
            LNameUser.Content = message;
            LNameUser.Foreground = color;
        }
        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            // Если статус открывшегося диалогового окна true
            if (FileDialogImage.ShowDialog() == true)
            {
                // конвертируем размер изображения
                using (Imaging.Image image = Imaging.Image.Load(FileDialogImage.FileName))
                {
                    // создаём ширину изображения
                    int NewWidth = 0;
                    // Создаём высоту изображения
                    int NewHeight = 0;
                    // проверяем какая из сторон больше
                    if (image.Width > image.Height)
                    {
                        // Расчитываем новую ширину относительно высоты
                        NewWidth = (int)(image.Width * (256f / image.Height));
                        // Задаём высоту изображения
                        NewHeight = 256;
                    }
                    else
                    {
                        NewWidth = 256;
                        NewHeight = (int)(image.Height * (256f / image.Width));
                    }
                    image.Resize(NewWidth, NewHeight);
                    image.Save("defau.jpg");
                }
                using (Imaging.RasterImage rasterImage = (Imaging.RasterImage)Imaging.Image.Load("IUser.jpg"))
                {
                    if (!rasterImage.IsCached)
                    {
                        rasterImage.CacheData();
                    }
                    int X = 0;
                    int Width = 256;
                    int Y = 0;
                    int Height = 256;
                    if (rasterImage.Width > rasterImage.Height)
                    {
                        X = (int)((rasterImage.Width - 256f) / 2);
                    }
                    else
                    {
                        Y = (int)((rasterImage.Height - 256f) / 2);
                        Imaging.Rectangle rectangle = new Imaging.Rectangle(X, Y, Width, Height);
                        rasterImage.Crop(rectangle);
                        rasterImage.Save("IUser.jpg");
                    }
                }
                DoubleAnimation StartAnimation = new DoubleAnimation();
                StartAnimation.From = 1;
                StartAnimation.To = 0;
                StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
                StartAnimation.Completed += delegate
                {
                    IUser.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\IUser.jpg"));
                    // Создаём анимацию конца
                    DoubleAnimation EndAnimation = new DoubleAnimation();
                    EndAnimation.From = 0;
                    EndAnimation.To = 1;
                    EndAnimation.Duration = TimeSpan.FromSeconds(1.2);
                    IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
                };
                IUser.BeginAnimation(Image.OpacityProperty, StartAnimation);
                BSetImages = true;
            }
            else
                BSetImages = false;
        }
        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}
