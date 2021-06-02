using System;
using System.Collections.Generic;

namespace Amogus
{
    public class Interpreter
    {
        private int _accumulator;
        private Stack<int> _stack = new();

        private IList<string> _program;
        private Action _selectedCommand;
        private int _pc;
        private Dictionary<int, int> _loops = new();
        private Random _random = new();

        private void IncrementAccumulator()
        {
            _accumulator++;
        }

        private void PushAccumulator()
        {
            _stack.Push(_accumulator);
        }

        private void PopStack()
        {
            _stack.Pop();
        }

        private void PrintStackChar()
        {
            Console.Write((char) _stack.Peek());
        }

        private void ReadChar()
        {
            _stack.Push(Console.ReadKey(true).KeyChar);
        }

        private void PopRandom()
        {
            for (var i = 0; i < _random.Next(0, _accumulator); i++)
            {
                _stack.Pop();
            }
        }

        private void PrintStackValue()
        {
            Console.Write(_stack.Peek());
        }

        private void DecrementAccumulator()
        {
            _accumulator--;
        }

        private void SetAccumulatorToStack()
        {
            _accumulator = _stack.Peek();
        }

        private void DuplicateStackValue()
        {
            _stack.Push(_stack.Peek());
        }

        private void SetAccumulatorToZero()
        {
            _accumulator = 0;
        }

        private static void PrintAmongUs()
        {
            Console.Write("AMONG US");
        }

        private void Loop()
        {
            // Return if the stack is empty
            if (_stack.Count <= 0) return;
            
            if (_stack.Peek() == 0)
                _pc = _loops[_pc];
        }

        private void LoopBack()
        {
            // Return if the stack is empty
            if (_stack.Count <= 0) return;
            
            if (_stack.Peek() != 0)
                _pc = _loops[_pc];
        }

        private void ScanLoops()
        {
            var loopStack = new Stack<int>();
            for (var i = 0; i < _program.Count; i++)
            {
                switch (_program[i])
                {
                    case "WHO?":
                        loopStack.Push(i);
                        break;
                    case "WHERE?":
                        if (!loopStack.TryPop(out var who))
                            throw new ArgumentException($"Mismatched loop at command {i}");
                        
                        _loops[who] = i;
                        _loops[i] = who;
                        break;
                }
            }
            
            if (loopStack.Count > 0)
                throw new ArgumentException($"Mismatched loop at command {loopStack.Peek()}");
        }
        
        private Action DecodeColorCommand(string colorCommand) => colorCommand switch
        {
               "RED" => IncrementAccumulator,
              "BLUE" => PushAccumulator,
            "PURPLE" => PopStack,
             "GREEN" => PrintStackChar,
            "YELLOW" => ReadChar,
              "CYAN" => PopRandom,
             "BLACK" => PrintStackValue,
             "WHITE" => DecrementAccumulator,
             "BROWN" => SetAccumulatorToStack,
              "LIME" => DuplicateStackValue,
              "PINK" => SetAccumulatorToZero,
            "ORANGE" => PrintAmongUs,
                   _ => throw new ArgumentException($"Invalid command \"{colorCommand}\"")
        };
        
        private void Decode(string command)
        {
            switch (command)
            {
                case "SUS":
                    _selectedCommand.Invoke();
                    break;
                case "WHO?":
                    Loop();
                    break;
                case "WHERE?":
                    LoopBack();
                    break;
                default:
                    _selectedCommand = DecodeColorCommand(command);
                    break;
            }
        }

        public void Execute()
        {
            ScanLoops();
            while (_pc < _program.Count)
            {
                Decode(_program[_pc]);
                _pc++;
            }
            
            Console.WriteLine();
        }

        public Interpreter(string program)
        {
            _program = program.Split(" ");
        }
    }
}