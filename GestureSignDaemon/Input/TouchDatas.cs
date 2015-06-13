﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace GestureSignDaemon.Input
{

    public interface ITouchData
    {
        bool Status { get; }
        int ID { get; }
        int X { get; }
        int Y { get; }
    }

    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct sTouchData : ITouchData
    {
        /// BYTE->unsigned char
        private byte status;
        /// BYTE->unsigned char
        private byte num;
        /// short
        private short x_position;
        /// short
        private short y_position;
        public int X { get { return x_position; } }
        public int Y { get { return y_position; } }
        public int ID { get { return num; } }
        public bool Status { get { return Convert.ToBoolean(status); } }
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 2)]
    public struct iTouchData : ITouchData
    {
        /// BYTE->unsigned char
        private byte status;
        /// BYTE->unsigned char
        private byte num;
        /// short
        private int x_position;
        /// short
        private int y_position;
        public int X { get { return x_position; } }
        public int Y { get { return y_position; } }
        public int ID { get { return num; } }
        public bool Status { get { return Convert.ToBoolean(status); } }
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 2)]
    public struct gTouchData : ITouchData
    {
        /// BYTE->unsigned char
        private byte status;
        /// BYTE->unsigned char
        private byte num;
        /// short
        private short x_position;
        /// short
        private short y_position;

        [MarshalAs(UnmanagedType.U4)]
        private int gap;
        public int X { get { return x_position; } }
        public int Y { get { return y_position; } }
        public int ID { get { return num; } }
        public bool Status { get { return Convert.ToBoolean(status); } }
    }
    // HID#FTSC0001&Col01#4&14bbeed5&0&0000#
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 2)]
    public struct dTouchData : ITouchData
    {
        /// BYTE->unsigned char
        private byte status;
        /// BYTE->unsigned char
        private byte num;
        /// short
        private short x;
        private short x_position;
        /// short
        private short y;
        private short y_position;

        [MarshalAs(UnmanagedType.U4)]
        private int gap;
        public int X { get { return x_position; } }
        public int Y { get { return y_position; } }
        public int ID { get { return num; } }
        public bool Status { get { return Convert.ToBoolean(status); } }
    }
    //HID#WCOM5008&Col01#4&2b144297&0&0000
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    public struct wcTouchData : ITouchData
    {
        /// BYTE->unsigned char
        private byte TouchDataStatus;
        /// BYTE->unsigned char
        private byte num;
        /// short
        private byte gap;

        private short x_position;
        /// short
        private short y_position;
        public int X { get { return x_position; } }
        public int Y { get { return y_position; } }
        public int ID { get { return num; } }
        public bool Status { get { return TouchDataStatus == (0x05); } }
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    public struct NtrgTouchData : ITouchData
    {
        private byte status;
        private short num;
        /// short
        private short x;
        private short x_position;
        /// short
        private short y;
        private short y_position;

        private short unknown1;
        private short unknown2;
        private int empty;

        public int X { get { return x_position; } }
        public int Y { get { return y_position; } }
        public int ID { get { return num; } }
        public bool Status { get { return (status) == 0xE7; } }
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    public struct AtmelTouchData : ITouchData
    {
        private byte data;
        private Int32 x;
        private Int32 y;
        private Int16 blank;

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int ID { get { return data >> 2; } }
        public bool Status { get { return (data & 0x1) == 1; } }
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    public struct AtmelTouchData2 : ITouchData
    {
        private byte data;
        private Int16 x1;
        private Int16 x2;
        private Int16 y1;
        private Int16 y2;
        private Int16 unknown1;

        public int X { get { return x1; } }
        public int Y { get { return y1; } }
        public int ID { get { return data >> 2; } }
        public bool Status { get { return (data & 0x1) == 1; } }
    }
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 1)]
    public struct ElanTouchData : ITouchData
    {
        private byte status;
        private Int16 unknown;
        private Int16 x1;
        private Int16 x2;
        private Int16 y1;
        private Int16 y2;

        public int X { get { return x1; } }
        public int Y { get { return y1; } }
        public int ID { get { return status >> 2; } }
        public bool Status { get { return (status & 1) == 1; } }
    }
}
