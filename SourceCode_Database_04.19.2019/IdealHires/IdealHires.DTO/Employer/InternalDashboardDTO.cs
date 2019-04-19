using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdealHires.DTO.Employer
{
    public class InternalDashboardDTO
    {
        public int TodayJob { get; set; }
        public int WeekJob { get; set; }
        public int MonthJob { get; set; }
        public int YearJob { get; set; }

        public int TodayExpiredJob { get; set; }

        public int WeekExpiredJob { get; set; }
        public int MonthExpiredJob { get; set; }
        public int YearExpiredJob { get; set; }
        public int Total { get; set; }
        public int TotalExpiredJob { get; set; }

        public int TotalTodayJob { get; set; }
        public int TotalWeekJob { get; set; }
        public int TotalMonthJob { get; set; }
        public int TotalYearJob { get; set; }
        public int AllColumnTotal { get; set; }
        public int TodayModifiedJobPost { get; set; }
        public int WeekModifiedJobPost { get; set; }
        public int MonthModifiedJobPost { get; set; }
        public int ModifiedYearJobPost { get; set; }
        public int TotalModifiedJobPost { get; set; }
        
        public int ModifiedJobTotalToday { get; set; }
        public int ModifiedJobTotalWeek { get; set; }
        public int ModifiedJobTotalMonth { get; set; }
        public int ModifiedJobTotalYear { get; set; }
        public int AllModifiedJobTotal { get; set; }
        public int EmpTProfile { get; set; }
    
        public int EmpWProfile { get; set; }

        public int EmpMProfile { get; set; }
        public int EmpYProfile { get; set; }

        public int NewEmpTotalProfile { get; set; }
        public int EmpTProfileNot { get; set; }
        public int EmpWProfileNot { get; set; }
        public int EmpMProfileNot { get; set; }
        public int EmpYProfileNot { get; set; }
        public int TotalEmpProfileNot { get; set; }
        public int TodayTotalProfileRow { get; set; }
        public int WeekTotalProfileRow { get; set; }
        public int MonthTotalProfileRow { get; set; }
        public int YearTotalProfileRow { get; set; }
        public int AllTotalProfileColumn { get; set; }
        public int CandTProfile { get; set; }

        public int CandWProfile { get; set; }

        public int CandMProfile { get; set; }
        public int CandYProfile { get; set; }

        public int CandTotalProfile { get; set; }
        public int CandTProfileNot { get; set; }
        public int CandWProfileNot { get; set; }
        public int CandMProfileNot { get; set; }
        public int CandYProfileNot { get; set; }
        public int TotalCandProfileNot { get; set; }
        public int TodayTotalCandProfileRow { get; set; }
        public int WeekTotalCandProfileRow { get; set; }
        public int MonthTotalCandProfileRow { get; set; }
        public int YearTotalCandProfileRow { get; set; }
        public int AllTotalCandProfileColumn { get; set; }
    }
}
