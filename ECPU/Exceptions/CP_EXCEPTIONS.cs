using System;
using System.Windows;

namespace ECPU.Exceptions
{
   public class CriticalFileNotFoundException : Exception
    {      

        public CriticalFileNotFoundException(string file) : base("Файл " + file + " не найден!")
        {

        }
    }

    public class RegistryException : Exception
    {

        public RegistryException(string file) : base("Не удалось записать в реестр по пути " + file)
        {

        }
    }
    public class LoadTableException : Exception
    {

        public LoadTableException(string table) : base("Не удалось загрузить таблицу " + table + " из БД!")
        {

        }
    }
    public class ApplicationNotFoundException : Exception
    {

        public ApplicationNotFoundException(string app) : base("Приложение " + app + " не найдено!")
        {

        }
    }

    public class INIDataNotFoundException : Exception
    {

        public INIDataNotFoundException(string key, string section, string file) : base("Данные для :" + " "+ section + " - "+ key + " в файле "+ file+ " не найдены. Произведите сброс настроек." )
        {
            MessageBox.Show(Message);
            Logger.addLine(false, Message);
            Logger.addLine(true, "Завершение приложения");
            Environment.Exit(1);
        }
    }
    public class TypeOfWindowItemNotFoundException : Exception
    {

        public TypeOfWindowItemNotFoundException(string element) : base("Тип элемента :" + element + " не предусмотрен.")
        {
            MessageBox.Show(Message);
            Logger.addLine(false, Message);
            Logger.addLine(true, "Завершение приложения");
            Environment.Exit(1);
        }
    }

    public class GameTypeInitializationException : Exception
    {

        public GameTypeInitializationException() : base("Не установлен вид игры!")
        {
            MessageBox.Show(Message);
            Logger.addLine(false, Message);
            Logger.addLine(true, "Завершение приложения");
            Environment.Exit(1);
        }
    }

    public class DownloadDBUpdateFileException : Exception
    {
        public DownloadDBUpdateFileException() : base("Не удалось скачать файл для проверки обновления базы данных!")
        {
            MessageBox.Show(Message);
            Logger.addLine(false, Message);
           
        }
    }

    public class NoInternetConnectionException : Exception
    {
        public NoInternetConnectionException() : base("Нет соединения с интернетами или сервер с обновлениями не доступен." + Environment.NewLine + "Попробуйте снова" )
        {
            MessageBox.Show(Message);
            Logger.addLine(false, "Нет соединения с интернетами или сервер с обновлениями не доступен.");

        }
    }

    public class oldNETFrameworkVersionException : Exception
    {
        public oldNETFrameworkVersionException() : base("Версия .NET FRAMEWORK, установленная на компьютере ниже требуемой." + Environment.NewLine + "Установите версию 4.6.1 или более позднюю")
        {
            MessageBox.Show(Message);
            Logger.addLine(false, "Версия .NET FRAMEWORK, установленная на компьютере ниже требуемой");
            Logger.addLine(true, "Завершение приложения");
            Environment.Exit(1);

        }
    }
 


    public class downloadArchiveUpdateException : Exception
    {
        public downloadArchiveUpdateException(string updatename) : base("Не удалось скачать архив для обновления: " + updatename)
        {
            
            Logger.addLine(false, "Не удалось скачать архив для обновления: " + Message); 
        }
    }
    public class unpackArchiveUpdateException : Exception
    {
        public unpackArchiveUpdateException(string updatename) : base("Не удалось распаковать архив для обновления: " + updatename)
        {
           
            Logger.addLine(false, "Не удалось распаковать архив для обновления: " + Message);
        }
    }

    public class gameBuildInvalidVersionForUpdateException : Exception
    {
        public gameBuildInvalidVersionForUpdateException(float actualVersion, float invalidUserVersion) : base("Обновление предназначено для сборки версии " + actualVersion.ToString() + Environment.NewLine + "У Вас сборка версии " + invalidUserVersion.ToString())
        {           
            Logger.addLine(false, "Ошибка обновления: " + Message);
        }
    }

}
