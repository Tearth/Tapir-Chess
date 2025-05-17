using Tapir.Core.Domain;

namespace Tapir.Services.Games.Domain.Rooms.ValueObjects
{
    public class TimeControl : ValueObject
    {
        public int Time { get; set; }
        public int Increment { get; set; }

        public TimeControl(int time, int increment)
        {
            Time = time;
            Increment = increment;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Time;
            yield return Increment;
        }
    }
}
