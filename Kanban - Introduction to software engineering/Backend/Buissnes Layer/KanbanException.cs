using System;

namespace IntroSE.Kanban.Backend.Buissnes_Layer;

public class KanbanException: Exception
{

    public KanbanException()
    {
    }

    public KanbanException(string message) : base(message)
    {
    }

    public KanbanException(string message, Exception inner) : base(message, inner)
    {
    }

}