using System.Collections.Generic;
using IntroSE.Kanban.Backend.Buissnes_Layer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model;

public class TasksModel
{
    private BoardService boardService;
    private ServiceFactory serviceFactory;
    private string _email;
    private string _boardName;
    public TasksModel(string email, string boardName)
    {
        this.serviceFactory = ServiceFactory.getServiceFactrory();
        this.boardService = serviceFactory.boardService;
        this._email = email;
        this._boardName = boardName;
    }
    /// <summary>
    /// This method get the Column from the right board .
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="boardName">The name of the board.</param>
    /// <param name="columnOrdinal">The asked column.</param>
    /// <returns>return a list with the Column from the right board . </returns>
    public List<string> GetColumn(string email, string boardName, int columnOrdinal)
    {
        return boardService.GetColumn(email, boardName, columnOrdinal);
    }
}