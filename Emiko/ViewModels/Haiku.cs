using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Emiko.Models;
using System.IO;

namespace ViewModels
{
    public class Haiku
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private String v1; //1st verse of haiku poem

        public String V1
        {
            get { return v1; }
            set { v1 = value; }
        }

        private String v2; //2nd verse of haiku poem

        public String V2
        {
            get { return v2; }
            set { v2 = value; }
        }

        private String v3; //3rd verse of haiku poem

        public String V3
        {
            get { return v3; }
            set { v3 = value; }
        }

        //initialize human evaluation possibilities
        public static EvaluationPossibility[] humanEval = { new EvaluationPossibility(2, "excellent"), new EvaluationPossibility(1, "so-so"), new EvaluationPossibility(0, "stupid") }; //human evaluation possibilities    

        private int humanFitness; //human evaluation

        public int HumanFitness
        {
            get { return humanFitness; }
            set { humanFitness = value; }
        }

        public Haiku() //to be evolved
        {
            pickRandomDb(); //fills the lines of poem with randomly chosen haiku from the haiku database
            this.humanFitness = 1; //set fitness values to default values
        }

        public Haiku(int x) //for helping purposes in selection&reproduction etc.
        {
            this.v1 = "";
            this.v2 = "";
            this.v3 = "";
            this.humanFitness = 1; //set fitness values to default values
        }

        public void pickRandomDb()
        {
            DbConnection db = new DbConnection();
            db.connect();
            SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM haiku", db.Connection); //determine nr of haikus in the database
            int max = (int)cmdCount.ExecuteScalar();

            SqlCommand command = new SqlCommand("SELECT * FROM haiku WHERE id = @id", db.Connection); //select random haiku
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            int x = rnd.Next(max);
            command.Parameters.AddWithValue("@id", x++);
            this.id = x;

            using (SqlDataReader reader = db.read(command))
            {
                while (reader.Read()) //read poem from the database
                {
                    this.v1 = reader["v1"].ToString();
                    this.v2 = reader["v2"].ToString();
                    this.v3 = reader["v3"].ToString();
                }
            }

            this.humanFitness = 1; //set default fitness value
            db.disconnect();
        }

        public void write()
        {
            if (this.humanFitness == 2) //if haiku was marked as Excellent
            { //write it to log file
                using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "log.txt", true)) //write haiku to log file
                {
                    sw.WriteLine(this.v1);
                    sw.WriteLine(this.v2);
                    sw.WriteLine(this.v3);

                    sw.WriteLine("--------------------------------------------");
                    sw.Close();
                }
            }
        }

        public Haiku crossOver(Haiku p2)
        {
            Haiku haiku = new Haiku(1);
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            int p;

            p = rnd.Next(3); //randomly choose a verse in poem that will be taken from the 2nd parent
            switch (p) //replace chosen verse
            {
                case 0:
                    haiku.v1 = p2.v1;
                    haiku.v2 = this.v2;
                    haiku.v3 = this.v3;
                    break;
                case 1:
                    haiku.v1 = this.v1;
                    haiku.v2 = p2.v2;
                    haiku.v3 = this.v3;
                    break;
                case 2:
                    haiku.v1 = this.v1;
                    haiku.v2 = this.v2;
                    haiku.v3 = p2.v3;
                    break;
            }

            return haiku;
        }
    }
}