namespace Events.Core.Models.General
{
    public enum Constants
    {
        UserId,
        RoleId,
    }


    public class NOTIFICATION
    {
        public static readonly long OPEN_NOTIFICATION = 10;
        public static readonly long CLOSED_NOTIFICATION = 12;
        public static readonly long IGNORED_NOTIFICATION = 11;
        public static readonly long CLOSED_INCIDENT = 9;
        public static readonly long INCIDENT = 8;
        public static readonly long EDIT_INCIDENT = 16;
        public static readonly long ADD_COMMENT = 6;
        public static readonly long ASSIGN_USER = 7;
        public static readonly long REQUEST_RESPONSE = 13;

    }
    public class TASK
    {
        public static readonly long OPEN = 5;
        public static readonly long IN_PROGRESS = 3;
        public static readonly long CLOSED = 4;
        public static readonly long ADD_COMMENT = 2;

    }



}
