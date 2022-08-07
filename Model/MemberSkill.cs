

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoneyHeistAPI.Model
{
    public class MemberSkill
    {
        public int Id { get; set; }

        public string SkillName { get; set; }
        public string SkillLevel { get; set; }


        //Navigation properties
        [JsonIgnore]

        public List<HeistMember> HeistMembers { get; set; } = new List<HeistMember>();


    }
}
