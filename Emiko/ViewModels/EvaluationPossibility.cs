using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class EvaluationPossibility
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public EvaluationPossibility(int i, string s)
        {
            this.id = i;
            this.name = s;
        }
    }
}