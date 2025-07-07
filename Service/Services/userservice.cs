using project2025.Models;
using project2025.Models.Responces;
using project2025.Repository;

namespace project2025.Service.Services
{
    public class userservice : Iservice
    {
        public readonly Irepository _repository;
        public userservice(Irepository repository)
        {
            _repository = repository;
        }

        public List<User> getUserDetails()
        {
            return _repository.getUserDetails();
        }
        public PostResponse PostSomeDetails(User user)
        {
            return _repository.PostSomeDetails(user);
        }

        public Task<PostResponse> PostLearnData(LearnPost learn)
        {
            return _repository.PostLearnData(learn);
        }

        public List<LearnPost> GetLearnData(int groupid)
        {
            return _repository.GetLearnData(groupid);
        }

        public PostResponse DeletedLearningById(int id)
        {
            return _repository.DeletedLearningById(id);
        }
        public PostResponse EditLearn(LearnPost learnPost)
        {
            return _repository.EditLearn(learnPost);
        }

        public LoginResponse Login(User user)
        {
            return _repository.Login(user);
        }

        public List<Group> getAllGroups()
        {
            return _repository.getAllGroups();
        }
        public PostResponse CreateGroup(Group group)
        {
            return _repository.CreateGroup(group); ;
        }
        public PostResponse DeleteGroup(int Id)
        {
            return _repository.DeleteGroup(Id);
        }

        public PostResponse CheckGroup(int user_id, int group_id)
        {
            return _repository.CheckGroup(user_id, group_id);
        }

        public PostResponse RequesAccess(RequestAcessPayload payload)
        {
            return _repository.RequesAccess(payload);
        }
        public List<RequestList> GetRequestLists(int userId)
        {
            return _repository.GetRequestLists(userId);
        }

        public PostResponse GrandAccess(RequestList access)
        {
            return _repository.GrandAccess(access);
        }
        public List<UsernameById> GetUsers(int[] userIDs)
        {
            return _repository.GetUsers(userIDs);
        }
        public List<GroupById> getGroups(int[] groupIds)
        {
            return _repository.getGroups(groupIds);
        }
        public PostResponse GrandAccessByMailId(AccessByMailId accessByMailId)
        {
            return _repository.GrandAccessByMailId(accessByMailId);
        }
        public int GetGroupIdByLearnId(int learnId)
        {
            return _repository.GetGroupIdByLearnId(learnId);
        }

        public UserPayload getUsersByGroupId(int groupId)
        {
            return _repository.getUsersByGroupId(groupId);
        }

        public PostResponse AddXo(AddXO addXO)
        {
            return _repository.AddXo(addXO);
        }
        public GetXOResponse xogame_Tables()
        {
            return _repository.xogame_Tables();
        }
        public PostResponse ClearXo()
        {
            return _repository.ClearXo();
        }
        public PostResponse PostFortess(Fortess[] postFortess)
        {
            return _repository.PostFortess(postFortess);
        }


    }
}
