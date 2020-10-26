using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sistema.Negocio;

namespace Sistema.Presentacion
{
    public partial class FrmCliente : Form
    {
        private string NombreAnt;
        public FrmCliente()
        {
            InitializeComponent();
        }
        private void Listar()
        {
            try
            {
                DgvListado.DataSource = NPersona.ListarClientes();
                this.Formato(); //hace referencia al Metodo Formato
                this.Limpiar();
                LblTotal.Text = "Total Registros:" + Convert.ToString(DgvListado.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Buscar()
        {
            try
            {
                DgvListado.DataSource = NPersona.BuscarClientes(TxtBuscar.Text);
                this.Formato(); //hace referencia al Metodo Formato
                LblTotal.Text = "Total Registros:" + Convert.ToString(DgvListado.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Limpiar()
        {   //Va a limpiar los TextBox
            TxtBuscar.Clear();
            TxtId.Clear();
            TxtNombre.Clear();
            TxtNumDocumento.Clear();
            TxtDireccion.Clear();
            TxtTelefono.Clear();
            TxtEmail.Clear();
            BtnInsertar.Visible = true; //Para cuando se habilite la funcion Modificar
            BtnActualizar.Visible = false;
            ErrorIcono.Clear(); //El errorprovider va a limpiar los textbox

            //van a estar estos botones no visibles al principio
            DgvListado.Columns[0].Visible = false;
            BtnEliminar.Visible = false;
            ChkSeleccionar.Checked = false;
        }

        private void Formato()  //Metodo que se ejecuta con el MEtodo Buscar
        {
            DgvListado.Columns[0].Visible = false; //Nada
            DgvListado.Columns[1].Width = 50;
            DgvListado.Columns[2].Width = 100;
            DgvListado.Columns[2].HeaderText = "Tipo Persona";
            DgvListado.Columns[3].Width = 170;
            DgvListado.Columns[4].Width = 100;
            DgvListado.Columns[4].HeaderText = "Documento";
            DgvListado.Columns[5].Width = 100;
            DgvListado.Columns[5].HeaderText = "Número Doc.";
            DgvListado.Columns[6].Width = 120;
            DgvListado.Columns[6].HeaderText = "Dirección";
            DgvListado.Columns[7].Width = 100;
            DgvListado.Columns[7].HeaderText = "Teléfono";
            DgvListado.Columns[8].Width = 120;
        }

        private void MensajeError(string Mensaje)
        {   //Este metodo va a mostrar el mensaje de error
            MessageBox.Show(Mensaje, "Sistema de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MensajeOk(string Mensaje)
        {   //Este metodo va a mostrar el mensaje de Ok
            MessageBox.Show(Mensaje, "Sistema de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        private void BtnInsertar_Click(object sender, EventArgs e)
        {
            try
            {
                string Rpta = "";
                if (TxtNombre.Text == string.Empty)
                {
                    this.MensajeError("Faltan ingresar algunos datos, seran remarcados.");
                    ErrorIcono.SetError(TxtNombre, "Ingrese un Nombre.");
                }
                else
                {
                    Rpta = NPersona.Insertar("Cliente", TxtNombre.Text.Trim(), CboTipoDocumento.Text, TxtNumDocumento.Text.Trim(), TxtDireccion.Text.Trim(), TxtTelefono.Text.Trim(), TxtEmail.Text.Trim());
                    if (Rpta.Equals("OK"))
                    {
                        this.MensajeOk("Se inserto de forma correcta el registro");
                        this.Listar();
                    }
                    else
                    {
                        this.MensajeError(Rpta);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void DgvListado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.Limpiar(); //Primero llamo al metodo limpiar
                BtnActualizar.Visible = true;   //Dejo que este visible el boton Actualizar
                BtnInsertar.Visible = false;    //Dejo oculto el boton insertar
                //ahora empiezo a enviar valores a las cajas de texto para que se muestren
                //Las celdas son las mismas que utilizo en el Procedimiento Almacenado Usuario_Listar
                TxtId.Text = Convert.ToString(DgvListado.CurrentRow.Cells["ID"].Value);
                this.NombreAnt = Convert.ToString(DgvListado.CurrentRow.Cells["Nombre"].Value);
                TxtNombre.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Nombre"].Value);
                //Para el CboTipoDocumento no vamos a seleccionar un valor, ya que tiene agregado Items de manera manual, asi que usar.Text
                CboTipoDocumento.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Tipo_Documento"].Value);
                TxtNumDocumento.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Num_Documento"].Value);
                TxtDireccion.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Direccion"].Value);
                TxtTelefono.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Telefono"].Value);
                TxtEmail.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Email"].Value);
                //El contenido del TextBox convertirlo a cadena de texto y mostrarlo en el DgvListado en la fila actual 
                //Una vez que tenga toda la informacion voy a mostrar el formulario tabcontrol 
                tabControl1.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Seleccione desde la celda nombre." + "| Error: " + ex.Message);
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                string Rpta = "";
                if (TxtId.Text == string.Empty ||TxtNombre.Text == string.Empty)
                {
                    this.MensajeError("Faltan ingresar algunos datos, seran remarcados.");
                    ErrorIcono.SetError(TxtNombre, "Ingrese un Nombre.");
                }
                else
                {
                    Rpta = NPersona.Actualizar(Convert.ToInt32(TxtId.Text),"Cliente",this.NombreAnt, TxtNombre.Text.Trim(), CboTipoDocumento.Text, TxtNumDocumento.Text.Trim(), TxtDireccion.Text.Trim(), TxtTelefono.Text.Trim(), TxtEmail.Text.Trim());
                    if (Rpta.Equals("OK"))
                    {
                        this.MensajeOk("Se actualizó de forma correcta el registro");
                        this.Listar();
                    }
                    else
                    {
                        this.MensajeError(Rpta);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Limpiar();
            tabControl1.SelectedIndex = 0;
        }

        private void DgvListado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //para poder seleccionar el checkBox de una Fila

            if (e.ColumnIndex == DgvListado.Columns["Seleccionar"].Index) //Si eh seleccionado una celda de la columna Seleccionar voy a ejecutar:
            {
                DataGridViewCheckBoxCell ChkEliminar = (DataGridViewCheckBoxCell)DgvListado.Rows[e.RowIndex].Cells["Seleccionar"];
                ChkEliminar.Value = !Convert.ToBoolean(ChkEliminar.Value); //va a determinar para marcar y desmarcar en la celda Seleccionar
            }
        }

        private void ChkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSeleccionar.Checked) //Si el checkBox esta marcado/tildado
            {
                DgvListado.Columns[0].Visible = true; //me va a parecer la columna visible
                BtnEliminar.Visible = true;
            }
            else  //Si no esta tildado los botones y columna seleccionar no se van a mostrar
            {
                DgvListado.Columns[0].Visible = false;
                BtnEliminar.Visible = false;
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcion;     //Se crea la Variable Opcion para guardar el resultado de la seleccion Cancelar/Ok
                //Para un cartel con los botones cancelar y Ok
                Opcion = MessageBox.Show("Realmente deseas eliminar el(los) registro(s)?", "Sistema de Ventas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (Opcion == DialogResult.OK)
                {
                    int Codigo;
                    string Rpta;
                    foreach (DataGridViewRow row in DgvListado.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            Codigo = Convert.ToInt32(row.Cells[1].Value);
                            Rpta = NPersona.Eliminar(Codigo);

                            if (Rpta.Equals("Ok"))
                            {
                                //Si se elimino el registro concatenamos el mensaje con la columna 3 que seria Nombre
                                this.MensajeOk("Se elimino el registro: " + Convert.ToString(row.Cells[3].Value));
                            }
                            else
                            {
                                this.MensajeError(Rpta);
                            }
                        }
                    }
                    this.Listar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            this.Listar();
        }
    }
}
