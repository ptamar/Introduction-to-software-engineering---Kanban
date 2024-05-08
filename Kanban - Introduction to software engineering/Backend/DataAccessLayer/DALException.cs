using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer;

public class DALException: Exception
{
    public DALException()
    {
    }

    public DALException(string message) : base(message)
    {
    }

    public DALException(string message, Exception inner): base(message, inner)
    {
    }
}