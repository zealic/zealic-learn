using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace HelloTask
{
  public sealed class Hello : Task
  {
    public override bool Execute()
    {
      HelloWorld.Say();
      return true;
    }
  }
}