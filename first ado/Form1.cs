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

namespace first_ado
{
    public partial class Form1 : Form
    {
        ADO d = new ADO();
        public Form1()
        {
            InitializeComponent();
        }
        public static int cpt;
        public void DisplayGrid()
        {
            Clear();
            d.Connecter();
            d.cmd.CommandText = "select * from stagaire";
            d.cmd.Connection = d.con;
            d.dr = d.cmd.ExecuteReader();
            d.dt.Load(d.dr);
            dataGridView1.DataSource = d.dt;
            d.dr.Close();
        }
        public void RemplirComboboxSearchById()
        {
            comboBox1.Items.Clear();
            d.cmd.CommandText = "select matricule from stagaire";
            d.cmd.Connection = d.con;
            d.dr = d.cmd.ExecuteReader();
            while (d.dr.Read())
            {
                comboBox1.Items.Add(d.dr[0]);
            }
            d.dr.Close();
        }
        DataSet datas = new DataSet();
        public void ExporterXml()
        {
            d.cmd.CommandText = "select * from stagaire";
            d.cmd.Connection = d.con;
            d.dr = d.cmd.ExecuteReader();
            datas.Tables.Add("st");
            datas.Tables["st"].Load(d.dr);
            string filepath = "";
            saveFileDialog1.Filter = "XML FILES |.*xml";
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filepath = saveFileDialog1.FileName;
            }
            datas.WriteXml(filepath);
            MessageBox.Show("done");

        }
        DataTable datat = new DataTable();
        
        public void ExportText()
        {
            d.cmd.CommandText = "select * from stagaire";
            d.cmd.Connection = d.con;
            d.dr = d.cmd.ExecuteReader();
            datat.Load(d.dr);
            string filepath = "";
            saveFileDialog1.Filter = "TEXT FILES |.*txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                filepath = saveFileDialog1.FileName;
                StreamWriter st = new StreamWriter(filepath);
                for (int i = 0; i < datat.Rows.Count; i++)
                {
                    st.WriteLine(datat.Rows[i][0].ToString() + "  " + datat.Rows[i][1].ToString() + "  " + datat.Rows[i][2].ToString() + "  " + datat.Rows[i][3].ToString() );
                }
                st.Close();
            }
            //datat.WriteXml(filepath);
        }
        public void AddColumnCheckBox()
        {
            DataGridViewCheckBoxColumn check = new DataGridViewCheckBoxColumn();
            check.Name = "suppression";
            dataGridView1.Columns.Add(check);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cpt = 0;
            DisplayGrid();
            RemplirComboboxSearchById();
            AddColumnCheckBox();
        }

        private void btnFermer_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("voulez vous quitter l'application","Confirmation",MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                d.Deconnecter();
                this.Close();
            }
     
        }
        public void Vider(Control f)
        {
            foreach (Control ct in f.Controls)
            {
                if(ct.GetType() == typeof(TextBox))
                {
                    ct.Text = "";
                }
                if(ct.Controls.Count != 0)
                {
                    Vider(ct);
                }
            }
        }

        private void btnVider_Click(object sender, EventArgs e)
        {
            Vider(this);
            
        }
        public void Ajouter()
        {
            d.cmd.CommandText = "insert into stagaire values ('"+ txtName.Text + "','"+ txtPrenom.Text + "','"+ txtMoyenne.Text + "','"+ txtAge.Text + "')";
            d.cmd.Connection = d.con;
            d.cmd.ExecuteNonQuery();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Ajouter();
            Clear();
            DisplayGrid();
        }
        public void Clear()
        {
            d.dt.Clear();
            /*
            for (int i = 0; i < d.dt.Rows.Count; i++)
            {
                d.dt.Rows.RemoveAt(i);
            }
            */
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        public void Supprimer()
        {
            int position = dataGridView1.CurrentRow.Index;
            int id = int.Parse(dataGridView1.Rows[position].Cells[0].Value.ToString());
            d.cmd.CommandText = "delete from stagaire where matricule  = " + id;
            d.cmd.Connection = d.con;
            d.cmd.ExecuteNonQuery();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //int x = dataGridView1.CurrentRow.Index;
            //x = x + 1;
            //MessageBox.Show(x.ToString());
            Supprimer();
            Clear();
            DisplayGrid();
        }
        public bool  CheckList()
        {
            if(txtAge.Text == string.Empty || txtName.Text == string.Empty || txtPrenom.Text == string.Empty || txtMoyenne.Text == string.Empty)
            {
                MessageBox.Show("all informations required");
                return false;
            }
            else
            {
                return true;
            }
        }
        public void Modifier()
        {
            if(CheckList() == true)
            {
                int position = dataGridView1.CurrentRow.Index;
                int id = int.Parse(dataGridView1.Rows[position].Cells[0].Value.ToString());
                d.cmd.CommandText = "UPDATE stagaire SET nom = +'"+txtName.Text+ "', prenom = +'" + txtPrenom.Text + "', moyenne = +'" + txtMoyenne.Text + "', age = +'" + txtAge.Text + "' WHERE matricule = " + id;
                d.cmd.Connection = d.con;
                d.cmd.ExecuteNonQuery();
            }
            

        }

        private void BtnModifier_Click(object sender, EventArgs e)
        {
            Modifier();
            Clear();
            DisplayGrid();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Clear();
            d.Connecter();
            d.cmd.CommandText = "select * from stagaire where nom = +'" + txtName.Text + "'or prenom = +'" + txtPrenom.Text + "'or moyenne = +'" + txtMoyenne.Text + "'or age = +'" + txtAge.Text + "'";
            d.cmd.Connection = d.con;
            d.dr = d.cmd.ExecuteReader();
            d.dt.Load(d.dr);
            dataGridView1.DataSource = d.dt;
            d.dr.Close();
        }

        private void btnDisplayAll_Click(object sender, EventArgs e)
        {
            DisplayGrid();
        }
        public void Naviger(int cpt)
        {
            txtName.Text = d.dt.Rows[cpt][1].ToString();
            txtPrenom.Text = d.dt.Rows[cpt][2].ToString();            
            txtMoyenne.Text = d.dt.Rows[cpt][3].ToString();
            txtAge.Text = d.dt.Rows[cpt][4].ToString();

        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            Naviger(0);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int x = d.dt.Rows.Count;
            Naviger(x-1);
        }
        
        private void btnNext_Click(object sender, EventArgs e)
        {
            cpt++;
            //MessageBox.Show(dataGridView1.Rows.Count.ToString());
            //MessageBox.Show(cpt.ToString());
            if(dataGridView1.Rows.Count - 1> cpt)
            {
                Naviger(cpt);
                //dataGridView1.Rows[cpt].Selected = true;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Index == cpt)
                    {
                        row.Selected = true;
                    }
                    else
                    {
                        row.Selected = false;
                    }

                }

            }
            else
            {
                return;
            }
            MessageBox.Show(cpt.ToString());


        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            cpt--;
            //MessageBox.Show(dataGridView1.Rows.Count.ToString());
            //MessageBox.Show(cpt.ToString());
            if (0<= cpt)
            {
                Naviger(cpt);
                dataGridView1.Rows[cpt].Selected = true;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Index == cpt)
                    {
                        row.Selected = true;
                    }
                    else
                    {
                        row.Selected = false;
                    }

                }
            }
            else
            {
                return;
            }

        }

        private void XML_Click(object sender, EventArgs e)
        {
            ExporterXml();
        }

        private void BtnTxt_Click(object sender, EventArgs e)
        {
            ExportText();
        }

        private void DeleteViaCheckBox_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(dr.Cells["suppression"].Value) == true)
                {
                    d.cmd.CommandText = "delete from stagaire where matricule = '"+dr.Cells["matricule"].Value.ToString()+"'";
                    d.cmd.Connection = d.con;
                    d.cmd.ExecuteNonQuery();
                }
            }
            //Clear();
            DisplayGrid();


        }
        public void SupprimerById( int id)
        {
            d.cmd.CommandText = "delete from stagaire where matricule  = " + id;
            d.cmd.Connection = d.con;
            d.cmd.ExecuteNonQuery();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            //int[] ids = { };
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if(dr.Selected == true)
                {
                    int position = dr.Index;
                    int id = int.Parse(dataGridView1.Rows[position].Cells[0].Value.ToString());
                    //MessageBox.Show(position.ToString());
                    //MessageBox.Show(id.ToString());
                    SupprimerById(id);
                    //DisplayGrid();
                }
                
            }

            /*int id = int.Parse(dataGridView1.Rows[position].Cells[0].Value.ToString());
            SupprimerById(id);
            DisplayGrid();
            */
            DisplayGrid();



        }
    }
}
