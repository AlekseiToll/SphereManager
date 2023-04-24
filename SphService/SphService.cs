using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SphServiceLib
{
	public class SphService
	{
		private static string currentDirectory_;

		private static object logFailedLock_ = new object();
		private static object logGeneralLock_ = new object();

		public static string logGeneralName;
		public static string logFailedName;

		public static void Init()
		{
			currentDirectory_ = System.AppDomain.CurrentDomain.BaseDirectory;

			logGeneralName = currentDirectory_ + "LogGeneral.txt";
			logFailedName = currentDirectory_ + "LogFailed.txt";
		}

		public static void DumpException(Exception ex, string info)
		{
			StreamWriter sw = null;

			try
			{
				lock (logFailedLock_)
				{
					try
					{
						sw = new StreamWriter(logFailedName, true);
						sw.WriteLine("Exception at " + DateTime.Now.ToString());

						if (ex is SphException)
						{
							sw.WriteLine(info);
							sw.WriteLine((ex as SphException).Message);
							return;
						}

						sw.WriteLine(info);
						System.Diagnostics.Debug.WriteLine(info);

						System.Diagnostics.Debug.WriteLine("--------- Outer Exception Data ---------");
						sw.WriteLine("========= Exception Dump ===============");
						sw.WriteLine("--------- Outer Exception Data ---------");
						WriteExceptionInfo(ex, ref sw);

						ex = ex.InnerException;
						while (ex != null)
						{
							System.Diagnostics.Debug.WriteLine("--------- Inner Exception Data ---------");
							sw.WriteLine("--------- Inner Exception Data ---------");
							WriteExceptionInfo(ex, ref sw);
							ex = ex.InnerException;
						}
						sw.WriteLine("========= end of Exception Dump ========");
					}
					finally
					{
						if (sw != null) sw.Close();
					}
				}
			}
			catch (IOException)
			{
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine("Error in DumpException() " + e.Message);
				throw e;
			}
		}

		public static void WriteToLogFailed(string s)
		{
			StreamWriter sw = null;

			try
			{
				System.Diagnostics.Debug.WriteLine(s);
				lock (logFailedLock_)
				{
					try
					{
						sw = new StreamWriter(logFailedName, true);

						sw.WriteLine("=======================================");
						sw.WriteLine(DateTime.Now);
						sw.WriteLine(s);
						sw.WriteLine("=======================================");
					}
					finally
					{
						if (sw != null) sw.Close();
					}
				}
			}
			catch (IOException)
			{
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error in WriteToLogFailed() " + ex.Message);
				throw;
			}
		}

		public static void WriteToLogGeneral(string s)
		{
			StreamWriter sw = null;
			try
			{
				System.Diagnostics.Debug.WriteLine(s);
				lock (logGeneralLock_)
				{
					try
					{
						sw = new StreamWriter(logGeneralName, true);
						sw.WriteLine(s);
					}
					finally
					{
						if (sw != null) sw.Close();
					}
				}
			}
			catch (IOException)
			{
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error in WriteToLogGeneral() " + ex.Message);
				throw;
			}
		}

		public static void WriteExceptionInfo(Exception ex, ref StreamWriter sw)
		{
			System.Diagnostics.Debug.WriteLine("\nMessage: {0}", ex.Message);
			sw.WriteLine("\nMessage: {0}", ex.Message);
			System.Diagnostics.Debug.WriteLine("\nException Type: {0}", ex.GetType().FullName);
			sw.WriteLine("\nException Type: {0}", ex.GetType().FullName);
			System.Diagnostics.Debug.WriteLine("\nSource: {0}", ex.Source);
			sw.WriteLine("\nSource: {0}", ex.Source);
			System.Diagnostics.Debug.WriteLine("\nStrackTrace: {0}", ex.StackTrace);
			sw.WriteLine("\nStrackTrace: {0}", ex.StackTrace);
			System.Diagnostics.Debug.WriteLine("\nTargetSite: {0}", ex.TargetSite.ToString());
			sw.WriteLine("\nTargetSite: {0}", ex.TargetSite);
		}

		public static string AppDirectory
		{
			get
			{
				return currentDirectory_;
			}
		}
	}

	public class SphException : Exception
	{
		private string message_;

		public SphException(string mess)
		{
			message_ = mess;
		}

		public override string Message
		{
			get { return message_; }
		}
	}
}
