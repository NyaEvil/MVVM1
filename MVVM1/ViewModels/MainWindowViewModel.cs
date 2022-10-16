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

namespace MVVM1.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
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



        public ICommand UpdateButton { get; }
        private bool UpdateCanExecute(object o) => true;
        private void UpdateOnExecuted(object o)
        {
            SqlConnection connection = new SqlConnection("Data Source=EVILCITADELPE\\NYAEVILSQL;Initial Catalog=MVVM;Integrated Security=True");
            connection.Open();
            SqlCommand com = new SqlCommand("SELECT 'Дата-время начала (план)', '№ задания', '№ ПЗ', 'Толщина (план)', 'Ширина (план)', 'Вес (план)', 'Длина (ПЗ)', 'Длина (по разметке)' FROM PracticeInfokom", connection);
            SqlDataReader reader = com.ExecuteReader();
            connection.Close();
            connection.Dispose();
            DataTable dt = new DataTable();
            dt.Load(reader);
        }

        public MainWindowViewModel()
        {
            UpdateButton = new LyambdaCommand(UpdateOnExecuted, UpdateCanExecute);
        }

    }
}
