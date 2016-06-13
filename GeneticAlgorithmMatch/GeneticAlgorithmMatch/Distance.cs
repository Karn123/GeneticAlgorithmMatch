using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmMatch
{
    /// <summary>
    /// 两场地距离
    /// </summary>
    class Distance
    {
        /// <summary>
        /// 场地1
        /// </summary>
        private string venueOne;
        /// <summary>
        /// 场地2
        /// </summary>
        private string venueTwo;
        /// <summary>
        /// 场地间距离
        /// </summary>
        private double venueDis;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_venueOne">场地1</param>
        /// <param name="_venueTwo">场地2</param>
        /// <param name="dis">距离</param>
        public Distance(string _venueOne,string _venueTwo,double dis)
        {
            VenueOne = _venueOne;
            VenueTwo = _venueTwo;
            venueDis = dis;
        }

        public double VenueDis
        {
            get
            {
                return venueDis;
            }

            set
            {
                venueDis = value;
            }
        }

        public string VenueTwo
        {
            get
            {
                return venueTwo;
            }

            set
            {
                venueTwo = value;
            }
        }

        public string VenueOne
        {
            get
            {
                return venueOne;
            }

            set
            {
                venueOne = value;
            }
        }
        /// <summary>
        /// 返回场地距离信息
        /// </summary>
        /// <returns></returns>
        public string getDisInfo()
        {
            return venueDis + " m from " + venueOne + " to " + venueTwo;
        }
        /// <summary>
        /// 是否包含场地v1和v2
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public bool ifContains(string v1,string v2)
        {
            return ((v1 == venueOne || v2 == venueOne) && (v1 == venueTwo || v2 == VenueTwo));
        }
    }
}