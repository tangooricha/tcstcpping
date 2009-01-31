/*
 * Created by SharpDevelop.
 * User: tangooricha
 * Date: 2009/01/25
 * Time: 12:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net.Sockets;

namespace TCSTCPPing
{
	class TCSTCPPing
	{
		public static void Main(string[] args)
		{
			// TODO: Implement Functionality Here
			_globeSetting globeSetting=new _globeSetting();
			if(args.Length==0)
			{
				Console.WriteLine("######################");
				Console.WriteLine("Powered by Tangooricha");
				Console.WriteLine("######################");
				Console.WriteLine("");
				Console.WriteLine("This is a simple network tool which is used to check the response time and the error of the remote host's TCP port powered by Tangooricha.");
				Console.WriteLine("");
				Console.WriteLine("Usage: TCSTCPPing <hostname>[:port] [-c <counter>|-t]");
				Console.WriteLine("TCSTCPPing - The tool's name.");
				Console.WriteLine("hostname   - The IP address or domain name of the remote target host.");
				Console.WriteLine("port       - The port that you use to check,the default value is 80.");
				Console.WriteLine("-c         - The switcher of the counter,means the times of the test you do.");
				Console.WriteLine("             And after the argument -c,you must give a integer number.");
				Console.WriteLine("-t         - It means test the remove host until you stop it.");
				Console.WriteLine("Argument -t and -c are mutually exclusive.");
				Console.WriteLine("");
				Console.WriteLine("");
				Console.Write("Press any key to continue . . . ");
				Console.ReadKey(true);
				return;
			}
			else
			{
				Console.WriteLine("######################");
				Console.WriteLine("Powered by Tangooricha");
				Console.WriteLine("######################");
				Console.WriteLine("");
				for(int i=1;i<args.Length;i++)
				{
					if(args[i]=="-t")
					{
						globeSetting.tSet=true;
					}
					else if(args[i]=="-c")
					{
						globeSetting.cSet=true;
						int.TryParse(args[i+1],out globeSetting.counter);
						i++;
					}
				}
				if(globeSetting.tSet==true&&globeSetting.cSet==true)
				{
					Console.WriteLine("Argument -t and -c are mutually exclusive.");
					Console.Write("Press any key to continue . . . ");
					Console.ReadKey(true);
					return;
				}
			}
			string hostNameAndPort=args[0];
			//string hostNameAndPort="www.microsoft.com:80";
			string hostName,hostPort;
			int hostPortNum;
			
			//Console.WriteLine(hostNameAndPort);
			if(hostNameAndPort.Contains(":"))
			{
				int hostNameLength=hostNameAndPort.IndexOf(":");
				hostName=hostNameAndPort.Remove(hostNameLength);
				hostName=hostNameAndPort.Remove(hostNameLength);
				Console.WriteLine("Target Address is {0}",hostName);
				hostPort=hostNameAndPort.Remove(0,hostNameLength+1);
				Console.WriteLine("Target Port is {0}",hostPort);
				int.TryParse(hostPort,out hostPortNum);
			}
			else
			{
				hostName=hostNameAndPort;
				Console.WriteLine("Target IP Address is {0}",hostName);
				hostPort="80";
				Console.WriteLine("Target Port is {0}",hostPort);
				hostPortNum=80;
			}
			
			for(int i=globeSetting.counter,j=0;i>0;i--)
			{
				if(globeSetting.tSet==true)
				{
					i++;
				}
				TcpClient client=new TcpClient();
				DateTime startTime,stopTime;
				TimeSpan duration;
				startTime=DateTime.Now;
				try
				{
					client.Connect(hostName,hostPortNum);
					stopTime=DateTime.Now;
					client.Close();
					duration=stopTime-startTime;
					Console.WriteLine("{0}	established	{1}ms",j+1,duration.TotalMilliseconds);
				}
				catch(SocketException e)
				{
					//Console.WriteLine("{0}",e.ErrorCode);
					if(e.ErrorCode==10061)
					{
						stopTime=DateTime.Now;
						duration=stopTime-startTime;
						Console.WriteLine("{0}	refuse	{1}ms",j+1,duration.TotalMilliseconds);
					}
					else
					{
						Console.WriteLine("{0}	Request timeout!	{1}	{2}",j+1,e.ErrorCode,e.Message);
					}
				}
				j++;
			}
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
	class _globeSetting
	{
		public bool tSet=false;
		public bool cSet=false;
		public int counter=10;
	}
}
