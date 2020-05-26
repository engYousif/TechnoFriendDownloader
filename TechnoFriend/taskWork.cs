using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace TechnoFriend
{
    public class taskWork
    {
        public static int kkk = 0;
        public static string filesPath;
        string server;
        string server_name;
        string path;
        long size;
        DateTime lastModifiedDate;
        CancellationTokenSource token;
        Task task1;
        ProgressBar prg = null;
        Define temp;
        DataClasses1DataContext d;
        public static string ServerPasser = "";

        FtpWebResponse wr;
        FtpWebResponse wr2;
        FtpWebResponse resp;
        Stream ftpStream;
        FileStream outputStream;
        Dispatcher dp;
        string theCall = "";
        public bool started = false;
        double parallelVal = 0.0;//Used as a helper for Progress value to be updated (works in parallel with Progress)

        public bool isRunning = false;
        public bool isFinished = false;

        public taskWork()
        {

            //this.size = size;
            //this.lastModifiedDate = lastModifiedDate;
            token = new CancellationTokenSource();
            task1 = null;
            //temp = tmp;
            //this.server = temp.Server;
            //this.server_name = temp.Server_Name;
            //this.path = temp.File_Path;
            //if (temp.File_Size.HasValue)
            //    this.size = temp.File_Size.Value;
            //if (temp.Last_Modified.HasValue)
            //this.lastModifiedDate = temp.Last_Modified.Value;
            //this.dp = dp;
            //prg = prog;
            d = new DataClasses1DataContext();


            filesPath = (from a in d.SettingsTables select a.path).Single<string>();

        }



        public string getServerID()
        {
            return server;
        }

        public Task invokeTask()
        {
            task1 = Task.Run(() =>
            {

                //    try
                //{
                //Get the size of the file

                isRunning = true;

                #region File Size
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + temp.Server + "/" + temp.File_Path));
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFTP.Credentials = new NetworkCredential(temp.Username, temp.Password);
                reqFTP.KeepAlive = false;



                try
                {
                    wr = (FtpWebResponse)reqFTP.GetResponse();
                    wr.Close();
                }
                catch (WebException ffff)
                {
                    if (ffff.Message.Contains("file not found"))
                        return;//exit the task
                    MessageBox.Show(ffff.Message + "\n " + ((FtpWebResponse)ffff.Response).StatusDescription);
                    return;
                }


                if (wr.ContentLength < 1024)
                {
                    temp.File_Size = 1;//Set the ListBoxItem size
                    temp.Unit = "KB";

                }
                if (wr.ContentLength > 1023 && wr.ContentLength < 1048576)
                {
                    temp.File_Size = long.Parse(Math.Round((double)(wr.ContentLength / 1024)).ToString());//Set the ListBoxItem size
                    temp.Unit = "KB";

                }
                else if (wr.ContentLength > 1048575)
                {
                    temp.File_Size = long.Parse(Math.Round((double)(wr.ContentLength / 1024 / 1024)).ToString());//Set the ListBoxItem size
                    temp.Unit = "MB";

                }
                else if (wr.ContentLength > 1073741823)
                {
                    temp.File_Size = long.Parse(Math.Round((double)(wr.ContentLength / 1024 / 1024 / 1024)).ToString());//Set the ListBoxItem size
                    temp.Unit = "GB";
                    
                }

                #endregion

                #region Last Modified
                //Get last date modified
                //reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + temp.Server + "/" + temp.File_Path));
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + temp.Server + "/" + temp.File_Path));
                reqFTP.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                reqFTP.UseBinary = false;
                reqFTP.Credentials = new NetworkCredential(temp.Username, temp.Password);
                wr2 = (FtpWebResponse)reqFTP.GetResponse();



                //save last modification date to database
                temp.Last_Modified = wr2.LastModified;
                d.SubmitChanges();
                #endregion

                #region Download Parameter
                //Set downloading parameters
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + temp.Server + "/" + temp.File_Path));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(temp.Username, temp.Password);
                resp = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = resp.GetResponseStream();
                #endregion

                //Pre downloading
                long size = wr.ContentLength;//File size
                DateTime lastModified = wr2.LastModified;//Last date of modification


                int bufferSize = 1024;
                int readCount = 0;
                byte[] buffer = new byte[bufferSize];

                //Download info
                string server = temp.Server;
                string server_name = temp.Server_Name;
                string file_path = temp.File_Path;
                string url = filesPath + "\\" + temp.Server + temp.Port + "\\" + lastModified.ToString("MM-dd-yyyy-hh-mm-ss-tt") + temp.File_Path;
                //long file_size = temp.File_Size.Value;

                //Pre Checking

                //Check directory existence
                if (!Directory.Exists(filesPath + "\\" + temp.Server + temp.Port))
                {
                    Directory.CreateDirectory(filesPath + "\\" + temp.Server + temp.Port);
                }
                outputStream = new FileStream(url, FileMode.OpenOrCreate);//Set the output file

                //Get the file properties
                FileInfo f = new FileInfo(url);
                

                #region Partial Download
                if (temp.Last_Modified == wr2.LastModified && size != f.Length && f.Exists == true && f.Length != 0)//means some bytes lost
                {
                    bool canceled = false;
                    //Time of downloading and the difference
                    Stopwatch timer = new Stopwatch();
                    //Downloaded bytes count
                    int bytescounter = 0;
                    //Milliseconds counter
                    long mscounter = 0;

                    //Move the pointer of stream to the last byte of the incomplete file
                    if (outputStream.CanSeek)
                    {
                        outputStream.Position = f.Length;
                    }

                    reqFTP.Abort();
                    //Set downloading parameters
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + temp.Server + "/" + temp.File_Path));
                    reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                    reqFTP.ContentOffset = f.Length;

                    reqFTP.UseBinary = true;
                    //reqFTP.Credentials = new NetworkCredential("MyUser", "Newnew10");
                    reqFTP.Credentials = new NetworkCredential(temp.Username, temp.Password);
                    reqFTP.KeepAlive = false;


                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();



                    Stream partialStream = response.GetResponseStream();
                    bufferSize = 1024;
                    readCount = partialStream.Read(buffer, 0, bufferSize);

                    if (theCall != "external")
                        dp.Invoke(() => prg.Value = (Convert.ToDouble(f.Length) / Convert.ToDouble(size)) * 100.0);

                    parallelVal += (Convert.ToDouble(f.Length) / Convert.ToDouble(size)) * 100.0;
                    temp.progress = Math.Round(parallelVal, 2, MidpointRounding.ToEven);
                    (from k in d.Defines
                     where k.Id == temp.Id
                     select k).ToList<Define>().ForEach(x => x.progress = temp.progress);

                    if (theCall != "external")
                        prg.Dispatcher.Invoke(() => prg.Value = temp.progress);
                    d.SubmitChanges();
                    while (readCount > 0)
                    {
                        timer.Start();//Time pointer


                        if (token.IsCancellationRequested)
                        {
                            try
                            {
                                canceled = true;
                                break;

                                //token.Token.ThrowIfCancellationRequested();
                            }
                            catch (Exception gg)
                            {

                                Close();
                                return;
                            }
                        }
                        else
                        {

                            outputStream.Write(buffer, 0, readCount); outputStream.Flush();
                            readCount = partialStream.Read(buffer, 0, bufferSize);


                            mscounter += timer.ElapsedMilliseconds;
                            bytescounter += bufferSize;//increment the bytescounter

                            if (mscounter < 1000 && bytescounter >= (bufferSize * temp.Speed))//If less than 1000 it means the speed is high and we should limit it.. If it is more than 1000 then we do not care because the speed is slow and we still have not reached the limits of the bandwidth-> the condition based on two variables, the other one represents the downloaded bytes during the period of 1000 milliseconds
                            {
                                TimeSpan elapsedMs = new TimeSpan(timer.ElapsedMilliseconds);
                                Task.Delay(TimeSpan.FromMilliseconds(1000) - elapsedMs).Wait();
                                mscounter = 0;
                                bytescounter = 0;
                            }
                            //MessageBox.Show("9");
                            //Dispatcher.Invoke(() => prg.Value += 100.0 / (size / (readCount)));
                            if (readCount == 0)
                            {
                                dp.Invoke(() => prg.Value = 100.0);
                                temp.progress = 100.0;
                                (from k in d.Defines
                                 where k.Id == temp.Id
                                 select k).ToList<Define>().ForEach(x => x.progress = temp.progress);
                                prg.Dispatcher.Invoke(() => prg.Value = temp.progress);
                            }
                            else
                            {
                                if (theCall != "external")
                                    dp.Invoke(() => prg.Value += 100.0 / (size / (readCount)));
                                parallelVal += 100.0 / (Convert.ToDouble(size) / Convert.ToDouble(readCount));//Helper
                                temp.progress = Math.Round(parallelVal, 2, MidpointRounding.ToEven);
                                d.SubmitChanges();
                                //(from k in d.Defines
                                // where k.Id == temp.Id
                                // select k).ToList<Define>().ForEach(x => x.progress = temp.progress);
                                if (theCall != "external")
                                    prg.Dispatcher.Invoke(() => prg.Value = temp.progress);
                            }
                            //dp.Invoke(() => prg.Value += 100.0 / (size / (readCount)));
                            timer.Reset();
                            mscounter = 0;
                            
                        }
                        

                    }
                    outputStream.Flush();
                    ServerPasser = temp.Server;
                    reqFTP.Abort();
                    response.Close();
                    partialStream.Close();
                    outputStream.Close();
                    wr2.Close();
                    wr.Close();
                    //MessageBox.Show("Download Completed..!");
                    if (canceled == false)//if canceled is true then we should not doDecrypt
                    {
                        byte[] key = Encoding.ASCII.GetBytes(@"2Techno0Friend20");
                        byte[] IV = Encoding.ASCII.GetBytes("@3l2-dld56/==-2k");
                        //DoDecrypt(path + "\\Backup\\eBackup.zip", path + "\\Backup\\Backup.zip", key, IV);
                        DoDecrypt(url, url, key, IV);
                        //readCount = ftpStream.Read(buffer, 0, bufferSize);
                    }


                }

                #endregion


                #region Initial Download
                else if (f.Exists == true && f.Length == 0)//Download from begining
                {
                    bool canceled = false;
                    //Read first block
                    readCount = ftpStream.Read(buffer, 0, bufferSize);

                    //Time of downloading and the difference
                    Stopwatch timer = new Stopwatch();
                    int bytescounter = 0;
                    long mscounter = 0;
                    //Status list's item update the size of the file


                    while (readCount > 0)
                    {
                        timer.Start();//Time pointer


                        if (token.IsCancellationRequested)
                        {
                            canceled = true;
                            break;
                            //token.Token.ThrowIfCancellationRequested();
                        }


                        outputStream.Write(buffer, 0, readCount);//Write
                        readCount = ftpStream.Read(buffer, 0, bufferSize);//Read


                        mscounter += timer.ElapsedMilliseconds;
                        bytescounter += bufferSize;//increment the bytescounter

                        if (mscounter < 1000 && bytescounter >= (bufferSize * temp.Speed))//If less than 1000 it means the speed is high and we should limit it.. If it is more than 1000 then we do not care because the speed is slow and we still have not reached the limits of the bandwidth-> the condition based on two variables, the other one represents the downloaded bytes during the period of 1000 milliseconds
                        {
                            TimeSpan elapsedMs = new TimeSpan(timer.ElapsedMilliseconds);
                            Task.Delay(TimeSpan.FromMilliseconds(1000) - elapsedMs).Wait();
                            mscounter = 0;
                            bytescounter = 0;
                        }

                        //Dispatcher.Invoke(() => prg.Value += 100.0 / (size / (readCount)));
                        if (readCount == 0)
                        {
                            if (theCall != "external")
                                dp.Invoke(() => prg.Value = 100.0);
                            temp.progress = 100.0;
                            (from k in d.Defines
                             where k.Id == temp.Id
                             select k).ToList<Define>().ForEach(x => x.progress = temp.progress);
                            if (theCall != "external")
                                prg.Dispatcher.Invoke(() => prg.Value = temp.progress);
                        }
                        else
                        {
                            if (theCall != "external")
                                dp.Invoke(() => prg.Value += 100.0 / (size / (readCount)));
                            parallelVal += 100.0 / (size / (readCount));
                            temp.progress = Math.Round(parallelVal, 2, MidpointRounding.ToEven);
                            (from k in d.Defines
                             where k.Id == temp.Id
                             select k).ToList<Define>().ForEach(x => x.progress = temp.progress);
                            if (theCall != "external")
                                prg.Dispatcher.Invoke(() => prg.Value = temp.progress);
                        }
                        timer.Reset();
                        mscounter = 0;
                        d.SubmitChanges();
                    }

                    ServerPasser = temp.Server;
                    ftpStream.Close();
                    reqFTP.Abort();
                    outputStream.Close();
                    wr2.Close();
                    wr.Close();
                    //MessageBox.Show("Download Finished..!");
                    if (canceled == false)
                    {
                        byte[] key = Encoding.ASCII.GetBytes(@"2Techno0Friend20");
                        byte[] IV = Encoding.ASCII.GetBytes("@3l2-dld56/==-2k");
                        //DoDecrypt(path + "\\Backup\\eBackup.zip", path + "\\Backup\\Backup.zip", key, IV);
                        DoDecrypt(url, url, key, IV);
                    }

                }

                #endregion

                #region Already Downloaded
                else if (f.Exists == true && f.Length == size)
                {
                    //MessageBox.Show("File already exist and complete !..");
                    this.Close();

                }
                #endregion

                #region Zero Size
                //if(f.Exists == true && f.Length == 0)
                //{
                //    MessageBox.Show("File size is zero!..Empty file.");
                //    this.Close();
                //}
                #endregion

                //}

                //catch (Exception ee)
                //{
                //    if (ee.Message.Contains("The remote name could not be resolved"))
                //    {
                //        MessageBox.Show("Please check your internet connection..or the remote server information is not correct.");
                //    }
                //    else
                //        MessageBox.Show(ee.Message);
                //}
                isRunning = false;
                isFinished = true;
            }, token.Token);


            return task1;
        }

        private void DoDecrypt(string inputFile, string outputFile, byte[] Key, byte[] IV)
        {
            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            RijndaelManaged RMCrypto = new RijndaelManaged();


            RMCrypto.Padding = PaddingMode.PKCS7;

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateDecryptor(Key, IV),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();
        }

        public Task getTask()
        {
            return invokeTask();
        }

        public void Cancel()
        {
            token.Cancel();
            started = false;


        }

        public void Close()
        {
            token.Cancel();
            try
            {
                ftpStream.Close();
                outputStream.Close();
                wr2.Close();
                wr.Close();
            }
            catch (Exception r) { }
        }



        public void TaskStart(Define tmp)
        {
            
            theCall = "external";
            token = new CancellationTokenSource();
            temp = tmp;
            this.server = temp.Server;
            this.server_name = temp.Server_Name;
            this.path = temp.File_Path;
            if (temp.File_Size.HasValue)
                this.size = temp.File_Size.Value;
            if (temp.Last_Modified.HasValue)
                this.lastModifiedDate = temp.Last_Modified.Value;
            //}
            
            d.SubmitChanges();

            try
            {
                started = true;
                invokeTask();// to initiate task1
                
                //task1.Start();

            }
            catch (Exception g) { }
        }
            
        public void Start(Define tmp, ref ProgressBar prog, Dispatcher dp, string call)
        {
            //token = new CancellationTokenSource();
            theCall = call;
            //if (temp == null && prg == null && this.dp == null)
            //{
                //temp = (from k in d.Defines where tmp.Id == k.Id select k).ToList()[0];
                temp = tmp;
                this.server = temp.Server;
                this.server_name = temp.Server_Name;
                this.path = temp.File_Path;
                if (temp.File_Size.HasValue)
                    this.size = temp.File_Size.Value;
                if (temp.Last_Modified.HasValue)
                    this.lastModifiedDate = temp.Last_Modified.Value;
                this.dp = dp;
                prg = prog;
            //}


            //Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => { MainWindow.progressBars[def.Id].Visibility = Visibility.Hidden; }));
            d.SubmitChanges();

            try
            {
                invokeTask();// to initiate task1
                started = true;
                task1.Start();

            }
            catch (Exception g) { }


        }










        public long getFileSize()
        {

            return this.size;

        }

        public string Status()
        {
            return task1.Status.ToString();
        }

    }
}
