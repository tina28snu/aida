using System;
using System.Data.SqlClient;

namespace WebApplication1.Models
{
    public class UserModel
    {
        public int idUser { get; set; }
        public string login { get; set; }
        public string password { get; set; }

        public int ConnectUser()
        {
            int idUserToFind = -1;
            SqlConnection oConn = new SqlConnection();
            oConn.ConnectionString = @"";
            try
            {
                oConn.Open();
                SqlCommand oCmd = new SqlCommand(@"SELECT idUser FROM Users WHERE login = @login AND password = @password", oConn);
                SqlParameter paramCheckLogin = new SqlParameter("@login", this.login);
                SqlParameter paramCheckPassword = new SqlParameter("@password", this.password);

                oCmd.Parameters.Add(paramCheckLogin);
                oCmd.Parameters.Add(paramCheckPassword);

                SqlDataReader odr = oCmd.ExecuteReader();
                if (odr.HasRows)
                {
                    odr.Read();
                    idUserToFind = (int)odr[0];
                    odr.Close();
                }
                else
                {
                    idUserToFind = -1;
                }
                oConn.Close();
            }
            catch (Exception)
            {
                idUserToFind = -1;
            }
            return idUserToFind;
        }
    }
}