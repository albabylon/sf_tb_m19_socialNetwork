using Moq;
using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;

namespace SocialNetwork.Tests
{
    public class FriendServiceTest
    {
        [Fact]
        public void GetFriendsByUserIdMustReturnCollection()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.FindById(1)).Returns(new UserEntity { id = 1, firstname = "masha" });
            mockUserRepository.Setup(x => x.FindById(2)).Returns(new UserEntity { id = 2, firstname = "sasha" });
            mockUserRepository.Setup(x => x.FindById(3)).Returns(new UserEntity { id = 3, firstname = "pasha" });
            mockUserRepository.Setup(x => x.FindById(4)).Returns(new UserEntity { id = 4, firstname = "dasha" });

            var mockFriendRepository = new Mock<IFriendRepository>();
            mockFriendRepository.Setup(x => x.FindAllByUserId(1)).Returns(
            [
                new() { user_id = 1, friend_id = 2 },
                new() { user_id = 1, friend_id = 3 },
                new() { user_id = 1, friend_id = 4 },
            ]);

            // Act
            var friendServiceTest = new FriendService(mockUserRepository.Object, mockFriendRepository.Object);

            // Assert
            Assert.True(friendServiceTest.GetFriendsByUserId(1).Count() == 3);
        }

        [Fact]
        public void AddFriendMustThrowFriendAlreadyAddException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockFriendRepository = new Mock<IFriendRepository>();

            var friendData = new FriendData()
            {
                UserId = 1,
                FriendEmail = "sasha@mail.ru"
            };

            var user = new UserEntity
            {
                id = 1,
                email = "masha@mail.ru"
            };

            var userFriend = new UserEntity
            {
                id = 2,
                email = "sasha@mail.ru"
            };

            var friendEntity = new FriendEntity()
            {
                user_id = friendData.UserId,
                friend_id = userFriend.id,
            };

            // Act
            mockUserRepository.Setup(x => x.FindByEmail(friendData.FriendEmail)).Returns(userFriend);
            mockUserRepository.Setup(x => x.FindById(friendEntity.user_id)).Returns(user);
            mockUserRepository.Setup(x => x.FindById(friendEntity.friend_id)).Returns(userFriend);
            mockFriendRepository.Setup(x => x.FindAllByUserId(friendData.UserId)).Returns(new List<FriendEntity> { friendEntity });

            var friendServiceTest = new FriendService(mockUserRepository.Object, mockFriendRepository.Object);
            friendServiceTest.GetFriendsByUserId(friendData.UserId);

            // Assert
            Assert.Throws<FriendAlreadyAddException>(() => friendServiceTest.AddFriend(friendData));
        }

        [Fact]
        public void RemoveFriendMustThrowFriendAlreadyRemoveException()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockFriendRepository = new Mock<IFriendRepository>();

            var friendData = new FriendData()
            {
                UserId = 1,
                FriendEmail = "petrov@mail.ru"
            };

            var user = new UserEntity
            {
                id = 1,
                email = "masha@mail.ru"
            };

            var userFriend = new UserEntity
            {
                id = 2,
                email = "sasha@mail.ru"
            };

            var friendEntity = new FriendEntity()
            {
                user_id = friendData.UserId,
                friend_id = userFriend.id,
            };

            // Act
            mockUserRepository.Setup(x => x.FindByEmail(friendData.FriendEmail)).Returns(userFriend);
            mockUserRepository.Setup(x => x.FindById(friendEntity.user_id)).Returns(user);
            mockUserRepository.Setup(x => x.FindById(friendEntity.friend_id)).Returns(userFriend);
            mockFriendRepository.Setup(x => x.FindAllByUserId(friendData.UserId)).Returns(new List<FriendEntity> { friendEntity });

            var friendServiceTest = new FriendService(mockUserRepository.Object, mockFriendRepository.Object);
            friendServiceTest.GetFriendsByUserId(friendData.UserId);

            // Assert
            Assert.Throws<FriendAlreadyRemoveException>(() => friendServiceTest.RemoveFriend(friendData));
        }
    }
}