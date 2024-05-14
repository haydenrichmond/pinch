namespace Elevator.Models
{
    public class Floor
    {
        public Floor(int floorNumber)
        {
            FloorNumber = floorNumber;
        }

        /// <summary>
        /// 0 denotes Ground floor (G)
        /// </summary>
        public int FloorNumber { get; set; }

        public Button Button { get; set; }
    }
}
