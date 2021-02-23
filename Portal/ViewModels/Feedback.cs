using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class Feedback
    {

    }

    public class WhatClientSays
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string client_name { get; set; }
        public bool is_published { get; set; }
        public string added_datetime { get; set; }
        public string client_image { get; set; }
    }
}