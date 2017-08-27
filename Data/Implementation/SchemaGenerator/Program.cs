using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.IO;

namespace SchemaGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration cfg = new Configuration().Configure();
            
            using (StreamWriter writer = File.CreateText("schema-script.txt"))
            {
                new SchemaExport(cfg).Create(writer, false);
            }
        }
    }
}
