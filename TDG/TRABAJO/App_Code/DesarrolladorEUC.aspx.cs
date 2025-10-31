
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class desarrollador : System.Web.UI.Page
{
    // Obtener cadena de conexión desde Web.config
    private static string connString = ConfigurationManager.ConnectionStrings["PoliticasEUC"].ConnectionString;

    // ============================
    // CRUD para EUC
    // ============================

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

    // ============================
    // CRUD para Plan de Automatización
    // ============================

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
}
