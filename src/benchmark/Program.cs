using BenchmarkDotNet.Running;

#if DEBUG
     var perf = new Perf();
    perf.GlobalSetup();
    //perf.DeserializeApiManifest();
    perf.AutoDeserializeApiManifest();
#endif

#if RELEASE
    BenchmarkRunner.Run(typeof(Program).Assembly);
#endif