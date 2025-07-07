using Microsoft.AspNetCore.Mvc;
using project2025.DBContexts;
using project2025.Middleware;
using project2025.Models;
using project2025.Models.Responces;
using project2025.Service;
using System.Linq.Expressions;
using System.Transactions;

namespace project2025.Repository.Repositories
{
    public class userrepo : Irepository
    {
        public readonly UserDbContext Context;
        public userrepo(UserDbContext context)
        {
            Context = context;
        }
        public List<User> getUserDetails()
        {
            var response = Context.Users.ToList();
            return response;
        }
        public PostResponse PostSomeDetails(User user)
        {
            PostResponse postresponse = new PostResponse();

            postresponse.message = "You did it manhh";
            postresponse.statuscode = 200;
            Context.Users.AddAsync(user);
            Context.SaveChanges();
            return postresponse;

        }
        public LoginResponse Login(User user)
        {
            var SelectedUSer = Context.Users.Where(a => a.email_id == user.email_id).FirstOrDefault();
            LoginResponse postResponse = new LoginResponse();
            if (SelectedUSer != null)
            {
                if (SelectedUSer.email_id == user?.email_id && SelectedUSer.password == user.password)
                {
                    postResponse.statuscode = 200;
                    postResponse.message = "User Logged In";
                    postResponse.userId = SelectedUSer.id;
                }
                else
                {
                    postResponse.statuscode = 420;
                    postResponse.message = "User Cant Login";
                }
            }
            return postResponse;
        }
        public async Task<PostResponse> PostLearnData(LearnPost learn)
        {
            PostResponse postresponse = new PostResponse();
          
            await Context.Learn.AddAsync(learn);
            await Context.SaveChangesAsync();
            postresponse.message = "You did it manhh";
            postresponse.statuscode = 200;
            return postresponse;
        }
        public List<LearnPost> GetLearnData(int groupid)
        {
            var response = Context.Learn.Where(a => a.is_deleted == 0 && a.group_id == groupid).ToList();
            return response;
        }
        public List<Group> getAllGroups()
        {
            PostResponse postResponse = new PostResponse();
            var response = Context.Groups.Where(a => a.is_deleted == 0).ToList();

            return response;
        }
        public PostResponse CreateGroup(Group group)
        {
            PostResponse postResponse = new PostResponse();
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Context.Groups.AddAsync(group);
                    Context.SaveChanges();
                    transaction.Commit();
                    postResponse.message = "Done";
                    postResponse.statuscode = 200;
                    var User = Context.Users.Where(a => a.id == group.created_by).FirstOrDefault();
                    var GroupId=Context.Groups.Where(a=>a.group_name==group.group_name&& a.created_by==group.created_by).FirstOrDefault();
                    if (User.email_id != null)
                    {
                        var accessByMailId = new AccessByMailId();
                        accessByMailId.group_id = GroupId.group_ids;
                        accessByMailId.EmailId = User.email_id;
                        GrandAccessByMailId(accessByMailId);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    postResponse.message = ex.Message;
                    postResponse.statuscode = 500;
                }
            }
            return postResponse;
        }
        public PostResponse DeletedLearningById(int id)
        {
            PostResponse postresponse = new PostResponse();
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var learningItem = Context.Learn.Where(a => a.learn_id == id).FirstOrDefault();
                    if (learningItem != null)
                    {
                        learningItem.is_deleted = 1;
                        Context.SaveChanges();
                        transaction.Commit();
                        postresponse.message = "You did it manhh";
                        postresponse.statuscode = 200;
                    }
                    else
                    {
                        transaction.Rollback();
                        postresponse.message = "No Such Id Found";
                        postresponse.statuscode = 500;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    postresponse.message = $"Error: {ex.Message}";
                    postresponse.statuscode = 500;
                }
            }
            return postresponse;
        }
        public PostResponse EditLearn(LearnPost learnPost)
        {
            PostResponse response = new PostResponse();
            var transaction = new TransactionScope();
            try
            {


                var LearnItem = Context.Learn.Where(a => a.learn_id == learnPost.learn_id).FirstOrDefault();
                if (LearnItem != null)
                {
                    LearnItem.message = learnPost.message;
                    LearnItem.name = learnPost.name;
                    Context.SaveChanges();
                    transaction.Complete();
                    response.statuscode = 200;
                    response.message = "updated";
                    return response;

                }
                else
                {
                    response.statuscode = 500;
                    response.message = "No Such Item";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                response.statuscode = 600;
                return response;
            }
            finally
            {
                transaction.Dispose();
            }


        }
        public PostResponse DeleteGroup(int Id)
        {
            var response = new PostResponse();
            var GroupSelected = Context.Groups.FirstOrDefault(a => a.group_ids == Id);


            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    if (GroupSelected != null)
                    {
                        GroupSelected.is_deleted = 1;
                        Context.SaveChanges();
                        transaction.Commit();
                        response.statuscode = 200;
                        response.message = "success";
                    }
                    else
                    {
                        response.message = "no such grouyp";
                        response.statuscode = 768;
                    }

                }
                catch (Exception ex)
                {
                    response.statuscode = 500;
                    response.message = ex.Message;
                }
                finally
                {
                    transaction.Dispose();
                }

            }
            return response;
        }
        public PostResponse CheckGroup(int user_id, int group_id)
        {
            var response = new PostResponse();
            var UserAvail = Context.Requests.FirstOrDefault(a => a.user_id == user_id && a.group_id == group_id);

            if (UserAvail != null)
            {
                if (UserAvail.approval_status == 1)
                {
                    response.statuscode = 200;
                    response.message = "user has access";
                }
                else
                {
                    response.statuscode = 500;
                    response.message = "no access";
                }
            }
            else
            {
                response.statuscode = 300;
                response.message = "user dont have access";
            }
            return response;
        }
        public PostResponse RequesAccess(RequestAcessPayload payload)
        {
            var response = new PostResponse();
            using (var transaction = new TransactionScope())
            {
                var Element = Context.Requests.Where(a => a.group_id == payload.group_id && a.user_id == payload.user_id).FirstOrDefault();

                if (Element==null)
                {
                    var requestList = new RequestList();
                    try
                    {
                        requestList.group_id = payload.group_id;
                        requestList.user_id = payload.user_id;
                        requestList.is_deleted = 0;
                        Context.Requests.Add(requestList);
                        Context.SaveChanges();
                        transaction.Complete();

                        response.statuscode = 200;
                        response.message = "Success";
                    }
                    catch (Exception ex)
                    {
                        response.statuscode = 500;
                        response.message = ex.Message;
                    }
                }
                else
                {
                    response.statuscode = 900;
                    response.message = "Request Pending";
                }
            }
            return response;
        }
        public List<RequestList> GetRequestLists(int userId)
        {
            var groupIds = Context.Groups
                                  .Where(g => g.created_by == userId)
                                  .Select(g => g.group_ids)
                                  .ToList();

            var response = Context.Requests
                      .Where(r => groupIds.Contains(r.group_id) && r.approval_status!=2 && r.approval_status!=1)
                      .GroupBy(r => new { r.user_id, r.group_id })  // Group by both user_id and group_id
                      .Select(g => g.First())  // Select the first entry from each group to ensure distinctness
                      .ToList();


            return response;
        }
        public PostResponse GrandAccess(RequestList access)
        {
            var response = new PostResponse();
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var Acesss = Context.Requests.FirstOrDefault(a => a.group_id == access.group_id && a.user_id == access.user_id);
                    if (Acesss != null)
                    {
                        Acesss.approval_status = access.is_deleted;
                        Context.SaveChanges();
                        transaction.Commit();
                        response.statuscode = 200;
                        response.message = "Sucessfully updated";
                    }
                    else
                    {
                        var GrandAccessTo = Context.Requests.Add(access);
                        Context.SaveChanges();
                        transaction.Commit();
                        response.statuscode = 200;
                        response.message = "success";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.statuscode = 500;
                    response.message = "Unsuccessfull";
                }

                finally { transaction.Dispose(); }
            }

            return response;
        }
        public PostResponse GrandAccessByMailId(AccessByMailId accessByMailId)
        {
            var response = new PostResponse();

            var User =Context.Users.Where(a=>a.email_id == accessByMailId.EmailId).FirstOrDefault();
            if (User != null)
            {
                RequestAcessPayload requestAcessPayload= new RequestAcessPayload();
                RequestList requestList = new RequestList();
                requestList.user_id = User.id;
                requestAcessPayload.user_id = User.id;
                requestAcessPayload.group_id = accessByMailId.group_id;
                requestList.group_id = accessByMailId.group_id;
                requestList.is_deleted = 1;
                RequesAccess(requestAcessPayload);
                var responses = GrandAccess(requestList);
                if (responses?.statuscode == 200)
                {
                    response.statuscode = responses.statuscode;
                    response.message = responses.message;
                }
                else
                {
                    response.statuscode = 570;
                    response.message = "Cant be done";
                }
            }
            return response;
        }
        public List<UsernameById> GetUsers(int[] userIDs)
        {
            var result = new List<UsernameById>();

            foreach (var userID in userIDs)
            {
                var user = Context.Users
                                  .Where(a => a.id == userID)
                                  .FirstOrDefault();

                if (user != null)
                {
                    result.Add(new UsernameById
                    {
                        User_Id = user.id,
                        Username = user.name
                    });
                }
            }

            return result;
        }
        public List<GroupById> getGroups(int[] groupIds)
        {
            var result = new List<GroupById>();

            foreach (var userID in groupIds)
            {
                var user = Context.Groups
                                  .Where(a => a.group_ids == userID)
                                  .FirstOrDefault();

                if (user != null)
                {
                    result.Add(new GroupById
                    {
                        Group_id = user.group_ids,
                        Group_name = user.group_name
                    });
                }
            }

            return result;
        }
        public int GetGroupIdByLearnId(int learnId)
        {
            var result = Context.Learn.Where(a => a.learn_id == learnId).FirstOrDefault();
            if (result != null)
            {
                return result.group_id;

            }
            else
            {
                return 0;
            }
        }
        public UserPayload getUsersByGroupId(int groupId)
        {
            var userIds = Context.Requests
                                 .Where(a => a.group_id == groupId && a.approval_status == 1)
                                 .Select(a => a.user_id)
                                 .ToList();

            var userDtos = Context.Users
                     .Where(u => userIds.Contains(u.id))
                     .Select(u => new UserDto
                     {
                         id = u.id,
                         name = u.name,
                         email_id = u.email_id
                     })
                     .ToList();

            var userPayload = new UserPayload
            {
                Users = userDtos
            };

            return userPayload;
        }
        public PostResponse AddXo(AddXO addXO)
        {
            var PostResponse = new PostResponse();
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var exist = Context.Xogames.FirstOrDefault(a => a.box_no == addXO.box_no);
                    if (exist == null)
                    {
                        var usersAlpha = Context.Xogames.Where(a => a.userid == addXO.userid).FirstOrDefault();
                        if (usersAlpha != null)
                        {
                            if (usersAlpha.box_value != addXO.box_value)
                            {
                                PostResponse.statuscode = 999;
                                PostResponse.message = "wait for your turn";
                                return PostResponse;
                            }
                        }

                        var xogame_table = new xogame_table
                        {
                            box_no = addXO.box_no,
                            box_value = addXO.box_value,
                            userid = addXO.userid

                        };

                        Context.Xogames.Add(xogame_table);
                        Context.SaveChanges();

                        // Get current board
                        var board = Context.Xogames.ToDictionary(x => x.box_no, x => x.box_value);

                        if (IsWinner(board, addXO.box_value))
                        {
                            PostResponse.statuscode = 209;
                            PostResponse.message = $"{addXO.box_value} Congragulations you won!";
                            // Context.Xogames.RemoveRange(Context.Xogames);
                            // Context.SaveChanges();
                        }
                        else if (board.Count == 9)
                        {
                            PostResponse.statuscode = 201;
                            PostResponse.message = "It's a draw!";
                            //  Context.Xogames.RemoveRange(Context.Xogames);
                            //  Context.SaveChanges();
                        }
                        else
                        {
                            PostResponse.statuscode = 200;
                            PostResponse.message = "Success";
                        }

                        transaction.Commit();
                    }
                    else
                    {
                        PostResponse.statuscode = 300;
                        PostResponse.message = "Select another column";
                    }
                }
                catch (Exception e)
                {
                    PostResponse.statuscode = 500;
                    PostResponse.message = e.Message;
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                }
            }
            return PostResponse;
        }
        public PostResponse ClearXo()
        {
            var PostResponse = new PostResponse();
            using(var transaction = Context.Database.BeginTransaction())
            try
            {
                 Context.Xogames.RemoveRange(Context.Xogames);
                    Context.SaveChanges();
                    transaction.Commit();
                    PostResponse.statuscode = 200;
                    PostResponse.message = "done";
            }
            catch (Exception e)
            {
                PostResponse.statuscode = 500;
                PostResponse.message = e.Message;
            }
            finally
            {
                    transaction?.Dispose();
            }
            return PostResponse;
        }
        private bool IsWinner(Dictionary<string, string> board, string player)
        {
            string[][] winningCombos = new string[][]
            {
        new[] { "00", "01", "02" },
        new[] { "10", "11", "12" },
        new[] { "20", "21", "22" },
        new[] { "00", "10", "20" },
        new[] { "01", "11", "21" },
        new[] { "02", "12", "22" },
        new[] { "00", "11", "22" },
        new[] { "02", "11", "20" }
            };

            foreach (var combo in winningCombos)
            {
                if (combo.All(pos => board.ContainsKey(pos) && board[pos] == player))
                {
                    return true;
                }
            }

            return false;
        }
        public GetXOResponse xogame_Tables()
        {
            var response = new GetXOResponse();
            var xogames = Context.Xogames.ToList();
            response.data = xogames;

            var board = xogames.ToDictionary(x => x.box_no, x => x.box_value);

            if (IsWinner(board, "X"))
            {
                response.statuscode = 101;
                response.message = "X won";
            }
            else if (IsWinner(board, "O"))
            {
                response.statuscode = 201;
                response.message = "O won";
            }
            else if (board.Count == 9)
            {
                response.statuscode = 301;
                response.message = "Game Drawn";
            }
            else
            {
                response.statuscode = 401;
            }

            return response;
        }
        public PostResponse PostFortess(Fortess[] postFortess)
        {
            var PostResponse = new PostResponse();
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var post in postFortess)
                    {

                        Context.fortesses.Add(post);
                        PostResponse.message = "Done";
                        PostResponse.statuscode = 200;
                    }
                    Context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    PostResponse.statuscode = 500;
                    PostResponse.message = "Couldn't Upload";
                }

            }
            return PostResponse;
        }

    }
}
