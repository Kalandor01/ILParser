namespace ELFParser
{
    public static class ELFInterpreter
    {
        #region Public methods
        public static void RumFromEntrypoint(ELFFile file)
        {
            // var entry = file.EntryPointHeader;
            var entry = file.SectionHeaders.FirstOrDefault(s => s.Name == Constants.SectionName.INSTRUCTIONS);
            
            if (entry is null)
            {
                return;
            }
            
            RunAsmInstructions(file.Header, (IAsmInstruction[])entry.Data!);
        }
        #endregion

        #region Private methods
        private static void InterpretX64AsmInstruction(Stack<object?> stack, X64AsmInstruction instruction)
        {
            
        }
        
        private static void RunX64AsmInstructions(X64AsmInstruction[] instructions)
        {
            var stack = new Stack<object?>();
            foreach (var instruction in instructions)
            {
                InterpretX64AsmInstruction(stack, instruction);
            }
        }

        private static void RunAsmInstructions(ELFHeader header, IAsmInstruction[] instructions)
        {
            switch (header.Architecture)
            {
                case ELFEnums.ELFArchitecture.X86:
                case ELFEnums.ELFArchitecture.AMD_X86_64:
                    RunX64AsmInstructions(instructions.Cast<X64AsmInstruction>().ToArray());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(header.Architecture), header.Architecture, "Unsupported architecture!");
            };
        }
        #endregion
    }
}