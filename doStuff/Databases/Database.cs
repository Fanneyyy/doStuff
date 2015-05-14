using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doStuff.Models.DatabaseModels;

namespace doStuff.Databases
{
    public class Database
    {
        protected IDataContext db;

        public Database(IDataContext dbContext)
        {
            db = dbContext ?? new DatabaseContext();       
        }

        public bool Save()
        {
            db.SaveChanges();
            return true;
        }

        #region GetRecordLists
        public List<User> GetFriends(int userId)
        {
            List<User> friends = new List<User>();

            List<UserToUserRelation> relations = (from f in db.UserToUserRelations
                                                  where (f.ReceiverId == userId || f.SenderId == userId) 
                                                  && f.Answer.HasValue 
                                                  && f.Answer.Value == true
                                                  && f.Active == true
                                                  select f).ToList();

            foreach(var relation in relations)
            {
                int id = (relation.SenderId == userId) ? relation.ReceiverId : relation.SenderId;
                User friend = GetUser(id);
                if(friend != null)
                {
                    friends.Add(friend);
                }
            }
            
            return friends;
        }

        public List<User> GetPendingRequests(int userId)
        {
            return (from r in db.UserToUserRelations
                    from u in db.Users
                    where r.SenderId == userId
                    && r.Active
                    && !r.Answer.HasValue
                    && u.UserID == r.ReceiverId
                    select u).ToList();
        }

        public List<User> GetFriendRequests(int userId)
        {
            return (from r in db.UserToUserRelations
                    from u in db.Users
                    where r.ReceiverId == userId
                    && r.Active
                    && !r.Answer.HasValue
                    && u.UserID == r.SenderId
                    select u).ToList();
        }
        public List<User> GetMembers(int groupId)
        {
            List<User> members = (from g in db.GroupToUserRelations
                                  from u in db.Users
                                  where g.GroupId == groupId 
                                  && g.Active == true
                                  && u.UserID == g.MemberId
                                  && u.Active == true
                                  select u).ToList();
            return members;
        }
        public List<User> GetAttendees(int eventId)
        {
            List<User> attendees = new List<User>();

            List<int> attendeeIds = (from r in db.EventToUserRelations
                                     where r.EventId == eventId && r.Active == true && r.Answer == true
                                     select r.AttendeeId).ToList();

            foreach(var id in attendeeIds)
            {
                attendees.Add(GetUser(id));
            }

            return attendees;
        }
        public List<Group> GetGroups(int userId)
        {
            List<Group> groups = (from r in db.GroupToUserRelations
                                  from g in db.Groups
                                  where r.MemberId == userId 
                                  && r.Active == true 
                                  && r.GroupId == g.GroupID
                                  && g.Active == true 
                                  orderby g.Name ascending
                                  select g).ToList();
            return groups;
        }
        public List<Event> GetEvents(int userId)
        {
            return (from e in db.Events
                    where e.OwnerId == userId 
                    && e.GroupId == null 
                    && e.Active == true
                    orderby e.CreationTime descending
                    select e).Take(10).ToList();
        }
        public List<Event> GetGroupEvents(int groupId)
        {
            return (from e in db.Events
                    where e.GroupId == groupId 
                    && e.Active == true
                    orderby e.CreationTime
                    select e).Take(10).ToList();
        }
        public List<Comment> GetComments(int eventId)
        {
            List<Comment> comments = (from r in db.EventToCommentRelations
                                      from c in db.Comments
                                      where r.EventId == eventId 
                                      && r.Active == true
                                      && c.CommentID == r.CommentId
                                      && c.Active == true
                                      select c).ToList();
            return comments;
        }
        #endregion
        #region RecordTables
            #region Exists
            public bool ExistsUser(int userId)
            {
                return (null != (from u in db.Users where u.UserID == userId && u.Active == true select u).SingleOrDefault());
            }
            public bool ExistsGroup(int groupId)
            {
                return (null != (from g in db.Groups where g.GroupID == groupId && g.Active == true select g).SingleOrDefault());
            }
            public bool ExistsEvent(int eventId)
            {
                return (null != (from e in db.Events where e.EventID == eventId && e.Active == true select e).SingleOrDefault());
            }
            public bool ExistsComment(int commentId)
            {
                return (null != (from c in db.Comments where c.CommentID == commentId && c.Active == true select c).SingleOrDefault());
            }
            #endregion
            #region Create
            public bool CreateUser(ref User user)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            public bool CreateGroup(ref Group group)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return true;
            }
            public bool CreateEvent(ref Event newEvent)
            {
                db.Events.Add(newEvent);
                db.SaveChanges();
                return true;
            }
            public bool CreateComment(ref Comment comment)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return true;
            }
            #endregion
            #region Remove
            public bool RemoveUser(int userId)
            {
                var user = (from u in db.Users
                            where u.UserID == userId
                            select u).SingleOrDefault();
                user.Active = false;
                db.SaveChanges();
                return true;
            }
            public bool RemoveGroup(int groupId)
            {
                var group = (from g in db.Groups
                             where g.GroupID == groupId
                             select g).SingleOrDefault();
                group.Active = false;
                db.SaveChanges();
                return true;
            }
            public bool RemoveEvent(int eventId)
            {
                var theEvent = (from e in db.Events
                                where e.EventID == eventId
                                select e).SingleOrDefault();
                theEvent.Active = false;
                db.SaveChanges();
                return true;
            }
            public bool RemoveComment(int commentId)
            {
                var comment = (from c in db.Comments
                               where c.CommentID == commentId
                               select c).SingleOrDefault();
                comment.Active = false;
                db.SaveChanges();
                return true;
            }
            #endregion
            #region Get
            public User GetUser(int userId)
            {
                return (from u in db.Users
                        where u.UserID == userId && u.Active == true
                        select u).SingleOrDefault();
            }

            public User GetUser(string userName)
            {
                return (from u in db.Users
                        where u.UserName.ToLower() == userName.ToLower() && u.Active == true
                        select u).SingleOrDefault();
            }

            public Group GetGroup(int groupId)
            {
                return (from g in db.Groups
                        where g.GroupID == groupId && g.Active == true
                        select g).SingleOrDefault();
            }

            public Event GetEvent(int eventId)
            {
                return (from e in db.Events
                        where e.EventID == eventId && e.Active == true
                        select e).SingleOrDefault();
            }

            public Comment GetComment(int commentId)
            {
                return (from c in db.Comments
                        where c.CommentID == commentId && c.Active == true
                        select c).SingleOrDefault();
            }
            #endregion
        #endregion
        #region RelationTable
            #region Exists
            public bool ExistsUserToUserRelation(int senderId, int receiverId)
            {
                var relation = (from r in db.UserToUserRelations
                                where r.SenderId == senderId && r.ReceiverId == receiverId && r.Active == true
                                select r).SingleOrDefault();
                return relation != null;
            }

            public bool ExistsGroupToUserRelation(int groupId, int userId)
            {
                var relation = (from r in db.GroupToUserRelations
                                where r.GroupId == groupId && r.MemberId == userId && r.Active == true
                                select r).SingleOrDefault();
                return relation != null;
            }

            public bool ExistsEventToUserRelation(int eventId, int userId)
            {
                var relation = (from r in db.EventToUserRelations
                                where r.EventId == eventId && r.AttendeeId == userId && r.Active == true
                                select r).SingleOrDefault();
                return relation != null;
            }

            public bool ExistsGroupToEventRelation(int groupId, int eventId)
            {
                var relation = (from r in db.GroupToEventRelations
                                where r.GroupId == groupId && r.EventId == eventId && r.Active == true
                                select r).SingleOrDefault();
                return relation != null;
            }

            public bool ExistsEventToCommentRelation(int eventId, int commentId)
            {
                var relation = (from r in db.EventToCommentRelations
                                where r.EventId == eventId && r.CommentId == commentId && r.Active == true
                                select r).SingleOrDefault();
                return relation != null;
            }
            #endregion
            #region Create
            public bool CreateUserToUserRelation(ref UserToUserRelation relation)
            {
                db.UserToUserRelations.Add(relation);
                db.SaveChanges();
                return true;
            }

            public bool CreateGroupToUserRelation(ref GroupToUserRelation relation)
            {
                db.GroupToUserRelations.Add(relation);
                db.SaveChanges();
                return true;
            }
            
            public bool CreateEventToUserRelation(ref EventToUserRelation relation)
            {
                db.EventToUserRelations.Add(relation);
                db.SaveChanges();
                return true;
            }

            public bool CreateGroupToEventRelation(ref GroupToEventRelation relation)
            {
                db.GroupToEventRelations.Add(relation);
                db.SaveChanges();
                return true;
            }

            public bool CreateEventToCommentRelation(ref EventToCommentRelation relation)
            {
                db.EventToCommentRelations.Add(relation);
                db.SaveChanges();
                return true;
            }
            #endregion
            #region Remove
            public bool RemoveUserToUserRelation(int id)
            {
                var relation = (from r in db.UserToUserRelations
                             where r.UserToUserRelationID == id
                             select r).SingleOrDefault();
                if (relation.Active != false)
                {
                    relation.Active = false;
                    db.SaveChanges();
                }
                return true;
            }

            public bool RemoveGroupToUserRelation(int id)
            {
                var relation = (from r in db.GroupToUserRelations
                             where r.GroupToUserRelationID == id
                             select r).SingleOrDefault();
                if (relation.Active != false)
                {
                    relation.Active = false;
                    db.SaveChanges();
                }
                return true;
            }

            public bool RemoveEventToUserRelation(int id)
            {
                var relation = (from r in db.EventToUserRelations
                             where r.EventToUserRelationID == id
                             select r).SingleOrDefault();
                if (relation.Active != false)
                {
                    relation.Active = false;
                    db.SaveChanges();
                }
                return true;
            }

            public bool RemoveGroupToEventRelation(int id)
            {
                var relation = (from r in db.GroupToEventRelations
                             where r.GroupToEventRelationID == id
                             select r).SingleOrDefault();
                if (relation.Active != false)
                {
                    relation.Active = false;
                    db.SaveChanges();
                }
                return true;
            }

            public bool RemoveEventToCommentRelation(int id)
            {
                var relation = (from r in db.EventToCommentRelations
                             where r.EventToCommentRelationID == id
                             select r).SingleOrDefault();
                if (relation.Active != false)
                {
                    relation.Active = false;
                    db.SaveChanges();
                }
                return true;
            }
            #endregion
            #region Get
            public UserToUserRelation GetUserToUserRelation(int senderId, int receiverId)
            {
                var  relation  = (from r in db.UserToUserRelations
                             where r.SenderId == senderId
                             && r.ReceiverId == receiverId
                             && r.Active == true
                             select r).SingleOrDefault();
                return  relation ;
            }
            public GroupToUserRelation GetGroupToUserRelation(int groupId, int userId)
            {
                var  relation  = (from r in db.GroupToUserRelations
                             where r.GroupId == groupId && r.MemberId == userId && r.Active == true
                             select r).SingleOrDefault();
                return  relation ;
            }

            public EventToUserRelation GetEventToUserRelation(int eventId, int userId)
            {
                var  relation  = (from r in db.EventToUserRelations
                             where r.EventId == eventId && r.AttendeeId == userId && r.Active == true
                             select r).SingleOrDefault();
                return  relation ;
            }

            public GroupToEventRelation GetGroupToEventRelation(int groupId, int eventId)
            {
                var  relation  = (from r in db.GroupToEventRelations
                             where r.GroupId == groupId && r.EventId == eventId && r.Active == true
                             select r).SingleOrDefault();
                return  relation ;
            }
            public EventToCommentRelation GetEventToCommentRelation(int eventId, int commentId)
            {
                var  relation  = (from r in db.EventToCommentRelations
                             where r.EventId == eventId && r.CommentId == commentId && r.Active == true
                             select r).SingleOrDefault();
                return  relation ;
            }
            #endregion
        #endregion
    }
}