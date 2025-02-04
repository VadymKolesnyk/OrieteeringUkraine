﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Tables
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public string InfoLink { get; set; }
        public string ResultsLink { get; set; }
        public string Location { get; set; }
        public int RegionId { get; set; }
        public int OrganizerId { get; set; }
        public List<EventGroup> EventGroup { get; set; }
        [ForeignKey("RegionId")]
        public Region Region { get; set; }
    }
}
