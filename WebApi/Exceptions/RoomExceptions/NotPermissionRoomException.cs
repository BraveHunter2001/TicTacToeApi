namespace WebApi.Exceptions.RoomExceptions
{
    public class NotPermissionRoomException : Exception
    {
        public NotPermissionRoomException(string message): base(message) { }
    }
}
