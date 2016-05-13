using System;
using System.Collections.Generic;
using System.IO;

namespace Registry
{
	public class Program{

		public static bool run = true;
		public static int counter = 0;
		public static int code = 0;
		public static int line = 1;
		public static string version = "1.0.5_b1";

		public static int currentLine = 0;
		public static List<string> lines = new List<string>();
		
		public static void Main(string[] args){

			Registry.add("pi",""+Math.PI);
			Registry.add("CURLINE","0");
			Registry.lockvar("pi");

			if (args.Length >= 1) { // we have a .reg file in the stream

				string ln;

				// Read the file into the buffer line by line.
				//Console.WriteLine("Loading File...");
				System.IO.StreamReader file = new System.IO.StreamReader (args [0]);
				while ((ln = file.ReadLine ()) != null) {
					lines.Add(ln);
				}
				//Console.WriteLine("File Loaded: {0} Lines", lines.ToArray().Length);
				file.Close ();

				
				
				// now we execute.
				while (currentLine < lines.ToArray().Length) {
					//Console.WriteLine("Line: {0}",currentLine);
					Registry.parseCommand(lines.ToArray()[currentLine]);
					currentLine++;
					Registry.add("CURLINE",""+currentLine);
				}
				
				// Suspend the screen.
			} else { // we dont have a file so we will interpret from the command line;
				Console.WriteLine("REGISTRY Language " + Program.version);
				Console.WriteLine("COPYRIGHT 2016 Infinite Strike.");
				while(run){
					Console.Write(">");
					Registry.parseCommand(Console.ReadLine());
				}
			}
				

			if (args.Length < 2 || args [1] == "false") {
				Console.WriteLine("Execution Haulted, Press enter to close: EXIT: " + code);
				Console.ReadLine();
			}

			Environment.Exit(code);
		}	
	}

	public class Registry{
		private static List<RegistryData> storage = new List<RegistryData>();
		private static List<string> l_list = new List<string>();

		public static bool lockvar(string key){
			if (!l_list.Contains (key)) {
				l_list.Add(key);
				return true;
			}
			return false;
		}

		public static bool unlockvar(string key){
			if (l_list.Contains (key)) {
				l_list.Remove(key);
				return true;
			}
			return false;
		}

		public static bool isLocked(string key){
			if (l_list.Contains (key)) {
				return true;
			}
			return false;
		}

		public static bool add(string key, string value){
			if (!isLocked (key)) {
				if (exists (key)) {
					set (key, value);
					return true;
				} else {
					storage.Add (new RegistryData (key, value));
					return true;
				}
			} else {
				return false;
			}
		}

		public static string get(string key){
			if (exists (key)) {
				return getObject (key).Data;
			} else {
				return "NULL";
			}
		}

		public static bool rem(string key){
			for (int i = 0; i < storage.ToArray().Length; i++){
				if (storage.ToArray()[i].Key.Equals(key)) {
					storage.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public static bool set(string key, string value){
			if (!isLocked (key)) {
				if (exists (key)) {
					getObject (key).Data = value;
					return true;
				} else {
					add (key, value);
					return true;
				}
			} else {
				return false;
			}
		}

		public static bool exists(string key){
			for (int i = 0; i < storage.ToArray().Length; i++){
				if (storage.ToArray()[i].Key.Equals(key)) {
					return true;
				}
			}
			return false;
		}

		public static RegistryData getObject(string key){
			for (int i = 0; i < storage.ToArray().Length; i++){
				if (storage.ToArray()[i].Key.Equals(key)) {
					return storage.ToArray()[i];
				}
			}
			return null;
		}

		public static void parseCommand(string comm){

			if (!comm.Contains (":")) {
				Console.WriteLine("COMPILATION ERROR: No command separator: Line " + Program.line);
				return;
			};

			String[] splited = comm.Split(':');

			String command = splited [0].Trim();
			String[] commandArgs = splited[1].Split(' ');

			Interpreter.interpret (command, commandArgs);
		}
	}

	public class RegistryData{
		public string Key;
		public string Data;

		public RegistryData(string Key, string Data){
			this.Key = Key;
			this.Data = Data;
		}
	}

	public class Interpreter{
		public static string[] keywords = {
			"PRINT", "SET", "GET", "REM",
			"ADD","##","ARITH","EXIT","READ",
			"GOTO","ARITH#SIN","ARITH#COS",
			"ARITH#TAN","ARITH#SQRT","LOCK","UNLOCK"
		};

		public static string[] constants = {
			"pi"
		};

		public static bool isCommand(string command){
			for(int i = 0; i < keywords.Length; i++){
				if (command.ToLower().Equals(keywords [i].ToLower())){
					return true;
				};
			}
			return false;
		}

		public static bool isConstant(string command){
			for(int i = 0; i < constants.Length; i++){
				if (command.ToLower().Equals(constants[i].ToLower())){
					return true;
				};
			}
			return false;
		}

		public static void interpret(string command, string[] args){;
			if(isCommand(command)){
				if (command.ToLower().Trim().Equals("##")){
					// we are a commend
					return;
				}
				if(command.ToLower().Trim().Equals("print")){

					if (args.Length < 1) {Console.WriteLine("COMPILATION ERROR: Not Enough Arguments for PRINT: Line "+ Program.line);}

					//Console.WriteLine (Registry.exists(args[0].Trim()));
					//Console.WriteLine ("CONSOLE GET " + Registry.get(args[1]).Trim());

					if (Registry.exists(args[1].Trim())) { // PRINT : x
						
						Console.WriteLine (Registry.get(args[1]).Trim());

					} else { // PRINT : This is a test
						Console.WriteLine(arrayToChars(args));
					}
					return;
				}
				if (command.ToLower().Trim().Equals("add")){
					if (args.Length < 2) {Console.WriteLine("COMPILATION ERROR: Not Enough Arguments for ADD: Line "+ Program.line);}
					bool suc = Registry.add(args[0],args[1]);
					if (suc == false) {
						Console.WriteLine("EXECUTION ERROR: Cannot create VALUE {1} with KEY {0} : Value may be locked: Line " + Program.line,args[1],args[0]);
					}
					return;
				}
				if (command.ToLower().Trim().Equals("set")){
					if (args.Length < 2) {Console.WriteLine("COMPILATION ERROR: Not Enough Arguments for SET: Line "+ Program.line);}
					bool suc = Registry.set(args[1],args[2]);
					if (suc == false) {
						Console.WriteLine("EXECUTION ERROR: Cannot set VALUE {1} to KEY {0} : Value may be locked: Line "+ Program.line,args[1],args[0]);
					}
					return;
				}
				if (command.ToLower().Trim().Equals("rem")){
					if (args.Length < 1) {Console.WriteLine("COMPILATION ERROR: Not Enough Arguments for REM: Line "+ Program.line);}
					bool suc = Registry.rem(args[0]);
					if (suc == false) {
						Console.WriteLine("EXECUTION ERROR: Cannot REMOVE {0}: Line "+ Program.line,args[0]);
					}
					return;
				}
				if (command.ToLower().Trim().Equals("exit")){
					if (args.Length == 0) {Console.WriteLine ("EXECUTION ERROR: Cannot invoke exit with no value: Line "+ Program.line); return;}
					Program.run = false;
					try{
						Program.code = int.Parse(regParse(args[1]));
					}catch(Exception e){
						Console.WriteLine ("FATAL ERROR: Cannot invoke exit with no value: Line " + Program.line);
						return;
					}
					return;
				}
				if (command.ToLower().Trim().Equals("arith")){
					if (args.Length < 1) {Console.WriteLine("COMPILATION ERROR: Not Enough Arguments for ARITH: Line "+ Program.line);}
					if (args[2].Contains ("+")) {
						string[] xpr = args [2].Split('+');
						Registry.add(args[1],arithmatic(xpr[0],"+",xpr[1]));
						return;
					}
					if (args [2].Contains ("-")) {
						string[] xpr = args [2].Split('-');
						Registry.add(args[1],arithmatic(xpr[0],"-",xpr[1]));
						return;
					}
					if (args [2].Contains ("/")) {
						string[] xpr = args [2].Split('/');
						Registry.add(args[1],arithmatic(xpr[0],"/",xpr[1]));
						return;
					}
					if (args [2].Contains ("*")) {
						string[] xpr = args [2].Split('*');
						Registry.add(args[1],arithmatic(xpr[0],"*",xpr[1]));
						return;
					}
					if (args [2].Contains ("%")) {
						string[] xpr = args [2].Split('%');
						Registry.add(args[1],arithmatic(xpr[0],"%",xpr[1]));
						return;
					}
					if (args [2].Contains ("^")) {
						string[] xpr = args [2].Split('^');
						Registry.add(args[1],arithmatic(xpr[0],"^",xpr[1]));
						return;
					}
				}
				if (command.ToLower ().Trim ().Equals ("read")) { // READ : var
					if(args.Length < 1){Console.WriteLine("COMPILATION ERROR: Not Enough Arguments to READ: Line"+ Program.line);}
					Console.Write(">>");
					string input = Console.ReadLine();
					Registry.add(args[1],input);
				}
				//arith
				if (command.ToLower ().Trim ().Equals ("arith#sin")) {
					if (args.Length < 1) {Console.WriteLine("COMPILATION ERROR: Not Enough Arguments for SIN: Line "+ Program.line);}
					Registry.add(args[1],""+Math.Sin(double.Parse(regParse(args[2]))));
				}
				if (command.ToLower ().Trim ().Equals ("arith#cos")) {
					if (args.Length < 1) {Console.WriteLine("COMPILATION ERROR: Not Enough Arguments for COS: Line "+ Program.line);}
					Registry.add(args[1],""+Math.Cos(double.Parse(regParse(args[2]))));
				}
				if (command.ToLower ().Trim ().Equals ("arith#tan")) {
					if (args.Length < 1) {Console.WriteLine("COMPILATION ERROR: Not Enough Arguments for TAN: Line "+ Program.line);}
					Registry.add(args[1],""+Math.Tan(double.Parse(regParse(args[2]))));
				}
				if (command.ToLower ().Trim ().Equals ("arith#sqrt")) {
					if (args.Length < 1) {Console.WriteLine("COMPILATION ERROR: Not Enough Arguments for SQRT: Line "+ Program.line);}
					Registry.add(args[1],""+Math.Sqrt(double.Parse(regParse(args[2]))));
				}
				if (command.ToLower ().Trim ().Equals ("goto")) {
					if (args.Length < 1) {Console.WriteLine("COMPILATION ERROR: Not Enough Arguments to GOTO: Line "+ Program.line);}
					try{
						Program.currentLine = int.Parse(args[1]) - 1;
						//Console.WriteLine(Program.currentLine);
					} catch (Exception e) {
						Console.WriteLine("FATAL ERROR: {0} : Line: {1}",e.Message,Program.line);
					}
					
				}
				if (command.ToLower ().Trim ().Equals ("lock")) {
					if (!isConstant (args [1])) {
						Console.WriteLine("EXECUTION ERROR: Cannot lock a system constant: Line " + Program.line );
					} else {
						bool suc = Registry.lockvar(args[1]);
						if (suc == false) {
							Console.WriteLine("EXECUTION ERROR: Cannot lock: " + args[1] + ": Line " + Program.line );
						}
					}
				}
				if (command.ToLower ().Trim ().Equals ("unlock")) {
					if (!isConstant (args [1])) {
						Console.WriteLine("EXECUTION ERROR: Cannot unlock a system constant: Line " + Program.line );
					} else {
						bool suc = Registry.unlockvar(args[1]);
						if (suc == false) {
							Console.WriteLine("EXECUTION ERROR: Cannot unlock: " + args[1] + ": Line " + Program.line );
						}
					}
				}
			}else{
				Console.WriteLine("COMPILATION ERROR: Unknown command {0}: Line "+ Program.line,command);
			}
		}

		public static string getOperation(string op){
			if (op.Contains ("+")) {
				return "+";
			}
			if (op.Contains ("-")) {
				return "-";
			}
			if (op.Contains ("/")) {
				return "/";
			}
			if (op.Contains ("*")) {
				return "*";
			}
			if (op.Contains ("%")) {
				return "%";
			}
			if (op.Contains("^")) {
				return "^";
			}
			return null;
		}

		public static string regParse(string s){
			if (Registry.exists (s)) {
				return Registry.get (s);
			} else {
				return s;
			}
		}

		public static string arithmatic(string x, string op, string y){
			try{
				string xx = regParse(x);
				string yy = regParse(y);


				if (op == "+") {
					return ""+(Double.Parse(xx) + Double.Parse(yy));
				}
				if (op == "-") {
					return ""+(Double.Parse(xx) - Double.Parse(yy));
				}
				if (op == "*") {
					return ""+(Double.Parse(xx) * Double.Parse(yy));
				}
				if (op == "/") {
					return ""+(Double.Parse(xx) / Double.Parse(yy));
				}
				if (op == "%") {
					return ""+(Double.Parse(xx) % Double.Parse(yy));
				}
				if (op == "^") {
					return ""+(Math.Pow(Double.Parse(xx),Double.Parse(yy)));
				}
				return ""+0;
			}catch(Exception e){
				Console.WriteLine("FATAL ERROR: {0} Line: {1}",e.Message,Program.line);
				return ""+(0);
			}
		}

		private static string arrayToChars(string[] args){
			String result = "";
			for (int i = 0; i < args.Length; i++) {
				result += args [i];
				result += " ";
			}
			return result.Trim();
		}
	}
}
