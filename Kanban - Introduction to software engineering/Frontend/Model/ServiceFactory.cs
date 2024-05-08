using IntroSE.Kanban.Backend.Buissnes_Layer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model;

public class ServiceFactory
{
    internal UserService userService;
    internal BoardService boardService;
    private UserController userController;
    private static ServiceFactory instance;

    /// <summary>
    /// Service factory, singleton pattern
    /// </summary>
    private ServiceFactory() 
    {
        this.userController = new UserController();
        userService = new UserService(userController);
        boardService = new BoardService(userController);
        userService.LoadData(); // Dangerous!!
        boardService.LoadData();
        instance = this;
    }

    public static ServiceFactory getServiceFactrory()
    {
        if (instance == null)
        {
            return new ServiceFactory();
        }
        else
        {
            return instance;
        }
    }
}