using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;

namespace SocialNetwork.BLL.Services
{
    public class FriendService
    {
        private readonly IUserRepository userRepository;
        private readonly IFriendRepository friendRepository;

        public FriendService()
        {
            userRepository = new UserRepository();
            friendRepository = new FriendRepository();
        }

        public IEnumerable<Friend> GetFriendsByUserId(int userId)
        {
            var friends = new List<Friend>();

            friendRepository.FindAllByUserId(userId).ToList().ForEach(f => 
            {
                var userEntity = userRepository.FindById(f.user_id);
                var friendUserEntity = userRepository.FindById(f.friend_id);

                friends.Add(new Friend(f.id, userEntity.email, friendUserEntity.email));
            });

            return friends;
        }

        public void AddFriend(FriendData friendData)
        {
            var userFriend = userRepository.FindByEmail(friendData.FriendEmail) ?? throw new UserNotFoundException();

            if (GetFriendsByUserId(friendData.UserId).Any(f => f.FriendEmail == friendData.FriendEmail))
                throw new FriendAlreadyAddException();

            var friendEntity = new FriendEntity()
            {
                user_id = friendData.UserId,
                friend_id = userFriend.id,
            };

            if (friendRepository.Create(friendEntity) == 0)
                throw new Exception();
        }

        public void RemoveFriend(FriendData friendData) 
        {
            var userFriend = userRepository.FindByEmail(friendData.FriendEmail) ?? throw new UserNotFoundException();

            if (!GetFriendsByUserId(friendData.UserId).Any(f => f.FriendEmail == friendData.FriendEmail))
                throw new FriendAlreadyRemoveException();

            if (friendRepository.Delete(userFriend.id) == 0)
                throw new Exception();
        }
    }
}
