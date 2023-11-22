using Npgsql;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelTasks
{
    class Program
    {
        private static object shortDateString;

        public static void Main(string[] args)
        {

            try{ 
            int i = 0;
            //
            Parallel.For(0, 20000,
                         index => {
                             i++;
                             doStuff("Task" + i.ToString());
                             
                         });
            }
            catch(Exception ex) {
               Console.WriteLine(ex.Message);

            }
            //Console.WriteLine("Directory '{0}':", args[0]);

            }
        private static NpgsqlConnection GetConnection()
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;
            string ConnStr = connection;
            return new NpgsqlConnection(ConnStr);

        }
        public static void doStuff(string strName)
        {

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {/* label1.Text = "Connected";*/ }
                var cmd2 = new NpgsqlCommand("INSERT INTO COMPANY ( NAME,AGE,ADDRESS,SALARY) VALUES ('Jan', 32, 'California', 28000.00 );", conn);
                var da = new NpgsqlDataAdapter(cmd2);
                var ds = new DataSet();
                NpgsqlDataReader dr = cmd2.ExecuteReader();

                dr.Close();
                conn.Close();

            }

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();

                var currentCulture = Thread.CurrentThread.CurrentCulture;
                try
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-us");
                    
                    var strTime = DateTime.Now.ToLongTimeString();
                    var strDateString = DateTime.Now.ToShortDateString().Replace("/ ", "-");

                    // Do something with shortDateString...
                }
                finally
                {
                    Thread.CurrentThread.CurrentCulture = currentCulture;
                }

                var dtNow = new DateTime();
                string TaskID = "";

                Random rnd = new Random();

                int rndNum = rnd.Next(1000000);

                var shortDateString = dtNow.ToString();
                var strSQL = @"insert into TaskResults(TaskID, TaskName, TaskDate) values('";
                strSQL += rndNum.ToString() + "','" + strName.ToString();
                strSQL += "','" + shortDateString + "'" + ")";

                var cmd2 = new NpgsqlCommand(strSQL, conn);
                int nNoAdded = cmd2.ExecuteNonQuery();

                conn.Close();

            }


            //MessageBox.Show(strName);

            Thread.Yield();

        }
    }
}



