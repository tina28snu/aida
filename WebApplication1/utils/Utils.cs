using System.Web;
using WebApplication1.Models;

namespace WebApplication1.utils
{
    public class Utils
    {
        public static UserModel ConnectedUSer
        {
            get { return (UserModel)HttpContext.Current.Session["User"]; }

            set { HttpContext.Current.Session["User"] = value; }
        }
    }
}