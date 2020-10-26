using System;
using System.Data;
using System.Windows.Forms;
using Sistema.Negocio;

namespace Sistema.Presentacion
{
    public partial class FrmVenta : Form
    {
      private DataTable DtDetalle = new DataTable();
        public FrmVenta()
        {
            InitializeComponent();
        }
        private void Listar()
        {
            try
            {
                DgvListado.DataSource = NVenta.Listar();
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
                DgvListado.DataSource = NVenta.Buscar(TxtBuscar.Text);
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
            TxtIdCliente.Clear();
            TxtNombreCliente.Clear();
            TxtSerieComprobante.Clear();
            TxtNumComprobante.Clear();
            DtDetalle.Clear();
            TxtSubTotal.Text = "0.00";
            TxtTotalImpuesto.Text = "0.00";
            TxtTotal.Text = "0.00";


            ErrorIcono.Clear(); //El errorprovider va a limpiar los textbox

            //van a estar estos botones no visibles al principio
            DgvListado.Columns[0].Visible = false;
            BtnAnular.Visible = false;
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
        private void CrearTabla()
        {   //Estas columnas se van a agregar en el DataTable
            this.DtDetalle.Columns.Add("idarticulo", System.Type.GetType("System.Int32"));
            this.DtDetalle.Columns.Add("codigo", System.Type.GetType("System.String"));
            this.DtDetalle.Columns.Add("articulo", System.Type.GetType("System.String"));
            this.DtDetalle.Columns.Add("stock", System.Type.GetType("System.Int32"));
            this.DtDetalle.Columns.Add("cantidad", System.Type.GetType("System.Int32"));
            this.DtDetalle.Columns.Add("precio", System.Type.GetType("System.Decimal"));
            this.DtDetalle.Columns.Add("descuento", System.Type.GetType("System.Decimal"));
            this.DtDetalle.Columns.Add("importe", System.Type.GetType("System.Decimal"));

            //La fuente de informacion que va a obtener el DgvDetalle viene del DtDetalle
            DgvDetalle.DataSource = this.DtDetalle;

            //Propiedades
            DgvDetalle.Columns[0].Visible = false;
            DgvDetalle.Columns[1].HeaderText = "CODIGO";
            DgvDetalle.Columns[1].Width = 100;
            DgvDetalle.Columns[2].HeaderText = "ARTICULO";
            DgvDetalle.Columns[2].Width = 200;
            DgvDetalle.Columns[3].HeaderText = "STOCK";
            DgvDetalle.Columns[3].Width = 50;
            DgvDetalle.Columns[4].HeaderText = "CANTIDAD";
            DgvDetalle.Columns[4].Width = 50;
            DgvDetalle.Columns[5].HeaderText = "PRECIO";
            DgvDetalle.Columns[5].Width = 70;
            DgvDetalle.Columns[6].HeaderText = "DESCUENTO";
            DgvDetalle.Columns[6].Width = 60;
            DgvDetalle.Columns[7].HeaderText = "IMPORTE";
            DgvDetalle.Columns[7].Width = 80;

            //Columnas de solo lectura
            //Datos que no se van a poder modificar
            DgvDetalle.Columns[1].ReadOnly = true;  //IDArticulo
            DgvDetalle.Columns[2].ReadOnly = true;  //Articulo
            DgvDetalle.Columns[3].ReadOnly = true;  //Stock
            DgvDetalle.Columns[7].ReadOnly = true;  //Importe
        }
        private void Formato()
        {
            DgvListado.Columns[0].Visible = false;
            DgvListado.Columns[1].Visible = false;
            DgvListado.Columns[2].Visible = false;
            DgvListado.Columns[0].Width = 100;  //Seleccionar
            DgvListado.Columns[3].Width = 150;  //
            DgvListado.Columns[4].Width = 150;  //Usuario
            DgvListado.Columns[5].Width = 100;
            DgvListado.Columns[5].HeaderText = "Documento";
            DgvListado.Columns[6].Width = 70;
            DgvListado.Columns[6].HeaderText = "Serie";
            DgvListado.Columns[7].Width = 70;
            DgvListado.Columns[7].HeaderText = "Número";
            DgvListado.Columns[8].Width = 60;
            DgvListado.Columns[9].Width = 100;
            DgvListado.Columns[10].Width = 100;
            DgvListado.Columns[11].Width = 100;
        }
        private void FormatoArticulos()
        {
            DgvArticulos.Columns[1].Visible = false;
            DgvArticulos.Columns[2].Width = 100;
            DgvArticulos.Columns[2].HeaderText = "Categoria";
            DgvArticulos.Columns[3].Width = 100;
            DgvArticulos.Columns[3].HeaderText = "Código";
            DgvArticulos.Columns[4].Width = 150;
            DgvArticulos.Columns[5].Width = 100;
            DgvArticulos.Columns[5].HeaderText = "Precio Venta";
            DgvArticulos.Columns[6].Width = 60;
            DgvArticulos.Columns[7].Width = 200;
            DgvArticulos.Columns[7].HeaderText = "Descripción";
            DgvArticulos.Columns[8].Width = 100;
        }

        private void FrmVenta_Load(object sender, EventArgs e)
        {
            this.Listar();
            this.CrearTabla();
        }
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }
        private void BtnBuscarCliente_Click(object sender, EventArgs e)
        {
            FrmVista_ClienteVenta vista = new FrmVista_ClienteVenta();
            vista.ShowDialog(); //ShowDialog permite que si el foco esta en este formulario no puedo
                                //regresar al anterior hasta que se cierre 
            TxtIdCliente.Text = Convert.ToString(Variables.IdCliente);
            TxtNombreCliente.Text = Variables.NombreCliente;
        }
        private void TxtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            //En el parametro "e" se obtiene la tecla presionada
            try
            {
                if (e.KeyCode == Keys.Enter)  // en este If evaluo si el usuario presiona Enter
                {
                    DataTable Tabla = new DataTable();
                    Tabla = NArticulo.BuscarCodigoVenta(TxtCodigo.Text.Trim());
                    //Voy a consultar si existe el articulo 
                    if (Tabla.Rows.Count <= 0)
                    {
                        this.MensajeError("No existe articulo con ese codigo de barrar o no hay stock de ese articulo");
                    }
                    else
                    // El orden de las columnas se da en el Procedimiento Almacenado articulo_buscar_precio_venta
                    {                           //ID                                Codigo                              Nombre                            Stock                                           Precio
                        this.AgregarDetalle(Convert.ToInt32(Tabla.Rows[0][0]), Convert.ToString(Tabla.Rows[0][1]), Convert.ToString(Tabla.Rows[0][2]), Convert.ToInt32(Tabla.Rows[0][4]), Convert.ToDecimal(Tabla.Rows[0][3]));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AgregarDetalle(int IdArticulo, string Codigo, string Nombre, int Stock, decimal Precio)
        {
            bool Agregar = true; // va a indicar que permita agregar articulos al detalle
            foreach (DataRow FilaTemp in DtDetalle.Rows)  //Declaro un objeto DataRow llamado FilaTemp de fila temporal
                                                          //y le indico que corra el listado DtDetalle que recorra todas las filas
                                                          // va a recorrer todos los detalles en el DGV y dentro de ese recorrido se va
                                                          //a identificar si ese articulo ya se encuentra agregado
            {
                if (Convert.ToInt32(FilaTemp["idarticulo"]) == IdArticulo)
                {
                    Agregar = false; // indico que no agregue 
                    this.MensajeError("El articulo ya a sido agregado. ");
                }
            }
            //Si no esta agregado lo va a agregar
            if (Agregar)
            {
                DataRow Fila = DtDetalle.NewRow();
                Fila["idarticulo"] = IdArticulo;
                Fila["codigo"] = Codigo;
                Fila["articulo"] = Nombre;
                Fila["stock"] = Stock;
                Fila["cantidad"] = 1;
                Fila["precio"] = Precio;
                Fila["descuento"] = 0;
                Fila["importe"] = Precio;
                this.DtDetalle.Rows.Add(Fila);
                this.CalcularTotales();
            }

        }
        private void CalcularTotales()
        {
            decimal Total = 0;
            decimal SubTotal = 0;
            if (DgvDetalle.Rows.Count == 0) //Si en el DgvDetalle tengo cero filas no va a recorrerlo
            {
                Total = 0;
            }
            else            //Si tengo al menos una fila
            {
                foreach (DataRow FilaTemp in DtDetalle.Rows) //va a empezar a recorrer todos los articulos
                {
                    //Total va a ser un acumulador
                    Total = Total + Convert.ToDecimal(FilaTemp["importe"]);
                }
            }
            SubTotal = Total / (1 + Convert.ToDecimal(TxtImpuesto.Text));
            TxtTotal.Text = Total.ToString("#0.00#"); //Le podemos dar el siguiente formato .ToString("#0.00#")
            TxtSubTotal.Text = SubTotal.ToString("#0.00#");
            TxtTotalImpuesto.Text = (Total - SubTotal).ToString("#0.00#");
        }

        private void BtnVerArticulos_Click(object sender, EventArgs e)
        {
            PanelArticulos.Visible = true;
        }

        private void BtnCerrarArticulos_Click(object sender, EventArgs e)
        {
            PanelArticulos.Visible = false;
        }

        private void BtnFiltrarArticulos_Click(object sender, EventArgs e)
        {
            try
            {
                DgvArticulos.DataSource = NArticulo.BuscarVenta(TxtBuscarArticulo.Text.Trim());
                this.FormatoArticulos();
                LblTotalArticulos.Text = "Total Registros: " + Convert.ToString(DgvArticulos.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DgvArticulos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int IdArticulo;
            string Codigo, Nombre;
            decimal Precio;
            int Stock;
            IdArticulo = Convert.ToInt32(DgvArticulos.CurrentRow.Cells["ID"].Value);
            Codigo = Convert.ToString(DgvArticulos.CurrentRow.Cells["Codigo"].Value);
            Nombre = Convert.ToString(DgvArticulos.CurrentRow.Cells["Nombre"].Value);
            Stock = Convert.ToInt32(DgvArticulos.CurrentRow.Cells["Stock"].Value);
            Precio = Convert.ToDecimal(DgvArticulos.CurrentRow.Cells["Precio_Venta"].Value);
            this.AgregarDetalle(IdArticulo, Codigo, Nombre, Stock, Precio);
        }

        private void DgvDetalle_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
                //El objeto Fila va a ser la fila donde se encuentra la celda que estoy modificando
                //Vamos a poder modificar la Cantidad, el Precio y el Descuento
            DataRow Fila = (DataRow)DtDetalle.Rows[e.RowIndex];
            //en la celda articulo voy a obtener ese valor y almacenarlo en la variable Articulo
            string Articulo = Convert.ToString(Fila["articulo"]);
            int Cantidad = Convert.ToInt32(Fila["cantidad"]);
            int Stock = Convert.ToInt32(Fila["stock"]);
            decimal Precio = Convert.ToDecimal(Fila["precio"]);
            decimal Descuento = Convert.ToDecimal(Fila["descuento"]);

            //condicion si la cantidad es mayor que el stock voy a mostradr un mensaje y a
            //dejar la cantidad = al stock
            if(Cantidad > Stock)
            {
                Cantidad = Stock;
                this.MensajeError("La cantidad de venta del articulo " + Articulo + " supera el Stock disponible " + Stock);
                Fila["cantidad"] = Cantidad;
                //Si llegara a superar el Stock ese Stock va a ser la nueva Cantidad
            }
            //Calculo el importe
            Fila["importe"] = (Precio * Cantidad) - Descuento;
            this.CalcularTotales();
        }

        private void BtnInsertar_Click(object sender, EventArgs e)
        {
            try
            {
                string Rpta = "";
                //Voy a validar el idcliente, impuesto....                                                          //Se valida si el DtDetalle tiene 0 filas
                if (TxtIdCliente.Text == string.Empty || TxtImpuesto.Text == string.Empty || TxtNumComprobante.Text == string.Empty || DtDetalle.Rows.Count == 0)
                {
                    //Si no se valida saltaran los siguientes errores
                    this.MensajeError("Faltan ingresar algunos datos, seran remarcados.");
                    ErrorIcono.SetError(TxtIdCliente, "Seleccione un Cliente"); //ErrorProvider
                    ErrorIcono.SetError(TxtImpuesto, "Ingrese un impuesto");
                    ErrorIcono.SetError(TxtNumComprobante, "Ingrese el número de comprobante");
                    ErrorIcono.SetError(DgvDetalle, "Debe tener al menos un detalle");
                }
                else
                {
                    Rpta = NVenta.Insertar(Convert.ToInt32(TxtIdCliente.Text), Variables.IdUsuario, CboComprobante.Text, TxtSerieComprobante.Text.Trim(), TxtNumComprobante.Text.Trim(), Convert.ToDecimal(TxtImpuesto.Text), Convert.ToDecimal(TxtTotal.Text), DtDetalle);
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
            tabControl1.SelectedIndex = 0;
        }

        private void DgvListado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DgvMostrarDetalle.DataSource = NVenta.ListarDetalle(Convert.ToInt32(DgvListado.CurrentRow.Cells["ID"].Value));
                decimal Total, SubTotal;
                decimal Impuesto = Convert.ToDecimal(DgvListado.CurrentRow.Cells["Impuesto"].Value);
                Total = Convert.ToDecimal(DgvListado.CurrentRow.Cells["Total"].Value);
                SubTotal = Total / (1 + Impuesto);

                TxtSubtotalD.Text = SubTotal.ToString("#0.00#");
                TxtTotalImpuestoD.Text = (Total - SubTotal).ToString("#0.00#");
                TxtTotalD.Text = Total.ToString("#0.00#");
                PanelMostrar.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCerrarDetalle_Click(object sender, EventArgs e)
        {
            PanelMostrar.Visible = false;
        }

        private void BtnAnular_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Opcion;     //Se crea la Variable Opcion para guardar el resultado de la seleccion Cancelar/Ok
                //Para un cartel con los botones cancelar y Ok
                Opcion = MessageBox.Show("Realmente deseas anular el(los) registro(s)?", "Sistema de Ventas", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (Opcion == DialogResult.OK)
                {
                    int Codigo;
                    string Rpta;
                    foreach (DataGridViewRow row in DgvListado.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            Codigo = Convert.ToInt32(row.Cells[1].Value);
                            Rpta = NVenta.Anular(Codigo);

                            if (Rpta.Equals("Ok"))
                            {
                                this.MensajeOk("Se anuló el registro: " + Convert.ToString(row.Cells[6].Value) + "-" + Convert.ToString(row.Cells[7].Value));
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
                BtnAnular.Visible = true;
            }
            else  //Si no esta tildado los botones y columna seleccionar no se van a mostrar
            {
                DgvListado.Columns[0].Visible = false;
                BtnAnular.Visible = false;
            }
        }

        private void BtnComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                Variables.IdVenta = Convert.ToInt32(DgvListado.CurrentRow.Cells["ID"].Value);
                Reportes.FrmReporteComprobanteVenta reporte = new Reportes.FrmReporteComprobanteVenta();
                reporte.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
