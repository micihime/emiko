using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ViewModels
{
    public class PopulationOfHaikus
    {
        public List<Haiku> population { set; get; } //population of haiku poems
        public int generation { set; get; } //generation counter

        public PopulationOfHaikus()
        {
            this.population = new List<Haiku>();
            this.generation = 0;

            for (int i = 0; i < 10; i++)  //population of individuals
            {
                Haiku haiku = new Haiku(); //creates new instance of haiku poem with default fitness values
                haiku.pickRandomTxt(); //fills the lines of poem with randomly chosen haiku from the haiku database
                this.population.Add(haiku); //adds this haiku to population
            }
        }

        public void Replace() //create new population with reproducing and injection method
        {
            Reproduce(Select()); //replacing current population with new one
            Injection();
            this.generation++; //increase generation counter
        }

        private List<int> Roulette() //roulette as method of parent selection
        {
            List<int> roulette = new List<int>();

            for (int i = 0; i < this.population.Count; i++) //creating roulette that contains index of individual in population
            {
                for (int j = 0; j < this.population[i].humanFitness; j++) //individual will have as many places in roulette as is his fitness value
                {
                    roulette.Add(i);
                }
            }

            return roulette;
        }

        private List<Haiku> Select() //selection of 4 parents
        {
            List<int> roulette = Roulette(); //using roulette method of selection
            List<Haiku> parents = new List<Haiku>();
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            int p;

            if (roulette.Count < 4) //if there are not enough parents in roulette (parents with 0 fitness value cannot be chosen to roulette)
            {
                for (int i = 0; i < roulette.Count; i++) 
                {
                    p = rnd.Next(roulette.Count); //select random parents from roulette
                    parents.Add(this.population[roulette[p]]); //add parent to parents list
                    roulette.RemoveAt(p); //remove the individual from roulette
                }
                for (int i = roulette.Count; i < 5; i++) //random creation of parents
                {
                    Haiku haiku = new Haiku(); //creates new instance of haiku poem with default fitness values
                    haiku.pickRandomTxt(); //fills the lines of poem with randomly chosen haiku from the haiku database
                    this.population.Add(haiku);
                }
            }
            else 
            {
                for (int i = 0; i < 4; i++)
                {
                    p = rnd.Next(roulette.Count); //select random parents from roulette
                    parents.Add(this.population[roulette[p]]); //add parent to parents list
                    roulette.RemoveAt(p); //removing the individual from roulette
                }
            }

            return parents;
        }

        private void Reproduce(List<Haiku> parents) //reproducing parents, creates 8 new individuals by crossing over parents
        {
            this.population.Clear();
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            Haiku child = new Haiku();

            for (int i = 0; i < parents.Count; i++) //for each parent
            {
                int x = rnd.Next(parents.Count); //select random second parent

                child = parents[i].crossOver(parents[x]); //perform cross over
                this.population.Add(child); //add new individual to population
                
                child = parents[x].crossOver(parents[i]); //perform cross over
                this.population.Add(child); //add new individual to population
            }
        }

        private void Injection() //injection method of 2 random individuals chosen from database
        {
            Haiku haiku = new Haiku(); //creates new instance of haiku poem with default fitness values
            haiku.pickRandomTxt(); //fills the lines of poem with randomly chosen haiku from the haiku database
            this.population.Add(haiku); //adds this haiku to population

            haiku.pickRandomTxt(); //fills the lines of poem with randomly chosen haiku from the haiku database
            this.population.Add(haiku); //adds this haiku to population
        }

        public void SerializeXml()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using (StreamWriter writer = new StreamWriter("population.xml"))
            {
                serializer.Serialize(writer.BaseStream, this);
            }
        }

        public PopulationOfHaikus DeserializeXml()
        {
            PopulationOfHaikus p = new PopulationOfHaikus();
            XmlSerializer serializer = new XmlSerializer(this.GetType());

            using (StreamReader reader = new StreamReader("population.xml"))
            {
                object deserialized = serializer.Deserialize(reader.BaseStream);
                p = (PopulationOfHaikus)deserialized;
            }

            return p;
        }
    }
}