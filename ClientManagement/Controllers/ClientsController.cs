using ClientManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.IO;
using PagedList;

namespace ClientManagement.Controllers
{
    public class ClientsController : Controller
    {
        //
        // GET: /Clients/
        static String cs="Data Source=SWARNA-LT\\SQLEXPRESS;Initial Catalog=Client;Integrated Security=True";
        SqlConnection con = new SqlConnection(cs);
        static List<Client> list = new List<Client>();
        

        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageIndex = 1;
            list= new List<Client>();
            Client c=new Client();
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from client",con);
            SqlDataReader data = cmd.ExecuteReader();
            while(data.Read())
            {
                if (Convert.ToInt32(data["active"]) == 1)
                {
                    c = new Client();
                    c.company = data["company"].ToString();
                    c.gender = data["gender"].ToString();
                    c.id = Convert.ToInt32(data["id"]);
                    c.name = data["name"].ToString();
                    c.designation = data["designation"].ToString();
                    c.created = DateTime.Parse(data["created"].ToString());
                    c.updated = DateTime.Parse(data["updated"].ToString());
                    c.imagedata = (byte[])data["imagedata"];
                    list.Add(c);
                }
                else
                    continue;
            }
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Client> cl = null;
            cl = list.ToPagedList(pageIndex, pageSize);
            return View(cl);
        }
        [HttpGet]
        public ActionResult Add()
        {            
            return View("Add");
        }
        [HttpPost]
        public ActionResult FormSubmit(HttpPostedFileBase filee,Models.Client c,string Button)
        {
            if (Button == "Add")
            {
                byte[] bytes = null;
                if (filee != null)
                {
                    using (BinaryReader br = new BinaryReader(filee.InputStream))
                    {
                        bytes = br.ReadBytes(filee.ContentLength);
                    }
                }
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into client (id,name,company,gender,created,updated,designation,dob,email,department,imagedata,active) values(@id,@name,@company,@gender,@created,@updated,@design,@dob,@email,@department,@url,@a)", con);
                cmd.Parameters.AddWithValue("@id", c.id);
                cmd.Parameters.AddWithValue("@name", c.name);
                cmd.Parameters.AddWithValue("@company", c.company);
                cmd.Parameters.AddWithValue("@gender", c.gender);
                cmd.Parameters.AddWithValue("@created", DateTime.Now);
                cmd.Parameters.AddWithValue("@updated", DateTime.Now);
                cmd.Parameters.AddWithValue("@dob", c.dob);
                cmd.Parameters.AddWithValue("@email", c.email);
                cmd.Parameters.AddWithValue("@department", c.department);
                cmd.Parameters.AddWithValue("@design", c.designation);
                cmd.Parameters.AddWithValue("@a", 1);
                cmd.Parameters.AddWithValue("@url", bytes);
                if (ModelState.IsValid)
                    cmd.ExecuteNonQuery();
                else
                    return View("Add");
            }
                return RedirectToAction("Index");
        }
        public ActionResult Admin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Admin(Models.Admin a)
        {
            String uname=a.username;
            String pass=a.password;
            con.Open();
            SqlCommand cmd=new SqlCommand("select * from admin where username=@uname",con);
            cmd.Parameters.AddWithValue("@uname",uname);
            SqlDataReader data=cmd.ExecuteReader();
            //String x=data.ToString()
            data.Read();
            if (data["password"].ToString() == pass)
                return RedirectToAction("Index");
            return View();
        }
        
        //GET method
        public ActionResult Edit(int id)
        {
            Client c = new Client();
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from client where id=@id",con);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader data = cmd.ExecuteReader();
            while(data.Read())
            {
                c = new Client();
                c.company = data["company"].ToString();
                c.gender = data["gender"].ToString();
                c.id = Convert.ToInt32(data["id"]);
                c.name = data["name"].ToString();
                c.designation = data["designation"].ToString();
                c.created = DateTime.Parse(data["created"].ToString());
                c.updated = DateTime.Parse(data["updated"].ToString());
                c.email = data["email"].ToString();
                c.department = data["department"].ToString();
                c.imagedata = (byte[])data["imagedata"];
            }
            return View(c);
        }
        [HttpPost]
        public ActionResult FormEdit(Models.Client c,string submit)
        {
            if (submit == "Save")
            {
                System.Diagnostics.Debug.WriteLine("qqqqqqqqqqqqqqqqqqqqqqqqq"+submit);
                con.Open();
                SqlCommand cmd = new SqlCommand("update client set name=@name,company=@company,gender=@gender,dob=@dob,department=@department,email=@email,updated=@update where id=@id", con);
                cmd.Parameters.AddWithValue("@id", c.id);
                cmd.Parameters.AddWithValue("@company", c.company);
                cmd.Parameters.AddWithValue("@name", c.name);
                cmd.Parameters.AddWithValue("@gender", c.gender);
                cmd.Parameters.AddWithValue("@dob", c.dob);
                cmd.Parameters.AddWithValue("@department", c.department);
                cmd.Parameters.AddWithValue("@email", c.email);
                cmd.Parameters.AddWithValue("@update", DateTime.Now);
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");

            }
            else if (submit == "Cancel")
                return RedirectToAction("Index"); 
            System.Diagnostics.Debug.WriteLine("oooooooooooooooooooooooooooo"+submit);
            return View("Edit");
        }
        
        //GET method
        public ActionResult Delete(int id)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update client set active=0 where id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            //cmd.Parameters.AddWithValue("@update", DateTime.Now);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }
        public ActionResult Functions(string act,string search)
        {
            int pageSize = 5;
            int pageIndex = 1;
            System.Diagnostics.Debug.WriteLine("oooooooooooooooooooooooooooo" + search);
            if (act == "Clear")
                return RedirectToAction("Index");
            else if (act == "Search")
            {
                Client c = new Client();
                list = new List<Client>();
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from client where name like '%' + @search + '%'", con);
                cmd.Parameters.AddWithValue("@search", search);
                SqlDataReader data = cmd.ExecuteReader();
                while (data.Read())
                {
  
                        c = new Client();
                        c.company = data["company"].ToString();
                        c.gender = data["gender"].ToString();
                        c.id = Convert.ToInt32(data["id"]);
                        c.name = data["name"].ToString();
                        c.designation = data["designation"].ToString();
                        c.created = DateTime.Parse(data["created"].ToString());
                        c.updated = DateTime.Parse(data["updated"].ToString());
                        c.imagedata = (byte[])data["imagedata"];
                        list.Add(c);
                   
                }
                pageIndex = 1;
                IPagedList<Client> cl = null;
                cl = list.ToPagedList(pageIndex, pageSize);
                return RedirectToAction("Index", cl);
            }
            else
                return RedirectToAction("Add");
        }
    }
}
