using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

//Перед использованием внести актуальный путь к mdf файлу в App.config/...connectionString=
namespace Testing
{
    public partial class Form1 : Form
    {
        public Dictionary<string, string> wmDict;
        public Dictionary<string, string> labelDict;

        private SqlConnection sqlConnection;
        public Form1()
        {
            Dictions dictions = new Dictions();
            InitializeComponent();
            wmDict = dictions.CreateWmDictionary();
            labelDict = dictions.CreateLabelDictionary();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RequestHistory"].ConnectionString);
            sqlConnection.Open();

            if (sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("SQL is connected!");
            }
            else
            {
                MessageBox.Show("SQL is not connected!" +
                    "\nНеобходимо указать верный путь к библиотеке RequestHistory.mdf.");
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 17)
            {
                MessageBox.Show("Некорректное число сомволов VIN-кода!");
                return;
            }

            string vin = textBox1.Text.ToUpper();
            string wmCode = $"{vin[0]}{vin[1]}";
            string labelCode = $"{vin[2]}";
            string country, label;

            string commandInsert = "INSERT INTO [TableHist] (Time, Vin) " +
                $"VALUES ('{DateTime.Now}', '{vin}')";

            if (wmDict.ContainsKey(wmCode))
            {
                country = wmDict[wmCode];
            }
            else
            {
                country = "Не определена";
            }

            if (labelDict.ContainsKey(labelCode))
            {
                label = labelDict[labelCode];
            }
            else
            {
                label = "Не определена";
            }

            SqlCommand command = new SqlCommand(commandInsert, sqlConnection);
            command.ExecuteNonQuery();



            MessageBox.Show($"Страна производителя: {country}." +
                $"\nМарка автомобиля: {label}.");
        }
        
    }
}
