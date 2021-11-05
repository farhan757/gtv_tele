using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using TeleSharp.TL.Contacts;
using TeleSharp.TL;
using TLSharp.Core;

namespace gtv_tele.Functions
{
    public class Telegram
    {
        private string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private FileSessionStore store;
        private TelegramClient client;
        private string phoneNumber = "6281574965152";//string.Empty;
        private string userAttemptsFileFullPath = string.Empty;
        private string hash = string.Empty;
        private int userAttempts;
        private List<string> randomUsernames = new List<string>();

        public string getHash
        {
            get { return hash; }
        }

        public async void Sending(Dictionary<string, Models.SenderModel> data)
        {            
            try
            {
                TelegramClient client = this.client;
                int cntr = 0;
                foreach (Models.SenderModel dt in data.Values)
                {
                    cntr++;
                    var result = await client.GetContactsAsync();
                    var users = result.Users.Where(x => x.GetType() == typeof(TLUser))
                                .Cast<TLUser>()
                                .FirstOrDefault(x => x.Phone == dt.NoReceipt);

                    if (users == null)
                    {
                        var contacts = new TLVector<TLInputPhoneContact>();
                        contacts.Add(new TLInputPhoneContact
                        {
                            FirstName = dt.NoReceipt,
                            LastName = dt.NoReceipt,
                            Phone = dt.NoReceipt
                        }
                        );
                        var req = new TLRequestImportContacts() { Contacts = contacts };
                        var test = await client.SendRequestAsync<TLImportedContacts>(req);

                        result = await client.GetContactsAsync();
                        users = result.Users.Where(x => x.GetType() == typeof(TLUser))
                                .Cast<TLUser>()
                                .FirstOrDefault(x => x.Phone == dt.NoReceipt);
                    }

                    await client.SendMessageAsync(new TLInputPeerUser() { UserId = users.Id }, dt.Message);                    
                    Thread.Sleep(10000);
                }
                int num4 = (int)MessageBox.Show("Successfully sent messages to users from the data!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            catch (Exception ex)
            {
                int num3 = (int)MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            this.SaveToUserAttemptsFile();            

        }

        public async Task SendCode(string code)
        {
            TelegramClient client = this.client;
            try
            {
                await client.MakeAuthAsync(this.phoneNumber, this.hash, code);
                int num4 = (int)MessageBox.Show("Successfully sent Code, Ready to Sending Telegram Blast", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception ex)
            {
                int num3 = (int)MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
        }

        public async void ConnectAsyn()
        {
            this.client = new TelegramClient(1107146, "e98c58d479dac9ec13c4d602010cf899", (ISessionStore)this.store, this.phoneNumber.ToString(), null, DataCenterIPVersion.PreferIPv4);

            try
            {
               this.client.ConnectAsync().Wait(1000);                
            }
            catch (Exception ex)
            {
                int num2 = (int)MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            /*if (!await this.client.IsPhoneRegisteredAsync(this.phoneNumber))
            {
                int num3 = (int)MessageBox.Show("Enter a registered phone number. Your input: '" + this.phoneNumber + "'.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                if (this.client.IsUserAuthorized())
                {
                    int num4 = (int)MessageBox.Show("Successfully Register and Connect", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //return;
                }
                else
                {
                    try
                    {
                        string str = await this.client.SendCodeRequestAsync(this.phoneNumber);
                        this.hash = str;
                    }
                    catch (Exception ex)
                    {
                        string message = ex.Message;
                        int num2 = (int)MessageBox.Show("Invalid phone number. Enter a phone number registered with Telegram.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        //return;
                    }
                }
                this.CreateUserAttemptsSession();
            }*/
            if (this.client.IsUserAuthorized())
            {
                int num4 = (int)MessageBox.Show("Successfully Register and Connect", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //return;
            }
            else
            {
                try
                {
                    string str = await this.client.SendCodeRequestAsync(this.phoneNumber);
                    this.hash = str;
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    int num2 = (int)MessageBox.Show("Invalid phone number. Enter a phone number registered with Telegram.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    //return;
                }
            }
            this.CreateUserAttemptsSession();            
        }

        public void CreateDirectoryForSessions()
        {
            string path = Path.Combine(this.appDataFolder, "TelegramsenderData\\Sessions");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DirectoryInfo basePath = new DirectoryInfo(path);
            DirectorySecurity accessControl = basePath.GetAccessControl();
            accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference)new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier)null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            basePath.SetAccessControl(accessControl);
            this.store = new FileSessionStore(basePath);
        }

        public void CreateUserAttemptsSession()
        {
            this.userAttemptsFileFullPath = Path.Combine(Path.Combine(this.appDataFolder, "TelegramsenderData\\Sessions"), this.phoneNumber + "AT.dat");
            if (!File.Exists(this.userAttemptsFileFullPath))
            {
                File.Create(this.userAttemptsFileFullPath).Close();
                this.GrantAccess(this.userAttemptsFileFullPath);
                File.WriteAllBytes(this.userAttemptsFileFullPath, BitConverter.GetBytes(5));
                this.userAttempts = 5;
            }
            else
                this.userAttempts = BitConverter.ToInt32(File.ReadAllBytes(this.userAttemptsFileFullPath), 0);
        }

        public void GrantAccess(string fullPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
            DirectorySecurity accessControl = directoryInfo.GetAccessControl();
            accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference)new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier)null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            directoryInfo.SetAccessControl(accessControl);
        }

        private void SaveToUserAttemptsFile()
        {
            if (!File.Exists(this.userAttemptsFileFullPath))
                return;
            File.WriteAllBytes(this.userAttemptsFileFullPath, BitConverter.GetBytes(this.userAttempts));
        }
    }
}