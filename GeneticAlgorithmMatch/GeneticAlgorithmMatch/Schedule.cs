using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmMatch
{
    /// <summary>
    /// 安排
    /// </summary>
    class Schedule
    {
        /// <summary>
        /// 时间安排List
        /// </summary>
        private List<TimeSlot> times;

        public List<TimeSlot> Times
        {
            get
            {
                return times;
            }

            set
            {
                times = value;
            }
        }

        public Schedule()
        {
            Times = new List<TimeSlot>();
        }
        /// <summary>
        /// 增加一个比赛安排时间槽
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="venueName"></param>
        public void addTimeSlot(DateTime startTime, string venueName)
        {
            TimeSlot slot = new TimeSlot(startTime, venueName);
            addTimeSlot(slot);
        }
        /// <summary>
        /// 增加比赛安排时间槽
        /// </summary>
        /// <param name="slot"></param>
        public void addTimeSlot(TimeSlot slot)
        {
            if (!timeSlotExists(slot))
                Times.Add(slot);
        }
        /// <summary>
        /// 时间槽是否存在
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public bool timeSlotExists(TimeSlot slot)
        {
            for (int i = 0; i < Times.Count; i++)
            {
                TimeSlot curr = Times[i];
                if (curr.equals(slot))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 增加时间安排列表
        /// </summary>
        /// <param name="slots"></param>
        public void addTimeSlots(List<TimeSlot> slots)
        {
            foreach(TimeSlot slot in slots)
            {
                addTimeSlot(slot);
            }
        }
        /// <summary>
        /// 返回时间槽数目
        /// </summary>
        /// <returns></returns>
        public int getTimeSlotNum()
        {
            return Times.Count();
        }
        /// <summary>
        /// 得到与下标相对应的时间槽
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TimeSlot getTimeSlot(int i)
        {
            return times[i];
        }
        /// <summary>
        /// 为第i个时间槽设定比赛队伍
        /// </summary>
        /// <param name="i">第i个时间槽</param>
        /// <param name="pair">一对比赛队伍</param>
        public void setTeams(int i, PairTeam pair)
        {
            times[i].setTeams(pair);
        }
        /// <summary>
        /// 时间槽安排是否合法(即时间安排是否有冲突或者是否队伍是否有重叠)
        /// </summary>
        /// <returns></returns>
        public bool isValid()
        {
            for (int i = 0; i < times.Count; i++)
            {
                TimeSlot tmp = times[i];
                for (int j = i+1; j < times.Count; j++)
                {
                    TimeSlot secondTime = times[j];
                    if (tmp.ifOverlaps(secondTime) && tmp.Teams.ifHaveDuplicate(secondTime.Teams))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public string getScheduleInfo()
        {
            string info = "";
            info += "+---------------------------------------------------------------+\n";
            info += "+                            比赛安排                           +\n";
            info += "+---------------------------------------------------------------+\n";
            for (int i = 0; i < times.Count; i++)
            {
                info += times[i].getTimeSlotInfo() + "\n";
            }
            return info;
        }
    }
}
