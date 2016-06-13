using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmMatch
{
    /// <summary>
    /// 读取场地、时间、队伍文件信息
    /// </summary>
    class LoadFile
    {
        /// <summary>
        /// 读取队伍信息到列表中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<string> loadFromTeamFile(string file)
        {
            List<string> teams = new List<string>();
            try
            {
                StreamReader sr = new StreamReader(file, System.Text.Encoding.Default);
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    teams.Add(line.Trim());
                }
                sr.Close();
            }
            catch
            {
                Console.WriteLine("队伍文件打开错误!");
            }
            return teams;
        }
        /// <summary>
        /// 读取时间信息到队伍中
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<TimeSlot> loadFromTimeFile(string file)
        {
            List<TimeSlot> time = new List<TimeSlot>();
            try
            {
                StreamReader sr = new StreamReader(file, System.Text.Encoding.Default);
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] info = line.Split('-');
                    string venue = info[0].Trim();

                    string[] date = info[1].Trim().Split('/');
                    int month = (int)Double.Parse(date[0].Trim()) - 1;
                    int day = (int)Double.Parse(date[1].Trim());
                    int year = (int)Double.Parse(date[2].Trim());

                    string[] specificTime = info[2].Trim().Split(':');
                    int hour = (int)Double.Parse(specificTime[0].Trim());
                    int minute = (int)Double.Parse(specificTime[1].Trim());
                  
                    //CultureInfo cI = new CultureInfo("en-US", false);
                    DateTime dt = new DateTime(year, month, day, hour, minute, 0/* ,cI.Calendar*/);
                    TimeSlot tS = new TimeSlot(dt, venue);
                    time.Add(tS);
                }
                sr.Close();
            }
            catch
            {
                Console.WriteLine("场地开放时间文件打开错误!");
            }
            return time;
        }
        /// <summary>
        /// 读取场地距离信息
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<Distance> loadFromDistanceFile(string file)
        {
            List<Distance> dis = new List<Distance>();
            try
            {
                StreamReader sr = new StreamReader(file, System.Text.Encoding.Default);
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] info = line.Split('-');
                    string venue_1 = info[0].Trim();
                    string venue_2 = info[1].Trim();
                    double distance = Double.Parse(info[2].Trim());
                    Distance d = new Distance(venue_1, venue_2, distance);
                    dis.Add(d);
                }
                sr.Close();
            }
            catch
            {
                Console.WriteLine("场地距离文件打开错误!");
            }
            return dis;
        }
    }
}
