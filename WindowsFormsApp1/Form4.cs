using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        public static string connectString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Database1.mdb";

        private OleDbConnection myConnection;
        public Form4()
        {
            InitializeComponent();
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "database1DataSet.Ученики". При необходимости она может быть перемещена или удалена.
            this.ученикиTableAdapter.Fill(this.database1DataSet.Ученики);

        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы хотите сохранить изменения в своей таблице?", "My Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                e.Cancel = true;
            }
            myConnection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cmd_text;
            Form5 f2 = new Form5();
            if (f2.ShowDialog() == DialogResult.OK)
            {
                cmd_text = "INSERT INTO Student VALUES (" +
                "'" + f2.textBox1.Text + "' , '" +
                f2.textBox2.Text + "' , '" +
                f2.textBox3.Text + "' , " +
                f2.textBox4.Text + ")";

                // создать соединение с базой данных
                myConnection.Close();
                SqlConnection sql_conn = new SqlConnection(connectString);

                // создать команду на языке SQL
                SqlCommand sql_comm = new SqlCommand(cmd_text, sql_conn);

                sql_conn.Open(); // открыть соединение
                sql_comm.ExecuteNonQuery(); // выполнить команду на языке SQL
                sql_conn.Close(); // закрыть соединение

                this.ученикиTableAdapter.Fill(this.database1DataSet.Ученики);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cmd_text;
            Form5 f2 = new Form5();
            int index;
            string num_book;

            index = dataGridView1.CurrentRow.Index;
            num_book = Convert.ToString(dataGridView1[0, index].Value);

            f2.textBox1.Text = num_book;
            f2.textBox2.Text = Convert.ToString(dataGridView1[1, index].Value);
            f2.textBox3.Text = Convert.ToString(dataGridView1[2, index].Value);
            f2.textBox4.Text = Convert.ToString(dataGridView1[3, index].Value);

            if (f2.ShowDialog() == DialogResult.OK)
            {
                cmd_text = "UPDATE Student SET Num_book = '" + f2.textBox1.Text + "', " +
                "[Name] = '" + f2.textBox2.Text + "', " +
                "[Group] = '" + f2.textBox3.Text + "', " +
                "Year = " + f2.textBox4.Text +
                "WHERE Num_book = '" + num_book + "'";

                myConnection.Close();
                SqlConnection sql_conn = new SqlConnection(connectString);
                SqlCommand sql_comm = new SqlCommand(cmd_text, sql_conn);

                sql_conn.Open();
                sql_comm.ExecuteNonQuery();
                sql_conn.Close();

                this.ученикиTableAdapter.Fill(this.database1DataSet.Ученики);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cmd_text = "DELETE FROM Student";
            int index;
            string num_book;

            index = dataGridView1.CurrentRow.Index;
            num_book = Convert.ToString(dataGridView1[0, index].Value);
            cmd_text = "DELETE FROM Student WHERE [Student].[Num_book] = '" + num_book + "'";

            myConnection.Close();
            SqlConnection sql_conn = new SqlConnection(connectString);
            SqlCommand sql_comm = new SqlCommand(cmd_text, sql_conn);

            sql_conn.Open();
            sql_comm.ExecuteNonQuery();
            sql_conn.Close();

            this.ученикиTableAdapter.Fill(this.database1DataSet.Ученики);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
            myConnection.Close();
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Удалить запись?", "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
