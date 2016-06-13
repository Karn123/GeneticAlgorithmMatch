using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmMatch
{
    /// <summary>
    /// 排比赛
    /// </summary>
    class GAMatch
    {
        /// <summary>
        /// 队伍列表
        /// </summary>
        protected List<string> teamNames;
        /// <summary>
        /// 场地距离列表
        /// </summary>
        protected List<Distance> veDistances;
        /// <summary>
        /// 所有比赛安排，即Timeslot的集合
        /// </summary>
        protected Schedule schedule;
        /// <summary>
        /// 配对的队伍列表
        /// </summary>
        private List<PairTeam> pairingTeams;
        /// <summary>
        /// 两场地最大距离
        /// </summary>
        private double maxDis = 0;
        /// <summary>
        /// 构造函数，读取文件内容并赋值给成员变量
        /// </summary>
        /// <param name="teamFilePath">队伍文件</param>
        /// <param name="timesFilePath">场地开放时间文件</param>
        /// <param name="distanceFilePath">场地距离文件</param>
        public GAMatch(string teamFilePath, string timesFilePath, string distanceFilePath)
        {
            schedule = new Schedule();
            try
            {
                teamNames = LoadFile.loadFromTeamFile(teamFilePath);
                schedule.addTimeSlots(LoadFile.loadFromTimeFile(timesFilePath));
                veDistances = LoadFile.loadFromDistanceFile(distanceFilePath);
            }
            catch
            {
                Console.WriteLine("文件读取出现错误！");
            }
            for (int i = 0; i < veDistances.Count; i++)
            {
                if (veDistances[i].VenueDis > maxDis)
                    maxDis = veDistances[i].VenueDis;
            }
        }
        /// <summary>
        /// 用遗传算法创建比赛安排表
        /// </summary>
        /// <param name="populationSize">每代的种群规模</param>
        /// <param name="survivalRate">存活率</param>
        /// <param name="mutationRate">突变率，未突变的群体进行交配并基因重组产生后代</param>
        /// <param name="generationNum">总共进化多少代</param>
        public void createScheduleUsingGeneticAlgorithm(int populationSize,int survivalRate,double mutationRate,int generationNum)
        {
            //第几代
            int generationCount = 0;
            //将所有队伍两两配对
            pairingTeams = pairingAllTeams(teamNames);
            //判断时间槽是否足够
            if(schedule.getTimeSlotNum()<pairingTeams.Count)
            {
                Console.WriteLine("时间槽安排不够，无法排完所有比赛！");
            }
            //当前代对应的种群
            List<Chromosome> generation = new List<Chromosome>();
            //生成初始种群
            for (int i = 0; i < populationSize; i++)
                generation.Add(new Chromosome(getRandomGeneSequence(pairingTeams.Count)));
            //依次生成下一代
            while (generationCount < generationNum && generation.Count > 1)
            {
                double max_fitness = 0;
                double avgFitness = 0;
                double totalFitness = 0;
                double minFitness = -1;
                //为每个个体设置适应度值，并更新最大适应度和最小适应度值
                for(int i=0;i<generation.Count;i++)
                {
                    setFitness(generation[i]);
                    double fitness = generation[i].Fitness;
                    avgFitness += fitness;
                    if (fitness > max_fitness)
                        max_fitness = fitness;
                    if (minFitness == -1 || fitness < minFitness)
                        minFitness = fitness;
                }
                //用相对适应度重新赋值
                for (int i = 0; i < generation.Count; i++)
                {
                    double fitness = generation[i].Fitness-minFitness;
                    totalFitness += fitness;
                    generation[i].Fitness = fitness;
                }
                //选择存活的个体
                List<Chromosome> chosen_survivors = new List<Chromosome>();
                int num_of_survivors = (int)((double)survivalRate * 0.01 * generation.Count);
                //打印当前代信息
                Console.WriteLine("当前为第：" + generationCount + "代 , 平均适应度为：" + avgFitness / generation.Count + ", 最大适应度为：" + max_fitness);
                //轮盘赌选择存活个体
                Random r = new Random(GetRandomSeed());
                for (int i = 0; i < num_of_survivors; i++)
                {
                    double survivor_rate = r.NextDouble();
                    double sum = 0;
                    for (int j = 0; j < generation.Count; j++)
                    {
                        sum += (generation[j].Fitness)/totalFitness;
                        if (survivor_rate <= sum)
                        {
                            chosen_survivors.Add(generation[j]);
                            break;
                        }
                    }
                }
                //生成下一代
                List<Chromosome> next_generation = new List<Chromosome>();
                //从存活的个体中选择一部分进行突变
                int mutation_chorosome_num = (int)((double)mutationRate * num_of_survivors);
                for(int i=0;i<mutation_chorosome_num;i++)
                {
                    int random = r.Next(num_of_survivors);//产生一个随机数
                    Chromosome ch = new Chromosome(chosen_survivors[random].Seq);
                    //创建突变的染色体段
                    ch.createExchangePart();
                    List<int> left = new List<int>();
                    List<int> middle = new List<int>();
                    List<int> right = new List<int>();

                    left = ch.getLeftPart();
                    //将中间部分的基因反转
                    middle=ch.getMiddlePart();
                    middle.Reverse();
                    right = ch.getRightPart();
                    //生成新的染色体基因序列
                    List<int> new_seq = new List<int>();
                    new_seq.AddRange(left);
                    new_seq.AddRange(middle);
                    new_seq.AddRange(right);
                    //将新的染色体个体添加到下一代中
                    Chromosome new_ch = new Chromosome(new_seq);
                    next_generation.Add(new_ch);
                }
                //存活的个体中，突变以外的个体进行基因重组产生后代,直到种群的数量达到预定值为止
                for(int i=0;i<generation.Count-mutation_chorosome_num;i+=2)
                {
                    Chromosome[] children = geneCrossOver(chosen_survivors[r.Next(num_of_survivors)], chosen_survivors[r.Next(num_of_survivors)]);
                    next_generation.Add(children[0]);
                    next_generation.Add(children[1]);
                }
                //产生完这一代，继续产生下一代
                generationCount++;
                generation = next_generation;
            }

            //从最后一代中找适应度最大的个体
            Chromosome bestOne = null;
            double maxFitness = 0;
            for(int i=0;i<generation.Count;i++)
            {
                Chromosome cur = generation[i];
                setFitness(cur);
                double fit = cur.Fitness;
                if(fit>maxFitness && isValidGeneSeq(cur))
                {
                    bestOne = cur;
                    maxFitness = fit;
                }
            }
            if(bestOne==null)
            {
                Console.WriteLine("无可行方案！");
            }
            else
            {
                setFitness(bestOne);
                Console.WriteLine("当前为第：" + generationCount + "代,"+ "  最大适应度为：" + maxFitness);
                Console.WriteLine("适应度：" + bestOne.Fitness + "\t 完成所有比赛需要经过场地间的总距离：" + bestOne.ToTalDistance+" 米 ");
                Console.WriteLine("比赛顺序：" + bestOne.getSeqInfo());
                for(int i=0;i<bestOne.Seq.Count;i++)
                {
                    schedule.setTeams(i, pairingTeams[bestOne.Seq[i]]);
                }
                Console.WriteLine(schedule.getScheduleInfo());
            }
        }
        /// <summary>
        /// 基因重组，产生两个新后代
        /// </summary>
        /// <param name="chromosome1"></param>
        /// <param name="chromosome2"></param>
        /// <returns></returns>
        private Chromosome[] geneCrossOver(Chromosome chromosome1, Chromosome chromosome2)
        {
            //随机生成交换的染色体左右端点
            chromosome1.createExchangePart();
            //将端点信息赋给染色体2
            chromosome2.setExchangePos(chromosome1.LeftPos, chromosome1.RightPos);
            //拿到染色体1的三部分
            List<int> left_part_of_ch_1 = chromosome1.getLeftPart();
            List<int> middle_part_of_ch_1 = chromosome1.getMiddlePart();
            List<int> right_part_of_ch_1 = chromosome1.getRightPart();
            //拿到染色体2的三部分
            List<int> left_part_of_ch_2 = chromosome2.getLeftPart();
            List<int> middle_part_of_ch_2 = chromosome2.getMiddlePart();
            List<int> right_part_of_ch_2 = chromosome2.getRightPart();

            List<int> new_ch_1 = new List<int>();
            List<int> new_ch_2 = new List<int>();
            //变换基因段的位置
            new_ch_1.AddRange(right_part_of_ch_1);
            new_ch_1.AddRange(left_part_of_ch_1);
            new_ch_1.AddRange(middle_part_of_ch_1);

            new_ch_2.AddRange(right_part_of_ch_2);
            new_ch_2.AddRange(left_part_of_ch_2);
            new_ch_2.AddRange(middle_part_of_ch_2);

            List<int> L1 = new List<int>();
            List<int> L2 = new List<int>();
            L1.AddRange(new_ch_1);
            L2.AddRange(new_ch_2);
            int L = new_ch_1.Count;
            //交换middle基因段之前，要把要交换的基因先删除
            for (int i=0;i<L;i++)
            {
                for (int j = 0; j < middle_part_of_ch_2.Count; j++)
                {
                    if (L1[i] == middle_part_of_ch_2[j])
                        new_ch_1.Remove(L1[i]);

                    if (L2[i] == middle_part_of_ch_1[j])
                        new_ch_2.Remove(L2[i]);
                }
            }
            //开始交换
            new_ch_1.InsertRange(chromosome1.LeftPos, middle_part_of_ch_2);
            new_ch_2.InsertRange(chromosome2.LeftPos, middle_part_of_ch_1);
            //返回结果
            Chromosome[] result = new Chromosome[2];
            result[0] = new Chromosome(new_ch_1);
            result[1] = new Chromosome(new_ch_2);
            return result;
         }
        
        /// <summary>
        /// 返回所有配对
        /// </summary>
        /// <param name="teams"></param>
        /// <returns></returns>
        public List<PairTeam> pairingAllTeams(List<string> teams)
        {
            List<PairTeam> pairs = new List<PairTeam>();
            for(int i=0;i<teams.Count;i++)
                for(int j=i+1;j<teams.Count;j++)
                {
                    pairs.Add(new PairTeam(teams[i], teams[j]));
                }
            return pairs;
        }
        /// <summary>
        /// 生成随机基因序列（即一条染色体个体）
        /// </summary>
        /// <param name="length">基因个数，即比赛序列长度</param>
        /// <returns></returns>
        public List<int> getRandomGeneSequence(int length)
        {
            List<int> L1 = new List<int>();
            for (int i = 0; i < length; i++)
                L1.Add(i);
            //这样实例化一个种子的随机数生成器。
            Random rand = new Random(GetRandomSeed());
            List<int> L2 = new List<int>();
            for(int i=0;i<length;i++)
            {
                int index = rand.Next(L1.Count);
                L2.Add(L1[index]);
                L1.RemoveAt(index);
            }
            return L2;
        }
        /// <summary>
        /// 生成随机种子
        /// </summary>
        /// <returns>种子</returns>
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        /// <summary>
        /// 为染色体个体设置适应度
        /// </summary>
        /// <param name="ch"></param>
        public void setFitness(Chromosome ch)
        {
            //初始化总的最大距离
            double maxTotalDist = maxDis * (teamNames.Count - 1) * teamNames.Count;
            //得到该比赛序列安排所需的距离长
            double ChromosomeSeqTotalDistance = findTotalDistance(ch);
            ch.ToTalDistance = ChromosomeSeqTotalDistance;
            //若该染色体基因序列合法
            if(isValidGeneSeq(ch))
            {
                ch.Fitness = (maxTotalDist - ChromosomeSeqTotalDistance);
            }
            else
            {
                ch.Fitness = (maxTotalDist - ChromosomeSeqTotalDistance) / 2;
            }
        }
        /// <summary>
        /// 返回该染色体基因序列所代表的比赛序列需要的总距离
        /// </summary>
        /// <param name="ch">染色体</param>
        /// <returns></returns>
        private double findTotalDistance(Chromosome ch)
        {
            double totalDis = 0;
            for(int i=0;i<teamNames.Count;i++)
            {
                string team_name = teamNames[i];
                string last_venue="";
                for(int j=0;j<ch.Seq.Count;j++)
                {
                    //seq的每个成员都是一场比赛的序号
                    int cur = ch.Seq[j];
                    //根据序号cur得到比赛的两支队伍，判断是否包含team_name这支队伍
                    if(pairingTeams[cur].ifContains(team_name))
                    {
                        string cur_venue = schedule.getTimeSlot(j).VenueName;
                        if (last_venue == "")
                            last_venue = cur_venue;
                        else if (last_venue != cur_venue)
                            totalDis += calculateDistBetweenTwoVenues(last_venue, cur_venue);
                    }
                }
            }
            return totalDis;
        }
        /// <summary>
        /// 计算两场地间距离
        /// </summary>
        /// <param name="last_venue"></param>
        /// <param name="cur_venue"></param>
        /// <returns></returns>
        private double calculateDistBetweenTwoVenues(string last_venue, string cur_venue)
        {
            for (int i=0; i < veDistances.Count; i++)
            {
                if(veDistances[i].ifContains(last_venue,cur_venue))
                {
                    return veDistances[i].VenueDis;
                }
            }
            return 0;
        }
        /// <summary>
        /// 判断该基因序列是否合法
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool isValidGeneSeq(Chromosome ch)
        {
            //为schedule设置比赛队伍安排
            for (int i = 0; i < ch.Seq.Count; i++)
                schedule.setTeams(i, pairingTeams[ch.Seq[i]]);
            //判断时间安排是否合法
            if (schedule.isValid())
            {
                clearTeams();
                return true;
            }
            clearTeams();
            return false;
        }
        /// <summary>
        /// 清空时间槽中的比赛队伍安排
        /// </summary>
        private void clearTeams()
        {
            for (int i = 0; i < schedule.getTimeSlotNum(); i++)
                schedule.setTeams(i, null);
        }

    }
}