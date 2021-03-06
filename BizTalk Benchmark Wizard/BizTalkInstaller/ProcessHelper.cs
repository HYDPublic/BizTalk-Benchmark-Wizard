﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BizTalkInstaller
{
    /// <summary>
    /// The process helper is used for executing external applications like logman.
    /// </summary>
    public class ProcessHelper : IDisposable
    {
        /// <summary>
        /// Any output information
        /// </summary>
        public static string OutPutMessage = string.Empty;
        /// <summary>
        /// Any exception information should be accessable here
        /// </summary>
        public static string ErrorMessage = string.Empty;
        /// <summary>
        /// The method will start a process and wait for it to complete. 
        /// If the process is not completed before [waitForExitSeconds] seconds, 
        /// a TimeOut exception is thrown.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <param name="waitForExitSeconds"></param>
        public void Execute(string fileName, string arguments, int waitForExitSeconds)
        {
            OutPutMessage = string.Empty;
            ErrorMessage = string.Empty;

            Process process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.WaitForExit(waitForExitSeconds); //Set to 1ms or process will never end.

            OutPutMessage = process.StandardOutput.ReadToEnd();
            ErrorMessage = process.StandardError.ReadToEnd();

            return;
        }
        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                if (ErrorMessage == null)
                    ErrorMessage = e.Data + "An Error has occurd:\n";
                else
                    ErrorMessage += e.Data;
            }
        }
        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                if (e.Data.Contains("The command completed successfully."))
                    OutPutMessage = null;
                else
                    OutPutMessage += "\n" + e.Data;
            }

        }

        #region IDisposable Members

        public void Dispose()
        {
            //ErrorMessage = null;
            //OutPutMessage = null;
        }

        #endregion
    }
}
