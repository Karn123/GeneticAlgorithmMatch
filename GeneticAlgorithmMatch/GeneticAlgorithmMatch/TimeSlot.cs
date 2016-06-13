using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmMatch
{
    /// <summary>
    /// 时间槽
    /// </summary>
    class TimeSlot
    {
        /// <summary>
        /// 比赛开始时间
        /// </summary>
        private DateTime startTime;
        /// <summary>
        /// 比赛场地名称
        /// </summary>
        private string venueName;
        /// <summary>
        /// 比赛队伍
        /// </summary>
        private PairTeam teams;

        public string VenueName
        {
            get
            {
                return venueName;
            }

            set
            {
                venueName = value;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                startTime = value;
            }
        }

        internal PairTeam Teams
        {
            get
            {
                return teams;
            }

            set
            {
                teams = value;
            }
        }

        public TimeSlot(DateTime t, string venue)
        {
            StartTime = t;
            VenueName = venue;
        }
        public TimeSlot(DateTime t, string venue, string first, string second)
        {
            StartTime = t;
            VenueName = venue;
            Teams = new PairTeam(first, second);
        }
        public void setTeams(PairTeam pair)
        {
            Teams = pair;
        }
        public void setTeams(string first, string second)
        {
            Teams = new PairTeam(first, second);
        }
        public bool equals(TimeSlot slot)
        {
            return (this.startTime == slot.startTime && this.venueName == slot.venueName);
        }
        /// <summary>
        /// 返回时间槽的比赛信息
        /// </summary>
        /// <returns></returns>
        public string getTimeSlotInfo()
        {
            return startTime.ToString() + ": " + teams.getTeamInfo() + " at " + venueName;
        }
        /// <summary>
        /// 时间是否冲突
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public bool ifOverlaps(TimeSlot slot)
        {
            DateTime first = new DateTime();
            first = startTime;

            DateTime second = new DateTime();
            second = slot.startTime;

            if (first.Year == second.Year && first.Month == second.Month && first.Day == second.Day)
            {
                DateTime calendar = new DateTime();
                calendar=startTime;

                int firstStart = calendar.Hour * 100 + calendar.Minute;
                calendar.AddHours(1);
              
                int firstEnd = calendar.Hour * 100 + calendar.Minute;

                calendar = slot.startTime;
                int secondStart = calendar.Hour * 100 + calendar.Minute;
                calendar.AddHours(1);
                int secondEnd = calendar.Hour * 100 + calendar.Minute;

                if ((firstStart > secondStart && firstStart < secondEnd) ||
                   (secondStart > firstStart && secondStart < firstEnd) ||
                   (firstStart == secondStart))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
