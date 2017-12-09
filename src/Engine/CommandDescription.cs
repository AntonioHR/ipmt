﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ipmt.Engine
{

    public class CommandDescription
    {
        public enum OptionType
        {
            Help, Edit, PatternFile, Algorithm, Count
        }

        public enum AlgorithmType
        {
            Default, BruteForce, KMP, AhoCorasick, Sellers
        }

        private static Dictionary<string, AlgorithmType> algos = new Dictionary<string, AlgorithmType> { { "bf", AlgorithmType.BruteForce }, { "kmp", AlgorithmType.KMP }, {"aho", AlgorithmType.AhoCorasick }, { "sel", AlgorithmType.Sellers} };


        List<OptionType> opts = new List<OptionType>();
        List<string> textFiles = new List<string>();


        public AlgorithmType Algorithm { get; private set; } = AlgorithmType.Default;
        public int EditDistance { get; private set; } = 0;
        public string Patternfile { get; private set; }
        public string Pattern { get; private set; }
        public IEnumerable<string> TextFiles { get { return textFiles.AsEnumerable(); } }
        public IEnumerable<OptionType> Options { get { return opts.AsEnumerable(); } }
        public bool Contains(OptionType type)
        {
            return opts.Contains(type);
        }
        public bool IsExactMatching
        {
            get
            {
                return Algorithm == AlgorithmType.BruteForce
                    || Algorithm == AlgorithmType.KMP
                    || Algorithm== AlgorithmType.AhoCorasick;
            }
        }

        public static CommandDescription ParseFrom(string[] commands)
        {
            CommandDescription result = new CommandDescription();

            int i = 0;

            while (i < commands.Length)
            {
                if (commands[i][0] != '-')
                    break;
                ReadOption(ref commands, ref i, result);
                i++;
            }

            if (i < commands.Length)
            {
                if (result.Contains(OptionType.PatternFile))
                {
                    result.Patternfile = commands[i];
                }
                else
                {
                    result.Pattern = commands[i];
                }
                i++;
            }

            while (i < commands.Length)
            {
                result.textFiles.Add(commands[i]);
                i++;
            }

            return result;
        }

        private static void ReadOption(ref string[] commands, ref int i, CommandDescription result)
        {
            switch (commands[i][1])
            {
                case '-':
                    ReadVerboseOption(ref commands, ref i, ref result);
                    break;
                case 'h':
                    ReadHelp(result);
                    break;
                case 'p':
                    ReadPatFile(result);
                    break;
                case 'a':
                    ReadAlgo(commands, ref i, result);
                    break;
                case 'c':
                    ReadCount(result);
                    break;
                case 'e':
                    ReadEdit(commands, ref i, result);
                    break;
            }
        }

        private static void ReadVerboseOption(ref string[] commands, ref int i, ref CommandDescription result)
        {
            switch (commands[i])
            {
                case "--help":
                    ReadHelp(result);
                    break;
                case "--pattern":
                    ReadPatFile(result);
                    break;
                case "--algorithm_name":
                    ReadAlgo(commands, ref i, result);
                    break;
                case "--count":
                    ReadCount(result);
                    break;
                case "--edit":
                    ReadEdit(commands, ref i, result);
                    break;
            }
        }

        private static void ReadEdit(string[] commands, ref int i, CommandDescription result)
        {
            result.opts.Add(OptionType.Algorithm);
            i++;
            result.EditDistance = Int32.Parse(commands[i]);
        }

        private static void ReadCount(CommandDescription result)
        {
            result.opts.Add(OptionType.Count);
        }

        private static void ReadAlgo(string[] commands, ref int i, CommandDescription result)
        {
            result.opts.Add(OptionType.Algorithm);
            i++;
            result.Algorithm = algos[commands[i]];
        }

        private static void ReadPatFile(CommandDescription result)
        {
            result.opts.Add(OptionType.PatternFile);
        }

        private static void ReadHelp(CommandDescription result)
        {
            result.opts.Add(OptionType.Help);
        }

    }


}