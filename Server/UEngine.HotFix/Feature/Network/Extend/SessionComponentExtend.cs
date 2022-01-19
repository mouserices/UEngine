public static class SessionComponentExtend
{
    public static void SetSessionRoomID(this SessionComponent sessionComponent, int connectionID, int roomID)
    {
        if (sessionComponent.ConnectedIDToSessions.TryGetValue(connectionID, out var session))
        {
            session.RoomID = roomID;
        }
    }
    
    public static void SetSessionPlayerID(this SessionComponent sessionComponent, int connectionID, int playerID)
    {
        if (sessionComponent.ConnectedIDToSessions.TryGetValue(connectionID, out var session))
        {
            session.PlayerID = playerID;
        }
    }

    public static long GetPlayerIDByConnectionID(this SessionComponent sessionComponent, int connectionID)
    {
        if (sessionComponent.ConnectedIDToSessions.TryGetValue(connectionID, out var session))
        {
            return session.PlayerID;
        }

        return -1;
    }

    public static Session GetSessionByConnectionID(this SessionComponent sessionComponent, int connectionID)
    {
        sessionComponent.ConnectedIDToSessions.TryGetValue(connectionID, out var session);
        return session;
    }
}