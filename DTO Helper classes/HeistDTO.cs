namespace MoneyHeistAPI.DTO_Helper_classes
{
    public class HeistDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartTimeISO8601 { get; set; }
        public DateTime EndTimeISO8601 { get; set; }
        public string SkillName { get; set; }
        public string SkillLevel { get; set; }
        public int NumberOfMembers { get; set; }

    }
}
