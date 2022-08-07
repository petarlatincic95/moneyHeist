using MoneyHeistAPI.Data;
using MoneyHeistAPI.DTO_Helper_classes;

namespace MoneyHeistAPI.Helper_Methodes
{
    public class HeistMemberSkillHelperClass
    {
        private readonly HeistDbContext _heistDbContext;
        public bool isRequestValid { get; set; } = true;

        public HeistMemberSkillHelperClass(HeistDbContext heistDbContext)
        {
            _heistDbContext = heistDbContext;
        }

        public async Task<bool> DoesMemberSkillAlreadyExist(HeistMemberSkillsDTO request)
        {
            var isRequestValid = false;
            var tempor = await _heistDbContext.MemberSkills.ToListAsync();
            for (int i = 0; i < tempor.Count; i++)
            {
                if (tempor[i].SkillName == request.SkillName && tempor[i].SkillLevel == request.SkillLevel)
                    

                    isRequestValid = true;

            }
            return isRequestValid;



        }
        public bool NewMemberSkillRequestCheck(HeistMemberSkillsDTO request)
        {

            var isRequestValid = false;
            if (request.SkillLevel.All(x => x == '*') && (request.SkillName.Length>0 && request.SkillName!="string"))
            
                isRequestValid = true;
            
                
                    
            
            return isRequestValid;

        }

        private async Task<bool> SkillNameCheck(string SkillName)
        {
            bool nameExistsIDB = false;
            var skillNamesFromDb = await (from skills in _heistDbContext.MemberSkills
                                          select skills.SkillName).ToListAsync();
            for (int i = 0; i < skillNamesFromDb.Count; i++)
            {
                if (skillNamesFromDb[i] == SkillName)
                    nameExistsIDB = true;

            }

            return nameExistsIDB;

        }
        public async Task<bool> AddSkillToMember(HeistMemberSkillsDTO request)
        {

            var heist_member_skill_chechk = new HeistMemberSkillHelperClass(_heistDbContext);
            var heistMemberSkill = new MemberSkill();
            var memberToadSkillTo = await _heistDbContext.HeistMembers.Where(x => x.Id == request.MemberId)
                                                                          .Include(y => y.MemberSkills).FirstAsync();

            if (( heist_member_skill_chechk.NewMemberSkillRequestCheck(request) == false) && (await DoesMemberSkillAlreadyExist(request) == false))
            {

                return (false);
            }
            else if (await DoesMemberSkillAlreadyExist(request) == true )
            {
                var skillToAddToMember = await _heistDbContext.MemberSkills
                                        .Where(x => x.SkillName == request.SkillName && x.SkillLevel == request.SkillLevel).FirstAsync();

                memberToadSkillTo.MemberSkills.Add(skillToAddToMember);
                await _heistDbContext.SaveChangesAsync();

                return true;

            }

            else if ((await DoesMemberSkillAlreadyExist(request) == false) &&  NewMemberSkillRequestCheck(request) == true)
            {
                heistMemberSkill.SkillName = request.SkillName;
                heistMemberSkill.SkillLevel = request.SkillLevel;
                await _heistDbContext.MemberSkills.AddAsync(heistMemberSkill);
                memberToadSkillTo.MemberSkills.Add(heistMemberSkill);
                await _heistDbContext.SaveChangesAsync();
                return true;

            }
            else
                return false;
                

            }

        public  async Task<bool> DeleteSkillFromMember(int MemberId, string SkillName)
        {
            
            var temporObj = new HeistMemberSkillHelperClass(_heistDbContext);
            bool temp = await temporObj.SkillNameCheck(SkillName);
            var memberToDeleteSkillFrom = _heistDbContext.HeistMembers.Where(c => c.Id == MemberId).Include(f => f.MemberSkills).FirstOrDefault();


            if (memberToDeleteSkillFrom == null)
                return (false);


            else if (temp == true)

            {

                memberToDeleteSkillFrom.MemberSkills.RemoveAll(h => h.SkillName == SkillName);


                await _heistDbContext.SaveChangesAsync();


                return true;
            }
            else
                return false;
        }
    }
}
