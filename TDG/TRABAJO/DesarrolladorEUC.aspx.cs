
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;

namespace TRABAJO
{
    public partial class DesarrolladorEUC : System.Web.UI.Page
    {
       
        // Obtener cadena de conexión desde Web.config
        private static string connString = ConfigurationManager.ConnectionStrings["PoliticasEUC"].ConnectionString;



        // [WebMethod]
        // public static object CreateEUC(string nombre, string descripcion, string criticidad, string estado) { /* return { ok=true, id=... } */ }

        // [WebMethod]
        // public static object UpdateEUC(int id, string nombre, string descripcion, string criticidad, string estado) { /* ok */ }


        // [WebMethod]
        // public static List<object> GetEUCs()  // devuelve [{EUCID,Nombre,Descripcion,Criticidad,Estado, Plan?, Doc?}]


        // [WebMethod]
        // public static object AddPlan(int idEUC, string responsable, string plan)  // return { ok=true, idPlan=... }

        // [WebMethod]
        // public static object AddDocumentacion(int idEUC, string nombreEUC, string proposito, string proceso, string uso, string insumos, string responsable, string docTecnica, string evControl) // { ok=true, idDoc=... }

        // [WebMethod]
        // public static object DeleteEUC(int id)
        // {
        //   using (var conn = new SqlConnection(connString))
        //    using (var tx = conn.BeginTransaction())
        //    {
        // DELETE FROM PlanAutomatizacion WHERE EUCID=@Id
        // DELETE FROM Documentacion WHERE EUCID=@Id
        // DELETE FROM EUC WHERE EUCID=@Id
        //        tx.Commit();
        //   }
        //  return new { ok = true };
        // }

        [WebMethod]
        public static string CreateEUC(string nombre, string descripcion, string criticidad)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "INSERT INTO EUC (Nombre, Descripcion, Criticidad, Estado) VALUES (@Nombre, @Descripcion, @Criticidad, 'Incompleto')";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.Parameters.AddWithValue("@Criticidad", criticidad);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return "EUC creada correctamente";
        }

        [WebMethod]
        public static string UpdateEUC(int id, string nombre, string descripcion, string criticidad)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE EUC SET Nombre=@Nombre, Descripcion=@Descripcion, Criticidad=@Criticidad WHERE EUCID=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.Parameters.AddWithValue("@Criticidad", criticidad);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return "EUC actualizada correctamente";
        }

        [WebMethod]
        public static string DeleteEUC(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "DELETE FROM EUC WHERE EUCID=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return "EUC eliminada correctamente";
        }

        [WebMethod]
        public static string AddPlan(int idEUC, string responsable, string plan)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "INSERT INTO PlanAutomatizacion (EUCID, Responsable, [Plan]) VALUES (@EUCID, @Responsable, @Plan)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EUCID", idEUC);
                cmd.Parameters.AddWithValue("@Responsable", responsable);
                cmd.Parameters.AddWithValue("@Plan", plan);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return "Plan agregado correctamente";
        }

        [WebMethod]
        public static string UpdatePlan(int idPlan, string responsable, string plan)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "UPDATE PlanAutomatizacion SET Responsable=@Responsable, [Plan]=@Plan WHERE IdPlan=@IdPlan";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdPlan", idPlan);
                cmd.Parameters.AddWithValue("@Responsable", responsable);
                cmd.Parameters.AddWithValue("@Plan", plan);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return "Plan actualizado correctamente";
        }

        [WebMethod]
        public static string DeletePlan(int idPlan)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "DELETE FROM PlanAutomatizacion WHERE IdPlan=@IdPlan";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdPlan", idPlan);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return "Plan eliminado correctamente";
        }

        // ============================
        // CRUD para Documentación
        // ============================

        [WebMethod]
        public static string AddDocumentacion(int idEUC, string nombreEUC, string proposito, string proceso, string uso, string insumos, string responsable, string docTecnica, string evControl)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"INSERT INTO Documentacion (EUCID, NombreEUC, Proposito, Proceso, Uso, Insumos, Responsable, DocTecnica, EvControl)
                             VALUES (@EUCID, @NombreEUC, @Proposito, @Proceso, @Uso, @Insumos, @Responsable, @DocTecnica, @EvControl)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EUCID", idEUC);
                cmd.Parameters.AddWithValue("@NombreEUC", nombreEUC);
                cmd.Parameters.AddWithValue("@Proposito", proposito);
                cmd.Parameters.AddWithValue("@Proceso", proceso);
                cmd.Parameters.AddWithValue("@Uso", uso);
                cmd.Parameters.AddWithValue("@Insumos", insumos);
                cmd.Parameters.AddWithValue("@Responsable", responsable);
                cmd.Parameters.AddWithValue("@DocTecnica", docTecnica);
                cmd.Parameters.AddWithValue("@EvControl", evControl);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return "Documentación agregada correctamente";
        }

        [WebMethod]
        public static string UpdateDocumentacion(int idDoc, string nombreEUC, string proposito, string proceso, string uso, string insumos, string responsable, string docTecnica, string evControl)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"UPDATE Documentacion SET NombreEUC=@NombreEUC, Proposito=@Proposito, Proceso=@Proceso, Uso=@Uso, Insumos=@Insumos, Responsable=@Responsable, DocTecnica=@DocTecnica, EvControl=@EvControl
                             WHERE IDoc=@IDoc";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDoc", idDoc);
                cmd.Parameters.AddWithValue("@NombreEUC", nombreEUC);
                cmd.Parameters.AddWithValue("@Proposito", proposito);
                cmd.Parameters.AddWithValue("@Proceso", proceso);
                cmd.Parameters.AddWithValue("@Uso", uso);
                cmd.Parameters.AddWithValue("@Insumos", insumos);
                cmd.Parameters.AddWithValue("@Responsable", responsable);
                cmd.Parameters.AddWithValue("@DocTecnica", docTecnica);
                cmd.Parameters.AddWithValue("@EvControl", evControl);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return "Documentación actualizada correctamente";
        }

        [WebMethod]
        public static string DeleteDocumentacion(int idDoc)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "DELETE FROM Documentacion WHERE IDoc=@IDoc";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IDoc", idDoc);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return "Documentación eliminada correctamente";
        }

        protected global::System.Web.UI.WebControls.GridView gvEUC;
        private void BindEUCs()
        {
            // Usa la misma cadena que ya usas al insertar
            string connectionString = "Data Source=localhost;Initial Catalog=PoliticasEUC;Integrated Security=True";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(
                @"SELECT EUCID, Nombre, Descripcion, Criticidad, Estado 
          FROM dbo.EUC
          ORDER BY EUCID DESC;", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                conn.Open();
                da.Fill(dt);
                gvEUC.DataSource = dt;
                gvEUC.DataBind();
            }
        }

        protected global::System.Web.UI.WebControls.TextBox txtNombre;
        protected global::System.Web.UI.WebControls.TextBox txtDescripcion;
        protected global::System.Web.UI.WebControls.DropDownList ddlCriticidad;
        protected global::System.Web.UI.WebControls.DropDownList ddlEstado;
        protected global::System.Web.UI.WebControls.Button btnGuardarEUC;

        protected void btnGuardarEUC_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();
            string criticidad = ddlCriticidad.SelectedValue;
            string estado = ddlEstado.SelectedValue;

            // Validación básica
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(descripcion))
            {
                // Mostrar mensaje de error (puedes usar un Label o Script)
                return;
            }

        // Conexión a SQL Server
        string connectionString = "Data Source=localhost;Initial Catalog=PoliticasEUC;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO EUC (Nombre, Descripcion, Criticidad, Estado)
                         VALUES (@Nombre, @Descripcion, @Criticidad, @Estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                cmd.Parameters.AddWithValue("@Criticidad", criticidad);
                cmd.Parameters.AddWithValue("@Estado", estado);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Cerrar modal y refrescar la página
            ScriptManager.RegisterStartupScript(this, GetType(), "CerrarModal", "$('#mdlEucNuevo').modal('hide');", true);
        }
    }

}