using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Musteriler
{
    public partial class Form1 : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-4CGLM6O\SQLEXPRESS;Initial Catalog=Musteriler;Integrated Security=True;Encrypt=False");

        public Form1()
        {
            InitializeComponent();
        }

        string tarih;
        string saat;
        void listele()
        {
            baglanti.Open();
            string sorgu = "select * from MusteriBilgi";
            SqlDataAdapter adapter = new SqlDataAdapter(sorgu, baglanti);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dataGridView1.DataSource = dt;
            baglanti.Close();
        }

        void temizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox8.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            guncelleSaatTarih();
            listele();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            guncelleSaatTarih();
        }

        void guncelleSaatTarih()
        {
            label9.Text = (tarih = DateTime.Now.ToString("dd.MM.yyyy"));
            label10.Text = (saat = DateTime.Now.ToString("HH:mm"));
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var id = dataGridView1.Rows[e.RowIndex].Cells[0].Value;

            string sorgu = "select * from MusteriBilgi where Id=@id";
            SqlDataAdapter adapter = new SqlDataAdapter(sorgu,baglanti);
            adapter.SelectCommand.Parameters.AddWithValue("@id",id);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count>0)
            {
                textBox1.Text = dt.Rows[0]["Id"].ToString();
                textBox6.Text = dt.Rows[0]["Name"].ToString();
                textBox5.Text = dt.Rows[0]["Lastname"].ToString();
                textBox4.Text = dt.Rows[0]["Tc"].ToString();
                textBox3.Text = dt.Rows[0]["Sehir"].ToString();
                textBox2.Text = dt.Rows[0]["Telefon"].ToString();
                textBox8.Text = dt.Rows[0]["Meslek"].ToString();

                if (dt.Rows[0]["MedeniDurum"].ToString() == "True")
                {
                    radioButton1.Checked = true;
                }
                else
                {
                    radioButton2.Checked = true;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string ad = textBox6.Text;
            string soyad = textBox5.Text;
            string tc = textBox4.Text;
            string sehir = textBox3.Text;
            string telefon = textBox2.Text;
            string meslek = textBox8.Text;
            bool medeniDurum = radioButton1.Checked;

            if (textBox6.Text != "" || textBox5.Text!="" || textBox4.Text != "" || textBox3.Text != "" || textBox2.Text != "" || textBox8.Text != "")
            {

                baglanti.Open();

                string sorgu = "insert into MusteriBilgi (Name,Lastname,Tc,Sehir,Telefon,MedeniDurum,Meslek) values(@Name,@Lastname,@Tc,@Sehir,@Telefon,@MedeniDurum,@Meslek)";

                SqlCommand cmd = new SqlCommand(sorgu, baglanti);
                cmd.Parameters.AddWithValue("@Name", ad);
                cmd.Parameters.AddWithValue("@Lastname", soyad);
                cmd.Parameters.AddWithValue("@Tc", tc);
                cmd.Parameters.AddWithValue("@Sehir", sehir);
                cmd.Parameters.AddWithValue("@Telefon", telefon);
                cmd.Parameters.AddWithValue("@MedeniDurum", medeniDurum);
                cmd.Parameters.AddWithValue("@Meslek", meslek);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Kayıt başarıyla eklendi.");

                baglanti.Close();

                listele();
            }
            else
            {
                MessageBox.Show("Lütfen kaydetmek için tüm alanları doldurun.");
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listele();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                string sorgu = "delete from MusteriBilgi where Id=@id";

                baglanti.Open();

                SqlCommand cmd = new SqlCommand(sorgu, baglanti);
                cmd.Parameters.AddWithValue("@id", textBox1.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Kayıt başarıyla silindi.");

                baglanti.Close();

                listele();

                temizle();
            }
            else
            {
                MessageBox.Show("Lütfen silmek için listeden bir müşteri seçin!");
            }

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            string ad = textBox6.Text;
            string soyad = textBox5.Text;
            string tc = textBox4.Text;
            string sehir = textBox3.Text;
            string telefon = textBox2.Text;
            string meslek = textBox8.Text;
            bool medeniDurum = radioButton1.Checked;

            if (textBox1.Text != "")
            {
                baglanti.Open();

                string sorgu ="update MusteriBilgi set Name=@Name, LastName=@LastName,Tc=@Tc,Sehir=@Sehir,Telefon=@Telefon,MedeniDurum=@MedeniDurum,Meslek=@Meslek where Id=@id";

                SqlCommand cmd = new SqlCommand(sorgu, baglanti);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@Name", ad);
                cmd.Parameters.AddWithValue("@Lastname", soyad);
                cmd.Parameters.AddWithValue("@Tc", tc);
                cmd.Parameters.AddWithValue("@Sehir", sehir);
                cmd.Parameters.AddWithValue("@Telefon", telefon);
                cmd.Parameters.AddWithValue("@MedeniDurum", medeniDurum);
                cmd.Parameters.AddWithValue("@Meslek", meslek);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Kayıt başarıyla güncellendi.");

                baglanti.Close();

                listele();
            }
            else
            {
                MessageBox.Show("Lütfen güncellemek için listeden bir müşteri seçin!");
            }
        }
    }
}
