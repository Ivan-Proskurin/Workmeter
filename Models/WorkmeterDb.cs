using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace Workmeter.Models
{
    public class WorkmeterDb
    {
        public List<WorkmeterTask> Tasks { get; set; }

        public void Save()
        {
            SaveDb(this);
        }

        private static WorkmeterDb _instance;
        public static WorkmeterDb Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = LoadDb();
                return _instance;
            }
        }

        public static string DbPath =>
            Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) ?? string.Empty, "tasks.xml");

        public static void CreateDb()
        {
            SaveDb(new WorkmeterDb());
        }

        public static void SaveDb(WorkmeterDb db)
        {
            using (var fs = new FileStream(DbPath, FileMode.Create))
            {
                var xs = new XmlSerializer(typeof(WorkmeterDb));
                xs.Serialize(fs, db);
            }
        }

        public static WorkmeterDb LoadDb()
        {
            if (!File.Exists(DbPath))
            {
                CreateDb();
            }
            using (var fs = new FileStream(DbPath, FileMode.Open))
            {
                var xs = new XmlSerializer(typeof(WorkmeterDb));
                return xs.Deserialize(fs) as WorkmeterDb;
            }
        }
    }
}