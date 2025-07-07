using project2025.Models;
using project2025.Models.Responces;

namespace project2025.Repository
{
    public interface Irepository
    {
        List<User> getUserDetails();
        PostResponse PostSomeDetails(User user);
        Task<PostResponse> PostLearnData(LearnPost learn);
        public List<LearnPost> GetLearnData(int groupid);
        public PostResponse DeletedLearningById(int id);
        public PostResponse EditLearn(LearnPost learnPost);
        public LoginResponse Login(User user);
        public List<Group> getAllGroups();
        public PostResponse CreateGroup(Group group);
        public PostResponse DeleteGroup(int Id);
        public PostResponse CheckGroup(int user_id, int group_id);
        public PostResponse RequesAccess(RequestAcessPayload payload);
        public List<RequestList> GetRequestLists(int userId);
        public PostResponse GrandAccess(RequestList access);
        public List<UsernameById> GetUsers(int[] userIDs);
        public List<GroupById> getGroups(int[] groupIds);
        public PostResponse GrandAccessByMailId(AccessByMailId accessByMailId);
        public int GetGroupIdByLearnId(int learnId);
        public UserPayload getUsersByGroupId(int groupId);
        public PostResponse AddXo(AddXO addXO);
        public GetXOResponse xogame_Tables();
        public PostResponse ClearXo();
        public PostResponse PostFortess(Fortess[] postFortess);

    }
}
