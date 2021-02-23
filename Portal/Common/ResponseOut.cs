using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Common
{
    public class ResponseOut
    {
        public string message { get; set; }
        public string status { get; set; }
        public int trnId { get; set; }
        public string indentMessage { get; set; }
        public string email { get; set; }
        public string hash { get; set; }
    }

    public class SingleResponseOut<TModel> : ResponseOut
    {
       public TModel Model { get; set; }
    }

    public class ListResponseOut<TModel> : ResponseOut
    {
       public IEnumerable<TModel> ListModel { get; set; }
    }

}