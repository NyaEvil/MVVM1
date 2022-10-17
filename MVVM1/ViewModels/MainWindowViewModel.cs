using MVVM1.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Input;
using MVVM1.Infrastructure.Commands;
using System.ComponentModel;
using System.Runtime;
using System.Windows.Documents;

namespace MVVM1.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Button Update

        //Свойства для обозначения сущностей объектов
        private DataTable dataslab;

        public DataTable DataSlab
        {
            get => dataslab;
            set
            {
                dataslab = value;
                OnPropertyChange();
            }
        }

        private DataTable datacrat;

        public DataTable DataCrat
        {
            get => datacrat;
            set
            {
                datacrat = value;
                OnPropertyChange();
            }
        }

        //свойства ддля привязки DateTime
        private string datebegin = DateTime.Now.ToString();

        public string DateBegin
        {
            get => datebegin;
            set
            {
                datebegin = value;
            }
        }

        private string dateend = DateTime.Now.ToString();

        public string DateEnd
        {
            get => dateend;
            set
            {
                dateend = value;
            }
        }

#region Кнопка "Обновить"
        public ICommand UpdateButton { get; }
        private bool UpdateCanExecute(object o) => true;
        private void UpdateOnExecuted(object o)
        {
        //Получение даты и времени с интерфейса и конвертирование в DateTime
            DateTime upd_begin;
            DateTime.TryParse(DateBegin, out upd_begin);
            DateTime upd_end;
            DateTime.TryParse(DateEnd, out upd_end);
        //Открытие подключения к БД и запрос нужных полей с условием даты и времени
            SqlConnection connection = new SqlConnection("Data Source=NYAEVIL-PC\\SQLEXPRESS;Initial Catalog=PracticeInf;Integrated Security=True");
            connection.Open();
            SqlCommand com = new SqlCommand($"SELECT \"Дата-время начала (план)\", \"№ задания\", \"№ ПЗ\", \"Толщина (план)\", \"Ширина (план)\", \"Вес (план)\", \"Длина (ПЗ)\", \"Длина (по разметке)\" FROM Table1 WHERE \"Дата-время начала (план)\" >= '{upd_begin}' AND \"Дата-время начала (план)\" <='{upd_end}'", connection);
            SqlDataReader reader = com.ExecuteReader();
        //Создание DataTable и загрузка в нее данных из запроса, после чего связка ее со свойством DataSlab
            DataTable dt = new DataTable();
            dt.Load(reader);
            DataSlab = dt;
        //Аналогичные действия для таблицы кратов
            com = new SqlCommand($"select \"№ ПЗ\", \"Длина (по разметке)\", \"Ширина (план)\", \"Вес (план)\", Round(\"Длина (Пз)\"/TRY_CAST(\"Длина (по разметке)\" AS FLOAT),0) AS \"Количество\" FROM Table1 as P WHERE \"Дата-время начала (план)\" >= '{upd_begin}' AND \"Дата-время начала (план)\" <='{upd_end}'", connection);
            reader = com.ExecuteReader();
            dt = new DataTable();
            dt.Load(reader);
            DataCrat = dt;
        //Закрытие и удаление подключения и связанных ресурсов
            connection.Close();
            connection.Dispose();
        }
        #endregion Кнопка "Обновить"
        #endregion Button Update

        public MainWindowViewModel()
        {
            UpdateButton = new LyambdaCommand(UpdateOnExecuted, UpdateCanExecute);
        }

    }
}
