using SocialNetwork.BLL.Services;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.PLL.Views;

namespace SocialNetwork;

class Program
{
    static IUserRepository userRepository;
    static IMessageRepository messageRepository;
    static IFriendRepository friendRepository;
    static IMessageService messageService;
    static IFriendService friendService;
    static UserService userService;
    public static MainView mainView;
    public static RegistrationView registrationView;
    public static AuthenticationView authenticationView;
    public static UserMenuView userMenuView;
    public static UserInfoView userInfoView;
    public static UserDataUpdateView userDataUpdateView;
    public static MessageSendingView messageSendingView;
    public static UserIncomingMessageView userIncomingMessageView;
    public static UserOutcomingMessageView userOutcomingMessageView;
    public static FriendView friendView;

    static void Main(string[] args)
    {
        userRepository = new UserRepository();
        messageRepository = new MessageRepository();
        friendRepository = new FriendRepository();

        messageService = new MessageService(userRepository, messageRepository);
        friendService = new FriendService(userRepository, friendRepository);
        userService = new UserService(userRepository, messageService, friendService);

        mainView = new MainView();
        registrationView = new RegistrationView(userService);
        authenticationView = new AuthenticationView(userService);
        userMenuView = new UserMenuView(userService);
        userInfoView = new UserInfoView();
        userDataUpdateView = new UserDataUpdateView(userService);
        messageSendingView = new MessageSendingView(messageService, userService);
        userIncomingMessageView = new UserIncomingMessageView();
        userOutcomingMessageView = new UserOutcomingMessageView();
        friendView = new FriendView(friendService);

        while (true)
        {
            mainView.Show();
        }
    }
}
