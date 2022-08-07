using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyHeistAPI.Data;
using MoneyHeistAPI.DTO_Helper_classes;
using MoneyHeistAPI.Helper_Methodes;
using MoneyHeistAPI.Model;

namespace MoneyHeistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberSkillsController : ControllerBase
    {
        private readonly HeistDbContext _heistDbContext;

        public MemberSkillsController(HeistDbContext heistDbContext)
        {
            _heistDbContext = heistDbContext;
        }

        [HttpPut()]
        public async Task<IActionResult> Put(HeistMemberSkillsDTO request)
        {
            var heist_member_skill_chechk = new HeistMemberSkillHelperClass(_heistDbContext);
            if (await heist_member_skill_chechk.AddSkillToMember(request) == false)
                return BadRequest();
            else
                return Ok();

        }


     
        [HttpDelete("MemberId,SkillName")]
        public async Task<IActionResult> DeleteMemberSkill(int MemberId,string SkillName)
        {
            var heist_member_skill_chechk = new HeistMemberSkillHelperClass(_heistDbContext);
            if (await heist_member_skill_chechk.DeleteSkillFromMember(MemberId, SkillName) == false)
                return BadRequest();
            else 
                return Ok();




        }

    }
}
