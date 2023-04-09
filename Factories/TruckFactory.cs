using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.classes;

namespace WindowsFormsApp1.Factories
{
    class TruckFactory : TransportFactory
    {
        public override Transport createTransport(List<string> fields)
        {
            Person.Sex sex = (Person.Sex)Enum.Parse(typeof(Person.Sex), fields[10]);
            Person person = new Person(fields[8], fields[9], sex, Int32.Parse(fields[11]));
            return new Truck(
                Int32.Parse(fields[0]),
                Boolean.Parse(fields[1]),              
                Int32.Parse(fields[2]),
                Int32.Parse(fields[3]),
                Int32.Parse(fields[4]),
                Color.Red,
                //(Color)Enum.Parse(typeof(Color), fields[5]),
                fields[6],
                Int32.Parse(fields[7]),
                person);
        }
    }
}
