# Demo multithreads

## Trainer Instructions

 Open up the [DemoMultiThreads](demo-multi-threads) project

 Walk through the

- `Cooking.GoCook` method, showing what a cook will do
- Implementation of Cooks as Threads, in the `ThreadsImplementation` class, showing that
  - each thread/cook has a name
  - we have to control the threads
  - wait for all threads to join back into the main thread
  
 Run the code with  to demo with threads.

- Implementation of Cooks as Tasks, in the `TasksImplementation` class, showing
  - `Task.TaskFactory.StartNew`
  - `Task.WaitAll`
  - the fact that the main thread is also doing some cooking in the call to `Cooking.GoCook()`

If you want to run it, modify `Main` to use the appropriate implementation