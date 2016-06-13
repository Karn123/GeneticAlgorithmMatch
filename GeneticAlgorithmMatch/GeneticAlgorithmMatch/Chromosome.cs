using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmMatch
{
    /// <summary>
    /// 染色体个体，即比赛顺序序列(序列长度为比赛的场数，每场比赛就是一个基因)
    /// </summary>
    class Chromosome
    {
        /// <summary>
        /// 基因列表
        /// </summary>
        private List<int> gene;
        /// <summary>
        /// 交叉互换的左端点基因位置
        /// </summary>
        private int leftPosition = -1;
        /// <summary>
        /// 交叉互换的右端点基因位置
        /// </summary>
        private int rightPosition = -1;
        /// <summary>
        /// 该个体的适应度
        /// </summary>
        private double fitness;
        /// <summary>
        /// 该比赛序列所要耗费的总的距离代价
        /// </summary>
        private double totalDistance;
        
        public Chromosome(List<int> _gene)
        {
            gene = _gene;
        }
        /// <summary>
        /// 根据另一个个体的交换位置设置自己的交换位置
        /// </summary>
        /// <param name="s">染色体个体</param>
        public void setExchangePos(Chromosome s)
        {
            leftPosition = s.leftPosition;
            rightPosition = s.rightPosition;
        }
        /// <summary>
        /// 根据所给的交换位置设置自己的交换位置
        /// </summary>
        ///<param name="L">左端点</param>
        ///<param name="R">右端点</param>
        public void setExchangePos(int L,int R)
        {
            leftPosition = L;
            rightPosition = R;
        }
        /// <summary>
        /// 随机生成要交换的部分染色体的左端点和右端点的值
        /// </summary>
        public void createExchangePart()
        {
            Random random = new Random();
            int firstPoint = random.Next(gene.Count + 1);
            int secondPoint = random.Next(gene.Count + 1);
            while (firstPoint == secondPoint)
                secondPoint = random.Next(gene.Count+ 1);
            leftPosition = (int)Math.Min(firstPoint, secondPoint);
            rightPosition = (int)Math.Max(firstPoint, secondPoint);
        }
        /// <summary>
        /// 拿到左端点左边部分的染色体（即基因集）
        /// </summary>
        /// <returns>基因集</returns>
        public List<int> getLeftPart()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < leftPosition; i++)
                list.Add(gene[i]);
            return list;
        }
        /// <summary>
        /// 拿到左端点和右端点之间的染色体，即要进行交叉互换的部分染色体
        /// </summary>
        /// <returns>基因集</returns>
        public List<int> getMiddlePart()
        {
            List<int> list = new List<int>();
            for (int i = leftPosition; i < rightPosition; i++)
                list.Add(gene[i]);
            return list;
        }
        /// <summary>
        /// 拿到右端点右边部分的染色体（即基因集）
        /// </summary>
        /// <returns>基因集</returns>
        public List<int> getRightPart()
        {
            List<int> list = new List<int>();
            for (int i = rightPosition; i < gene.Count; i++)
                list.Add(gene[i]);
            return list;
        }
        /// <summary>
        /// 返回该个体所代表的比赛顺序信息
        /// </summary>
        /// <returns>比赛顺序信息</returns>
        public string getSeqInfo()
        {
            string info = "";
            for (int i = 0; i < gene.Count; i++)
            {
                info += gene[i];
                if (i != gene.Count - 1)
                {
                    info += ",";
                }
            }
            return info;
        }
        public List<int> Seq
        {
            get
            {
                return gene;
            }

            set
            {
                gene = value;
            }
        }

        public int LeftPos
        {
            get
            {
                return leftPosition;
            }

            set
            {
                leftPosition = value;
            }
        }

        public int RightPos
        {
            get
            {
                return rightPosition;
            }

            set
            {
                rightPosition = value;
            }
        }

        public double Fitness
        {
            get
            {
                return fitness;
            }

            set
            {
                fitness = value;
            }
        }

        public double ToTalDistance
        {
            get
            {
                return totalDistance;
            }

            set
            {
                totalDistance = value;
            }
        }
   
    }
}
