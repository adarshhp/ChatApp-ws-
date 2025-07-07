using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project2025.Models;
using project2025.Models.Responces;
using project2025.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using project2025.Middleware;
using project2025.Service.Services;

namespace project2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usercontroller : ControllerBase
    {
        public readonly Iservice _service;
        private readonly SseService _sseService;

        public usercontroller(Iservice service, SseService sseService)
        {
            _service = service;
            _sseService = sseService;
        }



        [HttpGet("thegetapi")]
        public List<User> getUserDetails()
        {
            return _service.getUserDetails();
        }

        [HttpGet("getgroups")]
        public List<Group> getAllGroups()
        {
            return _service.getAllGroups();
        }

        [HttpPost("createGroup")]
        public async Task<PostResponse> CreateGroup(Group group)
        {
             await _sseService.BroadcastAsync(group);
            return _service.CreateGroup(group);

        }

        [HttpPost("signinapi")]
        public PostResponse PostSomeDetails(User user)
        {
            return _service.PostSomeDetails(user);
        }

        [HttpPost("loginapi")]
        public LoginResponse Login(User user)
        {
            return _service.Login(user);
        }

        [HttpPost("postlearn")]
        public Task<PostResponse> PostLearnData(LearnPost learn)
        {
            return _service.PostLearnData(learn);
        }

        [HttpGet("DeleteGroup")]
        public PostResponse DeleteGroup(int Id)
        {
            return _service.DeleteGroup(Id);
        }

        [HttpGet("GetLearnings")]
        public List<LearnPost> GetLearnData(int groupid)
        {
            return _service.GetLearnData(groupid);
        }

        [HttpPost("deleteLearing")]
        public async Task<IActionResult> DeletedLearningById(int id)
        {
            // Get group_id before deletion (since the message will be gone after deletion)
            int groupId = _service.GetGroupIdByLearnId(id); // You may need to add this helper method

            var result = _service.DeletedLearningById(id); // Do the actual delete
            await WebSocketBroadcaster.BroadcastUpdatedMessages(groupId, HttpContext.RequestServices); // THEN broadcast

            return Ok(result);
        }

        [HttpPost("EditLearning")]
        public async Task<IActionResult> EditLearn([FromBody] LearnPost learnPost)
        {
            var result = _service.EditLearn(learnPost); // Do the edit
            await WebSocketBroadcaster.BroadcastUpdatedMessages(learnPost.group_id, HttpContext.RequestServices); // THEN broadcast

            return Ok(result);
        }

        [HttpGet("GetRequests")]
        public List<RequestList> GetRequestLists(int userId)
        {
            return _service.GetRequestLists(userId);
        }

        [HttpGet("checkaccess")]
        public PostResponse CheckGroup(int user_id, int group_id)
        {
            return _service.CheckGroup(user_id, group_id);
        }

        [HttpPost("requestaccess")]
        public PostResponse RequesAccess(RequestAcessPayload payload)
        {
            return _service.RequesAccess(payload);
        }

        [HttpPost("grandaccess")]
        public PostResponse GrandAccess(RequestList access)
        {
            return _service.GrandAccess(access);
        }
        [HttpPost("grandaccessbyemailId")]
        public PostResponse GrandAccessByMailId(AccessByMailId accessByMailId)
        {
            return _service.GrandAccessByMailId(accessByMailId);
        }

        [HttpPost("getuserbyId")]
        public List<UsernameById> GetUsers(int[] userIDs)
        {
            return _service.GetUsers(userIDs);
        }
        [HttpPost("getgroupbyId")]
        public List<GroupById> getGroups(int[] groupIds)
        {
            return _service.getGroups(groupIds);
        }

        [HttpGet("getusersbygroupId")]

        public UserPayload getUsersByGroupId(int groupId)
        {
            return _service.getUsersByGroupId(groupId);
        }

        [HttpPost("PutXO")]
        public PostResponse AddXo(AddXO addXO)
        {
            return _service.AddXo(addXO);
        }
        [HttpGet("GetXO")]
        public GetXOResponse xogame_Tables()
        {
            return _service.xogame_Tables();
        }
        [HttpGet("clearXo")]
        public PostResponse ClearXo()
        {
            return _service.ClearXo();
        }
        [HttpPost("PostFortess")]
        public PostResponse PostFortess(Fortess[] postFortess)
        {
            return _service.PostFortess(postFortess);
        }

    }
}
