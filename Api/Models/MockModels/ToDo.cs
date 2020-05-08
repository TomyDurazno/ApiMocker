using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIMocker.Models.MockModels
{
    public class ToDo
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }

        public ToDo(string title, bool iscompleted)
        {
            Title = title;
            IsCompleted = iscompleted;
        }
    }
}