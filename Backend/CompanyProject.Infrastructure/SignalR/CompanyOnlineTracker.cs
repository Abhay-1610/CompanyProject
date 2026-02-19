using System.Collections.Concurrent;

namespace CompanyProject.Infrastructure.SignalR
{

    public static class CompanyOnlineTracker
    {

        private static ConcurrentDictionary<int, int> onlineUsers = new ConcurrentDictionary<int, int>();

        // Call this when a user connects
        public static int Increment(int companyId)
        {
            int count;

            if (onlineUsers.ContainsKey(companyId))
            {
                count = onlineUsers[companyId];
                count = count + 1;
                onlineUsers[companyId] = count;
            }
            else
            {
                count = 1;
                onlineUsers[companyId] = count;
            }

            return count;
        }

        // Call this when a user disconnects
        public static int Decrement(int companyId)
        {
            int count;

            count = onlineUsers[companyId];
            count = count - 1;

            if (count < 0)
            {
                count = 0;
            }

            onlineUsers[companyId] = count;

            return count;
        }
    }
}
