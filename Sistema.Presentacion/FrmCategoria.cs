using System;
using System.Windows.Forms;
using Sistema.Negocio;

namespace Sistema.Presentacion
{
    public partial class FrmCategoria : Form
    {
        private string NombreAnt; //se va a almacenar el nombre anterior ya que en
                    //actualizar no permite actualizar en un mismo registro la descripcion

        public FrmCategoria()
        {
            InitializeComponent();
        }

        private void Listar()
        {
            try
            {
                DgvListado.DataSource = NCategoria.Listar();
                this.Formato(); //hace referencia al Metodo Formato
                this.Limpiar();
                LblTotal.Text = "Total Registros:" + Convert.ToString(DgvListado.Rows.Count);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Buscar()
        {
            try
            {
                DgvListado.DataSource = NCategoria.Buscar(TxtBuscar.Text);
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
            TxtDescripcion.Clear();
            BtnInsertar.Visible = true; //Para cuando se habilite la funcion Modificar
            BtnActualizar.Visible = false;
            ErrorIcono.Clear(); //El errorprovider va a limpiar los textbox

            //van a estar estos botones no visibles al principio
            DgvListado.Columns[0].Visible = false;
            BtnActivar.Visible = false;
            BtnDesactivar.Visible = false;
            BtnEliminar.Visible = false;
            ChkSeleccionar.Checked = false;
        }

        private void MensajeError(string Mensaje)
        {   //Este metodo va a mostrar el mensaje de error
            MessageBox.Show(Mensaje, "Sistema de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MensajeOk(string Mensaje)
        {   //Este metodo va a mostrar el mensaje de Ok
            MessageBox.Show(Mensaje, "Sistema de Ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Formato()
        {
            DgvListado.Columns[0].Visible = false;
            DgvListado.Columns[1].Visible = false;
            DgvListado.Columns[2].Width = 250;
            DgvListado.Columns[3].Width = 400;
            DgvListado.Columns[3].HeaderText = "Descripción";
            DgvListado.Columns[4].Width = 150;         
        }

        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            //Hace referencia al metodo Listar()
            this.Listar();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            //Hace referencia al metodo Buscar()
            this.Buscar();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void BtnInsertar_Click(object sender, EventArgs e)
        {
            try
            {
                string Rpta = "";
                if (TxtNombre.Text == string.Empty)
                {
                    this.MensajeError("Faltan ingresar algunos datos, seran remarcados.");
                    ErrorIcono.SetError(TxtNombre, "Ingrese un Nombre"); //ErrorProvider
                }
                else
                {
                    Rpta = NCategoria.Insertar(TxtNombre.Text.Trim(), TxtDescripcion.Text.Trim());
                    if (Rpta.Equals("OK"))
                    {
                        this.MensajeOk("Se inserto de forma correcta el registro");
                        this.Limpiar();
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
            this.tabControl1.SelectedIndex = 0; // Para volver de un Tab a otro, usar 0,1,2,etc..
        }

        private void DgvListado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.Limpiar();
                BtnActualizar.Visible = true;
                BtnInsertar.Visible = false;
                TxtId.Text = Convert.ToString(DgvListado.CurrentRow.Cells["ID"].Value);
                this.NombreAnt = Convert.ToString(DgvListado.CurrentRow.Cells["Nombre"].Value);
                TxtNombre.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Nombre"].Value);
                TxtDescripcion.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Descripcion"].Value);
                tabControl1.SelectedIndex = 1;
            }
            catch(Exception)
            {
                MessageBox.Show("Seleccione la celda Nombre");
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                string Rpta = "";
                if (TxtNombre.Text == string.Empty || TxtId.Text == string.Empty) // || con alt+124
                {
                    this.MensajeError("Faltan ingresar algunos datos, seran remarcados.");
                    ErrorIcono.SetError(TxtNombre, "Ingrese un Nombre"); //ErrorProvider
                }
                else
                {
                    Rpta = NCategoria.Actualizar(Convert.ToInt32(TxtId.Text),this.NombreAnt, TxtNombre.Text.Trim(), TxtDescripcion.Text.Trim());
                    if (Rpta.Equals("OK"))
                    {
                        this.MensajeOk("Se actualizó de forma correcta el registro");
                        this.Limpiar();
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

        private void ChkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSeleccionar.Checked) //Si el checkBox esta marcado/tildado
            {
                DgvListado.Columns[0].Visible = true; //me va a parecer la columna visible
                BtnActivar.Visible = true;
                BtnDesactivar.Visible = true;
                BtnEliminar.Visible = true;
            }
            else  //Si no esta tildado los botones y columna seleccionar no se van a mostrar
            {
                DgvListado.Columns[0].Visible = false; 
                BtnActivar.Visible = false;
                BtnDesactivar.Visible = false;
                BtnEliminar.Visible = false;
            }
        }

        private void DgvListado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == DgvListado.Columns["Seleccionar"].Index) //Si eh seleccionado una celda de la columna Seleccionar voy a ejecutar:
            {
                DataGridViewCheckBoxCell ChkEliminar = (DataGridViewCheckBoxCell)DgvListado.Rows[e.RowIndex].Cells["Seleccionar"];
                ChkEliminar.Value = !Convert.ToBoolean(ChkEliminar.Value); //va a determinar para marcar y desmarcar en la celda Seleccionar
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcion;     //Se crea la Variable Opcion para guardar el resultado de la seleccion Cancelar/Ok
                //Para un cartel con los botones cancelar y Ok
                Opcion = MessageBox.Show("Realmente deseas eliminar el(los) registro(s)?", "Sistema de Ventas", MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                if(Opcion == DialogResult.OK)
                {
                    int Codigo;
                    string Rpta;
                    foreach(DataGridViewRow row in DgvListado.Rows)
                    {
                        if(Convert.ToBoolean(row.Cells[0].Value))
                        {
                            Codigo = Convert.ToInt32(row.Cells[1].Value);
                            Rpta = NCategoria.Eliminar(Codigo);

                            if(Rpta.Equals("Ok"))
                            {
                                this.MensajeOk("Se elimino el registro: " + Convert.ToString(row.Cells[2].Value));
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

        private void BtnActivar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcion;     //Se crea la Variable Opcion para guardar el resultado de la seleccion Cancelar/Ok
                //Para un cartel con los botones cancelar y Ok
                Opcion = MessageBox.Show("Realmente deseas activar el(los) registro(s)?", "Sistema de Ventas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (Opcion == DialogResult.OK)
                {
                    int Codigo;
                    string Rpta;
                    foreach (DataGridViewRow row in DgvListado.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            Codigo = Convert.ToInt32(row.Cells[1].Value);
                            Rpta = NCategoria.Activar(Codigo);

                            if (Rpta.Equals("Ok"))
                            {
                                this.MensajeOk("Se activó el registro: " + Convert.ToString(row.Cells[2].Value));
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

        private void BtnDesactivar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcion;     //Se crea la Variable Opcion para guardar el resultado de la seleccion Cancelar/Ok
                //Para un cartel con los botones cancelar y Ok
                Opcion = MessageBox.Show("Realmente deseas desactivar el(los) registro(s)?", "Sistema de Ventas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (Opcion == DialogResult.OK)
                {
                    int Codigo;
                    string Rpta;
                    foreach (DataGridViewRow row in DgvListado.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            Codigo = Convert.ToInt32(row.Cells[1].Value);
                            Rpta = NCategoria.Desactivar(Codigo);

                            if (Rpta.Equals("Ok"))
                            {
                                this.MensajeOk("Se desactivó el registro: " + Convert.ToString(row.Cells[2].Value));
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
    }
}
