using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace AndroidWebService
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Wenn der Aufruf dieses Webdiensts aus einem Skript zulässig sein soll, heben Sie mithilfe von ASP.NET AJAX die Kommentarmarkierung für die folgende Zeile auf. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        //login

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void Login(string UserName, string Password)  ///Tag =0 devele 1 is add
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();

            string Message = "";
            try
            {

                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-R6THQR0\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("Insert into Login(UserName,Password)values(@UserName,@Password) ")
                    {
                        CommandType = CommandType.Text,
                        Connection = connection
                    };
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    Message = " data is added";
                }
            }
            catch (Exception ex)
            {
                Message = " cannot access to the data";
            }

            var jsonData = new
            {
                Message = Message

            };
            HttpContext.Current.Response.Write(ser.Serialize(jsonData));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public void List()  ///Tag =0 devele 1 is add
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string UserID = "";
            string Message = "";
            try
            {
                SqlDataReader reader;
                using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-R6THQR0\\SQLEXPRESS;Initial Catalog=Users;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * from Login")
                    {
                        CommandType = CommandType.Text,
                        Connection = connection
                    };
                    connection.Open();
                    reader = cmd.ExecuteReader();
                    
                    Users[] user = new Users[reader.FieldCount-1];

                    int i = 0;
                    while (reader.Read())
                    {
                        user[i] = new Users(reader.GetString(1), reader.GetString(2));
                        i++;
                    }


                    HttpContext.Current.Response.Write(ser.Serialize(user));

                    reader.Close();
                    connection.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                Message = " cannot access to the data";
            }

            var jsonData = new
            {
                Message = Message
            };
            HttpContext.Current.Response.Write(ser.Serialize(jsonData));
        }

        class Users 
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public Users(string UserName, string Password)
            {
                this.UserName = UserName;
                this.Password = Password;
            }
        
        }


    }
}
