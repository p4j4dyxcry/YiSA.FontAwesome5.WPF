using System;

namespace YiSA.Foundation.CommandLine
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class CmdOption : Attribute
    {
        public string CommandName { get; set; } = string.Empty;
        public string ShortcutName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsRequired { get; set; } = false;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CmdFlagOption : CmdOption
    {
        
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CmdArgOption : CmdOption
    {
        public object? Default { get; set; } = null;
    }
}