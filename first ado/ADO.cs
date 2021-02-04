using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace first_ado
{
    class ADO
    {
        public SqlConnection con = new SqlConnection();
        public SqlCommand cmd = new SqlCommand();
        public SqlDataReader dr;
        public DataTable dt = new DataTable();
        //declaration de la method connecte
        public void Connecter()
        {
            if(con.State == ConnectionState.Closed || con.State == ConnectionState.Broken)
            {
                con.ConnectionString = "Data Source=DESKTOP-RKVCGVV;Initial Catalog=tdiadonetdevtechnology;Integrated Security=True";
                con.Open();
            }

        }
        //declaration de la method deconnecte
        public void Deconnecter()
        {
            if (con.State == ConnectionState.Open  )
            {
                //con.ConnectionString = "Data Source=DESKTOP-RKVCGVV;Initial Catalog=tdiadonetdevtechnology;Integrated Security=True";
                con.Close();
            }

        }
    }
}
