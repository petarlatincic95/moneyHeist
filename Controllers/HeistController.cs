using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyHeistAPI.Data;
using MoneyHeistAPI.DTO_Helper_classes;

namespace MoneyHeistAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class HeistController : ControllerBase
    {
        private readonly HeistDbContext heistDbContext;

        public HeistController(HeistDbContext heistDbContext)
        {
            this.heistDbContext = heistDbContext;
        }


        [HttpPost]
        public async Task AddHeist(HeistDTO request)
        {
            var helperObject = new HeistHelperClass(heistDbContext);
           
           await helperObject.AddHeist(request);
           






        }
        [HttpPut("{id}/Skills")]
        public async Task<IActionResult> Skills(int id,HeistSkillDTO request)
        { 
        var helperObject= new HeistHelperClass(heistDbContext);
            if (await helperObject.UpdateHeistSkill(id, request))
                return Ok();
            else
                return BadRequest();
              
        
        
        }


        [HttpPut("{heist_id}/Members")]
        public async Task<IActionResult> Members(int heist_id, List<string> membersToConfirm)
        {
            var temporObject = new HeistHelperClass(heistDbContext);
            if (await temporObject.ConfirmHeistMembers(heist_id, membersToConfirm) == true)
                return Ok("Members confirmed successfully.");
            else
                return NotFound("Not possible to confirm members!");





        }
        [HttpPut("{heist_id}/start")]

        public async Task<IActionResult> StartHeistManually(int heist_id)
        { 
            var temproObject=new HeistHelperClass(heistDbContext);
            if (await temproObject.StartHeist(heist_id) == true)
            {
               // temproObject.MessageMembervieEmail("tokio@heist.com", "Get your lazy ass up, heist is starting");
                return Ok();
            }
            else
                return NotFound("Not possible to start Heist!");
           

        }
        


        [HttpPut("startAutomatically")]

        public async Task<IActionResult> StartHeistAutomatically(int heist_id,DateTime startTime,DateTime endTime)
        {
          var temporObject=new HeistHelperClass(heistDbContext);
           if( temporObject.IsTimeInputInCorrectForm(startTime)!=true|| temporObject.IsTimeInputInCorrectForm(endTime) != true)
                return BadRequest();
            else 
            {
                if (temporObject.IsTimeInputBeforeNow(startTime) == true || temporObject.IsTimeInputAfterNow(endTime) == false)
                    return BadRequest();
                else
                {
                   if(await  temporObject.StartHeistautomatically(heist_id,startTime,endTime)!=true)
                   return BadRequest("Heist not possible to execute, invalid input");
                    else 
                    return Ok("Heist started successfully");
                }
                
            
            }





        }
        [HttpGet("{heist__id}")]
        public async Task<IActionResult> GetHeistbyId(int heist__id)
        {
            var temproObject = new HeistHelperClass(heistDbContext);
            return Ok(await temproObject.GetHeist(heist__id));



        }

        [HttpGet("outcome")]
        public async Task<IActionResult> HeistOutcome(int heist_id)
        {
            var temproObject = new HeistHelperClass(heistDbContext);
            return Ok(await temproObject.HeistOutcome(heist_id));


        }


        [HttpGet("{heist_id}/eligible_members")]
        public async Task<IActionResult> EligibleMembers(int heist_id)
        {
            var helperObject = new HeistHelperClass(heistDbContext);
           var output=  await helperObject.Get_Eligible_Members(heist_id);
            if (output.Count == 0)
                return NoContent();
            else
                return Ok(output);
            
        
        
        
        }
        [HttpGet("{heist_id}/members")]

        public async Task<IActionResult> GetMembers(int heist_id)
        {
            var temproObj = new HeistHelperClass(heistDbContext);
            var heist = await temproObj.GetHeist(heist_id);
            var members = heist.Members;
            return Ok(members);
        
        }

        [HttpGet("{heist_id}/skills")]

        public async Task<IActionResult> GetHeistSkills(int heist_id)
        {
            var temproObj = new HeistHelperClass(heistDbContext);
            var heist = await temproObj.GetHeist(heist_id);
            var skills = heist.HeistSkills;
            return Ok(skills);


        }
        [HttpGet("{heist_id}/status")]

        public async Task<IActionResult> GetHeistStatus(int heist_id)
        {
            var temproObj = new HeistHelperClass(heistDbContext);
            var heist = await temproObj.GetHeist(heist_id);
            var status = heist.Status;
         
            return Ok(status);
           


        }
        //[HttpGet("send_mail")]
        //public async Task<IActionResult> SendEmail(string email, string message)
        //{ 
         
           
        
        
        //}






    }
    }
   

    


