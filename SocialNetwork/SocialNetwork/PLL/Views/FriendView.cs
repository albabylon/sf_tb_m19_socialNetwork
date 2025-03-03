using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;

namespace SocialNetwork.PLL.Views
{
    public class FriendView
    {
        private readonly IFriendService friendService;

        public FriendView(IFriendService friendService)
        {
            this.friendService = friendService;
        }

        public void Show(User user)
        {
            while (true)
            {
                Console.WriteLine("Добавить в друзья (нажмите 1)");
                Console.WriteLine("Убрать из друзей (нажмите 2)");
                Console.WriteLine("Показать список друзей (нажмите 3)");
                Console.WriteLine("Выйти из меню друзей (нажмите 4)");

                string keyValue = Console.ReadLine();

                if (keyValue == "4") break;

                switch (keyValue)
                {
                    case "1":
                        {
                            Add(user);
                            break;
                        }
                    case "2":
                        {
                            Remove(user);
                            break;
                        }
                    case "3":
                        {
                            ShowAll(user);
                            break;
                        }
                }
            }
        }

        private void Add(User user)
        {
            var friendData = new FriendData();

            Console.Write("Введите почтовый адрес с кем хотите подружиться: ");
            friendData.FriendEmail = Console.ReadLine();
            friendData.UserId = user.Id;

            try
            {
                friendService.AddFriend(friendData);

                SuccessMessage.Show($"Пользователь {friendData.FriendEmail} успешно добавлен в друзья!");
            }

            catch (UserNotFoundException)
            {
                AlertMessage.Show("Пользователь не найден!");
            }

            catch (FriendAlreadyAddException)
            {
                AlertMessage.Show("Друг уже был добавлен ранее!");
            }

            catch (ArgumentNullException)
            {
                AlertMessage.Show("Введите корректное значение!");
            }

            catch (Exception)
            {
                AlertMessage.Show("Произошла ошибка при добавлении в друзья!");
            }
        }

        private void Remove(User user)
        {
            var friendData = new FriendData();

            Console.Write("Введите почтовый адрес кого необоходимо удалить из друзей: ");
            friendData.FriendEmail = Console.ReadLine();
            friendData.UserId = user.Id;

            try
            {
                friendService.RemoveFriend(friendData);

                SuccessMessage.Show($"Пользователь {friendData.FriendEmail} удален из друзей!");
            }

            catch (UserNotFoundException)
            {
                AlertMessage.Show("Пользователь не найден!");
            }

            catch (FriendAlreadyRemoveException)
            {
                AlertMessage.Show("Друг не был добавлен или был удален ранее!");
            }

            catch (ArgumentNullException)
            {
                AlertMessage.Show("Введите корректное значение!");
            }

            catch (Exception)
            {
                AlertMessage.Show("Произошла ошибка при удалении из друзей!");
            }
        }

        private void ShowAll(User user) 
        {
            Console.WriteLine("Список ваших друзей:");
            Console.WriteLine(string.Join("\n", friendService.GetFriendsByUserId(user.Id).Select(x => x.FriendEmail).ToList()));
            Console.WriteLine();
        }
    }
}
