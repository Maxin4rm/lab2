using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.classes;
using static WindowsFormsApp1.classes.Bus;

namespace WindowsFormsApp1.Factories
{
    class BusFactory : TransportFactory
    {
        public override Transport createTransport(List<string> fields)
        {
            Person.Sex sex = (Person.Sex)Enum.Parse(typeof(Person.Sex), fields[11]);
            Person person = new Person(fields[9], fields[10], sex, Int32.Parse(fields[12]));
            return new Bus(
                Int32.Parse(fields[0]),
                Boolean.Parse(fields[1]),
                (Bus.BusType)Enum.Parse(typeof(Bus.BusType), fields[2]),
                Int32.Parse(fields[3]),

                Int32.Parse(fields[4]),
                Int32.Parse(fields[5]),
                Color.Red,
                //(Color)Enum.Parse(typeof(Color), fields[6]),
                fields[7],
                Int32.Parse(fields[8]),
                person); 
        }
    }
}
