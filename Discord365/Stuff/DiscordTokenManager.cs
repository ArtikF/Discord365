using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord365
{
    class DiscordTokenManager
    {
        private const string TargetToken = "DiscordToken";

        public static string SavedToken
        {
            get
            {
                using (var cred = new Credential())
                {
                    cred.Target = TargetToken;
                    cred.Load();
                    return cred.Password;
                }
            }
            set
            {
                using (var cred = new Credential())
                {
                    cred.Password = value;
                    cred.Target = TargetToken;
                    cred.Type = CredentialType.Generic;
                    cred.PersistanceType = PersistanceType.LocalComputer;
                    cred.Save();
                }
            }
        }
    }
}
