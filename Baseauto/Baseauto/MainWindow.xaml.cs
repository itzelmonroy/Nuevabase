using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;//Agregamos libreria OleDB
using System.Data; //Agregamos System.Data

namespace WPFDBParte2
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexión
        DataTable dt;   //Agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            //Conectamos la base de datos a nuestro archivo Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Autos.mdb";
            MostrarDatos();
        }
        //Mostramos los registros de la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from autos";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Limpiamos todos los campos
        private void LimpiaTodo()
        {
            txtId.Text = "";
            cbcompania.SelectedIndex = 0;
            txtcosto.Text = "";
            txtnombre.Text = "";
            txtnum.Text = "";
            btnNuevo.Content = "Nuevo";
            txtId.IsEnabled = true;
        }
        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtId.Text != "")
            {
                if (txtId.IsEnabled == true)
                {
                    if (cbcompania.Text != "Selecciona marca de auto")
                    {
                        cmd.CommandText = "insert into autos(Id,Compania,Costo,Nombre,Telefono) " +
                            "Values(" + txtId.Text + ",'" + cbcompania.Text + "','" + txtcosto.Text + "',"
                      + txtnombre.Text + ",'" + txtnum.Text + "')";
                        cmd.ExecuteNonQuery();                                                                                                                                                                                                                                                                                                      
                        MostrarDatos();
                        MessageBox.Show(" agregado correctamente...");
                        LimpiaTodo();

                    }
                    else
                    {
                        MessageBox.Show("Favor de seleccionar la marca de auto....");
                    }
                }
                else
                {
                    cmd.CommandText = "update autos set Nombre='" + "',Id='" + txtId.Text+  cbcompania.Text + "',Costo=" + txtcosto.Text
                        + ",Nombre='" + txtnombre.Text + "',Telefono =" + txtnum.Text + "' where Id=" + txtId.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("El registro de autos ha sido actualizado...");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Favor de poner sus registros.......");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtId.Text = row["Id"].ToString();
                cbcompania.Text = row["Compañia"].ToString();
                txtcosto.Text = row["Costo"].ToString();
                txtnombre.Text = row["Nombre"].ToString();
                txtnum.Text = row["Telefono"].ToString();
                txtId.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar un auto...");
            }

        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "Delete from autos where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Registro de auto eliminado...");
                LimpiaTodo();
            }

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}



