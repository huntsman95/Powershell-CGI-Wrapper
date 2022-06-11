using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;

string defaultHeader = "Content type: text/html" + Environment.NewLine + Environment.NewLine;

string psargs = HttpUtility.UrlDecode(Environment.GetEnvironmentVariable("QUERY_STRING"));
psargs = psargs.Replace("\"", "\"\"");
string[] psargarray = psargs.Split('&');
for(int i = 0; i < psargarray.Length; i++)
{
    psargarray[i] = Regex.Replace(psargarray[i], @"(\w+)=(.*)", "-$1 \"$2\"");
}
psargs = String.Join(' ',psargarray);

Process cmd = new Process();
cmd.StartInfo.FileName = "pwsh.exe";
cmd.StartInfo.Arguments = "-noprofile -file " + args[0] + " " + psargs;
cmd.StartInfo.RedirectStandardInput = true;
cmd.StartInfo.RedirectStandardOutput = true;
cmd.StartInfo.CreateNoWindow = true;
cmd.StartInfo.UseShellExecute = false;
cmd.Start();

string OutputBuffer = "";

while (!cmd.HasExited)
{
    OutputBuffer += cmd.StandardOutput.ReadToEnd();
}

if (!(Regex.Match(OutputBuffer, "Content-type:.*").Success))
{
    OutputBuffer = defaultHeader + OutputBuffer;
}

//cmd.WaitForExit();
Console.WriteLine(OutputBuffer);