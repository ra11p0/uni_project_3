using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace projekt_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static ulong plObliczanieSilni(float plLiczba)
        {
            //deklaracje zmiennych
            ulong plSilnia = 1;
            //obliczanie silni
            for (int pli = 1; pli <= plLiczba; pli++)
            {
                plSilnia = plSilnia * (ulong)pli;
            }
            if (plLiczba == 0 || plLiczba == 1) return 1;

            //zwracanie wyniku silni
            return plSilnia;
        }

        static double plObliczanieSumySzeregu(float plX, float plEps, out ushort plK)
        {
            //deklaracja zmiennych
            float plW;
            float plSumaSzeregu;
            //Stan poczatkowy
            plK = 0;
            plW = 0.5F;
            plSumaSzeregu = 0.0F;
            //iteracyjne obliczanie sumy szeregu potegowego

            do
            {
                plSumaSzeregu += plW;
                plK++;
                if (plK < 23) plW = plW * (plX * (((1 / (float)plObliczanieSilni(plK - 1)) + 1) / (plK + (1 / plObliczanieSilni(plK - 1)))));
                else if (plK >= 23) plW = plW * (plX / plK);

            } while (Math.Abs(plW) > plEps);
            //zwracanie sumy szeregu
            return plSumaSzeregu;
        }

        private void plOdczytPliku(string plSciezkaPliku)
        {
            if (!System.IO.File.Exists(plSciezkaPliku)) return;
            int plLicznik = 0;
            string[] plLinie = System.IO.File.ReadAllLines(plSciezkaPliku);
            foreach (string plTresc in plLinie)
            {
                switch (plLicznik)
                {
                    case 0:
                        textBox2.Text = plTresc;
                        break;
                    case 1:
                        textBox3.Text = plTresc;
                        break;
                    case 2:
                        textBox5.Text = plTresc;
                        break;
                    case 3:
                        textBox4.Text = plTresc;
                        break;
                    case 4:
                        textBox6.Text = plTresc;
                        break;
                    case 5:
                        textBox8.Text = plTresc;
                        break;
                    case 6:
                        textBox7.Text = plTresc;
                        break;
                    case 7:
                        textBox9.Text = plTresc;
                        break;
                }
                plLicznik++;
            }
        }

        private void plZapisPliku(string plSciezkaPliku)
        {
            //if (!System.IO.File.Exists(plSciezkaPliku)) return;
            using (System.IO.StreamWriter plWriter = System.IO.File.CreateText(plSciezkaPliku))
            {
                int pli = 0;
                string[] plTabelaDanych = new string[8];
                plTabelaDanych[0] = textBox2.Text;
                plTabelaDanych[1] = textBox3.Text;
                plTabelaDanych[2] = textBox5.Text;
                plTabelaDanych[3] = textBox4.Text;
                plTabelaDanych[4] = textBox6.Text;
                plTabelaDanych[5] = textBox8.Text;
                plTabelaDanych[6] = textBox7.Text;
                plTabelaDanych[7] = textBox9.Text;
                foreach (string plTresc in plTabelaDanych)
                {
                    plWriter.WriteLine(plTabelaDanych[pli]);
                    pli++;
                }
            }
        }

        public string plSciezkaZapisuCalki;
        private void plOdczytCalkiPliku(string plSciezkaPliku)
        {
            if (!System.IO.File.Exists(plSciezkaPliku)) return;
            string[] plWiersz = new string[2];
            dataGridView1.Visible = true;
            button8.Visible = false;
            chart1.Visible = false;
            int plLicznik = 0;
            string[] plLinie = System.IO.File.ReadAllLines(plSciezkaPliku);
            
                foreach (string plTresc in plLinie)
                {
                    plLicznik++;
                    plWiersz[0] = String.Format("{0:0.00}", plLicznik);
                    plWiersz[1] = plTresc;
                    dataGridView1.Rows.Add(plWiersz);
                    
                }
        }

        int plIleWierszy = 0;

        private void plCalkaDoPliku(string plSciezka)
        {
            //if (!System.IO.File.Exists(plSciezka)) return;
            using (System.IO.StreamWriter plNowyWriter = System.IO.File.CreateText(plSciezka))
            {
                for (int pli = 0; pli < plIleWierszy-1; pli++)
                {
                    plNowyWriter.WriteLine(dataGridView1.Rows[pli].Cells[1].Value.ToString());
                }
            }
        }





        static float plObliczanieCalkiMetodaProstokatow(float plEpsSzeregu, float pla, float plb, float plEpsCalkowania, out int plLicznikPrzedzialow, out float plSzerokoscPrzedzialu)
        {
            //deklaracje pomocnicze 
            float plH, plCi, plCi_1, plSumaFx, plX;
            ushort plLicznikWyrazow;
            plLicznikPrzedzialow = 1;
            plCi = (plb - pla) * (float)plObliczanieSumySzeregu((pla + plb) / 2.0F, plEpsSzeregu, out plLicznikWyrazow);

            do
            {
                plCi_1 = plCi;
                plLicznikPrzedzialow++;
                plH = (plb - pla) / plLicznikPrzedzialow;
                plX = pla + plH / 2.0F;
                plSumaFx = 0.0F;
                for (ushort pli = 0; pli < plLicznikPrzedzialow; pli++)
                    plSumaFx += (float)plObliczanieSumySzeregu(plX + pli * plH, plEpsSzeregu, out plLicznikWyrazow);
                plCi = plH * plSumaFx;
            } while (Math.Abs(plCi - plCi_1) > plEpsCalkowania);
            plSzerokoscPrzedzialu = plH;
            return plCi;

        }

        private float plObliczanieCalkiMetodaTrapezow(float plD, float plG, float plEps, out int plLicznik)
        {
            float plH, plCi, plCi_1, plSumaFx;
            plH = plG - plD;
            float plSumaFaFb = (float)(plObliczanieSumySzeregu(plD, plEps, out ushort plK) + plObliczanieSumySzeregu(plG, plEps, out plK));
            plCi = plH * plSumaFaFb;
            plLicznik = 1;
            dataGridView1.Visible = true;
            button8.Visible = false;
            chart1.Visible = false;
            string[] plWiersz = new string[2];
            do
            {
                plCi_1 = plCi;
                plLicznik++;
                plH = (plG - plD) / plLicznik;
                plSumaFx = 0.0f;
                for (int pli = 1; pli < plLicznik; pli++)
                {
                    plSumaFx = plSumaFx + (float)plObliczanieSumySzeregu(plD + pli * plH, plEps, out plK);

                }
                plCi = plH * (plSumaFaFb + plSumaFx);
                plWiersz[0] = String.Format("{0:0.00}", plLicznik);
                plWiersz[1] = String.Format("{0:F8}", plCi);
                dataGridView1.Rows.Add(plWiersz);

            } while (Math.Abs(plCi - plCi_1) > plEps);
            plIleWierszy = plLicznik;
            return plCi;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int plZmiana;
            //wykrywanie nieprawidłowości w polach danych
            if (!int.TryParse(textBox1.Text, out plZmiana))
            {
                textBox1.Text = "";
            }
            if (plZmiana > 10 && textBox1.Text != "")
            {
                plZmiana = 10;
                textBox1.Text = plZmiana.ToString();
            }
            if (plZmiana < 1 && textBox1.Text != "")
            {
                plZmiana = 1;
                textBox1.Text = plZmiana.ToString();
            }
            if (textBox1.Text == "")
            {
                plZmiana = 1;
            }
            //zastosowanie wpisanej wartosci grubosci linii w pasku
            trackBar1.Value = plZmiana;
            //zastosowanie wpisanej grubosci linii 
            chart1.Series["F(X)"].BorderWidth = plZmiana;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //zastosowanie zmiany paska zmiany grubosci linii do pola wpisania wartosci
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (!float.TryParse(textBox3.Text, out plWartosc))
            {
                errorProvider2.SetError(textBox3, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                textBox3.Text = "";
            }
            else
            {
                errorProvider2.Clear();
                if (plWartosc >= 1)
                {
                    errorProvider2.SetError(textBox3, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                    textBox3.Text = "";
                }
                else if (plWartosc < 0)
                {
                    errorProvider2.SetError(textBox3, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                    textBox3.Text = "";
                }
                else errorProvider2.Clear();

            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (!float.TryParse(textBox4.Text, out plWartosc) && textBox4.Text != "-")
            {
                errorProvider3.SetError(textBox4, "Dopuszczalne są tylko liczby wymierne.");
                textBox4.Text = "";
            }
            else errorProvider3.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (!float.TryParse(textBox2.Text, out plWartosc) && textBox2.Text != "-")
            {
                errorProvider1.SetError(textBox2, "Dopuszczalne są tylko liczby wymierne.");
                textBox2.Text = "";
            }
            else errorProvider1.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //żądanie wyboru koloru 
            colorDialog1.ShowDialog();
            //zastosowanie koloru dla wziernika oraz wykresu
            checkBox1.BackColor = colorDialog1.Color;
            chart1.Series["F(X)"].Color = checkBox1.BackColor;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //żądanie wyboru koloru
            colorDialog1.ShowDialog();
            //zastosowanie koloru dla wziernika oraz wykresu
            checkBox2.BackColor = colorDialog1.Color;
            chart1.BackColor = checkBox2.BackColor;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void zamknijFormularzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //zamkniecie formularza 
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            if (textBox6.Text == "" || textBox5.Text == "" || textBox4.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Nie wprowadzono danych lub wprowadzone dane są nieprawidłowe.", "Błąd!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //wyłączenie aktywności przycisków i pól danych 
            button8.Visible = false;
            textBox6.Enabled = false;
            textBox5.Enabled = false;
            textBox4.Enabled = false;
            textBox3.Enabled = false;

            button5.Enabled = true;
            button4.Enabled = true;
            button7.Enabled = false;

            float plX = float.Parse(textBox4.Text);
            float plSumaSzeregu;
            float plEps = float.Parse(textBox3.Text);

            //Stan poczatkowy
            string[] plWiersz = new string[2];

            //iteracyjne obliczanie sumy szeregu potegowego
            dataGridView1.Rows.Clear();
            do
            {
                plSumaSzeregu = (float)plObliczanieSumySzeregu(plX, plEps, out ushort plK);
                plWiersz[0] = String.Format("{0:0.00}", plX);
                plWiersz[1] = String.Format("{0:F8}", plSumaSzeregu);
                dataGridView1.Rows.Add(plWiersz);
                plX += float.Parse(textBox6.Text);
            } while (plX <= float.Parse(textBox5.Text));
            //ukrycie wykresu jesli jest wyswietlony
            chart1.Visible = false;
            //pokazanie tabeli 
            dataGridView1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            if (textBox6.Text == "" || textBox5.Text == "" || textBox4.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Nie wprowadzono danych lub wprowadzone dane są nieprawidłowe.", "Błąd!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //wyłączenie aktywności przycisków i pól danych
            textBox6.Enabled = false;
            textBox5.Enabled = false;
            textBox4.Enabled = false;
            textBox3.Enabled = false;

            button5.Enabled = false;
            button7.Enabled = true;
            button4.Enabled = true;
            dataGridView1.Visible = false;
            chart1.Visible = true;
            button8.Visible = false;

            float plX = float.Parse(textBox4.Text);
            float plSumaSzeregu;
            float plEps = float.Parse(textBox3.Text);
            //Stan poczatkowy
            chart1.Series["F(X)"].Points.Clear();
            //iteracyjne obliczanie sumy szeregu potegowego
            do
            {
                plSumaSzeregu = (float)plObliczanieSumySzeregu(plX, plEps, out ushort plK);
                chart1.Series["F(X)"].Points.AddXY(plX, plSumaSzeregu);
                plX += float.Parse(textBox6.Text);
            } while (plX <= float.Parse(textBox5.Text));
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            //ustalenie grubosci linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderWidth = 1;
            trackBar1.Value = 1;
            textBox1.Text = 1.ToString();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            //ustalenie grubosci linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderWidth = 2;
            trackBar1.Value = 2;
            textBox1.Text = 2.ToString();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            //ustalenie grubosci linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderWidth = 3;
            trackBar1.Value = 3;
            textBox1.Text = 3.ToString();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            //ustalenie grubosci linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderWidth = 4;
            trackBar1.Value = 4;
            textBox1.Text = 4.ToString();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            //ustalenie grubosci linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderWidth = 5;
            trackBar1.Value = 5;
            textBox1.Text = 5.ToString();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            //ustalenie grubosci linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderWidth = 6;
            trackBar1.Value = 6;
            textBox1.Text = 6.ToString();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            //ustalenie grubosci linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderWidth = 7;
            trackBar1.Value = 7;
            textBox1.Text = 7.ToString();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            //ustalenie grubosci linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderWidth = 8;
            trackBar1.Value = 8;
            textBox1.Text = 8.ToString();
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            //ustalenie grubosci linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderWidth = 9;
            trackBar1.Value = 9;
            textBox1.Text = 9.ToString();
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            //ustalenie grubosci linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderWidth = 10;
            trackBar1.Value = 10;
            textBox1.Text = 10.ToString();
        }

        private void kropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ustalenie stylu linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            comboBox1.SelectedIndex = 1;
        }

        private void kreskowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ustalenie stylu linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            comboBox1.SelectedIndex = 2;
        }

        private void kreskowoKropkowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ustalenie stylu linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot;
            comboBox1.SelectedIndex = 3;
        }

        private void ciągłaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ustalenie stylu linii wykresu z menu górnego
            chart1.Series["F(X)"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ustalenie stylu linii wykresu
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    chart1.Series["F(X)"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
                    break;
                case 1:
                    chart1.Series["F(X)"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
                    break;
                case 2:
                    chart1.Series["F(X)"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
                    break;
                case 3:
                    chart1.Series["F(X)"].BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot;
                    break;

            }
        }

        private void kolorTłaWykresuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ustalenie koloru tła wykresu z menu górnego
            colorDialog1.ShowDialog();
            checkBox2.BackColor = colorDialog1.Color;
            chart1.BackColor = checkBox2.BackColor;
        }

        private void kolorLiniiWykresuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ustalenie koloru linii wykresu z menu górnego
            colorDialog1.ShowDialog();
            checkBox1.BackColor = colorDialog1.Color;
            chart1.Series["F(X)"].Color = checkBox1.BackColor;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //ukrycie lub wyświetlenie podpisów na wykresie 
            if (radioButton1.Checked == true)
            {
                radioButton2.Checked = false;
                chart1.Titles[1].Visible = false;
                chart1.Titles[2].Visible = false;
            }
            else
            {
                radioButton2.Checked = true;
                chart1.Titles[1].Visible = true;
                chart1.Titles[2].Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            if (textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Nie wprowadzono danych lub wprowadzone dane są nieprawidłowe.", "Błąd!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //wyłączenie aktywności przycisków i pól danych
            textBox2.Enabled = false;
            textBox3.Enabled = false;

            button5.Enabled = true;
            button7.Enabled = true;
            button4.Enabled = false;
            button8.Visible = true;
            button8.ForeColor = button7.ForeColor;
            //wyświetlenie obliczonej sumy szeregu
            button8.Text = "Obliczona suma szeregu o zmiennej niezależnej X = " + textBox2.Text + " jest równa: " + string.Format("{0:F4}", plObliczanieSumySzeregu(float.Parse(textBox2.Text), float.Parse(textBox3.Text), out ushort plLicznik));
            chart1.Visible = false;
            dataGridView1.Visible = false;
            
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (float.TryParse(textBox3.Text, out plWartosc))
            {
                if (plWartosc == 0 )
                {
                    errorProvider2.SetError(textBox3, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                    textBox3.Text = "";
                }
                else errorProvider2.Clear();
            }
            else if (textBox3.Text == "")
            {
                errorProvider2.SetError(textBox3, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                textBox3.Text = "";
            }
            else errorProvider2.Clear();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (!float.TryParse(textBox6.Text, out plWartosc))
            {
                errorProvider5.SetError(textBox6, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                textBox6.Text = "";
            }
            else
            {
                errorProvider5.Clear();
                if (plWartosc >= 1)
                {
                    errorProvider5.SetError(textBox6, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                    textBox6.Text = "";
                }
                else if (plWartosc < 0)
                {
                    errorProvider5.SetError(textBox6, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                    textBox6.Text = "";
                }
                else errorProvider5.Clear();

            }
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (float.TryParse(textBox6.Text, out plWartosc))
            {
                if (plWartosc == 0)
                {
                    errorProvider5.SetError(textBox6, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                    textBox6.Text = "";
                }
                else errorProvider5.Clear();
            }
            else if (textBox6.Text == "")
            {
                errorProvider5.SetError(textBox6, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                textBox6.Text = "";
            }
            else errorProvider5.Clear();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (!float.TryParse(textBox5.Text, out plWartosc) && textBox5.Text != "-")
            {
                errorProvider4.SetError(textBox5, "Dopuszczalne są tylko liczby wymierne, większe od Xd.");
                textBox5.Text = "";
            }
            else errorProvider4.Clear();
            
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc1, plWartosc2;
            if (float.TryParse(textBox5.Text, out plWartosc1))
            {
                if (float.TryParse(textBox4.Text, out plWartosc2))
                {
                    if (plWartosc1 <= plWartosc2)
                    {
                        errorProvider4.SetError(textBox5, "Dopuszczalne są tylko liczby wymierne, większe od Xd.");
                        textBox5.Text = "";
                    }
                }
                else errorProvider4.Clear();
            }
            else if (textBox5.Text == "-")
            {
                errorProvider4.SetError(textBox5, "Dopuszczalne są tylko liczby wymierne, większe od Xd.");
                textBox5.Text = "";
            }
            else errorProvider4.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //resetowanie formularza
            textBox2.Enabled = true;
            textBox6.Enabled = true;
            textBox5.Enabled = true;
            textBox4.Enabled = true;
            textBox3.Enabled = true;

            textBox2.Text = "";
            textBox6.Text = "";
            textBox5.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";

            button5.Enabled = true;
            button7.Enabled = true;
            button4.Enabled = true;
            dataGridView1.Visible = false;
            chart1.Visible = false;
            button8.Visible = false;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            if (textBox2.Text == "-")
            {
                errorProvider1.SetError(textBox2, "Dopuszczalne są tylko liczby wymierne.");
                textBox2.Text = "";
            }
            else errorProvider1.Clear();

        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            if (textBox4.Text == "-")
            {
                errorProvider3.SetError(textBox4, "Dopuszczalne są tylko liczby wymierne.");
                textBox4.Text = "";
            }
            else errorProvider3.Clear();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            if (textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "" || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Nie wprowadzono danych lub wprowadzone dane są nieprawidłowe.", "Błąd!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //zastosowanie metody obliczania całki 
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    textBox10.Text = string.Format("{0:f4}", plObliczanieCalkiMetodaProstokatow(float.Parse(textBox9.Text), float.Parse(textBox7.Text), float.Parse(textBox8.Text), float.Parse(textBox9.Text), out int plLicznikPrzedzialow, out float plSzerokoscPrzedzialu));
                    break;
                case 1:
                    textBox10.Text = String.Format("{0:f4}", plObliczanieCalkiMetodaTrapezow(float.Parse(textBox7.Text), float.Parse(textBox8.Text), float.Parse(textBox9.Text), out int plLicznik));
                    break;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (!float.TryParse(textBox9.Text, out plWartosc))
            {
                errorProvider6.SetError(textBox9, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                textBox9.Text = "";
            }
            else
            {
                errorProvider6.Clear();
                if (plWartosc >= 1)
                {
                    errorProvider6.SetError(textBox9, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                    textBox9.Text = "";
                }
                else if (plWartosc < 0)
                {
                    errorProvider6.SetError(textBox9, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                    textBox9.Text = "";
                }
                else errorProvider6.Clear();
            }
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (float.TryParse(textBox9.Text, out plWartosc))
            {
                if (plWartosc == 0)
                {
                    errorProvider6.SetError(textBox9, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                    textBox9.Text = "";
                }
                else errorProvider6.Clear();
            }
            else if (textBox9.Text == "")
            {
                errorProvider6.SetError(textBox9, "Dopuszczalne są tylko dodatnie liczby mniejsze od 1.");
                textBox9.Text = "";
            }
            else errorProvider6.Clear();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (!float.TryParse(textBox8.Text, out plWartosc) && textBox8.Text != "-")
            {
                errorProvider7.SetError(textBox8, "Dopuszczalne są tylko liczby wymierne, większe od Xd.");
                textBox8.Text = "";
            }
            else errorProvider7.Clear();
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc1, plWartosc2;
            if (float.TryParse(textBox8.Text, out plWartosc1))
            {
                if (float.TryParse(textBox7.Text, out plWartosc2))
                {
                    if (plWartosc1 <= plWartosc2)
                    {
                        errorProvider7.SetError(textBox8, "Dopuszczalne są tylko liczby wymierne, większe od Xd.");
                        textBox8.Text = "";
                    }
                }
                else errorProvider7.Clear();
            }
            else if (textBox8.Text == "-")
            {
                errorProvider7.SetError(textBox8, "Dopuszczalne są tylko liczby wymierne, większe od Xd.");
                textBox8.Text = "";
            }
            else errorProvider7.Clear();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            float plWartosc;
            if (!float.TryParse(textBox7.Text, out plWartosc) && textBox7.Text != "-")
            {
                errorProvider8.SetError(textBox7, "Dopuszczalne są tylko liczby wymierne.");
                textBox7.Text = "";
            }
            else errorProvider8.Clear();
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            //wykrywanie nieprawidłowości w polach danych
            if (textBox7.Text == "-")
            {
                errorProvider8.SetError(textBox7, "Dopuszczalne są tylko liczby wymierne.");
                textBox7.Text = "";
            }
            else errorProvider8.Clear();
        }

       

        private void odczytajTablicęZPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "zapis.txt";
            openFileDialog1.ShowDialog();
            string plSciezka = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
            plOdczytPliku(plSciezka);

        }

        private void zapiszTablicęWPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "zapis.txt";
            saveFileDialog1.ShowDialog();
            string plSciezka = saveFileDialog1.InitialDirectory + saveFileDialog1.FileName;
            plZapisPliku(plSciezka);
        }

        private void pogrubionaIKursywaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void zapiszWierszeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "zapisCalki.txt";
            saveFileDialog1.ShowDialog();
            string plSciezkaZapisuCalki = saveFileDialog1.InitialDirectory + saveFileDialog1.FileName;
            plCalkaDoPliku(plSciezkaZapisuCalki);

        }

        private void zmieńCzcionkęfontWszystkichKontrolekLabelWFormularzuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            Font plCzcionka = fontDialog1.Font;
            this.label1.Font = plCzcionka;
            this.label2.Font = plCzcionka;
            this.label3.Font = plCzcionka;
            this.label4.Font = plCzcionka;
            this.label5.Font = plCzcionka;
            this.label6.Font = plCzcionka;
            this.label7.Font = plCzcionka;
            this.label8.Font = plCzcionka;
            this.label9.Font = plCzcionka;
            this.label10.Font = plCzcionka;
            this.label11.Font = plCzcionka;
            this.label12.Font = plCzcionka;
            this.label13.Font = plCzcionka;
            this.label14.Font = plCzcionka;

        }

        private void zmieńKolorCzcionkifontuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //żądanie wyboru koloru 
            colorDialog1.ShowDialog();

            button1.ForeColor = colorDialog1.Color;
            button2.ForeColor = colorDialog1.Color;
            button3.ForeColor = colorDialog1.Color;
            button4.ForeColor = colorDialog1.Color;
            button5.ForeColor = colorDialog1.Color;
            button6.ForeColor = colorDialog1.Color;
            button7.ForeColor = colorDialog1.Color;
            button8.ForeColor = colorDialog1.Color;
        }

        private void odczytajDaneZPlikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "zapis.txt";
            openFileDialog1.ShowDialog();
            string plSciezka = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
            plOdczytCalkiPliku(plSciezka);
        }
    }
    
}
