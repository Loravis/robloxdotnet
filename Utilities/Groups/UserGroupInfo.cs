namespace Robloxdotnet.Utilities.Groups
{
    public class Owner
    {
        public ulong buildersClubMembershipType { get; set; }
        public bool hasVerifiedBadge { get; set; }
        public ulong userId { get; set; }
        public string username { get; set; }
        public string displayName { get; set; }
    }

    public class Shout
    {
        public string body { get; set; }
        public Owner poster { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
    }

    public class Group
    {
        public ulong id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Owner owner { get; set; }
        public Shout shout { get; set; }
        public ulong memberCount { get; set; }
        public bool isBuildersClubOnly { get; set; }
        public bool publicEntryAllowed { get; set; }
        public bool isLocked { get; set; }
        public bool hasVerifiedBadge { get; set; }
    }

    public class Role
    {
        public ulong id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public ulong rank { get; set; }
        public ulong memberCount { get; set; }
    }

    public class Data
    {
        public Group group { get; set; }
        public Role role { get; set; }
        public bool isPrimaryGroup { get; set; }
    }

    public class UserGroupInfo
    {
        public List<Data> data { get; set; }
    }

}
