using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace CSharpMySQLConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectStr = "server=127.0.0.1;port=3306;database=mygamedb;user=root;password=123456;";

            MySqlConnection conn = new MySqlConnection(connectStr);

            try
            {
                conn.Open();
                Console.WriteLine("已建立连接");

                string sql = "select * from users";
                MySqlCommand cmd = new MySqlCommand(sql,conn);
                //cmd.ExecuteReader();//执行查询
                //cmd.ExecuteNonQuery();//插入删除
                //cmd.ExecuteScalar();//执行查询，返回一个值

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString());
                }
                Console.WriteLine("数据库读取完成");

                reader.Close();

                //sql = "insert into users(username,password) values('yyyyy','234')";
                //sql = "update users set username='ssssss',password='111111' where id=4";
                //sql = "delete from users where id=4";
                //cmd = new MySqlCommand(sql,conn);
                //int result = cmd.ExecuteNonQuery();
                //if (result == 1)
                //{
                //    Console.WriteLine("数据库插入完成");
                //}
                //else
                //{
                //    Console.WriteLine("数据库插入失败");
                //}
                sql = "select count(*) from users";
                cmd = new MySqlCommand(sql,conn);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                Console.WriteLine("获取数据为" + count);


                Console.WriteLine("数据库操作完成");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                conn.Close();
            }

            Console.ReadKey();
        }
    }
}
