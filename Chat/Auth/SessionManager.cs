using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Exceptions;

namespace Chat.Auth
{
    /// <summary>
    /// Handle sessions with a manager
    /// </summary>
    public class SessionManager
    {
        List<Session> sessionList;

        /// <summary>
        /// Store all sessions in a list
        /// </summary>
        public List<Session> SessionList
        {
            get
            {
                return sessionList;
            }

            set
            {
                sessionList = value;
            }
        }

        public SessionManager()
        {
            SessionList = new List<Session>();
        }

        /// <summary>
        /// Add a session to the manager. Make sure it is not already stored with the GUID
        /// </summary>
        /// <param name="other">Other session</param>
        public void addSession(Session other)
        {
            foreach (Session session in SessionList.ToList())
            {
                if (session.Token == other.Token)
                {
                    throw new SessionAlreadyExistsException(session.Token.ToString());
                }
            }

            SessionList.Add(other);

        }

        /// <summary>
        /// Delete a session using its token
        /// </summary>
        /// <param name="token"></param>
        public void removeSession(Guid token)
        {
            Session sessionToDelete = null;

            foreach (Session session in SessionList.ToList())
            {
                if (session.Token == token)
                {
                    sessionToDelete = session;
                }
            }

            if (sessionToDelete == null)
            {
                throw new SessionUnknownException(token.ToString());
            }

            SessionList.Remove(sessionToDelete);
        }
    }
}