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
        private List<Haiku> population; //population of haiku poems
        
        private int generation = 0;

        public int Generation
        {
            get { return generation; }
            set { generation = value; }
        }

        public List<Haiku> Population
        {
            get { return population; }
            set { population = value; }
        }

        public PopulationOfHaikus()
        {
            this.population = new List<Haiku>();

            for (int i = 0; i < 10; i++)  //population of individuals
            {
                Haiku haiku = new Haiku(); //creates new instance of haiku poem with default fitness values
                this.population.Add(haiku); //adds this haiku to population
            }
        }

        public void Replace() //create new population with reproducing and injection method
        {
            Reproduce(Select()); //replacing current population with new one
            Injection(); //fresh material added to population
            generation++;
        }

        private List<int> Roulette() //roulette as method of parent selection
        {
            List<int> roulette = new List<int>();

            for (int i = 0; i < this.population.Count; i++) //creating roulette that contains index of individual in population
            {
                for (int j = 0; j < this.population[i].HumanFitness; j++) //individual will have as many places in roulette as is his fitness value
                {
                    roulette.Add(i);
                }
            }

            return roulette;
        }

        private List<Haiku> Select() //selection of 4 parents
        {
            List<int> roulette = Roulette(); //using roulette method of selection
            List<Haiku> parents = new List<Haiku>(1); 
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
                    Haiku haiku = new Haiku(); //selects new haiku
                    parents.Add(haiku);
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
            this.population.Clear(); //deletes old population
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            //replaces old population with new individuals
            for (int i = 0; i < parents.Count; i++) //for each parent
            {
                int x = rnd.Next(parents.Count); //select random second parent
                Haiku child1 = new Haiku(1); //creates empty haiku
                child1 = parents[i].crossOver(parents[x]); //perform cross over
                this.population.Add(child1); //add new individual to population
                Haiku child2 = new Haiku(1); //creates empty haiku
                child2 = parents[x].crossOver(parents[i]); //perform cross over
                this.population.Add(child2); //add new individual to population
            }
        }

        private void Injection() //injection method of 2 random individuals chosen from database
        {
            Haiku h1 = new Haiku(); //creates new instance of haiku poem, selects random from database with default fitness values
            this.population.Add(h1); //adds this haiku to population

            Haiku h2 = new Haiku(); //creates new instance of haiku poem, selects random from database with default fitness values
            this.population.Add(h2); //adds this haiku to population
        }

        public void SerializeXml()
        {
            XmlSerializer serializer = new XmlSerializer(this.generation.GetType());
            using (StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "population.xml"))
            {
                serializer.Serialize(writer.BaseStream, this.generation);
            }
        }

        public int DeserializeXml()
        {
            XmlSerializer serializer = new XmlSerializer(this.generation.GetType());

            using (StreamReader reader = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "population.xml"))
            {
                object deserialized = serializer.Deserialize(reader.BaseStream);
                this.generation = (int)deserialized;
            }

            return this.generation;
        }
    }
}