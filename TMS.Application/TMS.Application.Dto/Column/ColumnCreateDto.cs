﻿namespace TMS.Application.Dto.Column
{
    public class ColumnCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ColumnType { get; set; }
        public int Order { get; set; }
        public string Color { get; set; }
    }
}