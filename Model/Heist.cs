namespace MoneyHeistAPI.Model
{
    public class Heist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public  string StartTime { get; set; }
        public string EndTime { get; set; }
        public string? Status { get; set; } = "PLANNING";

        //Navigation properties
        public List<HeistSkill> HeistSkills { get; set; } = new List<HeistSkill>();
        public ICollection<HeistMember> Members { get; set; }=new List<HeistMember>();
    }
}
