using System;
using System.Linq;
using System.Diagnostics;
using WinAPI;

namespace MemoryManagement
{
    public class MemoryManagerFor32BitProcesses : WindowsAPI
    {
        private IntPtr _processHandle;
        private byte[] _bytes = null;

        private int GetProcessIDByProcessName(string processName)
        {
            var processes = Process.GetProcesses();
            if (processes.Count() != 0)
            {
                foreach (var process in processes)
                {
                    if (process.ProcessName == processName)
                    {
                        return process.Id;
                    }
                }
            }
            return 0;
        }

        public IntPtr GetProcessHandle()
        {
            return _processHandle;
        }

        public void FindProcessByNameThenAttachToIt(string name)
        {
            int processID = GetProcessIDByProcessName(name);
            uint allRightsToProcess = 0x001F0FFF;
            _processHandle = OpenProcess(allRightsToProcess, false, processID);
        }

        public void DetachFromProcess()
        {
            CloseHandle(_processHandle);
        }

        public MemoryManagerFor32BitProcesses ReadBytesFromAddress(int address, int numberOfBytesToRead)
        {
            _bytes = new byte[numberOfBytesToRead];
            IntPtr numberOfReadBytes;
            ReadProcessMemory(_processHandle, (IntPtr)address, _bytes, numberOfBytesToRead, out numberOfReadBytes);
            _bytes = null;
            return this;
        }

        public void WriteBytesToAddress(int address, int numberOfBytesToWrite)
        {
            _bytes = new byte[numberOfBytesToWrite];
            IntPtr numberOfWrittenBytes;
            WriteProcessMemory(_processHandle, (IntPtr)address, _bytes, numberOfBytesToWrite, out numberOfWrittenBytes);
            _bytes = null;
        }

        public int GetTargetAddressByPointersUsingOffsets(int[] offsetsInBytes, int baseAddressFromWhereToStartSearch, IntPtr processHandle)
        {
            int sizeOfTargetAddressInBytes = 4;
            byte[] buffer = new byte[sizeOfTargetAddressInBytes];
            IntPtr numberOfReadBytes;
            int nextPointerAddress = 0;
            int nextFoundValue = 0;

            ReadProcessMemory(processHandle, (IntPtr)baseAddressFromWhereToStartSearch, buffer, sizeOfTargetAddressInBytes, out numberOfReadBytes);
            for (int i = 0; i < offsetsInBytes.Length; i++)
            {
                nextFoundValue = BitConverter.ToInt32(buffer, 0);
                nextPointerAddress = nextFoundValue + offsetsInBytes[i];
                ReadProcessMemory(processHandle, (IntPtr)nextPointerAddress, buffer, sizeOfTargetAddressInBytes, out numberOfReadBytes);
            }
            return nextPointerAddress;
        }

        public int ConvertBytesToIntegerValue()
        {
            return BitConverter.ToInt32(_bytes, 0);
        }

        public MemoryManagerFor32BitProcesses ConvertIntegerValueToBytes(int value)
        {
            _bytes = BitConverter.GetBytes(value);
            return this;
        }

        public float ConvertBytesToFloatValue()
        {
            return BitConverter.ToSingle(_bytes, 0);
        }

        public MemoryManagerFor32BitProcesses ConvertFloatValueToBytes(float value)
        {
            _bytes = BitConverter.GetBytes(value);
            return this;
        }
    }
}
