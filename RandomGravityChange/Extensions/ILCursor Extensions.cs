namespace RandomGravityChange.Extensions;

public static class ILCursorExtensions
{
    public static ILCursor GoToNext(this ILCursor cursor, int count)
    {
        for (int i = 0; i < count; i++)
        {
            cursor.GotoNext();
        }

        return cursor;
    }
    public static ILCursor GoToPrev(this ILCursor cursor, int count)
    {
        for (int i = 0; i < count; i++)
        {
            cursor.GotoPrev();
        }

        return cursor;
    }

    public static ILCursor CreateCursor(this ILContext il, int startIndex = 0)
    {
        return new ILCursor(il).Goto(startIndex);
    }

    public static ILCursor GoToStart(this ILCursor cursor)
    {
        return cursor.Goto(0);
    }
}