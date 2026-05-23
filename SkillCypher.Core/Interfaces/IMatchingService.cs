namespace SkillCypher.Core.Interfaces{

    public interface IMatchingService{
        Task TriggerApplicantMatchAsync(int applicationId);
        Task TriggerJobMatchAsync(int jobId);   
    }
}