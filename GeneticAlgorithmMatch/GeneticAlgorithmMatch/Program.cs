using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GeneticAlgorithmMatch
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = System.Environment.CurrentDirectory;
            string team_file = path+"\\ResourceFile\\队伍.txt";
            string time_file = path+"\\ResourceFile\\场地开放时间.txt";
            string venue_file = path+"\\ResourceFile\\场地距离.txt";

            bool flag = true;
            GAMatch match = new GAMatch(team_file, time_file, venue_file);
            while(flag)
            {
                Console.Write("是否按默认设置(Yes/No):");
                string ans=Console.ReadLine();
                ans = ans.ToLower();
                if (ans == "y" || ans == "yes")
                    match.createScheduleUsingGeneticAlgorithm(100, 60, 0.1, 100);
                else
                {
                    Console.Write("输入种群规模：");
                    int population=Convert.ToInt32(Console.ReadLine());
                    Console.Write("输入种群存活率(0-100的整数)：");
                    int sur_rate= Convert.ToInt32(Console.ReadLine());
                    Console.Write("输入范围为[0,1]的种群突变率（剩下的发生基因重组）：");
                    double mutate_rate = Convert.ToDouble(Console.ReadLine());
                    Console.Write("输入要进化迭代的代数：");
                    int genera_num = Convert.ToInt32(Console.ReadLine());
                    match.createScheduleUsingGeneticAlgorithm(population, sur_rate, mutate_rate, genera_num);
                }
                Console.WriteLine("是否再次运行算法（Yes/No）：");
                string tmp = Console.ReadLine();
                tmp = tmp.ToLower();
                if (tmp == "y" || tmp == "yes")
                    flag = true;
                else
                    flag = false;
            }
        }
    }
}
