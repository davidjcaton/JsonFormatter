using System;
using Mono.Options;

namespace GlowingBrain.Utilities.JsonPrettyPrint
{
    class Settings
    {
        private readonly OptionSet _optionSet;

        public Settings()
        {
            _optionSet = new OptionSet
            {
                { "i=|inputfile=", v => InputFile = v },
                { "r=|rulesfile=", v => RulesFile = v },
                { "o=|outputfile=", v => OutputFile = v },
                { "?|h|help", v => ShowHelp = true },
                { "ser|showexamplerules", v => ShowExampleRulesFile = true }
            };
        }

        public string InputFile { get; private set; }
        public string RulesFile { get; private set; }
        public string OutputFile { get; private set; }
        public bool ShowHelp { get; private set; }
        public bool ShowExampleRulesFile { get; private set; }

        public void Parse(string[] args)
        {
            _optionSet.Parse(args);
        }

        public void WriteOptionDescriptions()
        {
            _optionSet.WriteOptionDescriptions(Console.Out);
        }
    }
}