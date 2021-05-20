using System;
using System.ComponentModel;

namespace EARIN_EX6.Helpers {
    public static class ExceptionHelper {

        public enum ExceptionType {
            [Description("Invalid input")]
            InvalidInput,

            [Description("Validation error")]
            ValidationError,
        }

        public static void ThrowAndExit(ExceptionType exceptionType, string errorMessage) {
            WriteErrorMessage(exceptionType, errorMessage);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            Environment.Exit(0x2);
        }

        public static void ThrowAndExit(ExceptionType exceptionType, params string[] errorMessages) {
            foreach (var errorMessage in errorMessages) {
                WriteErrorMessage(exceptionType, errorMessage);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            Environment.Exit(0x2);
        }

        public static void ThrowAndContinue(ExceptionType exceptionType, string errorMessage) {
            WriteErrorMessage(exceptionType, errorMessage);
        }

        private static void WriteErrorMessage(ExceptionType exceptionType, string errorMessage) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exceptionType.GetDescription());
            Console.WriteLine(errorMessage);
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
