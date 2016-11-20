using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Exceptions;

namespace Chat.Auth
{
    public class SessionManager
    {
        List<Session> sessionList;

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

        public void addSession(Session other)
        {

            foreach (Session session in SessionList)
            {
                if (session.Token == other.Token)
                {
                    throw new SessionAlreadyExistsException(session.Token.ToString());
                }
            }

            SessionList.Add(other);

        }

        public void removeSession(Guid token)
        {
            Session sessionToDelete = null;

            foreach (Session session in SessionList)
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