namespace MoneyHeistAPI.Model
{
    public class HeistSkill
    {
        public int Id { get; set; }
        public string SkillName { get; set; }
        public string SkillLevel { get; set; }
        public int RequiredMembers { get; set; }
        //Navigation Properties
        [JsonIgnore]
        public List<Heist> Heists { get; set; } = new List<Heist>();

    }
}
