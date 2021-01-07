using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SvgResourceGenerator.CommandLine
{
    public enum CommandLineParseResult
    {
        Success,
        Failed,
        Help,
        Version
    }
    
    public class Parser
    {
        internal class CommandObject
        {
            public string Command { get; set; } = string.Empty;
            public string Arg { get; set; } = string.Empty;
        }

        private static string Version => nameof(Version).ToLower();
        private static string Help => nameof(Help).ToLower();

        public CommandLineParseResult TryParse<TCommandLineArg>(string[] args,out TCommandLineArg resultArgs) where TCommandLineArg : class
        {
            var type = typeof(TCommandLineArg);
            var result = Activator.CreateInstance(type);

            try
            {
                var commands = ArgsToCommands(args).ToArray();

                if (commands.Any(x => x.Command == Help || x.Command == "h"))
                {
                    resultArgs = default;
                    return CommandLineParseResult.Help;
                }

                if (commands.Any(x => x.Command == Version || x.Command == "v"))
                {
                    resultArgs = default;
                    return CommandLineParseResult.Version;
                }

                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var cmdOptions = properties.Where(x => x.GetCustomAttributes().Any(y => y is CmdOption)).ToArray();
                var cmdArgProps = properties.Where(x => x.GetCustomAttributes().Any(y => y is CmdArgOption)).ToArray();
                var cmdFlagProps = properties.Where(x => x.GetCustomAttributes().Any(y => y is CmdFlagOption)).ToArray();

                var reqDictionary = new Dictionary<string, bool>();
                foreach (var op in cmdOptions)
                {
                    var attr = op.GetCustomAttribute<CmdArgOption>();

                    if (attr is null)
                    {
                        resultArgs = default;
                        return CommandLineParseResult.Failed;
                    }
                    
                    reqDictionary.Add(attr.ShortcutName+attr.CommandName, attr.IsRequired);
                }

                foreach (var cmd in commands)
                {
                    foreach (var prop in cmdArgProps)
                    {
                        var attr = prop.GetCustomAttribute<CmdArgOption>();
                        
                        if (attr is null)
                        {
                            resultArgs = default;
                            return CommandLineParseResult.Failed;
                        }
                        
                        if (attr.CommandName == cmd.Command ||
                            attr.ShortcutName == cmd.Command)
                        {
                            var value = (prop.GetValue(result));

                            if (value is null && prop.PropertyType == typeof(string))
                                value = string.Empty;

                            Utility.Parse(ref value, cmd.Arg);
                            prop.SetValue(result, value);
                            reqDictionary[attr.ShortcutName + attr.CommandName] = false;
                        }
                    }

                    foreach (var prop in cmdFlagProps)
                    {
                        var attr = prop.GetCustomAttribute<CmdArgOption>();

                        if (attr is null)
                        {
                            resultArgs = default;
                            return CommandLineParseResult.Failed;
                        }
                        
                        if (attr.CommandName == cmd.Command ||
                            attr.ShortcutName == cmd.Command)
                        {
                            prop.SetValue(result, true);
                            reqDictionary[attr.ShortcutName + attr.CommandName] = false;
                        }
                    }
                }

                if (reqDictionary.Any(req => req.Value))
                {
                    resultArgs = default;
                    return CommandLineParseResult.Failed;
                }

            }
            //例外が発生した場合は--helpを表示させる
            catch (Exception)
            {
                resultArgs = default;
                return CommandLineParseResult.Failed;
            }

            resultArgs = (TCommandLineArg)result!;
            return CommandLineParseResult.Success;
        }

        /// <summary>
        /// 引数からコマンドリストを取得する
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private IEnumerable<CommandObject> ArgsToCommands(string[] args)
        {
            var commands = new Stack<CommandObject>();
            foreach (var arg in args)
            {
                if (arg.StartsWith("-"))
                {
                    commands.Push(arg.StartsWith("--")
                        ? new CommandObject() {Command = arg.Substring(2)}
                        : new CommandObject() {Command = arg.Substring(1)});
                }
                else if (commands.Any())
                    commands.Peek().Arg = arg;                    
            }
            return commands;
        }

        /// <summary>
        /// 型からヘルプ情報一覧を出力する
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<string> TypeToCommandsInfo(Type type)
        {
            var commands = new List<string>();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var cmdArgProps = properties.Where(x => x.GetCustomAttributes().Any(y => y is CmdArgOption));
            var cmdFlagProps = properties.Where(x => x.GetCustomAttributes().Any(y => y is CmdFlagOption));

            commands.Add($"-v,--version{Environment.NewLine} @ show with version");

            foreach (var propertyInfo in cmdArgProps)
            {
                var attr = propertyInfo.GetCustomAttribute<CmdArgOption>()!;
                
                var builder = new StringBuilder();

                if (attr.ShortcutName != string.Empty)
                {
                    builder.Append($"-{attr.ShortcutName}");
                    if (attr.CommandName != string.Empty)
                        builder.Append($",--{attr.CommandName}");                        
                }
                else
                {
                    if (attr.CommandName != string.Empty)
                        builder.Append($"--{attr.CommandName}");
                }
                
                builder.Append($" <arg>");

                if (attr.IsRequired)
                {
                    builder.Append(" (required)");
                }
                builder.AppendLine();
                builder.Append($" @ {attr.Description}");
                
                if (attr.Description == string.Empty)
                    builder.Append("no commentary");
     
                commands.Add(builder.ToString());
            }
            foreach (var propertyInfo in cmdFlagProps)
            {
                var attr = propertyInfo.GetCustomAttribute<CmdFlagOption>()!;
                var builder = new StringBuilder();

                if (attr.ShortcutName != string.Empty)
                {
                    builder.Append($"-{attr.ShortcutName}");
                    if (attr.CommandName != string.Empty)
                        builder.Append($",--{attr.CommandName}");                        
                }
                else
                {
                    if (attr.CommandName != string.Empty)
                        builder.Append($"--{attr.CommandName}");
                }

                if (attr.IsRequired)
                {
                    builder.Append(" (required)");
                }
                builder.AppendLine();
                
                builder.Append($" @ {attr.Description}");

                if (attr.Description == string.Empty)
                    builder.Append("no commentary");
     
                commands.Add(builder.ToString());
            }
            return commands;
        }

        public string GetVersionText()
        {
            var ver = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location);

            return $"Version{ver.ProductVersion}";
        }

        public string GetHelpText(Type type)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"available arguments");
            foreach (var info in TypeToCommandsInfo(type))
            {
                stringBuilder.AppendLine(info);
            }

            return stringBuilder.ToString();
        }
    }
    internal static class Utility
    {
        public static void Parse(ref object? value, string arg)
        {
            try
            {
                switch (value)
                {
                    case int _:
                        value = int.Parse(arg);
                        break;
                    case short _:
                        value = short.Parse(arg);
                        break;
                    case long _:
                        value = long.Parse(arg);
                        break;
                    case uint _:
                        value = uint.Parse(arg);
                        break;
                    case ushort _:
                        value = ushort.Parse(arg);
                        break;
                    case ulong _:
                        value = ulong.Parse(arg);
                        break;
                    case float _:
                        value = float.Parse(arg);
                        break;
                    case double _:
                        value = double.Parse(arg);
                        break;
                    case decimal _:
                        value = decimal.Parse(arg);
                        break;
                    case char _:
                        value = char.Parse(arg);
                        break;
                    case string _:
                        value = arg;
                        break;
                    case byte _:
                        value = byte.Parse(arg);
                        break;
                    case sbyte _:
                        value = sbyte.Parse(arg);
                        break;
                    case bool _:
                        value = bool.Parse(arg);
                        break;
                    case Enum _:
                        value = Enum.Parse(value.GetType(), arg);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}