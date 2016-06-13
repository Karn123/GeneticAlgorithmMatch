using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmMatch
{
    /// <summary>
    /// 队伍配对
    /// </summary>
    class PairTeam
    {
        private string teamOne;
        private string teamTwo;

        public PairTeam() { }
        public PairTeam(string first, string second)
        {
            setTeam(first, second);
        }

        public bool setTeam(string first, string second)
        {
            if (first == second) return false;
            teamOne = first;
            teamTwo = second;
            return true;
        }
        public string TeamOne
        {
            get
            {
                return teamOne;
            }

            set
            {
                teamOne = value;
            }
        }

        public string TeamTwo
        {
            get
            {
                return teamTwo;
            }

            set
            {
                teamTwo = value;
            }
        }
        public string getTeamInfo()
        {
            return teamOne + " vs " + teamTwo;
        }
        public bool ifContains(string name)
        {
            return (teamOne == name || teamTwo == name);
        }
        public bool ifHaveDuplicate(PairTeam pair)
        {
            return (pair.ifContains(teamOne) || pair.ifContains(teamTwo));
        }
    }
}
