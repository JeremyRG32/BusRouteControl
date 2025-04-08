﻿namespace BusRouteControl.API.Dtos
{
    public class BusRouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Origin { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public List<ScheduleDto> Schedules { get; set; } = new();
    }
}
