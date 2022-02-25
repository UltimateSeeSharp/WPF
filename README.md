Tools for MVVM Wpf:

- ICommand
- NotifyProertyChanged
- Autofac
- Serilog

Load list in context:
context.Entry(project).Collection(item => item.Cutters!).Load();
