using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI
{
    public static class ScheduleRecommendationService
    {
        public static List<Schedule> RecommendSchedules(this List<Course> courses, Constraint constraint)
        {
            List<List<Class>> classTimeLists = courses.Select(course => course.Classes.ToList()).ToList();
            List<List<Class>> combinations = CartesianProduct(classTimeLists);

            List<Schedule> schedules = new List<Schedule>();
            foreach (List<Class> combination in combinations)
            {
                if (IsScheduleValid(combination, constraint))
                {
                    schedules.Add(new Schedule(combination));
                }
            }

            return schedules;
        }
        private static bool IsScheduleValid(List<Class> classTimes, Constraint constraint)
        {
            // Check that each class time is unique
            if (classTimes.Distinct().Count() != classTimes.Count())
                return false;

            // Check that no two class times overlap
            for (int i = 0; i < classTimes.Count; i++)
            {
                for (int j = i + 1; j < classTimes.Count; j++)
                {
                    if (classTimes[i].Day == classTimes[j].Day &&
                        classTimes[i].EndTime >= classTimes[j].StartTime &&
                        classTimes[j].EndTime >= classTimes[i].StartTime)
                    {
                        return false;
                    }
                }
            }

            // Check constraints
            if (constraint.MaxClassPerDay.HasValue)
            {
                var classesPerDay = classTimes
                    .GroupBy(c => c.Day)
                    .Select(group => group.Count());
                if (classesPerDay.Any(count => count > constraint.MaxClassPerDay))
                    return false;
            }

            if (constraint.TimeSlots != null)
            {
                foreach (var classTime in classTimes)
                {
                    if (constraint.TimeSlots.Any(slot =>
                        slot.Day == classTime.Day &&
                        classTime.EndTime >= slot.StartTime &&
                        slot.EndTime >= classTime.StartTime))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static List<List<Class>> CartesianProduct(List<List<Class>> lists)
        {
            List<List<Class>> result = new List<List<Class>>();
            result.Add(new List<Class>());

            foreach (var list in lists)
            {
                List<List<Class>> tmp1 = new List<List<Class>>();
                foreach (var item in result)
                {
                    foreach (var inner in list)
                    {
                        var newInner = new List<Class>(item) { inner };
                        tmp1.Add(newInner);
                    }
                }
                result = tmp1;
            }
            return result;
        }
    }

    public class Schedule
    {
        public List<Class> ClassTimes { get; set; }

        public Schedule(List<Class> classTimes)
        {
            ClassTimes = classTimes;
        }
    }

    public class Constraint
    {
        public int? MaxClassPerDay { get; set; }
        public List<TimeSlot>? TimeSlots { get; set; }

    }

    public class TimeSlot
    {
        public DayOfWeek Day { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}
