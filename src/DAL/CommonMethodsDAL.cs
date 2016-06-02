using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HRMS.DAL
{
    public class CommonMethodsDAL
    {
        private HRMSDBEntities dbContext = new HRMSDBEntities();

        //static
        public string GetMaxRoleForUser(params string[] userRoles)
        {
            try
            {
                //string[] role = Roles.GetRolesForUser(Membership.GetUser().UserName);
                string roleToPass = null;
                int no = 0;
                int[] arr = new int[userRoles.Length];
                int p = 0;
                foreach (string role in userRoles)
                {
                    // String role = userrole;
                    //  role.Replace("\\s", "");
                    if (role == UserRoles.HRAdmin)
                    {
                        no = (int)EmployeeRolesOrder.HRAdmin;
                    }
                    else
                    {
                        if (role == UserRoles.RMG)
                        {
                            no = (int)EmployeeRolesOrder.RMG;
                        }
                        else
                        {
                            if (role == UserRoles.HRExecutive)
                            {
                                no = (int)EmployeeRolesOrder.HRExecutive;
                            }
                            else
                            {
                                if (role == UserRoles.Developers)
                                {
                                    no = (int)EmployeeRolesOrder.Developers;
                                }
                                else if (role == UserRoles.Manager)
                                {
                                    no = (int)EmployeeRolesOrder.Manager;
                                }
                            }
                        }
                    }

                    if (no != 0)
                    {
                        arr[p] = no;
                        p++;
                        no = 0;
                    }
                }

                int maxVal = arr[0];
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] != 0)
                    {
                        if (arr[i] < maxVal)
                            maxVal = arr[i];
                    }
                }

                if (maxVal == (int)EmployeeRolesOrder.HRAdmin)
                {
                    roleToPass = UserRoles.HRAdmin;
                }
                else
                {
                    if (maxVal == (int)EmployeeRolesOrder.RMG)
                    {
                        roleToPass = UserRoles.RMG;
                    }
                    else
                    {
                        if (maxVal == (int)EmployeeRolesOrder.HRExecutive)
                        {
                            roleToPass = UserRoles.HRExecutive;
                        }
                        else
                        {
                            if (maxVal == (int)EmployeeRolesOrder.Developers)
                            {
                                roleToPass = "Developers";
                            }
                            else if (maxVal == (int)EmployeeRolesOrder.Manager)
                            {
                                roleToPass = "Manager";
                            }
                        }
                    }
                }
                return roleToPass;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            //byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            byte[] toEncryptArray;
            string content = cipherString.Replace("-", "");
            if (content.Contains(" "))
                toEncryptArray = Convert.FromBase64String(content.Replace(" ", "+"));
            else
                toEncryptArray = Convert.FromBase64String(content);

            System.Configuration.AppSettingsReader settingsReader =
                                                new System.Configuration.AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = (string)settingsReader.GetValue("SecurityKey",
                                                         typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
            // Get the key from config file

            string key = (string)settingsReader.GetValue("SecurityKey",
                                                             typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public List<EmployeeMailTemplate> GetEmailTemplate(int templateId)
        {
            try
            {
                List<EmployeeMailTemplate> template = new List<EmployeeMailTemplate>();

                template = (from emailBody in dbContext.HRMS_EmailTemplates
                            where emailBody.EmailTemplateId == templateId
                            select new EmployeeMailTemplate
                            {
                                Message = emailBody.EmailBody,
                                Subject = emailBody.EmailSubject
                            }).ToList();
                return template;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}