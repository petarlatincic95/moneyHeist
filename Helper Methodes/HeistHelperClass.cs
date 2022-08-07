
using System.Linq;
using System.Net.Mail;


using System.Net.Mail;
using FluentEmail.Smtp;
using FluentEmail.Core;

namespace MoneyHeistAPI.Helper_Methodes
{

    public class HeistHelperClass
    {
        private PeriodicTimer _timer;
        private readonly HeistDbContext _dbContext;
        //public delegate bool ImproveSkill(HeistMember member);
        //public ImproveSkill imprSkill;


        public HeistHelperClass(HeistDbContext dbContext)
        {
            _dbContext = dbContext;
        }




        public async Task<bool> AddHeist(HeistDTO request)
        {

            var newHeist = new Heist();
            var newHeistSkill = new HeistSkill();
            newHeistSkill.SkillName = request.SkillName;

            if (SkillLevelCheck(request) == true)
            {
                newHeistSkill.SkillLevel = request.SkillLevel;

                newHeistSkill.RequiredMembers = request.NumberOfMembers;
                await _dbContext.HeistSkills.AddAsync(newHeistSkill);
                await _dbContext.SaveChangesAsync();

                newHeist.Name = request.Name;
                newHeist.Location = request.Location;
                try
                {

                    newHeist.StartTime = request.StartTimeISO8601.ToString("yyyy-MM-ddTHH:mm:ssK");
                    newHeist.EndTime = request.EndTimeISO8601.ToString("yyyy-MM-ddTHH:mm:ssK");
                }
                catch
                {
                    throw new Exception("Invalid time formats");


                }
                var AddedSkill = await _dbContext.HeistSkills.Where(x => x.SkillLevel == request.SkillLevel && x.SkillName == request.SkillName).FirstAsync();
                await _dbContext.Heists.AddAsync(newHeist);
                await _dbContext.SaveChangesAsync();


                newHeist.HeistSkills.Add(AddedSkill);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            else
                return false;


        }





        public bool SkillLevelCheck(HeistDTO request)
        {
            bool isRequestValid = true;
            var requestSkillLevel = request.SkillLevel.ToCharArray();

            foreach (var item in requestSkillLevel)
            {
                if (item != '*' || requestSkillLevel.Length > 10)
                    isRequestValid = false;



            }
            return isRequestValid;
        }
        public bool SkillLevelCheck(HeistSkillDTO request)
        {
            bool isRequestValid = true;
            var requestSkillLevel = request.Level.ToCharArray();

            foreach (var item in requestSkillLevel)
            {
                if (item != '*' || requestSkillLevel.Length > 10)
                    isRequestValid = false;



            }
            return isRequestValid;
        }




        public async Task<bool> UpdateHeistSkill(int id, HeistSkillDTO request)
        {

            var heistToAddSkill = await _dbContext.Heists.Where(x => x.Id == id).Include(y => y.HeistSkills).Include(c => c.Members).FirstAsync();
            if (heistToAddSkill.HeistSkills.Count == 0)
                return false;
            else if (SkillLevelCheck(request) == false)
                return false;
            else
            {
                bool isSkillUnique = true;
                var skillToAdd = new HeistSkill();


                skillToAdd.SkillName = request.Name;
                skillToAdd.SkillLevel = request.Level;
                skillToAdd.RequiredMembers = request.Members;


                var skills = await _dbContext.HeistSkills.ToListAsync();

                for (int i = 0; i < skills.Count; i++)
                {
                    if (skills[i].SkillName == skillToAdd.SkillName && skills[i].SkillLevel == skillToAdd.SkillLevel)
                        isSkillUnique = false;
                }

                if (isSkillUnique == true)
                {
                    await _dbContext.HeistSkills.AddAsync(skillToAdd);
                    await _dbContext.SaveChangesAsync();


                    heistToAddSkill.HeistSkills.Add(skillToAdd);
                    await _dbContext.SaveChangesAsync();
                }
                return isSkillUnique;

            }

        }
        public async Task<bool> StartHeist(int heist_id)
        {
            var heistToStart = await _dbContext.Heists.Where(x => x.Id == heist_id).Include(y => y.Members).FirstAsync();
            if ((await _dbContext.Heists.FindAsync(heist_id) == null) || heistToStart.Status != "PLANNING")
                return false;
            else if (heistToStart.Members.Count == 0)    //We don't allow to start heist if there are no members confirmed.
                return false;
            else
            {

                heistToStart.Status = "IN_PROGRESS";
                await _dbContext.SaveChangesAsync();

                for (int i = 0; i < heistToStart.Members.Count; i++)
                {
                    await MessageMembervieEmail(heistToStart.Members.ElementAt(i).Email, heistToStart.Location.ToString() ,"You are  confirmed for Heist."); ;
                
                
                
                }


                return true;


            }




        }



        public async Task<List<HeistMember>> Get_Eligible_Members(int heistId)
        {

            var desiredHeist = await _dbContext.Heists.Where(x => x.Id == heistId).Include(j => j.HeistSkills).Include(k => k.Members).FirstAsync();                //Check id for possible null refernce if heist with given id not exist

            var potentialMembers = await (from member in _dbContext.HeistMembers
                                          where member.StatusField == "AVAILABLE" || member.StatusField == "RETIRED"
                                          select member).Include(h => h.MemberSkills).ToListAsync();
            var narrowedPotentialMembers = new List<HeistMember>();

            for (int i = 0; i < desiredHeist.HeistSkills.Count; i++)
            {
                for (int j = 0; j < potentialMembers.Count; j++)
                {

                    if (
                        potentialMembers[j].MemberSkills.Any(x => x.SkillName == desiredHeist.HeistSkills[i].SkillName)
                                                &&
                        potentialMembers[j].MemberSkills.Any(x => x.SkillLevel.Length >= desiredHeist.HeistSkills[i].SkillLevel.Length)
                        )

                        narrowedPotentialMembers.Add(potentialMembers[j]);



                }


            }
            return narrowedPotentialMembers;
        }
        public async Task<bool> IsMemberAlreadyParticipatingHeist(int id)
        {
            var Heists = await _dbContext.Heists.Include(x => x.Members).ToListAsync();
            int counter = 0;
            var heistMember = await _dbContext.HeistMembers.Where(x => x.Id == id).Include(x => x.MemberSkills).FirstAsync();
            for (int i = 0; i < Heists.Count; i++)
            {
                for (int j = 0; j < Heists[i].Members.Count; j++)
                {
                    if (Heists[i].Members.Contains(heistMember))
                        counter++;
                }


            }
            if (counter != 0)
                return true;
            else
                return false;



        }
        public async Task<bool> ConfirmHeistMembers(int id, List<string> members)
        {

            var heistToaddMembers = await _dbContext.Heists.Where(x => x.Id == id).Include(y => y.Members).FirstAsync();

            var allMembers = await _dbContext.HeistMembers.Include(x => x.MemberSkills).ToListAsync();
            for (int i = 0; i < allMembers.Count; i++)
            {
                if ((await IsMemberAlreadyParticipatingHeist(allMembers[i].Id) == true) && members.Contains(allMembers[i].Name))

                    members.RemoveAll(y => y == allMembers[i].Name);      //We exclude all members that participate in some other heist

            }


            var eligibleMembers = await Get_Eligible_Members(id);


            if ((await HeistStatus(id) == false))
                return false;                     //If Heist with given id is not found
            else if (members.Count==0)
                return false;                     //If there is no members to add 
            else
            {
                for (int i = 0; i < members.Count; i++)
                {

                    if (eligibleMembers.Exists(x => x.Name == members[i]))
                    {
                        var item = await _dbContext.HeistMembers.Where(x => x.Name == members[i]).FirstAsync();
                        heistToaddMembers.Members.Add(item);  //Adding member to heist

                        await _dbContext.SaveChangesAsync();

                    }

                }


                return true;



            }
        }
        public async Task<bool> HeistStatus(int heistId)
        {


            var heist = await _dbContext.Heists.Where(x => x.Id == heistId).FirstAsync();
            if (heist.Status == "PLANNING")
                return true;
            else
                return false;


        }
        public bool IsTimeInputInCorrectForm(DateTime input)
        {
            try
            {

                input.ToString("yyyy - MM - ddTHH:mm: ssK");
                return true;
            }
            catch
            {
                throw new Exception("Invalid time format");


            }



        }
        public bool IsTimeInputBeforeNow(DateTime timeInput)
        {

            if (timeInput <= DateTime.Now)
                return true;
            else
                return false;


        }

        public bool IsTimeInputAfterNow(DateTime timeInput)
        {

            if (timeInput >= DateTime.Now)
                return true;
            else
                return false;

        }

        public string ImproveSkillLevel(string skillLevel)
        {
            if (skillLevel.Length < 10)
            {
                var newSkillLevel = skillLevel.Append('*');

                return (string)newSkillLevel;
            }
            else return String.Empty;



        }



        public async Task<bool> StartHeistautomatically(int heist_id, DateTime startTime, DateTime endTime)
        {

            
            var heist =await _dbContext.Heists.Where(x => x.Id == heist_id).Include(y => y.Members).ThenInclude(c => c.MemberSkills).FirstAsync();
            if (heist.Status != "PLANNING" || heist.Members.Count == 0)

                return false;                                     // If Heist allreay started or there are no members that should participate in heist
            else
            {
                do
                {


                }
                while (startTime > DateTime.Now);
                heist.Status = "IN_PROGRESS";
                await _dbContext.SaveChangesAsync();

                for (int i = 0; i < heist.Members.Count; i++)
                {
                    await MessageMembervieEmail(heist.Members.ElementAt(i).Email, heist.Location.ToString(), "You are  confirmed for Heist."); ;



                }
                _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));//(86400)

                do
                {
                    //if (_timer.Equals(10))
                    //{
                        for (int i = 0; i < heist.Members.Count; i++)
                       {
                        for(int j=0;j<heist.Members.ElementAt(i).MemberSkills.Count;j++)
                        {
                           if(heist.HeistSkills.Exists(x=>(x.SkillName==heist.Members.ElementAt(i).MemberSkills.ElementAt(j).SkillName)&&
                                x.SkillLevel== heist.Members.ElementAt(i).MemberSkills.ElementAt(j).SkillLevel))

                            await IncreaseMemberSkillLevel(heist.Members.ElementAt(i).Id, heist.Members.ElementAt(i).MemberSkills.ElementAt(j));
                        }
                           
                       }


                    //}
                    

                }
                while (endTime > DateTime.Now);
                _timer.Dispose();
                heist.Status = "FINISHED";
                await _dbContext.SaveChangesAsync();

                if (heist.Status == "FINISHED")
                    return true;
                else
                    return false;
            }

        }

        private async Task<bool> IncreaseMemberSkillLevel(int id,MemberSkill skill)
        {

            var membertoUpdate = await _dbContext.HeistMembers.Where(x => x.Id == id).Include(y => y.MemberSkills).FirstAsync();
            
            {
                if (skill.SkillLevel.Length == 10)
                {

                }


                else
                {  
                    var newSkill = new MemberSkill();
                    
                    newSkill.SkillLevel = skill.SkillLevel.Insert(skill.SkillLevel.Length-1,"*");
                    newSkill.SkillName = skill.SkillName;
                    await _dbContext.MemberSkills.AddAsync(newSkill);
                    await _dbContext.SaveChangesAsync();
                    
                    


                }



            }
            return true;







        }

        public async Task<bool> MessageMembervieEmail(string sendTo,string heistName,string message )

        {
            var sender = new SmtpSender(() => new SmtpClient(host: "localhost")
            {

                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 25
                //DeliveryMethod=SmtpDeliveryMethod.SpecifiedPickupDirectory,
                //PickupDirectoryLocation= @"C:\MoneyHeist_EmailTestFolder"


            }); ;
            Email.DefaultSender = sender;
            var email = await Email
                .From(emailAddress: "petar@lat.com")
                .To(emailAddress: sendTo,
                 name: "Test")
                .Subject(subject: "Confirmation for " +heistName+ "Heist")
                .Body(body: message)
                .SendAsync();

            return true;
        }










        public async Task<string> HeistOutcome(int id)
        {
            var heist = await _dbContext.Heists.Where(x => x.Id == id).Include(y => y.Members).Include(z => z.HeistSkills).FirstAsync();
            int numberOfRequiredMembers = 0;
            Random rd = new Random();
            for (int i = 0; i < heist.HeistSkills.Count; i++)
            {
                numberOfRequiredMembers += heist.HeistSkills[i].RequiredMembers;

            }
            var numberOfMembers = heist.Members.Count;

            if (numberOfMembers / numberOfRequiredMembers < 0.5)
            {
                for (int i = 0; i < heist.Members.Count; i++)
                {
                   
                    int rand_num = rd.Next(0, 1);
                    if (rand_num <= 0.5)
                    {
                        heist.Members.ElementAt(i).StatusField = "INCARCERATED";
                        //await _dbContext.SaveChangesAsync();

                    }
                    else if(rand_num>0.5)
                    {
                        heist.Members.ElementAt(i).StatusField = "EXPIRED";
                       // await _dbContext.SaveChangesAsync();
                    }


                }
                return "FAILED";

            }
            else if (((numberOfMembers / numberOfRequiredMembers) > 0.5) && ((numberOfMembers / numberOfRequiredMembers) < 0.75))
            {

                for (int i = 0; i < heist.Members.Count; i++)
                {
                  
                    int rand_num1 = rd.Next(0, 1);
                    int rand_num2 = rd.Next(0, 1);

                    if (rand_num1 < 0.33)
                    {

                    }
                    else if (rand_num1 >= 0.33)

                    {
                        if (rand_num2 <= 0.5)
                        {
                            heist.Members.ElementAt(i).StatusField = "INCARCERATED";
                            //await _dbContext.SaveChangesAsync();

                        }
                        else if(rand_num2 >0.5)
                        {
                            heist.Members.ElementAt(i).StatusField = "EXPIRED";
                            //await _dbContext.SaveChangesAsync();
                        }

                    }




                }
                return "FAILED";
            }

            else if (((numberOfMembers / numberOfRequiredMembers) >= 0.75) && ((numberOfMembers / numberOfRequiredMembers) < 1))
            {

                for (int i = 0; i < heist.Members.Count; i++)
                {
              
                    int rand_num1 = rd.Next(0, 1);
                    int rand_num2 = rd.Next(0, 1);

                    if (rand_num1 < 0.667)
                    {

                    }
                    else if (rand_num1 >= 0.667)

                    {
                        if (rand_num2 <= 0.5)
                        {
                            heist.Members.ElementAt(i).StatusField = "INCARCERATED";
                            //await _dbContext.SaveChangesAsync();

                        }
                        else if(rand_num2 > 0.5)
                        {
                            heist.Members.ElementAt(i).StatusField = "EXPIRED";
                           // await _dbContext.SaveChangesAsync();
                        }

                    }




                }

                return "SUCCEEDED";


            }

            else if ((numberOfMembers / numberOfRequiredMembers) == 1)
                return "SUCCEEDED";
            else
                return "INVALID OPERATION";
        }
                    
        public async Task<Heist> GetHeist(int id)
        {

              
           
                return (await _dbContext.Heists.Where(x => x.Id == id).Include(y => y.HeistSkills).Include(g=>g.Members).ThenInclude(o=>o.MemberSkills).FirstAsync());
            
                    
                    
             
               

            //var temporHeist =await _dbContext.Heists.Where(x => x.Id.Equals(id)).Include(y => y.HeistSkills).FirstAsync();

        
    
        
        
        
        }
    }

}
