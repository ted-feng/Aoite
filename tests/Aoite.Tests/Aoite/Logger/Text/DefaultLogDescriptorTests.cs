﻿using System;
using Xunit;

namespace Aoite.Logger.Text
{
    public class DefaultLogDescriptorTests
    {
        [Fact()]
        public void Test1()
        {
            LogDescriptor desc = new LogDescriptor();
            var now = DateTime.Now;
            var str = desc.Describe(Log.Logger, new LogItem()
            {
                Time = now,
                Message = "测试内容。",
                Type = LogType.Info
            });
            Assert.Equal(now.ToString("HH:mm:ss.ffff") + " [消息] 测试内容。", str);
        }
    }
}
