using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GPA.Service.Utilities
{
    class Logger
    {
        public static void LogException(string Message)
        {
            try
            {
                using (StreamWriter w = File.AppendText("GPA.log"))
                {
                    w.Write("Exception : ");
                    w.WriteLine("{0} -- {1}", DateTime.Now.ToString(), Message);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException.ToString();
            } 
        }

        public static void LogException(Exception exp)
        {
            try
            {
                using (StreamWriter w = File.AppendText("GPA.log"))
                {
                    w.Write("Exception : ");
                    w.WriteLine("{0} -- {1} -- {2}", DateTime.Now.ToString(), exp.Message, exp.StackTrace);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException.ToString();

            }
        }

        public static void LogInformation(string Message)
        {
            try
            {
                using (StreamWriter w = File.AppendText("GPAInformation.log"))
                {
                    w.Write("Information : ");
                    w.WriteLine("{0} -- {1}", DateTime.Now.ToString(), Message);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException.ToString();               
            }

        }


    }
}
