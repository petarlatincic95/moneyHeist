using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyHeistAPI.Controllers;
using MoneyHeistAPI.Data;
using MoneyHeistAPI.DTO_Helper_classes;
using MoneyHeistAPI.Model;
using System.Linq;

namespace MoneyHeistAPI.Helper_Methodes
{

    public class HeistMemberHelperClass
    {
        private readonly HeistDbContext _heistDbContext;

        private bool isRequestValid = true;


        public HeistMemberHelperClass(HeistDbContext heistDbContext)
        {
            _heistDbContext = heistDbContext;
        }
        public HeistMemberHelperClass()
        {

        }
        public async Task <bool> HeistMemberRquestCheck(HeistMemberDTO request)
        {
            if (
            RequestSexCheck(request.Sex) == false ||
             await RequestEmailCheck(request) == false ||
            RequestStatusFieldCheck(request) == false ||
             RequestStatusFieldCheck(request) == false ||
            SkillLevelCheck(request) == false
                )

                return false;


            else
                return true;

        }
        private bool RequestSexCheck(string sex)
        {
            if (sex.ToUpper() == "M" || sex.ToUpper() == "F")
                return true;
            else return false;



        }
        private async Task<bool> RequestEmailCheck(HeistMemberDTO request)

        {
            
            var temporOutput =  await _heistDbContext.HeistMembers.ToListAsync();
            if (temporOutput.FindAll(x => x.Email == request.Email).Count==0)
                return true;
            else 
                return false;
            

        }

        private bool RequestStatusFieldCheck(HeistMemberDTO request)
        {
            var isRequestStatusFieldOK = false;
            if (                   (request.StatusField.ToUpper() == "AVAILABLE")
                                || (request.StatusField.ToUpper() == "EXPIRED")
                                || (request.StatusField.ToUpper() == "INCARCERATED")
                                || (request.StatusField.ToUpper() == "RETIRED"))

            {
                isRequestStatusFieldOK = true;
                return isRequestStatusFieldOK;
            }


            else
                return isRequestStatusFieldOK;

        }
        private bool SkillLevelCheck(HeistMemberDTO request)
        {
            var requestSkillLevel = request.SkillLevel.ToCharArray();
            var isSkillLevelOK = true;
            foreach (var item in requestSkillLevel)
            {
                if ((item != '*' )|| requestSkillLevel.Count() > 10)
                    isSkillLevelOK = false;


            }

            return isSkillLevelOK;   


        }

        

            public async Task<List<Model.HeistMember>> GetAllHeistMembers()
        {

            var output = await (_heistDbContext.HeistMembers
            .Include(e => e.MemberSkills)
            .ToListAsync());

            return output;




        }
        public async Task<Model.HeistMember> GetMemberById(int id)
        {
            var output = await (_heistDbContext.HeistMembers.Where(e => e.Id == id).Include(k => k.MemberSkills)).FirstAsync();
            var dummySkilLevel = "*";
            var dummySkillName = "";
            for (int i = 0; i < output.MemberSkills.Count; i++)
            {
                if (output.MemberSkills[i].SkillLevel.Length >= dummySkilLevel.Length)
                {
                    dummySkilLevel = output.MemberSkills[i].SkillLevel;
                    dummySkillName = output.MemberSkills[i].SkillName;
                }



            }
            output.MainSkill = dummySkillName;             
            return output;

        }


        public async Task<bool> AddMemberToDatabase(HeistMemberDTO request)

        {
            var newHeistMember = new Model.HeistMember();
            var newMemberSkill = new MemberSkill();
            var requestCheck = new HeistMemberHelperClass(_heistDbContext);


            

           


                newMemberSkill.SkillLevel = request.SkillLevel;
                newMemberSkill.SkillName = request.SkillName;

                await _heistDbContext.MemberSkills.AddAsync(newMemberSkill);
                await _heistDbContext.SaveChangesAsync();

              
                newHeistMember.StatusField = request.StatusField.ToUpper();
                newHeistMember.Email = request.Email;
                newHeistMember.Name = request.Name;
                newHeistMember.Sex = request.Sex.ToUpper();
                newHeistMember.MainSkill = request.SkillName;
                


                

                var idOfAddedSkill = await (from skill in _heistDbContext.MemberSkills
                                            where skill.SkillName == request.SkillName && skill.SkillLevel == request.SkillLevel
                                            select skill.Id).FirstOrDefaultAsync();



                var skillll = await _heistDbContext.MemberSkills.FindAsync(idOfAddedSkill);

                newHeistMember.MemberSkills.Add(skillll);
                await _heistDbContext.HeistMembers.AddAsync(newHeistMember);
               
                await _heistDbContext.SaveChangesAsync();
                return (true);




            

        }
        public async Task<List<MemberSkill>> GetMemberSkillsAsync(int id)
        {
            var temporMember =await  _heistDbContext.HeistMembers.Where(x => x.Id.Equals(id)).Include(y => y.MemberSkills).FirstAsync();
            return (temporMember.MemberSkills);
            
        
        
        
        }


    }
}
