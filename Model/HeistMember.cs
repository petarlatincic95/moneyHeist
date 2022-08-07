namespace MoneyHeistAPI.Model
{
    public class HeistMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }


        public string Email { get; set; }

        public string MainSkill { get; set; }
        public string StatusField { get; set; }

        // Navigation properties


        public List<MemberSkill> MemberSkills { get; set; } = new List<MemberSkill>();
    
    }
}
