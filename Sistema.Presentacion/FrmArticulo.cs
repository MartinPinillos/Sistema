using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Xml.Linq;
using Sistema.Negocio;
using System.IO;

namespace Sistema.Presentacion
{
    public partial class FrmArticulo : Form
    {
        private string RutaOrigen;  //Voy a almacenar la ruta absoulta de la imagen que estoy seleccionando
        private string RutaDestino; //Direcctorio donde voy a cargar la imagen
        private string Directorio = "C:\\Proyectos\\Imagenes\\";
        private string NombreAnt; // variable que se utilizara para  verificar si el articulo cambia o no cambia
        public FrmArticulo()
        {
            InitializeComponent();
        }

        private void Listar()
        {
            try
            {
                DgvListado.DataSource = NArticulo.Listar();
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
                DgvListado.DataSource = NArticulo.Buscar(TxtBuscar.Text);
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
            TxtCodigo.Clear();
            TxtNombre.Clear();
            PanelCodigo.BackgroundImage = null;
            BtnGuardarCodigo.Enabled = true;
            TxtPrecioVenta.Clear();
            TxtStock.Clear();
            TxtImagen.Clear();
            PicImagen.Image = null;
            TxtDescripcion.Clear();
            BtnInsertar.Visible = true; //Para cuando se habilite la funcion Modificar
            BtnActualizar.Visible = false;
            ErrorIcono.Clear(); //El errorprovider va a limpiar los textbox
            this.RutaDestino = "";
            this.RutaOrigen = "";

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

        private void CargarCategoria()
        {
            try
            {
                CboCategoria.DataSource = NCategoria.Seleccionar();
                CboCategoria.ValueMember = "idcategoria"; // el valor de los items lo voy a obtener de la columna idcategoria
                CboCategoria.DisplayMember = "nombre";      //El texto a mostrar de cada item lo  obtengo de la columna nombre
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Formato()
        {
            DgvListado.Columns[0].Visible = false;
            DgvListado.Columns[0].Width = 100; 
            DgvListado.Columns[1].Width = 50;
            DgvListado.Columns[2].Visible = false; //columna idcategoria
            DgvListado.Columns[3].Width = 100;
            DgvListado.Columns[3].HeaderText = "Categoría";
            DgvListado.Columns[4].Width = 100;
            DgvListado.Columns[4].HeaderText = "Código";
            DgvListado.Columns[5].Width = 150;
            DgvListado.Columns[6].Width = 100;
            DgvListado.Columns[6].HeaderText = "Precio Venta";
            DgvListado.Columns[7].Width = 60;
            DgvListado.Columns[8].Width = 200;
            DgvListado.Columns[8].HeaderText = "Descripción";
            DgvListado.Columns[9].Width = 100;
            DgvListado.Columns[10].Width = 100;
        }

        private void FrmArticulo_Load(object sender, EventArgs e)
        {
            this.Listar();
            this.CargarCategoria(); //al inicio para que me cargue las categorias
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void BtnCargarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog(); 
            //Con el Metodo Filter solamente me va a visualizar los siguientes formatos permitidos
            file.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (file.ShowDialog() == DialogResult.OK)    //si despues de mostrar la ventana para seleccionar un archivo el archivo es OK
            {
                PicImagen.Image = Image.FromFile(file.FileName);    //El PicImagen va a ser igual a la imagen
                TxtImagen.Text = file.FileName.Substring(file.FileName.LastIndexOf("\\") + 1); //le indico al Textbox cual es el nombre con Substring
                this.RutaOrigen = file.FileName; // en la variable RutaOrigen voy a almacenar todo el directorio
            }
        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            BarcodeLib.Barcode Codigo = new BarcodeLib.Barcode();
            Codigo.IncludeLabel = true; //Va a mostrar el texto abajo de las barrar
            PanelCodigo.BackgroundImage = Codigo.Encode(BarcodeLib.TYPE.CODE128, TxtCodigo.Text, Color.Black, Color.White, 400, 100);
            BtnGuardarCodigo.Enabled = true;
        }

        private void BtnGuardarCodigo_Click(object sender, EventArgs e)
        {
            Image imgFinal = (Image)PanelCodigo.BackgroundImage.Clone(); //Voy a clonar el codigo de barra y se va a almacenar
                                                                           //en el objeto imgFinal

            SaveFileDialog DiaologoGuardar = new SaveFileDialog(); //abre un cuadro de dialogo 
            DiaologoGuardar.AddExtension = true;                    // permite para guardar la extension
            DiaologoGuardar.Filter = "Image PNG (*.pgn) |*.png";    //solo permite el formato PNG
            DiaologoGuardar.ShowDialog();                           //muestra el showdialog para guardar
            if(!string.IsNullOrEmpty(DiaologoGuardar.FileName))     //Si selecciono un directorio esa imagen se va a guardar en ese
            {                                                        // directorio
                imgFinal.Save(DiaologoGuardar.FileName, ImageFormat.Png);
            }
            imgFinal.Dispose();
        }

        private void BtnInsertar_Click(object sender, EventArgs e)
        {
            try
            {
                string Rpta = "";
                //Si los siguientes datos estan incompletos dara mensaje de error.
                if (CboCategoria.Text == string.Empty || TxtNombre.Text == string.Empty || TxtPrecioVenta.Text == string.Empty || TxtStock.Text == string.Empty )
                {
                    this.MensajeError("Faltan ingresar algunos datos, seran remarcados.");
                    ErrorIcono.SetError(CboCategoria, "Seleccione una categoria.");
                    ErrorIcono.SetError(TxtNombre, "Ingrese un Nombre."); //ErrorProvider
                    ErrorIcono.SetError(TxtPrecioVenta, "Ingrese un precio.");
                    ErrorIcono.SetError(TxtStock, "Ingrese un stock inicial.");
                }
                else
                {
                    Rpta = NArticulo.Insertar(Convert.ToInt32(CboCategoria.SelectedValue),TxtCodigo.Text.Trim(),TxtNombre.Text.Trim(),Convert.ToDecimal(TxtPrecioVenta.Text),Convert.ToInt32(TxtStock.Text),TxtDescripcion.Text.Trim(),TxtImagen.Text.Trim());
                    if (Rpta.Equals("OK"))
                    {
                        this.MensajeOk("Se inserto de forma correcta el registro");
                        if(TxtImagen.Text != string.Empty) //Si la imagen es distinta de nulo entonces continuar con la condicion
                        {
                            this.RutaDestino = this.Directorio + TxtImagen.Text;
                            File.Copy(this.RutaOrigen, this.RutaDestino);
                        }
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
                this.Limpiar();
                BtnActualizar.Visible = true; // para que me muestre visible solo el boton actualizar
                BtnInsertar.Visible = false;
                TxtId.Text = Convert.ToString(DgvListado.CurrentRow.Cells["ID"].Value);
                CboCategoria.SelectedValue = Convert.ToString(DgvListado.CurrentRow.Cells["IdCategoria"].Value);
                TxtCodigo.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Codigo"].Value);
                this.NombreAnt = Convert.ToString(DgvListado.CurrentRow.Cells["Nombre"].Value);
                TxtNombre.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Nombre"].Value);
                TxtPrecioVenta.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Precio_Venta"].Value);
                TxtStock.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Stock"].Value);
                TxtDescripcion.Text = Convert.ToString(DgvListado.CurrentRow.Cells["Descripcion"].Value);
                string Imagen;
                Imagen = Convert.ToString(DgvListado.CurrentRow.Cells["Imagen"].Value);
                if(Imagen != string.Empty)
                {
                    PicImagen.Image = Image.FromFile(this.Directorio + Imagen);
                    TxtImagen.Text = Imagen;
                }
                else
                {
                    PicImagen.Image = null;
                    TxtImagen.Text = "";
                }
                tabControl1.SelectedIndex = 1;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Seleccione desde la celda Nombre." + "| Error: " + ex.Message);
            }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                string Rpta = "";
                //Si los siguientes datos estan incompletos dara mensaje de error.
                if (TxtId.Text == string.Empty || CboCategoria.Text == string.Empty || TxtNombre.Text == string.Empty || TxtPrecioVenta.Text == string.Empty || TxtStock.Text == string.Empty)
                {
                    this.MensajeError("Faltan ingresar algunos datos, seran remarcados.");
                    ErrorIcono.SetError(CboCategoria, "Seleccione una categoria.");
                    ErrorIcono.SetError(TxtNombre, "Ingrese un Nombre."); //ErrorProvider
                    ErrorIcono.SetError(TxtPrecioVenta, "Ingrese un precio.");
                    ErrorIcono.SetError(TxtStock, "Ingrese un stock inicial.");
                }
                else
                {
                    Rpta = NArticulo.Actualizar(Convert.ToInt32(TxtId.Text), Convert.ToInt32(CboCategoria.SelectedValue), TxtCodigo.Text.Trim(),this.NombreAnt, TxtNombre.Text.Trim(), Convert.ToDecimal(TxtPrecioVenta.Text), Convert.ToInt32(TxtStock.Text), TxtDescripcion.Text.Trim(), TxtImagen.Text.Trim());
                    if (Rpta.Equals("OK"))
                    {
                        this.MensajeOk("Se actualizó de forma correcta el registro");
                        if (TxtImagen.Text != string.Empty && this.RutaOrigen != string.Empty) //&& se agrega condicion si ya hay una imagen ya que
                        {                                                                     //puede haber una imagen ya cargada
                            this.RutaDestino = this.Directorio + TxtImagen.Text;
                            File.Copy(this.RutaOrigen, this.RutaDestino);
                        }
                        this.Listar();
                        tabControl1.SelectedIndex = 0;
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
                    string Rpta = "";
                    string Imagen = "";
                    foreach (DataGridViewRow row in DgvListado.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            Codigo = Convert.ToInt32(row.Cells[1].Value);
                            Imagen = Convert.ToString(row.Cells[9].Value);
                            Rpta = NArticulo.Eliminar(Codigo);

                            if (Rpta.Equals("Ok"))
                            {
                                this.MensajeOk("Se elimino el registro: " + Convert.ToString(row.Cells[5].Value));
                                File.Delete(this.Directorio+Imagen);
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
                    string Rpta = "";
                    foreach (DataGridViewRow row in DgvListado.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            Codigo = Convert.ToInt32(row.Cells[1].Value);
                            Rpta = NArticulo.Desactivar(Codigo);

                            if (Rpta.Equals("Ok"))
                            {
                                this.MensajeOk("Se desactivó el registro: " + Convert.ToString(row.Cells[5].Value));
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
                Opcion = MessageBox.Show("Realmente deseas desactivar el(los) registro(s)?", "Sistema de Ventas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (Opcion == DialogResult.OK)
                {
                    int Codigo;
                    string Rpta = "";
                    foreach (DataGridViewRow row in DgvListado.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            Codigo = Convert.ToInt32(row.Cells[1].Value);
                            Rpta = NArticulo.Activar(Codigo);

                            if (Rpta.Equals("Ok"))
                            {
                                this.MensajeOk("Se activó el registro: " + Convert.ToString(row.Cells[5].Value));
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

        private void BtnReporte_Click(object sender, EventArgs e)
        {
            Reportes.FrmReporteArticulos Reportes = new Reportes.FrmReporteArticulos();
            Reportes.ShowDialog();
        }
    }
}
