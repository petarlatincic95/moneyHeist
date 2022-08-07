using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoneyHeistAPI.Data;
using MoneyHeistAPI.DTO_Helper_classes;
using MoneyHeistAPI.Helper_Methodes;
using MoneyHeistAPI.Model;

namespace MoneyHeistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeistMember : ControllerBase

    {



        public HeistMember(HeistDbContext heistDbContext)
        {
            _heistDbContext = heistDbContext;



        }
        private readonly HeistDbContext _heistDbContext;





        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var helperObject = new HeistMemberHelperClass(_heistDbContext);
            var output = await helperObject.GetAllHeistMembers();

            return Ok(output);

        }
        [HttpGet("{member_id}")]
        public async Task<IActionResult> GetById(int member_id)
        {
            var helperObject = new HeistMemberHelperClass(_heistDbContext);
            var output = await helperObject.GetMemberById(member_id);

            return Ok(output);



        }


        [HttpGet("{member_id}/skills")]
        public async Task<IActionResult> GetMemberSkill(int member_id)
        { 
         var helperObject=new HeistMemberHelperClass(_heistDbContext);

            return Ok(await helperObject.GetMemberSkillsAsync(member_id));
            
        
        
        }





        [HttpPost]
        public async Task<IActionResult> Post(HeistMemberDTO request)
        {
            var helperObject = new HeistMemberHelperClass(_heistDbContext);
           
            bool isrequestOk = await helperObject.HeistMemberRquestCheck(request);

            if (isrequestOk==false)
                return BadRequest();
            else
            {
                await helperObject.AddMemberToDatabase(request);
                return Ok();
            }

        }


    }
}

        

    




