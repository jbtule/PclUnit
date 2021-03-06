﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TestAll
{
    class Program
    {
        static int Main(string[] args)
        {

           var whoTestsTestersDir = Path.GetFullPath(Path.Combine("..","..",".."));
           var runTestsConfig = Path.Combine(whoTestsTestersDir, "Tests", "RunTests.yml");
#if DEBUG
           var build = "Debug";
#else
           var build = "Release";
#endif

           var pclrunnerDir = Path.Combine(whoTestsTestersDir, "..", "Runner", "pclunit-runner", "bin", build);
           var conventionTesterPath = Path.Combine(whoTestsTestersDir, "ConventionTestProcessor", "bin", build, "ConventionTestProcessor.exe");

            var pclrunner = Path.Combine(pclrunnerDir, "pclunit-runner.exe");
            var testOutput = Path.Combine(pclrunnerDir, "TestAll.json");

            File.Delete(testOutput);

            var runnerArgs = String.Format("runConfig -d Configuration=Debug --noerror -o \"{0}\" \"{1}\"", testOutput, runTestsConfig);
            var runner = new Process()
                             {
                                 StartInfo = new ProcessStartInfo(pclrunner, runnerArgs)
                                                 {
                                                     UseShellExecute = false,
                                                 }
                             };



            runner.Start();
            runner.WaitForExit();

            var conventionProcessor = new Process()
            {
                StartInfo = new ProcessStartInfo(conventionTesterPath, string.Format("\"{0}\"", testOutput))
                {
                    UseShellExecute = false,
                }
            };
            conventionProcessor.Start();
            conventionProcessor.WaitForExit();

            return conventionProcessor.ExitCode;
        }
    }
}
