using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp1.classes.PassengerTrain;

namespace WindowsFormsApp1.classes
{
    class PassengerCar : Car
    {
        [Flags]
        public enum BodyShape { Hatchback, Pickup, Limousine, Coupe, Sedan, Minivan };
        [Name("Body Shape")]
        public BodyShape bodyShape;
        [Name("Number Of Seats")]
        public int numberOfSeats;

        public PassengerCar() { }

        public PassengerCar(BodyShape _bodyShape, int _numberOfSeats, int _horsePower, int _numberOfWheels, int _torgue, Color _color, string _model, int _maxSpeed, Person _driver) :
                      base(_horsePower, _numberOfWheels, _torgue, _color, _model, _maxSpeed, _driver)
        {
            this.bodyShape = _bodyShape;
            this.numberOfSeats = _numberOfSeats;
        }
    }
}
