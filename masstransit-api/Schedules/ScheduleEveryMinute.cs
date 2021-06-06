using MassTransit.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace masstransit_api.Schedules
{
    public class ScheduleEveryMinute : DefaultRecurringSchedule
    {
        public ScheduleEveryMinute()
        {
            CronExpression = "0/1 * * * *";
        }
    }
}
