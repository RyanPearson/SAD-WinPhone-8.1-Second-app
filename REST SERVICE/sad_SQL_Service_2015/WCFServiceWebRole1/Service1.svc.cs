using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WCFServiceWebRole1
{
    public class Service1 : IService1
    {
        public void addToList(Guid guid, string userid, string title, string task)
        {
            //  Adds a new task to the list_item table
            using (var context = new myTestDBEntities1())
            {
                context.list_item.Add(new list_item()
                {
                    guid = guid,
                    userid = userid,
                    title = title,
                    task = task
                });
                context.SaveChanges();
            }
        }
        public List_Item[] viewListJSON(string guid, string userid)
        {
            //takes parameters from html request, searches the table and returns json object
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection thisConnection = new SqlConnection(connectionString);
            SqlCommand getList = thisConnection.CreateCommand();
            List<List_Item> listItems = new List<List_Item>();
            thisConnection.Open();
            string sql = "select * from list_item where guid=@guid and userid=@userid";
            getList.Parameters.AddWithValue("@guid", guid);
            getList.Parameters.AddWithValue("@userid", userid);
            getList.CommandText = sql;
            SqlDataAdapter da = new SqlDataAdapter(getList);
            DataSet ds = new DataSet();
            da.Fill(ds, "list_item");
            foreach (DataRow dr in ds.Tables["list_item"].Rows)
            {
                listItems.Add(new List_Item()
                {
                    Id = Convert.ToInt32(dr[0]),
                    Guid = Convert.ToString(dr[1]),
                    Task = Convert.ToString(dr[2]),
                    Title = Convert.ToString(dr[3]),
                    UserID = Convert.ToString(dr[4])
                });
            }
            thisConnection.Close();
            return listItems.ToArray();
        }   
    }
}