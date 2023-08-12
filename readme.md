# Overview

Methods to work with process memory using WinAPI functions in C#.

# Installation

- clone this repository using `git clone <link>` command
- plase directory with classes to your project directory

# Usage

Create instance of memory manager then simply find process by name and attach to it. Use Task Manager to get process names.
```
MemoryManagerFor32BitProcesses memoryManager = new MemoryManagerFor32BitProcesses();
memoryManager.FindProcessByNameThenAttachToIt("process_name");
```
To detach from process use `DetachFromProcess` method.
```
memoryManager.DetachFromProcess();
```
Getting values from process memory
```
int integerValue = memoryManager.ReadBytesFromAddress(0x12345678, 4).ConvertBytesToIntegerValue();
float floatValue = memoryManager.ReadBytesFromAddress(0x12345678, 4).ConvertBytesToFloatValue();
```
Writing values to process memory
```
memoryManager.ConvertIntegerValueToBytes(integerValue).WriteBytesToAddress(0x12345678, 4);
memoryManager.ConvertFloatValueToBytes(floatValue).WriteBytesToAddress(0x12345678, 4);
```
Finding target address by pointers
```
int[] offsetsInBytes = new int[1]; //Declaring array of offsets in bytes
offsetsInBytes[0] = 0x84; //Setting an offset
int targetAddress = memoryManager.GetTargetAddressByPointersUsingOffsets(offsetsInBytes, 0x0073845C);
```
Checking if key is pushed down
```
if (KeyboardManager.IsKeyPushedDown(87))
{
    //W key is pushed down
}
```
